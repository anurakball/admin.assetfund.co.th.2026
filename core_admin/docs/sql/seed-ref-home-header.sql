-- seed ข้อมูล Asset Plus: ปรับแต่ง Header (HomeHeader) — ส่วน ALTER TABLE อยู่ใน 2026-09-17-web-home-header-footer.sql
-- คัดลอกจาก scratchpad ของ session ที่ทำงานจริงเมื่อ 17 ก.ย. 2569 เพื่อเก็บถาวร (scratchpad เป็นโฟลเดอร์ชั่วคราว)
-- ข้อมูลอ้างอิงเท่านั้น — อ้าง id ของ DB dev (แทนแถว SAM เดิม) ห้ามรันซ้ำบน dev · บนเซิร์ฟเวอร์ให้ใส่ข้อมูลผ่านหน้าหลังบ้านแทน หรือแก้ id ให้ตรงก่อนรัน

SET XACT_ABORT ON;
BEGIN TRAN;
IF COL_LENGTH('[2026_web_home_header]', 'en_img1') IS NULL
    ALTER TABLE [2026_web_home_header] ADD en_img1 nvarchar(255) NULL, pb_en_img1 nvarchar(255) NULL;
COMMIT;
GO
BEGIN TRAN;
UPDATE [2026_web_home_header] SET
    title = N'บริษัทหลักทรัพย์จัดการกองทุน แอสเซท พลัส จำกัด',
    en_title = N'Asset Plus Fund Management',
    img1 = N'Files/Site0/1/header/logo-dark.svg',
    en_img1 = N'Files/Site0/1/header/logo-dark.svg',
    pb_title = N'บริษัทหลักทรัพย์จัดการกองทุน แอสเซท พลัส จำกัด',
    pb_en_title = N'Asset Plus Fund Management',
    pb_img1 = N'Files/Site0/1/header/logo-dark.svg',
    pb_en_img1 = N'Files/Site0/1/header/logo-dark.svg',
    des = NULL, pb_des = NULL, this_type = NULL, pb_this_type = NULL,
    status = 1, pb_status = 1, show_front = 1, web_id = 0, sort = 10,
    updated_at = SYSDATETIMEOFFSET(), updated_by = N'user', approve_by = N'user'
WHERE id = 1;
COMMIT;
SELECT id, title, en_title, img1, en_img1, pb_title, pb_en_title, pb_img1, pb_en_img1, status, pb_status, show_front FROM [2026_web_home_header];
