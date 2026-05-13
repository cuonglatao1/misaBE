-- MISA Payroll - Schema
-- Encoding: UTF-8

CREATE DATABASE IF NOT EXISTS misa_payroll CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE misa_payroll;

-- =============================================
-- 1. pa_organization - Đơn vị công tác
-- =============================================
CREATE TABLE IF NOT EXISTS pa_organization (
    OrganizationId   CHAR(36)     NOT NULL PRIMARY KEY,
    OrganizationCode VARCHAR(50)  NOT NULL,
    OrganizationName VARCHAR(255) NOT NULL,
    ParentId         CHAR(36)     NULL,
    IsActive         TINYINT(1)   NOT NULL DEFAULT 1,
    CreatedDate      DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ModifiedDate     DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_org_parent (ParentId)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =============================================
-- 2. pa_salary_composition_system - Danh mục hệ thống
-- =============================================
CREATE TABLE IF NOT EXISTS pa_salary_composition_system (
    SystemId      CHAR(36)     NOT NULL PRIMARY KEY,
    SystemCode    VARCHAR(255) NOT NULL,
    SystemName    VARCHAR(255) NOT NULL,
    -- income | deduction | attendance | sales | employee_info | other
    ComponentType VARCHAR(50)  NOT NULL,
    -- fixed | variable | formula
    Nature        VARCHAR(50)  NOT NULL,
    -- taxable | exempt_full | exempt_partial
    TaxOption     VARCHAR(50)  NOT NULL DEFAULT 'taxable',
    Quota         TEXT         NULL,
    -- money | number | percent | text
    ValueType     VARCHAR(20)  NOT NULL DEFAULT 'money',
    ValueFormula  TEXT         NULL,
    Description   TEXT         NULL,
    -- yes | no | nonzero
    ShowOnPayslip VARCHAR(10)  NOT NULL DEFAULT 'yes',
    IsActive      TINYINT(1)   NOT NULL DEFAULT 1,
    CreatedDate   DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ModifiedDate  DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    UNIQUE KEY uk_system_code (SystemCode)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =============================================
-- 3. pa_salary_composition - Danh sách thành phần lương
-- =============================================
CREATE TABLE IF NOT EXISTS pa_salary_composition (
    CompositionId    CHAR(36)     NOT NULL PRIMARY KEY,
    CompositionCode  VARCHAR(255) NOT NULL,
    CompositionName  VARCHAR(255) NOT NULL,
    OrganizationId   CHAR(36)     NULL,
    ComponentType    VARCHAR(50)  NOT NULL,
    Nature           VARCHAR(50)  NOT NULL,
    TaxOption        VARCHAR(50)  NOT NULL DEFAULT 'taxable',
    AllowExceedQuota TINYINT(1)   NOT NULL DEFAULT 0,
    Quota            TEXT         NULL,
    ValueType        VARCHAR(20)  NOT NULL DEFAULT 'money',
    -- auto | manual
    ValueSource      VARCHAR(10)  NOT NULL DEFAULT 'manual',
    ValueFormula     TEXT         NULL,
    Description      TEXT         NULL,
    ShowOnPayslip    VARCHAR(10)  NOT NULL DEFAULT 'yes',
    -- system | manual | default
    SourceType       VARCHAR(20)  NOT NULL DEFAULT 'manual',
    SystemId         CHAR(36)     NULL,
    -- active | inactive
    Status           VARCHAR(20)  NOT NULL DEFAULT 'active',
    CreatedDate      DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ModifiedDate     DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    UNIQUE KEY uk_composition_code (CompositionCode),
    INDEX idx_sc_org    (OrganizationId),
    INDEX idx_sc_status (Status),
    INDEX idx_sc_type   (ComponentType)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =============================================
-- 4. pa_grid_config - Cấu hình cột bảng
-- =============================================
CREATE TABLE IF NOT EXISTS pa_grid_config (
    ConfigId      CHAR(36)     NOT NULL PRIMARY KEY,
    GridId        VARCHAR(100) NOT NULL,
    ColumnField   VARCHAR(100) NOT NULL,
    ColumnCaption VARCHAR(255) NOT NULL,
    ColumnWidth   INT          NOT NULL DEFAULT 150,
    IsVisible     TINYINT(1)   NOT NULL DEFAULT 1,
    IsFixed       TINYINT(1)   NOT NULL DEFAULT 0,
    -- left | right
    FixedPosition VARCHAR(10)  NULL,
    SortOrder     INT          NOT NULL DEFAULT 0,
    UserId        VARCHAR(36)  NULL,
    CreatedDate   DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ModifiedDate  DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    UNIQUE KEY uk_grid_col_user (GridId, ColumnField, UserId)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
