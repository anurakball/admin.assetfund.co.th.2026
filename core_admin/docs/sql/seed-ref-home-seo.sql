-- seed ข้อมูล Asset Plus: SEO & Code (HomeSEO)
-- คัดลอกจาก scratchpad ของ session ที่ทำงานจริงเมื่อ 17 ก.ย. 2569 เพื่อเก็บถาวร (scratchpad เป็นโฟลเดอร์ชั่วคราว)
-- ข้อมูลอ้างอิงเท่านั้น — อ้าง id ของ DB dev (แทนแถว SAM เดิม) ห้ามรันซ้ำบน dev · บนเซิร์ฟเวอร์ให้ใส่ข้อมูลผ่านหน้าหลังบ้านแทน หรือแก้ id ให้ตรงก่อนรัน

SET NOCOUNT ON; SET XACT_ABORT ON;
BEGIN TRAN;
DECLARE @title nvarchar(510) = N'บริษัทหลักทรัพย์จัดการกองทุน แอสเซท พลัส จำกัด';
DECLARE @en_title nvarchar(510) = N'Asset Plus Fund Management Co., Ltd.';
DECLARE @des nvarchar(max) = N'บลจ. แอสเซท พลัส — คุณค่าที่เหนือกว่าความมั่งคั่ง บริการกองทุนรวม กองทุนส่วนบุคคล และกองทุนสำรองเลี้ยงชีพ พร้อมข้อมูล NAV ผลการดำเนินงาน และเอกสารกองทุนครบถ้วน';
DECLARE @en_des nvarchar(max) = N'Asset Plus Fund Management — value beyond wealth. Mutual funds, private funds and provident funds with daily NAV, performance and fund documents.';
DECLARE @kw nvarchar(max) = N'กองทุนรวม, กองทุนส่วนบุคคล, กองทุนสำรองเลี้ยงชีพ, บลจ. แอสเซท พลัส, ASP FUND, NAV, ลงทุน, RMF, SSF, ThaiESG';
DECLARE @en_kw nvarchar(max) = N'mutual fund, private fund, provident fund, Asset Plus Fund Management, ASP FUND, NAV, investment, RMF, SSF, ThaiESG';
UPDATE [2026_web_home_seo] SET
  updated_at = SYSDATETIMEOFFSET(), updated_by = N'user', approve_by = N'user', status = 1, pb_status = 1, show_front = 1, web_id = 0,
  title = @title, pb_title = @title, en_title = @en_title, pb_en_title = @en_title,
  des = @des, pb_des = @des, en_des = @en_des, pb_en_des = @en_des,
  info = @kw, pb_info = @kw, en_info = @en_kw, pb_en_info = @en_kw,
  ga_embed_head = NULL, pb_ga_embed_head = NULL, ga_embed_body = NULL, pb_ga_embed_body = NULL,
  fb_embed_head = NULL, pb_fb_embed_head = NULL, fb_embed_body = NULL, pb_fb_embed_body = NULL,
  ot1_embed_head = NULL, pb_ot1_embed_head = NULL, ot1_embed_body = NULL, pb_ot1_embed_body = NULL,
  ot2_embed_head = NULL, pb_ot2_embed_head = NULL, ot2_embed_body = NULL, pb_ot2_embed_body = NULL,
  ot3_embed_head = NULL, pb_ot3_embed_head = NULL, ot3_embed_body = NULL, pb_ot3_embed_body = NULL
WHERE id = 1;
COMMIT;
SELECT id, status, pb_status, show_front, pb_title, pb_en_title, left(pb_des,40) des, left(pb_info,40) kw FROM [2026_web_home_seo];
