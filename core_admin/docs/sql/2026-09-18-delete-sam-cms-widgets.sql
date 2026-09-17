-- 18 ก.ย. 2569 — ลบข้อมูล SAM ที่ระบบ page builder ของ Asset Plus ไม่ใช้ (สำรองไว้ก่อนแล้วที่ docs/backup-sam-widgets/ บนเครื่อง dev)
--   [2026_web_cms_page]      เหลือแถวเดียว id 1 = หน้าแรก (is_home = 1)  — เมนู SAM (เกี่ยวกับเรา, ทรัพย์สินรอการขาย, ...) ลบทิ้ง
--   [2026_web_widget_group]  เหลือ 3 กลุ่มของ Asset Plus (DEFAULT/MODERN/CLASSIC)  — กลุ่ม SAM id 1-3 ลบทิ้ง
--   [2026_web_widget]        เหลือ 18 widget ของ Asset Plus (section_key ไม่ว่าง)     — widget SAM id 1-27 ลบทิ้ง
--   [2026_web_widget_group2] / [2026_web_widget2]  ชุดของ microsite SAM — ลบทั้งหมด (ระบบนี้ไม่มี microsite)
-- idempotent: รันซ้ำได้ (เงื่อนไขอิงข้อมูล ไม่อิง id)
SET XACT_ABORT ON;
BEGIN TRAN;
DELETE FROM [2026_web_cms_page] WHERE NOT (id = 1 AND is_home = 1);
DELETE FROM [2026_web_widget] WHERE section_key IS NULL OR section_key = '';
DELETE FROM [2026_web_widget_group] WHERE id NOT IN (SELECT DISTINCT cat_id FROM [2026_web_widget]);
DELETE FROM [2026_web_widget2];
DELETE FROM [2026_web_widget_group2];
COMMIT;
SELECT 'cms_page' t, COUNT(*) n FROM [2026_web_cms_page]
UNION ALL SELECT 'widget_group', COUNT(*) FROM [2026_web_widget_group]
UNION ALL SELECT 'widget', COUNT(*) FROM [2026_web_widget]
UNION ALL SELECT 'widget_group2', COUNT(*) FROM [2026_web_widget_group2]
UNION ALL SELECT 'widget2', COUNT(*) FROM [2026_web_widget2];
