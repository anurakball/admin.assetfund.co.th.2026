# Playbook: ทำ 1 เมนู CMS ให้ครบวงจร (หลังบ้าน → DB → front-end → Preview) + สเปกรายเมนู

> ย้ายมาจาก `core_admin/CLAUDE.md` เมื่อ 17 ก.ย. 2569 (ลดขนาดไฟล์ที่โหลดทุก session) — เนื้อหาเดิมทั้งหมด ไม่ได้ตัด
> **เปิดอ่านเมื่อจะทำเมนู CMS** (ผู้ใช้สั่ง "เพิ่มเมนูหลังบ้าน … / front-end ดึง … / Preview") · ฉบับย่อที่ปรับตามประสบการณ์จริงอยู่ `docs/CMS-MENU-HANDOFF.md` §6–§7 (อ่านคู่กัน — ถ้าขัดกันยึด handoff)
> path ทุกตัวสัมพัทธ์กับ `core_admin/` เหมือน CLAUDE.md


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
- ⚠ **ห้ามใส่ `<p class="cms-note">` (ย่อหน้าอธิบายเพิ่มเติมใต้ `<legend>`) ในฟอร์มหลังบ้านอีก** — ผู้ใช้สั่งตัดออกทั้งหมด 17 ก.ย. 2569 เพราะหน้าจอมีข้อความเยอะเกินไป (ลบจาก `HomeHeader/Edit.cshtml` + `HomeFooter/Edit.cshtml` แล้ว)
  ที่ยังใช้ได้ตามปกติเหมือน back-end SAM: `<fieldset class="cms-section">` + `<legend>` จัดกลุ่มช่อง, คำใบ้สั้น ๆ ในป้ายชื่อช่องแบบ `<span style="font-weight: normal;color: orange;">(.svg, .webp, .png พื้นโปร่ง ตัวอักษรสีขาว)</span>`, placeholder, ธงภาษา `fi fi-th` / `fi fi-us` ฯลฯ — คำอธิบายที่ยาวกว่านั้นให้เขียนไว้ในเอกสาร (CLAUDE.md / handoff) ไม่ใช่บนหน้าจอ

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
