# ตรวจสอบการทำงาน Back-end ทั้งหมด (admin.sam.or.th)

ตรวจจาก: โค้ด config กลาง `Areas/Admin/Helpers/AdminMenu.cs` (นิยาม table/ฟิลด์/สิทธิ์ทุกเมนู) + controller/view จริง + ข้อมูลจริงใน PostgreSQL `sam` + เปิดหน้าจริงผ่าน browser (login: user)

รวมทั้งหมด **33 กลุ่มเมนู / ~127 เมนูย่อย** ไล่ลำดับตั้งแต่เมนูแรกถึงเมนูสุดท้าย

---

## กลไกพื้นฐานที่ใช้ร่วมกันทั้งระบบ (เข้าใจตรงนี้ก่อน จะเข้าใจทุกเมนู)

ทุก controller เป็น "thin wrapper" สืบทอด `AdminCoreController` และดึง config จาก `AdminMenu.cs` → หน้า List/Add/Edit/Export ถูก generate อัตโนมัติจาก config (ตาราง, คอลัมน์, ฟิลด์ฟอร์ม, สิทธิ์)

**ประเภทของ "เครื่องยนต์เนื้อหา" (content engine) ที่เมนูส่วนใหญ่ใช้ซ้ำ:**

| ชนิดตาราง | ลักษณะ | ใช้ทำเมนูแบบไหน |
|---|---|---|
| `web_core_single` | 1 เมนู = 1 ระเบียน แก้ไขอย่างเดียว มีฟิลด์ `t1..t50`+`en_t1..50` ที่หน้า Edit จะ map เป็น "ช่องกรอกมีป้ายกำกับ 2 ภาษา จัดกลุ่มเป็น section" ตรงกับบล็อกบนหน้าเว็บจริง | หน้าเนื้อหาคงที่ (About, Debt, ข้อความหน้าแรก, template อีเมล, ค่าตั้งค่า) |
| `web_core_item` | หลายรายการการ์ดง่ายๆ (cat_id, title, des, img1, url, sort) | รายการซ้ำๆ เช่น กรรมการ, รายงานประจำปี, สไลด์ |
| `web_core_group` | หมวด/กลุ่ม (title + t1..t30) เป็น parent | หมวดหมู่ให้เมนูอื่นอ้างอิง (dropdown) |
| `web_core_news` | บทความเต็ม (title, date, รูป, ไฟล์แนบ 6 ไฟล์, des, info rich-text, seo_url) | ข่าว/บทความ/ดาวน์โหลด/ประกาศ |

**Workflow อนุมัติ (approval):** ทุกโมดูลที่มี `CanApprove=true` ใช้คอลัมน์คู่ขนาน `pb_*` (pending) — เมื่อแก้ไข ข้อมูลจะเข้าคิว "รออนุมัติ" ในฟิลด์ `pb_` และหน้าเว็บจริงยังแสดงค่าเดิม จนกด **Approve** ค่าจึงถูก copy จาก `pb_*` → ฟิลด์จริง (ตรงกับที่หน้า List แสดงป้าย "Published / รออนุมัติ")

**เมนูรายการ submission (ผู้ใช้กรอกฟอร์มจากหน้าเว็บ):** ตั้ง `CanAdd/Edit/Approve=false` เหลือ **ดู Detail + Export Excel + ลบ** เท่านั้น

**สิทธิ์:** เมนู "สิทธิ์การใช้" กำหนด can-add/edit/delete/move/status/export/approve รายโมดูลให้แต่ละ role → เก็บใน `web_admin_module` และ filter `[ModuleCheck]` ตรวจทุก request

---

## 1) หน้าเว็บไซต์ (8 เมนู)

**1. จัดการเมนูเว็บไซต์ (CMSPage)** — `web_cms_page`
สร้าง/จัดโครงสร้างเมนู+หน้าเพจของเว็บแบบ tree หลายระดับ พร้อม page builder
- Add/Edit: หัวข้อ TH/EN, Friendly URL (seo_url), รายละเอียดย่อ, `page_type` (1=Group Menu, 2=URL Link, 3=Page Content, 4=System Link), เนื้อหา rich-text, SEO keyword/description, Script Header/Body, วันเริ่ม/สิ้นสุดแสดงผล
- พิเศษ: เลือก page_type แล้วสลับฟอร์มย่อย; แท็บ "ตกแต่งเพจ" = **drag-drop builder** (SortableJS) ลาก Widget วางในพื้นที่ แก้ผ่าน iframe แล้วเก็บลำดับใน `box_layout`/`box_data`; มี Add/Edit/Delete/Move/Status/Approve ครบ (ข้อมูลจริง 51 หน้า: type4=37, type1=11, type3=3)

**2. หน้า Intro Page (HomeIntroPage)** — `web_home_intro_page`
หน้าต้อนรับ (splash) ก่อนเข้าเว็บ ในโอกาสพิเศษ
- Add/Edit: หัวข้อ, สีพื้นหลัง (color picker), รูป 1120×630, เนื้อหา rich-text, ปุ่มเข้าเว็บ 3 ปุ่ม (ข้อความ/สี/URL), วันเริ่ม-สิ้นสุด (ตั้งเวลาแสดงล่วงหน้า)

**3. หน้า Pop-Up (HomePopUp)** — `web_home_pop_up`
ป๊อปอัปหน้าแรก
- Add/Edit: หัวข้อ, รูป 800×440, URL TH/EN + target, วันเริ่ม-สิ้นสุดแสดงผล

