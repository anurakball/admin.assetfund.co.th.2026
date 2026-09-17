# หลังบ้านเดิมของ Asset Plus (`tb_*`) — รายละเอียดการพอร์ต, ws_schedule, กฎที่ต้องรักษา

> ย้ายมาจาก `core_admin/CLAUDE.md` เมื่อ 17 ก.ย. 2569 (ลดขนาดไฟล์ที่โหลดทุก session) — เนื้อหาเดิมทั้งหมด ไม่ได้ตัด
> **เปิดอ่านเมื่อ**: จะพอร์ตเมนู `tb_*` เพิ่ม, แก้เมนู `Ap*`, แตะ `AdminLegacyController` / `AssetPlusImporter` / ws_schedule, หรือ front-end จะอ่านตาราง `tb_*`
> ตาราง 20 เมนูที่พอร์ตแล้ว + กฎย่อที่ต้องรักษาอยู่ใน CLAUDE.md หัวข้อ "เมนูที่พอร์ตมาจากหลังบ้านเดิม" · ไฟล์นี้มี: ทะเบียนเมนูเดิมทั้งหมด, สคีมาเดิม vs ใหม่, ไฟล์ที่เกี่ยวข้อง, พฤติกรรมที่คัดลอกมา, drill-down, ws_schedule, ข้อควรระวัง

### เมนูทั้งหมดของหลังบ้านเดิม vs ที่พอร์ตแล้ว (สำรวจ 17 ก.ย. 2569)

ทะเบียนเมนูของหลังบ้านเดิมอยู่ที่ `assetplus/backoffice/all_module.aspx` (`arr_group` = 13 กลุ่ม, `arr_item[n]` = `"ชื่อเมนู,tb_table$ชื่อเมนู,tb_table…"`)
เมนูซ้าย (`left.aspx`) สร้างจากทะเบียนนี้แล้วตัดด้วยสิทธิ์ใน `tb_admin_module` (1 แถว / `name` = ชื่อตาราง / `access_id`) — โครงเดียวกับ `2026_web_admin_module` ของระบบใหม่
ทุกเมนูมีโฟลเดอร์ `mod_<table>/` ที่ไฟล์ชุดเดียวกัน: `list.aspx` · `add.aspx` · `edit.aspx` · `move.aspx` · `mod_config.aspx` (+ `iframe_data.aspx`) — **จะพอร์ตเมนูเพิ่มให้เริ่มอ่าน `mod_config.aspx` แล้วตามด้วย `add.aspx` / `edit.aspx`**

