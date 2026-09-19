# Front-end (7169) ทุกหน้า → เชื่อมกับเมนูหลังบ้าน (Back-end) ตัวไหน

ตรวจจาก: โค้ด front-end `D:\Project\sam.or.th` (HomeController/MemberController/AuthController + layoutContentService + partials) + เดินหน้าจริง browser + เทียบกับ audit หลังบ้าน (`docx/backend-menu-audit.md`) — #N = เลขเมนูหลังบ้านในเอกสารนั้น

## กลไกสำคัญที่ต้องรู้ก่อน
- **หน้า CMS ทุกหน้า** resolve จาก `web_cms_page` (ตาม `seo_url`) แล้วอ่านโค้ด `pb_box_data` (เช่น `abo-vis`, `dep-ove`) → HomeController switch เลือก table/ view ให้
- **เนื้อหา CMS** อยู่ PostgreSQL `sam` (แชร์กับ admin) และ front-end อ่าน **คอลัมน์ `pb_*`** = ค่าที่ "อนุมัติแล้ว" (ตรงกับ approval workflow หลังบ้าน)
- **ข้อมูลทรัพย์สิน NPA ทุกใบ** อยู่คนละฐาน = **MySQL `sam_npa.tb_product2`** (ระบบ NPA เดิม) ไม่ใช่ CMS — PostgreSQL เป็นแค่ overlay (สถานะ/ย่าน/facility) + เก็บ submission

---

# A. แถบบนสุด (Header / ทุกหน้าใช้ร่วม)

| ส่วน front-end | ตาราง/แหล่งข้อมูล | เมนูหลังบ้าน |
|---|---|---|
| โลโก้ + Template/สีธีม | `web_home_header` (pb_this_type/pb_img1/pb_des) | **โลโก้ / Template (HomeHeader)** #4 |
| เมนูหลัก (mega-menu) + แผนผังเว็บไซต์ | `web_cms_page` (pb_*, cat_id เรียกซ้ำ) + cache ไฟล์ `cache_menu/` | **จัดการเมนูเว็บไซต์ (CMSPage)** #1 |
| ปุ่ม "ระบบสมาชิก" (Login/Register) | Session + `web_member` / `web_agent` | **จัดการสมาชิก (MemberList)** #82 / **AgentList** #86 |
| ปุ่มค้นหา (/th/search) | → หน้าค้นหา (MySQL tb_product2) | (ดูหน้าค้นหา) |
| สลับภาษา TH/EN | `web_cms_page` (seo_url TH/EN) | **CMSPage** #1 |
| หน้า Intro (/intropage, /th/intro-page) | `web_home_intro_page` | **หน้า Intro Page (HomeIntroPage)** #2 |
| Pop-up หน้าแรก | `web_home_pop_up` | **หน้า Pop-Up (HomePopUp)** #3 |
| SEO/Meta + tracking script (GA/FB Pixel) ทุกหน้า | `web_home_seo` | **SEO & Code (HomeSEO)** #8 |

---

# B. เมนูหลัก (Main nav) — ไล่ตามลำดับเมนู + submenu

## 1) เกี่ยวกับเรา

### รู้จักเรา
| หน้า front-end | ตาราง | เมนูหลังบ้าน |
|---|---|---|
| /th/วิสัยทัศน์พันธกิจ | `web_core_single` (m.8) | **วิสัยทัศน์/พันธกิจ (AboutVision)** #18 |
| /th/ประวัติความเป็นมา | `web_core_single` (m.9) | **ประวัติความเป็นมา (AboutHistory)** #19 |
| /th/โครงสร้างองค์กร | `web_core_item` (m.4) | **โครงสร้างองค์กร (AboutStructure)** #20 |
| /th/รายงานประจำปี | `web_core_item` (m.2) | **รายงานประจำปี (AboutAnnual)** #21 |
| /th/รายงานการเงิน | `web_core_item` (m.3) | **รายงานการเงิน (AboutFin)** #22 |