**4. โลโก้ / Template (HomeHeader)** — `web_home_header` (แก้ไขอย่างเดียว)
ตั้งค่าโลโก้และเทมเพลตส่วนหัว
- Edit: อัปโหลดโลโก้ (132×30), เลือก Template 1/2/3, แถบสีหลัก (สว่าง/มืด); มีล็อกแถวกันแก้ซ้อน

**5. จัดการเมนู Footer 1 (CMSPageFooter1)** — `web_cms_page_footer1`
จัดเมนูลิงก์คอลัมน์ Footer ชุดที่ 1 (ฟอร์มเดียวกับ CMSPage แต่ซ่อน "Group Menu" บังคับเป็น Page Content)

**6. จัดการเมนู Footer 2 (CMSPageFooter2)** — `web_cms_page_footer2`
เหมือนข้อ 5 แต่เป็นคอลัมน์ Footer ชุดที่ 2

**7. ปรับแต่ง Footer (HomeFooter)** — `web_home_footer` (แก้ไขอย่างเดียว)
ตั้งค่าเนื้อหาส่วนท้ายเว็บ
- Edit: เบอร์ Call Center, รูป, URL Facebook/Twitter/LINE/Youtube, ลิงก์ App Store/Google Play, Copyright TH/EN

**8. SEO & Code (HomeSEO)** — `web_home_seo` (แก้ไขอย่างเดียว)
ตั้ง Meta SEO + สคริปต์ติดตามทั้งไซต์
- Edit: Title/Description/Keyword TH+EN; Tracking script แยก Header/Body สำหรับ Google Analytics, Facebook Pixel, และ Other 1-3

---

## 2) ข้อมูลหน้าแรก (9 เมนู)

**9. รูปสไลด์หน้าแรก (HomeImageSlide)** — `web_core_item`
จัดการแบนเนอร์สไลด์หน้าแรก (แต่ละสไลด์ = 1 รายการ, CRUD ครบ + เรียงลำดับ)
- Add/Edit: หัวข้อ TH/EN, รูป PC 1920×1080, รูป Mobile 750×1040, URL TH/EN + target, วันเริ่ม-สิ้นสุด

**10. ตั้งค่ารูปสไลด์ (HomeImageConf)** — `web_home_image_conf` (แก้ไขอย่างเดียว)
ตั้งค่าพฤติกรรม slider (ไม่ใช่ตัวรูป)
- Edit: `effect` (Swiper: slide/fade/cube/coverflow/flip/cards/creative), `speed` (วินาที)

**11–17. SAM ใส่ใจ / ภาพรวมการบริหารหนี้ / ปิดหนี้ไว ไปต่อได้ / ทรัพย์เด่นและน่าสนใจ / คุณกำลังมองหาอะไร / บ้านเด่นทำเลดี / วิสัยทัศน์** (HomeSamText, HomeSamText2–7) — `web_core_single` (แก้ไขอย่างเดียว)
แต่ละอันคือ **1 บล็อกเนื้อหาบนหน้าแรก** แก้ผ่านช่องกรอกมีป้ายกำกับ 2 ภาษา (หัวข้อ/รายละเอียด/รูป/ปุ่ม ตามดีไซน์บล็อกนั้น) — มี approval

---

## 3) เกี่ยวกับเรา (16 เมนู)

**18. วิสัยทัศน์/พันธกิจ (AboutVision)** — `web_core_single` (แก้ไข) — บล็อกเนื้อหาหน้า "เกี่ยวกับเรา"
**19. ประวัติความเป็นมา (AboutHistory)** — `web_core_single` (แก้ไข) — timeline/ประวัติองค์กร
**20. โครงสร้างองค์กร (AboutStructure)** — `web_core_item` (แก้ไข) — ผังโครงสร้าง (รายการ)
**21. รายงานประจำปี (AboutAnnual)** — `web_core_item` (CRUD) — List: ปี, รูป; แต่ละรายการ = 1 ปี พร้อมไฟล์/รูปปก
**22. รายงานการเงิน (AboutFin)** — `web_core_item` (CRUD) — เหมือนรายงานประจำปี (รายปี)
**23. คณะกรรมการบริษัท (AboutBoard1)** — `web_core_item` (CRUD) — List: ชื่อ, รูป; การ์ดกรรมการ (ชื่อ/ตำแหน่ง/รูป)
**24. คณะกรรมการอื่นๆ (กลุ่ม) (AboutBoard2cat)** — `web_core_group` — หมวดของกรรมการอื่นๆ
**25. คณะกรรมการอื่นๆ (AboutBoard2)** — `web_core_item` (CRUD) — รายชื่อกรรมการอื่นๆ (ผูก cat จากข้อ 24)
**26. คณะผู้บริหารระดับสูง (กลุ่ม) (AboutBoard3cat)** — `web_core_group` — หมวดผู้บริหารระดับสูง
**27. คณะผู้บริหารระดับสูง (AboutBoard3)** — `web_core_item` (CRUD) — การ์ดผู้บริหารระดับสูง
**28. คณะผู้บริหารฝ่าย (สายงาน) (AboutBoard4cat)** — `web_core_group` — สายงาน (ระดับ 1)
**29. คณะผู้บริหารฝ่าย (กลุ่ม) (AboutBoard4sub)** — `web_core_group` — กลุ่มย่อยในสายงาน (ระดับ 2)
**30. คณะผู้บริหารฝ่าย (AboutBoard4)** — `web_core_item` (CRUD) — การ์ดผู้บริหารฝ่าย (โครง 3 ระดับ: สายงาน→กลุ่ม→คน)
**31. บริหารสินทรัพย์ด้อยคุณภาพ (AboutText1)** — `web_core_single` (แก้ไข) — บล็อกเนื้อหาบริการ
**32. บริหารทรัพย์สินรอการขาย (AboutText2)** — `web_core_single` (แก้ไข) — บล็อกเนื้อหาบริการ
**33. สร้างโอกาสทางธุรกิจ บสส. (AboutText3)** — `web_core_single` (แก้ไข) — บล็อกเนื้อหาบริการ

