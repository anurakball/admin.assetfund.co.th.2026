# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

> ⏩ **งาน CMS (เพิ่มเมนูซ้าย → DB → front-end → Preview) — session ใหม่อ่าน `docs/CMS-MENU-HANDOFF.md` ก่อน**
> เป็นเอกสารส่งต่องานฉบับเต็ม (อัปเดตล่าสุด 18 ก.ย. 2569 — **§14 = งานล่าสุด: page builder หน้าแรก `CMSPage` + ข้อมูล Widget, ผลทดสอบ 7 เมนู, เหตุการณ์กู้ไฟล์ `AdminMenu.cs`**): สถานะ 7 เมนู CMS + งาน Login/reCAPTCHA/Dashboard, ความต้องการผู้ใช้ทั้งหมด, ไทม์ไลน์, **สูตรทำเมนูครบวงจร**, **เทคนิค Playwright พร้อมโค้ด**, กับดัก, ไฟล์ค้าง commit, **แผนงานต่อไป**, งานตอน deploy · สคริปต์ DB ถาวรอยู่ `docs/sql/`
> ⚠ **ห้าม `git checkout --` / `git restore` / `git stash` ใน repo นี้** — มีงานค้าง commit ข้าม session (เคยทำหายแล้วต้องกู้จาก transcript 18 ก.ย. 2569)
> จากนั้นค่อยอ่านหัวข้อ "Playbook: ทำ 1 เมนู CMS ให้ครบวงจร" ในไฟล์นี้

> 📁 **ทุก path ในเอกสารนี้สัมพัทธ์กับโฟลเดอร์ที่ไฟล์นี้อยู่ (`core_admin/`)** — ที่เดียวกับ `.csproj` และโค้ดทั้งหมด
> repo root คือโฟลเดอร์แม่ (`admin.assetfund.co.th.2026/`) มีแค่ `core_admin.sln` + ไฟล์ตั้งค่า git/IDE

> 🗂 **โปรเจกต์ข้างเคียงใน workspace มี CLAUDE.md แยกของตัวเอง — จะแตะโปรเจกต์ไหนให้อ่านไฟล์ของโปรเจกต์นั้นก่อนเสมอ** (ไฟล์นี้ไม่ทำซ้ำเนื้อหา)
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

#### ⚠ สถานะเมนูด้านซ้ายตอนนี้ : เมนูของ SAM ส่วนใหญ่ยังถูกซ่อน

ตั้งแต่ 2026-08-29 เมนูเดิมที่ยกมาจาก SAM **ถูกซ่อนไว้เกือบทั้งหมด** ที่เปิดอยู่คือกลุ่ม `ผู้ดูแลระบบ`,
เมนู `tb_*` ที่พอร์ตจากหลังบ้านเดิมของ Asset Plus และเมนู SAM ที่ถูกเปิดคืนทีละตัว
(ณ 17 ก.ย. 2569 บ่าย กลุ่ม **"หน้าเว็บไซต์"** เปิด 6 เมนู: หน้า Intro Page, หน้า Pop-Up, ปรับแต่ง Header, ปรับแต่ง Footer, SEO & Code, Get Other Indices · กลุ่ม **"ข้อมูลหน้าแรก"** (ถัดจากหน้าเว็บไซต์) เปิด 2 เมนู: **จัดการ Widget (CMSPage = page builder หน้าแรก — เมนูแรกของกลุ่ม ย้ายมาจาก "หน้าเว็บไซต์" และเปลี่ยนชื่อจาก "จัดการเมนูเว็บไซต์" ตามคำสั่งผู้ใช้)**, รูปสไลด์หน้าแรก — กลุ่ม "หน้าหลัก" ถูกยุบแล้ว · กลุ่ม **"Widget"** (ถัดจาก "ผู้ดูแลระบบ") เปิด 2 เมนู: กลุ่ม Widget, Widget ทั้งหมด (ข้อมูล widget ของ page builder)
วิธีทำดู "Playbook: ทำ 1 เมนู CMS ให้ครบวงจร" ด้านล่าง)
รายการจริงดูที่ `docs/backend-menu-status.html`

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

## Playbook: ทำ 1 เมนู CMS ให้ครบวงจร (หลังบ้าน → DB → front-end → Preview)

เขียนจากงานจริง "หน้าหลัก > รูปสไลด์หน้าแรก" (`HomeImageSlide`) ที่ทำสำเร็จครบทั้งสายเมื่อ 16 ก.ย. 2569:
เปิดคืนเมนู SAM ที่ถูกซ่อน → ใส่ข้อมูล Asset Plus → ทดสอบหลังบ้าน → front-end อ่าน DB → ปุ่ม Preview ใช้ได้จริง
**session ใหม่ที่อ่านหัวข้อนี้ต้องทำเมนูถัดไปได้เองทั้งสาย** — ทำครบทุกขั้นก่อนรายงานว่าเสร็จ

```
Phase A  หลังบ้าน (repo นี้)             ขั้น 0–11   ผู้ใช้สั่งว่า "เพิ่มเมนูหลังบ้าน <กลุ่ม> > <เมนู>"
Phase B  front-end (assetfund.co.th.2026) ขั้น 12–17  ผู้ใช้สั่งว่า "front-end ดึง <เมนู> จาก DB (+ preview)"
Phase C  ทดสอบปลายทางร่วม 2 ฝั่ง + ส่งมอบ ขั้น 18–19
```

ภาพรวมสถาปัตยกรรมที่ทุกขั้นยึด:

```
[หลังบ้าน] ฟอร์ม Create/Edit ──บันทึก──▶ คอลัมน์ฉบับร่าง (title, img1, ...)   pb_status = 0
           ปุ่ม Approve      ──copy──▶  คอลัมน์ pb_* (pb_title, pb_img1, ...) pb_status = 1, show_front = 1
[front-end] หน้าจริง  อ่าน pb_* + กรอง status/show_front/ช่วงวันที่
            /_preview/page/<Module>/<id>  อ่านคอลัมน์ฉบับร่าง (alias เป็น pb_*) ไม่กรองอะไร  ← ปุ่ม [Preview] ในหลังบ้านเปิดใน iframe
```

ทั้งสองโปรเจกต์อยู่ใน VS Code workspace เดียวกันแต่**คนละ git repo คนละพอร์ต** — admin `https://localhost:7300` (.NET 10) · front-end `https://localhost:7310` (.NET 9)
ทุกคำสั่ง `dotnet` / `git` ต้องรันในโฟลเดอร์ของโปรเจกต์นั้น

### Phase A — หลังบ้าน

### 0. แยกประเภทงานก่อน

| ประเภท | ตัวอย่าง | เครื่องยนต์ |
|---|---|---|
| **A. เปิดคืนเมนู SAM ที่ซ่อนอยู่** (พบบ่อยสุด) | `HomeImageSlide` | `AdminCoreController` + ตาราง `2026_web_*` — ของเกือบครบแล้ว **ใช้ของเดิม 100% ห้ามรื้อโครง** |
| B. เมนูใหม่บนเครื่องยนต์ `web_core_*` | การ์ด/หมวด/บทความชุดใหม่ | เหมือน A แต่ต้องสร้าง config/controller/view และเลือก `module_id` ใหม่ |
| C. เมนูบนตารางเดิม `tb_*` | `ApFundCat` | `AdminLegacyController` — ดูหัวข้อ "เมนูที่พอร์ตมาจากหลังบ้านเดิม" ด้านล่าง |

**ค่าเริ่มต้นที่ผู้ใช้กำหนด**: สิทธิ์ให้ **Super Admin (`access_id = 1`) อย่างเดียว** · ข้อมูลตัวอย่างต้องเป็นของ **Asset Plus** (ห้ามเหลือเนื้อหา SAM)
โดยยกข้อความ + รูปจริงจาก front-end `d:\Project\assetfund.co.th.2026` (ดูจาก `Controllers/*` / `Services/Mock*Service.cs`)

### 1. สำรวจของที่มีอยู่ (ประเภท A)

```bash
grep -rn "<Module>" --include=*.cs --include=*.cshtml --include=*.js Areas wwwroot/js   # ทุกจุดที่อ้างถึง
diff --strip-trailing-cr ../../admin.sam.or.th/core_admin/Areas/Admin/Views/<Module>/Edit.cshtml Areas/Admin/Views/<Module>/Edit.cshtml
```
จุดที่ต้องเจอ: บรรทัดเมนูที่ถูก `//` ใน `Menu()` · `new Module(){ Name = "<Module>" ...}` ใน `AllModule()` ·
`Controllers/<Module>Controller.cs` · `Views/<Module>/Create|Edit.cshtml` · กิ่งพิเศษใน `Views/AdminCore/Index.cshtml`
(เช่นปุ่ม Pin ของ `HomeImageSlide`) · ทะเบียน `PreviewMenu.cs` · ตารางและแถวสิทธิ์ใน DB
ถ้าอยากรู้ว่า front-end ของ SAM อ่านเมนูนี้อย่างไร (เงื่อนไขแสดงผล / ลำดับ) ดู `d:\Project\sam.or.th\Controllers\HomeController.cs` + `Helpers\PreviewMap.cs`

### 2. เมนูด้านซ้าย — `Areas/Admin/Helpers/AdminMenu.cs` → `Menu()`

```csharp
new() { Title = "รูปสไลด์หน้าแรก", ModuleName = "HomeImageSlide", Link = "HomeImageSlide", Icon = "fa-regular fa-images" },
```
- `ModuleName` = `Name` ใน `AllModule()` = ชื่อ controller (ไม่มีคำว่า Controller) · `Link` คือ path ต่อจาก `/Admin/`
- ใส่ใน `SubMenu` ของกลุ่มที่ผู้ใช้ระบุ — กลุ่มใหม่ใช้ `listMenu.Add(new() { Title, Icon, SubMenu = new() { ... } })` ครอบด้วย `#region`
- **บรรทัดเดิมที่ถูก `//` ในกลุ่ม SAM ให้แทนด้วย comment ชี้ทางว่าย้ายไปไหน** อย่าเปิดซ้ำ 2 ที่ (เมนูเดียวกันจะขึ้น 2 กลุ่ม)
- Icon ใช้ Font Awesome 6 (`fa-solid` / `fa-regular`)
- หน้าจัดการสิทธิ์ (`AdminAccess/Edit`) สร้างรายการโมดูลจาก `Menu()` — **เมนูที่ไม่อยู่ใน `Menu()` จะตั้งสิทธิ์ผ่าน UI ไม่ได้**
  (ยกเว้นเมนู drill-down ที่ต้อง insert สิทธิ์ด้วย SQL)

### 3. Module config — `AllModule()` ใน `AdminMenu.cs`

```csharp
new Module() { Name = "HomeImageSlide", Config = new Module.ModuleConfig() {
    Text = "รูปสไลด์หน้าแรก", TextBreadcrumb = "หน้าหลัก/รูปสไลด์หน้าแรก",
    Table = "web_core_item", TableModuleID = 1, OrderBy = "sort", Sort = "asc",
    CanAdd = true, CanEdit = true, CanDelete = true, CanMove = true, CanStatus = true, CanApprove = true,
    UseViewCreateFrom = "HomeImageSlide", UseViewEditFrom = "HomeImageSlide",
    FieldSearch = FieldSearch_Default, ListData = ListData_DefaultImg,
    FieldCreate = FieldCreate_CoreItem, FieldUpdate = FieldUpdate_CoreItem, FieldApprove = FieldApprove_CoreItem } },
```
- **`TextBreadcrumb` ต้องเป็น `<กลุ่มใหม่>/<ชื่อเมนู>`** — ใช้ทั้ง breadcrumb และข้อความใน Admin Log (`action_info`) — เมนู SAM ที่เปิดคืนมักยังเป็นชื่อกลุ่มเดิม ต้องแก้
- `Table` ชื่อตรรกะไม่มี prefix · `TableModuleID` แยกเมนูที่ใช้ตาราง `web_core_*` ร่วมกัน
  (เมนูใหม่หาเลขว่างด้วย `select module_id, count(*) from [2026_web_core_item] group by module_id`)
