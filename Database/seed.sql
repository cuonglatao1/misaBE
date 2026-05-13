USE misa_payroll;

-- =============================================
-- Seed pa_organization
-- =============================================
INSERT INTO pa_organization (OrganizationId, OrganizationCode, OrganizationName, ParentId, IsActive) VALUES
('org-001', 'MISA', 'Misa Test pdthien 2024', NULL, 1),
('org-002', 'IT', 'Phòng Công nghệ thông tin', 'org-001', 1),
('org-003', 'HR', 'Phòng Nhân sự', 'org-001', 1),
('org-004', 'ACC', 'Phòng Kế toán', 'org-001', 1);

-- =============================================
-- Seed pa_salary_composition_system (Danh mục hệ thống)
-- =============================================
INSERT INTO pa_salary_composition_system
(SystemId, SystemCode, SystemName, ComponentType, Nature, TaxOption, Quota, ValueType, ValueFormula, Description, ShowOnPayslip, IsActive)
VALUES
('sys-001', 'LUONG_CB',    'Lương cơ bản',         'income',    'fixed',   'taxable',      NULL,  'money',   NULL,   'Lương cơ bản theo hợp đồng', 'yes', 1),
('sys-002', 'LUONG_DONG_BH','Lương đóng BH',        'income',    'fixed',   'taxable',      NULL,  'money',   NULL,   'Lương dùng để tính BHXH/BHYT/BHTN', 'yes', 1),
('sys-003', 'BHXH',         'Bảo hiểm xã hội',       'deduction', 'formula', 'taxable',      '8%',  'percent', '=LUONG_DONG_BH*0.08', 'BHXH người lao động đóng', 'yes', 1),
('sys-004', 'BHYT',         'Bảo hiểm y tế',         'deduction', 'formula', 'taxable',      '1.5%','percent', '=LUONG_DONG_BH*0.015','BHYT người lao động đóng', 'yes', 1),
('sys-005', 'BHTN',         'Bảo hiểm thất nghiệp',  'deduction', 'formula', 'taxable',      '1%',  'percent', '=LUONG_DONG_BH*0.01', 'BHTN người lao động đóng', 'yes', 1),
('sys-006', 'THUE_TNCN',    'Thuế TNCN',              'deduction', 'formula', 'taxable',      NULL,  'money',   NULL,   'Thuế thu nhập cá nhân', 'yes', 1),
('sys-007', 'THAM_NIEN',    'Thâm niên (tháng)',      'employee_info','variable','taxable', NULL,  'number',  NULL,   'Số tháng thâm niên', 'no', 1),
('sys-008', 'TY_LE_DS',     'Tỷ lệ hoàn thành doanh số','sales','variable','taxable',  NULL,  'percent', '=DOANH_SO_THUC_TE/DOANH_SO_MUC_TIEU', 'Tỷ lệ hoàn thành doanh số', 'yes', 1),
('sys-009', 'CONG_DOAN',    'Công đoàn',              'deduction', 'formula', 'taxable',      '1%',  'percent', '=LUONG_CB*0.01',      'Công đoàn phí', 'yes', 1),
('sys-010', 'PC_AN_TRUA',   'Phụ cấp ăn trưa',       'income',    'fixed',   'exempt_full',  '730000','money', NULL,   'Phụ cấp ăn trưa theo tháng', 'yes', 1);

-- =============================================
-- Seed pa_salary_composition (mẫu dữ liệu)
-- =============================================
INSERT INTO pa_salary_composition
(CompositionId, CompositionCode, CompositionName, OrganizationId, ComponentType, Nature,
 TaxOption, AllowExceedQuota, Quota, ValueType, ValueSource, ValueFormula,
 Description, ShowOnPayslip, SourceType, SystemId, Status)
