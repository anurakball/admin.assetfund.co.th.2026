-- seed ข้อมูล Asset Plus: หน้า Intro Page (HomeIntroPage) — ฉบับสุดท้าย (seed_intro2)
-- คัดลอกจาก scratchpad ของ session ที่ทำงานจริงเมื่อ 16 ก.ย. 2569 เพื่อเก็บถาวร (scratchpad เป็นโฟลเดอร์ชั่วคราว)
-- ข้อมูลอ้างอิงเท่านั้น — อ้าง id ของ DB dev (แทนแถว SAM เดิม) ห้ามรันซ้ำบน dev · บนเซิร์ฟเวอร์ให้ใส่ข้อมูลผ่านหน้าหลังบ้านแทน หรือแก้ id ให้ตรงก่อนรัน

SET NOCOUNT ON; SET XACT_ABORT ON;
BEGIN TRAN;
DELETE FROM [2026_web_home_intro_page] WHERE id = 30;
DECLARE @title nvarchar(510) = N'ประกาศแจ้งเตือนภัย: ระวังมิจฉาชีพแอบอ้างชื่อ บลจ. แอสเซท พลัส';
DECLARE @en_title nvarchar(510) = N'Scam alert: beware of fraudsters impersonating Asset Plus Fund Management';
DECLARE @info nvarchar(max) = N'<p style="text-align:center;">ขณะนี้พบกลุ่มมิจฉาชีพนำเลขที่บัญชีของบริษัทไปแอบอ้าง เพื่อหลอกลวงประชาชนในการจองซื้อหน่วยลงทุน<br>บริษัท<strong>ไม่มีนโยบาย</strong>รับทำธุรกรรมซื้อขายหุ้นรายวัน หรือชักชวนให้โอนเงินผ่านช่องทางส่วนบุคคลใด ๆ ทั้งสิ้น</p><p style="text-align:center;"><small>หากมีข้อสงสัย กรุณาติดต่อผู้ดูแลบัญชีของท่าน หรือ Customer Care 02-672-1111 | customercare@assetfund.co.th</small></p>';
DECLARE @en_info nvarchar(max) = N'<p style="text-align:center;">Fraudsters are using the company''s account numbers to solicit unit subscriptions.<br>Asset Plus <strong>never</strong> offers daily stock trading or asks you to transfer money through personal channels.</p><p style="text-align:center;"><small>Questions? Contact your account manager or Customer Care 02-672-1111 | customercare@assetfund.co.th</small></p>';
DECLARE @img nvarchar(510) = N'Files/Site0/1/intro_page/scam-alert.jpg';
UPDATE [2026_web_home_intro_page] SET
  status = 1, pb_status = 1, show_front = 1,
  title = @title, pb_title = @title, en_title = @en_title, pb_en_title = @en_title,
  info = @info, pb_info = @info, en_info = @en_info, pb_en_info = @en_info,
  img1 = @img, pb_img1 = @img,
  url = N'/news-announcements', pb_url = N'/news-announcements', en_url = N'/news-announcements', pb_en_url = N'/news-announcements',
  title_2_text = N'ติดต่อ Customer Care', pb_title_2_text = N'ติดต่อ Customer Care', title_2_url = N'/about/contact', pb_title_2_url = N'/about/contact'
WHERE id = 27;
COMMIT;
SELECT id, status, pb_status, show_front, pb_title, pb_img1, pb_url, pb_title_1_text, pb_title_2_text, pb_title_2_url,
  CASE WHEN title = pb_title AND info = pb_info AND img1 = pb_img1 AND main_color = pb_main_color AND title_2_url = pb_title_2_url THEN 'draft=approved' ELSE 'DIFF' END d
FROM [2026_web_home_intro_page] ORDER BY id;
