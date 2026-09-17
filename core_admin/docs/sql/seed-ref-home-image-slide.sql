-- seed ข้อมูล Asset Plus: รูปสไลด์หน้าแรก (HomeImageSlide, web_core_item module_id 1)
-- คัดลอกจาก scratchpad ของ session ที่ทำงานจริงเมื่อ 16 ก.ย. 2569 เพื่อเก็บถาวร (scratchpad เป็นโฟลเดอร์ชั่วคราว)
-- ข้อมูลอ้างอิงเท่านั้น — อ้าง id ของ DB dev (แทนแถว SAM เดิม) ห้ามรันซ้ำบน dev · บนเซิร์ฟเวอร์ให้ใส่ข้อมูลผ่านหน้าหลังบ้านแทน หรือแก้ id ให้ตรงก่อนรัน

SET NOCOUNT ON;
SET XACT_ABORT ON;
BEGIN TRAN;
DECLARE @now datetimeoffset(7) = SYSDATETIMEOFFSET();
DECLARE @s TABLE (sort int, title nvarchar(255), en_title nvarchar(255), img nvarchar(200), img_m nvarchar(200), url nvarchar(200));
INSERT @s VALUES
 (10, N'กองทุนเปิด แอสเซทพลัส ดีเฟนส์ อิควิตี้ (ASP-DEFENSE) ลงทุนในอุตสาหกรรม Aerospace & Defense',
      N'Asset Plus Defense Equity Fund (ASP-DEFENSE) invests in the Aerospace & Defense industry',
      N'Files/Site0/1/home/hero-asp-defense.jpg', N'Files/Site0/1/home/hero-asp-defense-mobile.jpg', N'/funds/asp-defense'),
 (20, N'กองทุนเปิด แอสเซทพลัส ฮิวแมนนอยด์ (A-HUMANOID) ลงทุนในธุรกิจหุ่นยนต์และ AI ระดับโลก',
      N'Asset Plus Humanoid Fund (A-HUMANOID) invests in global robotics and AI businesses',
      N'Files/Site0/1/home/hero-a-humanoid.jpg', N'Files/Site0/1/home/hero-a-humanoid-mobile.jpg', N'/funds/a-humanoid'),
 (30, N'กองทุนเปิด แอสเซทพลัส เอเชีย เซมิคอนดักเตอร์ (A-ASEMI) ลงทุนในห่วงโซ่การผลิตชิปแห่งเอเชีย',
      N'Asset Plus Asia Semiconductor Fund (A-ASEMI) invests in Asia''s chip manufacturing supply chain',
      N'Files/Site0/1/home/hero-a-asemi.jpg', N'Files/Site0/1/home/hero-a-asemi-mobile.jpg', N'/funds/a-asemi');

-- ลบแถวเดโมของ SAM ทั้งหมดใน module 1 แล้วใส่ข้อมูลจริงของ Asset Plus (ฉบับร่าง = ฉบับอนุมัติ)
DELETE FROM [2026_web_core_item] WHERE module_id = 1;

INSERT INTO [2026_web_core_item]
 (created_at, updated_at, created_by, updated_by, sort, status, pb_status, approve_by, show_front, web_id, module_id,
  title, pb_title, en_title, pb_en_title,
  img1, pb_img1, en_img1, pb_en_img1,
  img1_icon, pb_img1_icon, en_img1_icon, pb_en_img1_icon,
  url, pb_url, en_url, pb_en_url, url_target, pb_url_target,
  issue_date_config, pb_issue_date_config, issue_date, pb_issue_date, expiry_date, pb_expiry_date)
SELECT @now, @now, N'user', N'user', sort, 1, 1, N'user', 1, 0, 1,
  title, title, en_title, en_title,
  img, img, img, img,
  img_m, img_m, img_m, img_m,
  url, url, url, url, N'_top', N'_top',
  1, 1, @now, @now, DATEADD(year, 1, @now), DATEADD(year, 1, @now)
FROM @s ORDER BY sort;
COMMIT;
SELECT id, sort, status, pb_status, show_front, pb_title, pb_img1, pb_img1_icon, pb_url FROM [2026_web_core_item] WHERE module_id = 1 ORDER BY sort;
