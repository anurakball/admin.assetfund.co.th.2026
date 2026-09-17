-- 18 ก.ย. 2569 — page builder หน้าแรก (CMSPage/Edit/1) ของ Asset Plus
-- 1) เพิ่มคอลัมน์ section_key / pb_section_key ใน [2026_web_widget] (SAM ไม่มี)
--    = ชื่อ partial ฝั่ง front-end (Views/Home/Partials/_<key>.cshtml) ที่ widget นี้แทน  (idempotent — รันซ้ำได้)
-- 2) สิทธิ์เมนู CMSPage ของ Super Admin: เหลือ แก้ไข + อนุมัติ (ตาราง web_cms_page เหลือแถวเดียว id 1)
SET XACT_ABORT ON;
BEGIN TRAN;
IF COL_LENGTH('[2026_web_widget]', 'section_key') IS NULL
    ALTER TABLE [2026_web_widget] ADD section_key nvarchar(100) NULL, pb_section_key nvarchar(100) NULL;
UPDATE [2026_web_admin_module]
   SET can_add = 0, can_edit = 1, can_delete = 0, can_move = 0, can_status = 0, can_export = 0, can_approve = 1,
       updated_at = SYSDATETIMEOFFSET(), updated_by = 'user'
 WHERE mod_name = 'CMSPage' AND access_id = 1 AND web_id = 0;
COMMIT;