---

## 4) ทรัพย์สินรอการขาย (1 เมนู)

**34. ขั้นตอนการซื้อทรัพย์ (NpaText1)** — `web_core_single` (แก้ไข) — บล็อกเนื้อหาอธิบายขั้นตอนการซื้อทรัพย์ NPA

---

## 5) บริหารหนี้ด้อยคุณภาพ (5 เมนู)

**35. ภาพรวมการบริหารหนี้ (DebtText1)** — `web_core_single` (แก้ไข)
ตัวอย่างจริงจากหน้า Edit: จัดเป็น section — แบนเนอร์ส่วนหัว (หัวข้อ/รูป/รายละเอียด 2 ภาษา), หัวข้อ/รายละเอียด, **กราฟข้อมูล** (label+ค่า เช่น ประนอมหนี้ 70 / ขายทอดตลาด 30), **ตารางสถิติ** (150,000+ ครอบครัว ฯลฯ)
**36. ขั้นตอนการปรับโครงสร้างหนี้ (DebtText2)** — `web_core_single` (แก้ไข)
**37. สิทธิประโยชน์ลูกหนี้ (DebtText3)** — `web_core_single` (แก้ไข)
**38. ลงทะเบียนปรับโครงสร้างหนี้ (DebtText5)** — `web_core_single` (แก้ไข) — เนื้อหา/คำอธิบายหน้าลงทะเบียน NPL
**39. คลินิกแก้หนี้ (DebtText6)** — `web_core_single` (แก้ไข) — เนื้อหาโครงการคลินิกแก้หนี้
*(DebtText4 "ดาวน์โหลดแบบฟอร์ม" ถูกคอมเมนต์ออกจากเมนู)*

---

## 6) ข่าวประชาสัมพันธ์ (2 เมนู)

**40. ข่าวประชาสัมพันธ์ (กลุ่ม) (NewsGroup)** — `web_core_group` — หมวดข่าว
**41. ข่าวประชาสัมพันธ์ (News)** — `web_core_news` (CRUD)
- Add/Edit: หมวด, หัวข้อ TH/EN, วันที่, รูป, ไฟล์แนบ 6 ไฟล์, เนื้อหา rich-text, seo_url — List มีรูปประกอบ + approval

---

## 7) บทความและวารสาร (2 เมนู)

**42. บทความและวารสาร (กลุ่ม) (ArticleGroup)** — `web_core_group` — หมวดบทความ
**43. บทความและวารสาร (Article)** — `web_core_news` (CRUD) — เหมือน News (บทความ/วารสาร มีไฟล์ดาวน์โหลด)

---

## 8) วิดีโอ/สื่อประชาสัมพันธ์ (3 เมนู)

**44. รายการ (กลุ่ม) (VDOGroup)** — `web_core_group` — หมวดหลัก
**45. รายการ (VDOGroupsub)** — `web_core_group` — กลุ่มย่อย (โครง 2 ระดับ)
**46. วิดีโอ/สื่อประชาสัมพันธ์ (VDO)** — `web_core_news` (CRUD) — List แสดงคอลัมน์ VDO; เก็บ URL วิดีโอ (YouTube ฯลฯ)

---

## 9) ประกาศ (4 เมนู)

**47. ประกาศอัตราดอกเบี้ย (AnnounceRate)** — `web_core_news` (CRUD) — ประกาศอัตราดอกเบี้ย (มีไฟล์แนบ)
**48. ประกาศจาก NPL/NPA (AnnounceNPA)** — `web_core_news` (CRUD) — ใช้ฟอร์มเดียวกับ AnnounceRate
**49. ประกาศทั่วไป (กลุ่ม) (AnnounceOtherGroup)** — `web_core_group` — หมวดประกาศทั่วไป
**50. ประกาศทั่วไป (AnnounceOther)** — `web_core_news` (CRUD) — ประกาศทั่วไป

---

## 10) ประกาศจัดซื้อจัดจ้าง (4 เมนู)

**51. ประกาศจัดซื้อจัดจ้าง (AnnouncePro)** — `web_core_news` (CRUD) — ประกาศจัดซื้อจัดจ้าง (แนบไฟล์ TOR/เอกสาร)
**52. ประเภท (AnnounceProGroup)** — `web_core_group` — ประเภทการจัดซื้อ
**53. วิธีการจัดซื้อ (AnnounceProType)** — `web_core_group` — วิธีการจัดซื้อ (dropdown)
**54. รายชื่อผู้ลงทะเบียน (AnnounceSubmit)** — `web_announce_submit` (ดู Detail + Export + ลบ)
- ผู้กรอกข้อมูลจากประกาศ; Detail/Export: เลขผู้เสียภาษี, ชื่อ-สกุล, ชื่อสถานประกอบการ, ที่อยู่เต็ม, เบอร์, email, ไฟล์อัปโหลด + สถานะไฟล์ *(view นี้เป็นต้นแบบที่หลายเมนู submission ใช้ร่วม)*

