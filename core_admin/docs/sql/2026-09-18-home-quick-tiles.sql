-- ข้อมูลหน้าแรก > มูลค่าหน่วยฯ (3 Links) (โมดูล HomeSamText) — Asset Plus 18 ก.ย. 2569 · รันซ้ำได้ (idempotent)
-- 1) แถว [2026_web_core_single] module_id 1 (เดิม SAM ใส่ใจ) → ไทล์ลิงก์ด่วน 3 อัน (ยกจาก hardcode ใน front-end _NavPrices.cshtml)
--    ช่องของไทล์ที่ n: b=(n-1)*6 → t{b+1}/en_ หัวข้อ · t{b+2}/en_ คำอธิบาย · t{b+3}/en_ URL · t{b+4} Target · t{b+5} รูป · t{b+6} ไอคอน
--    ⚠ ขั้นนี้เขียนทับเนื้อหาของแถว — ถ้าหลังบ้านบนเซิร์ฟเวอร์แก้ข้อมูลไปแล้ว ให้ข้ามขั้น 1 (รันเฉพาะขั้น 2–3)
-- 2) widget 'มูลค่าหน่วยลงทุน' 3 เวอร์ชัน (section_key NavPrices / V2 / V3) → mod_name = HomeSamText (ปุ่มดินสอใน page builder)
-- 3) HTML ตัวอย่างของ widget ดังกล่าว: ข้อความ/ลิงก์/รูป/ไอคอนของไทล์ → token |||pb_tN||| ให้ builder เติมค่าจริงจาก DB
--    (อ้าง widget ด้วย section_key ไม่ใช้ id — บนเซิร์ฟเวอร์ id อาจต่างจาก dev)
-- รูป: อัปโหลด core_admin/wwwroot/Files/Site0/1/home/tiles/{performance,nav,calendar}.jpg ขึ้นเซิร์ฟเวอร์ admin ด้วย (ไม่ไปกับ git/publish)
SET NOCOUNT ON; SET XACT_ABORT ON;
BEGIN TRAN;

-- 1) ------------------------------------------------------------------------------------------
DECLARE @sql nvarchar(max) = N'UPDATE [2026_web_core_single] SET ';
DECLARE @i int = 1;
WHILE @i <= 50 BEGIN
  SET @sql += CONCAT(N't', @i, N'=NULL, en_t', @i, N'=NULL, pb_t', @i, N'=NULL, pb_en_t', @i, N'=NULL', CASE WHEN @i < 50 THEN N', ' ELSE N'' END);
  SET @i += 1;
END
SET @sql += N' WHERE module_id = 1 AND web_id = 0;';
EXEC sp_executesql @sql;
UPDATE [2026_web_core_single] SET
  title = N'ลิงก์ด่วน ข้างตารางมูลค่าหน่วยลงทุน',
  en_title = NULL,
  t1 = N'ผลการดำเนินงานทั้งหมด',
  en_t1 = N'Fund Performance',
  t2 = N'เปรียบเทียบผลตอบแทนย้อนหลังของแต่ละกองทุน',
  en_t2 = N'Compare historical returns of every fund',
  t3 = N'/funds/performance',
  t4 = N'_top',
  t5 = N'Files/Site0/1/home/tiles/performance.jpg',
  t6 = N'bi-graph-up-arrow',
  t7 = N'มูลค่าหน่วยลงทุนทั้งหมด',
  en_t7 = N'All Fund NAV',
  t8 = N'ค้นหามูลค่าหน่วยลงทุน (NAV) ย้อนหลังของทุกกองทุน',
  en_t8 = N'Search historical NAV of every fund',
  t9 = N'/funds/nav',
  t10 = N'_top',
  t11 = N'Files/Site0/1/home/tiles/nav.jpg',
  t12 = N'bi-currency-exchange',
  t13 = N'ปฏิทินกองทุน',
  en_t13 = N'Fund Calendar',
  t14 = N'ตารางวันทำการซื้อขายและวันหยุดของแต่ละกองทุน',
  en_t14 = N'Trading days and holidays of each fund',
  t15 = N'/funds/calendar',
  t16 = N'_top',
  t17 = N'Files/Site0/1/home/tiles/calendar.jpg',
  t18 = N'bi-calendar3',
  issue_date_config = N'1',
  status = 1,
  show_front = 1,
  pb_status = 1,
  updated_at = SYSDATETIMEOFFSET()
