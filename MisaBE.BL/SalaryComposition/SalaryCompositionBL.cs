using MisaBE.BL.Base;
using MisaBE.Common.DTOs;
using MisaBE.Common.Entities;
using MisaBE.DL.SalaryComposition;
using MisaBE.DL.SalaryCompositionSystem;

namespace MisaBE.BL.SalaryComposition
{
    public class SalaryCompositionBL : BaseBL<Common.Entities.SalaryComposition>, ISalaryCompositionBL
    {
        private readonly ISalaryCompositionDL _compositionDL;
        private readonly ISalaryCompositionSystemDL _systemDL;

        public SalaryCompositionBL(
            ISalaryCompositionDL compositionDL,
            ISalaryCompositionSystemDL systemDL)
            : base(compositionDL)
        {
            _compositionDL = compositionDL;
            _systemDL = systemDL;
        }

        protected override async Task<List<string>> ValidateAsync(
            Common.Entities.SalaryComposition entity, bool isInsert, string? id = null)
        {
            var errors = new List<string>();

            // Required fields
            if (string.IsNullOrWhiteSpace(entity.CompositionCode))
                errors.Add("Mã thành phần không được để trống");
            else if (entity.CompositionCode.Length > 255)
                errors.Add("Mã thành phần không được vượt quá 255 ký tự");

            if (string.IsNullOrWhiteSpace(entity.CompositionName))
                errors.Add("Tên thành phần không được để trống");
            else if (entity.CompositionName.Length > 255)
                errors.Add("Tên thành phần không được vượt quá 255 ký tự");

            if (string.IsNullOrWhiteSpace(entity.ComponentType))
                errors.Add("Loại thành phần không được để trống");

            if (string.IsNullOrWhiteSpace(entity.Nature))
                errors.Add("Tính chất không được để trống");

            // Unique code check
            if (!string.IsNullOrWhiteSpace(entity.CompositionCode))
            {
                var excludeId = isInsert ? null : (id ?? entity.CompositionId);
                if (await _compositionDL.IsCodeExistsAsync(entity.CompositionCode, excludeId))
                    errors.Add($"Mã thành phần '{entity.CompositionCode}' đã tồn tại");
            }

            return errors;
        }

        public async Task<ServiceResult> CloneAsync(string id)
        {
            var original = await _compositionDL.GetByIdAsync(id);
            if (original == null)
                return ServiceResult.Fail("Không tìm thấy bản ghi");

            var clone = new Common.Entities.SalaryComposition
            {
                CompositionCode  = $"{original.CompositionCode}_COPY",
                CompositionName  = $"{original.CompositionName} (Bản sao)",
                OrganizationId   = original.OrganizationId,
                ComponentType    = original.ComponentType,
                Nature           = original.Nature,
                TaxOption        = original.TaxOption,
                AllowExceedQuota = original.AllowExceedQuota,
                Quota            = original.Quota,
                ValueType        = original.ValueType,
                ValueSource      = original.ValueSource,
                ValueFormula     = original.ValueFormula,
                Description      = original.Description,
                ShowOnPayslip    = original.ShowOnPayslip,
                SourceType       = "manual",
                Status           = "active"
            };

            // Ensure unique code with numeric suffix
            var baseCode = clone.CompositionCode;
            var suffix = 1;
            while (await _compositionDL.IsCodeExistsAsync(clone.CompositionCode))
                clone.CompositionCode = $"{baseCode}_{suffix++}";

            var newId = await _compositionDL.InsertAsync(clone);
            return ServiceResult.Ok(newId, "Nhân bản thành công");
        }

        public async Task<ServiceResult> ChangeStatusAsync(string id, string status)
        {
            if (status != "active" && status != "inactive")
                return ServiceResult.Fail("Trạng thái không hợp lệ");

            var affected = await _compositionDL.UpdateStatusAsync(id, status);
            if (affected == 0)
                return ServiceResult.Fail("Không tìm thấy bản ghi");

            var msg = status == "active" ? "Đang sử dụng" : "Ngừng sử dụng";
            return ServiceResult.Ok(null, $"Đã chuyển sang trạng thái: {msg}");
        }

        public async Task<ServiceResult> AddFromSystemAsync(string systemId)
        {
            var system = await _systemDL.GetByIdAsync(systemId);
            if (system == null)
                return ServiceResult.Fail("Không tìm thấy danh mục hệ thống");

            var entity = new Common.Entities.SalaryComposition
            {
                CompositionCode = system.SystemCode,
                CompositionName = system.SystemName,
                ComponentType   = system.ComponentType,
                Nature          = system.Nature,
                TaxOption       = system.TaxOption,
                Quota           = system.Quota,
                ValueType       = system.ValueType,
                ValueFormula    = system.ValueFormula,
                Description     = system.Description,
                ShowOnPayslip   = system.ShowOnPayslip,
                SourceType      = "system",
                SystemId        = system.SystemId,
                Status          = "active"
            };

            // Ensure unique code
            if (await _compositionDL.IsCodeExistsAsync(entity.CompositionCode))
            {
                var suffix = 1;
                while (await _compositionDL.IsCodeExistsAsync($"{system.SystemCode}_{suffix}"))
                    suffix++;
                entity.CompositionCode = $"{system.SystemCode}_{suffix}";
            }

            var newId = await _compositionDL.InsertAsync(entity);
            return ServiceResult.Ok(newId, "Thêm từ danh mục hệ thống thành công");
        }
    }
}
