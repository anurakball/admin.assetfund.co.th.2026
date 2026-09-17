# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

> ⏩ **งาน CMS (เพิ่มเมนูซ้าย → DB → front-end → Preview) — session ใหม่อ่าน `docs/CMS-MENU-HANDOFF.md` ก่อน**
> เป็นเอกสารส่งต่องานฉบับเต็ม (อัปเดตล่าสุด 18 ก.ย. 2569 — **§14 = งานล่าสุด: page builder หน้าแรก `CMSPage` + ข้อมูล Widget, ผลทดสอบ 7 เมนู, เหตุการณ์กู้ไฟล์ `AdminMenu.cs`**): สถานะ 7 เมนู CMS + งาน Login/reCAPTCHA/Dashboard, ความต้องการผู้ใช้ทั้งหมด, ไทม์ไลน์, **สูตรทำเมนูครบวงจร**, **เทคนิค Playwright พร้อมโค้ด**, กับดัก, ไฟล์ค้าง commit, **แผนงานต่อไป**, งานตอน deploy · สคริปต์ DB ถาวรอยู่ `docs/sql/`
> ⚠ **ห้าม `git checkout --` / `git restore` / `git stash` ใน repo นี้** — มีงานค้าง commit ข้าม session (เคยทำหายแล้วต้องกู้จาก transcript 18 ก.ย. 2569)
> จากนั้นค่อยอ่าน `docs/cms-menu-playbook.md` (Playbook 19 ขั้น + สเปกรายเมนู) — ไฟล์นี้เหลือแค่กฎที่ต้องรู้ทุก session

> 📁 **ทุก path ในเอกสารนี้สัมพัทธ์กับโฟลเดอร์ที่ไฟล์นี้อยู่ (`core_admin/`)** — ที่เดียวกับ `.csproj` และโค้ดทั้งหมด
> repo root คือโฟลเดอร์แม่ (`admin.assetfund.co.th.2026/`) มีแค่ `core_admin.sln` + ไฟล์ตั้งค่า git/IDE