WHERE module_id = 1 AND web_id = 0;
UPDATE [2026_web_core_single] SET pb_title = title, pb_en_title = en_title, pb_issue_date_config = issue_date_config, pb_issue_date = issue_date, pb_expiry_date = expiry_date, pb_t1 = t1, pb_en_t1 = en_t1, pb_t2 = t2, pb_en_t2 = en_t2, pb_t3 = t3, pb_en_t3 = en_t3, pb_t4 = t4, pb_en_t4 = en_t4, pb_t5 = t5, pb_en_t5 = en_t5, pb_t6 = t6, pb_en_t6 = en_t6, pb_t7 = t7, pb_en_t7 = en_t7, pb_t8 = t8, pb_en_t8 = en_t8, pb_t9 = t9, pb_en_t9 = en_t9, pb_t10 = t10, pb_en_t10 = en_t10, pb_t11 = t11, pb_en_t11 = en_t11, pb_t12 = t12, pb_en_t12 = en_t12, pb_t13 = t13, pb_en_t13 = en_t13, pb_t14 = t14, pb_en_t14 = en_t14, pb_t15 = t15, pb_en_t15 = en_t15, pb_t16 = t16, pb_en_t16 = en_t16, pb_t17 = t17, pb_en_t17 = en_t17, pb_t18 = t18, pb_en_t18 = en_t18 WHERE module_id = 1 AND web_id = 0;

-- 2) ------------------------------------------------------------------------------------------
UPDATE [2026_web_widget] SET mod_name = N'HomeSamText', pb_mod_name = N'HomeSamText' WHERE section_key IN (N'NavPrices', N'NavPricesV2', N'NavPricesV3');

-- 3) ------------------------------------------------------------------------------------------
UPDATE [2026_web_widget] SET info = REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(info, N'href="/funds/performance" class="quick-tile quick-tile--', N'href="|||pb_t3|||" class="quick-tile quick-tile--'), N'quick-tile__title">ผลการดำเนินงานทั้งหมด<', N'quick-tile__title">|||pb_t1|||<'), N'quick-tile__text">เปรียบเทียบผลตอบแทนย้อนหลังของแต่ละกองทุน<', N'quick-tile__text">|||pb_t2|||<'), N'src="/media/images/home/tiles/performance.jpg"', N'src="/|||pb_t5|||"'), N'href="/funds/nav" class="quick-tile quick-tile--', N'href="|||pb_t9|||" class="quick-tile quick-tile--'), N'quick-tile__title">มูลค่าหน่วยลงทุนทั้งหมด<', N'quick-tile__title">|||pb_t7|||<'), N'quick-tile__text">ค้นหามูลค่าหน่วยลงทุน (NAV) ย้อนหลังของทุกกองทุน<', N'quick-tile__text">|||pb_t8|||<'), N'src="/media/images/home/tiles/nav.jpg"', N'src="/|||pb_t11|||"'), N'href="/funds/calendar" class="quick-tile quick-tile--', N'href="|||pb_t15|||" class="quick-tile quick-tile--'), N'quick-tile__title">ปฏิทินกองทุน<', N'quick-tile__title">|||pb_t13|||<'), N'quick-tile__text">ตารางวันทำการซื้อขายและวันหยุดของแต่ละกองทุน<', N'quick-tile__text">|||pb_t14|||<'), N'src="/media/images/home/tiles/calendar.jpg"', N'src="/|||pb_t17|||"'),
  pb_info = REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(pb_info, N'href="/funds/performance" class="quick-tile quick-tile--', N'href="|||pb_t3|||" class="quick-tile quick-tile--'), N'quick-tile__title">ผลการดำเนินงานทั้งหมด<', N'quick-tile__title">|||pb_t1|||<'), N'quick-tile__text">เปรียบเทียบผลตอบแทนย้อนหลังของแต่ละกองทุน<', N'quick-tile__text">|||pb_t2|||<'), N'src="/media/images/home/tiles/performance.jpg"', N'src="/|||pb_t5|||"'), N'href="/funds/nav" class="quick-tile quick-tile--', N'href="|||pb_t9|||" class="quick-tile quick-tile--'), N'quick-tile__title">มูลค่าหน่วยลงทุนทั้งหมด<', N'quick-tile__title">|||pb_t7|||<'), N'quick-tile__text">ค้นหามูลค่าหน่วยลงทุน (NAV) ย้อนหลังของทุกกองทุน<', N'quick-tile__text">|||pb_t8|||<'), N'src="/media/images/home/tiles/nav.jpg"', N'src="/|||pb_t11|||"'), N'href="/funds/calendar" class="quick-tile quick-tile--', N'href="|||pb_t15|||" class="quick-tile quick-tile--'), N'quick-tile__title">ปฏิทินกองทุน<', N'quick-tile__title">|||pb_t13|||<'), N'quick-tile__text">ตารางวันทำการซื้อขายและวันหยุดของแต่ละกองทุน<', N'quick-tile__text">|||pb_t14|||<'), N'src="/media/images/home/tiles/calendar.jpg"', N'src="/|||pb_t17|||"')