### การจัดการองค์กร
| หน้า front-end | ตาราง | เมนูหลังบ้าน |
|---|---|---|
| /th/คณะกรรมการบริษัท | `web_core_item` (m.5) | **คณะกรรมการบริษัท (AboutBoard1)** #23 |
| /th/คณะกรรมการอื่นๆ | `web_core_item` (m.6) + `web_core_group` (แท็บกลุ่ม) | **คณะกรรมการอื่นๆ (AboutBoard2)** #25 + **กลุ่ม (AboutBoard2cat)** #24 |
| /th/คณะผู้บริหารระดับสูง | `web_core_item` (m.7) + `web_core_group` | **คณะผู้บริหารระดับสูง (AboutBoard3)** #27 + **กลุ่ม (AboutBoard3cat)** #26 |
| /th/คณะผู้บริหารฝ่าย | `web_core_item` (m.8) + `web_core_group` (สายงาน/กลุ่มย่อย) | **คณะผู้บริหารฝ่าย (AboutBoard4)** #30 + **สายงาน (AboutBoard4cat)** #28 + **กลุ่ม (AboutBoard4sub)** #29 |

### นโยบายการดำเนินงาน
| หน้า front-end | ตาราง | เมนูหลังบ้าน |
|---|---|---|
| /th/การบริหารสินทรัพย์ด้อยคุณภาพ-บสส. | `web_core_single` (m.10) | **บริหารสินทรัพย์ด้อยคุณภาพ (AboutText1)** #31 |
| /th/การบริหารทรัพย์สินรอการขาย | `web_core_single` (m.11) | **บริหารทรัพย์สินรอการขาย (AboutText2)** #32 |
| /th/การสร้างโอกาสทางธุรกิจ-บสส. | `web_core_single` (m.12) | **สร้างโอกาสทางธุรกิจ บสส. (AboutText3)** #33 |