> 🗂 **กฎ (ผู้ใช้กำหนด): ไฟล์นี้โหลดอัตโนมัติเฉพาะโปรเจกต์ admin — โปรเจกต์ข้างเคียงมี CLAUDE.md ของตัวเองที่ไม่ได้โหลดเอง ต้องเปิดอ่านก่อนทำงาน**
> - งานใดที่เกี่ยวข้องกับ `D:\Projectssetfund.co.th.2026` (front-end) → **อ่าน `D:\Projectssetfund.co.th.2026\CLAUDE.md` ด้วยเสมอ**
> - งานใดที่เกี่ยวข้องกับ `D:\Projectssetfund.co.th.old` (เว็บเดิม / หลังบ้านเดิม) → **อ่าน `D:\Projectssetfund.co.th.old\CLAUDE.md` ด้วยเสมอ**
> (ไฟล์นี้ไม่ทำซ้ำเนื้อหาของสองไฟล์นั้น — ตารางด้านล่างบอกแค่ว่าแต่ละไฟล์มีอะไร)
>
> | โปรเจกต์ | CLAUDE.md | มีอะไรอยู่ในนั้น |
> |---|---|---|
> | **front-end** — เว็บสาธารณะตัวใหม่ (.NET 9 MVC, https://localhost:7310) | `D:\Project\assetfund.co.th.2026\CLAUDE.md` | สถานะเมนูที่ต่อ DB แล้ว · `Helpers/DBHelper.cs` + `PreviewMap.cs` · วิธีต่อเมนูถัดไปกับ DB · กฎ **commit ทันทีหลังแก้** · **ห้ามแก้ `wwwroot/css` ตรง ๆ** (compile จาก `scss/`) · ทดสอบใน browser 7 ขั้น · attribute routing · วิธีดึงงานจาก static web |
> | **เว็บเก่า / ระบบเดิม** — เว็บสาธารณะเดิม + หลังบ้านเดิม `assetplus/backoffice` (ASP.NET WebForms, .NET Framework 4.0, **อ่านอย่างเดียว ห้ามพัฒนาต่อ**) | `D:\Project\assetfund.co.th.old\CLAUDE.md` | **วิธีรัน IIS Express พอร์ต 8099 · วิธีเข้าหลังบ้านเดิม (URL, user/pass, IP allow-list, รหัสผ่านเป็น MD5, SQL ปลดล็อก/reset)** · สถาปัตยกรรม include + `main_class` · หน้าที่ต้องมี `?id=` · ไฟล์ legacy ที่ตายแล้ว · ช่องโหว่ที่รู้อยู่แล้ว |
>
> **ทางลัดเข้าหลังบ้านเดิม** (รายละเอียดเต็มอยู่ในไฟล์ข้างบน — พอร์ต 8099 **ไม่ได้รันค้างไว้** ต้องสั่งเองทุกครั้ง):
> ```bash
> "C:\Program Files\IIS Express\iisexpress.exe" /path:"d:\Project\assetfund.co.th.old" /port:8099 /clr:v4.0
> ```
> → `http://localhost:8099/assetplus/backoffice/Default.aspx` login `user` / `Asset@2026` (ช่อง `#user` / `#pass`, ปุ่ม submit เป็น `<input type="image">`) → เข้าหน้า Approve List `index2.aspx?a=1` พร้อมเมนูซ้าย
> เว็บสาธารณะเดิมเปิดที่ `http://localhost:8099/default.aspx` (ไม่ใช่ `index.aspx`) · ทั้งเว็บเดิมและหลังบ้านเดิมใช้ **DB `asset_plus_uat` ตัวเดียวกับระบบใหม่** (ตาราง `tb_*` — แก้ในหลังบ้านเดิมจะเห็นในเมนู `Ap*` ของระบบนี้ทันทีและกลับกัน)

## Commands

```bash
dotnet build                       # build
dotnet run                         # run (HTTP  http://localhost:5300)
dotnet run --launch-profile https  # run (HTTPS https://localhost:7300)
dotnet publish core_admin.csproj -c Release -o publish   # ของขึ้นเซิร์ฟเวอร์ (ดูหัวข้อ Deploy / Publish)
```

No test project exists in this solution.

## Git — ขั้นตอนสุดท้ายของทุกงานคือ commit + push ⚠ (ผู้ใช้สั่ง 17 ก.ย. 2569)

**ทำงานเสร็จ (build ผ่าน + ทดสอบใน browser แล้ว) ให้ `git commit` แล้ว `git push` ทันที ทุกโปรเจกต์ที่มีไฟล์ถูกแก้ในงานนั้น** — ไม่ต้องรอถาม
(กฎนี้แทนกฎเดิม "admin ไม่ commit จนกว่าผู้ใช้สั่ง" — ผู้ใช้ยกเลิกแล้ว)

| โปรเจกต์ | โฟลเดอร์ที่รัน git (= repo root) | ปลายทาง push (branch `master`) |
|---|---|---|
| admin (repo นี้) | `D:\Project\admin.assetfund.co.th.2026` (แม่ของ `core_admin/`) | `https://github.com/anurakball/admin.assetfund.co.th.2026.git` |
| front-end | `D:\Project\assetfund.co.th.2026` | `https://github.com/anurakball/assetfund.co.th.2026.git` |

- ทั้งสอง repo **ไม่มี remote ชื่อ `origin`** (ผู้ใช้ push ผ่าน TortoiseGit แบบ "Arbitrary URL") → push ด้วย URL ตรง ๆ:
  ```bash
  git push https://github.com/anurakball/admin.assetfund.co.th.2026.git master   # ใน D:\Project\admin.assetfund.co.th.2026
  git push https://github.com/anurakball/assetfund.co.th.2026.git master         # ใน D:\Project\assetfund.co.th.2026
  ```
  (ถ้าอยากพิมพ์สั้นลง ตั้ง `git remote add origin <URL>` ได้ — ยังไม่ได้ตั้งเพราะผู้ใช้ไม่ได้สั่ง)
- 1 งาน = 1 commit ข้อความบอกว่าแก้อะไร (ไม่ใช่ `...`) ลงท้าย `Co-Authored-By:` ตาม system reminder ของ session นั้น
- แก้แค่ฝั่งเดียวก็ commit + push แค่ฝั่งนั้น · **push ไม่ผ่าน (credential/เน็ต) ให้แจ้งผู้ใช้ทันที** อย่าเงียบ · ห้าม `--force`
- ยังคง **ห้าม `git checkout --` / `git restore` / `git stash`** ใน repo admin (เคยทำงานหาย — ดู handoff §8)
- `wwwroot/Files/` และ `docs/*` (ยกเว้น `CMS-MENU-HANDOFF.md`, `preview-spec.md`, `sql/*.sql`) ถูก gitignore — รูปที่อัปโหลด/สร้างใหม่**ไม่ไปกับ push** ต้องอัปโหลดขึ้นเซิร์ฟเวอร์เอง

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
  ⚠ ทำไว้ตอน front-end ยังเป็นเว็บ SAM — ใช้ดูว่าเมนูหลังบ้านแต่ละตัวป้อนอะไร ไม่ใช่ผังของ front-end ใหม่
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
> ไม่งั้นเมนูไม่ขึ้นและเปิดหน้าไม่ได้ · ตาราง `tb_*` ใช้ `AdminLegacyController` แทน (ดูหัวข้อ "เมนูที่พอร์ตมาจากหลังบ้านเดิม" + `docs/legacy-backoffice.md`)

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

#### สถานะเมนูด้านซ้าย: เมนู SAM ส่วนใหญ่ยังถูกซ่อน

ตั้งแต่ 2026-08-29 เมนูที่ยกมาจาก SAM ถูก **`//` comment ทีละบรรทัดใน `Menu()`** (ไม่ลบ controller/view/`ModuleConfig` — ทุกโมดูลยังเข้าทาง URL `/Admin/<Link>` ได้และเปิดคืนได้ทันที ทุกบรรทัดมี `,` ปิดท้ายแล้ว)
ที่เปิดอยู่: กลุ่ม "หน้าเว็บไซต์", "ข้อมูลหน้าแรก", 4 กลุ่มเมนู `tb_*` ของ Asset Plus, "ผู้ดูแลระบบ", "Widget" — **รายการจริงดูที่ `docs/backend-menu-status.html`** (อย่านับจากที่นี่)

> **กฎ: แก้เมนูด้านซ้ายเมื่อไร (เพิ่ม/แก้ชื่อ/ย้าย/ซ่อน/เปิด) ต้องอัปเดต `docs/backend-menu-status.html` ทุกครั้ง** — รายงาน static 2 ตาราง (เปิด / ปิด) ให้เจ้าของโปรเจกต์เปิดดู
> `docs/*` ถูก gitignore ทั้งโฟลเดอร์ ยกเว้นที่ un-ignore ใน `.gitignore` (`backend-menu-status.html`, `preview-spec.md`, `CMS-MENU-HANDOFF.md`, `cms-menu-playbook.md`, `legacy-backoffice.md`, `sql/*.sql`)


## งานเมนู CMS (หลังบ้าน → DB → front-end → Preview) — อ่านเอกสารแยกก่อนทำ

งานที่ผู้ใช้สั่งบ่อยที่สุดคือ "เพิ่มเมนูหลังบ้าน `<กลุ่ม> > <เมนู>` → front-end ดึงจาก DB → ปุ่ม Preview ใช้ได้ → ทดสอบละเอียด 2 ฝั่ง"
**ขั้นตอนเต็มไม่ได้อยู่ในไฟล์นี้** — เปิด 2 ไฟล์นี้ก่อนลงมือ:

| ไฟล์ | มีอะไร |
|---|---|
| `docs/CMS-MENU-HANDOFF.md` | **อ่านก่อนเสมอ** — สถานะ, ความต้องการผู้ใช้ทั้งหมด, สูตรทำเมนู (§6), เทคนิค Playwright (§7), กับดัก (§8), คำสั่ง/SQL (§9), เช็คลิสต์เปิด session (§10), แผนงาน (§12), deploy (§13) |
| `docs/cms-menu-playbook.md` | Playbook ฉบับเต็ม 19 ขั้น (Phase A หลังบ้าน / B front-end / C ทดสอบร่วม) + **สเปกระดับคอลัมน์ของทุกเมนูที่ทำแล้ว** (front-end ทำตามอะไร, gate, ฟิลด์ → ตำแหน่งบนหน้าเว็บ, กฎที่ผู้ใช้กำหนด) |

ภาพรวมที่ทุกขั้นยึด (ต้องรู้แม้ไม่ได้ทำเมนู):

```
[หลังบ้าน] ฟอร์ม Create/Edit ──บันทึก──▶ คอลัมน์ฉบับร่าง (title, img1, ...)   pb_status = 0 (ฟอร์ม Edit ล็อกจนกว่าจะ Approve)
           ปุ่ม Approve      ──copy──▶  คอลัมน์ pb_* (pb_title, pb_img1, ...) pb_status = 1, show_front = 1
           ปุ่ม Status / Move ──────▶  status / sort (ไม่มีคู่ pb_ → มีผลกับหน้าเว็บทันที)
[front-end] หน้าจริง  อ่าน pb_* + กรอง status/show_front/ช่วงวันที่ (ผ่าน preview.Gate() เท่านั้น)
            /_preview/page/<Module>/<id>?lang=  อ่านคอลัมน์ฉบับร่าง (alias เป็น pb_*) ไม่กรอง  ← ปุ่ม [Preview] เปิดใน iframe
```

- admin `https://localhost:7300` (.NET 10, repo นี้) · front-end `https://localhost:7310` (.NET 9, `d:\Project\assetfund.co.th.2026`, CLAUDE.md แยก) — **คนละ repo คนละพอร์ต** ทุกคำสั่ง `dotnet`/`git` รันในโฟลเดอร์ของโปรเจกต์นั้น
- ค่าเริ่มต้นที่ผู้ใช้กำหนด (ไม่ต้องถาม): สิทธิ์ให้ **Super Admin (`access_id = 1`) เท่านั้น** · ข้อมูลตัวอย่างเป็นของ **Asset Plus** (ห้ามเหลือเนื้อหา SAM) · ยึดโครง SAM ไม่รื้อ เปลี่ยนแค่สี/ข้อความ/ข้อมูล · cache front-end = 0 · Preview ต้องใช้ได้จริง · **หน้าพรีวิวห้ามมีแถบ/ป้ายสรุปใด ๆ** (ต้องเหมือนหน้าเว็บจริง) · ทดสอบ Playwright ทั้ง 2 ฝั่งก่อนรายงาน
- ฟอร์มหลังบ้าน: **ห้ามใส่ `<p class="cms-note">`** (ย่อหน้าอธิบายใต้ legend — ผู้ใช้สั่งตัดออกทั้งหมด 17 ก.ย. 2569 เพราะข้อความบนหน้าจอเยอะเกินไป) · ที่ใช้ได้ตามปกติแบบ SAM: `<fieldset class="cms-section">` + `<legend>`, คำใบ้สั้นสีส้มในป้ายชื่อช่อง `<span style="font-weight: normal;color: orange;">(…)</span>`, placeholder, ธงภาษา — คำอธิบายยาวเขียนในเอกสาร ไม่ใช่บนหน้าจอ
- แก้ `.cs` ต้องรีสตาร์ท server (ฆ่า `core_admin.exe` ก่อน build ไม่งั้น MSB3027) · แก้ `.cshtml` ไม่ต้อง
- แก้เมนูด้านซ้าย / `ModuleConfig` / ฟิลด์ / `Table` / `TableModuleID` เมื่อไร → ต้องตรวจผลกระทบต่อ Preview ของเมนูนั้น (ทะเบียน 2 ฝั่ง `PreviewMenu.cs` ↔ front-end `PreviewMap.cs` ต้อง sync เอง) และอัปเดต `docs/backend-menu-status.html`

### กับดักที่กัดบ่อยที่สุด (ฉบับเต็มอยู่ playbook ขั้น 11 + handoff §8)

- **SQL ดิบที่ใช้ `Module.Config.Table` ต้องครอบ `Db.T()`** (`web_core_item` ไม่มีจริง ชื่อจริง `[2026_web_core_item]`) — พังเงียบเพราะ `catch` คืนค่า default · `_db.Insert`/`_db.Update` เติม prefix ให้เอง
- **ห้ามใส่ฟิลด์ที่ฟอร์มไม่มีใน `FieldUpdate`** — บันทึกแล้วถูกเขียนทับเป็น NULL
- `AdminCoreController.Edit` **ไม่กรอง `module_id`** (เปิดแถวของ module อื่นได้ถ้ารู้ id) — ยังไม่แก้เพราะกระทบทุกเมนู `web_core_*`
- เมนู SAM บางตัวมี `Views/<Module>/Index.cshtml` สำเนาเก่าของตัวเอง (ไม่มีปุ่ม Preview) → ลบทิ้งให้ใช้ `AdminCore/Index.cshtml`
- `wwwroot/Files/` (รูปจาก elFinder / รูปที่สร้าง) **ไม่ไปกับ git และ publish** — ขึ้นเซิร์ฟเวอร์ต้องอัปโหลดเอง
- ชื่อ admin ที่แสดง (`created_by`) มาจาก `[2026_web_admin].name` ซึ่งยังเป็นของ SAM ("สยามอี ซีเอ็มเอส")

### เมนู CMS ที่ทำครบสายแล้ว (สเปกละเอียดอยู่ `docs/cms-menu-playbook.md` ท้ายไฟล์)

| เมนู (กลุ่มปัจจุบัน) | Module | ตาราง / module_id | front-end service |
|---|---|---|---|
| ข้อมูลหน้าแรก > จัดการ Widget (page builder หน้าแรก) | `CMSPage` | `web_cms_page` แถวเดียว id 1 + `web_widget_group` 3 / `web_widget` 18 (`section_key`) | `SqlHomeLayoutService` |
| ข้อมูลหน้าแรก > รูปสไลด์หน้าแรก | `HomeImageSlide` | `web_core_item` / 1 | `SqlHeroSlideService` |
| หน้าเว็บไซต์ > หน้า Intro Page | `HomeIntroPage` | `web_home_intro_page` (ตารางเดี่ยว) | `SqlIntroPageService` (`/intro-page`) |
| หน้าเว็บไซต์ > หน้า Pop-Up | `HomePopUp` | `web_home_pop_up` (ตารางเดี่ยว, จัดเรียงได้) | `SqlPopupBannerService` |
| หน้าเว็บไซต์ > ปรับแต่ง Header | `HomeHeader` | `web_home_header` แถวเดียว (+คอลัมน์ `en_img1` ที่เพิ่มเอง) | `SqlSiteHeaderService` |
| หน้าเว็บไซต์ > ปรับแต่ง Footer | `HomeFooter` | `web_home_footer` แถวเดียว (+12 คอลัมน์ที่เพิ่มเอง) | `SqlSiteFooterService` |
| หน้าเว็บไซต์ > SEO & Code | `HomeSEO` | `web_home_seo` แถวเดียว — **ไม่มีปุ่ม Preview** | `SqlSiteSeoService` |

ทุกเมนูข้างบน: front-end ต่อ DB แล้ว + Preview ใช้ได้ (ยกเว้น SEO ที่ถอดปุ่ม) · คอลัมน์ที่เพิ่มเองมี script ใน `docs/sql/` ที่**เซิร์ฟเวอร์จริงต้องรัน** · เมนูอื่นของ front-end ยังเป็น `Mock*Service`

## เมนูที่พอร์ตมาจากหลังบ้านเดิมของ Asset Plus (ตาราง `tb_*`)

หลังบ้านเดิมคือ ASP WebForms ที่ `http://localhost:8099/assetplus/backoffice/`
(ซอร์สอยู่ที่ `D:\Project\assetfund.co.th.old\assetplus\backoffice` — **วิธีรัน / login / reset รหัส ดู `D:\Project\assetfund.co.th.old\CLAUDE.md`**)
เมนู 20 ตัวถูกสร้างขึ้นใหม่ในระบบนี้ โดย **ใช้ตารางเดิมร่วมกัน ห้ามแก้โครงสร้างตาราง**

| กลุ่มเมนู | เมนู | Module | ตารางเดิม | โฟลเดอร์ต้นทาง |
|---|---|---|---|---|
| หน้าหลัก | Get Other Indices | `ApOtherIndices` | `tb_home_other_indices` | mod_tb_home_other_indices |
| ข้อมูลกองทุน | ประเภทกองทุนรวม | `ApFundCat` | `tb_fund_cat` | mod_tb_fund_cat |
| └ ↳ *(drill-down)* | รายชื่อกองทุน | `ApFund` | `tb_fund` | mod_tb_fund + mod_main_fund |
| └ ↳ *(drill-down)* | เอกสารกองทุน | `ApFundDoc` | `tb_fund_doc` | mod_tb_fund_doc + mod_main_fund_doc |
| ข้อมูลกองทุน | Get Fund Fact Sheet | `ApFundFactSheet` | `tb_fund_fundfact` | mod_tb_fund_fundfact |
| ข้อมูลกองทุน | Get NAV | `ApFundNav` | `tb_fund_nav` | mod_tb_fund_nav |
| ข้อมูลกองทุน | Delete NAV | `ApFundNavDelete` | `tb_fund_nav` | mod_tb_fund_nav_del |
| ข้อมูลกองทุน | Get Performance | `ApFundPerformance` | `tb_fund_performance` | mod_tb_fund_performance |
| กองทุนส่วนบุคคล | รู้จักกองทุนส่วนบุคคล | `ApPrivate` | `tb_fund_private` | mod_tb_fund_private |
| กองทุนส่วนบุคคล | ขั้นตอนการลงทุน | `ApPrivateProcess` | `tb_fund_private_investment_process` | mod_tb_fund_private_investment_process |
| กองทุนส่วนบุคคล | นโยบายการลงทุน | `ApPrivatePolicy` | `tb_fund_private_investment_policy` | mod_tb_fund_private_investment_policy |
| กองทุนส่วนบุคคล | คำถามที่พบบ่อย | `ApPrivateQanda` | `tb_fund_private_investment_qanda` | mod_tb_fund_private_investment_qanda |
| กองทุนส่วนบุคคล | ติดต่อเรา | `ApPrivateContact` | `tb_fund_private_contact_us` | mod_tb_fund_private_contact_us |
| กองทุนส่วนบุคคล | ติดต่อกองทุนส่วนบุคคล | `ApPrivateInterested` | `tb_fund_private_interested` | mod_tb_fund_private_interested |
| ปฏิทินกองทุน | หมวดหมู่ปฏิทิน | `ApCalendarCat` | `tb_calendar_category` | mod_tb_calendar_category |
| ปฏิทินกองทุน | ปฏิทินกองทุน | `ApCalendar` | `tb_calendar` | mod_tb_calendar |
| กองทุนสำรองเลี้ยงชีพ | เกี่ยวกับกองทุนสำรองฯ | `ApProv` | `tb_fund_prov` | mod_tb_fund_prov |
| กองทุนสำรองเลี้ยงชีพ | Factsheet (Group) | `ApProvSheetCat` | `tb_fund_prov_sheet_cat` | mod_tb_fund_prov_sheet_cat |
| กองทุนสำรองเลี้ยงชีพ | Factsheet | `ApProvSheet` | `tb_fund_prov_sheet` | mod_tb_fund_prov_sheet |
| กองทุนสำรองเลี้ยงชีพ | ข้อมูลอื่นๆ | `ApProvOther` | `tb_fund_prov_other` | mod_tb_fund_prov_other |

### กฎที่ต้องรักษาไว้ (ฉบับเต็ม + ws_schedule + drill-down อยู่ `docs/legacy-backoffice.md`)

- สคีมาต่างจาก `2026_web_*`: ไม่มี prefix (`Db.T()` ข้ามชื่อที่ขึ้นต้น `tb_` ให้เอง) · `lastcreate`/`lastupdate` เป็น **unix seconds**, `last_user`/`pb_last_user` แทน `created_by`/`approve_by` · บางตาราง `id` ไม่ใช่ IDENTITY (`LegacyIdManual`) · ที่เหมือนกัน: `sort/status/pb_status/show_front` + คู่ `pb_*` + ตรรกะ Approve
- เครื่องยนต์: `Controllers/AdminLegacyController.cs` (Index/Create/Edit/Delete/Status/Approve/Move ของ `tb_*`) · เมนู: `AssetPlusLegacyControllers.cs`, `AssetPlusImportControllers.cs` (Get …), `AssetPlusFundControllers.cs` (drill-down), `AssetPlusPrivateControllers.cs` · `WsScheduleController.cs` · `Helpers/AssetPlusImporter.cs` + `AssetPlusWsClient.cs` (SOAP) · **`ModuleConfig` ทั้ง 20 เมนูอยู่ `Helpers/AdminMenuAssetPlus.cs`** (`AllModule()` ต่อท้ายด้วย `.Concat`)
- เมนู `Ap*` ใช้ตารางเดิม**ร่วมกับหลังบ้านเดิม** (`assetplus/backoffice`) — **ห้ามแก้โครงสร้างตาราง `tb_*`** · Edit ตั้ง `pb_status = 0` + เขียนคิว `tb_admin_approve` · Approve copy `pb_<field> = <field>` แล้วลบคิว (Approve List ของหลังบ้านเดิมจึงตรงกัน)
- **เมนู "Get …" / ws_schedule** (`tb_fund_nav`, `tb_fund_performance`, `tb_home_other_indices`, `tb_fund_fundfact*`): แถวใหม่ `Flag = 1`, แถวเดิมของคีย์เดียวกัน `Flag = 0` (เก็บเป็นประวัติ ไม่ลบ), เผยแพร่ทันที (`status/pb_status/show_front = 1`) · ตรรกะอยู่ `Helpers/AssetPlusImporter.cs` ที่เดียว (ใช้ทั้งกดเองและ scheduler) · endpoint `https://<host>/ws_schedule/ws_get_{nav,other_indices,performance,fundfact}` ไม่ต้อง login (ตั้ง `WsSchedule:Key` กัน) · วันที่ที่ส่ง web service ต้องเป็น **ค.ศ.** (`InvariantCulture`)
- **ไฟล์อัปโหลดของเมนู `tb_*` เก็บเป็นชื่อไฟล์เปล่า** ลงโฟลเดอร์ของเว็บเดิม (`LegacyUpload:Path/Url`, เอกสารกองทุน `LegacyUploadDoc:*`) · ชื่อไฟล์เอกสารกองทุน `<fundcode>_<slug>[_en].<ext>` **ห้ามเปลี่ยน** (เว็บเดิมอ้างตรง ๆ)
- `ApFund` / `ApFundDoc` เป็น drill-down จาก `ApFundCat` (ไม่อยู่เมนูซ้าย) · เพิ่มกองทุน = สร้างเอกสารมาตรฐาน 18 แถว (`file_id` 1–18 ลบไม่ได้) · เปลี่ยน `fundcode` ต้องตามแก้ `tb_fund_doc`
- วันที่ในฟอร์มเป็น **พ.ศ.** (culture th-TH — `LegacyFields()` ต้องใช้ culture ปัจจุบัน) แต่ค้นหาช่วงวันที่ใน session เป็น ค.ศ. `yyyy-MM-dd` (ต้อง `InvariantCulture`) · `pb_status` ของตารางเดิมไม่ได้เป็น 0/1 เสมอ (`tb_fund_doc` มี 10/20/30) → "ไม่ใช่ 1 = ยังไม่อนุมัติ" · `sort` เป็น NULL ได้
- `tb_admin_log.action_info` เป็น `text` codepage 874 — **ห้ามใส่อักขระนอกโค้ดเพจไทย** (เช่น `·`)
- เมนูของหลังบ้านเดิมที่**ยังไม่พอร์ต**: หน้า Page (openaccount/aboutus/investment), คณะกรรมการ, ประกาศ, ข่าว/กิจกรรม/คอลัมภ์, ร่วมงานกับเรา, PDPA — ทะเบียนเมนูเดิมอยู่ `assetplus/backoffice/all_module.aspx` · จะพอร์ตเพิ่มให้เริ่มอ่าน `mod_<table>/mod_config.aspx` แล้ว `add.aspx`/`edit.aspx` (รายละเอียดใน `docs/legacy-backoffice.md`)


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
**ฝั่ง front-end ใช้งานได้แล้วเฉพาะเมนูที่ต่อ DB แล้ว** (`HomeImageSlide`, `HomeIntroPage`, `HomePopUp`, `HomeHeader`, `HomeFooter`, `CMSPage` (ลำดับ widget หน้าแรกฉบับร่าง — โหมด `page` ไม่ใช่ `cms`) — `HomeSEO` ถูกถอดปุ่ม Preview ออกแล้ว) — มี `Helpers/PreviewMap.cs`
+ route `/_preview/page/{module}/{id}` ใน `Controllers/HomeController.cs` ของ `d:\Project\assetfund.co.th.2026`
เมนูอื่นในทะเบียน `PreviewMenu.cs` ยังกดได้แต่ iframe จะขึ้น 404 ของ front-end จนกว่าจะต่อ DB + เพิ่มใน `PreviewMap.Pages`

- **ห้ามมีแถบ/ป้าย/กล่องสรุปฉบับร่างใด ๆ บนหน้าพรีวิว (ผู้ใช้สั่งลบ 17 ก.ย. 2569 — ดูแล้วเข้าใจว่าหน้าเว็บแสดงผลผิด) พรีวิวต้องหน้าตาเหมือนหน้าเว็บจริงทุกอย่าง** ต่างแค่ข้อมูลของเมนูที่พรีวิวเป็นฉบับร่าง (ตรวจได้ด้วยการ diff `<body>` ของหน้าพรีวิวกับหน้าจริงตอนฉบับร่าง = ฉบับอนุมัติ ต้องได้ 0 บรรทัด) · ข้อยกเว้นเดียวที่ยังมี: popup ประกาศไม่เด้งตอนพรีวิวเมนูอื่นของหน้าแรก (กันบังสิ่งที่กำลังดู)
- **ทดสอบพรีวิวต้องรัน front-end แบบ https (`https://localhost:7310`)** — admin เป็น https ถ้า iframe ไป http จะโดน mixed content block
- front-end ตอบพรีวิวพร้อม `Content-Security-Policy: frame-ancestors 'self' <AdminURL>` → ถ้า `AdminURL` ใน appsettings ของ front-end
  ไม่ตรงกับโดเมน admin ที่เปิดอยู่ iframe จะว่างเปล่า (dev = `https://localhost:7300`)

> **กฎ: แก้ back-end ของเมนูใด — ฟิลด์ / `ModuleConfig` / `ListData` / `Table` / `TableModuleID` / ชื่อโมดูล —
> ต้องตรวจผลกระทบต่อ Preview ของเมนูนั้นด้วยเสมอ** (พังหรือแสดงค่าผิดโดยไม่มีใครรู้)
> ทะเบียน 2 ฝั่งเป็นคนละโปรเจกต์ ต้อง sync กันเอง: `PreviewMenu.cs` ↔ `PreviewMap.cs`

📄 **สเปกเต็ม** (3 โหมด item/page/cms, กฎระดับแถว, เช็คลิสต์เมื่อแก้ back-end, หลักการที่ห้ามพัง, วิธีทดสอบ)
→ `docs/preview-spec.md`

### Frontend

Razor + jQuery + CKEditor + elFinder · ไม่มี npm/bundler (lib commit ตรง ๆ ใน `wwwroot/lib/`)
UI ที่ใช้ซ้ำ (breadcrumb, pagination, ปุ่ม action) เป็น view component ที่ `Areas/Admin/Views/ViewComponents/`

## Deploy / Publish

```bash
dotnet publish core_admin.csproj -c Release -o publish   # → โฟลเดอร์ publish/ (ของที่เอาขึ้นเซิร์ฟเวอร์)
```

หรือกด Publish ใน Visual Studio ด้วย **`Properties/PublishProfiles/FolderProfile.pubxml`**
(profile เดียวที่มี — ปลายทาง `publish\`, `DeleteExistingFiles=true`)

> ⚠ **อย่าเก็บไฟล์อื่นไว้ใน `publish/`** เพราะถูกล้างทุกครั้งที่ publish · โฟลเดอร์นี้ gitignore แล้ว
> ⚠ ระบุ `core_admin.csproj` ให้ชัดเจน — ในโฟลเดอร์นี้มี `core_admin.sln` อีกตัว (ซ้ำกับที่ repo root
> แต่คนละ GUID) ถ้าไม่ระบุ `dotnet` จะเลือก .sln ตัวใน

### สิ่งที่ publish แล้ว "ไปด้วย" และ "ไม่ไป"

กฎอยู่ที่ `<Content Update .../>` ใน `core_admin.csproj` — **แก้ที่นั่นที่เดียว** และมีคอมเมนต์กำกับทุกข้อ

| ไป | ไม่ไป (ตั้งใจ) |
|---|---|
| assembly + `wwwroot/` (css/js/โลโก้/lib) | `appsettings*.json` และ `.json` อื่นที่ root |
| `Storage/` (เทมเพลตอีเมล/ฟอนต์ ที่โค้ดอ่านตอนรัน) | `wwwroot/Files/`, `wwwroot/uploads/`, `wwwroot/images/bg/` (ของที่ผู้ใช้อัปโหลด) |
| `web.config` (pipeline เติม handler ให้เอง) | `wwwroot/scss/` (ซอร์สของ css), `Logs/`, `App_Data/`, `docs/`, `.claude/` |

- **`wwwroot/` ต้องไปด้วย** เพราะธีม/โลโก้ของ Asset Plus อยู่ในนั้น (`?v=ap2026` — ดูหัวข้อ Branding)
  แต่ 3 โฟลเดอร์อัปโหลดถูกกันไว้ ไม่งั้น deploy จะเอาของบนเครื่อง dev ไปทับไฟล์จริงบนเซิร์ฟเวอร์
  **รายการนี้ต้องตรงกับ `.gitignore`** — แก้ที่ไหนต้องตามไปแก้อีกที่
- `Microsoft.VisualStudio.Web.CodeGeneration.Design` ตั้ง `PrivateAssets="all"` ไว้ (มันเป็นแค่ตัว scaffold)
  ห้ามถอดออก ไม่งั้น Roslyn Workspaces/Features + MSBuild + NuGet.* + EntityFrameworkCore
  จะกลับมาอยู่ใน publish อีก ~60 MB ทั้งที่ไม่มีโค้ดไหนเรียก
- `DefaultItemExcludes` กัน `publish/` + `node_modules/` ไม่ให้ถูก glob กลับเข้าไปใน publish รอบถัดไป
  (Web SDK กวาด `**/*.json` / `**/*.config` เป็น Content เอง — bin/obj มันกันให้แล้ว แต่ `publish/` ไม่)

### ต้องตั้งบนเซิร์ฟเวอร์เอง (ไม่ได้ไปกับ publish)

1. **`appsettings.json`** — ไฟล์ในรีโปเก็บแต่โครง ช่องรหัสผ่านเว้นว่างไว้หมด **ค่าจริงอยู่บนเซิร์ฟเวอร์เท่านั้น**
   ต้องมีไฟล์นี้ ไม่งั้นแอปต่อ DB ไม่ได้ · คีย์ที่ต้องกรอก:
   `DBConnection` · `MySQLConnection` · `AppKey` · `API:*` · `MailSettings:*` · `RootURL` / `FrontURL` ·
   `LegacyUpload:Path` / `:Url` · `LegacyUploadDoc:Path` / `:Url` · `WsSchedule:XmlPath` / `:Key` ·
   **`GoogleReCaptcha:SiteKey` / `:SecretKey` (คีย์จริงของโดเมน — ไม่มี = login ไม่ได้, คีย์ทดสอบ = ไม่กันบอท)** ·
   `AssetPlusWS:URL` · `LINEAPI:*`
2. **`ASPNETCORE_ENVIRONMENT` ห้ามเป็น `Development`** — ไม่งั้น Serilog เปิด console sink,
   หน้า error กลายเป็น developer exception page และถ้า `EnableSqlDebugLog` เป็น `true` ด้วย
   `DBHelper` จะพ่น SQL ทุกคำสั่งออก log (ต้องเข้าเงื่อนทั้งสองถึงจะพ่น)
3. **สิทธิ์เขียนของ app pool** : `Logs/` (Serilog), `App_Data/ws_schedule/` (XML ที่ ws_schedule ดึงมา),
   `wwwroot/Files/` (elFinder) และโฟลเดอร์ตาม `LegacyUpload:Path` / `LegacyUploadDoc:Path` (อยู่ในเว็บเดิม)
4. **`web.config`** ตั้ง `maxAllowedContentLength` = 80 MB ให้ตรงกับ `MaxRequestBodySize` ใน `Program.cs`
   (ค่า default ของ IIS คือ 30 MB จะตัดไฟล์ทิ้งก่อนถึงเพดาน 50 MB ของ elFinder)
5. **ws_schedule** ต้องตั้ง scheduler ยิงมาที่ `https://<host>/ws_schedule/...` เอง (ดูหัวข้อ ws_schedule)
   และควรตั้ง `WsSchedule:Key` ด้วย ไม่งั้นใครก็ยิงได้

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

- ปกติเซิร์ฟเวอร์รันค้างอยู่แล้วที่ `https://localhost:7300/` — **ไม่ต้อง `dotnet run` ใหม่ถ้าเข้าได้**
- ถ้าเข้าไม่ได้ → `dotnet run --launch-profile https` แล้ว **รอจนพอร์ต 7300 listen** (ใช้เวลาสักครู่) ค่อยทดสอบ
- ⚠ **พอร์ต 7140/5140 ไม่ใช่ของโปรเจกต์นี้แล้ว** (ย้ายมา 7300/5300 เมื่อ 14 ก.ย. 2569) — admin อีก 4 โปรเจกต์บนเครื่องนี้
  (admin.egth, admin.sam.or.th, admin.sm, adminweb.thaicreditbank.com) ยังใช้ 7140 อยู่ ถ้าเจออะไรรันที่ 7140 **อย่าคิดว่าเป็นตัวนี้**
- ทดสอบด้วย **Playwright** และ **ต้อง login ก่อนเสมอ** ด้วย `user` / `P@ssw0rd`
- ลำดับการเข้าหลังบ้าน: `https://localhost:7300/` → redirect ไป `/Admin/User/Login?webID=&targetUrl=/Admin`
  → กรอก user/pass โดยปล่อย dropdown ไว้ที่ **เว็บไซต์หลัก** (webID = 0) → `/Admin/User/Dashboard`
  → กดปุ่ม **Admin Panel** → `/Admin/User/LastActivity` คือหน้าหลังบ้าน
- เก็บ screenshot ไว้ใน `.playwright-mcp/` เสมอ (git ignore แล้ว เป็นไฟล์ชั่วคราว ลบทิ้งได้)

### รัน server ให้อยู่รอดหลังปิด Claude Code
`dotnet run` ที่สั่งผ่าน background task ของ Claude Code จะ**ถูก kill เมื่อ session จบ** ถ้าอยากให้รันค้าง ให้ spawn แบบหลุด job object ด้วย WMI:

```powershell
$log = "$env:TEMP\core_admin-7300.log"
$cmd = 'cmd /c "dotnet run --launch-profile https > "' + $log + '" 2>&1"'
Invoke-CimMethod -ClassName Win32_Process -MethodName Create -Arguments @{CommandLine=$cmd; CurrentDirectory="d:\Project\admin.assetfund.co.th.2026\core_admin"}
```
หยุดด้วย `Get-NetTCPConnection -LocalPort 7300 | Select -Expand OwningProcess | Stop-Process -Force` (หรือรันจาก terminal แยกเองก็ได้)

### กลไก login (`Areas/Admin/Controllers/UserController.cs`)

- **หน้า Login (`Views/Login/Index.cshtml`) ไม่มี dropdown เลือกเว็บไซต์แล้ว** (ผู้ใช้สั่งซ่อน 17 ก.ย. 2569) — ส่ง `<input type="hidden" name="web_id" value="0">` แทน
- **Google reCAPTCHA v2 ("I'm not a robot")** ใต้ช่องรหัสผ่าน · ตรวจฝั่ง server ที่ `Areas/Admin/Helpers/ReCaptcha.cs` **ก่อนเช็ก username/password ทุกครั้ง**
  ใน `Login [HttpPost]` — ไม่มี token / token ไม่ผ่าน / ติดต่อ Google ไม่ได้ / ไม่ได้ตั้งคีย์ = ไม่ให้เข้า (fail closed) และเขียน Admin Log `login_fail_captcha`
  - config: `appsettings → GoogleReCaptcha:SiteKey` / `:SecretKey` / `:VerifyUrl` (มีใน `appsettings.json`, `appsettings.Development.json` และ `.example`)
  - ⚠ **ตอนนี้ทุกไฟล์ใส่ "คีย์ทดสอบของ Google"** (`6LeIxAcTAAAAAJcZVRqyHh71UMIEGNQ_MXjiZKhI` / `6LeIxAcTAAAAAGG-vFI1TnRWxMZNFuojJ4WifJWe`) —
    ใช้บน localhost ได้ แต่กล่องขึ้นข้อความแดง "for testing purposes only" และ **Google ตอบผ่านทุก token (รวม token ปลอม)** จึงยังกันบอทไม่ได้จริง
    → **ขึ้นเซิร์ฟเวอร์ต้องสมัครคีย์ของโดเมนจริง** ที่ https://www.google.com/recaptcha/admin (แบบ v2 Checkbox) แล้วแทนที่ทั้ง 2 ค่า · แนะนำแยกคีย์ dev (อนุญาต localhost) กับ production
  - **modal "คุณหลุดออกจากระบบ กรุณา Login อีกครั้ง"** (`_PartialAdminMenu.cshtml`) POST ไป endpoint เดียวกัน จึงมีกล่อง reCAPTCHA ด้วย —
    โหลดสคริปต์ Google ตอน modal เด้งครั้งแรกเท่านั้น (`ensureReloginCaptcha()`, render แบบ explicit) · server ตอบ `captcha` เมื่อไม่ผ่าน · ทุกผลลัพธ์ reset กล่อง (token ใช้ครั้งเดียว)
  - ทดสอบ Playwright: ติ๊กด้วย `page.frameLocator('iframe[title="reCAPTCHA"]').locator('#recaptcha-anchor').click()` แล้วรอ `[aria-checked="true"]` (ใน modal ใช้ `#relogin_recaptcha iframe[title="reCAPTCHA"]` — มี iframe เปล่าอีกตัว ห้ามใช้ `iframe` เฉย ๆ)
- **หน้า Dashboard มีแถบชื่อผู้ใช้ + ปุ่ม Logout มุมขวาบน** (`Views/User/Dashboard.cshtml` — layout `_LayoutIntro` ไม่มี header ของหลังบ้าน) · POST `/Admin/User/Logout` + anti-forgery token + SweetAlert ยืนยันเหมือน `logOut()` ของหน้าอื่น

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

## เว็บไซต์ front-end (เว็บสาธารณะ) — คนละแอป คนละ repo

| แอป | โปรเจกต์ | URL (dev) |
|---|---|---|
| Admin (repo นี้) | `d:\Project\admin.assetfund.co.th.2026\core_admin` | https://localhost:7300 |
| Public site | `d:\Project\assetfund.co.th.2026` (.NET 9, attribute routing, **CLAUDE.md แยก — อ่านก่อนแตะ**) | https://localhost:7310 (ผู้ใช้เปิดของตัวเองที่ http://localhost:5310 — **ห้ามฆ่า**) |

```
admin (core_admin) ──เขียน──▶ SQL Server asset_plus_uat ◀──อ่าน── front-end   ([2026_web_*] + tb_*)
```
**DB schema และตรรกะ Approve คือ "สัญญา" ที่ front-end พึ่งอยู่** — เปลี่ยนชื่อคอลัมน์/ตาราง/`module_id`/วิธีตั้ง `pb_*` `status` `show_front` `Flag` เมื่อไร ต้องตามไปแก้ query ฝั่ง front-end (`Services/Sql*Service.cs`) ด้วย
เมนูที่ต่อแล้วดูตาราง "เมนู CMS ที่ทำครบสายแล้ว" ด้านบน · ที่เหลือยัง `Mock*Service` (แผนอยู่ handoff §12.2)

**ข้อตกลงระหว่างสองแอปที่ต้องรักษาไว้** (เขียนไว้ทั้งสองฝั่ง — แก้แล้วต้องตามไปแก้อีกฝั่ง):
- **front-end อ่านค่าที่อนุมัติแล้ว (`pb_*`) เท่านั้น** (ตรงกับเว็บเดิม) ยกเว้นระบบ Preview ที่ตั้งใจอ่านฉบับร่าง · ตารางที่ "Get …"/ws_schedule เขียนต้องกรอง **`Flag = 1`**
- **รูป/ไฟล์อยู่ฝั่ง admin** (`wwwroot/Files/` ของสองแอปแยกกัน): DB เก็บ `Files/Site0/1/...` (ไม่มี `/` นำหน้า) front-end ต่อ `AdminURL + "/" + path` · รูปใน CKEditor ต้องเป็น URL สัมบูรณ์ชี้โดเมน admin (elFinder ใส่ให้แล้ว) · ไฟล์ของเมนู `tb_*` ต่อจาก `LegacyUpload(Doc):Url`
- **ห้ามใช้ชื่อ session cookie ซ้ำกัน** (อยู่ `localhost` เดียวกัน cookie ไม่แยก port): admin = `AssetPlus.Admin.Session` · front-end ใช้ `AssetPlus.IntroSeen`, `hide_announcement` (ยังไม่เปิด session)
- ปุ่ม [Insert E-Form] แทรก `{{{E-Form:<หัวข้อ>:<id>}}}` — front-end ยังไม่มี parser
- ทะเบียน Preview ต้องคู่กัน: `Areas/Admin/Helpers/PreviewMenu.cs` ↔ front-end `Helpers/PreviewMap.cs` · front-end ตอบพรีวิวพร้อม CSP `frame-ancestors 'self' <AdminURL>` — `AdminURL` ใน appsettings ของ front-end ต้องตรงโดเมน admin ไม่งั้น iframe ว่าง · **ต้องรัน front-end แบบ https** (admin เป็น https → iframe http โดน mixed content)
- ⚠ `/home/preview` ของ front-end **ไม่ใช่** ระบบ Preview ของ admin (เป็นหน้าเทียบดีไซน์) · ข้อมูลเก่าใน `2026_web_cms_page.info` / `2026_web_core_news` (3 แถว) ยังมี URL รูป `https://localhost:7140/...` (พอร์ตเดิม) ต้องแก้ข้อมูลก่อนใช้

> 📌 อย่าสับสนกับ `d:\Project\sam.or.th` / `d:\Project\admin.sam.or.th` — ลูกค้าคนละราย โค้ด admin ชุดนี้พอร์ตมาจากที่นั่น เปิดดูเฉพาะตอนอยากเทียบว่าของเดิมทำอย่างไร