---

## 11) ร่วมงานกับเรา (3 เมนู)

**55. ประกาศรับสมัครงาน (AnnounceJob)** — `web_core_news` (CRUD) — ประกาศตำแหน่งงาน (รายละเอียด/คุณสมบัติ)
**56. รายชื่อผู้สมัครงาน (AnnounceJobSubmitFull)** — `web_job_submit_full` (ดู Detail + Export + ลบ)
- ใบสมัครงานฉบับเต็ม อ่านจาก `form_json`: ข้อมูลส่วนตัว, ครอบครัว (บิดา/มารดา/พี่น้อง/สมรส/บุตร), การศึกษา, ประวัติงาน, ฝึกอบรม, ภาษา/คอมพิวเตอร์, บุคคลอ้างอิง, ผู้ติดต่อฉุกเฉิน, เอกสารแนบ (resume), คำรับรอง, **ข้อมูลอ่อนไหว** (ศาสนา/กรุ๊ปเลือด/สุขภาพ/ประวัติคดี)
- พิเศษ: Export Excel ชุดใหญ่ (คลี่ทุกฟิลด์ form_json เป็นคอลัมน์)
**57. วัฒนธรรมองค์กร (JobCMS)** — `web_core_single` (แก้ไข) — เนื้อหาวัฒนธรรมองค์กรหน้าร่วมงาน

---

## 12) ติดต่อเรา (3 เมนู)

**58. สำนักงานใหญ่/สาขา (ContactOffice)** — `web_core_news` (CRUD) — ข้อมูลที่ตั้งสาขา (ที่อยู่/แผนที่/เบอร์)
**59. ช่องทางร้องเรียน/ข้อเสนอแนะ (ContactCMS)** — `web_core_single` (แก้ไข) — เนื้อหาช่องทางร้องเรียน
**60. รายชื่อผู้ติดต่อ (ContactSubmit)** — `web_contact_submit` (ดู Detail + Export + ลบ)
- ผู้กรอกฟอร์มติดต่อเรา; Detail: คำนำหน้า, ชื่อ-สกุล, เบอร์, email, ข้อความ

---

## 13) ดาวน์โหลด (2 เมนู)

**61. ดาวน์โหลด (กลุ่ม) (DownloadGroup)** — `web_core_group` — หมวดไฟล์ดาวน์โหลด
**62. ดาวน์โหลด (Download)** — `web_core_news` (CRUD) — ไฟล์เอกสาร/แบบฟอร์มให้ดาวน์โหลด (แนบไฟล์ได้หลายไฟล์)
*(DownloadSubmit "รายชื่อผู้กรอกข้อมูลก่อนดาวน์โหลด" มีในโค้ดแต่ถูกคอมเมนต์ออกจากเมนู)*

---

## 14) คำถามที่พบบ่อย (2 เมนู)

**63. คำถามที่พบบ่อย (กลุ่ม) (FAQGroup)** — `web_core_group` — หมวด FAQ
**64. คำถามที่พบบ่อย (FAQ)** — `web_core_news` (CRUD) — คู่คำถาม-คำตอบ (จัดหมวด + ค้นหาได้)

---

## 15) ลิงค์หน่วยงาน (4 เมนู)

**65. ลิงค์หน่วยงาน (กลุ่ม) (LinkGroup)** — `web_core_group` — หมวดลิงก์
**66. ลิงค์หน่วยงาน (Link)** — `web_core_news` (CRUD) — ลิงก์หน่วยงานภายนอก (โลโก้ + URL)
**67. แลกเปลี่ยนลิงค์ (LinkCMS)** — `web_core_single` (แก้ไข) — เนื้อหาหน้าแลกลิงก์
**68. รายชื่อผู้แลกเปลี่ยน (LinkSubmit)** — `web_link_submit` (ดู Detail + Export + ลบ) — ผู้ขอแลกลิงก์: ชื่อ, email, URL, ไฟล์ banner

---

## 16) โปรโมชั่น (2 เมนู)

**69. โปรโมชั่น (กลุ่ม) (PromotionGroup)** — `web_core_group` — หมวดโปรโมชัน
**70. โปรโมชั่น (Promotion)** — `web_core_news` (CRUD) — รายการโปรโมชัน (รูป/รายละเอียด/ช่วงเวลา)

---

## 17) Banner (2 เมนู) — ⚠️ ไม่ทำงาน

**71. กลุ่ม Banner (BannerGroup)** — `web_banner_group`
**72. Banner ทั้งหมด (Banner)** — `web_banner`
- ⚠️ **ยืนยันจาก browser: เปิดแล้ว Internal Server Error (500)** เพราะตาราง `web_banner`/`web_banner_group` **ไม่มีในฐานข้อมูล** และ config มีแค่ฟิลด์ `title`,`sort` ไม่มี view เฉพาะ → เป็นโมดูล **stub ที่ยังไม่ implement / ใช้งานไม่ได้ในสถานะปัจจุบัน**

---

## 18) E-Form (3 เมนู)