| กลุ่ม (หลังบ้านเดิม) | เมนู → ตาราง | สถานะในระบบใหม่ |
|---|---|---|
| หน้าหลัก | Image Slide → `tb_home_bg` · Footer Banner → `tb_home_footer_banner` · Pop up → `tb_home_popup` · Popup Banner → `tb_home_announce` | **ไม่พอร์ต (ตั้งใจ)** — ระบบใหม่ใช้เมนู CMS บนตาราง `2026_web_*` แทน (`HomeImageSlide`, `HomeFooter`, `HomePopUp`, `HomeIntroPage`) |
| หน้าหลัก | Get Other Indices → `tb_home_other_indices` | พอร์ตแล้ว (`ApOtherIndices`) — หน้า live ของเว็บเดิมเองก็ไม่ได้อ่านตารางนี้ |
| ข้อมูลกองทุน | ประเภทกองทุนรวม `tb_fund_cat` (+ drill-down `tb_fund`, `tb_fund_doc`) · Get Fund Fact Sheet · Get NAV · Delete NAV · Get Performance | พอร์ตครบ (ตารางด้านบน) |
| กองทุนส่วนบุคคล | 6 เมนู `tb_fund_private*` | พอร์ตครบ |
| ปฏิทินกองทุน | `tb_calendar_category` · `tb_calendar` | พอร์ตครบ |
| กองทุนสำรองเลี้ยงชีพ | 4 เมนู `tb_fund_prov*` | พอร์ตครบ |
| การทำรายการ | Page → `tb_page_openaccount` | **ยังไม่พอร์ต** — เว็บเดิมอ่านใน `include/footer.aspx` |
| เกี่ยวกับเรา | Page → `tb_page_aboutus` · คณะกรรมการและผู้บริหาร → `tb_board_director` | **ยังไม่พอร์ต** — เว็บเดิม: `careers.aspx`, `contact-us.aspx`, `include/footer.aspx` / `board.aspx`, `board_ceo.aspx` |
| มุมมองการลงทุน | Page → `tb_page_investment` | **ยังไม่พอร์ต** — เว็บเดิมอ่านใน `include/footer.aspx` |
| ประกาศ | ประเภทประกาศ → `tb_announcement` (+ ลูก `tb_announcement_file`) | **ยังไม่พอร์ต** — เว็บเดิม `funds-annoucement.aspx` |
| ข่าวสาร กิจกรรม | ข่าวสาร บลจ. → `tb_news_category` (+ ลูก `tb_news`) · กิจกรรม → `tb_news_activities_category` (+ ลูก `tb_news_activities`, `_gallery`) · คอลัมภ์ในนามบริษัท → `tb_news_column_file` | **ยังไม่พอร์ต** — เว็บเดิม `asset-plus-news.aspx`, `activities.aspx`, `activities-details.aspx`, `published-articles.aspx`, `default.aspx` |
| ร่วมงานกับเรา | รายละเอียด → `tb_job_description` · ตำแหน่งงานว่าง → `tb_job_position` | **ยังไม่พอร์ต** — เว็บเดิม `careers.aspx` |
| Manage PDPA | เนื้อหา → `tb_pdpa` · Cookie Accept → `tb_pdpa_cookie_policy` | **ยังไม่พอร์ต** — เว็บเดิม `include/header.aspx` |
| Administrator | Admin User `tb_admin` · Admin Group `tb_admin_access` · Log `tb_admin_log` · Approve List `tb_admin_approve` | ระบบใหม่ใช้ `2026_web_admin*` ของตัวเอง — แต่ยัง**เขียน** `tb_admin_approve` (Edit/Approve ของ `AdminLegacyController`) และ `tb_admin_log` (ws_schedule) ให้หลังบ้านเดิมเห็นตรงกัน |

โฟลเดอร์ `mod_*` ที่ไม่อยู่ในทะเบียน = เมนูลูก drill-down (`mod_tb_fund`, `mod_tb_fund_doc`, `mod_tb_news`, `mod_tb_news_activities*`, `mod_tb_news_journal*`, `mod_tb_announcement_file`)
หรือของเก่าที่เลิกใช้ (`mod_main_*`, `mod_core_*`, `mod_tb_nav_agent`) · `mod_file_manager` = file manager · `mod_change_pwd` = หน้าบังคับเปลี่ยนรหัส

**บทบาทผู้ใช้ของหลังบ้านเดิม** — `tb_admin_access` 16 แถว (`admin_type` 1 = Content / 2 = Approver) แยกเป็นคู่ `<ฝ่าย>_Content` (แก้ไข) / `<ฝ่าย>_Approver` (อนุมัติ)
ตามแผนก CustomerCare, FundAccount, Product, Registrar, Compliance, FundSupport + `Root` (id 1) · `tb_admin` มีบัญชีจริง ~40 คน (ของ production ที่ติดมากับ backup)
ระบบใหม่ตอนนี้มีแค่ Super Admin (`access_id = 1`) — **ถ้าต้องออกแบบสิทธิ์จริง ให้ยึดโครง Content/Approver รายแผนกนี้เป็นต้นแบบ**

**config ของระบบเดิมที่ระบบใหม่คัดลอกมา** (`assetplus/web.config`): `ASPWSService.ASPWS` = `AssetPlusWS:URL` ของเรา (ค่าเก่า `58.137.100.115` ถูก comment ทิ้ง เหลือ `167.179.243.42`) ·
`Con_*` = DB (ตอนนี้ชี้ `.`/`sa`/`asset_plus_uat` ใน git ของโปรเจกต์นั้นเลย — repo นั้นไม่มี remote) ·
`Text_Image` / `Text_File` / `Text_VDO` / `Text_FileManager` = ข้อความบอกนามสกุล+ขนาดไฟล์ที่อัปโหลดได้ (รูป .jpg/.gif/.png/.bmp 1 MB, PDF 20 MB) ใช้อ้างอิงตอนตั้งกฎอัปโหลดของเมนู `Ap*`

### ตารางเดิม vs ตารางระบบใหม่

