-- seed ข้อมูล Asset Plus: ปรับแต่ง Footer (HomeFooter) — ส่วน ALTER TABLE อยู่ใน 2026-09-17-web-home-header-footer.sql
-- คัดลอกจาก scratchpad ของ session ที่ทำงานจริงเมื่อ 17 ก.ย. 2569 เพื่อเก็บถาวร (scratchpad เป็นโฟลเดอร์ชั่วคราว)
-- ข้อมูลอ้างอิงเท่านั้น — อ้าง id ของ DB dev (แทนแถว SAM เดิม) ห้ามรันซ้ำบน dev · บนเซิร์ฟเวอร์ให้ใส่ข้อมูลผ่านหน้าหลังบ้านแทน หรือแก้ id ให้ตรงก่อนรัน

SET XACT_ABORT ON;
BEGIN TRAN;
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
SET XACT_ABORT ON;
BEGIN TRAN;
UPDATE [2026_web_home_footer] SET
    title = N'บริษัทหลักทรัพย์จัดการกองทุน แอสเซท พลัส จำกัด',
    en_title = N'Asset Plus Fund Management',
    img1 = N'Files/Site0/1/footer/logo-light.svg', en_img1 = N'Files/Site0/1/footer/logo-light.svg',
    address = N'ชั้น 17 อาคารสาธรซิตี้ทาวเวอร์ เลขที่ 175 สาทรใต้ แขวงทุ่งมหาเมฆ เขตสาทร กรุงเทพฯ 10120',
    en_address = N'17th Floor, Sathorn City Tower, 175 South Sathorn Road, Thungmahamek, Sathorn, Bangkok 10120',
    address_url = N'https://www.google.com/maps/search/?api=1&query=Sathorn+City+Tower+175+South+Sathorn+Road+Bangkok',
    tel = N'02 672 1111',
    email = N'customercare@assetfund.co.th',
    sc_ln = N'', sc_fb = N'https://www.facebook.com/aspfund.official',
    sc_yt = N'https://www.youtube.com/channel/UCzUKmISpgeBHddc2ZHlpbrA',
    sc_bd = N'https://www.blockdit.com/pages/5f0d526200e1520cc2d535b9',
    sc_tw = NULL, sc_tt = NULL,
    promo_year = N'2026', tagline_1 = N'VALUE BEYOND WEALTH', tagline_2 = N'คุณค่าที่เหนือกว่าความมั่งคั่ง',
    app_title = N'ดาวน์โหลดแอป ASP FUND', en_app_title = N'Download ASP FUND App',
    sc_as = N'', sc_gp = N'', app_qr = N'Files/Site0/1/footer/icon-app.png',
    cr = N'© 2026 Asset Plus Fund Management Co., Ltd.', en_cr = N'© 2026 Asset Plus Fund Management Co., Ltd.',
    info = NULL, en_info = NULL,
    btn1_text = NULL, en_btn1_text = NULL, btn1_url = NULL, en_btn1_url = NULL, btn1_url_target = NULL, btn1_bg_color = NULL, btn1_text_color = NULL,
    btn2_text = NULL, en_btn2_text = NULL, btn2_url = NULL, en_btn2_url = NULL, btn2_url_target = NULL, btn2_bg_color = NULL, btn2_text_color = NULL,
    btn3_text = NULL, en_btn3_text = NULL, btn3_url = NULL, en_btn3_url = NULL, btn3_url_target = NULL, btn3_bg_color = NULL, btn3_text_color = NULL,
    status = 1, pb_status = 1, show_front = 1, web_id = 0, sort = 10,
    updated_at = SYSDATETIMEOFFSET(), updated_by = N'user', approve_by = N'user'
WHERE id = 1;
-- copy draft -> approved for every content column
UPDATE [2026_web_home_footer] SET
    pb_title = title, pb_en_title = en_title, pb_img1 = img1, pb_en_img1 = en_img1,
    pb_address = address, pb_en_address = en_address, pb_address_url = address_url,
    pb_tel = tel, pb_email = email, pb_sc_ln = sc_ln, pb_sc_fb = sc_fb, pb_sc_yt = sc_yt, pb_sc_bd = sc_bd, pb_sc_tw = sc_tw, pb_sc_tt = sc_tt,
    pb_promo_year = promo_year, pb_tagline_1 = tagline_1, pb_tagline_2 = tagline_2,
    pb_app_title = app_title, pb_en_app_title = en_app_title, pb_sc_as = sc_as, pb_sc_gp = sc_gp, pb_app_qr = app_qr,
    pb_cr = cr, pb_en_cr = en_cr, pb_info = info, pb_en_info = en_info,
    pb_btn1_text = NULL, pb_en_btn1_text = NULL, pb_btn1_url = NULL, pb_en_btn1_url = NULL, pb_btn1_url_target = NULL, pb_btn1_bg_color = NULL, pb_btn1_text_color = NULL,
    pb_btn2_text = NULL, pb_en_btn2_text = NULL, pb_btn2_url = NULL, pb_en_btn2_url = NULL, pb_btn2_url_target = NULL, pb_btn2_bg_color = NULL, pb_btn2_text_color = NULL,
    pb_btn3_text = NULL, pb_en_btn3_text = NULL, pb_btn3_url = NULL, pb_en_btn3_url = NULL, pb_btn3_url_target = NULL, pb_btn3_bg_color = NULL, pb_btn3_text_color = NULL
WHERE id = 1;
COMMIT;
SELECT id, status, pb_status, show_front, pb_title, pb_en_title, pb_img1, pb_tel, pb_email, pb_sc_fb, pb_sc_bd, pb_promo_year, pb_tagline_1, pb_app_title, pb_app_qr, pb_cr FROM [2026_web_home_footer];