**73. E-Form (กลุ่ม) (EFormGroup)** — `web_core_group` — หมวดของฟอร์ม
**74. E-Form (EForm)** — `web_eform` (CRUD)
- **ตัวสร้างฟอร์มออนไลน์แบบลากวาง** (jQuery-formBuilder 3.8.3); ชนิดฟิลด์: header/paragraph/text/textarea/number/date/checkbox/radio/select/file/autocomplete; เก็บโครงฟอร์มเป็น JSON ใน `input_element`
- Add/Edit: กลุ่ม, หัวข้อ TH/EN, คำอธิบาย, ตัว builder, ข้อความปุ่มส่ง, ข้อความขอบคุณ, **อีเมลเจ้าหน้าที่รับแจ้ง** (คั่นด้วยคอมมา), วันเริ่ม-สิ้นสุด (ข้อมูลจริง 7 ฟอร์ม)
**75. รายชื่อผู้กรอกแบบฟอร์ม (EFormSubmit)** — `web_eform_submit` (ดู Detail + Export + ลบ, ค้นช่วงวันที่)
- List: แบบฟอร์ม, สรุปข้อมูลที่กรอก, IP, วันที่ส่ง; Detail: field/value + UTM tracking + IP
- พิเศษ: **Export Excel แบบ pivot** (แต่ละคำถาม = 1 คอลัมน์, ไฟล์แนบเป็น URL) ผ่าน `EFormSubmitHelper` (ข้อมูลจริง 41 รายการ)

---

## 19) เมนู Footer (6 เมนู)

**76. แบบสำรวจ (FooterPoll)** — `web_core_single` (CRUD — หลายระเบียน)
- แบบสำรวจ/โพลหน้าเว็บ (ข้อมูลจริง: คำถาม 44–53 เช่น "ความพึงพอใจต่อเว็บไซต์", "ท่านรู้จัก SAM จากช่องทางใด") — ใช้ view PollVote
**77. ประกาศความเป็นส่วนตัว (FooterPrivacy1)** — `web_core_single` (แก้ไข) — เนื้อหานโยบายความเป็นส่วนตัว
**78. รายชื่อผู้ยินยอม (FooterPrivacy1Submit)** — `web_privacy1_submit` (ดู Detail + Export + ลบ)
- หลักฐาน consent: Browser, IP Address, วันที่
**79. ข้อตกลงและเงื่อนไข (FooterPrivacy2)** — `web_core_single` (แก้ไข) — เนื้อหาข้อตกลง/เงื่อนไข
**80. รายชื่อผู้ยินยอม (FooterPrivacy2Submit)** — `web_privacy2_submit` (ดู Detail + Export + ลบ) — เหมือนข้อ 78
**81. เปิดเผยข้อมูลความโปร่งใส (FooterLink)** — `web_footer_link` (CRUD, ใช้ view CMSPage)
- ลิงก์ Footer แบบเปิดเผยข้อมูล (ITA); page_type เพิ่ม "File Link" อัปโหลด PDF ≤20MB

---

## 20) สมาชิก (4 เมนู)

**82. จัดการสมาชิก (MemberList)** — `web_member` (แก้ไข + ลบ + Export + Detail)
- List: ชื่อ-สกุล, ประเภทสมาชิก, ประเภท, สถานะยืนยันอีเมล, วันที่สมัคร; Edit: member_type, type, ยืนยันอีเมล, ชื่อ/สกุล, email, มือถือ, LINE UUID, password
- พิเศษ: password hash BCrypt (เว้นว่าง=ไม่เปลี่ยน), ajax `SearchMember`, รองรับ LINE login (ข้อมูลจริง 5 สมาชิก)
**83. รายงานสมาชิก (MemberReport)** — dashboard สถิติ (อ่านอย่างเดียว)
- การ์ดตัวเลข member+agent (ทั้งหมด/active/ยืนยันอีเมล/รายวัน-เดือน) + กราฟแนวโน้ม 6 เดือน
**84. อีเมลเจ้าหน้าที่แจ้งเตือน (MemberEmail)** — `web_core_single` (แก้ไข)
- ตั้งอีเมล CC เจ้าหน้าที่ที่รับแจ้งเตือน (ใช้ตอนส่งอีเมลต้อนรับสมาชิก/agent)
**85. ส่งข่าวสารสมาชิก (MemberNews)** — `web_member` (ฟอร์มส่งอีเมล)
- เลือกผู้รับ (ทุกอีเมล/กำหนดเอง) + ประเภทสมาชิก (สมาชิก/Investor/Agency/Sale), หัวเรื่อง, เนื้อหา HTML → **ส่ง SMTP จริง** (MailKit) + log + หน้าสรุปผล

---

## 21) ตัวแทนขายทรัพย์ (Agent) (5 เมนู)