ตารางเดิมอยู่ใน database เดียวกัน (`asset_plus_uat`) แต่ **ไม่มี prefix `2026_`**
`Db.T()` จึงข้ามการเติม prefix ให้ทุกชื่อที่ขึ้นต้นด้วย `tb_` (ดู `Db.IsLegacy()`)
ปลอดภัยเพราะชื่อตรรกะของระบบใหม่ทุกตัวขึ้นต้นด้วย `web_` / `api_` และไม่มีตาราง `2026_tb_*` ใน DB

สคีมาต่างกันตรงนี้ — จึงต้องใช้ `AdminLegacyController` แทน `AdminCoreController`:

| ระบบใหม่ (`2026_web_*`) | ระบบเดิม (`tb_*`) |
|---|---|
| `web_id` (แยก microsite) | ไม่มี |
| `created_at` / `updated_at` (datetimeoffset) | `lastcreate` / `lastupdate` (**unix seconds**) |
| `created_by` / `updated_by` | `last_user` |
| `approve_by` | `pb_last_user` |
| `id` เป็น IDENTITY ทุกตาราง | บางตารางไม่ใช่ (ต้องคำนวณ `MAX(id)+1` เอง → `LegacyIdManual`) |

ที่ **เหมือนกัน** และใช้ซ้ำได้ทั้งหมด: `sort` / `status` / `pb_status` / `show_front` / คู่คอลัมน์ `pb_*`
และตรรกะ Approve (`pb_<field> = <field>`, `pb_status = 1`, `show_front = 1`) ตรงกันทั้งสองระบบ

### ไฟล์ที่เกี่ยวข้อง

