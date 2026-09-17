-- seed ข้อมูล Asset Plus: หน้า Pop-Up (HomePopUp)
-- คัดลอกจาก scratchpad ของ session ที่ทำงานจริงเมื่อ 16 ก.ย. 2569 เพื่อเก็บถาวร (scratchpad เป็นโฟลเดอร์ชั่วคราว)
-- ข้อมูลอ้างอิงเท่านั้น — อ้าง id ของ DB dev (แทนแถว SAM เดิม) ห้ามรันซ้ำบน dev · บนเซิร์ฟเวอร์ให้ใส่ข้อมูลผ่านหน้าหลังบ้านแทน หรือแก้ id ให้ตรงก่อนรัน

SET NOCOUNT ON; SET XACT_ABORT ON;
BEGIN TRAN;
DELETE FROM [2026_web_home_pop_up];
DECLARE @now datetimeoffset(7) = SYSDATETIMEOFFSET();
DECLARE @p TABLE (sort int, title nvarchar(510), en_title nvarchar(510), img nvarchar(510), url nvarchar(max), en_url nvarchar(max), target nvarchar(200));
INSERT @p VALUES
 (10, N'ประกาศแจ้งเตือนภัย: ระวังมิจฉาชีพแอบอ้างชื่อ บลจ. แอสเซท พลัส', N'Scam alert: beware of fraudsters impersonating Asset Plus Fund Management',
      N'Files/Site0/1/pop_up/scam-alert.jpg', N'/news-announcements', N'/news-announcements', N'_top'),
 (20, N'ติดตาม Facebook ใหม่ของ บลจ. แอสเซท พลัส @aspfund.official', N'Follow the new Asset Plus Fund Management Facebook page @aspfund.official',
      N'Files/Site0/1/pop_up/follow-facebook.jpg', N'https://www.facebook.com/aspfund.official', N'https://www.facebook.com/aspfund.official', N'_blank');
INSERT INTO [2026_web_home_pop_up]
 (created_at, updated_at, created_by, updated_by, sort, status, pb_status, approve_by, show_front, web_id,
  title, pb_title, en_title, pb_en_title, img1, pb_img1, url, pb_url, en_url, pb_en_url, url_target, pb_url_target,
  issue_date_config, pb_issue_date_config, issue_date, pb_issue_date, expiry_date, pb_expiry_date,
  title_1_url_target, title_2_url_target, title_3_url_target, pb_title_1_url_target, pb_title_2_url_target, pb_title_3_url_target)
SELECT @now, @now, N'user', N'user', sort, 1, 1, N'user', 1, 0,
  title, title, en_title, en_title, img, img, url, url, en_url, en_url, target, target,
  N'1', N'1', @now, @now, DATEADD(year, 1, @now), DATEADD(year, 1, @now),
  N'_top', N'_top', N'_top', N'_top', N'_top', N'_top'
FROM @p ORDER BY sort;
COMMIT;
SELECT id, sort, status, pb_status, show_front, pb_title, pb_img1, pb_url, pb_url_target FROM [2026_web_home_pop_up] ORDER BY sort;