**86. รายชื่อผู้สมัครตัวแทน (AgentList)** — `web_agent` (แก้ไข + อนุมัติ + ลบ + Export + Detail)
- List: รหัส, ชื่อ, นามสกุล, เลขบัตร/ภาษี, สถานะยืนยัน, วันที่สมัคร; Detail/Edit: ที่อยู่ทะเบียนบ้าน+ติดต่อ (แยกหมู่/ซอย/ถนน/ตำบล/อำเภอ/จังหวัด/ไปรษณีย์), ไฟล์แนบ 1-4, รหัส NPA ที่สนใจ 1-30 (จำนวนช่องมาจากค่าคงที่ `AdminMenu.NpaSlotCount` ที่เดียว), approved
- พิเศษ: ajax `UpdateApproved`; เมื่ออนุมัติ→**ส่งอีเมลต้อนรับ**ครั้งเดียว + sync member_type; ajax dropdown อำเภอ/ตำบล (ข้อมูลจริง 2 ราย)
**87. นำเข้าข้อมูลผู้สมัคร (AgentImport)** — นำเข้าไฟล์ Excel
- `DownloadTemplate` สร้าง .xlsx 61 คอลัมน์ (9 สมาชิก + 22 ตัวแทน + รหัส NPA 30 ช่องอยู่ขวาสุด คอลัมน์ 32-61); `Import` อ่าน Excel, ตรวจซ้ำ username/email/idcard, gen รหัสผ่าน+BCrypt+codeid, INSERT ทั้ง web_member+web_agent ใน transaction, คืนรายงานสำเร็จ/ข้าม
**88. รายงานตัวแทน (AgentReport)** — dashboard สถิติ (เหมือน MemberReport)
**89. อีเมลเจ้าหน้าที่แจ้งเตือน (AgentEmail)** — `web_core_single` (แก้ไข) — อีเมล CC เจ้าหน้าที่ฝั่งตัวแทน
**90. ส่งข่าวสารเอเจนต์ (AgentNews)** — `web_agent` (ฟอร์มส่งอีเมล)
- ส่งอีเมลถึงตัวแทน (ดึง email จาก member ที่ join agent status=1 หรือกำหนดเอง) → SMTP + log + สรุปผล

---

## 22) ข้อมูลทรัพย์สิน (NPA) (6 เมนู)

**91. กำหนดสถานะทรัพย์สิน (NpaStatus)** — `web_npa_status` (CRUD)
- ตั้ง Highlight/แนะนำ + ผูกย่าน/ทำเล + Facility ให้ทรัพย์ NPA
- Add/Edit: ค้นหา NPA (autocomplete), highlight (ใช่/ไม่), suggest, group_id (ย่าน), facility_id
- พิเศษ: `SearchProduct` เปิด **MySQL (`sam_npa`) ค้นตาราง `tb_product2`** คืน JSON autocomplete (ข้อมูลจริง: รหัสทรัพย์ 2T1095, 2E0042…)
**92. รายชื่อทรัพย์ Wishlist (NpaFav)** — `web_npa_fav` (CRUD)
- ผูกทรัพย์ NPA กับสมาชิก (favorites); autocomplete 2 ชุด (NPA เรียก NpaStatus/SearchProduct, สมาชิกเรียก MemberList/SearchMember)
**93. รายการ ย่าน/ทำเล (NpaLocate)** — `web_core_group` (module 18)
- master ย่าน/ทำเล (ข้อมูลจริง: รถไฟสายสีเขียว, แอร์พอร์ตลิงก์, ติดถนนสายหลัก…) ใช้เป็น dropdown ใน NpaStatus
**94. รายการ Facility (NpaFacility)** — `web_core_group` (module 19) — master สิ่งอำนวยความสะดวก
**95. ยื่นข้อเสนอซื้อ (NpaOffer)** — `web_npa_offer` (ดู Detail + Export + ลบ)
- ผู้ยื่นข้อเสนอซื้อทรัพย์; Detail: รหัสทรัพย์, ชื่อ-สกุล, เบอร์, email, **ราคาที่เสนอ**, เอกสารแนบ
**95.1 อีเมลเจ้าหน้าที่แจ้งเตือน (NpaOfferEmail)** — `web_core_single` (module 50, แก้ไข) — อีเมลเจ้าหน้าที่ที่รับแจ้งเตือนของ **ยื่นข้อเสนอซื้อ** โดยเฉพาะ
- front-end `/th/asset-offer` อ่าน `pb_t2` (fallback `t2`) → อีเมลแรก = To ที่เหลือ = CC; ไม่ตั้งค่า = ไม่ส่ง (log warning) แต่ยังบันทึกข้อมูลปกติ
- คนละระเบียนกับ BuyNPAEmail (module 31) และเมนูอีเมลแจ้งเตือนอื่น ๆ ทั้งหมด

---

## 23) ลูกค้าปรับโครงสร้างหนี้ (2 เมนู)

**96. รายชื่อผู้ลงทะเบียน (NPL) (NplRegister)** — `web_npl_register` (ดู Detail + Export + ลบ)
- List: ชื่อ, นามสกุล, มือถือ, อีเมล, วันที่; Detail: + ธนาคารเจ้าหนี้เดิม, เลขบัตร, รหัสลูกหนี้, วัตถุประสงค์/ช่วงเวลาที่สะดวกติดต่อ
**97. อีเมลเจ้าหน้าที่แจ้งเตือน (NplEmail)** — `web_core_single` (แก้ไข) — อีเมล CC เจ้าหน้าที่ NPL

---

## 24) โครงการคลินิกแก้หนี้ (1 เมนู)

**98. แก้ไขรายละเอียดโครงการ (CliWeb)** — `web_core_single` (แก้ไข) — เนื้อหาหน้าโครงการคลินิกแก้หนี้ (เชื่อมไป debtclinicbysam.com)

---

## 25) แจ้งความประสงค์ซื้อทรัพย์ (2 เมนู)

