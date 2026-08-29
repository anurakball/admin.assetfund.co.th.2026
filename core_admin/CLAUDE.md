# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
dotnet build                  # build the project
dotnet run                    # run (HTTP on http://localhost:5140)
dotnet run --launch-profile https  # run (HTTPS on https://localhost:7140)
```

No test project exists in this solution.

## Architecture

ASP.NET Core 10 MVC application serving an admin panel for the SAM website (`admin.sam.or.th`). The app communicates with a separate backend API (`sam.or.th/api`) via `WebService.cs` for most data mutations and reads certain data directly from the database via `DBHelper.cs`.

### Two Databases

| Role | Type | Database | Config key |
|---|---|---|---|
| Primary | SQL Server | `asset_plus_uat` (ตาราง prefix `2026_`) | `DBConnection` |
| Secondary | MySQL | `sam_npa` | `MySQLConnection` |

> **ชื่อตาราง**: ตารางของระบบนี้อยู่ใน `asset_plus_uat` ร่วมกับตารางของระบบเดิมอีก 127 ตัว (ชื่อชนกันหลายตัว)
> จึงเติม prefix `2026_` ทุกตาราง และเพราะชื่อขึ้นต้นด้วยตัวเลข T-SQL บังคับให้ครอบ `[ ]` เสมอ
> เวลาเขียน SQL ใหม่ให้ใช้ `Db.T("web_admin")` แทนการพิมพ์ชื่อตรง ๆ
>
> **ข้อควรระวังของ T-SQL** (ต่างจาก PostgreSQL เดิม): ไม่มี `LIMIT` (ใช้ `TOP` / `OFFSET…FETCH` ซึ่งต้องมี `ORDER BY`),
> ไม่มี `ILIKE` (คอลเลชัน `Thai_CI_AS` ไม่สนตัวพิมพ์อยู่แล้ว), ไม่มี `::cast` (ใช้ `CAST`/`TRY_CAST`),
> `GROUP BY` ห้ามมี subquery และอ้างลำดับคอลัมน์ไม่ได้, `SUM(CASE…)` คืน `NULL` เมื่อไม่มีแถว (ครอบ `COALESCE`),
> `COUNT(*)` คืน `int` (ไม่ใช่ `bigint`), และ database นี้อยู่ที่ compatibility level 100 จึงใช้ `OPENJSON`/`STRING_SPLIT` ไม่ได้

All queries are raw parameterized SQL — no ORM. `DBHelper.cs` wraps `Microsoft.Data.SqlClient` and `MySqlConnector`, and logs queries to console in Development (except module/access-check queries).

### System audit reference (อ่านก่อนงานที่ต้องเข้าใจภาพรวมทั้งระบบ)

มีเอกสาร audit ฉบับเต็ม 2 ไฟล์ (สร้างไว้แล้ว — ใช้แทนการ re-audit ทุกเมนู):
- `docs/backend-menu-audit.md` — ทุกเมนูหลังบ้าน ~127 เมนู / 33 กลุ่ม (table, สิทธิ์, ฟิลด์, หน้าที่) เรียง #1–#127
- `docs/frontend-to-backend-map.md` — ทุกหน้า front-end (7169) map ไปเมนูหลังบ้าน (#N อ้างไฟล์บน)

ข้อเท็จจริงสถาปัตยกรรมหลัก (durable — ใช้ตั้งต้นได้เลย):
- **ทรัพย์สิน NPA ทุกใบอยู่ MySQL `sam_npa.tb_product2`** (ค้นหา/detail/แผนที่/นับ/LED source=2/AI). SQL Server `asset_plus_uat` เป็น overlay เท่านั้น: `web_npa_status` (สถานะ/highlight/ผูกย่าน+facility), `web_core_group` (ย่าน=NpaLocate, facility=NpaFacility), + เก็บ submission
- **Approval workflow** ใช้คอลัมน์คู่ `pb_*` (pending) — front-end อ่าน `pb_*` (ค่าที่อนุมัติแล้ว); กด Approve จึง copy `pb_*`→ฟิลด์จริง
- เมนูหลังบ้านเกือบทั้งหมดเป็น thin wrapper ของ `AdminCoreController` + config กลางใน `Helpers/AdminMenu.cs` (`AllModule()`); "เครื่องยนต์เนื้อหา" ใช้ซ้ำ 4 แบบ: `web_core_single` (1 เมนู=1 ระเบียน แก้ไข t1..t50 เป็น section 2 ภาษา), `web_core_item` (การ์ดซ้ำ), `web_core_group` (หมวด), `web_core_news` (บทความเต็ม)
- Front-end (`D:\Project\sam.or.th`) มี 3 controllers; routing หลักอยู่ใน `HomeController.Index()` (~9,000 บรรทัด) resolve หน้า CMS จาก `web_cms_page.seo_url` แล้ว switch ตามโค้ด `pb_box_data` (เช่น `abo-vis`, `dep-ove`) เลือก table/view
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

### Module System (`Helpers/Module.cs` + `Helpers/AdminMenu.cs`)

`Module.ModuleConfig` declares (ชื่อจริงในโค้ด — อย่าเดาจากชื่อทั่วไป):
- `Table` — ชื่อตาราง **แบบตรรกะ** (ไม่มี prefix) เช่น `"web_core_news"`; จุดที่ประกอบ SQL ต้องผ่าน `Db.T()` เอง
- `TableModuleID` — ค่า `module_id` ที่ใช้แยกเมนูที่ใช้ตารางร่วมกัน (`web_core_*` ใช้ตารางเดียวหลายเมนู)
- `TableCate*` — ตารางหมวด + ฟิลด์/หัวข้อ/การเรียง สำหรับ dropdown กรองกลุ่ม
- `ListData` — คอลัมน์ + ป้ายภาษาไทยที่แสดงในหน้า list
- `FieldSearch` / `FieldSearchIsEqual` — ฟิลด์ในช่องค้นหา (และฟิลด์ที่ต้องเทียบแบบตรงตัว)
- `EnableDateSearch` / `EnableIssueDate` — เปิดตัวกรองช่วงวันที่ / ระบบ "วันที่แสดงผล"
- `OrderBy` / `Sort` / `Page` / `PerPage` — การเรียงและแบ่งหน้าเริ่มต้น
- `FieldCreate` / `FieldUpdate` / `FieldApprove` — ฟิลด์ที่ยอมให้เขียนในแต่ละขั้น
- `ExportData` — เปิด Excel export (EPPlus)
- `CanAdd` / `CanEdit` / `CanDelete` / `CanMove` / `CanStatus` / `CanExport` / `CanApprove` — ธงสิทธิ์ที่ `[ModuleCheck]` ตรวจ
- `UseView*From` — ยืม view ของโมดูลอื่นแทนการสร้างซ้ำ

`AdminMenu.cs` นิยาม **161 โมดูล** และจัดเป็น sidebar **146 เมนู / 35 กลุ่ม**

### Authentication & Authorization

Session-based auth (no ASP.NET Identity). Session keys: `admin_login`, `admin_user`, `admin_pass`, `admin_web_id`. Every request through `[AdminLogin]` re-validates the session against the DB via `AdminHelpers.CheckAdmin()`.

`[ModuleCheck]` checks `web_admin_module` table for per-user CRUD permissions on the current module.

### Key Helpers

| File | Purpose |
|---|---|
| `Helpers/DBHelper.cs` | Raw SQL execution (SQL Server + MySQL) |
| `Helpers/Db.cs` | `Db.T("web_admin")` → `[2026_web_admin]` — ชื่อตรรกะ → ชื่อจริงใน SQL Server |
| `Helpers/AdminHelpers.cs` | Session auth, menu loading |
| `Helpers/Utility.cs` | BCrypt hashing, Serilog logging, MailKit email |
| `Helpers/WebService.cs` | HTTP client calls to the backend API |
| `Services/AuditLogService.cs` | Writes to `2026_api_audit_log` (SQL Server, scoped) |

### Conventions for New Controllers

- Place under `Areas/Admin/Controllers/`
- Inherit `AdminCoreController`
- Override `Config()` and return a populated `ModuleConfig`; only override `Index()` for complex queries
- Place views under `Areas/Admin/Views/{ControllerName}/`
- Register the module in `AdminMenu.cs`

### ระบบ Preview (ดูตัวอย่างหน้าเว็บของ "ฉบับร่าง") ⚠ ต้องเช็คทุกครั้งที่แก้ back-end

> 🔌 **สถานะปัจจุบัน: พรีวิวใช้งานจริงไม่ได้ชั่วคราว** — iframe ชี้ไป front-end (7169) ซึ่งยังอ่าน PostgreSQL `sam`
> ส่วน admin ย้ายมา SQL Server แล้ว จึงไม่เห็นข้อมูลที่แก้จาก admin
> โค้ดฝั่ง admin (`PreviewMenu.cs`) แปลงเป็น T-SQL ไว้เรียบร้อยแล้ว รอแค่ย้าย front-end ตามมา
> เนื้อหาด้านล่างยังใช้อ้างอิงได้ทั้งหมด

> **กฎ: เมื่อแก้ไขระบบ back-end ของเมนูใด ให้ตรวจผลกระทบต่อ Preview ของเมนูนั้นด้วยเสมอ**
> (แก้ฟิลด์/`ModuleConfig`/`ListData`/ตาราง/`module_id`/ชื่อโมดูล → พรีวิวอาจพังหรือแสดงค่าผิดโดยไม่มีใครรู้)

ทุกรายการ (ไม่จำกัดสถานะอนุมัติ) จะมีปุ่ม **[Preview]** หน้าคอลัมน์แรกในหน้า list
กดแล้วเปิด modal ขนาดเกือบเต็มจอ (`max-width:95vw` × iframe สูง `85vh` จัดกลางจอ — คลิกขอบนอก modal เพื่อปิดได้) iframe ไปที่หน้าเว็บ front-end ที่แสดง **ค่าฉบับร่าง** (คอลัมน์ที่ไม่ใช่ `pb_*`)
ขณะที่หน้าเว็บจริงยังคงแสดงค่าที่อนุมัติแล้ว — ปัจจุบันรองรับ **64 เมนู**

| ไฟล์ | ฝั่ง | หน้าที่ |
|---|---|---|
| `Areas/Admin/Helpers/PreviewMenu.cs` | admin | ทะเบียน `ชื่อโมดูล → โหมด (item/page/cms)` + สร้าง URL + `PreviewRowGate` (กฎระดับแถว) |
| `Areas/Admin/Views/Shared/_PartialFrontPreviewModal.cshtml` | admin | modal เต็มจอ + iframe + ปุ่มปิด/สลับภาษา |
| `openFrontPreview()` ใน `wwwroot/js/Admin/admin_site.js` | admin | เปิด/ปิด modal, โหลด iframe |
| `Areas/Admin/Views/AdminCore/Index.cshtml` | admin | ปุ่ม `[Preview]` (branch `hasFrontPreview && head.Key == previewColumnKey`) + สร้าง `previewGate` |
| `d:\Project\sam.or.th\Helpers\PreviewMap.cs` | front-end | ทะเบียนเมนู + `PreviewState` (สลับ SQL เป็นคอลัมน์ฉบับร่าง) + `RowPreviewable()` (กฎระดับแถว) |
| `PreviewItem/PreviewPage/PreviewCms` ใน `HomeController.cs` | front-end | route `/_preview/{item,page,cms}/{module}/{id}` |

**3 โหมด**
- `item` — เมนูที่มีหน้ารายละเอียดรายตัว (News, AnnouncePro, Promotion) → เรนเดอร์ view นั้นด้วยค่าร่าง
- `page` — เนื้อหาไปโผล่เป็น section/รายการในหน้าเว็บหน้าหนึ่ง (ส่วนใหญ่) → เปิดหน้านั้นทั้งหน้าผ่าน `Index()` เดิม แต่ข้อมูลของเมนูเป้าหมายอ่านจากคอลัมน์ฉบับร่าง
- `cms` — แก้ตัวหน้า CMS เอง (CMSPage, CMSPageFooter1/2) → โหลดระเบียนตาม id ด้วยคอลัมน์ฉบับร่าง (เฉพาะ `page_type` 3 กับ 4)

**กฎระดับแถว (ปิดพรีวิวรายแถวที่ "แก้แล้วพรีวิวไม่เห็นผล")**
บางแถวหน้าเว็บ front-end ไม่ได้อ่านค่าของมันเลย พรีวิวจึงดูเหมือนแก้ไม่สำเร็จ → **ซ่อนปุ่ม** (admin: `PreviewRowGate`)
และ **ตอบ 404** (front-end: `PreviewMap.RowPreviewable()`) — กฎมาจากการกวาดทดสอบทุกแถวของทุกเมนู (423 แถว):
- เมนู `cms`: `page_type` ไม่ใช่ 3/4 **หรือ** `box_data ∈ {npa-sea, art, staff, cal}` (view เขียนหัวข้อ/เนื้อหาไว้ตายตัว)
- `LinkGroup`: หมวดที่ไม่มีลิงก์เผยแพร่ (Organization.cshtml ข้ามหมวดว่างทั้งหมวด)
- `AboutBoard3cat`: หมวดที่ไม่มีสมาชิก + หมวดแรกตาม sort (Executives.cshtml ไม่แสดงหัวข้อหมวดแรก)
- `NpaLocate`: ย่านที่ไม่ได้ถูกเลือกใน `web_core_single` module 6 (`pb_t2..pb_t11`) — หน้าแรกสร้างชิปจากช่องพวกนี้เท่านั้น
- เมนู `web_core_single` ที่ front-end อ่านด้วย `LoadCoreSingle()` (`ORDER BY id DESC LIMIT 1`): ระเบียนที่ไม่ใช่ id มากสุด
⚠ กฎ 2 ฝั่งต้อง sync กันเสมอ (คนละโปรเจกต์) — ถ้าแก้ view ฝั่ง front-end ให้เรนเดอร์แถวเหล่านี้ได้แล้ว ต้องถอนกฎออกทั้ง 2 ฝั่ง

**หลักการที่ห้ามพัง**
- `PreviewState.Cols()` / `Gate()` **นอกโหมดพรีวิวต้องคืน SQL เดิมทุกตัวอักษร** — ถ้าเพี้ยนจะกระทบหน้าเว็บจริงทุกหน้า
- โหมดพรีวิว **ตัดเงื่อนไข** `status` / `show_front` / ช่วงวันที่ ของเมนูเป้าหมายทิ้ง และปิด pagination/ตัวกรอง (แสดงทุกรายการในหน้าเดียว) เพื่อให้เห็นรายการที่กำลังแก้เสมอ
- สถานะเก็บที่ `HttpContext.Items` (อายุ 1 request) เพราะ `layoutContentService` เป็น Singleton — **ห้ามเปลี่ยนเป็น static/field ของ service**
- ทะเบียน 2 ฝั่งเป็นคนละโปรเจกต์ **ต้อง sync กันเอง** (`PreviewMenu.cs` ↔ `PreviewMap.cs`) — ชื่อโมดูลถูกส่งเป็น segment ใน URL

**เช็คลิสต์เมื่อแก้ back-end**
1. เปลี่ยนชื่อโมดูล / `Table` / `TableModuleID` → แก้ทะเบียนทั้ง 2 ฝั่ง
2. เพิ่ม/ลบฟิลด์ที่หน้าเว็บใช้ → ตรวจว่าคอลัมน์นั้นมีคู่ `pb_*` ครบ (พรีวิว alias `x AS pb_x` อัตโนมัติจาก `information_schema`)
3. เปลี่ยน `ListData` → ปุ่มผูกกับ **คอลัมน์แรก** ของ `ListData` (ถ้าคอลัมน์แรกเป็นรูป ปุ่มจะไปอยู่ผิดที่)
4. เพิ่มเมนูใหม่ที่มีหน้าบนเว็บ → เพิ่มในทะเบียน 2 ฝั่ง แล้วทดสอบว่าพรีวิวเห็นค่าร่าง / หน้าจริงไม่เห็น
5. แตะ loader กลางใน `HomeController.cs` (`LoadCore*`) → ทดสอบ regression หน้าเว็บจริงทุกหน้า

**วิธีทดสอบ** — ตั้ง marker ในคอลัมน์ฉบับร่าง แล้วยืนยัน 2 ทางเสมอ: พรีวิว **ต้องเห็น** marker และหน้าเว็บจริง **ต้องไม่เห็น** จากนั้นคืนค่าเดิม

### Frontend

Razor views with jQuery, CKEditor (rich text), and elFinder (file manager). View components for shared UI (breadcrumbs, pagination, action buttons) live in `Areas/Admin/Views/ViewComponents/`.

Static assets under `wwwroot/`. No npm/bundler pipeline — libraries are committed directly to `wwwroot/lib/`.

## Database Connections (Development)

```
SQL Server: Server=.; Database=asset_plus_uat; User ID=sa; Password=<see appsettings.Development.json>
MySQL:      Server=localhost; Database=sam_npa;        UserID=root;     Password=<see appsettings.Development.json>
```

**ที่มาของข้อมูล** — ตาราง `2026_*` ทั้ง 69 ตัวถูกย้ายมาจาก PostgreSQL `asset_fund_temp` (ซึ่งเป็นสำเนาของ `sam`) เมื่อ 2026-08-29
ยืนยันแล้วว่าตรงกันครบ 31,036 แถว / 395,809 cell รวมถึง identity seed และลำดับ `sort`

การแปลงชนิดข้อมูลที่ใช้ (จำไว้เวลาเทียบค่ากับ PostgreSQL เดิม):

| PostgreSQL | SQL Server |
|---|---|
| `text` / `varchar(n)` | `nvarchar(max)` / `nvarchar(n)` — ใช้ `n*` เพราะข้อมูลเป็นภาษาไทย |
| `timestamptz` | `datetimeoffset(7)` (เก็บ `+07:00`) |
| `timestamp` | `datetime2(7)` |
| `boolean` | `bit` |
| `jsonb` | `nvarchar(max)` (อ่านด้วย `JSON_VALUE`) |
| `identity`/`serial` | `IDENTITY(1,1)` |

⚠️ **คอลัมน์เวลา**: `DBHelper` แปลงให้ 2 ทางเพื่อให้โค้ดเดิมทำงานเหมือนเดิม —
ตอน **เขียน** ผูก offset ของเครื่องให้ `DateTime` ก่อน (ไม่งั้น SQL Server ถือเป็น `+00:00` แล้วเวลาเพี้ยน 7 ชม.)
และตอน **อ่าน** แปลง `DateTimeOffset` กลับเป็น `DateTime` แบบ UTC (พฤติกรรมที่ Npgsql เคยให้ — ทุก view เรียก `.ToLocalTime()` เองอยู่แล้ว)
อย่าถอดตัวแปลงนี้ออกโดยไม่ไล่แก้ทุกจุดที่ cast เป็น `DateTime`

**ตรวจข้อมูลตรง ๆ** (มี `sqlcmd` ติดตั้งอยู่แล้ว):
```bash
"/c/Program Files/Microsoft SQL Server/Client SDK/ODBC/170/Tools/Binn/sqlcmd"   -S . -U sa -P <password> -d asset_plus_uat -h -1 -W   -Q "set nocount on; select top 5 id, title from [2026_web_core_news] order by id desc;"
```

## ทดสอบการทำงานเว็บไซต์
- เมื่อต้องการทดสอบการทำงานของเว็บไซต์ ให้ใช้ **Playwright** เข้าไปที่ `https://localhost:7140/` (ปกติเซิร์ฟเวอร์รันอยู่แล้ว ไม่ต้อง `dotnet run` ใหม่)
- ถ้า `https://localhost:7140/` เข้าไม่ได้ (เซิร์ฟเวอร์ไม่ได้รัน) ให้ `dotnet run` ก่อน แล้วค่อยทดสอบด้วย Playwright
- เมื่อสั่ง `dotnet run` ต้องรอสักครู่ (เซิร์ฟเวอร์ใช้เวลาเริ่มทำงานนานหน่อย) ให้รอจนพอร์ต 7140 listen ก่อน แล้วค่อยเข้าทดสอบด้วย Playwright
- **ทุกครั้งที่เข้าเว็บไซต์เพื่อทดสอบ ต้อง login ก่อนเสมอ** ด้วยข้อมูลนี้:
  - username: `user`
  - password: ดูใน `appsettings.Development.json` (ไฟล์ local ไม่ commit) หรือถามเจ้าของโปรเจกต์