WHERE section_key = N'NavPrices';

UPDATE [2026_web_widget] SET info = REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(info, N'href="/funds/performance" class="quick-bar"', N'href="|||pb_t3|||" class="quick-bar"'), N'quick-bar__title">ผลการดำเนินงานทั้งหมด<', N'quick-bar__title">|||pb_t1|||<'), N'quick-bar__text">เปรียบเทียบผลตอบแทนย้อนหลังของแต่ละกองทุน<', N'quick-bar__text">|||pb_t2|||<'), N'src="/media/images/home/tiles/performance.jpg"', N'src="/|||pb_t5|||"'), N'href="/funds/nav" class="quick-bar"', N'href="|||pb_t9|||" class="quick-bar"'), N'quick-bar__title">มูลค่าหน่วยลงทุนทั้งหมด<', N'quick-bar__title">|||pb_t7|||<'), N'quick-bar__text">ค้นหามูลค่าหน่วยลงทุน (NAV) ย้อนหลังของทุกกองทุน<', N'quick-bar__text">|||pb_t8|||<'), N'src="/media/images/home/tiles/nav.jpg"', N'src="/|||pb_t11|||"'), N'href="/funds/calendar" class="quick-bar"', N'href="|||pb_t15|||" class="quick-bar"'), N'quick-bar__title">ปฏิทินกองทุน<', N'quick-bar__title">|||pb_t13|||<'), N'quick-bar__text">ตารางวันทำการซื้อขายและวันหยุดของแต่ละกองทุน<', N'quick-bar__text">|||pb_t14|||<'), N'src="/media/images/home/tiles/calendar.jpg"', N'src="/|||pb_t17|||"'),
  pb_info = REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(pb_info, N'href="/funds/performance" class="quick-bar"', N'href="|||pb_t3|||" class="quick-bar"'), N'quick-bar__title">ผลการดำเนินงานทั้งหมด<', N'quick-bar__title">|||pb_t1|||<'), N'quick-bar__text">เปรียบเทียบผลตอบแทนย้อนหลังของแต่ละกองทุน<', N'quick-bar__text">|||pb_t2|||<'), N'src="/media/images/home/tiles/performance.jpg"', N'src="/|||pb_t5|||"'), N'href="/funds/nav" class="quick-bar"', N'href="|||pb_t9|||" class="quick-bar"'), N'quick-bar__title">มูลค่าหน่วยลงทุนทั้งหมด<', N'quick-bar__title">|||pb_t7|||<'), N'quick-bar__text">ค้นหามูลค่าหน่วยลงทุน (NAV) ย้อนหลังของทุกกองทุน<', N'quick-bar__text">|||pb_t8|||<'), N'src="/media/images/home/tiles/nav.jpg"', N'src="/|||pb_t11|||"'), N'href="/funds/calendar" class="quick-bar"', N'href="|||pb_t15|||" class="quick-bar"'), N'quick-bar__title">ปฏิทินกองทุน<', N'quick-bar__title">|||pb_t13|||<'), N'quick-bar__text">ตารางวันทำการซื้อขายและวันหยุดของแต่ละกองทุน<', N'quick-bar__text">|||pb_t14|||<'), N'src="/media/images/home/tiles/calendar.jpg"', N'src="/|||pb_t17|||"')
WHERE section_key = N'NavPricesV2';