- `ListData` คอลัมน์แรก = คอลัมน์ที่มีปุ่ม Preview · ชุด field มาตรฐานอยู่ท้าย `AdminMenu.cs` (`FieldCreate_CoreItem` ฯลฯ)
- ทุก `name` ของ input ในฟอร์มต้องอยู่ใน `FieldCreate` / `FieldUpdate` / `FieldApprove` — และ**ห้ามใส่ฟิลด์ที่ฟอร์มไม่มี**ใน `FieldUpdate`
  (บันทึกแล้วจะถูกเขียนทับเป็น NULL — ดูตัวอย่าง `EnableIssueDate = false` ใน `AdminMenu.cs`)
- `Can*` ต้องสอดคล้องกับแถวสิทธิ์ใน DB (ขั้น 6) ปุ่มจึงจะขึ้น

### 4. Controller — `Areas/Admin/Controllers/<Module>Controller.cs`

```csharp
public class HomeImageSlideController : AdminCoreController {
    public HomeImageSlideController(IWebHostEnvironment h, IConfiguration c, IHttpContextAccessor x) : base(h, c, x)
    { Module = _admin.GetModule("HomeImageSlide"); } }
```
Index / Create / Edit / Delete / Status / Approve / Move / Export มาจาก `AdminCoreController` ทั้งหมด — override เฉพาะที่พฤติกรรมต่างจริง

### 5. View — `Areas/Admin/Views/<Module>/Create.cshtml` + `Edit.cshtml`

- หน้า list **ไม่มีไฟล์ของตัวเอง** ใช้ `Views/AdminCore/Index.cshtml` ร่วมกัน (กิ่งพิเศษต่อเมนูเขียน `if (mod.Name == "...")` ในไฟล์นั้น)
- helper ในฟอร์ม (`AdminHelpers.cs`): `InputText` · `InputFileManeger` (ปุ่มเปิด elFinder + ปุ่มลบรูปออกจากเนื้อหา) ·
  `InputTextLinkCMSPage` (URL + Link Helper) · `InputUrlTarget` (`_top` / `_blank`) · `InputDateTime` — **อาร์กิวเมนต์ตัวที่ 2 = ชื่อคอลัมน์ใน DB**
- ฟอร์ม 2 ภาษาใช้คู่ `x` / `en_x` · วันที่แสดงผลใช้ `issue_date_config` (1 = ตลอดเวลา, 2 = กำหนดช่วง) + `issue_date` / `expiry_date` เป็น **พ.ศ.**
- validation ฝั่ง client อยู่ใน `sConfirmCustom(form)` ท้ายไฟล์ (เมนู slide บังคับ `title` **และ** `en_title` + ตรวจช่วงวันที่)
- เมนูที่ยกจาก SAM เปลี่ยนแค่สี (`confirmButtonColor: '#00295A'`) และข้อความ — ไม่ต้องแก้โครง

### 6. Database — ตาราง, ข้อมูลตัวอย่าง, สิทธิ์

**ตาราง** — ตรวจก่อนว่ามีหรือยัง: `select name from sys.tables where name = '2026_web_core_item'`
(เมนู SAM ส่วนใหญ่มีตารางแล้วเพราะย้ายมาทั้ง 69 ตาราง · ตารางที่*ไม่มี*อยู่ในหัวข้อ "เมนูที่ใช้ไม่ได้" ด้านบน)
ตารางใหม่: prefix `2026_`, `id` IDENTITY, `created_at/updated_at` `datetimeoffset`, `created_by/updated_by/approve_by`,
`sort/status/pb_status/show_front/web_id` และคู่ `pb_<field>` ของทุกฟิลด์เนื้อหา

**ข้อมูลตัวอย่าง** (แทนเนื้อหา SAM) — เขียน SQL เป็นไฟล์ใน scratchpad แล้วรันด้วย
`sqlcmd ... -b -f 65001 -i "$(cygpath -w <file.sql>)"` (**ต้อง `-f 65001` และ path แบบ Windows** ไม่งั้นภาษาไทยเพี้ยน / เปิดไฟล์ไม่ได้)
- สำรองแถวเดิมก่อนลบเสมอ (`select * ... > scratchpad/backup_*.txt`) · ครอบด้วย `BEGIN TRAN ... COMMIT` + `SET XACT_ABORT ON`
- ใส่ทั้งฉบับร่างและ `pb_*` ให้ตรงกัน + `status = 1, pb_status = 1, show_front = 1, web_id = 0, module_id = <n>`, `sort` = 10, 20, 30 …
- **รูป**: copy ไฟล์ไปไว้ใต้ `wwwroot/Files/Site0/1/<โฟลเดอร์>/` แล้วเก็บ path แบบ **`Files/Site0/1/<โฟลเดอร์>/<ไฟล์>` (ไม่มี `/` นำหน้า)**
  — รูปแบบเดียวกับที่ elFinder ใส่ให้ · root ของ elFinder คือ `Files/Site{webID}/{admin_id}` (แสดงในหน้าต่างว่า "Files/Site0")
  ⚠ `wwwroot/Files/` ไม่ไปกับ git และ publish → **ต้องอัปโหลดรูปขึ้นเซิร์ฟเวอร์เอง**

**สิทธิ์** — ตาราง `[2026_web_admin_module]` (1 แถว / `mod_name` / `access_id` / `web_id`):
```sql
select * from [2026_web_admin_module] where mod_name = '<Module>';          -- เมนู SAM ส่วนใหญ่มีแถวของ access_id 1 อยู่แล้ว
insert into [2026_web_admin_module] (created_at, updated_at, created_by, updated_by, sort, status, mod_name, access_id,
  can_add, can_edit, can_delete, can_move, can_status, can_export, can_approve, web_id)
values (sysdatetimeoffset(), sysdatetimeoffset(), 'user', 'user', 0, 0, '<Module>', 1, 1, 1, 1, 1, 1, 0, 1, 0);
```
หรือเปิด `/Admin/AdminAccess/Edit/1` แล้วติ๊กในหน้าจอ (ได้ผลเหมือนกัน) · ไม่มีแถว = เมนูไม่ขึ้นและ `[ModuleCheck]` ปฏิเสธ
(`2026_web_admin_access` ตอนนี้มีบทบาทเดียวคือ id 1 = Super Admin · elFinder ต้องมีสิทธิ์ `FileManager` ด้วย)

### 7. Preview (ฝั่ง admin)

ลงทะเบียนใน `Areas/Admin/Helpers/PreviewMenu.cs` (`{ "<Module>", "page" | "item" | "cms" }`) — เมนู SAM มีอยู่แล้วส่วนใหญ่
ปุ่ม [Preview] จะขึ้นหน้าคอลัมน์แรกของ `ListData` และเปิด `{FrontURL}/_preview/{mode}/{Module}/{id}` ใน iframe
(`FrontURL` ใน `appsettings.Development.json` = `https://localhost:7310`)
**ตราบใดที่ front-end ยังไม่ทำ Phase B ของเมนูนี้ iframe จะขึ้น 404 ของ front-end** — เป็นเรื่องปกติ ไม่ใช่บั๊กของ admin

### 8. Build + restart

แก้ `.cs` ต้องรีสตาร์ท server — **ต้องฆ่า `core_admin.exe` ด้วย** ไม่งั้น build ล้มเพราะไฟล์ถูกล็อก (MSB3027):
```powershell
Get-Process core_admin -ErrorAction SilentlyContinue | Stop-Process -Force
dotnet build core_admin.csproj -nologo -v q     # แล้ว spawn ตามหัวข้อ "รัน server ให้อยู่รอด..." (ใส่ --no-build ได้)
```
แก้แค่ `.cshtml` ไม่ต้องรีสตาร์ท

### 9. ทดสอบ (Playwright + ตรวจ DB ทุกขั้น)

| หัวข้อ | สิ่งที่ต้องเห็น |
|---|---|
| เมนู / breadcrumb | เมนูขึ้นในกลุ่มถูกที่ + active · breadcrumb = `TextBreadcrumb` · หน้า AdminAccess มีเมนูพร้อม checkbox สิทธิ์ |
| List | แถว + รูป thumbnail โหลดได้ (`naturalWidth > 0`) · ปุ่มตาม `Can*` · ค้นหาไทย / อังกฤษ / ไม่พบ / อักขระพิเศษ `' OR 1=1 --` / Clear |
| Create | ฟอร์มว่าง → แจ้งเตือน · ขาด EN title → แจ้งเตือน · เลือกรูปผ่าน elFinder จริง · บันทึกครบ 2 ภาษา + `_blank` + กำหนดช่วงวันที่ → DB ตรงทุกคอลัมน์, `pb_status = 0`, `show_front = 0`, `sort = max + 10` |
| Edit | ค่าเดิมโหลดครบ + preview รูป · แก้แล้ว `pb_status = 0` แต่ `pb_*` ยังเป็นค่าเดิม · ลบรูปออก → คอลัมน์เป็น NULL · วันที่สิ้นสุด < วันที่เริ่ม → แจ้งเตือน |
| Approve | ทุกคอลัมน์ใน `FieldApprove` = `pb_*` · `pb_status = 1, show_front = 1, approve_by` |
| Status | เปิด / ปิด (/ ปักหมุด ถ้ามี) · แถวใหม่ที่ยังไม่เคยเผยแพร่ **ปิดไม่ได้** (ตั้งใจ — เหมือน SAM) |
| Move | กด "จัดเรียง" ก่อน แล้วลากด้วย **`locator.dragTo()`** (SortableJS ใช้ HTML5 drag — `mouse.down/move/up` ไม่ทำงาน) ทั้งขึ้นและลง → `sort` เรียงใหม่ 10, 20, 30 … |
| Delete | กดยกเลิก → ไม่ลบ · ยืนยัน → ลบ + `sort` เรียงใหม่ |
| อื่น ๆ | ไม่ login → เด้งไปหน้า Login · id ที่ไม่มี → กลับหน้า list · Admin Log มี add/edit/move/approve/delete · console ไม่มี error · มือถือ 390px ไม่มี scroll แนวนอน |

ของที่เห็นทุกหน้าและ**ไม่เกี่ยวกับเมนู**: warning `moment.defineLocale` และ Google Fonts โหลดไม่ได้ใน browser ทดสอบ
ปิดท้ายด้วย**ลบข้อมูลทดสอบผ่าน UI** แล้ว query ยืนยันว่าเหลือเฉพาะข้อมูลจริง (ฉบับร่าง = `pb_*`)

### 10. เอกสาร (ปิด Phase A)