**99. รายการผู้สนใจซื้อทรัพย์ (BuyNPA)** — `web_npa_buy` (ลบอย่างเดียว) — รายการผู้แจ้งความประสงค์ซื้อทรัพย์
**100. อีเมลเจ้าหน้าที่แจ้งเตือน (BuyNPAEmail)** — `web_core_single` (แก้ไข) — อีเมล CC เจ้าหน้าที่

---

## 26) นัดหมายชมทรัพย์ (2 เมนู)

**101. รายการนัดหมายชมทรัพย์ (MeetNPA)** — `web_meet_npa` — ⚠️ **ไม่ทำงาน**
- ยืนยันจาก browser: เปิดแล้ว error; ตาราง `web_meet_npa` **ไม่มีในฐานข้อมูล** และไม่มี controller เฉพาะ → route ใช้งานไม่ได้ (config ตั้ง CRUD ไว้แต่มีแค่ฟิลด์ generic)
**102. อีเมลเจ้าหน้าที่แจ้งเตือน (MeetNPAEmail)** — `web_core_single` (แก้ไข) — อีเมล CC เจ้าหน้าที่นัดชมทรัพย์ (ตัวนี้ทำงานปกติ)

---

## 27) Subscription (3 เมนู)

**103. รายชื่อผู้ลงทะเบียน (Subscription)** — `web_subscription` (เปลี่ยนสถานะ + ลบ)
- รายชื่อผู้ subscribe รับข่าวสาร; List: E-mail, วันที่ (ข้อมูลจริง 3 ราย)
**104. ส่งข่าวสารทางอีเมล (SubscriptionEmail)** — `web_subscription` (ฟอร์มส่งอีเมล)
- ผู้รับ (ทั้งหมด/กำหนดเอง), หัวเรื่อง, เนื้อหา HTML, **แนบไฟล์ได้ 3 ไฟล์** → ส่ง SMTP + บันทึกประวัติลง `web_subscription_news` + สรุปผล
**105. ข่าวสารทางอีเมลย้อนหลัง (SubscriptionNews)** — `web_subscription_news` (ดู Detail + ลบ)
- ประวัติการส่ง; List: หัวเรื่อง, ประเภทผู้รับ, จำนวน, สำเร็จ, วันที่; Detail: total/success/fail + เนื้อหา + ไฟล์แนบ

---

## 28) Monitor (3 เมนู)

**106. Server Health (ServerStatus)** — health check จริง (dashboard)
- แสดง uptime, หน่วยความจำ, thread, response time, environment, สถานะ DB
- พิเศษ: ajax `CheckWebServerStatus` (อ่าน Process ปัจจุบัน, เตือนถ้า >500ms) + `CheckDbStatus` (Npgsql `SELECT 1` วัดเวลา)
**107. Error Logs (ErrorLogs)** — เปิดดูไฟล์ log ในโฟลเดอร์ `/Logs`
- List: ชื่อไฟล์, ขนาด, วันที่; อ่านเนื้อหาผ่าน ajax จำกัด 5MB + กัน path traversal
**108. Google Analytics (GoogleAnalytics)** — ⚠️ **stub**
- config มีแค่ `title,sort` ไม่มี dashboard/เชื่อม GA API จริงในโค้ด → ยังไม่ implement

---

## 29) Setting (4 เมนู)

**109. File Manager (FileManager)** — `web_file_manager` — คลังไฟล์/รูปกลางของระบบ (ถูกเรียกผ่าน helper `InputFileManeger` ในโมดูลอื่น เวลาเลือกรูป/ไฟล์)
**110. Google Map Key (GoogleKey)** — `web_core_single` (แก้ไข) — เก็บ Google Map API key (textarea)
**111. AI Settings (AISetting)** — `web_core_single` (แก้ไข)
- เลือก API Service (OpenGPT/Gemini/Claude), ApiKey, เปิด/ปิด AI Search
**112. LDAP Azure AD (LDAPSetting)** — `web_core_single` (แก้ไข)
- ตั้งค่าเชื่อม LDAP/Azure AD: URL, Port, Base DN, Username, Password, SSL

---

## 30) Landing / Microsite (3 เมนู) — ⚠️ ยังไม่สมบูรณ์

**113. Microsite (Microsite)** — `web_microsite`
- เปิดหน้า List ได้ (ตารางมีจริง) แต่ config ฟอร์มมีแค่ `title,sort` → **หน้า Add/Edit ยังเป็น placeholder** (ยังไม่ลงฟิลด์ landing page จริง แม้ตารางจะมีคอลัมน์เนื้อหา/pb_* ครบ)
**114. Lead Form (MicrositeForm)** — `web_microsite_form` — placeholder (title,sort เท่านั้น)
**115. รายชื่อผู้กรอกข้อมูล (MicrositeSubmit)** — `web_microsite_submit` — placeholder (title,sort เท่านั้น)

---

## 31) Logs (7 เมนู) — viewer อ่านอย่างเดียว + Export Excel

query ตาราง log ใน PostgreSQL ตรง + filter panel + pagination + ExportExcel (EPPlus)

