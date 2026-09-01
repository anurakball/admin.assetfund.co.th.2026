# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

> 📁 **ทุก path ในเอกสารนี้สัมพัทธ์กับโฟลเดอร์ที่ไฟล์นี้อยู่ (`core_admin/`)** — ที่เดียวกับ `.csproj` และโค้ดทั้งหมด
> repo root คือโฟลเดอร์แม่ (`admin.assetfund.co.th.2026/`) มีแค่ `core_admin.sln` + ไฟล์ตั้งค่า git/IDE

## Commands

```bash
dotnet build                       # build
dotnet run                         # run (HTTP  http://localhost:5140)
dotnet run --launch-profile https  # run (HTTPS https://localhost:7140)
```

No test project exists in this solution.

## Architecture

ASP.NET Core 10 MVC application serving the admin panel of **ASSET PLUS - Fund Management** (`admin.assetfund.co.th`). The app communicates with a separate backend API (`assetfund.co.th/api`) via `WebService.cs` for most data mutations and reads certain data directly from the database via `DBHelper.cs`.

> โค้ดชุดนี้ port มาจากโปรเจกต์ admin ของ SAM (`admin.sam.or.th`) — งาน rebrand เป็น Asset Plus ทำไปแล้วในระดับโค้ด (ดูหัวข้อ **Branding** ด้านล่าง) แต่ **ข้อมูลใน DB ยังเป็นเนื้อหาเดิมของ SAM** (ข่าว/ประกาศ/รูปที่อัปโหลด/ชื่อหมวด) ซึ่งต้องแทนที่ด้วยเนื้อหาของ Asset Plus ผ่านหน้าหลังบ้านเอง

### Branding — ASSET PLUS - Fund Management