- `docs/backend-menu-status.html` — ย้ายแถวจากตาราง "ปิด" ไป "เปิด" + แก้ตัวเลขทั้ง 2 หัวตาราง + วันที่อัปเดต
- CLAUDE.md — เพิ่มแถวในตาราง "เมนูที่ทำตาม playbook แล้ว" (ท้าย playbook) พร้อม **สเปกที่ front-end ต้องทำตาม**:
  ตาราง/`module_id`, คอลัมน์ `pb_*` ที่ฟอร์มมี → แสดงตรงไหนบนหน้าเว็บ, เงื่อนไข gate (status = 1 หรือ > 0 ถ้ามีปักหมุด), ลำดับ, กฎพิเศษที่ผู้ใช้กำหนด
  — Phase B จะอ่านแถวนี้เป็นโจทย์
- **commit + push ฝั่ง admin ทันทีเมื่อจบงาน** (กฎใหม่ 17 ก.ย. 2569 — ดูหัวข้อ "Git" ต้นไฟล์) และบอกในรายงานว่า commit อะไร push ไปไหน

### 11. กับดักที่เจอจริง

- **SQL ดิบที่ใช้ `Module.Config.Table` ต้องครอบ `Db.T()` เสมอ** — `web_core_item` ไม่มีจริงใน SQL Server (ชื่อจริง `[2026_web_core_item]`)
  ของ SAM ไม่เจอเพราะ PostgreSQL ไม่มี prefix · ที่ `AdminHelpers.cs` (หา sort ถัดไป + `reSort()`) เคยตกหล่น 4 จุด
  พังแบบ**เงียบ** เพราะ `catch` คืน `sort = 10` ทุกแถวใหม่ และการเรียงใหม่หลัง move/delete ไม่ทำงาน — แก้แล้ว 16 ก.ย. 2569
  ตรวจซ้ำได้ด้วย `grep -rnE "Module\.Config\.Table[,)]" Areas | grep -E "ExecuteQuery|ExecuteNonQuery" | grep -v "Db.T("`
  (`_db.Insert` / `_db.Update` เติม prefix ให้เองแล้ว)
- **`AdminCoreController.Edit` ไม่กรอง `module_id`** — `/Admin/HomeImageSlide/Edit/15` เปิดแถวของ module 2 ได้ (SAM เป็นเหมือนกัน)
  ต้องรู้ id และล็อกอินอยู่แล้ว · ยังไม่แก้เพราะกระทบทุกเมนู `web_core_*` — ถ้าจะแก้ให้แก้ที่ `AdminCoreController` ครั้งเดียว
- checkbox หน้าแรกของ list ใช้กับ **Export เท่านั้น** ไม่มีลบหลายแถว
- **DEFAULT constraint ของคอลัมน์วันที่ 92 ตัวใน 24 ตาราง `2026_*` เคยเป็น literal `'2023-04-21 …+07'`** (ตกค้างจากการย้าย PostgreSQL) ซึ่ง SQL Server แปลงเป็น `datetimeoffset` ไม่ได้
  → INSERT ใด ๆ ที่ไม่ส่งคอลัมน์นั้น (เช่น `pb_issue_date` ตอน Create) พังด้วย "Conversion failed when converting date and/or time" — **แก้แล้ว 16 ก.ย. 2569** (เปลี่ยนเป็น `+07:00` ทั้งหมด, script ใน scratchpad `fix_defaults.sql`)
  ⚠ **DB บนเซิร์ฟเวอร์จริงต้องรัน fix เดียวกัน** — ตรวจด้วย `select count(*) from sys.default_constraints where definition like '%+07'')'` ต้องได้ 0
- **เมนู SAM บางตัวมี `Views/<Module>/Index.cshtml` ของตัวเอง** (สำเนาเก่าของ `AdminCore/Index.cshtml` จาก initial commit — ไม่มีปุ่ม Preview และ fix หลัง ๆ)
  ASP.NET เลือกไฟล์นั้นก่อน shared view → **ลบทิ้ง** ให้ใช้ `AdminCore/Index.cshtml` (ทำแล้วกับ `HomeIntroPage`) · ตรวจก่อนเปิดเมนูอื่น: `ls Areas/Admin/Views/<Module>/`
- ฟอร์ม Edit ล็อกทั้งฟอร์มเมื่อ `pb_status = 0` (`checkLockedRow()` ใน `admin_site.js`) — ตอนทดสอบ Playwright ต้อง Approve ก่อนถึงจะ `fill()` ได้
- ชื่อที่แสดงของ admin (`created_by` / `updated_by`) คือคอลัมน์ `name` ของ `[2026_web_admin]` ซึ่งยังเป็นของ SAM ("สยามอี ซีเอ็มเอส") ส่วน `approve_by` คือ username

### Phase B — front-end ดึงข้อมูลจาก DB + Preview (`d:\Project\assetfund.co.th.2026`)

**อ่าน `d:\Project\assetfund.co.th.2026\CLAUDE.md` ก่อนเสมอ** — กฎของโปรเจกต์นั้น: แก้เสร็จ **ต้อง `git commit` + `git push` ทันที** (push ด้วย URL ตรง — ดูหัวข้อ "Git" ต้นไฟล์นี้),
ต้องทดสอบใน browser ครบ 7 ขั้น, **ห้ามแก้ `wwwroot/css/*.css` ตรง ๆ** (compile จาก `scss/` ด้วย Visual Studio เท่านั้น)
โครงสร้างพื้นฐานของ Phase B (ทำไว้แล้วตอน `HomeImageSlide` — เมนูถัดไป**ไม่ต้องสร้างซ้ำ**):

| ไฟล์ (ใน front-end) | มีไว้ทำอะไร |
|---|---|
| `Helpers/DBHelper.cs` | `_db.q(sql, params)` → `List<Dictionary<string, object?>>` · `DBHelper.T("web_core_item")` → `[2026_web_core_item]` · DB ล้มคืน list ว่าง + log · เคารพ `DBCacheTime` (**0 = ปิด cache** ตามผู้ใช้สั่ง) และไม่ cache request ที่เป็นพรีวิว |
| `Helpers/PreviewMap.cs` | `PreviewState` (เก็บใน `HttpContext.Items`) — `Cols()` alias ฉบับร่างเป็น `pb_*`, `Gate()` ตัดเงื่อนไขในโหมดพรีวิว, `HomeSlideGate` / `DefaultGate` (T-SQL) · ทะเบียน `PreviewMap.Pages` |
| `Controllers/HomeController.cs` → `PreviewPage()` | route `/_preview/page/{module}/{id:long}?lang=` ตรวจทะเบียน + `RowExists` → `preview.Set()` → เรนเดอร์หน้าแรก · ใส่ `X-Robots-Tag`, `no-store`, `CSP frame-ancestors 'self' <AdminURL>` |
| `Services/SqlHeroSlideService.cs` | **แม่แบบ** service ที่อ่าน DB + รองรับพรีวิว — ลอกโครงนี้ทุกครั้ง |
| `appsettings*.json` | `ConnectionStrings:DBConnection` (dev `Server=.;Database=asset_plus_uat;User ID=sa;Password=sasa;TrustServerCertificate=True;Encrypt=False`) · `AdminURL` (dev `https://localhost:7300` — ต่อหน้า path รูป `Files/...`) · `DBCacheTime: 0` · `appsettings.json` เก็บแต่โครง ค่าจริงอยู่บนเซิร์ฟเวอร์ |
| `Program.cs` | `AddHttpContextAccessor()` + `AddMemoryCache()` + `AddSingleton<DBHelper>()` แล้ว · ทุก service ลงทะเบียนที่นี่บรรทัดเดียว |

### 12. หาว่าหน้าไหน / service ไหนของ front-end ต้องเปลี่ยนจาก mock เป็น DB

- หา interface ใน `Services/I*Service.cs` ที่หน้านั้นใช้ (`Program.cs` มีคอมเมนต์ไทยบอกทุกบรรทัด) และ view ที่อ่าน model นั้น
- ถ้าเป็นเนื้อหาที่ยังไม่มี service (เช่น hero เดิม hardcode ใน `HomeController.BuildHomeViewModel`) → สร้าง interface ใหม่ `IXxxService` ใน `Services/`
- **model ของ view ต้องรองรับทุกฟิลด์ที่ฟอร์มหลังบ้านมี** (เปิด `Views/<Module>/Edit.cshtml` ฝั่ง admin ไล่ดู `Input*("...", "<คอลัมน์>", ...)`) —
  เช่น hero เพิ่ม `Id`, `Target`, `IsPinned` ใน `HeroSlide` · ฟิลด์ใน `FieldCreate_*` ที่ฟอร์ม**ไม่มี**ไม่ต้องแสดง (เหมือน SAM)
- เทียบกับ SAM ว่าเมนูนี้แสดงบนเว็บอย่างไร: `grep -n "<Module>\|module_id = '<n>'" d:/Project/sam.or.th/Controllers/HomeController.cs` แล้วดู view ที่ใช้ `ViewBag.<Key>`

### 13. เขียน `Services/Sql<ชื่อ>Service.cs` (ลอกจาก `SqlHeroSlideService.cs`)

```csharp
public sealed class SqlXxxService : IXxxService
{
    private const string Table = "web_core_item";   // ชื่อตรรกะ (ไม่มี prefix) — ตรงกับ Table ใน ModuleConfig ฝั่ง admin
    private const int ModuleId = 1;                 // = TableModuleID ฝั่ง admin
    private readonly DBHelper _db; private readonly IHttpContextAccessor _ctx; private readonly string _adminUrl;
    public SqlXxxService(DBHelper db, IHttpContextAccessor ctx, IConfiguration config)
    { _db = db; _ctx = ctx; _adminUrl = (config["AdminURL"] ?? "").TrimEnd('/'); }

    public IReadOnlyList<Xxx> Get(string lang = "th")
    {
        var preview = PreviewState.For(_ctx.HttpContext);   // นอกพรีวิว = โหมดปกติเสมอ
        var rows = _db.q(
            $"SELECT id, status, {preview.Cols(Table, ModuleId, "pb_title, pb_en_title, pb_img1, ...")} " +
            $"FROM {DBHelper.T(Table)} WHERE module_id = {ModuleId} AND web_id = 0 " +
            preview.Gate(Table, ModuleId, PreviewState.DefaultGate) +   // เมนูที่มีปุ่มปักหมุดใช้ HomeSlideGate (status > 0)
            "ORDER BY sort ASC, id ASC");                               // มีปักหมุด: ORDER BY status DESC, sort ASC, id ASC
        ...map แต่ละ row → model (ค่าไทย/อังกฤษผ่าน L(), รูปผ่าน ImageUrl())...
    }
}
```
กฎที่ห้ามหลุด:
- **SELECT เฉพาะ `pb_*`** (คอลัมน์ธรรมดาคือฉบับร่าง) — ยกเว้น `id`, `status`, `sort` ที่ไม่มีคู่ `pb_`
- ใส่ `web_id = 0` เสมอ (เว็บไซต์หลัก) และ `module_id` ตรงกับ admin — ตาราง `web_core_*` ใช้ร่วมกันหลายเมนู
- gate ต้องผ่าน `preview.Gate(...)` เท่านั้น ห้ามเขียน `AND status = 1` ตรง ๆ ไม่งั้นพรีวิวไม่เห็นแถวที่ปิด/ยังไม่ถึงกำหนด
- `Cols()` alias ได้เฉพาะชื่อที่ขึ้นต้น `pb_` — คอลัมน์ที่ต้องการทั้งร่างและอนุมัติให้ใส่ใน list นี้
- path รูป/ไฟล์จาก DB = `Files/Site0/1/...` (ไม่มี `/` นำหน้า, ไฟล์อยู่ฝั่ง admin) → `AdminURL + "/" + path` · URL เต็ม (`http…`) ใช้ตามนั้น · ว่าง = null
- ภาษา: `pb_x` ไทย / `pb_en_x` อังกฤษ, อังกฤษว่างให้ fallback ไทย (เหมือน `UHelper.L` ของ SAM) — front-end ตัวใหม่ยังไม่มีระบบสลับภาษา แต่พรีวิวส่ง `?lang=en` มา
- วันที่ที่ส่งเข้า SQL ใช้ `SYSDATETIMEOFFSET()` ฝั่ง DB — อย่า format `DateTime` จาก C# (culture th-TH เป็น พ.ศ.)
- แถวที่ข้อมูลไม่พอแสดง (เช่นไม่มีรูปหลัก) ให้ **ข้าม** ไม่ใช่ throw