VALUES
('cmp-001','LUONG_CB',    'Lương cơ bản',         'org-001','income',    'fixed',  'taxable',     0, NULL,   'money',  'manual','5000000',   'Lương cơ bản theo hợp đồng','yes','system','sys-001','active'),
('cmp-002','BHXH',        'Bảo hiểm xã hội',       'org-001','deduction', 'formula','taxable',     0, '8%',  'percent','manual','=LUONG_DONG_BH*0.08','BHXH người lao động đóng','yes','system','sys-003','active'),
('cmp-003','BHYT',        'Bảo hiểm y tế',         'org-001','deduction', 'formula','taxable',     0, '1.5%','percent','manual','=LUONG_DONG_BH*0.015','BHYT người lao động đóng','yes','system','sys-004','active'),
('cmp-004','BHTN',        'Bảo hiểm thất nghiệp',  'org-001','deduction', 'formula','taxable',     0, '1%',  'percent','manual','=LUONG_DONG_BH*0.01', 'BHTN người lao động đóng','yes','system','sys-005','active'),
('cmp-005','THUE_TNCN',   'Thuế TNCN',              'org-001','deduction', 'formula','taxable',     0, NULL,  'money',  'manual',NULL,         'Thuế thu nhập cá nhân','yes','system','sys-006','active'),
('cmp-006','PC_AN_TRUA',  'Phụ cấp ăn trưa',       'org-001','income',    'fixed',  'exempt_full', 0, '730000','money','manual','730000',     'Phụ cấp ăn trưa','yes','manual', NULL,    'active'),
('cmp-007','PC_DI_LAI',   'Phụ cấp đi lại',        'org-001','income',    'fixed',  'exempt_full', 0, '500000','money','manual','500000',     'Phụ cấp đi lại','yes','manual', NULL,    'active'),
('cmp-008','THUONG_HS',   'Thưởng hiệu suất',      'org-001','income',    'variable','taxable',    0, NULL,  'money',  'manual',NULL,         'Thưởng hiệu suất KPI','yes','manual', NULL,    'active'),
('cmp-009','KHAU_TRU_MUON','Khấu trừ đi muộn',      'org-001','deduction', 'variable','taxable',   0, NULL,  'money',  'manual',NULL,         'Khấu trừ đi muộn về sớm','yes','manual', NULL,    'active'),
('cmp-010','TONG_PHU_CAP','Tổng phụ cấp',          'org-001','income',    'variable','exempt_full',0, NULL,  'money',  'manual','=SUM(PC_AN_TRUA,PC_DI_LAI)','Tổng các khoản phụ cấp','no','manual',  NULL,    'active');

-- =============================================
-- Seed pa_grid_config (default columns)
-- =============================================
INSERT INTO pa_grid_config
(ConfigId, GridId, ColumnField, ColumnCaption, ColumnWidth, IsVisible, IsFixed, FixedPosition, SortOrder, UserId)
VALUES
('gcfg-01','salary_composition','CompositionCode',  'Mã thành phần',              160, 1, 0, NULL, 0,  NULL),
('gcfg-02','salary_composition','CompositionName',  'Tên thành phần',             250, 1, 0, NULL, 1,  NULL),
('gcfg-03','salary_composition','OrganizationName', 'Đơn vị áp dụng',            200, 1, 0, NULL, 2,  NULL),
('gcfg-04','salary_composition','ComponentType',    'Loại thành phần',            160, 1, 0, NULL, 3,  NULL),
('gcfg-05','salary_composition','Nature',           'Tính chất',                 160, 1, 0, NULL, 4,  NULL),
('gcfg-06','salary_composition','TaxOption',        'Chịu thuế',                150, 1, 0, NULL, 5,  NULL),
('gcfg-07','salary_composition','Quota',            'Định mức',                  150, 1, 0, NULL, 6,  NULL),
('gcfg-08','salary_composition','ValueType',        'Kiểu giá trị',              150, 1, 0, NULL, 7,  NULL),
('gcfg-09','salary_composition','ValueFormula',     'Giá trị',                  200, 1, 0, NULL, 8,  NULL),
('gcfg-10','salary_composition','Description',      'Mô tả',                    250, 1, 0, NULL, 9,  NULL),
('gcfg-11','salary_composition','ShowOnPayslip',    'Hiển thị trên phiếu lương', 180, 1, 0, NULL, 10, NULL),
('gcfg-12','salary_composition','SourceType',       'Nguồn tạo',                130, 1, 0, NULL, 11, NULL),
('gcfg-13','salary_composition','Status',           'Trạng thái',                130, 1, 0, NULL, 12, NULL);
