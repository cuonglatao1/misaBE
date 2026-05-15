-- ============================================================
-- Cập nhật dữ liệu đơn vị công tác (pa_organization)
-- Cấu trúc cây theo yêu cầu:
--
-- Misa Test pdthien 2024
-- ├── Chi nhánh miền Bắc
-- │   └── Khối sản xuất
-- │       ├── Trung tâm kinh doanh
-- │       └── Trung tâm hỗ trợ khách hàng
-- └── Chi nhánh miền Nam
--     └── Trung tâm kinh doanh
-- ============================================================

USE misa_payroll;

-- Xóa dữ liệu cũ
DELETE FROM pa_organization;

-- ============================================================
-- Chèn cây tổ chức
-- ============================================================
INSERT INTO pa_organization
    (OrganizationId, OrganizationCode, OrganizationName, ParentId, IsActive)
VALUES
    -- Level 0: Công ty
    ('org00001-0000-0000-0000-000000000001', 'CT001', 'Misa Test pdthien 2024', NULL, 1),

    -- Level 1: Chi nhánh
    ('org00001-0000-0000-0000-000000000002', 'CN001', 'Chi nhánh miền Bắc',  'org00001-0000-0000-0000-000000000001', 1),
    ('org00001-0000-0000-0000-000000000006', 'CN002', 'Chi nhánh miền Nam',  'org00001-0000-0000-0000-000000000001', 1),

    -- Level 2: Khối (dưới Chi nhánh miền Bắc)
    ('org00001-0000-0000-0000-000000000003', 'KSX01', 'Khối sản xuất',       'org00001-0000-0000-0000-000000000002', 1),

    -- Level 3: Trung tâm (dưới Khối sản xuất)
    ('org00001-0000-0000-0000-000000000004', 'TT001', 'Trung tâm kinh doanh',           'org00001-0000-0000-0000-000000000003', 1),
    ('org00001-0000-0000-0000-000000000005', 'TT002', 'Trung tâm hỗ trợ khách hàng',   'org00001-0000-0000-0000-000000000003', 1),

    -- Level 2: Trung tâm kinh doanh (dưới Chi nhánh miền Nam)
    ('org00001-0000-0000-0000-000000000007', 'TT003', 'Trung tâm kinh doanh',           'org00001-0000-0000-0000-000000000006', 1);

-- ============================================================
-- Cập nhật OrganizationId trong pa_salary_composition
-- Phân bổ các thành phần lương vào đơn vị tương ứng
-- ============================================================

-- 1/3 đầu thuộc Trung tâm kinh doanh (Bắc)
UPDATE pa_salary_composition
SET    OrganizationId = 'org00001-0000-0000-0000-000000000004'
WHERE  CompositionId IN (
    SELECT CompositionId FROM (
        SELECT CompositionId FROM pa_salary_composition ORDER BY CreatedDate LIMIT 18446744073709551615
    ) t
    LIMIT 7
);

-- 1/3 giữa thuộc Trung tâm hỗ trợ khách hàng
UPDATE pa_salary_composition
SET    OrganizationId = 'org00001-0000-0000-0000-000000000005'
WHERE  OrganizationId IS NULL
ORDER BY CreatedDate
LIMIT 7;

-- 1/3 cuối thuộc Trung tâm kinh doanh (Nam)
UPDATE pa_salary_composition
SET    OrganizationId = 'org00001-0000-0000-0000-000000000007'
WHERE  OrganizationId IS NULL
ORDER BY CreatedDate
LIMIT 7;

-- Phần còn lại (nếu có) → thuộc công ty (NULL = áp dụng toàn công ty)
-- OrganizationId vẫn là NULL cho các bản ghi chưa được gán

SELECT 'Xong! Kiểm tra kết quả:' AS '';

SELECT 
    o.OrganizationName,
    p.OrganizationName AS ParentName,
    o.OrganizationCode
FROM pa_organization o
LEFT JOIN pa_organization p ON o.ParentId = p.OrganizationId
ORDER BY o.OrganizationId;