### 14. สลับ service ใน `Program.cs` + ลงทะเบียนพรีวิว

```csharp
builder.Services.AddSingleton<IXxxService, SqlXxxService>();   // แทนบรรทัด Mock เดิม (คงคอมเมนต์ไทยไว้)
```
`Helpers/PreviewMap.cs` → เพิ่มใน `PreviewMap.Pages`: `new("<Module>", "<table>", <module_id>, PreviewMap.BoxDataHome)`
- ชื่อ `<Module>` ต้องตรงกับ `PreviewMenu.cs` ฝั่ง admin ทุกตัวอักษร (อยู่ใน URL)
- `BoxDataHome` = เนื้อหาอยู่บนหน้าแรก (`PreviewPage()` เรนเดอร์ `Index` ให้เลย) · **เนื้อหาอยู่หน้าอื่น** → ต้องขยาย `PreviewPage()`
  ให้เรียก action/view ของหน้านั้นด้วย model ที่สร้างในโหมดพรีวิว (ดูวิธีของ SAM: `RenderAsIndex(Index(lang, pageSeo))` ใน `sam.or.th/Controllers/HomeController.cs`)
- ถ้าเมนูมี "หน้ารายละเอียดรายตัว" (ข่าว/บทความ) ใช้โหมด `item` — ตอนนี้ front-end ตอบ 404 อยู่ ต้องเขียน `PreviewItem()` ตามแบบ SAM ก่อน

### 15. View — แสดงทุกฟิลด์ที่หลังบ้านแก้ได้ + โหมดพรีวิว

- ลิงก์: ครอบ `<a>` เฉพาะเมื่อมี URL (hero: ต้องมี**ทั้ง** `pb_url` และ `pb_url_target` — กฎผู้ใช้) · `_blank` → `target="_blank" rel="noopener"` · อื่น ๆ `_top`
- โหมดพรีวิว (`ViewBag.IsPreview == true`): ซ่อนสิ่งที่บังเนื้อหา (popup ประกาศ) และพาไปยังแถวที่กำลังแก้ (`ViewBag.PreviewRowId` → hero ใส่ `data-initial-slide` ให้ `home.js` เปิดสไลด์นั้น)
- ใส่ `data-<x>-id="@item.Id"` บน element ของแต่ละแถว — ใช้ตอนทดสอบ (หา element จาก id ใน DB)
- ห้ามแตะ `wwwroot/css` — ถ้าต้อง style ใหม่ให้แก้ `scss/` แล้วบอกผู้ใช้ compile ใน Visual Studio

### 16. Build + run front-end แบบ https

```powershell
Get-Process asset-fund -ErrorAction SilentlyContinue | Stop-Process -Force      # ชื่อ process ของ front-end
dotnet build asset-fund.csproj -nologo -v q
# spawn หลุด job object (ดู CLAUDE.md ของ front-end) โดยใช้ --launch-profile https → https://localhost:7310
```
**ต้อง https** — admin เป็น https ถ้า iframe ชี้ไป `http://localhost:5310` browser จะบล็อก (mixed content) และ `FrontURL` ของ admin ก็ตั้งเป็น 7310

### 17. ทดสอบฝั่ง front-end (Playwright) — ทำครบทุกข้อก่อน commit

| หัวข้อ | สิ่งที่ต้องเห็น |
|---|---|
| หน้าจริง | ข้อมูลตรงกับ `pb_*` ใน DB ทุกแถว · รูป `src` ชี้ `https://localhost:7300/Files/...` และ `naturalWidth > 0` · ลิงก์/target ถูกตามกฎ · 1440px และ 390px ไม่มี scroll แนวนอน · มือถือเลือกรูป Mobile (`img.currentSrc`) · console ไม่มี error ใหม่ (favicon 404 เป็นของเดิม) |
| ฉบับร่าง vs หน้าจริง | ตั้ง marker ในคอลัมน์ฉบับร่างของ 1 แถว (SQL หรือแก้ผ่านหลังบ้านโดยไม่อนุมัติ) → หน้าจริง**ต้องไม่เห็น** · `/_preview/page/<Module>/<id>` **ต้องเห็น** ทั้ง `?lang=th` และ `?lang=en` · พรีวิวไม่มี popup · เปิดที่แถวนั้นทันที |
| gate | ปิดแถว (`status = 0`) / ช่วงวันที่หมดอายุ (`pb_issue_date_config = 2` + `pb_expiry_date` อดีต) / รูปหลักว่าง → หน้าจริงหาย, พรีวิวยังเห็น · ปักหมุด (`status = 2`) → ขึ้นก่อน (ถ้าเมนูมี) |
| header พรีวิว | `content-security-policy: frame-ancestors 'self' https://localhost:7300` · `x-robots-tag: noindex` · `cache-control: no-store` · หน้าจริง**ไม่มี** CSP นี้ |
| 404 | module ไม่รู้จัก / id ของ module อื่น / id ไม่มี / `id` ไม่ใช่ตัวเลข / `/_preview/item/...` `/_preview/cms/...` → 404 หน้าธีม |
| regression | หน้าอื่นที่ใช้ model เดียวกัน (`/home/preview`, `/salepage`) และหน้าทั่วไป (`/funds`, `/about`, path มั่ว → 404) ยังปกติ |
| Playwright tips | popup ประกาศเด้งหลังโหลด ~2 วิ และบัง click → ถอด `#announcementModal, .modal-backdrop` ออกจาก DOM ก่อนคลิก · ทดสอบ `_blank` ด้วย `a.click()` ใน `$eval` + `context.waitForEvent('page')` · Swiper loop จัด DOM ใหม่ อย่าเทียบลำดับจาก DOM ให้ดู `.swiper-slide-active` |

**คืนข้อมูลทดสอบให้เป็นค่าจริงเสมอ** (ผ่าน UI หลังบ้านหรือ SQL) แล้ว query ยืนยัน ฉบับร่าง = `pb_*` ทุกแถว
จากนั้น **`git add -A && git commit` + `git push https://github.com/anurakball/assetfund.co.th.2026.git master`** ใน front-end (ข้อความ commit บอกว่าทำอะไร)

### Phase C — ทดสอบปลายทางร่วม 2 ฝั่ง + ส่งมอบ

### 18. flow จริงผ่านหน้าจอ admin (ต้องผ่านครบ 4 จังหวะ)

1. **แก้ไข** ผ่าน `/Admin/<Module>/Edit/<id>` (เปลี่ยนข้อความ + URL + target) → บันทึก → หน้าจริง (7310) **ยังเป็นค่าเดิม**
2. **กดปุ่ม [Preview]** ในหน้า list → modal เปิด iframe `https://localhost:7310/_preview/page/<Module>/<id>?lang=th` → เห็นค่าใหม่ทันที (ไม่ต้องรีสตาร์ท = cache 0 จริง)
   สลับปุ่ม "อังกฤษ" → iframe โหลด `?lang=en` เห็นค่าอังกฤษ · Esc ปิด modal ได้
   (Playwright เข้าถึง iframe ข้าม origin ได้: `page.frames().find(f => f.url().includes('/_preview/'))`)
3. **Approve** → หน้าจริงเห็นค่าใหม่ทันที
4. **Status** (ปิด / ปักหมุด) ผ่านปุ่มในหน้า list → หน้าจริงเปลี่ยนตาม (หาย / ขึ้นก่อน) → คืนค่า

### 19. ปิดงาน — เอกสาร 2 ฝั่ง + รายงาน

- **admin `CLAUDE.md`**: ตาราง "เมนูที่ทำตาม playbook แล้ว" → ใส่สถานะ "front-end ต่อแล้ว + Preview ใช้ได้" และชื่อ service · หัวข้อ "เว็บไซต์ front-end" → อัปเดตรายการเมนูที่ต่อแล้ว
- **admin `docs/preview-spec.md`**: บรรทัดสถานะบนสุด (เมนูที่พรีวิวได้จริง)
- **front-end `CLAUDE.md`**: ตาราง "สถานะปัจจุบัน" (เมนูที่ต่อ DB แล้ว) + รายชื่อไฟล์ที่แตกต่างจาก static web (`assetfund.co.th.html`) เพราะ copy ทับตรง ๆ ไม่ได้อีกแล้ว
- **commit + push ทั้ง 2 ฝั่งที่มีไฟล์แก้** (หัวข้อ "Git" ต้นไฟล์) แล้วรายงานผู้ใช้: สิ่งที่แก้ทั้ง 2 ฝั่ง, ผลทดสอบทุกข้อ (บอกตรง ๆ ถ้าข้ามข้อไหน), commit/push ที่ทำ, สิ่งที่ยังไม่ทำ (เช่นเมนูตั้งค่าที่ยังไม่เปิด)

### เมนูที่ทำตาม playbook แล้ว