- หลัง login เว็บโหลดเสร็จ ให้กดปุ่ม **Admin Panel** เพื่อเข้าสู่หน้าหลัก
- เก็บ screenshot ไว้ใน `.playwright-mcp/` เสมอ (โฟลเดอร์นี้ถูก git ignore แล้ว เป็นไฟล์ชั่วคราว ลบทิ้งได้)

## เว็บไซต์ front-end (เว็บสาธารณะ / public site)

Front-end คือ**คนละแอป**กับ admin นี้ อยู่คนละโปรเจกต์ — เดิมใช้ PostgreSQL `sam` ร่วมกัน แต่ admin ย้ายมาใช้ SQL Server `asset_plus_uat` (ตาราง prefix `2026_`) แล้ว จึง**ไม่ได้ใช้ฐานข้อมูลตัวเดียวกันอีก**

| แอป | โปรเจกต์ | URL (dev) |
|---|---|---|
| Admin (back-end) | `d:\Project\admin.sam.or.th\core_admin` (repo นี้) | https://localhost:7140 |
| Public site (front-end) | `d:\Project\sam.or.th` | https://localhost:7169 |

> ⚠️ **สองแอปนี้แยก DB กันแล้ว** — admin เขียนลง SQL Server `asset_plus_uat` (ตาราง `2026_*`) ส่วน front-end ยังอ่าน PostgreSQL `sam` อยู่
> ดังนั้น **ข้อมูลที่แก้ผ่าน admin จะยังไม่ปรากฏบน front-end (7169)** และระบบ Preview ก็ใช้ไม่ได้จนกว่าจะย้าย front-end ตามมา
> (เดิมทั้งคู่ใช้ `sam` ร่วมกัน จึงเคยเปิด 7169 ยืนยันผลได้ทันที — workflow นั้นใช้ไม่ได้แล้ว)
> ระหว่างนี้ให้ยืนยันผลการแก้ไขด้วยการ query SQL Server ตรง ๆ แทน