ทั้งหมดอยู่ใต้ `Areas/Admin/` — `Controllers/AdminLegacyController.cs` คือเครื่องยนต์กลาง
(Index/Create/Edit/Delete/Status/Approve/Move) ของตาราง `tb_*` · `AssetPlusLegacyControllers.cs` = เมนู CRUD 6 ตัว
· `AssetPlusImportControllers.cs` = เมนู "Get ..." 4 ตัว + Delete NAV
· `AssetPlusFundControllers.cs` = รายชื่อกองทุน + เอกสารกองทุน (drill-down)
· `AssetPlusPrivateControllers.cs` = กลุ่มกองทุนส่วนบุคคล + เกี่ยวกับกองทุนสำรองฯ
· `Controllers/WsScheduleController.cs` = endpoint ให้ scheduler เรียก (พอร์ตจาก ws_schedule/*.aspx)
· `Helpers/AssetPlusImporter.cs` = ตรรกะแปลง XML → ตารางเดิม (ใช้ร่วมกันทั้งเมนู "Get ..." และ ws_schedule)
· `Helpers/AssetPlusWsClient.cs` = ตัวเรียก SOAP
`ASPWS.asmx` · `Views/Ap*/` = ฟอร์มของแต่ละเมนู

**`ModuleConfig` ของทั้ง 20 เมนูอยู่ที่ `Areas/Admin/Helpers/AdminMenuAssetPlus.cs`** (`AssetPlusLegacyModules()`)
ซึ่ง `AdminMenu.AllModule()` ต่อท้ายด้วย `.Concat(...)` — เพิ่มเมนูใหม่ให้แก้ที่ไฟล์นั้น

### พฤติกรรมที่คัดลอกมาจากระบบเดิม

- **Approve** : `pb_<field> = <field>` ทุกฟิลด์ใน `FieldApprove`, `pb_status=1`, `show_front=1`, `pb_last_user=<user>`
  แล้วลบคิวใน `tb_admin_approve` (เมนู Approve List ของหลังบ้านเดิมจึงยังเห็นตรงกัน)
- **Edit** ตั้ง `pb_status = 0` เสมอ (กลับไปรออนุมัติใหม่) และเขียนคิว `tb_admin_approve`
- **Move** : `sort ± 15` แล้วเรียงใหม่เป็น 10, 20, 30…
- **`tb_calendar_category`** : `datatype` ห้ามซ้ำ และถ้าแก้ `datatype` ต้อง cascade ไป `tb_calendar.datatype`
- **อัปโหลดไฟล์** (`img1` / `en_img1` ของ Factsheet / ข้อมูลอื่นๆ) เก็บเป็น **ชื่อไฟล์เปล่า**
  รูปแบบเดิม `<table>_<rand 0-999>_<unix>_<field>.<ext>` และเขียนไฟล์ลงโฟลเดอร์ upload ของเว็บเดิม
  ตั้งค่าที่ `appsettings → LegacyUpload:Path` และ `LegacyUpload:Url`
- **อัปโหลดไฟล์ของ `ApFundDoc` ใช้คนละโฟลเดอร์และคนละกฎการตั้งชื่อ**
  เก็บที่ `upload_otherdocs` (ตั้งค่าที่ `LegacyUploadDoc:Path` / `LegacyUploadDoc:Url`)
  ชื่อไฟล์ถูกกำหนดตายตัวเป็น `<fundcode>_<slug>[_en].<ext>` — `slug` มาจาก `file_id` (1–18)
  หรือจากช่อง "ชื่อไฟล์" (`file_n`) ถ้าเป็นเอกสารที่ผู้ใช้เพิ่มเอง (`file_id = 0`)
  **ห้ามแก้กฎนี้** เพราะเว็บเดิมอ้างไฟล์ด้วยชื่อดังกล่าวโดยตรง
- **เมนู "Get ..."** : เลือกวันที่ → เรียก web service → แปลง XML → เขียนลงตารางเดิม
  แถวเดิมของคีย์เดียวกันถูกตั้ง `Flag = 0` แล้ว insert แถวใหม่ `Flag = 1`, `status/pb_status/show_front = 1` (เผยแพร่ทันที)
  - endpoint : `appsettings → AssetPlusWS:URL` (ค่าเดิมจาก `assetplus/web.config` = `http://167.179.243.42:53556/ws/ASPWS.asmx`)
  - operation : `MartketOtherIndices(date)` / `NAVAnnounce()` / `FundReturnPerformance(date)` / `FundFactSheet(fundDate)`
  - **ระบบใหม่เพิ่ม "อัปโหลดไฟล์ XML"** ไว้ใช้เมื่อ web service เข้าไม่ถึง (ตอนพัฒนา endpoint นี้ ping ไม่ผ่าน)
    โครงสร้างไฟล์เดียวกับที่ระบบเดิมเซฟไว้ใน `mod_*/xml_file/`
  - Fund Fact Sheet map แบบ generic : element ใน XML ที่ชื่อ **ตรงกับคอลัมน์จริง** จะถูกเขียนลงคอลัมน์นั้น
    (ตาราง `tb_fund_fundfact` มี ~270 คอลัมน์ — วิธีนี้รองรับ element ใหม่โดยไม่ต้องแก้โค้ด)
  - **ตรรกะการนำเข้าจริงอยู่ที่ `Areas/Admin/Helpers/AssetPlusImporter.cs`** ไม่ได้อยู่ใน controller
    เพราะใช้ร่วมกับ `ws_schedule` (ดูหัวข้อถัดไป) — แก้ที่เดียว มีผลทั้ง "กดเอง" และ "ตัวจับเวลาเรียก"

### เมนูลูกแบบ drill-down (รายชื่อกองทุน / เอกสารกองทุน)

ทั้งสองเมนูนี้ **ไม่อยู่ในเมนูด้านซ้าย** (ตรงกับหลังบ้านเดิม) เข้าถึงผ่านปุ่มในหน้ารายการของเมนูแม่:

```
ประเภทกองทุนรวม (ApFundCat)
  └─[จัดการกองทุน]→ รายชื่อกองทุน (ApFund)
        └─[จัดการไฟล์]→ เอกสารกองทุน (ApFundDoc)
```

- `ApFund` ผูกกับหมวดด้วย `cat_id` (= `tb_fund_cat.id`) · `ApFundDoc` ผูกกับกองทุนด้วย **`fundcode`** (ไม่ใช่ id)
  ระบบเดิมเก็บ `fundcode` ไว้ทั้งใน `tb_fund_doc.cat_id` และ `tb_fund_doc.fundcode` — ระบบใหม่ทำตาม
- **เพิ่มกองทุนใหม่ → สร้างแถวเอกสารมาตรฐาน 18 รายการ (`file_id` 1–18) ให้อัตโนมัติ**
  แก้ไขกองทุนที่ยังไม่มีแถวเหล่านี้ก็สร้างย้อนหลังให้ (ตรงกับ mod_main_fund/add.aspx + edit.aspx)
- **เปลี่ยน `fundcode` → ตามไปแก้ทุกแถวใน `tb_fund_doc`** (`cat_id` / `fundcode` / `pb_*`)
  **ลบกองทุน → ลบเอกสารของ fundcode นั้นทิ้งด้วย**
- เอกสารมาตรฐาน 18 รายการ **ลบไม่ได้** (ซ่อน checkbox + ปุ่มลบ และกันซ้ำที่ฝั่ง server)
  ลบได้เฉพาะเอกสารที่ผู้ใช้เพิ่มเอง (`file_id = 0`)
- ชื่อเอกสารของแถวมาตรฐานมาจากตารางคงที่ `ApFundDocController.FixedDocNames` (ไม่ได้เก็บใน DB)

### ws_schedule — ตัวจับเวลาดึงข้อมูลอัตโนมัติ

พอร์ตมาจาก `backoffice/ws_schedule/*.aspx` ของระบบเดิม · โค้ดอยู่ที่ `Areas/Admin/Controllers/WsScheduleController.cs`
**ไม่ต้องล็อกอินหลังบ้าน** (scheduler เรียกเอง) — URL คงรูปเดิมไว้ทั้งชุด ย้ายมาโดยแก้แค่ชื่อโฮสต์:

| ระบบเดิม | ระบบใหม่ (รับทั้งมีและไม่มี `.aspx`, ทั้ง GET และ POST) |
|---|---|
| `…/ws_schedule/ws_get_nav.aspx` | `https://<host>/ws_schedule/ws_get_nav` |
| `…/ws_schedule/ws_get_other_indices.aspx` | `https://<host>/ws_schedule/ws_get_other_indices` |
| `…/ws_schedule/ws_get_performance.aspx` | `https://<host>/ws_schedule/ws_get_performance` |
| `…/ws_schedule/ws_get_fundfact.aspx` | `https://<host>/ws_schedule/ws_get_fundfact` |
| `…/ws_schedule/test.aspx` | `https://<host>/ws_schedule/test` (หน้าตรวจสถานะ + ผังการตั้งค่า) |

**พารามิเตอร์ที่ส่งให้ web service เหมือนระบบเดิมทุกตัว** (ห้ามเปลี่ยน — ฝั่ง ASPWS ตรวจชื่อพารามิเตอร์):

| operation | พารามิเตอร์ | ค่าที่ส่ง |
|---|---|---|
| `NAVAnnounce` | *(ไม่มี)* | — |
| `MartketOtherIndices` | `date` | วันนี้ `dd/MM/yyyy` |
| `FundReturnPerformance` | `date` | วันนี้ `dd/MM/yyyy` |
| `FundFactSheet` | `fundDate` | วันนี้ `dd/MM/yyyy` |

⚠ **วันที่ต้องเป็น ค.ศ.** — culture ของแอปเป็น th-TH (ปฏิทินพุทธ) ถ้าใช้ `ToString("dd/MM/yyyy")` เฉย ๆ
จะได้ปี 2569 แล้ว web service ไม่รู้จัก · ทั้ง `WsScheduleController` และ `AssetPlusImporter.DateIn()`
จึงบังคับ `InvariantCulture` ไว้ (คอลัมน์ `*DateIn` / `*DateFormat` ในตารางเดิมเก็บ ค.ศ. ทั้งหมด)

พารามิเตอร์เสริมของระบบใหม่ (ไม่กระทบการทำงานเดิม): `?date=dd/MM/yyyy` (ดึงย้อนหลัง) ·
`?format=json` (ให้ scheduler อ่านผลง่าย) · `?key=…` (กันคนนอกยิง ตั้งที่ `WsSchedule:Key` — ไม่ตั้ง = เปิดเหมือนเดิม)

ตั้งค่าเพิ่มที่ `appsettings`: `WsSchedule:XmlPath` (โฟลเดอร์เก็บ XML ที่ดึงมา — ค่าปกติ `App_Data/ws_schedule`
**ตั้งใจไม่ให้อยู่ใต้ `wwwroot`** เพราะระบบเดิมเก็บไว้ในที่ที่โหลดจากเว็บได้) และ `WsSchedule:Key`

ผลการทำงานทุกครั้งถูกบันทึกลง `tb_admin_log` (`action_table = 'ws_schedule'`, `user_id = 'ws_auto'`)
เมนู Log ของหลังบ้านเดิมจึงเห็นด้วย
⚠ `tb_admin_log.action_info` เป็นชนิด `text` (codepage 874) — **ห้ามใส่อักขระนอกโค้ดเพจไทย** (เช่น `·`) จะกลายเป็น `?`

**ตารางที่เขียน** (ตรงกับระบบเดิมทุกตัว):

| endpoint | ตารางหลัก | ตารางอื่น |
|---|---|---|
| ws_get_other_indices | `tb_home_other_indices` | — |
| ws_get_nav | `tb_fund_nav` | — |
| ws_get_performance | `tb_fund_performance` | `tb_fund_performance_hd` (แถว `id = 1` = หัวตาราง) |
| ws_get_fundfact | `tb_fund_fundfact` | ตารางลูก 11 ตัว `tb_fund_fundfact_*` |

กฎการเขียนที่ต้องรักษาไว้ (front-end เดิมอ่านตามนี้):
- แถวใหม่ `Flag = 1` · แถวเดิมของคีย์เดียวกันถูกตั้ง `Flag = 0` (ไม่ลบทิ้ง เก็บเป็นประวัติ)
- `status = 1, pb_status = 1, show_front = 1` → เผยแพร่ทันทีโดยไม่ต้องรออนุมัติ
- `sort` = `MAX(sort) + 10` ไล่ขึ้นทีละแถว · `last_user` / `pb_last_user` = `ws_auto`
- ตารางลูกของ fundfact ใช้ **ลบด้วย fundcode แล้ว insert ใหม่** (ไม่ใช่ Flag)

**`FundCodeMark` ของ `tb_fund_performance`** — แถวที่ `NAVPerUnit` + `InceptionDateTH` + `InceptionDateEN`
ว่างครบสามช่องคือแถว "เกณฑ์มาตรฐาน" ต้องยืมรหัสกองทุนที่อยู่เหนือมัน แล้วล้างตัวจำ
(ลอกมาทั้งท่อนรวมถึงกรณี benchmark ติดกัน 2 แถวที่แถวหลังจะได้ค่าว่าง — อย่า "แก้ให้ดีขึ้น")

### ข้อควรระวัง

- **วันที่ในฟอร์มเป็น พ.ศ.** (culture ของแอปคือ th-TH) — ตัวแปลงใน `LegacyFields()` ใช้ culture ปัจจุบัน
  จึงบันทึก `31/12/2569` เป็น `2026-12-31` ถูกต้อง ห้ามเปลี่ยนไปใช้ `InvariantCulture`
- **ห้ามลบกลุ่มที่ยังมีลูก** — `ApCalendarCat` ผูกลูกด้วย `datatype` (ไม่ใช่ `cat_id`) จึงตั้ง
  `LegacyParentField = "datatype"` ด้วย (หลังบ้านเดิมไม่ได้กันไว้ ระบบใหม่กันเพิ่มเพื่อไม่ให้เกิด orphan)
- **สิทธิ์เมนู** อยู่ใน `2026_web_admin_module` — เพิ่มเมนูใหม่ต้อง insert สิทธิ์ให้ `access_id` ที่ต้องการ
  ไม่งั้นเมนูจะไม่ขึ้นและเปิดหน้าไม่ได้
- **`pb_status` ของตารางเดิมไม่ได้เป็น 0/1 เสมอ** — `tb_fund_doc` มี ~1,500 แถวที่ค่าเป็น 10/20/30…
  (หลังบ้านเดิม insert ค่า `sort` ลงคอลัมน์นี้ผิด) หลังบ้านเดิมถือว่า "ทุกค่าที่ไม่ใช่ 1 = ยังไม่อนุมัติ"
  หน้า list ของระบบใหม่จึงขึ้นปุ่ม Approve ให้ทุกแถวที่ `pb_status <> 1` เมื่อ `LegacyTable = true`
- **คอลัมน์ `sort` ของตารางเดิมเป็น NULL ได้** (`tb_fund_doc` 28 แถว) — โค้ดที่คำนวณเลขลำดับในหน้า list
  ต้องกัน string ว่างเสมอ
- **ค้นหาด้วยช่วงวันที่ (`EnableDateSearch`) รับค่าเป็น ค.ศ.** — ค่าใน session เป็น `yyyy-MM-dd`
  ต้องอ่านด้วย `InvariantCulture` (culture ของแอปเป็น th-TH ปฏิทินพุทธ ถ้าใช้ culture ปัจจุบันจะเพี้ยน 543 ปี)
  ⚠ ต่างจาก `LegacyFields()` ที่รับวันที่จาก**ฟอร์ม**เป็น พ.ศ. และต้องใช้ culture ปัจจุบัน