UPDATE [2026_web_widget] SET info = REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(info, N'href="/funds/performance" class="tile-stack__item', N'href="|||pb_t3|||" class="tile-stack__item'), N'tile-stack__title">ผลการดำเนินงานทั้งหมด<', N'tile-stack__title">|||pb_t1|||<'), N'tile-stack__text">เปรียบเทียบผลตอบแทนย้อนหลังของแต่ละกองทุน<', N'tile-stack__text">|||pb_t2|||<'), N'<i class="bi bi-graph-up-arrow" aria-hidden="true"></i>', N'<i class="bi |||pb_t6|||" aria-hidden="true"></i>'), N'href="/funds/nav" class="tile-stack__item', N'href="|||pb_t9|||" class="tile-stack__item'), N'tile-stack__title">มูลค่าหน่วยลงทุนทั้งหมด<', N'tile-stack__title">|||pb_t7|||<'), N'tile-stack__text">ค้นหามูลค่าหน่วยลงทุน (NAV) ย้อนหลังของทุกกองทุน<', N'tile-stack__text">|||pb_t8|||<'), N'<i class="bi bi-currency-exchange" aria-hidden="true"></i>', N'<i class="bi |||pb_t12|||" aria-hidden="true"></i>'), N'href="/funds/calendar" class="tile-stack__item', N'href="|||pb_t15|||" class="tile-stack__item'), N'tile-stack__title">ปฏิทินกองทุน<', N'tile-stack__title">|||pb_t13|||<'), N'tile-stack__text">ตารางวันทำการซื้อขายและวันหยุดของแต่ละกองทุน<', N'tile-stack__text">|||pb_t14|||<'), N'<i class="bi bi-calendar3" aria-hidden="true"></i>', N'<i class="bi |||pb_t18|||" aria-hidden="true"></i>'),
  pb_info = REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(pb_info, N'href="/funds/performance" class="tile-stack__item', N'href="|||pb_t3|||" class="tile-stack__item'), N'tile-stack__title">ผลการดำเนินงานทั้งหมด<', N'tile-stack__title">|||pb_t1|||<'), N'tile-stack__text">เปรียบเทียบผลตอบแทนย้อนหลังของแต่ละกองทุน<', N'tile-stack__text">|||pb_t2|||<'), N'<i class="bi bi-graph-up-arrow" aria-hidden="true"></i>', N'<i class="bi |||pb_t6|||" aria-hidden="true"></i>'), N'href="/funds/nav" class="tile-stack__item', N'href="|||pb_t9|||" class="tile-stack__item'), N'tile-stack__title">มูลค่าหน่วยลงทุนทั้งหมด<', N'tile-stack__title">|||pb_t7|||<'), N'tile-stack__text">ค้นหามูลค่าหน่วยลงทุน (NAV) ย้อนหลังของทุกกองทุน<', N'tile-stack__text">|||pb_t8|||<'), N'<i class="bi bi-currency-exchange" aria-hidden="true"></i>', N'<i class="bi |||pb_t12|||" aria-hidden="true"></i>'), N'href="/funds/calendar" class="tile-stack__item', N'href="|||pb_t15|||" class="tile-stack__item'), N'tile-stack__title">ปฏิทินกองทุน<', N'tile-stack__title">|||pb_t13|||<'), N'tile-stack__text">ตารางวันทำการซื้อขายและวันหยุดของแต่ละกองทุน<', N'tile-stack__text">|||pb_t14|||<'), N'<i class="bi bi-calendar3" aria-hidden="true"></i>', N'<i class="bi |||pb_t18|||" aria-hidden="true"></i>')
WHERE section_key = N'NavPricesV3';

COMMIT;

-- ตรวจผล: ไทล์ครบ 3 + widget ละ 12 token (ข้อความ/ลิงก์/รูปหรือไอคอน × 3 ไทล์) ไม่เหลือข้อความเดิม
SELECT id, pb_title, pb_t1, pb_t7, pb_t13 FROM [2026_web_core_single] WHERE module_id = 1 AND web_id = 0;
SELECT id, section_key, pb_mod_name, (LEN(pb_info) - LEN(REPLACE(pb_info, N'|||pb_t', N''))) / 7 AS tokens FROM [2026_web_widget] WHERE section_key LIKE N'NavPrices%' ORDER BY id;