- **ดูผลหน้าเว็บที่สร้าง/แก้ผ่าน admin** ได้ที่ front-end โดยตรง — หน้า CMSPage (Page Content) เปิดที่ `https://localhost:7169/th/<seo_url>` (ภาษาอังกฤษใช้ `/en/<en_seo_url>`) เช่น `/th/prawat-kan-khai-sinsap-tuayang`
- **ทดสอบ front-end ด้วย Playwright** ได้เลย ไม่ต้อง login (เป็นหน้าสาธารณะ) ถ้าพอร์ต 7169 เข้าไม่ได้แปลว่าแอป front-end ไม่ได้รัน
- **รูปในเนื้อหา CMS ต้องใช้ URL แบบสัมบูรณ์ชี้มาที่โดเมน admin** (`https://localhost:7140/Files/...` หรือ `https://localhost:7140/assets/...`) เพราะไฟล์อัปโหลด (elFinder) เก็บที่ฝั่ง admin และสองแอปมี `wwwroot/Files` แยกกัน — ถ้าใช้ path สัมพัทธ์ (`/Files/...`) รูปจะ 404 บน front-end ปุ่มแทรกรูปของ elFinder จะใส่ URL สัมบูรณ์ให้อัตโนมัติอยู่แล้ว
- **ปุ่ม [Insert E-Form]** ในตัวแก้ไขเนื้อหา แทรกเป็น symbol tag `{{{E-Form:<หัวข้อ>:<id>}}}` — ปัจจุบัน front-end **ยังไม่มี parser** สำหรับ tag นี้ จึงแสดงเป็นข้อความดิบ (เป็น tag สัญลักษณ์ไว้ก่อน)
- **ข้อควรระวัง cookie ชนกัน:** cookie แยกตาม host เท่านั้น ไม่แยก port ทั้งสองแอปอยู่บน `localhost` เดียวกัน ถ้าใช้ชื่อ session cookie เหมือนกัน (ค่า default `.AspNetCore.Session`) การเข้า front-end จะเขียนทับ cookie ของ admin ทำให้ admin หลุด login — จึงตั้งชื่อ cookie ของ admin เป็น `SAM.Admin.Session` แล้วใน `Program.cs`

