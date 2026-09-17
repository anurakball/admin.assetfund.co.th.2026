-- 17 ก.ย. 2569 — คอลัมน์ที่เมนู "หน้าเว็บไซต์ > ปรับแต่ง Header" และ "ปรับแต่ง Footer" ของ Asset Plus ต้องมี (SAM ไม่มี)
-- รันบนทุก DB ที่ deploy (dev รันแล้ว) — idempotent: มีคอลัมน์อยู่แล้วจะข้าม
-- อ้างอิง: core_admin/CLAUDE.md หัวข้อ "เมนูที่ทำตาม playbook แล้ว" (HomeHeader / HomeFooter) และ docs/CMS-MENU-HANDOFF.md §2.1
SET XACT_ABORT ON;
BEGIN TRAN;

-- ปรับแต่ง Header: โลโก้ภาษาอังกฤษ (Alt Text ใช้ title / en_title ที่มีอยู่แล้ว)
IF COL_LENGTH('[2026_web_home_header]', 'en_img1') IS NULL
    ALTER TABLE [2026_web_home_header] ADD en_img1 nvarchar(255) NULL, pb_en_img1 nvarchar(255) NULL;

-- ปรับแต่ง Footer: โลโก้ / ที่อยู่ / ลิงก์แผนที่ / Blockdit / ปี / สโลแกน / หัวข้อแอป / QR
IF COL_LENGTH('[2026_web_home_footer]', 'img1') IS NULL
    ALTER TABLE [2026_web_home_footer] ADD
        img1 nvarchar(255) NULL, en_img1 nvarchar(255) NULL, pb_img1 nvarchar(255) NULL, pb_en_img1 nvarchar(255) NULL,
        address nvarchar(max) NULL, en_address nvarchar(max) NULL, pb_address nvarchar(max) NULL, pb_en_address nvarchar(max) NULL,
        address_url nvarchar(max) NULL, pb_address_url nvarchar(max) NULL,
        sc_bd nvarchar(255) NULL, pb_sc_bd nvarchar(255) NULL,
        promo_year nvarchar(20) NULL, pb_promo_year nvarchar(20) NULL,
        tagline_1 nvarchar(255) NULL, pb_tagline_1 nvarchar(255) NULL,
        tagline_2 nvarchar(255) NULL, pb_tagline_2 nvarchar(255) NULL,
        app_title nvarchar(255) NULL, en_app_title nvarchar(255) NULL, pb_app_title nvarchar(255) NULL, pb_en_app_title nvarchar(255) NULL,
        app_qr nvarchar(255) NULL, pb_app_qr nvarchar(255) NULL;

COMMIT;
GO
-- ตรวจผล: ต้องได้ 2 แถว
SELECT '2026_web_home_header' t, COL_LENGTH('[2026_web_home_header]', 'pb_en_img1') ok
UNION ALL
SELECT '2026_web_home_footer', COL_LENGTH('[2026_web_home_footer]', 'pb_app_qr');
GO