ชื่อที่ใช้อ้างถึงบริษัท (ยึดตาม https://www.assetfund.co.th):

| ใช้ตรงไหน | ข้อความ |
|---|---|
| ชื่อเต็มภาษาไทย | บริษัทหลักทรัพย์จัดการกองทุน แอสเซท พลัส จำกัด |
| ชื่อย่อภาษาไทย (header มือถือ) | บลจ. แอสเซท พลัส |
| ชื่ออังกฤษ / brand line | ASSET PLUS - Fund Management |
| ชื่อระบบหลังบ้าน | Asset Plus Admin Management |

**พาเลตสี** (แทนที่สีเขียว/ส้มของ SAM เดิมทั้งหมด):

| บทบาท | สี | ใช้กับ |
|---|---|---|
| Primary (navy) | `#00295A` | sidebar, header, หัวตาราง, ปุ่มหลัก, หัวข้อหน้า, ปุ่มยืนยัน SweetAlert |
| Primary light | `#004699` | เมนูที่ active, ปุ่ม edit, ช่วงวันที่ใน datepicker |
| Primary sub | `#00326E` | พื้นหลัง submenu ใน sidebar |
| Accent (cyan) | `#00B4E5` | เส้นใต้หัวข้อ, hover ในเมนู, `.btn--secondary`, marker ของ list |

เขียว/ส้มของ SAM เดิม และน้ำเงินที่ตกค้างจาก template ถูกแทนที่หมดแล้ว — อย่าเอากลับมา

**ไฟล์ที่ถือพาเลต** — แก้ที่นี่เวลาปรับสี:
- `wwwroot/scss/variables/_color.scss` → source ของ `wwwroot/css/main.css` / `main.min.css` (ธีมหลักหลังบ้าน)
  ⚠ `main.css` กับ `main.min.css` **ไม่ได้ compile ต่อกัน** (min ถูกแก้มือ) เวลาเปลี่ยนสีต้องแก้ทั้ง 3 ไฟล์
  ⚠ ในไฟล์นี้มีแค่ `$color-primary` (`#00295A`) กับ `$color-secondary` (`#00B4E5`)
  ส่วน `#004699` / `#00326E` **hardcode อยู่ใน `scss/base/_button.scss`, `scss/base/_datepicker.scss`, `scss/layouts/_default.scss`** ต้องไล่แก้ที่นั่นด้วย
- `wwwroot/css/main_cms.min.css` + `wwwroot/css2/main_cms.min.css` → design token `--color-primary-*` / `--color-secondary-*`
  (ใช้ในหน้า Login และ Dashboard/Intro)
- `wwwroot/css/pages/intro.min.css` → พื้นหลัง Dashboard (`.bg--primary`)
- `wwwroot/js/Admin/admin_site.js` → `confirmButtonColor` ของ SweetAlert
- `Areas/Admin/Views/Shared/_AdminLayout.cshtml` → ธีมเนื้อหาใน CKEditor (`.ck-content`)

**โลโก้ / ไอคอน**:

| ไฟล์ | ใช้ที่ | หมายเหตุ |
|---|---|---|
| `wwwroot/assets/images/icon/assetplus-logo-white.png` | sidebar, header มือถือ, Dashboard | โลโก้กลับสี บนแผ่นพื้น `#00295A` — ต้องวางบนพื้นสี primary เท่านั้น |
| `wwwroot/images/logo/logo-32.png` (+ `-64`, `-128`, `logo.png`) | ฟอร์ม Login, modal re-login | โลโก้สีจริง พื้นโปร่ง ใช้บนพื้นขาว |
| `wwwroot/favicon.ico` | ทุกหน้า | สัญลักษณ์ infinity บนพื้น navy (64/48/32/16) |

**Cache busting**: view ใน `Areas/Admin/` ไม่มี `_ViewImports.cshtml` จึงใช้ tag helper `asp-append-version` ไม่ได้
(`~/` ยังทำงานเพราะ Razor แปลงให้เอง) — CSS/รูปที่เปลี่ยนตอน rebrand จึงต่อท้ายด้วย `?v=ap2026` เอง
**ถ้าแก้สีหรือโลโก้อีกครั้ง ต้องขยับเลขนี้** ไม่งั้น browser ของผู้ใช้จะยังเห็นของเก่า

### Two Databases

| Role | Type | Database | Config key |
|---|---|---|---|
| Primary | SQL Server | `asset_plus_uat` (ตาราง prefix `2026_`) | `DBConnection` |
| Secondary | MySQL | `sam_npa` | `MySQLConnection` |

> **ชื่อตาราง**: ตารางของระบบนี้อยู่ใน `asset_plus_uat` ร่วมกับตารางของระบบเดิมอีก 127 ตัว (ชื่อชนกันหลายตัว)
> จึงเติม prefix `2026_` ทุกตาราง และเพราะชื่อขึ้นต้นด้วยตัวเลข T-SQL บังคับให้ครอบ `[ ]` เสมอ
> เวลาเขียน SQL ใหม่ให้ใช้ `Db.T("web_admin")` แทนการพิมพ์ชื่อตรง ๆ
>
> **ข้อจำกัดเฉพาะของ DB ตัวนี้** (นอกเหนือจากไวยากรณ์ T-SQL ปกติ): compatibility level **100**
> จึงใช้ `OPENJSON` / `STRING_SPLIT` ไม่ได้ · คอลเลชัน `Thai_CI_AS` (ไม่สนตัวพิมพ์อยู่แล้ว)

All queries are raw parameterized SQL — no ORM. `DBHelper.cs` wraps `Microsoft.Data.SqlClient` and `MySqlConnector`, and logs queries to console in Development (except module/access-check queries).

### System audit reference (อ่านก่อนงานที่ต้องเข้าใจภาพรวมทั้งระบบ)

มีเอกสาร audit ฉบับเต็ม 2 ไฟล์ (สร้างไว้แล้ว — ใช้แทนการ re-audit ทุกเมนู):
- `docs/backend-menu-audit.md` — ทุกเมนูหลังบ้าน ~127 เมนู / 33 กลุ่ม (table, สิทธิ์, ฟิลด์, หน้าที่) เรียง #1–#127
- `docs/frontend-to-backend-map.md` — map หน้าเว็บ → เมนูหลังบ้าน (#N อ้างไฟล์บน)
  ⚠ ทำไว้ตอนที่ front-end คือเว็บของ SAM (`d:\Project\sam.or.th`) ใช้ดูว่า**เมนูหลังบ้านแต่ละตัวมีไว้ป้อนอะไร**ได้ แต่ไม่ใช่ผังของ front-end ตัวใหม่
- `docs/backend-menu-status.html` — **รายงานสถานะเมนูด้านซ้าย** (เปิดใช้งาน vs ถูก comment ปิดไว้) เปิดด้วย browser ได้เลย · ต้องอัปเดตทุกครั้งที่แก้เมนูด้านซ้าย

ข้อเท็จจริงสถาปัตยกรรมหลัก (durable — ใช้ตั้งต้นได้เลย):
- **ทรัพย์สิน NPA ทุกใบอยู่ MySQL `sam_npa.tb_product2`** (ค้นหา/detail/แผนที่/นับ/LED source=2/AI). SQL Server `asset_plus_uat` เป็น overlay เท่านั้น: `web_npa_status` (สถานะ/highlight/ผูกย่าน+facility), `web_core_group` (ย่าน=NpaLocate, facility=NpaFacility), + เก็บ submission
- **Approval workflow** ใช้คอลัมน์คู่ `pb_*` (pending) — front-end อ่าน `pb_*` (ค่าที่อนุมัติแล้ว); กด Approve จึง copy `pb_*`→ฟิลด์จริง
- เมนูหลังบ้านเกือบทั้งหมดเป็น thin wrapper ของ `AdminCoreController` + config กลางใน `Areas/Admin/Helpers/AdminMenu.cs` (`AllModule()`); "เครื่องยนต์เนื้อหา" ใช้ซ้ำ 4 แบบ: `web_core_single` (1 เมนู=1 ระเบียน แก้ไข t1..t50 เป็น section 2 ภาษา), `web_core_item` (การ์ดซ้ำ), `web_core_group` (หมวด), `web_core_news` (บทความเต็ม)
- **เมนูที่ใช้ไม่ได้/ยังไม่ทำ** (ยืนยันด้วยการกวาดทุกเมนูเมื่อ 2026-08-29 — เป็นปัญหาเดิม ไม่เกี่ยวกับการย้ายมา SQL Server เพราะตารางเหล่านี้ไม่เคยมีใน PostgreSQL เดิมเช่นกัน):
  - **ตารางไม่มีใน DB** → หน้า list ยังเปิดได้แต่ query ภายในพัง: `web_banner`, `web_banner_group` (Banner, กลุ่ม Banner), `web_file_manager` (FileManager — ตัว elFinder เองใช้ได้ปกติ), `web_google_analytics` (stub), `web_microsite_submit`, `web_meet_npa` (MeetNPA — front-end เขียนลง MySQL `tb_appointment` แทน)
  - **ไม่มี controller**: `MeetNPA`, `EFormEmail` → 404
  - **ไม่มีไฟล์ view**: `MicrositeForm/Create.cshtml`, `SubscriptionEmail/Create.cshtml` (กระทบหน้า Create ของ MemberNews / AgentNews / MicrositeForm)
  - **โค้ดตายที่อ้างตารางไม่มีจริง**: `web_branch_translation` (สาขา BranchMain), `web_product_main` (`InputSelectDB_Product` ไม่มีใครเรียก)
- npa-matching (จับคู่ทรัพย์) เขียน MySQL legacy (`tb_want*`) — ไม่มีเมนูหลังบ้านรองรับ; cookie consent เก็บใน cookie ไม่ลง DB; `web_pdpa_consent` เขียนจาก LINE API เท่านั้น

### Admin Area Pattern

Everything under `Areas/Admin/` is the admin panel. All admin controllers:

1. Inherit `AdminCoreController`
2. Receive `[AdminLogin]`, `[ModuleCheck]`, and `[XssValidate]` filters from the base class
3. Override `Config()` to return a `Module.ModuleConfig` struct that drives generic list/search/export behaviour

`AdminCoreController.Index()` builds a dynamic `SELECT` from `ModuleConfig` fields — adding date-range filters, LIKE/exact-match search, pagination, and sort. Controllers only override `Index()` when they need non-standard query logic.

> **เพิ่ม controller ใหม่**: view ไปที่ `Areas/Admin/Views/{ControllerName}/` และ **ต้องลงทะเบียนโมดูลใน `AdminMenu.cs`**
> ไม่งั้นเมนูไม่ขึ้นและเปิดหน้าไม่ได้ · ตาราง `tb_*` ใช้ `AdminLegacyController` แทน (ดูหัวข้อเมนู legacy ด้านล่าง)

### Module System (`Areas/Admin/Helpers/Module.cs` + `Areas/Admin/Helpers/AdminMenu.cs`)

`Module.ModuleConfig` — ฟิลด์ที่ความหมายไม่ตรงกับชื่อ (ที่เหลืออ่าน `Module.cs` เอา):
- `Table` — ชื่อตาราง **แบบตรรกะ** (ไม่มี prefix) เช่น `"web_core_news"`; จุดที่ประกอบ SQL ต้องผ่าน `Db.T()` เอง
- `TableModuleID` — `module_id` ที่ใช้แยกเมนูที่ใช้ตารางร่วมกัน (`web_core_*` ใช้ตารางเดียวหลายเมนู)
- `UseView{List,Create,Edit,Detail}From` — ยืม view ของโมดูลอื่นแทนการสร้างซ้ำ
- `ListData` — คอลัมน์ที่แสดงในหน้า list · ปุ่ม Preview ผูกกับ **คอลัมน์แรก** ของรายการนี้

ชื่อฟิลด์ที่เหลือ (ใช้ตามนี้ อย่าประดิษฐ์เอง): `TableCate*` · `FieldSearch` / `FieldSearchIsEqual` ·
`EnableDateSearch` / `EnableIssueDate` / `EnableViewDetail` · `OrderBy` / `Sort` / `Page` / `PerPage` ·
`FieldCreate` / `FieldUpdate` / `FieldApprove` · `ExportData` (Excel ผ่าน EPPlus) ·
`Can{Add,Edit,Delete,Move,Status,Export,Approve}` — ธงสิทธิ์ที่ `[ModuleCheck]` ตรวจ

`AdminMenu.cs` นิยามโมดูลทั้งหมดใน `AllModule()` และจัดเป็นกลุ่ม sidebar ใน `Menu()`
(จำนวนเมนู/กลุ่มเปลี่ยนบ่อยเพราะยังเปิด-ปิดอยู่ — ดูของจริงที่ `docs/backend-menu-status.html` อย่านับจากที่นี่)

#### ⚠ สถานะเมนูด้านซ้ายตอนนี้ : เปิดเฉพาะกลุ่ม "ผู้ดูแลระบบ"

ตั้งแต่ 2026-08-29 เมนูด้านซ้าย **ถูกปิดไว้ทั้งหมด เหลือเฉพาะกลุ่ม `ผู้ดูแลระบบ`** (4 เมนู)
บวกเมนูคงที่ในเทมเพลต (Dashboard / View profile / Change Password / Last Activity)

- ปิดด้วยการ **`//` comment ทีละบรรทัดใน `Menu()`** เท่านั้น — *ไม่มีการลบ* controller / view / `ModuleConfig` ใด ๆ
  ทุกโมดูลยังเข้าถึงได้ตรง ๆ ทาง URL (`/Admin/<Link>`) และพร้อมเปิดคืนทันที
- ทุกกลุ่มที่ถูกซ่อนมี comment กำกับ `//----- [ซ่อนจากเมนูด้านซ้าย] ...` ไว้ใต้ `#region`
- **เปิดคืน**: ลบ `//` ทั้งบรรทัด `listMenu.Add(...)` ของกลุ่ม + บรรทัดเมนูที่ต้องการ
  (ทุกบรรทัดมี `,` ปิดท้ายแล้ว เปิดชุดไหนก็ได้โดย syntax ไม่พัง)

> **กฎ: แก้เมนูด้านซ้ายเมื่อไร ต้องอัปเดต `docs/backend-menu-status.html` ทุกครั้ง**
> (เพิ่ม / แก้ชื่อ / ซ่อน / เปิดเมนู) — ไฟล์นี้เป็นรายงาน static 2 ตาราง
> (เมนูที่เปิดใช้งาน / เมนูที่ปิดไว้) ให้เจ้าของโปรเจกต์เปิดดูได้ว่าตอนนี้เมนูไหนเปิดเมนูไหนปิด
> `docs/*` ถูก gitignore ทั้งโฟลเดอร์ — **track จริงแค่ 2 ไฟล์**คือไฟล์นี้กับ `docs/preview-spec.md`
> (un-ignore ไว้ใน `.gitignore`) เอกสาร audit ตัวอื่นใน `docs/` มีอยู่เฉพาะเครื่องที่สร้างมัน

## เมนูที่พอร์ตมาจากหลังบ้านเดิมของ Asset Plus (ตาราง `tb_*`)

หลังบ้านเดิมคือ ASP WebForms ที่ `http://localhost:8099/assetplus/backoffice/`
(ซอร์สอยู่ที่ `D:\Project\assetfund.co.th.old\assetplus\backoffice`)
เมนู 11 ตัวถูกสร้างขึ้นใหม่ในระบบนี้ โดย **ใช้ตารางเดิมร่วมกัน ห้ามแก้โครงสร้างตาราง**

| กลุ่มเมนู | เมนู | Module | ตารางเดิม | โฟลเดอร์ต้นทาง |
|---|---|---|---|---|
| หน้าหลัก | Get Other Indices | `ApOtherIndices` | `tb_home_other_indices` | mod_tb_home_other_indices |
| ข้อมูลกองทุน | ประเภทกองทุนรวม | `ApFundCat` | `tb_fund_cat` | mod_tb_fund_cat |
| ข้อมูลกองทุน | Get Fund Fact Sheet | `ApFundFactSheet` | `tb_fund_fundfact` | mod_tb_fund_fundfact |
| ข้อมูลกองทุน | Get NAV | `ApFundNav` | `tb_fund_nav` | mod_tb_fund_nav |
| ข้อมูลกองทุน | Delete NAV | `ApFundNavDelete` | `tb_fund_nav` | mod_tb_fund_nav_del |
| ข้อมูลกองทุน | Get Performance | `ApFundPerformance` | `tb_fund_performance` | mod_tb_fund_performance |
| ปฏิทินกองทุน | หมวดหมู่ปฏิทิน | `ApCalendarCat` | `tb_calendar_category` | mod_tb_calendar_category |
| ปฏิทินกองทุน | ปฏิทินกองทุน | `ApCalendar` | `tb_calendar` | mod_tb_calendar |
| กองทุนสำรองเลี้ยงชีพ | Factsheet (Group) | `ApProvSheetCat` | `tb_fund_prov_sheet_cat` | mod_tb_fund_prov_sheet_cat |
| กองทุนสำรองเลี้ยงชีพ | Factsheet | `ApProvSheet` | `tb_fund_prov_sheet` | mod_tb_fund_prov_sheet |
| กองทุนสำรองเลี้ยงชีพ | ข้อมูลอื่นๆ | `ApProvOther` | `tb_fund_prov_other` | mod_tb_fund_prov_other |

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
· `AssetPlusImportControllers.cs` = เมนู "Get ..." 4 ตัว + Delete NAV · `Helpers/AssetPlusWsClient.cs` = ตัวเรียก SOAP
`ASPWS.asmx` · `Views/Ap*/` = ฟอร์มของแต่ละเมนู

**`ModuleConfig` ของทั้ง 11 เมนูอยู่ที่ `Areas/Admin/Helpers/AdminMenuAssetPlus.cs`** (`AssetPlusLegacyModules()`)
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
- **เมนู "Get ..."** : เลือกวันที่ → เรียก web service → แปลง XML → เขียนลงตารางเดิม
  แถวเดิมของคีย์เดียวกันถูกตั้ง `Flag = 0` แล้ว insert แถวใหม่ `Flag = 1`, `status/pb_status/show_front = 1` (เผยแพร่ทันที)
  - endpoint : `appsettings → AssetPlusWS:URL` (ค่าเดิมจาก `assetplus/web.config` = `http://167.179.243.42:53556/ws/ASPWS.asmx`)
  - operation : `MartketOtherIndices(date)` / `NAVAnnounce()` / `FundReturnPerformance(date)` / `FundFactSheet(fundDate)`
  - **ระบบใหม่เพิ่ม "อัปโหลดไฟล์ XML"** ไว้ใช้เมื่อ web service เข้าไม่ถึง (ตอนพัฒนา endpoint นี้ ping ไม่ผ่าน)
    โครงสร้างไฟล์เดียวกับที่ระบบเดิมเซฟไว้ใน `mod_*/xml_file/`
  - Fund Fact Sheet map แบบ generic : element ใน XML ที่ชื่อ **ตรงกับคอลัมน์จริง** จะถูกเขียนลงคอลัมน์นั้น
    (ตาราง `tb_fund_fundfact` มี ~270 คอลัมน์ — วิธีนี้รองรับ element ใหม่โดยไม่ต้องแก้โค้ด)

### ข้อควรระวัง

- **วันที่ในฟอร์มเป็น พ.ศ.** (culture ของแอปคือ th-TH) — ตัวแปลงใน `LegacyFields()` ใช้ culture ปัจจุบัน
  จึงบันทึก `31/12/2569` เป็น `2026-12-31` ถูกต้อง ห้ามเปลี่ยนไปใช้ `InvariantCulture`
- **ห้ามลบกลุ่มที่ยังมีลูก** — `ApCalendarCat` ผูกลูกด้วย `datatype` (ไม่ใช่ `cat_id`) จึงตั้ง
  `LegacyParentField = "datatype"` ด้วย (หลังบ้านเดิมไม่ได้กันไว้ ระบบใหม่กันเพิ่มเพื่อไม่ให้เกิด orphan)
- **สิทธิ์เมนู** อยู่ใน `2026_web_admin_module` — เพิ่มเมนูใหม่ต้อง insert สิทธิ์ให้ `access_id` ที่ต้องการ
  ไม่งั้นเมนูจะไม่ขึ้นและเปิดหน้าไม่ได้

### Authentication & Authorization

Session-based auth (no ASP.NET Identity). Session keys: `admin_login`, `admin_user`, `admin_pass`, `admin_web_id`. Every request through `[AdminLogin]` re-validates the session against the DB via `AdminHelpers.CheckAdmin()`.

`[ModuleCheck]` checks `web_admin_module` table for per-user CRUD permissions on the current module.

### Key Helpers

> ⚠ helper ทุกตัวอยู่ใต้ **`Areas/Admin/Helpers/`** ไม่ใช่ `Helpers/` ที่ root (มีแต่ `Services/` เท่านั้นที่อยู่ root)

| File | Purpose |
|---|---|
| `Areas/Admin/Helpers/DBHelper.cs` | Raw SQL execution (SQL Server + MySQL) |
| `Areas/Admin/Helpers/Db.cs` | `Db.T("web_admin")` → `[2026_web_admin]` — ชื่อตรรกะ → ชื่อจริงใน SQL Server |
| `Areas/Admin/Helpers/AdminHelpers.cs` | Session auth, menu loading |
| `Areas/Admin/Helpers/Utility.cs` | `GenerateSHA512String` (hash รหัสผ่าน), `Encrypt`/`Decrypt` (AES), `Email()` ผ่าน MailKit/MimeKit |
| `Areas/Admin/Helpers/WebService.cs` | HTTP client calls to the backend API |
| `Services/AuditLogService.cs` | Writes to `2026_api_audit_log` (SQL Server, scoped) |

> แพ็กเกจ `BCrypt.Net-Next` ติดตั้งอยู่ใน csproj แต่ **`Utility.cs` ไม่ได้ใช้** — การ hash รหัสผ่านใช้ SHA512 (ดูหัวข้อ "กลไก login")
> ส่วน Serilog ตั้งค่าที่ `Program.cs` (`builder.Host.UseSerilog()`) ไม่ได้อยู่ใน `Utility.cs`

### ระบบ Preview (ดูตัวอย่างหน้าเว็บของ "ฉบับร่าง")

พรีวิวค่า "ฉบับร่าง" (คอลัมน์ที่ไม่ใช่ `pb_*`) ผ่านปุ่ม **[Preview]** ในหน้า list (เมนูที่รองรับ ดูทะเบียนใน `PreviewMenu.cs`)
**ฝั่ง admin ทำเสร็จแล้ว** (`Areas/Admin/Helpers/PreviewMenu.cs` + `_PartialFrontPreviewModal.cshtml`
+ `openFrontPreview()` ใน `wwwroot/js/Admin/admin_site.js` + ปุ่มใน `AdminCore/Index.cshtml`)
แต่ **ยังใช้งานจริงไม่ได้** เพราะฝั่ง front-end ยังไม่มี `Helpers/PreviewMap.cs` และ route `/_preview/...`

> **กฎ: แก้ back-end ของเมนูใด — ฟิลด์ / `ModuleConfig` / `ListData` / `Table` / `TableModuleID` / ชื่อโมดูล —
> ต้องตรวจผลกระทบต่อ Preview ของเมนูนั้นด้วยเสมอ** (พังหรือแสดงค่าผิดโดยไม่มีใครรู้)
> ทะเบียน 2 ฝั่งเป็นคนละโปรเจกต์ ต้อง sync กันเอง: `PreviewMenu.cs` ↔ `PreviewMap.cs`

📄 **สเปกเต็ม** (3 โหมด item/page/cms, กฎระดับแถว, เช็คลิสต์เมื่อแก้ back-end, หลักการที่ห้ามพัง, วิธีทดสอบ)
→ `docs/preview-spec.md`

### Frontend

Razor + jQuery + CKEditor + elFinder · ไม่มี npm/bundler (lib commit ตรง ๆ ใน `wwwroot/lib/`)
UI ที่ใช้ซ้ำ (breadcrumb, pagination, ปุ่ม action) เป็น view component ที่ `Areas/Admin/Views/ViewComponents/`

## Database Connections (Development)

```
SQL Server: Server=.; Database=asset_plus_uat; User ID=sa;   Password=sasa
MySQL:      Server=localhost; Database=sam_npa;        UserID=root; Password=(ว่าง)
```

**ที่มาของข้อมูล** — ตาราง `2026_*` ทั้ง 69 ตัวถูกย้ายมาจาก PostgreSQL `asset_fund_temp` (ซึ่งเป็นสำเนาของ `sam`) เมื่อ 2026-08-29
ยืนยันแล้วว่าตรงกันครบ 31,036 แถว / 395,809 cell รวมถึง identity seed และลำดับ `sort`

ชนิดข้อมูลที่แปลงแล้วและมีผลกับการเขียนโค้ด: `timestamptz` → `datetimeoffset(7)` (เก็บ `+07:00`),
`jsonb` → `nvarchar(max)` (อ่านด้วย `JSON_VALUE`), `text`/`varchar` → `nvarchar` ทั้งหมด (ข้อมูลเป็นภาษาไทย)

⚠️ **คอลัมน์เวลา**: `DBHelper` แปลงให้ 2 ทางเพื่อให้โค้ดเดิมทำงานเหมือนเดิม —
ตอน **เขียน** ผูก offset ของเครื่องให้ `DateTime` ก่อน (ไม่งั้น SQL Server ถือเป็น `+00:00` แล้วเวลาเพี้ยน 7 ชม.)
และตอน **อ่าน** แปลง `DateTimeOffset` กลับเป็น `DateTime` แบบ UTC (พฤติกรรมที่ Npgsql เคยให้ — ทุก view เรียก `.ToLocalTime()` เองอยู่แล้ว)
อย่าถอดตัวแปลงนี้ออกโดยไม่ไล่แก้ทุกจุดที่ cast เป็น `DateTime`

**ตรวจข้อมูลตรง ๆ** (มี `sqlcmd` ติดตั้งอยู่แล้ว):
```bash
"/c/Program Files/Microsoft SQL Server/Client SDK/ODBC/170/Tools/Binn/sqlcmd"   -S . -U sa -P sasa -d asset_plus_uat -h -1 -W   -Q "set nocount on; select top 5 id, title from [2026_web_core_news] order by id desc;"
```

## รันและทดสอบเว็บไซต์

- ปกติเซิร์ฟเวอร์รันค้างอยู่แล้วที่ `https://localhost:7140/` — **ไม่ต้อง `dotnet run` ใหม่ถ้าเข้าได้**
- ถ้าเข้าไม่ได้ → `dotnet run --launch-profile https` แล้ว **รอจนพอร์ต 7140 listen** (ใช้เวลาสักครู่) ค่อยทดสอบ
- ทดสอบด้วย **Playwright** และ **ต้อง login ก่อนเสมอ** ด้วย `user` / `P@ssw0rd`
- ลำดับการเข้าหลังบ้าน: `https://localhost:7140/` → redirect ไป `/Admin/User/Login?webID=&targetUrl=/Admin`
  → กรอก user/pass โดยปล่อย dropdown ไว้ที่ **เว็บไซต์หลัก** (webID = 0) → `/Admin/User/Dashboard`
  → กดปุ่ม **Admin Panel** → `/Admin/User/LastActivity` คือหน้าหลังบ้าน
- เก็บ screenshot ไว้ใน `.playwright-mcp/` เสมอ (git ignore แล้ว เป็นไฟล์ชั่วคราว ลบทิ้งได้)

### รัน server ให้อยู่รอดหลังปิด Claude Code
`dotnet run` ที่สั่งผ่าน background task ของ Claude Code จะ**ถูก kill เมื่อ session จบ** ถ้าอยากให้รันค้าง ให้ spawn แบบหลุด job object ด้วย WMI:

```powershell
$log = "$env:TEMP\core_admin-7140.log"
$cmd = 'cmd /c "dotnet run --launch-profile https > "' + $log + '" 2>&1"'
Invoke-CimMethod -ClassName Win32_Process -MethodName Create -Arguments @{CommandLine=$cmd; CurrentDirectory="d:\Project\admin.assetfund.co.th.2026\core_admin"}
```
หยุดด้วย `Get-NetTCPConnection -LocalPort 7140 | Select -Expand OwningProcess | Stop-Process -Force` (หรือรันจาก terminal แยกเองก็ได้)

### กลไก login (`Areas/Admin/Controllers/UserController.cs`)

- รหัสผ่านเก็บเป็น **SHA512 hex ตัวพิมพ์เล็ก** (`Utility.GenerateSHA512String`, UTF8, ไม่มี salt — คอลัมน์ `vsalt` ไม่ได้ใช้)
- ตาราง `[2026_web_admin]` (prefix `2026_` มาจาก `Db.Prefix`) — ปัจจุบันมี account เดียวคือ id=1 `user`, `access_id=1`, `web_id=0`
- 2FA/OTP **ถูก comment ปิดไว้ทั้งระบบ** แล้ว จึงไม่ต้องกรอก OTP
- กรอกผิดครบ `ConfigPassword:LoginFailCount` (5) ครั้ง → `LockUser()` ตั้ง `status=0` ขึ้นข้อความ "Username ของท่านถูกระงับ"
- `last_change_password_at` + `ConfigPassword:ExpiresInDay` (600 วัน) < วันนี้ → บังคับเปลี่ยนรหัสผ่านก่อนใช้งาน
- ตัวนับ login ผิดเก็บใน **session** ไม่ใช่ DB — ปิด/เปิดเบราว์เซอร์ใหม่ก็รีเซ็ตตัวนับแล้ว แต่ `status=0` ต้องแก้ที่ DB เท่านั้น

### รีเซ็ต / เปลี่ยนรหัสผ่าน admin ด้วย SQL (เมื่อ login ไม่ผ่านหรือโดนล็อก)

```bash
# 1) ทำ hash ของรหัสใหม่ (SHA512 hex ตัวเล็ก)
printf '%s' 'P@ssw0rd' | sha512sum
# 2) เขียนลง DB + ปลดล็อก + ปิด force change password
sqlcmd -S . -U sa -P sasa -d asset_plus_uat -Q "update [2026_web_admin] set password='<hash>', status=1, force_change_password=0, use_otp=0, last_change_password_at=getdate() where id=1;"
```

hash ของ `P@ssw0rd` =
`6bfcc4026b5f162799a6dc8305c09db9c1674ac616bd5c7422a45fbb6d0816ac163047c47a1f426f4f4c6b5b5042c671eabc4fdc7310fd5b183eef59dc274604`

## เว็บไซต์ front-end (เว็บสาธารณะ / public site)

Front-end คือ**คนละแอป คนละโปรเจกต์**กับ admin นี้

| แอป | โปรเจกต์ | URL (dev) |
|---|---|---|
| Admin (back-end) — repo นี้ | `d:\Project\admin.assetfund.co.th.2026\core_admin` | https://localhost:7140 |
| Public site (front-end) | `d:\Project\assetfund.co.th.2026` | https://localhost:7301 |

> ⚠️ **ยังไม่ได้ต่อกัน** — front-end ตัวใหม่ยังอยู่ระหว่างสร้าง ทุก service ยังเป็น `Mock*Service.cs` และ `appsettings.json` ยังไม่มี connection string
> ดังนั้น **ข้อมูลที่แก้ผ่าน admin จะยังไม่ปรากฏบน front-end** และระบบ Preview ก็ยังใช้ไม่ได้
> ระหว่างนี้ให้ยืนยันผลการแก้ไขด้วยการ query SQL Server ตรง ๆ
> (เมื่อ front-end ต่อ DB จริงแล้ว ให้กลับมาลบย่อหน้านี้)

> 📌 อย่าสับสนกับ `d:\Project\sam.or.th` / `d:\Project\admin.sam.or.th` ที่อยู่ในเครื่องเดียวกัน — เป็นของลูกค้าคนละราย
> โค้ด admin ชุดนี้ port มาจากที่นั่น (ดู Architecture ด้านบน) เอกสารเก่าจึงเคยอ้างถึง เปิดดูได้เฉพาะตอนอยากเทียบว่าของเดิมทำไว้อย่างไร

**ข้อตกลงระหว่างสองแอปที่ต้องรักษาไว้** (เขียนไว้ทั้งสองฝั่ง — แก้แล้วต้องตามไปแก้อีกฝั่ง):

- **รูปในเนื้อหา CMS ต้องใช้ URL แบบสัมบูรณ์ชี้มาที่โดเมน admin** (`https://localhost:7140/Files/...` หรือ `/assets/...`) เพราะไฟล์อัปโหลด (elFinder) เก็บที่ฝั่ง admin และสองแอปมี `wwwroot/Files` แยกกัน — ถ้าใช้ path สัมพัทธ์ รูปจะ 404 บน front-end (ปุ่มแทรกรูปของ elFinder ใส่ URL สัมบูรณ์ให้อัตโนมัติแล้ว)
- **ปุ่ม [Insert E-Form]** ในตัวแก้ไขเนื้อหา แทรกเป็น symbol tag `{{{E-Form:<หัวข้อ>:<id>}}}` — front-end ต้องมี parser มารับ ไม่งั้นจะแสดงเป็นข้อความดิบ (ยังไม่มี)
- **ห้ามใช้ชื่อ session cookie ซ้ำกัน** — cookie แยกตาม host เท่านั้น ไม่แยก port และทั้งสองแอปอยู่บน `localhost` เดียวกัน ถ้าชื่อชนกัน (ค่า default `.AspNetCore.Session`) การเข้า front-end จะเขียนทับ cookie ของ admin ทำให้ admin หลุด login
  → admin ตั้งเป็น `AssetPlus.Admin.Session` ไว้แล้วใน `Program.cs` **front-end ต้องใช้ชื่ออื่น**
- **ระบบ Preview** ต้องมีทะเบียนคู่กัน 2 ฝั่ง — ดูสเปกที่หัวข้อ "ระบบ Preview" ด้านบน