## 2) ทรัพย์สินรอการขาย
| หน้า front-end | ตาราง/แหล่งข้อมูล | เมนูหลังบ้าน |
|---|---|---|
| /th/ค้นหาทรัพย์สิน (+ /th/search + npa-search-result) | **MySQL `tb_product2`** (JOIN provinces/district) + การ์ด highlight จาก `web_npa_status.pb_highlight_status`; filter zones→`pb_group_id`, nearby→`pb_facility_id` | ทรัพย์เอง = ระบบ NPA (MySQL, ไม่มีเมนู CMS) + **กำหนดสถานะทรัพย์สิน (NpaStatus)** #91 + **ย่าน/ทำเล (NpaLocate)** #93 + **Facility (NpaFacility)** #94 |
| /th/ประเภททรัพย์ | นับจาก **MySQL `tb_product2`** GROUP BY product_type + ชื่อจาก `product_group` | ระบบ NPA (MySQL) + หน้า wrapper `web_cms_page` (**CMSPage** #1) |
| /th/โปรโมชั่นทรัพย์เด่น | `web_core_news` (m.13) | **โปรโมชั่น (Promotion)** #70 |
| /th/ขั้นตอนการซื้อทรัพย์ | `web_core_single` (m.13) + FAQ `web_core_news` | **ขั้นตอนการซื้อทรัพย์ (NpaText1)** #34 |
| /th/ทรัพย์มือสองต้องบอกต่อ | `web_cms_page` (box builder) | **จัดการเมนูเว็บไซต์ (CMSPage)** #1 |

## 3) บริหารหนี้ด้อยคุณภาพ
| หน้า front-end | ตาราง | เมนูหลังบ้าน |
|---|---|---|
| /th/ภาพรวมการบริหารหนี้ | `web_core_single` (m.14) | **ภาพรวมการบริหารหนี้ (DebtText1)** #35 |
| /th/ขั้นตอนการปรับโครงสร้างหนี้ | `web_core_single` (m.15) | **ขั้นตอนการปรับโครงสร้างหนี้ (DebtText2)** #36 |
| /th/สิทธิประโยชน์ลูกหนี้ | `web_core_single` (m.16) + FAQ `web_core_news` | **สิทธิประโยชน์ลูกหนี้ (DebtText3)** #37 |
| /th/ดาวน์โหลดแบบฟอร์ม-เอกสารที่เกี่ยวข้อง | `web_core_news` (m.10) | **ดาวน์โหลด (Download)** #62 |
| /th/ลงทะเบียนปรับโครงสร้างหนี้ | เนื้อหา `web_core_single` (m.18); **ฟอร์ม submit → INSERT `web_npl_register`** | **ลงทะเบียนปรับโครงสร้างหนี้ (DebtText5)** #38 + ฟอร์มลง **รายชื่อผู้ลงทะเบียน NPL (NplRegister)** #96 |
| /th/คลินิกแก้หนี้ | `web_core_single` (m.19) + FAQ | **คลินิกแก้หนี้ (DebtText6)** #39 / **CliWeb** #98 |

## 4) ข่าวสารและประกาศ

### ข่าวสาร
| หน้า front-end | ตาราง | เมนูหลังบ้าน |
|---|---|---|
| /th/ข่าวประชาสัมพันธ์ (+ หน้า detail ข่าว) | `web_core_news` (m.1) + `web_core_group` (หมวด) | **ข่าวประชาสัมพันธ์ (News)** #41 + **กลุ่ม (NewsGroup)** #40 |
| /th/บทความและวารสาร (+ detail) | `web_core_news` (m.2) + `web_core_group` (m.6) | **บทความและวารสาร (Article)** #43 + **กลุ่ม (ArticleGroup)** #42 |
| /th/วิดีโอ-สื่อประชาสัมพันธ์ (+ playlist) | `web_core_news` (m.3) + `web_core_group` (m.7 หมวด, m.8 ชุด) | **วิดีโอ/สื่อ (VDO)** #46 + **รายการ (VDOGroup #44 / VDOGroupsub #45)** |

### ประกาศ
| หน้า front-end | ตาราง | เมนูหลังบ้าน |
|---|---|---|
| /th/ประกาศอัตราดอกเบี้ย | `web_core_news` (m.4) | **ประกาศอัตราดอกเบี้ย (AnnounceRate)** #47 |
| /th/ประกาศจัดซื้อจัดจ้าง (+ detail + ฟอร์มลงทะเบียน) | `web_core_news` (m.7) + ประเภท `web_core_group` (m.10); **ฟอร์ม → INSERT `web_announce_submit`** | **ประกาศจัดซื้อจัดจ้าง (AnnouncePro)** #51 + **ประเภท (AnnounceProGroup)** #52 + ฟอร์มลง **รายชื่อผู้ลงทะเบียน (AnnounceSubmit)** #54 |
| /th/ประกาศ-nplnpa | `web_core_news` (m.5) | **ประกาศจาก NPL/NPA (AnnounceNPA)** #48 |
| /th/ประกาศทั่วไป | `web_core_news` (m.6) + `web_core_group` | **ประกาศทั่วไป (AnnounceOther)** #50 + **กลุ่ม (AnnounceOtherGroup)** #49 |

## 5) ร่วมงานกับเรา
| หน้า front-end | ตาราง | เมนูหลังบ้าน |
|---|---|---|
| /th/ตำแหน่งงานว่าง (+ หน้าสมัครงาน) | `web_core_news` (m.8) + เนื้อหา `web_core_single` (m.20); **ฟอร์มสมัคร → INSERT `web_job_submit_full`** (form_json เต็ม) | **ประกาศรับสมัครงาน (AnnounceJob)** #55 + ใบสมัครลง **รายชื่อผู้สมัครงาน (AnnounceJobSubmitFull)** #56 |
| /th/วัฒนธรรมองค์กร | `web_core_single` (m.20) | **วัฒนธรรมองค์กร (JobCMS)** #57 |

## 6) ติดต่อเรา
| หน้า front-end | ตาราง | เมนูหลังบ้าน |
|---|---|---|
| /th/สำนักงานใหญ่-สาขา | `web_core_news` (m.9) | **สำนักงานใหญ่/สาขา (ContactOffice)** #58 |
| /th/ช่องทางร้องเรียน-ข้อเสนอแนะ | เนื้อหา `web_core_single` (m.21); **ฟอร์ม → INSERT `web_contact_submit`** | **ช่องทางร้องเรียน (ContactCMS)** #59 + ฟอร์มลง **รายชื่อผู้ติดต่อ (ContactSubmit)** #60 |
| /th/ดาวน์โหลดแบบฟอร์ม | `web_core_news` (m.10) + `web_core_group` | **ดาวน์โหลด (Download)** #62 + **กลุ่ม (DownloadGroup)** #61 |
| /th/คำถามที่พบบ่อย | `web_core_news` (m.11) + `web_core_group` (m.14) | **คำถามที่พบบ่อย (FAQ)** #64 + **กลุ่ม (FAQGroup)** #63 |
| /th/หน่วยงานที่เกี่ยวข้อง | `web_core_news` (m.12) + `web_core_group` (m.15) | **ลิงค์หน่วยงาน (Link)** #66 + **กลุ่ม (LinkGroup)** #65 |
| /th/แลกเปลี่ยนลิงค์ | เนื้อหา `web_core_single` (m.22); **ฟอร์ม → INSERT `web_link_submit`** | **แลกเปลี่ยนลิงค์ (LinkCMS)** #67 + ฟอร์มลง **รายชื่อผู้แลกเปลี่ยน (LinkSubmit)** #68 |

---

# C. หน้าแรก (Homepage /th) — ไล่แต่ละบล็อกจากบนลงล่าง
| บล็อก front-end | ตาราง/แหล่งข้อมูล | เมนูหลังบ้าน |
|---|---|---|
| สไลด์แบนเนอร์หน้าแรก | `web_core_item` (m.1) | **รูปสไลด์หน้าแรก (HomeImageSlide)** #9 |
| ตั้งค่าสไลด์ (effect/speed) | `web_home_image_conf` | **ตั้งค่ารูปสไลด์ (HomeImageConf)** #10 |
| ช่องค้นหา — ค้นหาปกติ | → npa-search-result (**MySQL tb_product2**) | ระบบ NPA (MySQL) + NpaStatus #91 |
| ช่องค้นหา — ค้นหาจาก AI | AI แปลงเป็น SQL → **MySQL tb_product2**; provider+key จาก `web_core_single` (m.34) | **AI Settings (AISetting)** #111 (+ log **LogsAI** #118) |
| ช่องค้นหา — ตามแผนที่ | **MySQL tb_product2** (lat/long) + Google Map key จาก `web_core_single` (m.33) | **Google Map Key (GoogleKey)** #110 |
| ปุ่ม "จับคู่ทรัพย์สิน NPA" | → npa-matching (**MySQL legacy** tb_want) | ⚠️ ไม่มีเมนูหลังบ้าน (ตาราง legacy) |
| ปุ่ม "ทรัพย์ขายทอดตลาดกรมบังคับคดี" | **MySQL tb_product2** WHERE product_source=2 | ระบบ NPA (MySQL) |
| "SAM ใส่ใจ" (หัวข้อ+4 ลิงก์) | `web_core_single` (m.1); 4 ลิงก์ (debt-relief/auction-items/sam-installment/second-hand-property) = `web_cms_page` | **SAM ใส่ใจ (HomeSamText)** #11 + **CMSPage** #1 |
| "ภาพรวมการบริหารหนี้" (ตัวเลขสถิติ) | `web_core_single` (m.2) | **ภาพรวมการบริหารหนี้ (HomeSamText2)** #12 |
| "ปิดหนี้ไว ไปต่อได้" | `web_core_single` (m.3) | **ปิดหนี้ไว ไปต่อได้ (HomeSamText3)** #13 |
| "ทรัพย์เด่นและน่าสนใจ" (แท็บจังหวัด) | config `web_core_single` (m.4) + ทรัพย์จาก **MySQL tb_product2** (is_recommend) | **ทรัพย์เด่นและน่าสนใจ (HomeSamText4)** #14 |
| "คุณกำลังมองหาอะไร" (นับตามประเภท) | config `web_core_single` (m.5) + count **MySQL tb_product2** | **คุณกำลังมองหาอะไร (HomeSamText5)** #15 |
| "บ้านเด่นทำเลดี" (ย่าน/ใกล้รถไฟฟ้า) | config `web_core_single` (m.6) + `web_core_group` (m.18 ย่าน, m.19 nearby) | **บ้านเด่นทำเลดี (HomeSamText6)** #16 + **NpaLocate** #93 + **NpaFacility** #94 |
| "ข่าวสาร" 3 แท็บ | `web_core_news` (m.1 ข่าว / m.2 บทความ / m.3 วิดีโอ) | **News** #41 / **Article** #43 / **VDO** #46 |
| "วิสัยทัศน์" (ท้ายหน้า) | `web_core_single` (m.7) | **วิสัยทัศน์ (HomeSamText7)** #17 |

---

# D. Footer (ทุกหน้าใช้ร่วม)
| ส่วน front-end | ตาราง | เมนูหลังบ้าน |
|---|---|---|
| สายด่วน 1443 / social / App links / copyright / banner | `web_home_footer` | **ปรับแต่ง Footer (HomeFooter)** #7 |
| ปุ่ม "สมัครรับข่าวสาร" | **POST → INSERT `web_subscription`** | **รายชื่อผู้ลงทะเบียน (Subscription)** #103 |
| เมนู footer คอลัมน์ 1 | `web_cms_page_footer1` | **จัดการเมนู Footer 1 (CMSPageFooter1)** #5 |
| เมนู footer คอลัมน์ 2 | `web_cms_page_footer2` | **จัดการเมนู Footer 2 (CMSPageFooter2)** #6 |
| /th/แผนผังเว็บไซต์ (sitemap) | `web_cms_page` (tree) | **CMSPage** #1 |
| /th/แบบสำรวจ (+ โหวต survey/vote) | `web_core_single` (m.23); vote เพิ่มคะแนน t11-t20 | **แบบสำรวจ (FooterPoll)** #76 |
| /th/สำหรับเจ้าหน้าที่ | หน้า static (ไม่มี endpoint/DB ฝั่ง front) | — (LDAP อยู่ฝั่ง admin: **LDAPSetting** #112) |
| /th/คำนวณสินเชื่อ | หน้า static (คำนวณ JS) | — (ไม่มีตาราง) |
| /th/ประกาศความเป็นส่วนตัว | เนื้อหา `web_core_single` (FooterPrivacy1); **"รับทราบ" → INSERT `web_privacy1_submit`** | **ประกาศความเป็นส่วนตัว (FooterPrivacy1)** #77 + **รายชื่อผู้ยินยอม (FooterPrivacy1Submit)** #78 |
| /th/ข้อตกลงและเงื่อนไข | เนื้อหา `web_core_single` (FooterPrivacy2); **"รับทราบ" → INSERT `web_privacy2_submit`** | **ข้อตกลงและเงื่อนไข (FooterPrivacy2)** #79 + **รายชื่อผู้ยินยอม (FooterPrivacy2Submit)** #80 |
| /th/การเปิดเผยข้อมูลและความโปร่งใส (ITA) | `web_footer_link` / `web_cms_page_footer*` | **เปิดเผยข้อมูลความโปร่งใส (FooterLink)** #81 |
| "การตั้งค่าคุกกี้" (cookie consent) | **เก็บใน cookie `sam_cookie_consent` เท่านั้น — ไม่ลง DB** | — |

---

# E. หน้าทรัพย์สิน (detail / ฟอร์ม) — นอกเมนู แต่เป็นหน้าจริง
| หน้า front-end | ตาราง/แหล่งข้อมูล | เมนูหลังบ้าน |
|---|---|---|
| /th/npa-detail/{code}, /th/asset-detail/{code} | **MySQL tb_product2** (+ tb_npa_image/tb_video/tb_promotion/tb_attach) | ระบบ NPA (MySQL); สถานะจาก **NpaStatus** #91 |
| /th/npa-search-result, /th/led-search-result | **MySQL tb_product2** (led = source=2) | ระบบ NPA (MySQL) |
| /th/search-by-map (+ /api/npa/nearby) | **MySQL tb_product2** + Google Map key | **GoogleKey** #110 |
| /th/npa/e-brochure, /th/npa/compare-results | LocalStorage ฝั่ง JS | — (ไม่มี DB) |
| /th/design-by-your-self (สร้างโบรชัวร์เอง) | **MySQL tb_product2** / tb_npa_image | ระบบ NPA (MySQL) |
| /th/asset-purchasing (+submit) | **POST → INSERT `web_npa_buy`** | **รายการผู้สนใจซื้อทรัพย์ (BuyNPA)** #99 ✅ |
| /th/asset-offer (+submit) | **POST → INSERT `web_npa_offer`** (+ไฟล์แนบ+อีเมล) | **ยื่นข้อเสนอซื้อ (NpaOffer)** #95 ✅ |
| /th/npa-appoint/submit (นัดชมทรัพย์) | **POST → INSERT MySQL `tb_appointment`** | ⚠️ **MeetNPA #101** ชี้ `web_meet_npa` (ตารางไม่มี/เมนูใช้ไม่ได้) — front-end เขียนคนละที่ (MySQL) |
| /th/npa-fav/add, /remove (บันทึกทรัพย์สนใจ) | **POST → INSERT/DELETE `web_npa_fav`** | **รายชื่อทรัพย์ Wishlist (NpaFav)** #92 ✅ |
| /th/apply-agent (+searchnpa/getdata/upload) | ค้น **MySQL tb_product2**; ฟอร์ม → **INSERT `web_agent`** (+web_agency_upload) | **รายชื่อผู้สมัครตัวแทน (AgentList)** #86 ✅ |

---

# F. ระบบสมาชิก + Auth — หน้าจริง (ส่วนใหญ่ไม่อยู่ในเมนู)
| หน้า/endpoint front-end | ตาราง | เมนูหลังบ้าน |
|---|---|---|
| /th/login (+ login/submit) | `web_member` (BCrypt, gate confirm_email) | **จัดการสมาชิก (MemberList)** #82 |
| /{lang}/social-login/{provider} (google/facebook/apple/line) | find-or-create `web_member` | **MemberList** #82 |
| /th/register (+ register/submit, register-confirm) | INSERT `web_member`; อีเมลยืนยัน CC เจ้าหน้าที่จาก `web_core_single` (m.26) | **MemberList** #82 + **อีเมลเจ้าหน้าที่แจ้งเตือน (MemberEmail)** #84 |
| /th/forgot-password, /th/reset-password | `web_member` (token refemail) | **MemberList** #82 |
| /th/member-dashboard, /th/member-profile | `web_member` | **MemberList** #82 |
| /th/member-agent-info / -documents / -properties | `web_agent` (+ upload) | **รายชื่อผู้สมัครตัวแทน (AgentList)** #86 |
| /th/wishlist (ทรัพย์ที่บันทึก) | `web_npa_fav` | **NpaFav** #92 |
| /th/history-npa (ประวัติการเข้าชม) | ประวัติการดูทรัพย์ของสมาชิก | **NpaFav/Member** (overlay) |
| /th/staff (สำหรับเจ้าหน้าที่) | static (LDAP อยู่ฝั่ง admin เท่านั้น) | **LDAPSetting** #112 (admin) |
| /th/calculator | static (JS) | — |
| E-Form (tag `{{{E-Form:id}}}` ในหน้า CMS) + /eform/submit | นิยาม `web_eform`; **submit → INSERT `web_eform_submit`** | **E-Form (EForm)** #74 + **รายชื่อผู้กรอกแบบฟอร์ม (EFormSubmit)** #75 |

---

# G. LINE LIFF API (AuthController, `/api/*`, token-based) — สำหรับแอป LINE
| endpoint | ตาราง | เมนูหลังบ้าน |
|---|---|---|
| register / edit-member / get-member / verify-* | `web_member` | **MemberList** #82 |
| apply-agency / get-agency / edit-agency / upload-file-agency | `web_agent` (+ web_agency_upload) | **AgentList** #86 |
| search-code-name | **MySQL tb_product2** | ระบบ NPA |
| update-consent (PDPA) | `web_pdpa_consent` | **ให้ความยินยอม PDPA (LogsPDPA)** #117 |
| (ทุก endpoint log) | `api_audit_log` | **LINE LIFF Data API (LogsLINE)** #120 |

---

# สรุปประเด็นสำคัญ / จุดที่ไม่ตรงกัน
1. **ทรัพย์สินทุกใบ = MySQL `sam_npa.tb_product2`** (ระบบ NPA เดิม คนละฐานกับ CMS PostgreSQL) — CMS/admin ทำแค่ overlay: สถานะ highlight/แนะนำ (`web_npa_status` = NpaStatus), ย่าน/facility (`web_core_group` = NpaLocate/NpaFacility), และเก็บ submission
2. **led-search-result** = tb_product2 กรอง `product_source=2` (ไม่ใช่ external API กรมบังคับคดี — ถูก import เข้า MySQL แล้ว)
3. **นัดชมทรัพย์ (npa-appoint)** เขียนลง **MySQL `tb_appointment`** ไม่ใช่ `web_meet_npa` — สอดคล้องกับที่พบว่าเมนู **MeetNPA #101 ใช้ไม่ได้** (ตารางไม่มีใน DB)
4. **จับคู่ทรัพย์ (npa-matching)** เขียน MySQL legacy (`tb_want`/`tb_want_match_up`) — **ไม่มีเมนูหลังบ้านรองรับ**
5. **Cookie consent** เก็บใน cookie เท่านั้น ไม่ลง DB; ส่วน `web_pdpa_consent` เขียนจาก **LINE API** เท่านั้น
6. **Banner (#71-72) และ Microsite (#113-115)** หลังบ้าน — **ไม่พบการใช้งานบน front-end** (สอดคล้องกับที่เป็น stub/ใช้ไม่ได้)
7. ฟอร์ม front-end ที่ **ตรงกับเมนูหลังบ้าน PostgreSQL**: ลงทะเบียน NPL→NplRegister, จัดซื้อจัดจ้าง→AnnounceSubmit, สมัครงาน→AnnounceJobSubmitFull, ร้องเรียน→ContactSubmit, แลกลิงก์→LinkSubmit, สมัครรับข่าว→Subscription, ซื้อทรัพย์→BuyNPA, ยื่นข้อเสนอ→NpaOffer, บันทึกทรัพย์→NpaFav, สมัครตัวแทน→AgentList, privacy→FooterPrivacy1/2Submit, E-Form→EFormSubmit