| กลุ่ม > เมนู | Module | ตาราง / module_id | ข้อมูล | หมายเหตุสำหรับ front-end |
|---|---|---|---|---|
| หน้าหลัก > รูปสไลด์หน้าแรก | `HomeImageSlide` | `web_core_item` / 1 | 3 สไลด์กองทุน (ASP-DEFENSE, A-HUMANOID, A-ASEMI) · รูปใน `Files/Site0/1/home/hero-*.jpg` | **front-end ต่อแล้ว + Preview ใช้ได้** (16 ก.ย. 2569) — `Services/SqlHeroSlideService.cs` |
| หน้าหลัก > หน้า Intro Page | `HomeIntroPage` | `web_home_intro_page` / ไม่มี module_id (ตารางเดี่ยว) | 1 แถว (id 27) "ประกาศแจ้งเตือนภัย" · รูป `Files/Site0/1/intro_page/scam-alert.jpg` | **front-end ต่อแล้ว + Preview ใช้ได้** (16 ก.ย. 2569) — `Services/SqlIntroPageService.cs` + route `/intro-page` — ดูสเปกด้านล่าง |
| หน้าเว็บไซต์ > หน้า Pop-Up | `HomePopUp` | `web_home_pop_up` / ไม่มี module_id (ตารางเดี่ยว) | 2 แถว (id 10 ประกาศเตือนภัย → `/news-announcements`, id 11 ติดตาม Facebook → `_blank`) · รูป `Files/Site0/1/pop_up/{scam-alert,follow-facebook}.jpg` | **front-end ต่อแล้ว + Preview ใช้ได้** (16 ก.ย. 2569) — `Services/SqlPopupBannerService.cs` → modal `#announcementModal` บนหน้าแรก — ดูสเปกด้านล่าง |
| หน้าเว็บไซต์ > SEO & Code | `HomeSEO` | `web_home_seo` / ไม่มี module_id (ตารางเดี่ยว, **แถวเดียว id 1**, `CanAdd/Delete/Status = false`) | title/description/keywords ของ Asset Plus · ช่องโค้ดฝัง (GA/Pixel/Other 1–3) ว่าง | **front-end ต่อแล้ว + Preview ใช้ได้** (17 ก.ย. 2569) — `Services/SqlSiteSeoService.cs` → `Views/Shared/_SeoHead.cshtml` + `_SeoBody.cshtml` ในทุก layout — ดูสเปกด้านล่าง |
| หน้าเว็บไซต์ > ปรับแต่ง Header | `HomeHeader` | `web_home_header` / ไม่มี module_id (ตารางเดี่ยว, **แถวเดียว id 1**, `CanAdd/Delete/Status/Move = false`) | โลโก้ `Files/Site0/1/header/logo-dark.svg` (TH = EN) · Alt TH "บริษัทหลักทรัพย์จัดการกองทุน แอสเซท พลัส จำกัด" / EN "Asset Plus Fund Management" | **front-end ต่อแล้ว + Preview ใช้ได้** (17 ก.ย. 2569) — `Services/SqlSiteHeaderService.cs` → `Views/Shared/_PartialHeader.cshtml` + `_PartialHeaderSale.cshtml` — ดูสเปกด้านล่าง |
| หน้าเว็บไซต์ > ปรับแต่ง Footer | `HomeFooter` | `web_home_footer` / ไม่มี module_id (ตารางเดี่ยว, **แถวเดียว id 1**, `CanAdd/Delete/Status/Move = false`) | โลโก้ `Files/Site0/1/footer/logo-light.svg`, ที่อยู่/โทร/อีเมล/โซเชียล/ปี/สโลแกน/แอป/copyright ของ Asset Plus (ยกจาก `_PartialFooter.cshtml` เดิม) · QR `Files/Site0/1/footer/icon-app.png` | **front-end ต่อแล้ว + Preview ใช้ได้** (17 ก.ย. 2569) — `Services/SqlSiteFooterService.cs` → `Views/Shared/_PartialFooter.cshtml` + `_PartialFooterSale.cshtml` — ดูสเปกด้านล่าง |
| ข้อมูลหน้าแรก > จัดการ Widget (ย้ายจาก "หน้าเว็บไซต์" + เปลี่ยนชื่อจาก "จัดการเมนูเว็บไซต์" 17 ก.ย. 2569 — `TextBreadcrumb = ข้อมูลหน้าแรก/จัดการ Widget`) | `CMSPage` | `web_cms_page` / ไม่มี module_id (**แถวเดียว id 1 = หน้าแรก `is_home = 1`**, `CanAdd/Delete/Status/Move = false`) + ข้อมูล widget ใน `web_widget_group` (3 กลุ่ม = DEFAULT/MODERN/CLASSIC) และ `web_widget` (18 = 6 section × 3 เวอร์ชัน, คอลัมน์ใหม่ `section_key`) | `box_layout = wg_28,wg_29,wg_30,wg_31,wg_32,wg_33` (Version 1 ทั้งหน้า) · ไอคอน `Files/Site0/1/widget_icons/assetplus/icon-<SectionKey>.png` (6 ไฟล์ glyph ขาว ใช้ร่วมกัน 3 เวอร์ชัน) + `icon-group-{default,modern,classic}.png` (glyph น้ำเงิน #00295A) — สร้างด้วย ComfyUI 17 ก.ย. 2569 แทน screenshot · ชื่อ widget = ชื่อ section ไทยล้วน ("แบนเนอร์หน้าแรก") ชื่อกลุ่ม = `DEFAULT`/`MODERN`/`CLASSIC` (ผู้ใช้สั่งตัด "(Hero) — DEFAULT" / "(Version 1)" ออก 17 ก.ย. 2569) | **front-end ต่อแล้ว + Preview ใช้ได้** (18 ก.ย. 2569) — `Services/SqlHomeLayoutService.cs` → `Views/Home/Index.cshtml` วน partial ตามลำดับ — ดูสเปกด้านล่าง |

> ⚠ **17 ก.ย. 2569 ผู้ใช้สั่งยุบกลุ่ม "หน้าหลัก"** — เมนู `HomeImageSlide` / `HomeIntroPage` / `ApOtherIndices` ย้ายไปอยู่ท้ายกลุ่ม **"หน้าเว็บไซต์"** (ลำดับตาม SAM: กลุ่ม "หน้าเว็บไซต์" ก่อน แล้วต่อด้วยกลุ่ม "ข้อมูลหน้าแรก") และแก้ `TextBreadcrumb` เป็น `หน้าเว็บไซต์/...` แล้ว
> ชื่อ "หน้าหลัก >" ในตารางด้านบนคือชื่อกลุ่ม ณ วันที่ทำ — ปัจจุบันทุกเมนูอยู่ใต้ "หน้าเว็บไซต์"

**`HomeImageSlide` — สิ่งที่ front-end ทำตามอยู่** (`SqlHeroSlideService.cs` — ยึดตาม SAM `sam.or.th/Helpers/PreviewMap.cs` → `HomeSlideGate`):
- `status` มี **3 ค่า**: 0 = ปิด, 1 = แสดง, **2 = ปักหมุด** → กรอง **`status > 0`** (ถ้าใช้ `= 1` สไลด์ที่ปักหมุดจะหาย)
  และเรียง **`ORDER BY status DESC, sort ASC, id ASC`** (หมุดขึ้นก่อน)
- `show_front = 1` และ `(pb_issue_date_config = 1 OR (pb_issue_date < SYSDATETIMEOFFSET() AND pb_expiry_date > SYSDATETIMEOFFSET()))`
- `module_id = 1 AND web_id = 0` · อ่านคอลัมน์ `pb_*`: `pb_title` (ใช้เป็น alt), `pb_img1` (PC), `pb_img1_icon` (**Mobile**), `pb_url`, `pb_url_target` + `pb_en_*`
- path รูปเป็น `Files/...` ไม่มี `/` นำหน้า และไฟล์อยู่ฝั่ง admin → ต่อเป็น `https://<admin host>/Files/...`
- ขนาดแนะนำในฟอร์ม: PC 1920×1080 · Mobile 750×1040 (front-end เดิมแนะนำ 1920×720 / 828×1000)
- **ครอบ `<a>` ก็ต่อเมื่อมีทั้ง `pb_url` และ `pb_url_target`** (ผู้ใช้กำหนด) · `_blank` → `target="_blank" rel="noopener"` · ไม่มีรูป PC = ข้ามสไลด์ · ไม่มีรูป Mobile = ใช้รูป PC
- ฟิลด์ใน `FieldCreate_CoreItem` ที่ฟอร์มสไลด์**ไม่มี** (`title_color`, `des`, `des_color`, `title_url`, `img1_icon` ใช้เป็นรูป Mobile, `date_news`, `cat_id`) → front-end ไม่แสดง (เหมือน SAM)

**`HomeIntroPage` — สิ่งที่ front-end ทำตามอยู่** (`SqlIntroPageService.cs` + `Views/Home/Intro.cshtml` — ยึดตาม SAM `Views/Home/Intro.cshtml` + `title == "intro-page"` ใน `HomeController`):
- ตารางเดี่ยว **ไม่มี `module_id`** (`PreviewMap.NoModule`) · หน้าจริงเอาแถว **id ล่าสุด** ที่ผ่าน `status = 1 AND show_front = 1 AND (pb_issue_date_config = '1' OR ช่วงวันที่ <= / >=)`
  (`issue_date_config` ตารางนี้เป็น **nvarchar(2)** — เทียบด้วย `'1'`)
- flow: เข้า `/` ครั้งแรก → redirect `/intro-page` → ปุ่มที่ 1 "เข้าสู่เว็บไซต์" ไป `/?intro=1` → ตั้งคุกกี้ `AssetPlus.IntroSeen` (session cookie) → ไม่เด้งอีก
  (SAM ใช้ session — เว็บนี้ใช้คุกกี้เพราะยังไม่เปิด session และห้ามชนชื่อกับ admin) · ไม่มี intro ที่เผยแพร่ → `/intro-page` ข้ามไป `/` เอง
- ฟิลด์ → การแสดง: `pb_title` (h1 + `<title>`), `pb_main_color` (สีพื้นทั้งหน้า — front-end เลือกสีตัวอักษร/โลโก้ขาวหรือดำให้เองตามความสว่าง),
  `pb_img1` (+ `pb_url`/`pb_url_target` = ลิงก์รูป), `pb_info` (HTML จาก CKEditor), `pb_title_{1..3}_text/_color/_color_text/_url/_url_target` (ปุ่ม — แสดงเฉพาะที่มีข้อความ, ปุ่ม 1 ไม่ใช้ URL)
- **ฟอร์มนี้เป็นไทยอย่างเดียว** (ไม่มีช่อง `en_title`, `en_info` เป็น hidden ค่าว่าง) → บันทึกจากหลังบ้านครั้งใดค่า `en_*` จะถูกล้าง · พรีวิว `?lang=en` จึงเห็นไทย
- ฟอร์ม Edit ถูก**ล็อก**เมื่อ `pb_status = 0` (รออนุมัติ) — ต้อง Approve ก่อนแก้ต่อ (พฤติกรรม `checkLockedRow()` ของทุกเมนูที่ `CanApprove`)
- พรีวิว: `/_preview/page/HomeIntroPage/{id}` เรนเดอร์ `Intro.cshtml` ด้วยแถวนั้น (front-end เพิ่ม `PreviewState.RowFilter()` — SAM พรีวิวได้แต่แถว id มากสุด) และไม่ตั้งคุกกี้
- `CanMove = false` (ไม่มีจัดเรียง) · หน้า Edit บนมือถือ 390px ล้นแนวนอน ~85px จากตาราง 3 ปุ่มของฟอร์ม SAM — ของเดิม ไม่ได้แก้

**`HomePopUp` — สิ่งที่ front-end ทำตามอยู่** (`SqlPopupBannerService.cs` + `Views/Home/Partials/_PopupAnnouncement.cshtml` — ยึดตาม SAM `layoutContentService.GetHomePopUp()` + `_PartialFooter.cshtml`):
- กลุ่มเมนู "หน้าเว็บไซต์" ถูก**เปิดใช้แล้ว**แต่มีเมนูย่อยเดียว (ที่เหลือยัง `//`) — เปิดเมนูอื่นในกลุ่มนี้ทีละบรรทัดได้เลย
- ตารางเดี่ยว `web_home_pop_up` (`NoModule`) · หน้าจริงกรอง `status = 1 AND show_front = 1 AND (pb_issue_date_config = '1' OR ช่วงวันที่ < >)` เรียง `sort ASC, id ASC` สูงสุด **10 ใบ** (SAM `PopUpMax`)
- **จัดเรียงได้ (`CanMove = true` + `can_move = 1` ใน `2026_web_admin_module` — เปิด 17 ก.ย. 2569)** เพราะ front-end แสดงหลายใบเป็นสไลด์ต่อกันตามลำดับ `sort` · ใช้เครื่องยนต์กลาง (ปุ่ม "จัดเรียง" → ลาก `.move_handle` → `Move` → `reSort()` กิ่งตารางไม่มี `module_id` เรียงใหม่ 10, 20, 30…)
  ⚠ `sort` **ไม่มีคู่ `pb_*`** → การจัดเรียงมีผลกับหน้าเว็บ**ทันทีโดยไม่ต้อง Approve** และไม่เปลี่ยน `pb_status` (เหมือน `HomeImageSlide`) · ค้นหาอยู่หรือเรียงคอลัมน์อื่น = ไม่มีที่จับลาก และ server ปฏิเสธ Move · แถวรูปสูง ~300px — ทดสอบ Playwright ต้องขยาย viewport ให้สูง (เช่น 2200px) และลากไปที่ `targetPosition` ขอบบน/ล่างของแถวปลายทาง ไม่งั้น SortableJS ไม่รับ
- **ฟอร์มจริงมีแค่**: หัวข้อ, รูป (แนะนำ 1530×665 แต่ front-end ตัวนี้ใช้รูปแนวตั้ง ~2479×2917 เต็มกรอบ `object-fit`), URL ไทย/อังกฤษ, URL Target, วันที่แสดงผล
  — ช่องสีพื้นหลัง/เนื้อหา/ปุ่ม 1–3 ที่อยู่ใน `FieldCreate` ถูกปิดไว้ในฟอร์มของ SAM (บันทึกเป็น NULL) และ front-end ไม่เรนเดอร์ (เหมือน SAM แสดงรูป+ลิงก์อย่างเดียว)
- ฟิลด์ → การแสดง: `pb_img1` (รูปเต็มกรอบ), `pb_title` (alt), `pb_url`/`pb_en_url` (ครอบ `<a>` เมื่อมี · ค่า `#` = ไม่ลิงก์), `pb_url_target` (`_blank` → `rel=noopener`) · แถวไม่มีรูปถูกข้าม
- popup เด้งหลังโหลด ~1.2 วิ · ผู้ใช้ติ๊ก "ไม่ต้องแสดงอีก" = คุกกี้ `hide_announcement` (session) · **ไม่มีแถวที่เผยแพร่ = ไม่มี modal ใน DOM เลย**
- พรีวิว `/_preview/page/HomePopUp/{id}` = หน้าแรกที่เด้ง popup เปิดที่ใบนั้น (`data-initial-slide`) และ**ไม่สนคุกกี้ hide** (`data-preview="1"`) · พรีวิวเมนูอื่นของหน้าแรกจะไม่เด้ง popup (`ViewBag.PreviewModule`)

**`HomeSEO` — สิ่งที่ front-end ทำตามอยู่** (`SqlSiteSeoService.cs` + `Views/Shared/_SeoHead.cshtml` / `_SeoBody.cshtml` — ยึดตาม `layoutContentService.GetHomeSeo()` + `_Layout.cshtml` ของ SAM):
- ตารางเดี่ยว `web_home_seo` แถวเดียว (id 1, `web_id = 0`) **ไม่มี gate status** (เป็นค่าตั้งค่าของเว็บ) · เมนูมีแค่ Edit + Approve (ไม่มี Create/Delete/Status/Move)
- ฟิลด์ → การแสดง (ทุก layout: `_Layout`, `_LayoutSale`, `_LayoutIntro`): `pb_title`/`pb_en_title` → `<title>` เป็น `"<ชื่อหน้า> | <Title>"` + `og:site_name` ·
  `pb_des` → `meta description` **เฉพาะหน้าที่ไม่ได้ตั้ง `ViewData["MetaDescription"]` เอง** (หน้าแรก/กองทุนตั้งเองจึงไม่เปลี่ยน) · `pb_info` → `meta keywords` ·
  `pb_{ga,fb,ot1,ot2,ot3}_embed_head` → ท้าย `<head>` (ลำดับ GA, FB, Other1–3) · `pb_*_embed_body` → ต้น `<body>` — เรนเดอร์ด้วย `Html.Raw` (แอดมินวาง `<script>` ทั้งก้อน) ส่วน title/description/keywords ห้าม Raw
- **ไม่มีปุ่ม Preview** (ผู้ใช้สั่งถอด 17 ก.ย. 2569) — `PreviewMenu.cs` comment ไว้ และ front-end ถอดจาก `PreviewMap.Pages` → `/_preview/page/HomeSEO/1` ตอบ 404
- ⚠ ฟอร์ม Edit ของเมนูนี้**ไม่ล็อก**ตอน `pb_status = 0` (`checkLockedRow` ถูก comment ไว้ใน `Views/HomeSEO/Edit.cshtml` ของ SAM) — ต่างจากเมนูอื่น

**`HomeHeader` — สิ่งที่ front-end ทำตามอยู่** (`SqlSiteHeaderService.cs` + `Views/Shared/_PartialHeader.cshtml` / `_PartialHeaderSale.cshtml` — ยึดตาม `ViewBag.HomeHeader` (`pb_img1`) ใน `_PartialHeader` ของ SAM แต่**เปลี่ยนฟอร์ม**: SAM คือ "โลโก้ / Template" มีเลือก Template + โทนสี ซึ่งเว็บใหม่ไม่ใช้):
- ผู้ใช้กำหนด (17 ก.ย. 2569): header แก้ได้ **2 อย่างเท่านั้น** — รูปโลโก้ (TH/EN) และ Alt Text (TH/EN) · **ลิงก์โลโก้ fix ไปหน้าแรก `/` เสมอ** ไม่มีช่อง URL · ไม่มีรูป = ใช้โลโก้เดิมของเว็บ (`media/images/logo/logo-dark.svg`) เป็น default
- ฟิลด์ในฟอร์ม (`Field_HomeHeader`): `title` / `en_title` = Alt Text (TH บังคับ — ใช้เป็นชื่อรายการใน list + Admin Log, EN ว่าง = ใช้ TH) · `img1` / `en_img1` = โลโก้ (EN ว่าง = ใช้ TH; ทั้งคู่ว่าง = default)
  ⚠ **`en_img1` / `pb_en_img1` เป็นคอลัมน์ที่เพิ่มเองใน `[2026_web_home_header]` (SAM ไม่มี) — เซิร์ฟเวอร์จริงต้อง `ALTER TABLE ... ADD en_img1 nvarchar(255) NULL, pb_en_img1 nvarchar(255) NULL`** (script ที่ track ใน git: `docs/sql/2026-09-17-web-home-header-footer.sql`)
- คอลัมน์ SAM ที่เลิกใช้: `this_type` (Template) / `des` (โทนสี) — ตั้งเป็น NULL แล้ว และ**ลบกิ่ง `else if (Module.Name == "HomeHeader")` + `BuildMicrositeBoxLayout()` ออกจาก `AdminCoreController.Edit`** (เดิมเขียน `box_layout` ลง `web_cms_page` ตาม Template ที่เลือก — เว็บใหม่ไม่มี)
- ตารางเดี่ยว `NoModule` · **ไม่มี gate status** (SAM `WHERE id = 1`; หลังบ้าน `CanStatus = false`) → front-end อ่าน `TOP 1 ... WHERE web_id = 0 ORDER BY id` + `RowFilter` ในโหมดพรีวิว · path รูป `Files/...` → `AdminURL + "/" + path`
- แสดงที่: `<a href="/" aria-label="{alt}"><img src="{logo}" alt="{alt}" class="header--logo__image" data-header-logo="db|default">` ทั้ง header ปกติและ header ของ `/salepage` · โลโก้ใน cookie modal (`_CookiesModal.cshtml`) และหน้า intro **ยังเป็นไฟล์ static** (นอกขอบเขตที่ผู้ใช้ระบุ)
- พรีวิว `/_preview/page/HomeHeader/1` = หน้าแรกที่โลโก้ header เป็นฉบับร่าง (ไม่มีแถบสรุป — ห้ามมีแถบ/ป้าย/กล่องสรุปฉบับร่างใด ๆ บนหน้าพรีวิว (ผู้ใช้สั่งลบ 17 ก.ย. 2569 — ดูแล้วเข้าใจว่าหน้าเว็บแสดงผลผิด) พรีวิวต้องหน้าตาเหมือนหน้าเว็บจริงทุกอย่าง) · `?lang=en` เห็นค่า EN (fallback TH)
- ฟอร์ม Edit ล็อกเมื่อ `pb_status = 0` (`checkLockedRow`) เหมือนเมนูทั่วไป · ปุ่มลบรูป (`#id_img1_del`) มี SweetAlert ยืนยันก่อน

**`HomeFooter` — สิ่งที่ front-end ทำตามอยู่** (`SqlSiteFooterService.cs` + `Views/Shared/_PartialFooter.cshtml` / `_PartialFooterSale.cshtml` — ยึดโครง query จาก `layoutContentService.GetHomeFooter()` ของ SAM แต่**ฟอร์มออกแบบใหม่ตาม footer ของ Asset Plus**):
- ผู้ใช้กำหนด (17 ก.ย. 2569): แก้ได้ทุกอย่างใน **`<div class="footer__brand">`** (โลโก้, ที่อยู่, โทร, อีเมล, โซเชียลแต่ละอัน) และ **`<div class="footer__promo">`** (เลขปี, สโลแกน, หัวข้อ/ลิงก์แอป, QR) + บรรทัด copyright
  **ไม่รวม** `<div class="footer__nav">` (เมนู 5 คอลัมน์) และลิงก์ล่างสุด "นโยบายความเป็นส่วนตัว / นโยบายคุกกี้ / ข้อกำหนดการใช้งาน / แผนผังเว็บไซต์" (`footer__legal`) — จะทำเป็นเมนูหลังบ้านแยกทีหลัง ตอนนี้ยัง hardcode ใน view · คำเตือน ก.ล.ต. (`footer__disclaimer`) ก็ยัง hardcode
- ฟิลด์ในฟอร์ม (`Field_HomeFooter` 24 ช่อง) → การแสดง:
  `title`/`en_title` = alt โลโก้ (TH บังคับ) · `img1`/`en_img1` = โลโก้ (ว่าง = default `media/images/logo/logo-light.svg`, ลิงก์ไป `/` เสมอ) ·
  `address`/`en_address` (textarea — ขึ้นบรรทัดใหม่ได้ แสดงเป็น `<br>`) + `address_url` (มี = ครอบ `<a target=_blank rel=noopener noreferrer>`, ไม่มี = `<span class="list__static">`) ·
  `tel` (แสดงตามที่พิมพ์; `href="tel:"` คำนวณเอง: ตัดทุกอย่างที่ไม่ใช่ตัวเลข, นำหน้า `0` → `+66`) · `email` (`mailto:`) ·
  `sc_ln` / `sc_fb` / `sc_yt` / `sc_bd` = URL ของ LINE / Facebook / YouTube / Blockdit (ไอคอนคงที่ใน front-end `media/images/icons/`; ช่องว่างหรือไม่ใช่ http(s) = **ซ่อนไอคอนนั้น**; ไม่มีเลย = ไม่มี `<ul class="footer__social">`) ·
  `promo_year` (ว่าง = ปี ค.ศ. ปัจจุบัน) · `tagline_1` / `tagline_2` (แสดงเป็น 2 บรรทัดพร้อมกัน ไม่ใช่คู่ภาษา; ว่าง = ซ่อนบรรทัด) · `app_title`/`en_app_title` ·
  `sc_as` / `sc_gp` = URL App Store / Google Play (มี = ปุ่มเป็น `<a target=_blank>`, ไม่มี = `<span>` เหมือนเดิม; รูปปุ่มคงที่) · `app_qr` (รูป QR; ว่าง = ไม่แสดง) · `cr`/`en_cr` = copyright (ว่าง = "© <ปี> Asset Plus Fund Management Co., Ltd.")
  ⚠ **คอลัมน์ที่เพิ่มเองใน `[2026_web_home_footer]` (SAM ไม่มี): `img1, en_img1, address, en_address, address_url, sc_bd, promo_year, tagline_1, tagline_2, app_title, en_app_title, app_qr` + คู่ `pb_*` ทุกตัว — เซิร์ฟเวอร์จริงต้อง ALTER TABLE ด้วย** (script ที่ track ใน git: `docs/sql/2026-09-17-web-home-header-footer.sql`)
- คอลัมน์ SAM ที่เลิกใช้และไม่อยู่ในฟอร์ม: `sc_tw`, `sc_tt`, `info`/`en_info`, `btn{1..3}_*` (ตั้ง NULL แล้ว) · `email` ของ SAM เคยเก็บ**รูป** footer (`Files/Site0/1/footer-banner.webp`) — ตอนนี้เป็นอีเมลจริง
- ฝั่ง admin: ช่อง URL ทุกช่องตรวจ `^https?://` ใน `sConfirmCustom()` · ช่องอีเมลเป็น `<input type=email>` (browser บล็อกเองก่อนถึง JS) · `ListData_Default` (ไม่โชว์รูป เพราะโลโก้ตัวอักษรขาวบนพื้นขาวมองไม่เห็น) · thumbnail โลโก้ในฟอร์มใส่พื้น `#00295A`
- ตารางเดี่ยว `NoModule` · **ไม่มี gate status** (เหมือน SAM/HomeHeader) · หน้า `/salepage` ใช้ `_PartialFooterSale` ซึ่งไม่มีปี/สโลแกน/`footer__nav` (ตาม static เดิม) แต่โลโก้/ติดต่อ/โซเชียล/แอป/copyright มาจาก DB เหมือนกัน
- พรีวิว `/_preview/page/HomeFooter/1` = หน้าแรกที่ footer เป็นฉบับร่าง (ไม่มีแถบสรุป — เลื่อนลงไปดู footer ใน iframe) · `?lang=en` ใช้ `pb_en_*` (fallback TH)
- ทดสอบใน view ใช้ `data-footer-*` attribute (`data-footer-logo`, `-address`, `-tel`, `-email`, `-social`, `-year`, `-tagline1/2`, `-app-title`, `-store`, `-qr`, `-copyright`)

**`CMSPage` — page builder หน้าแรก (18 ก.ย. 2569) — สิ่งที่ front-end ทำตามอยู่** (`SqlHomeLayoutService.cs` + `Views/Home/Index.cshtml` — ยกกลไก "ตกแต่งเพจ" ของ SAM มาทั้งชุด แต่ทำให้ data-driven):
- ผู้ใช้กำหนด: ตาราง `web_cms_page` **เหลือแถวเดียว id 1** (หน้าแรก) — เมนูซ้าย "จัดการ Widget" (ชื่อเดิม "จัดการเมนูเว็บไซต์") เปิด list ที่มี 1 แถว (ปุ่ม แก้ไข / Approve / Preview เท่านั้น) · `/Admin/CMSPage/Edit/1` เข้าแท็บ "ตกแต่งเพจ" ทันที (เงื่อนไข `is_home = 1` ของ SAM) · แถว SAM id 8–79 และข้อมูล widget ของ SAM ลบแล้ว (`docs/sql/2026-09-18-delete-sam-cms-widgets.sql`, สำรองไว้ `docs/backup-sam-widgets/` เฉพาะเครื่อง dev)
- **builder**: พาเลตต์ซ้าย = accordion 3 กลุ่มจาก `web_widget_group` (DEFAULT = Version 1 / MODERN = Version 2 / CLASSIC = Version 3 ของ `/salepage`) → การ์ด widget จาก `web_widget` (`cat_id`) ลากด้วย SortableJS (clone) ลงแคนวาสขวา · ปุ่มต่อการ์ด: เลื่อนขึ้น/ลง (delegate ครั้งเดียว), ดินสอ "แก้ไขข้อมูล" (เฉพาะ widget ที่มี `mod_name` — hero → `/Admin/HomeImageSlide`), รีเฟรช, ลบ (คืนการ์ดให้พาเลตต์) · widget ที่อยู่บนแคนวาสถูกซ่อนจากพาเลตต์ (กันใส่ซ้ำ) · Save → `sortableMain.toArray()` → `box_layout = "wg_<web_widget.id>,…"` (ฉบับร่าง, `pb_status = 0`) → Approve → `pb_box_layout`
- **ตัวอย่างในการ์ด** (`WidgetAjaxController.Index`): `web_widget.pb_info` = HTML `<section>` ทั้งก้อนที่ snapshot จาก `/salepage` (ข้อความ hardcode ตามที่ผู้ใช้ตกลง) · hero ใช้ `mod_name = HomeImageSlide` + บล็อก `|||REPEAT|||…|||/REPEAT|||` เติมสไลด์จริงจาก `web_core_item` module 1 · แท็บ builder โหลด **CSS/Swiper ของ front-end จาก `FrontURL`** (`css/vendor.min.css`, `css/main.min.css`, `vendors/swiper/*`) แทน `css_home` ของ SAM → front-end ต้องส่ง header CORS ให้ฟอนต์ (`Program.cs` ของ front-end อนุญาต origin = `AdminURL`) · path `/media/...` ใน HTML ถูก `RewriteFrontAssetUrl()` ชี้ไป FrontURL ตอนแสดงตัวอย่าง
- **คอลัมน์ใหม่ `web_widget.section_key` / `pb_section_key`** (script `docs/sql/2026-09-18-web-widget-section-key.sql` — เซิร์ฟเวอร์จริงต้องรัน) = ชื่อ partial ฝั่ง front-end `Views/Home/Partials/_<key>.cshtml` (`Hero`, `NavPrices`, `FeaturedFunds`, `ExploreThemes`, `Insights`, `Distributors` + ท้าย `V2`/`V3`) · ฟอร์ม Widget มีช่อง "Section Key" และ "Mod Name" · front-end อ่าน `pb_box_layout` → แยก `wg_<id>` → `SELECT pb_section_key FROM web_widget WHERE id IN (…)` → เรนเดอร์ partial ตามลำดับ (SAM เทียบ `"wg_2"` แบบ hardcode ใน view — ไม่ทำตาม)
- **กฎ front-end**: key ที่ไม่มี partial / id ที่ไม่มีในตาราง / token ที่ไม่ใช่ `wg_n` → ข้าม · key ซ้ำ → เอาตัวแรก · `pb_box_layout` ว่าง = หน้าแรกไม่มี section (ตั้งใจ) · **อ่านแถว id 1 ไม่ได้ (DB ล้ม) = ใช้ลำดับ Version 1** · เวอร์ชัน V2/V3 ใช้ข้อมูลชุดเดียวกับ V1 (ต่างแค่ markup) · `/salepage`, `/home/preview` ไม่ใช้ค่านี้ (เครื่องมือเทียบดีไซน์)
- **Preview**: `PreviewMenu.cs` เปลี่ยน `CMSPage` จาก `cms` เป็น **`page`** → `/_preview/page/CMSPage/1` = หน้าแรกที่เรียงตาม `box_layout` ฉบับร่าง (SAM พรีวิวการจัดเรียงไม่ได้) · `?lang=en` เหมือน th (widget ไม่มีข้อความ 2 ภาษา) · popup ไม่เด้ง (กฎเดิม)
- **ไอคอน widget** (17 ก.ย. 2569): การ์ดในพาเลตต์ builder พื้น**น้ำเงิน** (`creator.scss` `.list-group-item`) → ไอคอน widget เป็น glyph **ขาว**บนพื้นโปร่ง (แบบ `widget_icons/white/` ของ SAM) · หัว accordion กลุ่มตอนพับพื้น**ขาว** → ไอคอนกลุ่มเป็น glyph **น้ำเงิน** (แบบ `template1.jpg` ของ SAM) · หน้า list ของเมนู Widget แสดงรูปบนพื้น `#CCC` (`AdminCore/Index.cshtml`) · สร้างด้วย ComfyUI: prompt "black pictogram on white" → invert เป็น alpha → tint (สคริปต์ `docs/comfyui-icons.py`) · **ปุ่ม Back ในหน้า builder** ต้อง override `.btn-icon` เพราะ `main.min.css` ของ front-end ที่โหลดมามี class ชื่อเดียวกัน (ปุ่มไอคอน 2.75rem) — CSS อยู่ใน `Views/CMSPage/Edit.cshtml`
- ข้อความในแต่ละ section (หัวข้อ/ปุ่ม) **ยัง hardcode ใน partial** (ผู้ใช้เลือก 18 ก.ย. 2569) — ถ้าจะให้แก้ได้ต้องเปิดเมนู `HomeSamText2–6` ทำฟอร์มใหม่แล้วผูก `mod_name` · รายการไดนามิก (NAV, กองทุนแนะนำ, ธีม, บทความ, ตัวแทนขาย) ยัง mock
- แก้บั๊ก SAM ที่ยกมาด้วย: hidden `name="box_data2"` → `box_data`, หา id ด้วย `innerHTML.substring(36,39)` → `dataset.id`, SQL ต่อ id ตรง ๆ 2 จุด → parameter, ปุ่มเลื่อนขึ้น/ลง bind ซ้ำ, `= ANY(@box)` ของ PostgreSQL ใน `PreviewMenu.cs` → `IN (...)` · `WidgetAjaxController.Manage` เปลี่ยนจาก map id → โมดูล hardcode เป็นอ่าน `pb_mod_name`
- ทดสอบ Playwright: หน้า builder **ห้ามใช้ `waitUntil: 'networkidle'`** (poll session ทุก 5 วิ) ใช้ `domcontentloaded` + รอการ์ดโหลด · ลากจากพาเลตต์ด้วย `dragTo()` ค้าง "waiting for scheduled navigations" → ใช้ synthetic `pointerdown/mousedown` + `DragEvent` (`dragstart/dragover/drop`) แทน (ดู handoff §7) · ลากบนแคนวาสใช้ `.my-handle` + `dragTo()` ได้ · ปุ่มเครื่องมือโผล่ตอน hover → `hover()` แล้ว `click({ force: true })`

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

## เว็บไซต์ front-end (เว็บสาธารณะ / public site)

Front-end คือ**คนละแอป คนละโปรเจกต์**กับ admin นี้

| แอป | โปรเจกต์ | URL (dev) |
|---|---|---|
| Admin (back-end) — repo นี้ | `d:\Project\admin.assetfund.co.th.2026\core_admin` | https://localhost:7300 |
| Public site (front-end) | `d:\Project\assetfund.co.th.2026` | https://localhost:7310 |

ทั้งสองโปรเจกต์เปิดอยู่ใน **VS Code multi-root workspace เดียวกัน** (`assetfund.co.th.admin.front.2026`)
แต่เป็น **git repo แยกกัน** — commit / สั่ง `dotnet` ต้องอยู่ในโฟลเดอร์ของโปรเจกต์นั้น ๆ

**แผนสถาปัตยกรรม (ผู้ใช้กำหนด 16 ก.ย. 2569)**: front-end จะ**อ่านข้อมูลตรงจาก SQL Server `asset_plus_uat`**
ซึ่งเป็น database เดียวกับที่ admin เขียน — admin คือระบบจัดการเนื้อหา ส่วน front-end เป็นฝั่งอ่านอย่างเดียว

```
admin (core_admin) ──เขียน──▶ SQL Server asset_plus_uat ◀──อ่าน── front-end (assetfund.co.th.2026)
                              ├ [2026_web_*]  ตารางระบบใหม่
                              └ tb_*          ตารางเดิมของ Asset Plus (ไม่มี prefix)
```

ผลต่อฝั่ง admin: **DB schema และตรรกะ Approve คือ "สัญญา" ที่ front-end พึ่งอยู่**
เปลี่ยนชื่อคอลัมน์ / ตาราง / `module_id` / วิธีตั้ง `pb_*` `status` `show_front` `Flag` เมื่อไร หน้าเว็บจะพังหรือหายเงียบ ๆ
→ ต้องตามไปแก้ query ฝั่ง front-end ด้วย (ดูตาราง "หน้าเว็บ ↔ เมนูหลังบ้าน" ด้านล่าง)

> ⚠️ **ต่อกันแล้วบางส่วน (16 ก.ย. 2569)** — front-end อ่าน SQL Server ผ่าน `Helpers/DBHelper.cs` (Microsoft.Data.SqlClient, cache ปิด `DBCacheTime = 0`)
> **เมนูที่ต่อแล้ว: `HomeImageSlide` (hero banner หน้าแรก), `HomeIntroPage` (หน้า intro `/intro-page`), `HomePopUp` (popup ประกาศบนหน้าแรก), `HomeSEO` (title/meta/โค้ดฝังของทุกหน้า), `HomeHeader` (โลโก้ + alt บน header ทุกหน้า) และ `HomeFooter` (โลโก้/ที่อยู่/ติดต่อ/โซเชียล/ปี+สโลแกน/แอป/copyright ของ footer ทุกหน้า — ไม่รวมเมนู footer 5 คอลัมน์และลิงก์นโยบาย), `CMSPage` (ลำดับ + เวอร์ชันของ 6 section บนหน้าแรก จาก page builder — 18 ก.ย. 2569)** — แก้/อนุมัติในหลังบ้านแล้วหน้าเว็บเปลี่ยนทันที และปุ่ม Preview ใช้ได้จริง
> เมนูอื่นทั้งหมดยังเป็น `Mock*Service.cs` → ข้อมูลที่แก้ผ่าน admin ยังไม่ปรากฏ ต้องยืนยันด้วย query SQL Server ตรง ๆ
> วิธีต่อเมนูถัดไปอยู่ใน CLAUDE.md ของ front-end หัวข้อ "วิธีต่อเมนูถัดไปกับ DB"

> 📌 อย่าสับสนกับ `d:\Project\sam.or.th` / `d:\Project\admin.sam.or.th` ที่อยู่ในเครื่องเดียวกัน — เป็นของลูกค้าคนละราย
> โค้ด admin ชุดนี้ port มาจากที่นั่น (ดู Architecture ด้านบน) เอกสารเก่าจึงเคยอ้างถึง เปิดดูได้เฉพาะตอนอยากเทียบว่าของเดิมทำไว้อย่างไร

### ภาพรวม front-end (ตรวจเมื่อ 16 ก.ย. 2569)

**จะแก้อะไรในโปรเจกต์ front-end ให้อ่าน `d:\Project\assetfund.co.th.2026\CLAUDE.md` ก่อนเสมอ** — กฎคนละชุดกับที่นี่:
แก้เสร็จต้อง commit + push ทันที (URL ในหัวข้อ "Git" ต้นไฟล์), ต้องทดสอบใน browser ครบ 7 ขั้น,
**ห้ามแก้ `wwwroot/css/*.css` ตรง ๆ** (compile จาก `scss/` ด้วย Web Compiler ของ Visual Studio เท่านั้น — เครื่องนี้ไม่มี sass/npm)

| หัวข้อ | Front-end | ต่างจาก admin ตรงไหน |
|---|---|---|
| Framework | ASP.NET Core **9.0** MVC · namespace `AssetFund` · `Nullable` เปิด | admin เป็น .NET **10** — แชร์โค้ด/แพ็กเกจข้ามกันต้องเช็คเวอร์ชัน |
| Routing | **attribute routing** (`[Route("funds")]` + `[HttpGet("nav")]`) — หา URL ให้ดู attribute อย่าเดาจากชื่อ controller | admin ใช้ `/Admin/{controller}/{action}` |
| แหล่งข้อมูล | `Program.cs` เป็นทะเบียนเดียว — ต่อของจริง = เขียน service ใหม่ที่ implement interface เดิมแล้ว**สลับบรรทัดเดียว** | admin ใช้ raw SQL ผ่าน `DBHelper` |
| Session / login | **ยังไม่มี session หรือ auth เลย** (`/login` เป็น OTP mock) — กฎชื่อ cookie ในหัวข้อข้อตกลงด้านล่างจึงเป็นกฎกันไว้ล่วงหน้า | — |
| ต้นทางโค้ด | copy มาจาก static web `d:\Project\assetfund.co.th.html` (พอร์ต 5301/7301) — sync ล่าสุด commit `8f1b651` | — |

⚠ **`/home/preview` ของ front-end ไม่ใช่ระบบ Preview ของ admin** — เป็นหน้าเครื่องมือภายในไว้เทียบดีไซน์หน้าแรกหลาย variant
ส่วน Preview ฉบับร่างของ admin ใช้ route `/_preview/page/{module}/{id}` (front-end มีแล้ว — โหมด `item`/`cms` ยังตอบ 404 เพราะยังไม่มีเมนูไหนใช้)

**หน้าเว็บ ↔ เมนูหลังบ้านที่น่าจะเป็นแหล่งข้อมูล** (⚠ ยัง*ไม่ได้ต่อจริง* — จับคู่จากชื่อ service/หน้า ต้องยืนยันตอนต่อ DB):

| หน้า front-end | Service | เมนู admin / ตาราง |
|---|---|---|
| `/funds`, `/funds/{code}` | `IFundService` | `ApFundCat` / `ApFund` / `ApFundDoc` / `ApFundFactSheet` (`tb_fund_cat`, `tb_fund`, `tb_fund_doc`, `tb_fund_fundfact*`) |
| `/funds/nav` + ตาราง NAV หน้าแรก | `IFundService.GetNavSummary` | `ApFundNav` (`tb_fund_nav`) |
| `/funds/performance` | `IFundService` | `ApFundPerformance` (`tb_fund_performance`, `_hd`) |
| `/funds/calendar` | `IFundService` | `ApCalendarCat` / `ApCalendar` (`tb_calendar*`) |
| `/private-fund` | `IPrivateFundService` | กลุ่มกองทุนส่วนบุคคล (`tb_fund_private*`) |
| `/provident-fund` | `IProvidentFundService` | กลุ่มกองทุนสำรองเลี้ยงชีพ (`tb_fund_prov*`) |
| `/news`, `/announcements`, `/articles`, `/events` | `IInsightService` | ยังไม่มีเมนูกำหนดชัด (น่าจะ `web_core_news` / `web_core_group`) |
| หน้าอื่น (`/about/*`, `/services/*`, `/faq`, `/careers`, `/privacy`, `/terms` …) | `Mock*Service` แยกตัว | ยังไม่มีเมนูรองรับ |

ยังไม่มีหน้าไหนใน front-end ที่ใช้ "Get Other Indices" (`tb_home_other_indices`)

⚠ **ข้อมูลเก่าใน DB ยังมี URL รูปชี้ไป `https://localhost:7140/Files/...`** (พอร์ตเดิมของ admin ก่อนย้ายมา 7300) —
`2026_web_cms_page.info`/`pb_info` 1 แถว, `2026_web_core_news` 3 แถว (`info`/`en_info` + `pb_*`) ณ 16 ก.ย. 2569
จะ 404 บน front-end จนกว่าจะแก้ข้อมูล (ใน `2026_web_admin_log` ก็มี แต่เป็นประวัติ ไม่ต้องแก้)

**ข้อตกลงระหว่างสองแอปที่ต้องรักษาไว้** (เขียนไว้ทั้งสองฝั่ง — แก้แล้วต้องตามไปแก้อีกฝั่ง):

- **รูปในเนื้อหา CMS ต้องใช้ URL แบบสัมบูรณ์ชี้มาที่โดเมน admin** (`https://localhost:7300/Files/...` หรือ `/assets/...`) เพราะไฟล์อัปโหลด (elFinder) เก็บที่ฝั่ง admin และสองแอปมี `wwwroot/Files` แยกกัน — ถ้าใช้ path สัมพัทธ์ รูปจะ 404 บน front-end (ปุ่มแทรกรูปของ elFinder ใส่ URL สัมบูรณ์ให้อัตโนมัติแล้ว)
- **ปุ่ม [Insert E-Form]** ในตัวแก้ไขเนื้อหา แทรกเป็น symbol tag `{{{E-Form:<หัวข้อ>:<id>}}}` — front-end ต้องมี parser มารับ ไม่งั้นจะแสดงเป็นข้อความดิบ (ยังไม่มี)
- **ห้ามใช้ชื่อ session cookie ซ้ำกัน** — cookie แยกตาม host เท่านั้น ไม่แยก port และทั้งสองแอปอยู่บน `localhost` เดียวกัน ถ้าชื่อชนกัน (ค่า default `.AspNetCore.Session`) การเข้า front-end จะเขียนทับ cookie ของ admin ทำให้ admin หลุด login
  → admin ตั้งเป็น `AssetPlus.Admin.Session` ไว้แล้วใน `Program.cs` **front-end ต้องใช้ชื่ออื่น** (ตอนนี้ front-end ยังไม่ได้เปิด session)
- **ระบบ Preview** ต้องมีทะเบียนคู่กัน 2 ฝั่ง — ดูสเปกที่หัวข้อ "ระบบ Preview" ด้านบน
- **front-end อ่านค่าที่อนุมัติแล้ว (`pb_*`) เท่านั้น** — ตรงกับเว็บเดิม (`assetfund.co.th.old` เช่น
  `SELECT id, pb_title, pb_en_title FROM tb_fund_cat WHERE status='1' AND show_front='1' ORDER BY sort ASC`)
  admin จึงต้องคงพฤติกรรม "แก้ไข → `pb_status=0` รออนุมัติ / Approve → copy ลง `pb_*` + `show_front=1`" ไว้
  ยกเว้นระบบ Preview ที่ตั้งใจอ่านคอลัมน์ฉบับร่าง
- **ตารางที่เมนู "Get ..." / ws_schedule เขียน** (`tb_fund_nav`, `tb_fund_performance`, `tb_home_other_indices`, `tb_fund_fundfact`)
  front-end ต้องกรอง **`Flag = 1`** (แถว `Flag = 0` คือประวัติ) — เว็บเดิมทำแบบนี้ทุกจุด
- **ไฟล์อัปโหลดของเมนู `tb_*`** เก็บเป็น**ชื่อไฟล์เปล่า** — front-end ต้องต่อ URL เอง จาก `LegacyUpload:Url` (Factsheet / ข้อมูลอื่นๆ)
  หรือ `LegacyUploadDoc:Url` (เอกสารกองทุน) · ค่า dev ตอนนี้ชี้ไปเว็บเดิม `http://localhost:8099/assetplus/upload[_otherdocs]/`

