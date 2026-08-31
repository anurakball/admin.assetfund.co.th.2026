# สเปกระบบ Preview (ดูตัวอย่างหน้าเว็บของ "ฉบับร่าง")

> แยกออกมาจาก `CLAUDE.md` เพราะเป็น**สเปกสำหรับสร้างฝั่ง front-end** ไม่ใช่คู่มือแก้โค้ดฝั่ง admin
> สรุปย่อ + กฎที่ต้องจำอยู่ใน `CLAUDE.md` หัวข้อ "ระบบ Preview"

> 🔌 **สถานะปัจจุบัน: พรีวิวยังใช้งานจริงไม่ได้ — รอฝั่ง front-end**
> ระบบนี้ต้องมีโค้ดครบ **2 ฝั่ง** แต่ตอนนี้มีแค่ฝั่ง admin
> - ฝั่ง admin: **ทำเสร็จแล้ว** (`PreviewMenu.cs`, modal, ปุ่ม) และแปลงเป็น T-SQL เรียบร้อย
> - ฝั่ง front-end: **ยังไม่ได้ทำ** — front-end ตัวใหม่ (`d:\Project\assetfund.co.th.2026`) ยังไม่มี `Helpers/PreviewMap.cs` และยังไม่มี route `/_preview/...`
>
> ของเดิมที่เคยใช้คู่กันคือเว็บ SAM (`d:\Project\sam.or.th`) ซึ่งเป็นคนละโปรเจกต์กับที่กำลังทำ
> เนื้อหาด้านล่างคือ**สเปกของฝั่ง admin ที่มีอยู่จริง** ใช้เป็นโจทย์ตอนสร้างฝั่ง front-end ได้เลย

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
| `Helpers/PreviewMap.cs` | front-end — **ยังไม่มี** | ทะเบียนเมนู + `PreviewState` (สลับ SQL เป็นคอลัมน์ฉบับร่าง) + `RowPreviewable()` (กฎระดับแถว) |
| `PreviewItem/PreviewPage/PreviewCms` | front-end — **ยังไม่มี** | route `/_preview/{item,page,cms}/{module}/{id}` |

> 2 แถวล่างคือสิ่งที่ต้องสร้างใน `d:\Project\assetfund.co.th.2026` (ดูตัวอย่างที่ทำไว้แล้วได้จาก `d:\Project\sam.or.th\Helpers\PreviewMap.cs`)

**3 โหมด**
- `item` — เมนูที่มีหน้ารายละเอียดรายตัว (News, AnnouncePro, Promotion) → เรนเดอร์ view นั้นด้วยค่าร่าง
- `page` — เนื้อหาไปโผล่เป็น section/รายการในหน้าเว็บหน้าหนึ่ง (ส่วนใหญ่) → เปิดหน้านั้นทั้งหน้าผ่าน `Index()` เดิม แต่ข้อมูลของเมนูเป้าหมายอ่านจากคอลัมน์ฉบับร่าง
- `cms` — แก้ตัวหน้า CMS เอง (CMSPage, CMSPageFooter1/2) → โหลดระเบียนตาม id ด้วยคอลัมน์ฉบับร่าง (เฉพาะ `page_type` 3 กับ 4)

**กฎระดับแถว (ปิดพรีวิวรายแถวที่ "แก้แล้วพรีวิวไม่เห็นผล")**
บางแถวหน้าเว็บ front-end ไม่ได้อ่านค่าของมันเลย พรีวิวจึงดูเหมือนแก้ไม่สำเร็จ → **ซ่อนปุ่ม** (admin: `PreviewRowGate`)
และ **ตอบ 404** (front-end: `PreviewMap.RowPreviewable()`) — กฎมาจากการกวาดทดสอบทุกแถวของทุกเมนู (423 แถว):
⚠ กฎชุดนี้ได้มาจากพฤติกรรม view ของเว็บ SAM ตัวเดิม — ตอนทำ front-end ใหม่ **ต้องทวนใหม่ทุกข้อ** ข้อไหนไม่ตรงให้ถอนออกจาก `PreviewRowGate` ด้วย
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