## สร้างรูปด้วย ComfyUI (AI image generation)

เครื่องนี้มี **ComfyUI Desktop** ใช้สร้างรูปประกอบ/พื้นหลังได้ — ทำงานผ่าน HTTP API ตรง ๆ ไม่ต้องเปิด UI

| หัวข้อ | ค่า |
|---|---|
| API | `http://127.0.0.1:8188` (POST `/prompt`, GET `/history/{id}`, `/object_info`, `/system_stats`) |
| ถ้าไม่ได้รัน (พอร์ต 8188 ไม่ listen) | เปิด `C:\Users\ball\AppData\Local\Programs\Comfy Desktop\Comfy Desktop.exe` แล้วรอ ~40 วิ |
| โฟลเดอร์ output ของ ComfyUI | `C:\Users\ball\AppData\Local\Comfy-Desktop\ComfyUI-Shared\output` |
| **ที่เก็บรูปที่สร้างเสร็จ** | **`docs/ai-images/`** (ย้ายมาที่นี่เสมอ) |
| GPU | RTX 4060 Laptop 8GB VRAM |

**สคริปต์พร้อมใช้** — `docs/comfyui-gen.py` (ส่งงานเข้า queue, รอจนเสร็จ, ย้ายไฟล์มา `docs/ai-images/` ให้อัตโนมัติ):