**116. Activities สมาชิก (LogsMember)** — `web_member_activity_log` — วันที่, member, activity code/name, category, severity, status, IP, device/browser/os, location, duration, method, url, description, error
**117. ให้ความยินยอม PDPA (LogsPDPA)** — `web_pdpa_consent` — consent code/title, ยินยอม/ไม่, policy_version, IP, user_agent, วันที่
**118. AI Services API (LogsAI)** — `api_service_logs` — service, api, member, model, tokens, HTTP status, IP, duration, success, error
**119. CRM Data API (LogsCRM)** — `api_crm_logs` — service, module/function, endpoint, status, success, duration, ip, member, trace_id, environment, error
**120. LINE LIFF Data API (LogsLINE)** — `api_audit_log` — endpoint, success, uid, detail, ip, error_code
**121. Google Map API (LogsGoogleMap)** — `api_google_map_logs` — api_name, endpoint, method, response_status, success, response_time, ip, error
**121.1 ค้นหาทรัพย์ NPA (LogsNpaSearch)** — `api_npa_search_log` — **คำค้นหาเป็นข้อมูลหลัก**: keyword, search_mode, lang, filters (jsonb), sort, page, result_count, query_string, ip, user_agent, referer
- front-end เขียน log ทุกครั้งที่ค้นหาทรัพย์ (`HomeController.LogNpaSearch`) และลบ log เก่ากว่า **30 วัน** อัตโนมัติ → หน้านี้ดูย้อนหลังได้ 30 วัน
- นอกจากตาราง log ยังมี **อันดับคำค้นหายอดนิยม 10 อันดับ** + **คำค้นหาที่ค้นแล้วไม่พบผลลัพธ์** (กดคำเพื่อเจาะดูได้ โดยคงตัวกรองเดิม)
- ค้นหาได้ด้วย: ข้อความค้นหา (ILIKE + ไฮไลต์คำที่ตรง), มี/ไม่มีข้อความค้นหา, พบ/ไม่พบผลลัพธ์, ประเภทการค้นหา, ภาษา, ตัวกรองที่ใช้ (รายคีย์ / มี / ไม่มี), Client IP, ช่วงวันที่
- Export Excel 2 ชีต: รายการค้นหา + สรุปคำค้นหายอดนิยม (ตามเงื่อนไขที่กรองอยู่)

---

## 32) ผู้ดูแลระบบ (4 เมนู)

**122. สิทธิ์การใช้ (AdminAccess)** — `web_admin_access` (CRUD)
- สร้าง "บทบาทสิทธิ์" (role) + เมทริกซ์ checkbox สิทธิ์รายโมดูล (add/edit/delete/move/status/export/approve) มี "เลือกทั้งหมด"
- พิเศษ: บันทึกลง `web_admin_module` + เขียน audit log ทุกครั้ง (ข้อมูลจริง: role "Super Admin")
**123. ผู้ดูแลระบบ (AdminUser)** — `web_admin` (CRUD)
- บัญชีแอดมิน: บทบาท, username, password, ชื่อ/สกุล, ฝ่าย, email, ใช้ OTP
- พิเศษ: password **SHA512**; ตรวจซ้ำรหัสผ่านเดิม (`web_admin_password_log`), เช็ค username ซ้ำ, ลบตัวเองไม่ได้, อัปเดต session ตัวเองเมื่อแก้บัญชีตน
**124. Admin Logs (AdminLog)** — `web_admin_log` (ดู Detail + Export, ค้นช่วงวันที่)
- audit trail ของแอดมิน (โมดูลอื่นเรียกผ่าน `ActionLogs`); Detail: action, URL, ตาราง, IP, **old_value/new_value (JSON diff)**
**125. ผู้ใช้ที่ไม่เข้าใช้งาน (AdminUserNotLogin)** — `web_admin`
- แอดมินที่ **ไม่เคย login เลย** (`id NOT IN (SELECT admin_user_id FROM web_admin_log WHERE action IN ('login','login_2fa'))`); ค้นตามช่วงวันที่สร้าง

---

## 33) Widget (2 เมนู) — ส่วนเสริมของ Page Builder

**126. กลุ่ม Widget (WidgetGroup)** — `web_widget_group` (CRUD) — จัดกลุ่ม Widget (หัวข้อ + ICON)
**127. Widget ทั้งหมด (Widget)** — `web_widget` (CRUD)
- สร้างชิ้นส่วน Widget ที่นำไป **ลาก-วางใน page builder ของ CMSPage**
- Add/Edit: กลุ่ม, หัวข้อ, ICON, Mod Name, **HTML Code**; render/refresh ผ่าน `/Admin/WidgetAjax`

---

## สรุปประเด็นด้าน "การทำงาน" ที่พบ

| สถานะ | เมนู |
|---|---|
| ✅ ทำงานปกติ | เมนูส่วนใหญ่ (~120 เมนู) — CMS content, submission list, email sender, log viewer, admin |
| ⚠️ ใช้งานไม่ได้ (500/ตารางไม่มี) | **Banner, กลุ่ม Banner** (`web_banner*` ไม่มีใน DB), **นัดหมายชมทรัพย์ MeetNPA** (`web_meet_npa` ไม่มี ไม่มี controller) |
| ⚠️ ยังไม่สมบูรณ์ (placeholder) | **Microsite, Lead Form, MicrositeSubmit** (ฟอร์มมีแค่ title/sort), **Google Analytics (Monitor)** (ไม่มี dashboard จริง) |

หมายเหตุ: เมนูที่ถูกคอมเมนต์ออกจาก sidebar (ไม่แสดง) เช่น DebtText4 (ดาวน์โหลดแบบฟอร์ม), DownloadSubmit, NpaReport, BuyNPAReport, NplCustomer, LogsLDAP, LogsMatchAPI — โค้ด/config ยังอยู่แต่ไม่ปรากฏในเมนู