```bash
python docs/comfyui-gen.py docs/comfyui-jobs.sample.json
```

ไฟล์ jobs เป็น JSON list: `{"prefix": "nature-bg/misty-lake", "w": 1536, "h": 864, "seed": 101234, "prompt": "..."}`
(ดูตัวอย่าง prompt แนวธรรมชาติที่ใช้ได้ผลใน `docs/comfyui-jobs.sample.json`)

**โมเดลที่ติดตั้งไว้ — Z-Image Turbo เท่านั้น** (ไม่มี checkpoint ธรรมดา ต้องใช้ 3 ไฟล์แยก):
- `diffusion_models/z_image_turbo_bf16.safetensors` → `UNETLoader`
- `text_encoders/qwen_3_4b.safetensors` → `CLIPLoader` โดย **`type` ต้องเป็น `lumina2`** (ไม่มี type ชื่อ `z_image`)
- `vae/ae.safetensors` → `VAELoader`

**ค่า sampler ที่ถูกต้อง** (ลอกจาก template `image_z_image_turbo.json`): `ModelSamplingAuraFlow` shift=3 → `KSampler` steps=8, **cfg=1**, `res_multistep` / `simple`, denoise=1, latent = `EmptySD3LatentImage`
เพราะ cfg=1 negative prompt จึงไม่มีผล → ต่อ negative ด้วย `ConditioningZeroOut` ของ positive (อย่าเขียน negative prompt เปล่า ๆ แล้วคาดหวังผล)

- ความเร็วจริง: **~30 วิ/รูป** ที่ 1536×864 (รูปแรกช้ากว่าเพราะโหลดโมเดล) — ส่งเข้า queue ทีเดียวหลายรูปได้ ComfyUI ทำทีละงานแต่โมเดลค้างใน VRAM
- template ต้นฉบับของทุกโมเดลอยู่ที่ `C:\Users\ball\AppData\Local\Comfy-Desktop\ComfyUI-Installs\ComfyUI\ComfyUI\.venv\Lib\site-packages\comfyui_workflow_templates_json\templates\` — เวลาจะใช้โมเดลใหม่ ให้อ่าน template ตัวนั้นก่อน (บางไฟล์เก็บ node จริงไว้ใน `definitions.subgraphs` ไม่ใช่ `nodes`) แล้วค่อยแปลงเป็น API format
- ตรวจโมเดลที่มีจริง: `GET /models/{checkpoints,diffusion_models,vae,text_encoders,loras}`
