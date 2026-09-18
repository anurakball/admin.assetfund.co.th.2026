# CMS-MENU-HANDOFF — เอกสารส่งต่องาน ASSET PLUS (หลังบ้าน ↔ front-end ↔ Preview)

> **▶ START HERE — session ใหม่อ่านไฟล์นี้ให้จบก่อนทำอะไรทั้งนั้น**
> อัปเดตล่าสุด: **18 ก.ย. 2569 (session 3 — เพิ่ม page builder หน้าแรก `CMSPage` + ข้อมูล Widget)** · ฉบับก่อน 17 ก.ย. 2569 (เขียนใหม่ทั้งไฟล์) · เขียนโดย Claude ตามคำสั่งผู้ใช้ก่อน context เต็ม
> ไฟล์นี้ track ใน git ของ admin (`core_admin/docs/CMS-MENU-HANDOFF.md`, un-ignore ใน `.gitignore`)
> ถ้าขัดกับ `CLAUDE.md` ของโปรเจกต์ใด **ให้ยึดไฟล์นี้** แล้วไปแก้ `CLAUDE.md` ให้ตรง
>
> ลำดับอ่าน: **§0 → §1 → §2 → §14 (งานล่าสุด: page builder + Widget — อ่านให้จบ) → §10 (เช็คลิสต์เปิด session) → §12 (แผนงานต่อไป)** แล้วค่อยเปิดหัวข้ออื่นตามงานที่ได้รับ
> รายละเอียดระดับคอลัมน์ของแต่ละเมนูอยู่ใน **`core_admin/docs/cms-menu-playbook.md`** ท้ายไฟล์ หัวข้อ "เมนูที่ทำตาม playbook แล้ว" (ย้ายจาก CLAUDE.md 17 ก.ย. 2569 · ไฟล์นี้สรุป ไม่ copy ซ้ำทุกบรรทัด)

---

## สารบัญ

0. สถานะตอนนี้ในหน้าเดียว
1. บริบทโปรเจกต์ + คนละ repo คนละพอร์ต
2. ความต้องการของผู้ใช้ทั้งหมด (ตั้งแต่แรกจนถึงตอนนี้) + วิธีทำงานที่ผู้ใช้คาดหวัง
3. ไทม์ไลน์งานที่ทำไปแล้ว (ทุกรอบ)
4. สิ่งที่ทำเสร็จ — รายเมนู (หลังบ้าน + DB + front-end + Preview)
5. งานนอกเมนู CMS ที่ทำแล้ว (Login, reCAPTCHA, Dashboard, เมนูซ้าย, Preview)
6. **ขั้นตอนทำ 1 เมนูให้ครบวงจร** (สูตรที่ใช้จริง — Phase A หลังบ้าน / B front-end / C ทดสอบร่วม)
7. **เทคนิคทดสอบ Playwright ที่ใช้ได้จริง** (โค้ดพร้อมใช้)
8. กับดักที่เจอจริง (อย่าเสียเวลาซ้ำ)
9. คำสั่ง / ค่า / ไฟล์ SQL ที่ใช้บ่อย
10. เช็คลิสต์เปิด session ใหม่
11. ไฟล์ค้าง commit + กฎ commit
12. **แผนงานต่อไป** (เรียงตามความน่าจะมา + สิ่งที่ต้องรู้ล่วงหน้า)
13. งานที่ต้องทำตอน deploy ขึ้นเซิร์ฟเวอร์จริง
14. **session 3 (18 ก.ย. 2569) — page builder หน้าแรก (`CMSPage`) + ข้อมูล Widget: รายละเอียดเต็ม, ผลทดสอบ 7 เมนู, เหตุการณ์กู้ไฟล์, สถานะเครื่อง**

---

## 0. สถานะตอนนี้ในหน้าเดียว

| เรื่อง | สถานะ ณ 17 ก.ย. 2569 |
|---|---|
| เมนู CMS ที่ทำครบวงจร (หลังบ้าน + DB + front-end + Preview) | **7 เมนู**: **`CMSPage` (จัดการ Widget — ชื่อเดิม "จัดการเมนูเว็บไซต์" = page builder หน้าแรก, 18 ก.ย.)**, `HomeIntroPage`, `HomePopUp`, `HomeHeader`, `HomeFooter`, `HomeSEO` (กลุ่ม "หน้าเว็บไซต์") และ `CMSPage` + `HomeImageSlide` (กลุ่ม "ข้อมูลหน้าแรก" — `CMSPage` เป็นเมนูแรก) + กลุ่มเมนู "Widget" (ข้อมูล widget 3 กลุ่ม × 6) |
| ปุ่ม Preview ในหลังบ้าน | ใช้ได้ **6 เมนู** (รวม `CMSPage` = ลำดับ widget ฉบับร่าง) — `HomeSEO` ถูกถอดปุ่มตามคำสั่งผู้ใช้ · **หน้าพรีวิวต้องหน้าตาเหมือนเว็บจริง 100% ห้ามมีแถบ/ป้ายสรุปใด ๆ** |
| หน้า Login หลังบ้าน | ซ่อน dropdown เว็บไซต์ (ส่ง hidden `web_id=0`) · มี **Google reCAPTCHA v2** ทั้งหน้า Login และ modal re-login · ⚠ **ใช้คีย์ทดสอบของ Google อยู่ (ผ่านทุก token = ยังไม่กันบอทจริง)** |
| Dashboard หลังบ้าน | มีชื่อผู้ใช้ + ปุ่ม Logout มุมขวาบน |
| front-end git | commit ครบ ล่าสุด **`3f4d86f`** "Render home sections in the order saved by the admin page builder" · working tree สะอาด · push ไม่ได้ (ไม่มี remote) |
| admin git | **ผู้ใช้ commit เองแล้ว `f175199` (17 ก.ย. 2569 15:56)** รวมงาน session 1–3 ทั้งหมด (AdminMenu, CMSPage builder, Widget, ReCaptcha, docs/sql ฯลฯ) · ค้างแค่ `CLAUDE.md` + `docs/CMS-MENU-HANDOFF.md` ที่แก้หลังจากนั้น · **ยังคงห้าม `git checkout --`/`restore`/`stash`** (§8 ข้อ 20) |
| ข้อมูลจริงใน DB | สะอาด ไม่มีแถวทดสอบค้าง (ยืนยัน 18 ก.ย. หลัง regression ทั้ง 7 เมนู — ทุกตารางเท่า snapshot ก่อนทดสอบ) · `web_cms_page` เหลือแถวเดียว id 1 `pb_box_layout` ล่าสุดที่ผู้ใช้จัดเอง = `wg_28,wg_29,wg_30,wg_31,wg_33,wg_32` (ตัวแทนขายก่อนบทความ) · ⚠ **Intro (id 27) และ Pop-Up (id 10, 11) มี `status = 0`** (ผู้ใช้ปิดเอง — **อย่าเปิดเองโดยไม่ถาม**) → หน้าเว็บจริงไม่มี intro และ popup |
| เซิร์ฟเวอร์ dev | admin `https://localhost:7300` (spawn หลุด job object, build ล่าสุด 18 ก.ย.) · front-end `https://localhost:7310` รันจาก **build แยกใน `%TEMP%\assetfund-7310-bin`** (ดู §9.3) · **18 ก.ย. 2569 (บ่าย): `launchSettings.json` ของ front-end สลับให้ profile `https` เป็นตัวแรก** (ผู้ใช้สั่ง) → เมื่อผู้ใช้รันจาก Visual Studio จะได้ 7310 + 5310 จาก instance เดียวของผู้ใช้ และ browser เปิด 7310 — **ห้ามฆ่า** · ถ้า 7310 เป็นของผู้ใช้ Claude ห้ามเปิด build แยกซ้อน แก้โค้ดแล้วให้ผู้ใช้ restart · ถ้าหน้า 5310/7310 ไม่ตรงกับโค้ด = ผู้ใช้ยังไม่ rebuild ให้บอกผู้ใช้ ไม่ต้องแก้โค้ด |
| งานที่น่าจะมาถัดไป | เมนู footer 5 คอลัมน์ + ลิงก์นโยบาย / ข้อความในแต่ละ section ของหน้าแรกให้แก้ได้ (`HomeSamText2–6`) / ข่าว-ประกาศ / ข้อมูลกองทุน — ดู §12 |

---

## 1. บริบทโปรเจกต์

### 1.1 โปรเจกต์ใน workspace (VS Code multi-root `assetfund.co.th.admin.front.2026`) — คนละ git repo

| โปรเจกต์ | path | เทคโนโลยี | URL dev | บทบาท |
|---|---|---|---|---|
| **admin (หลังบ้าน)** | `d:\Project\admin.assetfund.co.th.2026\core_admin` | ASP.NET Core **10** MVC, namespace `thaicredit_hr_admin` | `https://localhost:7300` (http 5300) | ระบบจัดการเนื้อหา — **เขียน** DB · login `user` / `P@ssw0rd` |
| **front-end (เว็บสาธารณะใหม่)** | `d:\Project\assetfund.co.th.2026` | ASP.NET Core **9** MVC, namespace `AssetFund`, attribute routing | `https://localhost:7310` (http 5310) | **อ่าน** DB อย่างเดียว · ไม่มี login |
| เว็บเดิม + หลังบ้านเดิม | `d:\Project\assetfund.co.th.old` | ASP.NET WebForms .NET 4 | `http://localhost:8099` (ต้องสั่งรัน IIS Express เอง) | **อ่านอย่างเดียว ห้ามพัฒนา** · ใช้เทียบพฤติกรรมเดิม · วิธีรันอยู่ใน CLAUDE.md ของโปรเจกต์นั้น |
| static web (ต้นทาง front-end) | `d:\Project\assetfund.co.th.html` | โครงเดียวกับ front-end | 5301/7301 | ฝั่งที่ทีมดีไซน์แก้หน้าตาก่อน · **copy ทับ front-end ตรง ๆ ไม่ได้แล้ว** (ดู CLAUDE.md front-end) |
| **ต้นแบบ SAM** (คนละลูกค้า) | `d:\Project\admin.sam.or.th\core_admin` + `d:\Project\sam.or.th` | admin .NET 10 + front .NET (PostgreSQL) | — | **admin ของเราพอร์ตมาจากที่นี่ทั้งชุด** · ใช้ดูว่าเมนูเดิมทำงาน/พรีวิวอย่างไร |

### 1.2 ฐานข้อมูล (สัญญาระหว่าง 2 แอป)

- SQL Server `asset_plus_uat` (`Server=.;User ID=sa;Password=sasa`) — admin เขียน / front-end อ่าน
- ตารางระบบใหม่ prefix **`2026_`** ต้องครอบ `[ ]` (`[2026_web_core_item]`) · admin ใช้ `Db.T("web_x")`, front-end ใช้ `DBHelper.T("web_x")`
- ตารางเดิมของ Asset Plus `tb_*` ไม่มี prefix (เมนู `Ap*` ใช้ร่วมกับหลังบ้านเดิม)
- compatibility level **100** → ห้าม `OPENJSON` / `STRING_SPLIT` · collation `Thai_CI_AS`
- **ทุกตารางเนื้อหามีคอลัมน์คู่**: ฉบับร่าง (`title`) ↔ ฉบับอนุมัติ (`pb_title`)

```
[หลังบ้าน] ฟอร์ม Create/Edit ──บันทึก──▶ คอลัมน์ฉบับร่าง (title, img1, ...)   pb_status = 0 (Edit ล็อกฟอร์มจนกว่าจะ Approve)
           ปุ่ม Approve      ──copy──▶  pb_title = title, ...                  pb_status = 1, show_front = 1, approve_by
           ปุ่ม Status / Move ─────────▶ status / sort (ไม่มีคู่ pb_ → มีผลกับหน้าเว็บทันที ไม่ต้อง Approve)
[front-end] หน้าจริง  อ่านเฉพาะ pb_* + gate (status/show_front/ช่วงวันที่)
            /_preview/page/<Module>/<id>?lang=th|en  อ่านคอลัมน์ฉบับร่าง (alias เป็น pb_*) ไม่กรอง gate ← ปุ่ม [Preview] เปิดใน iframe
```

### 1.3 ข้อตกลงที่ห้ามพัง

- path รูปจาก elFinder เก็บเป็น `Files/Site0/1/<โฟลเดอร์>/<ไฟล์>` (ไม่มี `/` นำหน้า) · ไฟล์อยู่ฝั่ง **admin** → front-end ต่อ `AdminURL + "/" + path`
- `wwwroot/Files/` ของ admin **ไม่ไปกับ git และ publish** → ขึ้นเซิร์ฟเวอร์ต้องอัปโหลดรูปเอง
- cookie ห้ามชื่อชนกัน (อยู่ localhost เดียวกัน) — admin ใช้ `AssetPlus.Admin.Session`, front-end ใช้ `AssetPlus.IntroSeen`, `hide_announcement`
- ทะเบียน Preview 2 ฝั่งต้อง sync เอง: admin `Areas/Admin/Helpers/PreviewMenu.cs` ↔ front-end `Helpers/PreviewMap.cs` (`PreviewMap.Pages`)
- front-end **ห้ามแก้ `wwwroot/css/*.css` ตรง ๆ** (compile จาก `scss/` ด้วย Visual Studio เท่านั้น — เครื่องนี้ไม่มี sass/npm) → style ใหม่ใส่ `<style>` ใน view

---

## 2. ความต้องการของผู้ใช้ทั้งหมด + วิธีทำงานที่ผู้ใช้คาดหวัง

### 2.1 ความต้องการหลัก (ตั้งแต่เริ่ม ยังใช้อยู่ทั้งหมด)

1. **เปิดเมนูหลังบ้านทีละเมนู** — ส่วนใหญ่คือเมนู SAM ที่ถูก `//` ซ่อนไว้ใน `AdminMenu.cs → Menu()` ให้จัดการได้เต็มรูปแบบ **ยึดโครง SAM ไม่รื้อ** เปลี่ยนแค่สี/ข้อความ/ข้อมูล
2. **ข้อมูลตัวอย่างต้องเป็นของ Asset Plus** ยกข้อความ+รูปจาก front-end (mock/hardcode เดิม) — **ห้ามเหลือเนื้อหา SAM**
3. **สิทธิ์ให้ Super Admin (`access_id = 1`) อย่างเดียว**
4. **front-end เลิก mock → อ่าน DB จริง** · `DBCacheTime = 0` ให้เห็นผลทันที
5. **ปุ่ม [Preview] ต้องใช้ได้จริง** ทุกเมนูที่ทำ (ยกเว้นที่ผู้ใช้สั่งถอด)
6. **ทดสอบละเอียดหลายเงื่อนไขใน browser ทั้ง 2 ฝั่ง** (Playwright MCP) ก่อนรายงาน — ผู้ใช้พิมพ์ทุกครั้งว่า *"ทดสอบอย่างละเอียด, ทดสอบหลาย ๆ เงื่อนไข, หลาย ๆ แบบ, ให้ครบถ้วน (ทดสอบฝั่ง browser เพิ่มเติมด้วย)"*
7. **ขั้นตอนสุดท้ายของทุกงาน = commit + push ทุกโปรเจกต์ที่มีไฟล์แก้** (ผู้ใช้สั่ง 17 ก.ย. 2569 เย็น — ยกเลิกกฎเดิม "admin ไม่ commit จนกว่าจะสั่ง") · ทั้ง 2 repo ไม่มี `origin` → `git push https://github.com/anurakball/admin.assetfund.co.th.2026.git master` (รันใน `D:\Project\admin.assetfund.co.th.2026`) และ `git push https://github.com/anurakball/assetfund.co.th.2026.git master` (รันใน `D:\Project\assetfund.co.th.2026`) · รายละเอียดหัวข้อ "Git" ต้นไฟล์ `core_admin/CLAUDE.md`
8. **ปิดงานทุกครั้งต้องอัปเดตเอกสาร**: ไฟล์นี้, `CLAUDE.md` ทั้ง 2 ฝั่ง, `docs/backend-menu-status.html` (เมื่อแตะเมนูซ้าย), `docs/preview-spec.md` (เมื่อแตะ Preview)

### 2.2 คำสั่งเฉพาะที่ผู้ใช้กำหนดระหว่างทาง (ต้องจำ)

| วันที่ | คำสั่ง | ผลที่ทำไป |
|---|---|---|
| 16 ก.ย. | สไลด์หน้าแรก: ครอบ `<a>` เฉพาะเมื่อมีทั้ง URL **และ** target | `SqlHeroSlideService` |
| 16 ก.ย. | cache = 0 | `DBCacheTime: 0` |
| 17 ก.ย. | **ยุบกลุ่มเมนู "หน้าหลัก"** ย้ายเข้า "หน้าเว็บไซต์" | ทำแล้ว |
| 17 ก.ย. | **Header**: แก้ได้แค่โลโก้ (TH/EN) + Alt Text (TH/EN) · **ลิงก์โลโก้ fix หน้าแรกเสมอ** · ไม่มีรูปใช้โลโก้เดิมเป็น default | ฟอร์มใหม่ทั้งไฟล์ |
| 17 ก.ย. | **Footer**: แก้ได้ทุกอย่างใน `footer__brand` + `footer__promo` + copyright · **ไม่รวม `footer__nav` และลิงก์นโยบายล่างสุด** ("จะทำเมนูด้านซ้ายแยกไปอีก") | ฟอร์มใหม่ทั้งไฟล์ |
| 17 ก.ย. | ไอคอนเมนู "ปรับแต่ง Header" ห้ามเป็นตัว H | `fa-solid fa-signature` |
| 17 ก.ย. | Pop-Up ต้อง**จัดเรียงได้** (front-end แสดงหลายใบเป็นสไลด์) | `CanMove = true` + `can_move = 1` |
| 17 ก.ย. | ถามเรื่องเงื่อนไขวันที่ใน query (แค่ถาม ไม่ให้แก้) | ตอบแล้ว (ดู §4 gate) |
| 17 ก.ย. | Login: **ซ่อน dropdown เว็บไซต์** แต่ยังส่ง `web_id = 0` | hidden input |
| 17 ก.ย. | Login: **reCAPTCHA v2 checkbox** ใต้ฟอร์ม · site key ใน `appsettings.json` + `.Development.json` · **ตอนนี้ตั้งแบบรันบน local ไปก่อน** | คีย์ทดสอบ Google |
| 17 ก.ย. | Dashboard: ปุ่ม **Logout มุมขวาบน** เหมือนหน้าอื่น | ทำแล้ว |
| 17 ก.ย. | LastActivity: ลบ `;` ท้ายวันที่ | ทำแล้ว |
| 17 ก.ย. | **Preview ห้ามมีแถบสรุปฉบับร่าง ทุกเมนู ทุกจุด ทุกกรณี** (ผู้ใช้ดูแล้วเข้าใจว่าเว็บแสดงผิด) — พรีวิวต้องเหมือนหน้าเว็บจริงที่สุด | ลบแถบ SEO/Header/Footer + ป้ายหน้า Intro |
| 17 ก.ย. | **SEO & Code ไม่ต้องมีปุ่ม Preview** | ถอดทั้ง 2 ฝั่ง |
| 17 ก.ย. | เพิ่มกลุ่มเมนูหลัก **"ข้อมูลหน้าแรก"** ถัดจาก "หน้าเว็บไซต์" และย้าย "รูปสไลด์หน้าแรก" ไปอยู่ในกลุ่มนี้ | ทำแล้ว |
| 17 ก.ย. (บ่าย) | ย้าย **"จัดการเมนูเว็บไซต์" (`CMSPage`)** จาก "หน้าเว็บไซต์" ไปเป็น**เมนูแรก**ของ "ข้อมูลหน้าแรก" | ทำแล้ว · บรรทัดเดิมใน "หน้าเว็บไซต์" เหลือ comment ชี้ทาง |
| 17 ก.ย. (บ่าย) | เปลี่ยนชื่อเมนู `CMSPage` จาก "จัดการเมนูเว็บไซต์" เป็น **"จัดการ Widget"** | ทำแล้ว · `Title` ใน `Menu()` + `Text` + `TextBreadcrumb = ข้อมูลหน้าแรก/จัดการ Widget` (Admin Log ใหม่ใช้ชื่อนี้ แถวเก่ายังเป็นชื่อเดิม) · อย่าสับสนกับกลุ่มเมนู "Widget" (ข้อมูล widget) ที่อยู่ท้ายเมนูซ้าย |
| 17 ก.ย. (บ่าย) | ไอคอนเมนู `CMSPage` เปลี่ยนจาก `fa-sitemap` (ของ SAM) | `fa-solid fa-layer-group` |
| 17 ก.ย. (บ่าย) | **ชื่อ + ไอคอน widget**: กลุ่มตัด "(Version n)" เหลือ `DEFAULT`/`MODERN`/`CLASSIC` · widget ตัด "(Hero) — DEFAULT" เหลือชื่อไทยล้วน · ไอคอนเปลี่ยนจาก screenshot เป็น**รูป icon สร้างด้วย ComfyUI** ทุกรายการ (เหมือน back-end SAM) | แก้ใน DB ทั้ง draft + `pb_*` (SQL ไม่ผ่าน UI จึงไม่มี Admin Log) · ไฟล์ `widget_icons/assetplus/icon-*.png` · seed อ้างอิงแก้ตาม · สคริปต์ `docs/comfyui-icons.py` |
| 17 ก.ย. (บ่าย) | ปุ่ม Back ในหน้า builder ลูกศรกับคำว่า Back ไม่ตรงกัน | CSS override `.btn-icon` ใน `Views/CMSPage/Edit.cshtml` (ชนกับ `.btn-icon` ของ front-end) |
| 18 ก.ย. | NAV หน้าแรกดึงจริงจาก `tb_fund_nav` (5 กอง, % มากสุด, ลิงก์ `/funds/nav`) · **ลิงก์ด่วน** (ไทล์ข้างตาราง NAV) ทำเมนูหลังบ้านแบบ "SAM ใส่ใจ" · ทั้งสองต้องทำงานทุกการจัดวาง widget (V1/V3 อย่างเดียว, 2, 3 ตัว) และ**ดึงข้อมูลไม่ซ้ำซ้อน** | `SqlNavPriceService` + `SqlHomeQuickTileService` · query ครั้งเดียวต่อ request เฉพาะเมื่อมี widget NAV (วัดด้วย Extended Events) · เมนู `HomeSamText` = "ข้อมูลหน้าแรก > ลิงก์ด่วน" — สเปก `docs/cms-menu-playbook.md` |
| 17 ก.ย. (เย็น) | **ห้ามใช้ `<p class="cms-note">`** (ย่อหน้าอธิบายใต้ legend) ในฟอร์มหลังบ้าน — ข้อความบนหน้าจอเยอะเกินไป · ตัดออกทั้งหมด + จดใน CLAUDE.md · ของอื่นตาม SAM (`fieldset.cms-section`, `legend`, span สีส้มในป้ายชื่อช่อง) ยังใช้ได้ | ลบ 7 ย่อหน้า + CSS ใน `HomeHeader/Edit.cshtml`, `HomeFooter/Edit.cshtml` · กฎอยู่ Playbook ขั้น 5 ของ `CLAUDE.md` |

### 2.3 วิธีทำงาน / รายงานที่ผู้ใช้คาดหวัง

- **ตอบเป็นภาษาไทย** · รายงานสั้น ตรง นำด้วยผลลัพธ์ · บอกตรง ๆ ถ้าข้ามข้อไหน/ทดสอบไม่ได้
- ทำงานยาวต่อเนื่องได้เลย **ไม่ต้องถามระหว่างทาง** ถ้าเป็นเรื่องที่มีค่าเริ่มต้นชัด (สิทธิ์ Super Admin, ข้อมูล Asset Plus, cache 0, Preview ต้องใช้ได้, ทดสอบ browser 2 ฝั่ง, commit front-end)
- ถ้าต้องขยายขอบเขตเพื่อความปลอดภัย/ความถูกต้อง (เช่นใส่ reCAPTCHA ใน modal re-login ด้วย) ให้ทำแล้ว**อธิบายเหตุผลในรายงาน**
- **ห้ามเปลี่ยนข้อมูลจริงที่ผู้ใช้ตั้งเอง** (เช่น status ของ intro/popup) — ทดสอบด้วยแถว `[TEST-…]` ที่สร้างเองแล้วลบผ่าน UI ตอนจบ
- **ห้ามฆ่า process ของผู้ใช้** (front-end ที่ผู้ใช้รันจาก Visual Studio — ตั้งแต่ 18 ก.ย. 2569 ถือทั้ง 7310 และ 5310) — ถ้า 7310 ว่างค่อยใช้ build แยก (§9.3) และปิดก่อนจบงาน
- จบงานต้องมี: สิ่งที่แก้ 2 ฝั่ง · ผลทดสอบ · ข้อมูลคืนค่าแล้ว · ไฟล์ค้าง commit ฝั่ง admin · สิ่งที่ยังไม่ทำ/ต้องทำตอน deploy

---

## 3. ไทม์ไลน์งานที่ทำไปแล้ว

| session / รอบ | งาน |
|---|---|
| ก่อนหน้า (≤14 ก.ย.) | rebrand admin จาก SAM เป็น Asset Plus · ย้าย DB PostgreSQL → SQL Server `2026_*` · พอร์ตเมนูหลังบ้านเดิม 20 ตัว (`Ap*` บนตาราง `tb_*`) + ws_schedule · ซ่อนเมนู SAM เกือบทั้งหมด |
| session 1 (16–17 ก.ย.) | วางโครงพื้นฐาน front-end อ่าน DB (`DBHelper`, `PreviewMap`, route `/_preview/page`) · ทำครบวงจร **`HomeImageSlide`**, **`HomeIntroPage`**, **`HomePopUp`**, **`HomeSEO`** · แก้บั๊ก `Db.T()` ใน `AdminHelpers` (sort) · แก้ DEFAULT constraint วันที่ · ลบ `Index.cshtml` สำเนาเก่า · ยุบกลุ่ม "หน้าหลัก" · เขียน playbook ใน `CLAUDE.md` + ไฟล์นี้ฉบับแรก |
| session 2 รอบ 1 (17 ก.ย.) | **`HomeHeader`** (ฟอร์มใหม่ + ALTER `en_img1`) และ **`HomeFooter`** (ฟอร์มใหม่ 24 ช่อง + ALTER 12 คอลัมน์) ครบวงจร · front-end commit `8f9bd07` |
| session 2 รอบ 2 | เปลี่ยนไอคอนเมนู Header เป็น `fa-signature` |
| session 2 รอบ 3 | ตอบคำถามเงื่อนไขวันที่ใน query (ไม่แก้โค้ด) · เปิด **จัดเรียง (Move) ให้ `HomePopUp`** |
| session 2 รอบ 4 | Login ซ่อน dropdown + **reCAPTCHA** (หน้า Login + modal re-login) · Dashboard ปุ่ม Logout · LastActivity ลบ `;` · **ลบแถบสรุปฉบับร่างทุกจุดใน Preview** + ป้ายหน้า Intro · ถอด Preview ของ SEO · กลุ่มเมนู **"ข้อมูลหน้าแรก"** · front-end commit `5e6a921` |
| session 2 ปิดงาน | เขียนไฟล์นี้ใหม่ทั้งหมด · เก็บ SQL จาก scratchpad เข้า `docs/sql/` |
| session 3 (18 ก.ย.) | **`CMSPage` page builder หน้าแรก** (เปิดเมนู, เหลือแถว id 1, แก้บั๊ก builder ของ SAM 5 จุด, CSS จาก FrontURL) + **ข้อมูล Widget** (ALTER `section_key`, 3 กลุ่ม × 6 widget จาก `/salepage`, เปิดกลุ่มเมนู "Widget") + front-end `SqlHomeLayoutService` + Preview `CMSPage` โหมด page + CORS ฟอนต์ · ลบข้อมูล SAM (`web_cms_page` 8–79, widget ทั้งหมด) · กู้ `AdminMenu.cs` จาก transcript หลังพลาด `git checkout` |

---

## 4. สิ่งที่ทำเสร็จ — รายเมนู

### 4.1 ตารางสรุป

| กลุ่ม > เมนู | Module | ตาราง | สิทธิ์/ปุ่ม (`Can*`) | ข้อมูลจริงตอนนี้ | front-end (service → view) | Preview |
|---|---|---|---|---|---|---|
| หน้าเว็บไซต์ > หน้า Intro Page | `HomeIntroPage` | `[2026_web_home_intro_page]` ตารางเดี่ยว | Add/Edit/Delete/Status/Approve (ไม่มี Move) | id 27 "ประกาศแจ้งเตือนภัย" รูป `Files/Site0/1/intro_page/scam-alert.jpg` · **status 0 (ปิดอยู่)** | `SqlIntroPageService` → route `/intro-page` → `Views/Home/Intro.cshtml` + `_LayoutIntro` | `BoxDataIntro` เรนเดอร์ Intro ด้วยแถวนั้น |
| หน้าเว็บไซต์ > หน้า Pop-Up | `HomePopUp` | `[2026_web_home_pop_up]` ตารางเดี่ยว | Add/Edit/Delete/**Move**/Status/Approve | id 10 ประกาศเตือนภัย → `/news-announcements` (sort 10), id 11 ติดตาม Facebook → `_blank` (sort 20) · **ทั้งคู่ status 0** | `SqlPopupBannerService` → modal `#announcementModal` (`Views/Home/Partials/_PopupAnnouncement.cshtml`) สไลด์ Swiper ไม่วนลูป สูงสุด 10 ใบ | หน้าแรก + เด้ง popup เปิดที่ใบนั้น ไม่สนคุกกี้ hide |
| หน้าเว็บไซต์ > ปรับแต่ง Header | `HomeHeader` | `[2026_web_home_header]` แถวเดียว id 1 **+`en_img1`** | Edit/Approve | โลโก้ `Files/Site0/1/header/logo-dark.svg` (TH=EN) · Alt TH "บริษัทหลักทรัพย์จัดการกองทุน แอสเซท พลัส จำกัด" / EN "Asset Plus Fund Management" | `SqlSiteHeaderService` → `@inject` ใน `Views/Shared/_PartialHeader.cshtml` + `_PartialHeaderSale.cshtml` | หน้าแรก (โลโก้เป็นฉบับร่าง) |
| หน้าเว็บไซต์ > ปรับแต่ง Footer | `HomeFooter` | `[2026_web_home_footer]` แถวเดียว id 1 **+12 คอลัมน์** | Edit/Approve | โลโก้ `Files/Site0/1/footer/logo-light.svg`, ที่อยู่/โทร 02 672 1111/อีเมล/FB/YT/Blockdit (LINE ว่าง), ปี 2026, สโลแกน, หัวข้อแอป, QR `footer/icon-app.png`, copyright | `SqlSiteFooterService` → `@inject` ใน `_PartialFooter.cshtml` + `_PartialFooterSale.cshtml` | หน้าแรก (footer เป็นฉบับร่าง) |
| หน้าเว็บไซต์ > SEO & Code | `HomeSEO` | `[2026_web_home_seo]` แถวเดียว id 1 | Edit/Approve (ฟอร์มไม่ล็อกตอนรออนุมัติ — ของ SAM) | title/description/keywords TH-EN · โค้ดฝังว่าง | `SqlSiteSeoService` → `Views/Shared/_SeoHead.cshtml` + `_SeoBody.cshtml` ในทุก layout | **ไม่มี** (ถอดแล้ว → 404) |
| หน้าเว็บไซต์ > Get Other Indices | `ApOtherIndices` | `tb_home_other_indices` | เมนูเดิม Asset Plus | ไม่ได้แตะ | front-end ยังไม่ใช้ | — |
| ข้อมูลหน้าแรก > จัดการ Widget | `CMSPage` | `[2026_web_cms_page]` **แถวเดียว id 1** (`is_home = 1`) + `[2026_web_widget_group]` 3 + `[2026_web_widget]` 18 (`section_key`) | Edit/Approve เท่านั้น (`CanAdd/Delete/Status/Move = false`) · กลุ่ม "Widget" = CRUD เต็ม | `box_layout = pb_box_layout = wg_28…wg_33` (Version 1) · widget id 28–45 (cat 4/5/6 = DEFAULT/MODERN/CLASSIC) | `SqlHomeLayoutService` → `Index.cshtml` วน partial `_<section_key>` | หน้าแรกเรียงตาม `box_layout` ฉบับร่าง (`page` mode) |
| **ข้อมูลหน้าแรก** > รูปสไลด์หน้าแรก | `HomeImageSlide` | `[2026_web_core_item]` `module_id = 1` | Add/Edit/Delete/Move/Status(+ปักหมุด)/Approve | id 163/164/165 = ASP-DEFENSE, A-HUMANOID, A-ASEMI · รูป `Files/Site0/1/home/hero-*.jpg` (PC+Mobile) | `SqlHeroSlideService` → `Views/Home/Partials/_Hero.cshtml` | หน้าแรก เปิดที่สไลด์นั้น (`data-initial-slide`) |

### 4.2 เงื่อนไขแสดงผล (gate) ที่ front-end ใช้

| เมนู | WHERE | ORDER BY |
|---|---|---|
| รูปสไลด์ | `module_id = 1 AND web_id = 0 AND status > 0 AND show_front = 1 AND (pb_issue_date_config = 1 OR (pb_issue_date < now AND pb_expiry_date > now))` | `status DESC, sort ASC, id ASC` (ปักหมุด status 2 ขึ้นก่อน) |
| Intro | `web_id = 0 AND status = 1 AND show_front = 1 AND (pb_issue_date_config = '1' OR (pb_issue_date <= now AND pb_expiry_date >= now))` | `id DESC` เอาแถวเดียว |
| Pop-Up | `web_id = 0 AND status = 1 AND show_front = 1 AND (pb_issue_date_config = '1' OR (< now AND > now))` | `sort ASC, id ASC` สูงสุด 10 |
| SEO / Header / Footer | `web_id = 0` **ไม่มี gate status/วันที่** (ค่าตั้งค่าของเว็บ) | `id ASC` เอาแถวแรก |

- `now` = `SYSDATETIMEOFFSET()` ฝั่ง DB (อย่า format วันที่จาก C# เพราะ culture th-TH เป็น พ.ศ.)
- `issue_date_config`: 1 = แสดงตลอด, 2 = ตามช่วงวันที่ · ตาราง `web_home_*` เป็น **nvarchar** (เทียบ `'1'`)
- ข้อมูลจริงทุกแถวตอนนี้ตั้ง config = 1 (ไม่มีแถวไหนถูกตัดเพราะวันที่)
- โหมด Preview: `preview.Gate()` คืน `""` (เห็นแถวที่ปิด/หมดอายุด้วย) และ `preview.RowFilter()` บังคับแถวที่พรีวิวสำหรับเมนูแถวเดียว

### 4.3 เมนูแบบ "ออกแบบฟอร์มใหม่" (Header / Footer) — ต่างจากเมนูอื่นอย่างไร

ฟอร์ม SAM ไม่ตรงกับเว็บ Asset Plus (SAM Header = เลือก Template + โทนสี, SAM Footer = Call Center + รูป + ปุ่ม 3 ปุ่ม) ผู้ใช้จึงบอกจุดที่แก้ได้บนหน้าเว็บ แล้วเรา**เขียน `Edit.cshtml` ใหม่ทั้งไฟล์** แต่ยังใช้เครื่องยนต์เดิม (`AdminCoreController` + Approve เดิม):

- `AdminMenu.cs` มี `Field_HomeHeader` / `Field_HomeFooter` (ใช้เป็น FieldCreate/Update/Approve ชุดเดียว) — **ทุก `name` ในฟอร์มต้องอยู่ในลิสต์ และห้ามใส่ฟิลด์ที่ฟอร์มไม่มี** (จะถูกเขียนทับเป็น NULL)
- **ALTER TABLE เพิ่มคอลัมน์ (DB dev ทำแล้ว)** — script idempotent: `core_admin/docs/sql/2026-09-17-web-home-header-footer.sql`
  - header: `en_img1` (+`pb_`)
  - footer: `img1, en_img1, address, en_address, address_url, sc_bd, promo_year, tagline_1, tagline_2, app_title, en_app_title, app_qr` (+`pb_` ทุกตัว)
- คอลัมน์ SAM ที่เลิกใช้ตั้ง NULL แล้ว: header `this_type`, `des` · footer `sc_tw`, `sc_tt`, `info`/`en_info`, `btn1..3_*`
- ลบกิ่ง `else if (Module.Name == "HomeHeader")` + `BuildMicrositeBoxLayout()` ออกจาก `AdminCoreController.Edit` (เขียน `box_layout` ตาม Template — ไม่ใช้แล้ว)
- ฝั่ง front-end มีค่า fallback เท่ากับของเดิมที่เคย hardcode: `Models/SiteHeader.cs` / `Models/SiteFooter.cs` (`Default`) → DB ล้ม/ไม่มีแถว หน้าเว็บยังเหมือนเดิม
- กฎการแสดงผล Footer: ช่องว่าง = ซ่อน element นั้น · ปีว่าง = ปี ค.ศ. ปัจจุบัน · `tel:` คำนวณเป็น `+66…` · URL ต้องขึ้นต้น http(s) ไม่งั้นไม่ทำลิงก์ · ปุ่มสโตร์ไม่มี URL = `<span>` · textarea ที่อยู่ขึ้นบรรทัดใหม่ = `<br>` · `/salepage` ใช้ `_PartialFooterSale` (ไม่มีปี/สโลแกน/เมนู)
- ฟอร์ม Footer ตรวจ `^https?://` ทุกช่อง URL ใน `sConfirmCustom()` · อีเมลเป็น `<input type=email>` · list ใช้ `ListData_Default` (โลโก้ขาวบนพื้นขาวมองไม่เห็น) · thumbnail โลโก้ในฟอร์มพื้น `#00295A`

### 4.4 โครงสร้างพื้นฐานที่มีแล้ว (เมนูถัดไปใช้ซ้ำ)

**front-end** (`d:\Project\assetfund.co.th.2026`)

| ไฟล์ | หน้าที่ |
|---|---|
| `Helpers/DBHelper.cs` | `_db.q(sql, params)` → `List<Dictionary<string, object?>>` · `DBHelper.T()` · DB ล้มคืนลิสต์ว่าง + log · cache ตาม `DBCacheTime` (0) · ไม่ cache request พรีวิว |
| `Helpers/PreviewMap.cs` | `PreviewState` (ใน `HttpContext.Items`): `Cols()` alias ฉบับร่าง → `pb_*`, `Gate()`, `RowFilter()`, `HomeSlideGate`, `DefaultGate` · `PreviewMap.Pages` **5 เมนู** · `NoModule = 0` · `BoxDataHome` / `BoxDataIntro` · `RowExists()` |
| `Controllers/HomeController.cs` | `Index()` (intro redirect) · `IntroPage()` · **`PreviewPage()`** route `/_preview/page/{module}/{id:long}` → ตรวจทะเบียน + `RowExists` → headers (`X-Robots-Tag`, `no-store`, `CSP frame-ancestors 'self' <AdminURL>`) → `preview.Set()` → `switch (kind.BoxData)` · `/_preview/item|cms/...` → 404 |
| `Services/Sql*Service.cs` (6 ตัว) | แม่แบบ: `SqlHeroSlideService` (หลายแถว+module_id+ปักหมุด), `SqlIntroPageService` (แถวเดียว+gate+RowFilter), `SqlPopupBannerService` (หลายแถว ตารางเดี่ยว), `SqlSiteSeoService` / `SqlSiteHeaderService` / `SqlSiteFooterService` (แถวเดียว ไม่มี gate) |
| `Views/Shared/_PartialHeader*`, `_PartialFooter*`, `_SeoHead`, `_SeoBody` | `@inject I…Service` ในตัว partial + อ่าน `ViewBag.PreviewLang` — layout เรนเดอร์หลัง action จึงได้ฉบับร่างเองโดยไม่ต้องแก้ `PreviewPage()` |
| `Program.cs` | ทะเบียน service บรรทัดเดียวต่อ interface (มีคอมเมนต์ไทย) · สลับ Mock → Sql ที่นี่ |
| `appsettings*.json` | `ConnectionStrings:DBConnection` · `AdminURL` (dev `https://localhost:7300`) · `DBCacheTime: 0` |

**admin** (`core_admin`)

| ไฟล์ | หน้าที่ |
|---|---|
| `Areas/Admin/Helpers/AdminMenu.cs` | `Menu()` = เมนูซ้าย (บรรทัด `//` = ซ่อน) · `AllModule()` = `ModuleConfig` ทุกเมนู · `Field_HomeHeader/Footer` |
| `Areas/Admin/Controllers/AdminCoreController.cs` | เครื่องยนต์ Index/Create/Edit/Delete/Status/Approve/Move ของ `2026_web_*` |
| `Areas/Admin/Views/AdminCore/Index.cshtml` | หน้า list กลางทุกเมนู (ปุ่ม Preview หน้าคอลัมน์แรกของ `ListData`, ปุ่มจัดเรียง, ฯลฯ) |
| `Areas/Admin/Helpers/PreviewMenu.cs` | ทะเบียนเมนูที่มีปุ่ม Preview (ของ SAM 64 ตัว — `HomeSEO` comment แล้ว) |
| `Areas/Admin/Helpers/AdminHelpers.cs` | `InputText`, `InputTextArea`, `InputFileManeger` (elFinder), `InputTextLinkCMSPage`, `InputUrlTarget`, `InputDateTime`, `InputEmail`… · `getSort()` / `reSort()` |
| `Areas/Admin/Helpers/ReCaptcha.cs` | ตรวจ reCAPTCHA กับ Google (ใหม่ รอบ 4) |
| `docs/backend-menu-status.html` | รายงานเมนูเปิด 32 / ปิด 147 — **อัปเดตทุกครั้งที่แตะเมนูซ้าย** |
| `docs/preview-spec.md` | สเปก Preview · `docs/sql/*.sql` สคริปต์ DB (§9.4) |

---

## 5. งานนอกเมนู CMS ที่ทำแล้ว (session 2 รอบ 4)

### 5.1 หน้า Login (`core_admin/Views/Login/Index.cshtml` + `UserController.Login`)

- **dropdown เลือกเว็บไซต์ถูกเอาออก** → `<input type="hidden" id="id_web_id" name="web_id" value="0">` · CSS `.select-webid` ลบแล้ว
- **Google reCAPTCHA v2 checkbox** อยู่ใต้ช่องรหัสผ่าน เหนือปุ่ม Log In · สคริปต์ `https://www.google.com/recaptcha/api.js?hl=th`
  - client: กด Log In โดยไม่ติ๊ก → ข้อความแดง `#recaptcha_error` ไม่ส่งฟอร์ม · หมดอายุ → ข้อความให้ติ๊กใหม่ · จอ ≤360px ย่อกล่อง 0.88
  - server: `UserController.Login [HttpPost]` เรียก `ReCaptcha.Verify()` **ก่อนเช็ก username/password** · ไม่ผ่าน → Admin Log `login_fail_captcha` + ข้อความเตือน (หน้า Login) หรือตอบ `captcha` (modal)
  - **fail closed**: ไม่มีคีย์ / ติดต่อ Google ไม่ได้ = ไม่ให้เข้า (หน้า Login ขึ้นกล่องแดงถ้าไม่มี SiteKey)
- config `GoogleReCaptcha:SiteKey` / `:SecretKey` / `:VerifyUrl` อยู่ใน `appsettings.json`, `appsettings.Development.json` (gitignore) และ `appsettings.Development.json.example`
  - ⚠ **ตอนนี้ทุกไฟล์เป็นคีย์ทดสอบของ Google**: site `6LeIxAcTAAAAAJcZVRqyHh71UMIEGNQ_MXjiZKhI` / secret `6LeIxAcTAAAAAGG-vFI1TnRWxMZNFuojJ4WifJWe` — ใช้บน localhost ได้, กล่องมีข้อความแดง "for testing purposes only", **Google ตอบผ่านทุก token (ทดสอบแล้ว token ปลอมก็ผ่าน)**
- ⚠ ไฟล์ `Views/Login/Index.cshtml` มีการแก้ของ**ผู้ใช้เอง**ค้างมาก่อน (สุ่มพื้นหลัง `Random.Shared.Next(0, 10)` → bg0–bg9) ปนกับของ Claude

### 5.2 modal re-login (`Areas/Admin/Views/Shared/_PartialAdminMenu.cshtml`)

- modal "คุณหลุดออกจากระบบ กรุณา Login อีกครั้ง" เด้งจาก `callApi()` (poll `/Admin/User/AjaxCheckSession` ทุก 5 วิ) และ POST ไป `/Admin/User/Login` พร้อม `relogin=1` — **endpoint เดียวกับหน้า Login จึงต้องมี reCAPTCHA ด้วย** (ไม่งั้นบอทยิงผ่านช่องนี้ได้)
- `ensureReloginCaptcha()` โหลดสคริปต์ Google แบบ `render=explicit` **ตอน modal เด้งครั้งแรกเท่านั้น** (หน้าหลังบ้านปกติไม่โหลด) → `grecaptcha.render('#relogin_recaptcha')`
- ไม่ติ๊ก → ข้อความใน modal ไม่ส่ง · server ตอบ `captcha` → ข้อความ · `0` → alert เดิม · ทุกผลลัพธ์ `grecaptcha.reset()` (token ใช้ครั้งเดียว)

### 5.3 Dashboard (`Areas/Admin/Views/User/Dashboard.cshtml`)

- layout `_LayoutIntro` ไม่มี header หลังบ้าน → เพิ่ม `.dashboard-userbar` (absolute มุมขวาบน): ไอคอน+ชื่อ (`admin_name admin_surname` จาก session) + ปุ่ม **Logout**
- ปุ่มเรียก `dashboardLogOut()` → SweetAlert "ออกจากระบบ ?" (ใช่/ไม่ใช่) → submit ฟอร์มซ่อน POST `/Admin/User/Logout` พร้อม `@Html.AntiForgeryToken()` · มือถือ <576px ซ่อนชื่อ
- โหลด `~/js/sweetalert2.min.js` ใน `@section Scripts`

### 5.4 อื่น ๆ

- `Areas/Admin/Views/User/LastActivity.cshtml`: ลบ `;` ท้ายคอลัมน์ Action Date
- **Preview สะอาด 100%**: ลบแถบสรุป `data-seo-preview` / `data-header-preview` / `data-footer-preview` ออกจาก front-end `Views/Home/Index.cshtml` (+ `@inject` ที่ไม่ใช้แล้ว) และป้าย `.intro__preview-badge` จาก `Views/Home/Intro.cshtml` · ทดสอบ diff `<body>` หน้าพรีวิว vs หน้าจริง = 0 บรรทัด
  - ข้อยกเว้นเดียวที่ยังมี (ตั้งใจ): popup ประกาศ**ไม่เด้ง**ตอนพรีวิวเมนูอื่นของหน้าแรก (กันบังสิ่งที่กำลังดู) — ถ้าผู้ใช้ขอให้เหมือนเว็บจริง 100% ต้องแก้ `Views/Home/Index.cshtml` เงื่อนไข `ViewBag.PreviewModule == "HomePopUp"`
- **ถอด Preview ของ `HomeSEO`**: admin `PreviewMenu.cs` comment · front-end `PreviewMap.Pages` เอาออก
- **เมนูซ้าย**: เปิดกลุ่ม `#region ข้อมูลหน้าแรก` (`fa-solid fa-book-open`) ถัดจาก "หน้าเว็บไซต์" · ย้าย `HomeImageSlide` เข้ากลุ่มนี้ · `TextBreadcrumb = "ข้อมูลหน้าแรก/รูปสไลด์หน้าแรก"` · เมนู SAM อื่นในกลุ่ม (`HomeImageConf`, `HomeSamText1–7`) ยัง `//`

### 5.5 เมนูซ้ายตอนนี้ (ตรงกับหน้าจอจริง)

```
Dashboard
หน้าเว็บไซต์ (fa-solid fa-globe)
   หน้า Intro Page (HomeIntroPage)              fa-regular fa-window-restore
   หน้า Pop-Up (HomePopUp)                      fa-solid fa-clone
   ปรับแต่ง Header (HomeHeader)                 fa-solid fa-signature
   //จัดการเมนู Footer 1 (CMSPageFooter1)        ← ยังไม่เปิด
   //จัดการเมนู Footer 2 (CMSPageFooter2)        ← ยังไม่เปิด
   ปรับแต่ง Footer (HomeFooter)                 fa-regular fa-copyright
   SEO & Code (HomeSEO)                         fa-solid fa-sliders (ไม่มีปุ่ม Preview)
   Get Other Indices (ApOtherIndices)           เมนู tb_*
ข้อมูลหน้าแรก (fa-solid fa-book-open)
   จัดการ Widget (CMSPage)                      fa-solid fa-layer-group  ← page builder หน้าแรก list 1 แถว → Edit/1 = แท็บ "ตกแต่งเพจ" · ย้ายมาจาก "หน้าเว็บไซต์" 17 ก.ย. (ผู้ใช้สั่งให้อยู่ลำดับแรก)
   รูปสไลด์หน้าแรก (HomeImageSlide)            fa-regular fa-images
   //ตั้งค่ารูปสไลด์ (HomeImageConf), //HomeSamText…HomeSamText7   ← เนื้อหา SAM ยังไม่เปิด
ข้อมูลกองทุน / กองทุนส่วนบุคคล / ปฏิทินกองทุน / กองทุนสำรองเลี้ยงชีพ   ← เมนู Ap* (AdminMenuAssetPlus.cs) ไม่ได้แตะ
ผู้ดูแลระบบ (สิทธิ์การใช้, ผู้ดูแลระบบ, Admin Logs, ผู้ใช้ที่ไม่เข้าใช้งาน)
Widget (bi bi-collection)                          ← เปิด 18 ก.ย. (ผู้ใช้สั่งให้อยู่ถัดจากผู้ดูแลระบบ) · Widget2/WidgetGroup2 ยัง comment
   กลุ่ม Widget (WidgetGroup)                    3 แถว = DEFAULT / MODERN / CLASSIC (= Version 1/2/3 ของ /salepage — ตัด "(Version n)" ออก 17 ก.ย.)
   Widget ทั้งหมด (Widget)                       18 แถว = 6 section × 3 เวอร์ชัน · ฟอร์มมี Section Key + Mod Name + HTML Code
PERSONAL INFORMATION: View profile, Change Password, Last Activity
```
กลุ่ม SAM อื่นที่ยัง `//` ทั้งกลุ่ม: เนื้อหาหน้าแรก (microsite), เกี่ยวกับเรา, ข่าว/ประกาศ, ทรัพย์ NPA, ฯลฯ — ดู `docs/backend-menu-status.html`

---

## 6. ขั้นตอนทำ 1 เมนูให้ครบวงจร (สูตรที่ใช้จริง)

> ฉบับเต็ม 19 ขั้น + สเปกรายเมนูอยู่ใน **`core_admin/docs/cms-menu-playbook.md`** (ย้ายออกจาก CLAUDE.md 17 ก.ย. 2569 เพื่อลดขนาดไฟล์ที่โหลดทุก session) — ส่วนนี้คือฉบับที่ปรับตามประสบการณ์ 6 เมนู
> คำสั่งผู้ใช้แบบปกติ: *"เพิ่มเมนูหลังบ้าน `<กลุ่ม> >> <เมนู>` จนถึง front-end + Preview ครบ … ทดสอบละเอียดทั้ง 2 ฝั่ง … ปิดงานด้วยอัปเดต handoff, CLAUDE.md 2 ฝั่ง, commit front-end"*

### 6.0 ตัดสินใจก่อนเริ่ม

| คำถาม | ถ้าใช่ |
|---|---|
| เมนูนี้มีใน SAM (ถูก `//` ใน `Menu()`)? | ประเภท A — เปิดคืน ใช้ของเดิม |
| ฟอร์ม SAM ตรงกับสิ่งที่หน้าเว็บ Asset Plus ต้องแก้ได้? | ใช้ฟอร์มเดิม เปลี่ยนสี/ข้อความ · **ไม่ตรง → ถามผู้ใช้ว่าจุดไหนบนหน้าเว็บต้องแก้ได้ แล้วเขียนฟอร์มใหม่แบบ Header/Footer (§4.3)** |
| ต้องมีคอลัมน์ที่ตารางไม่มี? | ALTER TABLE (+`pb_`) → เขียน script idempotent ลง `docs/sql/` |
| หน้าเว็บที่แสดงคือหน้าแรก? | Preview `BoxDataHome` · หน้าอื่นต้องเพิ่ม `case` ใน `PreviewPage()` |
| ค่าอยู่ใน layout (header/footer/head)? | `@inject` service ใน partial — ไม่ต้องแก้ controller |
| เนื้อหาหลายแถว + front-end เป็นสไลด์/รายการ? | เปิด `CanMove` + `can_move = 1` |
| เมนูใช้ตาราง `tb_*`? | ใช้ `AdminLegacyController` (ดู CLAUDE.md admin หัวข้อ "เมนูที่พอร์ตมาจากหลังบ้านเดิม") |

### Phase A — หลังบ้าน (`core_admin`)

1. **สำรวจ**
   ```bash
   cd /D/Project/admin.assetfund.co.th.2026/core_admin
   grep -rn "<Module>" --include=*.cs --include=*.cshtml --include=*.js Areas wwwroot/js
   ls Areas/Admin/Views/<Module>/          # มี Index.cshtml ของตัวเอง = สำเนาเก่า → git rm (ทำกับ 5 เมนูแล้ว)
   diff --strip-trailing-cr ../../admin.sam.or.th/core_admin/Areas/Admin/Views/<Module>/Edit.cshtml Areas/Admin/Views/<Module>/Edit.cshtml
   ```
   ดูฝั่ง SAM ว่า front-end อ่านอย่างไร: `grep -rn "<table>\|<Module>" d:/Project/sam.or.th --include=*.cs --include=*.cshtml` (`Controllers/HomeController.cs`, `Services/layoutContentService.cs`, `Helpers/PreviewMap.cs`)
2. **ดู front-end ว่าจุดที่จะต่ออยู่ไหน** (อ่าน markup จริงก่อนออกแบบฟอร์ม): `Views/Shared/_Partial*.cshtml`, `Views/Home/Partials/*`, `Services/Mock*Service.cs`, `Controllers/HomeController.BuildHomeViewModel()`
3. **เมนูซ้าย** `AdminMenu.cs → Menu()`: ลบ `//` หรือย้ายบรรทัดเข้ากลุ่มที่ผู้ใช้ระบุ (บรรทัดเดิมให้เหลือ comment ชี้ทาง) · กลุ่มใหม่ = `listMenu.Add(new() { Title, Icon, SubMenu = new() { … } })` ครอบ `#region` · ไอคอน Font Awesome (หลังบ้านโหลด FA7 — ตรวจชื่อด้วย `grep -c "\.fa-<name>\b" wwwroot/assets/fonts/fontawesome7/css/all.min.css`) · **อย่าใช้ `fa-window-maximize`** (เป็นไอคอนสำรองของระบบ)
4. **`AllModule()`**: `TextBreadcrumb = "<กลุ่ม>/<เมนู>"` (ใช้ใน Admin Log ด้วย) · `Table` ชื่อตรรกะ · `TableModuleID` · `ListData` (คอลัมน์แรก = ปุ่ม Preview) · `Field*` ตรงกับฟอร์ม · `Can*`
5. **ฟอร์ม** `Views/<Module>/Edit.cshtml` (+`Create.cshtml` ถ้า `CanAdd`): helper `_admin.Input*("label", "<คอลัมน์>", value)` · สีปุ่มยืนยัน `#00295A` · validation ใน `sConfirmCustom(form)` · `checkLockedRow(modConfig, jsonRow)` · ใน JS ของ Razor ต้องเขียน `@@` แทน `@`
6. **DB** (เขียน SQL ลง scratchpad รันด้วย `sqlcmd … -b -f 65001 -i "$(cygpath -w file.sql)"`)
   - สำรองแถวเดิมก่อน: `select * from [2026_x] > scratchpad/backup_x.txt`
   - ALTER (ถ้าต้องมี) คั่นด้วย `GO` ก่อน UPDATE คอลัมน์ใหม่
   - แทนข้อมูล SAM ด้วยของ Asset Plus: ใส่ทั้ง `x` และ `pb_x` เท่ากัน + `status=1, pb_status=1, show_front=1, web_id=0` (+`module_id`, `sort` 10,20,30)
   - รูป: copy จาก front-end `wwwroot/media/...` → `core_admin/wwwroot/Files/Site0/1/<โฟลเดอร์>/` เก็บ path `Files/Site0/1/<โฟลเดอร์>/<ไฟล์>`
   - สิทธิ์: `select * from [2026_web_admin_module] where mod_name='<Module>'` · ให้ `can_*` ตรง `Can*` (เมนู SAM มีแถว access_id 1 อยู่แล้ว ส่วนใหญ่ต้อง update)
   - เก็บ seed ถาวร: copy ลง `docs/sql/seed-ref-<module>.sql` (scratchpad หายได้)
7. **Build + restart admin** (§9.2) — แก้แค่ `.cshtml` ไม่ต้องรีสตาร์ท
8. **ทดสอบหลังบ้าน** (§7): เมนู/active/breadcrumb · list + thumbnail · ค้นหา ไทย/อังกฤษ/ไม่พบ/`' OR 1=1 --`/Clear · Create/Edit/validation · elFinder · ลบรูป · Approve (ตรวจ DB ทุก `pb_*`) · Status · Move (ถ้ามี) · Delete (ยกเลิก+ยืนยัน) · AdminAccess checkbox · ไม่ login → Login · Admin Log · มือถือ 390px

### Phase B — front-end (`d:\Project\assetfund.co.th.2026`)

9. **model**: `Models/<X>.cs` รองรับทุกฟิลด์ที่ฟอร์มมี + `static Default` = ค่าที่ hardcode เดิม (ถ้าเป็นเนื้อหาที่ต้องมีเสมอ)
10. **interface + service**: `Services/I<X>Service.cs` + `Services/Sql<X>Service.cs` ลอกแม่แบบที่ใกล้สุด (§4.4)
    ```csharp
    var preview = PreviewState.For(_ctx.HttpContext);
    var rows = _db.q(
        $"SELECT TOP n id, status, {preview.Cols(Table, ModuleId, "pb_title, pb_en_title, pb_img1, …")} " +
        $"FROM {DBHelper.T(Table)} WHERE web_id = 0 [AND module_id = {ModuleId}] " +
        preview.Gate(Table, ModuleId, <GATE>) +          // ไม่มี gate ก็ไม่ต้องใส่
        preview.RowFilter(Table, ModuleId) +             // เมนูที่แสดงแถวเดียว
        "ORDER BY …");
    ```
    กฎ: อ่านแต่ `pb_*` (ยกเว้น `id/status/sort`) · gate ผ่าน `preview.Gate()` เท่านั้น · ตารางเดี่ยวใช้ `PreviewMap.NoModule` · รูป `AdminURL + "/" + path` (URL เต็มใช้ตามนั้น) · ภาษา `pb_en_x` ว่าง → ไทย · แถวข้อมูลไม่พอ → ข้าม · วันที่ใช้ `SYSDATETIMEOFFSET()`
11. **ลงทะเบียน**: `Program.cs` `builder.Services.AddSingleton<I<X>Service, Sql<X>Service>();` (คอมเมนต์ไทยบอกเมนูหลังบ้าน) · `Helpers/PreviewMap.cs` → `PreviewMap.Pages` เพิ่ม `new("<Module>", "<table>", <moduleId|NoModule>, BoxDataHome)` (ชื่อ Module ตรงกับ admin ทุกตัวอักษร) · ถ้าผู้ใช้ไม่ต้องการ Preview ให้ comment ใน `PreviewMenu.cs` ฝั่ง admin ด้วย
12. **view**: แทนค่า hardcode ด้วย model · ใส่ `data-<x>-*` attribute ไว้ทดสอบ · ครอบ `<a>` เฉพาะเมื่อมี URL · `_blank` → `rel="noopener"` · โหมดพรีวิวใช้ได้แค่ "พาไปที่แถว" (`data-initial-slide`) — **ห้ามเพิ่มแถบ/ป้าย/กล่องสรุปใด ๆ**
13. **build + รัน front-end 7310** (§9.3) — แก้ view ต้อง rebuild ทุกครั้ง
14. **ทดสอบ front-end** (§7): หน้าจริงตรง `pb_*` · รูปโหลด (`naturalWidth>0`) · ลิงก์/target/rel · 1440 และ 390px ไม่ล้น · console/network ไม่มี error ใหม่ (favicon 404 ของเดิม) · ฉบับร่าง ≠ หน้าจริง / = พรีวิว (th+en) · gate (ปิด/หมดอายุ/รูปว่าง) · header พรีวิว · 404 (module มั่ว, id มั่ว, `abc`, `/_preview/item|cms`) · regression `/funds`, `/about`, `/salepage`, `/home/preview`, path มั่ว · **diff `<body>` พรีวิว vs หน้าจริงต้อง 0 บรรทัด**

### Phase C — ทดสอบร่วมผ่าน UI + ปิดงาน

15. แก้ไขผ่านหลังบ้าน → หน้าจริงยังเดิม → กด [Preview] ใน list → iframe เห็นค่าใหม่ → สลับ "อังกฤษ" → Esc ปิด → Approve → หน้าจริงเปลี่ยน → Status/Move → หน้าจริงตาม
16. **คืนข้อมูลจริงผ่าน UI** (ลบแถว `[TEST-…]`, แก้ค่ากลับ + Approve) → query ยืนยัน ฉบับร่าง = `pb_*` ทุกแถว, `sort` ต่อเนื่อง
17. เอกสาร: ไฟล์นี้ (§0, §3, §4, §5.5, §11, §12) · `core_admin/CLAUDE.md` (ตาราง "เมนู CMS ที่ทำครบสายแล้ว" — สั้น) + `core_admin/docs/cms-menu-playbook.md` (สเปกรายเมนูฉบับเต็ม) · `docs/backend-menu-status.html` (ย้ายแถว + ตัวเลข 2 หัวตาราง + วันที่) · `docs/preview-spec.md` (บรรทัดสถานะ) · front-end `CLAUDE.md` (ตาราง "สถานะปัจจุบัน" + รายชื่อไฟล์ที่ต่างจาก static web)
18. **commit + push ทุก repo ที่มีไฟล์แก้** (§2.1 ข้อ 7 — admin ด้วย, push ด้วย URL ตรง) ลงท้าย `Co-Authored-By:` ตาม system reminder · รายงานผู้ใช้ตาม §2.3

---

## 7. เทคนิคทดสอบ Playwright ที่ใช้ได้จริง

ใช้ `mcp__playwright__browser_run_code_unsafe` (โหลดด้วย ToolSearch `select:mcp__playwright__browser_run_code_unsafe` ก่อน) · เปิด context ใหม่ต่อชุดทดสอบ: `page.context().browser().newContext({ ignoreHTTPSErrors: true, viewport: {…} })`

### 7.1 login หลังบ้าน (มี reCAPTCHA แล้ว — ต้องติ๊กจริง)

```js
await p.goto('https://localhost:7300/Admin/User/Login', { waitUntil: 'networkidle' });
await p.fill('input[name="username"]', 'user');
await p.fill('input[name="password"]', 'P@ssw0rd');
await p.waitForSelector('iframe[title="reCAPTCHA"]');
const fr = p.frameLocator('iframe[title="reCAPTCHA"]').first();
await fr.locator('#recaptcha-anchor').click();
await fr.locator('#recaptcha-anchor[aria-checked="true"]').waitFor();
await p.click('button[type="submit"]'); await p.waitForLoadState('networkidle');   // → /Admin/User/Dashboard
```
- **1 บัญชี 1 เซสชัน**: login ใน context ใหม่จะเตะ context เก่าหลุด → ทุกสคริปต์ควรเช็ก `page.url().includes('Login')` แล้ว login ใหม่
- ทดสอบ modal re-login: อยู่หน้าหลังบ้านแล้ว `ctx.clearCookies({ name: 'AssetPlus.Admin.Session' })` → รอ `#relogin_section.show` → กล่อง `#relogin_recaptcha iframe[title="reCAPTCHA"]` (**ต้องมี `[title]`** — มี iframe เปล่าอีกตัว)

### 7.2 SweetAlert / ฟอร์ม

- ปุ่มบันทึก: `#fm_cms button[type="submit"]` (ห้ามใช้ `button[type=submit]` เฉย ๆ — ชนกับปุ่มใน modal re-login ที่ซ่อนอยู่)
- ยืนยัน: `await p.waitForSelector('.swal2-popup'); await p.click('.swal2-confirm')` · หลังกด ถ้าจะคลิกต่อต้องรอ `.swal2-container` detached
- ปุ่มลบรูป `#id_<field>_del` มี swal "ลบออกจากเนื้อหา ?" ก่อน
- `<input type=email>` ค่าผิด → browser บล็อกก่อน ไม่มี swal
- ฟอร์ม Edit ล็อก (`disabled`) เมื่อ `pb_status = 0` → Approve ก่อนแก้ต่อ (ยกเว้น HomeSEO)
- ปุ่ม Approve ในแถว: `tr[data-id="<id>"] button[onclick="approveRow('<Module>', '<id>', '1')"]` (มี Unapprove และปุ่ม status สีเขียวคลาสเดียวกัน)
- ปุ่มลบในแถว: `tr[data-id="<id>"] [onclick*="deleteRow"]` · ปุ่ม Preview: `[onclick*="openFrontPreview"]`
- ใส่รูปโดยไม่เปิด elFinder: `page.evaluate(v => { document.querySelector('[name="img1"]').value = v; checkFileFM('id_img1'); }, 'Files/Site0/1/...')` · เปิด elFinder จริง: คลิก `button[onclick*="openElFinder('id_img1'"]` → dblclick โฟลเดอร์ → dblclick ไฟล์

### 7.3 จัดเรียง (SortableJS)

```js
await p.setViewportSize({ width: 1440, height: 2200 });          // แถวมีรูปสูง ~300px ต้องให้ทั้งตารางอยู่ในจอ
await p.goto('https://localhost:7300/Admin/<Module>?orderby=sort&sort=asc&text=');
await p.locator('tr[data-id="15"] .move_handle')
       .dragTo(p.locator('tr[data-id="10"]'), { targetPosition: { x: 20, y: 5 } });   // y=5 วางเหนือแถว, y=290 วางใต้แถว
await p.waitForSelector('.swal2-popup'); await p.click('.swal2-confirm');              // "เลื่อนขึ้น ?" / "เลื่อนลง ?"
```
- ค้นหาอยู่ / เรียงคอลัมน์อื่น = ไม่มี `.move_handle` และ server ปฏิเสธ Move

### 7.4 Preview ใน iframe / front-end

```js
await p.click('[onclick*="openFrontPreview"]'); await p.waitForTimeout(2000);
const fr = p.frames().find(f => f.url().includes('/_preview/'));
await p.locator('.modal.show button:has-text("อังกฤษ")').click();   // สลับภาษา (frame โหลดใหม่ ต้อง find ใหม่)
await p.keyboard.press('Escape');                                    // ปิด modal
```
- หน้าแรก front-end ต้องข้าม intro: `https://localhost:7310/?intro=1`
- popup: `#announcementModal.show` · สไลด์ `#popupSlider .swiper-slide[data-popup-id]` · ใบที่เห็น `.swiper-slide-active` · ปุ่ม `#popupNext` / `#popupPrev` (disabled ที่ปลาย) · dot `#popupPagination > *` · ถอด `#announcementModal, .modal-backdrop` ก่อนคลิกอย่างอื่น
- ลิงก์ `_blank`: `Promise.all([ctx.waitForEvent('page'), p.$eval(sel, a => a.click())])`
- เก็บ console error / response ≥400: `p.on('console', …)`, `p.on('response', …)`
- screenshot เก็บที่ `core_admin/.playwright-mcp/` (gitignore) — ห้ามทิ้งใน repo front-end

### 7.4b page builder (`/Admin/CMSPage/Edit/1`) — 18 ก.ย. 2569

- **ห้าม `waitUntil: 'networkidle'`** (หน้าหลังบ้าน poll `AjaxCheckSession` ทุก 5 วิ + การ์ดโหลดตัวอย่างด้วย `.load()`) → `domcontentloaded` แล้ว `waitForFunction` ให้ทุก `#tools_module > .itemDrag [class^="wgs_"]` ไม่มีคำว่า Loading
- ลำดับบนแคนวาส: `page.evaluate(() => sortableMain.toArray())` → `["wg_28", …]`
- **ลากจากพาเลตต์ → แคนวาส**: `locator.dragTo()` และ `page.mouse` ค้างที่ "waiting for scheduled navigations" (native HTML5 DnD) → ยิง synthetic event แทน (ใช้ได้จริง ทดสอบแล้ว):
  ```js
  await p.evaluate(async ([wg, dstWg]) => {
    const src = document.querySelector(`#accordion_widget .itemDrag[data-id="${wg}"]`), dst = document.querySelector(`#tools_module > .itemDrag[data-id="${dstWg}"]`);
    const sr = src.getBoundingClientRect(), dr = dst.getBoundingClientRect(), dt = new DataTransfer();
    src.dispatchEvent(new PointerEvent('pointerdown', { bubbles: true, cancelable: true, button: 0, clientX: sr.x + 10, clientY: sr.y + 10, pointerType: 'mouse', isPrimary: true }));
    src.dispatchEvent(new MouseEvent('mousedown', { bubbles: true, cancelable: true, button: 0, clientX: sr.x + 10, clientY: sr.y + 10 }));
    const fire = (el, t, x, y) => el.dispatchEvent(new DragEvent(t, { bubbles: true, cancelable: true, dataTransfer: dt, clientX: x, clientY: y }));
    fire(src, 'dragstart', sr.x + 10, sr.y + 10); await new Promise(r => setTimeout(r, 80));
    fire(dst, 'dragenter', dr.x + 200, dr.y + 5); fire(dst, 'dragover', dr.x + 200, dr.y + 5); await new Promise(r => setTimeout(r, 80));
    fire(dst, 'dragover', dr.x + 200, dr.y + 5); fire(dst, 'drop', dr.x + 200, dr.y + 5); fire(src, 'dragend', dr.x + 200, dr.y + 5);
  }, ['wg_35', 'wg_29']);   // วาง wg_35 เหนือ wg_29
  ```
- ลากบนแคนวาสเอง: `p.locator('#tools_module > .itemDrag[data-id="wg_33"] .my-handle').dragTo(p.locator('… [data-id="wg_32"]'), { targetPosition: { x: 300, y: 8 } })` ใช้ได้ (ผลไม่แม่นเสมอ — ใช้ปุ่มลูกศรถ้าต้องการลำดับแน่นอน)
- ปุ่มเครื่องมือของการ์ด (ขึ้น/ลง/ดินสอ/ลบ) โผล่ตอน hover → `card.locator('.my-handle').hover()` แล้ว `click({ force: true })`
- บันทึก: `#fm_cms button[type="submit"]` → swal → รอ `waitForURL(/\/Admin\/CMSPage(\?|$)/)` · Approve/Preview อยู่หน้า list แถว `tr[data-id="1"]`
- เปิดกลุ่มในพาเลตต์: `button[data-bs-target="#collapse_<group id>"]` (dev: 4 = DEFAULT, 5 = MODERN, 6 = CLASSIC)

### 7.5 ตรวจด้วย curl (เร็ว ไม่ต้อง login)

```bash
B=https://localhost:7310
curl -skI "$B/_preview/page/HomeHeader/1" | grep -i "content-security\|x-robots\|cache-control"
curl -sk "$B/?intro=1" | grep -o 'data-popup-id="[0-9]*"'
# diff หน้าพรีวิวกับหน้าจริง (ต้องได้ 0 บรรทัดเมื่อฉบับร่าง = อนุมัติ)
norm() { tr -d '\r' | sed -n '/<body/,/<\/body>/p' | grep -v "__RequestVerificationToken\|?v=" | sed 's/[[:space:]]\+/ /g' | grep -v '^ *$'; }
diff <(curl -sk "$B/?intro=1" | norm) <(curl -sk "$B/_preview/page/HomeFooter/1?lang=th" | norm) | grep -c '^[<>]'
```
ภาษาไทยใน HTML ถูก encode เป็น `&#xE01;` — จะเทียบข้อความให้ใช้ Playwright แทน curl

---

## 8. กับดักที่เจอจริง

1. **DEFAULT constraint วันที่ใน `2026_*` เคยเป็น literal `'…+07'`** → INSERT ที่ไม่ส่งคอลัมน์นั้นพัง "Conversion failed…" · dev แก้แล้ว · **เซิร์ฟเวอร์จริงต้องรัน `docs/sql/2026-09-16-fix-default-constraints.sql`** (ตรวจ `select count(*) from sys.default_constraints where definition like '%+07'')'` = 0)
2. **SQL ดิบที่ใช้ `Module.Config.Table` ต้องครอบ `Db.T()`** (`AdminHelpers.cs` เคยตก 4 จุด → sort = 10 ทุกแถว) — แก้แล้ว ยังไม่ commit
3. **`Views/<Module>/Index.cshtml` ของเมนู SAM บางตัวเป็นสำเนาเก่า** ไม่มีปุ่ม Preview → `git rm`
4. ฟอร์ม Edit ล็อกเมื่อ `pb_status = 0` (ยกเว้น HomeSEO)
5. `AdminCoreController.Edit` ไม่กรอง `module_id` (SAM ก็เป็น) — ยังไม่แก้
6. `HomeImageSlide` status 3 ค่า (2 = ปักหมุด) → front-end `status > 0`
7. ฟอร์มที่เป็นไทยอย่างเดียว/ซ่อนฟิลด์ → บันทึกแล้ว `en_*` เป็น NULL → front-end ต้อง fallback ไทย
8. Swiper loop จัด DOM ใหม่ → ดู `.swiper-slide-active` · popup Swiper ไม่ loop
9. front-end `wwwroot/favicon/*` ไม่มีในรีโป → 404 ทุกหน้า (ของเดิม)
10. ชื่อผู้แก้ไขในหลังบ้านยังเป็น "สยามอี ซีเอ็มเอส" (`[2026_web_admin].name`) — ข้อมูล SAM ที่ยังเหลือ
11. รูปใน `wwwroot/Files/` ไม่ไปกับ git/publish (ตอนนี้: `Site0/1/home/hero-*.jpg` 6, `intro_page/scam-alert.jpg`, `pop_up/{scam-alert,follow-facebook}.jpg`, `header/logo-{dark,light}.svg`, `footer/{logo-light.svg,icon-app.png,qr-code.jpg}`)
12. **Razor กิน `@` ใน JS** → regex อีเมลต้องเขียน `@@` (build error RZ1005)
13. `sqlcmd -i` ที่มี `ALTER TABLE ADD` + `UPDATE` คอลัมน์ใหม่ → ต้องมี `GO` คั่น
14. `InputTextLink()` / `InputTextLinkCMSPage()` แปะ `[Link Helper]` ที่ต้องมี component `LinkHelper` → ช่อง URL ภายนอกใช้ `InputText(…, maxlength 2000)`
15. **ผู้ใช้เปิด `dotnet watch` ที่พอร์ต 5310 ถือ `bin/` ของ front-end** → `dotnet build` ปกติจะล็อก/รัน `--launch-profile https` จะ bind 5310 ไม่ได้ → ใช้ build แยก (§9.3) · **ห้ามฆ่า process ของผู้ใช้**
16. `dotnet run` ที่ spawn ด้วย WMI แล้วพอร์ตไม่ขึ้น → ดู log `%TEMP%\assetfund-7310.log` / `%TEMP%\core_admin-7300.log` (มักเป็นพอร์ตชน)
17. **คีย์ทดสอบ reCAPTCHA ผ่านทุก token** — อย่าสรุปว่ากันบอทได้จนกว่าจะใช้คีย์จริง
18. หน้า LastActivity ใช้ `document.querySelectorAll('table')` ใน Playwright แล้วได้ 0 (โครง DOM ไม่ปกติ) → ตรวจจาก `document.body.innerText` แทน
19. เมนูที่ `sort` เปลี่ยน (Move) มีผลกับหน้าเว็บทันทีโดยไม่ต้อง Approve — บอกผู้ใช้เสมอเมื่อเปิด `CanMove` เมนูใหม่
20. **ห้าม `git checkout -- <file>` / `git restore` ใน repo admin** — มีงานค้าง commit ข้าม session · 18 ก.ย. เผลอทำกับ `AdminMenu.cs` งาน 2 session หาย ต้อง replay ทุก Edit/Bash จาก transcript `~/.claude/projects/<project>/*.jsonl` (สคริปต์ `replay_adminmenu.py` ใน scratchpad session 3) ยืนยันด้วยเลขบรรทัดตรงกันทุกจุด · สคริปต์ที่ assert พังก่อนเขียน = ไฟล์ยังไม่ถูกแตะ ไม่ต้อง revert
21. Bash tool ของ harness พัง (`unexpected EOF while looking for matching`) เมื่อ heredoc มี python `'''` → เขียนสคริปต์ลงไฟล์ด้วย Write แล้ว `python file.py` · `sqlcmd -W` ใช้ร่วมกับ `-y 0` ไม่ได้ (dump คอลัมน์ยาวใช้ `-y 0` อย่างเดียว)
22. PowerShell `$b -notmatch "succeeded"` บน array คืน array ไม่ใช่ bool → `if` เป็นจริงทั้งที่ build ผ่าน (server ไม่ถูก start) — เช็คด้วย `$LASTEXITCODE` แทน
23. builder ของ SAM มีบั๊กที่แก้แล้ว 18 ก.ย.: hidden `name="box_data2"`, `innerHTML.substring(36,39)`, SQL ต่อ id ตรง 2 จุด, bind ปุ่มเลื่อนซ้ำ, `= ANY(@box)` (PostgreSQL) ใน `PreviewMenu.cs`, ซ่อนการ์ดพาเลตต์ด้วย `getElementById` ที่ id ซ้ำกับการ์ดที่ลากมา (→ `querySelector` ใน `#accordion_widget`)
24. แท็บ builder โหลด CSS/Swiper จาก `FrontURL` (front-end ต้องรันแบบ https 7310 และส่ง CORS ให้ฟอนต์) — ถ้า front-end ไม่รัน ตัวอย่างในการ์ดจะไม่มีสไตล์

---

## 9. คำสั่ง / ค่า / ไฟล์ SQL ที่ใช้บ่อย

### 9.1 DB

```bash
SQL="/c/Program Files/Microsoft SQL Server/Client SDK/ODBC/170/Tools/Binn/sqlcmd"
"$SQL" -S . -U sa -P sasa -d asset_plus_uat -W -s"|" -f 65001 -Q "set nocount on; select ..."
"$SQL" -S . -U sa -P sasa -d asset_plus_uat -W -s"|" -b -f 65001 -i "$(cygpath -w /path/file.sql)"   # ไฟล์ภาษาไทย

# ตรวจสถานะข้อมูล 6 เมนู
select id,sort,status,pb_status,show_front,pb_issue_date_config from [2026_web_core_item] where module_id=1 order by sort;
select id,status,pb_status,show_front from [2026_web_home_intro_page];
select id,sort,status,pb_status,show_front from [2026_web_home_pop_up] order by sort;
select id,pb_status,pb_title from [2026_web_home_seo];
select id,pb_status,title,img1,en_img1 from [2026_web_home_header];
select id,pb_status,title,img1,tel,email,promo_year,cr from [2026_web_home_footer];
select mod_name,can_add,can_edit,can_delete,can_move,can_status,can_approve from [2026_web_admin_module] where access_id=1 and mod_name like 'Home%';
select top 10 action, action_info, created_at from [2026_web_admin_log] order by id desc;
-- page builder หน้าแรก (18 ก.ย.)
select id,is_home,pb_status,box_layout,pb_box_layout from [2026_web_cms_page];                        -- ต้องมีแถวเดียว id 1
select id,cat_id,sort,title,section_key,pb_section_key,mod_name from [2026_web_widget] order by cat_id,sort; -- 18 แถว id 28-45
select id,sort,title from [2026_web_widget_group] order by sort;                                          -- 3 แถว id 4/5/6
```

### 9.2 admin (7300)

```powershell
Get-Process core_admin -ErrorAction SilentlyContinue | Stop-Process -Force
Set-Location "D:\Project\admin.assetfund.co.th.2026\core_admin"
dotnet build core_admin.csproj -nologo -v q
$log="$env:TEMP\core_admin-7300.log"; $cmd='cmd /c "dotnet run --no-build --launch-profile https > "'+$log+'" 2>&1"'
Invoke-CimMethod -ClassName Win32_Process -MethodName Create -Arguments @{CommandLine=$cmd; CurrentDirectory="d:\Project\admin.assetfund.co.th.2026\core_admin"}
```

### 9.3 front-end (7310) — build แยกของ Claude (ใช้ได้เฉพาะตอน 7310 ว่าง)

> ⚠ ตั้งแต่ 18 ก.ย. 2569 profile เริ่มต้นของ front-end คือ `https` (7310 + 5310) — ถ้าผู้ใช้รันอยู่ **7310 เป็นของผู้ใช้ ห้ามเปิดซ้อน/ห้ามฆ่า** ให้ทดสอบกับ instance ของผู้ใช้ (บอกผู้ใช้ restart หลังแก้โค้ด) · ใช้สูตรด้านล่างเฉพาะตอนไม่มีใครถือ 7310 และ **ปิดตัวนี้ก่อนจบงาน** ไม่งั้นผู้ใช้กดรันแล้วชนพอร์ต

```powershell
# ดูว่าใครถือพอร์ต: Get-NetTCPConnection -LocalPort 5310,7310 -State Listen | select LocalPort,OwningProcess
# ฆ่าเฉพาะตัว 7310 ที่ Claude เปิด (commandline มี "--urls https://localhost:7310") ห้ามแตะ PID ของ dotnet watch / 5310
Set-Location "D:\Project\assetfund.co.th.2026"
$out = "$env:TEMP\assetfund-7310-bin"
dotnet build asset-fund.csproj -nologo -v q -o $out
$log="$env:TEMP\assetfund-7310.log"
$cmd='cmd /c "set ASPNETCORE_ENVIRONMENT=Development&& dotnet "'+$out+'\asset-fund.dll" --urls https://localhost:7310 --contentRoot d:\Project\assetfund.co.th.2026 > "'+$log+'" 2>&1"'
Invoke-CimMethod -ClassName Win32_Process -MethodName Create -Arguments @{CommandLine=$cmd; CurrentDirectory="d:\Project\assetfund.co.th.2026"}
```
ถ้าผู้ใช้ไม่ได้เปิด watch: ใช้ `dotnet build asset-fund.csproj` + `dotnet run --no-build --launch-profile https` ตามปกติได้ (ต้องฆ่าตัวที่ถือ 5310/7310 ก่อน)

### 9.4 ไฟล์ SQL ที่เก็บถาวร (`core_admin/docs/sql/` — track ใน git)

| ไฟล์ | ใช้ทำอะไร |
|---|---|
| `2026-09-16-fix-default-constraints.sql` | **deploy ต้องรัน** — แก้ DEFAULT วันที่ `+07` (idempotent) |
| `2026-09-17-web-home-header-footer.sql` | **deploy ต้องรัน** — ALTER เพิ่มคอลัมน์ Header/Footer (idempotent) |
| `2026-09-18-web-widget-section-key.sql` | **deploy ต้องรัน** — ALTER `web_widget` เพิ่ม `section_key`/`pb_section_key` + สิทธิ์ CMSPage เหลือ edit/approve (idempotent) |
| `2026-09-18-delete-sam-cms-widgets.sql` | **deploy ต้องรัน** — ลบเมนู SAM ใน `web_cms_page` (เหลือ id 1) และ widget/widget2 ของ SAM (idempotent) · สำรองก่อน |
| `seed-ref-home-widgets.sql` | **อ้างอิงเท่านั้น** — 3 กลุ่ม × 6 widget (HTML snapshot จาก `/salepage` 426 KB) + `box_layout` ของ id 1 · บน dev รันแล้ว ห้ามรันซ้ำ · deploy: รันได้ถ้าตารางยังไม่มี widget ของ Asset Plus (id ที่ได้จะต่างจาก dev → ตรวจ `box_layout` ที่สคริปต์คำนวณให้เอง) |
| `seed-ref-home-image-slide.sql`, `seed-ref-home-intro-page.sql`, `seed-ref-home-popup.sql`, `seed-ref-home-seo.sql`, `seed-ref-home-header.sql`, `seed-ref-home-footer.sql` | **อ้างอิงเท่านั้น** — ข้อมูล Asset Plus ที่ใส่แทน SAM (อ้าง id ของ dev ห้ามรันซ้ำบน dev) |

### 9.5 ค่าอื่น

- admin login `user` / `P@ssw0rd` (hash SHA512 ใน CLAUDE.md admin) · `web_id = 0` · Super Admin `access_id = 1`
- Preview URL: `{FrontURL}/_preview/page/{Module}/{id}?lang=th|en` · `FrontURL` dev `https://localhost:7310` · front-end `AdminURL` dev `https://localhost:7300`
- scratchpad session 1: `C:\Users\ball\AppData\Local\Temp\claude\d--Project-admin-assetfund-co-th-2026-core-admin\4baeaceb-…\scratchpad\` · session 2: `…\bee81cc1-…\scratchpad\` (backup แถวเดิมก่อนแก้อยู่ที่นั่น — อาจถูกลบเมื่อไหร่ก็ได้)

---

## 10. เช็คลิสต์เปิด session ใหม่

1. อ่านไฟล์นี้ให้จบ → `core_admin/CLAUDE.md` (โหลดอัตโนมัติ — กฎ/สถาปัตยกรรม/login) → `core_admin/docs/cms-menu-playbook.md` (Playbook 19 ขั้น + สเปกรายเมนู) เมื่อจะทำเมนู → `d:\Project\assetfund.co.th.2026\CLAUDE.md` ("สถานะปัจจุบัน" + "ระบบ Preview" + "วิธีต่อเมนูถัดไป")
2. ตรวจเซิร์ฟเวอร์: `Get-NetTCPConnection -LocalPort 7300,7310,5310 -State Listen` + ดูชื่อ process (`Get-CimInstance Win32_Process -Filter "ProcessId=<pid>"`) — 5310 ที่เป็น `dotnet watch` คือของผู้ใช้ · ไม่มี 7300/7310 ให้ spawn ตาม §9.2 / §9.3
3. `git status` ทั้ง 2 repo: admin ควรเห็นไฟล์ตาม §11 · front-end ต้องสะอาด (ล่าสุด `bc15ab1`)
4. ตรวจ DB ตาม §9.1: ไม่มีแถว `[TEST…]`/`[DRAFT…]` · ทุกแถว `pb_status = 1` และฉบับร่าง = `pb_*` · intro/popup status 0 เป็นค่าที่ผู้ใช้ตั้ง (ไม่ต้องแก้) · `web_cms_page` 1 แถว, `web_widget` 18 แถว (id 28–45), `web_widget_group` 3 แถว (id 4/5/6), `widget2`/`group2` ว่าง
4b. ตรวจว่าหน้าแรก 7310 เรียง section ตรงกับ `pb_box_layout` (curl `grep -o '<section class="section[^"]*"'`) — และถ้าผู้ใช้ใช้ 5310 ให้เทียบด้วย (ต่างกัน = 5310 ยังไม่ rebuild)
5. ลองเปิด `https://localhost:7300/Admin/User/Login` ด้วย Playwright (§7.1) ให้แน่ใจว่า reCAPTCHA ยังโหลดได้ (ต้องมีอินเทอร์เน็ต)
6. รับคำสั่ง → ทำตาม §6 ทั้งสาย → ปิดงานตาม §6 ข้อ 16–18 และอัปเดตไฟล์นี้

---

## 11. ไฟล์ค้าง commit + กฎ commit

### admin (`core_admin`) — ผู้ใช้ commit เองแล้ว `f175199` (17 ก.ย. 2569 15:56) — รายการด้านล่างคือสิ่งที่อยู่ใน commit นั้น (เก็บไว้เป็นบันทึกว่าไฟล์ไหนแก้อะไร) · ค้างจริงตอนนี้: `CLAUDE.md`, `docs/CMS-MENU-HANDOFF.md`

```
M  .gitignore                                   un-ignore docs/CMS-MENU-HANDOFF.md, docs/sql/*.sql
M  Areas/Admin/Controllers/AdminCoreController.cs   ลบกิ่ง HomeHeader Template + BuildMicrositeBoxLayout (ลบอย่างเดียว)
M  Areas/Admin/Controllers/UserController.cs    ตรวจ reCAPTCHA ใน Login POST + ส่ง SiteKey ให้ view
A  Areas/Admin/Helpers/ReCaptcha.cs             (ไฟล์ใหม่ ยัง untracked)
M  Areas/Admin/Helpers/AdminHelpers.cs          fix Db.T() 4 จุด (บั๊ก sort)
M  Areas/Admin/Helpers/AdminMenu.cs             เปิด 6 เมนู, ยุบ "หน้าหลัก", กลุ่ม "ข้อมูลหน้าแรก", breadcrumb, Field_HomeHeader/Footer, CanMove Pop-Up, ไอคอน · (18 ก.ย.) เปิด CMSPage + กลุ่ม Widget, CMSPage Can*=false, Widget section_key
M  Areas/Admin/Helpers/AdminMenuAssetPlus.cs    breadcrumb ApOtherIndices
M  Areas/Admin/Helpers/PreviewMenu.cs           ถอด HomeSEO · (18 ก.ย.) CMSPage -> page, แก้ ANY(@box)
M  Areas/Admin/Controllers/WidgetAjaxController.cs   (18 ก.ย.) Manage อ่าน pb_mod_name แทน map id hardcode
M  Areas/Admin/Views/CMSPage/Edit.cshtml       (18 ก.ย.) CSS จาก FrontURL, fix box_data2/substring/SQL/bind, Swiper hero, ดินสอเฉพาะ mod_name
M  Areas/Admin/Views/Shared/_PartialToolWidget.cshtml   (18 ก.ย.) parameterized + ดินสอเฉพาะ mod_name
M  Areas/Admin/Views/Widget/{Create,Edit}.cshtml, WidgetAjax/Manage.cshtml   (18 ก.ย.) ช่อง Section Key
M  Areas/Admin/Views/HomeHeader/Edit.cshtml, HomeFooter/Edit.cshtml    ฟอร์มใหม่ทั้งไฟล์
D  Areas/Admin/Views/{HomeIntroPage,HomePopUp,HomeSEO,HomeHeader,HomeFooter}/Index.cshtml   สำเนาเก่า (staged)
M  Areas/Admin/Views/Shared/_PartialAdminMenu.cshtml   reCAPTCHA ใน modal re-login
M  Areas/Admin/Views/User/Dashboard.cshtml      ปุ่ม Logout
M  Areas/Admin/Views/User/LastActivity.cshtml   ลบ ;
M  Views/Login/Index.cshtml                     ⚠ ปนกัน: ของผู้ใช้ (สุ่มพื้นหลัง bg0–bg9) + ของ Claude (ซ่อน dropdown + reCAPTCHA)
M  appsettings.json, appsettings.Development.json.example   GoogleReCaptcha (คีย์ทดสอบ)
M  CLAUDE.md, docs/backend-menu-status.html, docs/preview-spec.md
?? docs/CMS-MENU-HANDOFF.md, docs/sql/ (11 ไฟล์)
```
นอก git: `appsettings.Development.json` (มี GoogleReCaptcha แล้ว) · รูปใน `wwwroot/Files/Site0/1/{home,intro_page,pop_up,header,footer}/` + `widget_icons/assetplus/` (21 รูป thumbnail widget) · `docs/backup-sam-widgets/` (dump ข้อมูล SAM ก่อนลบ) · DB dev (ALTER 2 ตาราง, fix DEFAULT, ข้อมูล Asset Plus)

ถ้าผู้ใช้สั่ง commit admin: แบ่ง commit ตามเรื่องได้ (เมนู CMS / Login-reCAPTCHA / เอกสาร) · ถามเรื่อง `Views/Login/Index.cshtml` เพราะมีงานของผู้ใช้ปน · ลงท้าย `Co-Authored-By:` ตาม system reminder ของ session นั้น

### front-end — commit ครบ (push ด้วย URL ตรง ดู §2.1 ข้อ 7)

```
bc15ab1 Point CLAUDE.md at the rewritten CMS handoff document
5e6a921 Remove draft summary bars from preview pages and drop SEO preview
8f9bd07 Read header logo and footer brand/promo blocks from SQL Server with draft preview
6847fec Point CLAUDE.md at the CMS menu handoff document in the admin repo
f8b558a Read site SEO and embed codes from SQL Server into every layout
a259e17 Read home announcement pop-ups from SQL Server with draft preview
d2b1787 Add intro page read from SQL Server with draft preview
edfdfb7 Point CLAUDE.md at the full end-to-end CMS menu playbook in the admin repo
bd64d92 Read home hero slides from SQL Server and add draft preview route
```

---

## 12. แผนงานต่อไป

รูปแบบคำสั่งที่ผู้ใช้ใช้: *"เพิ่มเมนูหลังบ้าน `<กลุ่ม> >> <เมนู>` … front-end … Preview ครบ … ทดสอบละเอียด"* → ทำตาม §6 · ถ้าผู้ใช้ไม่ระบุกลุ่ม ให้ยึดกลุ่มตาม SAM แล้วบอกในรายงาน

### 12.1 งานที่ผู้ใช้พูดถึงแล้ว (มีโอกาสมาถัดไปสูง)

| งาน | สิ่งที่ต้องรู้ล่วงหน้า / แนวทาง |
|---|---|
| **เมนู footer 5 คอลัมน์ (`footer__nav`) + ลิงก์นโยบายล่างสุด (`footer__legal`)** | ผู้ใช้บอกตอนสั่ง Footer ว่า "จะทำเมนูด้านซ้ายแยกไปอีก" · SAM มี `CMSPageFooter1` / `CMSPageFooter2` (ตาราง `web_cms_page_footer1/2`, Preview โหมด `cms`) อยู่ในกลุ่ม "หน้าเว็บไซต์" (ยัง `//`) · front-end ตอนนี้ hardcode ใน `Views/Shared/_PartialFooter.cshtml` (5 คอลัมน์: กองทุนรวม / กองทุนส่วนบุคคล+สำรองเลี้ยงชีพ / บริการของเรา / ข่าวสาร / เกี่ยวกับเรา) และ `_PartialFooterSale.cshtml` (ไม่มี nav, legal 3 ลิงก์) · **front-end ใช้ attribute routing ไม่ใช่ seo_url แบบ SAM** → ต้องตกลงกับผู้ใช้ว่าจะเก็บ "หัวข้อ + ลิงก์ + ลำดับ" แบบไหน (น่าจะ web_core_group=หัวคอลัมน์ + web_core_item=ลิงก์ หรือใช้ CMSPageFooter1/2 เดิม) · ถ้าใช้ตารางเดิมของ SAM ต้องดูว่า Preview `cms` มีความหมายหรือไม่ (ไม่มีหน้า CMS ให้แสดง อาจถอดปุ่มแบบ SEO) |
| **ข้อความในแต่ละ section ของหน้าแรกให้แก้ได้จากหลังบ้าน** | ผู้ใช้เลือก 18 ก.ย. ให้ hardcode ไปก่อน · แนวทาง: เปิด `HomeSamText2–6` (web_core_single module 2–6) เปลี่ยนชื่อ/ฟอร์มใหม่แบบ Header/Footer แล้วใส่ `mod_name` ใน widget (ตัวอย่างในหลังบ้านจะเติม token `|||pb_t1|||` เอง) + front-end อ่านค่าไปแทน hardcode ใน partial · hero ทำแบบนี้แล้ว (`mod_name = HomeImageSlide` + `|||REPEAT|||`) |
| **เมนูหน้าอื่น (`CMSPage` แบบ SAM = เมนูหลัก header + หน้า CMS)** | ผู้ใช้ตัดสินใจ 18 ก.ย. ว่า `web_cms_page` เหลือแถวเดียว (หน้าแรก) — เมนูหลักของ front-end ยัง hardcode ใน `_PartialHeader.cshtml` · ถ้าจะทำต้องออกแบบใหม่ (ไม่ใช้ตารางนี้) |
| **ใช้ reCAPTCHA คีย์จริง** | สมัครที่ https://www.google.com/recaptcha/admin (v2 Checkbox) แยก dev (localhost) / production (โดเมนจริง) → แก้ `GoogleReCaptcha:SiteKey/SecretKey` · ไม่ต้องแก้โค้ด |
| กลุ่ม "ข้อมูลหน้าแรก" เมนูอื่น | `HomeImageConf` (ตั้งค่า effect/ความเร็วสไลด์ — ผู้ใช้เคยบอก "ยังไม่ต้อง"; front-end `wwwroot/js/pages/home.js` hardcode delay 4000) · `HomeSamText…7` เป็นเนื้อหา SAM (ใส่ใจ/บริหารหนี้/ทรัพย์เด่น) **ไม่ตรงกับเว็บ Asset Plus** → ต้องถามผู้ใช้ว่า section ไหนของหน้าแรก Asset Plus ต้องแก้ได้ (หน้าแรกมี: hero, NAV summary, กองทุนแนะนำ, ธีมกองทุน, Insights, ผู้สนับสนุนการขาย) |

### 12.2 งานใหญ่ที่อยู่ในความต้องการตั้งแต่แรก (front-end ยัง mock)

| หน้า front-end | Service (mock) | แหล่งข้อมูลหลังบ้านที่น่าจะใช้ | หมายเหตุ |
|---|---|---|---|
| ~~NAV หน้าแรก~~ **ต่อแล้ว 18 ก.ย. 2569** (`SqlNavPriceService` ← `tb_fund_nav` + `tb_fund`) · ส่วนต่าง ๆ ที่เหลือของหน้าแรก (กองทุนแนะนำ, ธีม, ผู้สนับสนุน) | `IFundService`, `IDistributorService` | `tb_fund*`, `tb_fund_nav` (`Flag = 1`) | ต้องออกแบบเมนูสำหรับ "กองทุนแนะนำ/ธีม" (ยังไม่มีเมนูหลังบ้าน) |
| `/funds`, `/funds/{code}`, `/funds/nav`, `/funds/performance`, `/funds/calendar` | `IFundService` | เมนู `Ap*` มีครบ (`tb_fund_cat`, `tb_fund`, `tb_fund_doc`, `tb_fund_fundfact*`, `tb_fund_nav`, `tb_fund_performance(_hd)`, `tb_calendar*`) | ตาราง `tb_*` ใช้ร่วมกับเว็บเดิม · ไฟล์แนบเป็นชื่อไฟล์เปล่า ต่อ URL จาก `LegacyUpload(Doc):Url` · ดูวิธีเว็บเดิมอ่านใน `d:\Project\assetfund.co.th.old` · Preview ของเมนู `tb_*` ยังไม่มีกลไก (ต้องออกแบบ) |
| `/private-fund` | `IPrivateFundService` | `tb_fund_private*` (6 เมนู `ApPrivate*`) | |
| `/provident-fund` | `IProvidentFundService` | `tb_fund_prov*` (4 เมนู `ApProv*`) | |
| `/news`, `/announcements`, `/articles`, `/events` | `IInsightService` | SAM `web_core_news` / `web_core_group` หรือตารางเดิม `tb_news*`, `tb_announcement*` (หลังบ้านเดิมยังไม่พอร์ต) | ต้องมีหน้ารายละเอียด → Preview โหมด `item` (`PreviewItem()` ยังไม่มี — ลอกแนว SAM) · ⚠ ข้อมูลเก่าใน `2026_web_core_news` มีรูปชี้ `https://localhost:7140/...` |
| `/about/*`, `/services/*`, `/faq`, `/careers`, `/privacy`, `/terms`, `/cookie-policy`, `/sitemap`, `/contact` | `Mock*Service` แยกตัว | ยังไม่มีเมนูรองรับ (หลังบ้านเดิมมี `tb_page_aboutus`, `tb_board_director`, `tb_job_*`, `tb_pdpa*` ยังไม่พอร์ต) | ต้องคุยกับผู้ใช้ว่าจะใช้เครื่องยนต์ `web_core_single`/`web_cms_page` ของ SAM หรือพอร์ต `tb_*` |

### 12.3 หนี้ทางเทคนิค / เรื่องที่ควรเสนอผู้ใช้

- ~~commit ฝั่ง admin ต้องถามก่อน~~ 17 ก.ย. เย็น ผู้ใช้สั่งให้ commit + push เองทุกครั้งที่จบงาน (§2.1 ข้อ 7)
- ทั้ง 2 repo ยังไม่มี remote `origin` (push ด้วย URL ตรง) — เสนอผู้ใช้ตั้ง `origin` ได้
- `AdminCoreController.Edit` ไม่กรอง `module_id`
- ชื่อผู้ใช้หลังบ้านยังเป็นของ SAM ("สยามอี ซีเอ็มเอส")
- popup ไม่เด้งตอนพรีวิวเมนูอื่นของหน้าแรก (ข้อยกเว้นเดียวของ "พรีวิวเหมือนเว็บจริง")
- สิทธิ์ผู้ใช้จริง: หลังบ้านเดิมมีบทบาท Content/Approver รายแผนก (`tb_admin_access`) — ระบบใหม่มีแค่ Super Admin
- ⚠ **ความปลอดภัย (ยืนยันจาก DB แล้ว 17 ก.ย.)**: `UserController.Login` บันทึก `old_value = JsonConvert.SerializeObject(f)` ลง `[2026_web_admin_log]` ตอน `login_fail_user` / `login_fail_password` / `login_fail_user_lock` / `login_fail_user_web` → **รหัสผ่านที่กรอกผิดถูกเก็บเป็นข้อความธรรมดา** (พฤติกรรมเดิมจาก SAM) · ยังไม่ได้แก้และยังไม่ได้แจ้งผู้ใช้ — ควรเสนอให้ตัด `password` ออกจาก `old_value` และล้างแถวเก่า

---

## 13. งานที่ต้องทำตอน deploy ขึ้นเซิร์ฟเวอร์จริง

1. **DB**: รัน `docs/sql/2026-09-18-home-quick-tiles.sql` (ลิงก์ด่วน + token ใน widget NAV — ขั้น 1 เขียนทับเนื้อหาแถว) · รัน `docs/sql/2026-09-16-fix-default-constraints.sql`, `docs/sql/2026-09-17-web-home-header-footer.sql`, `docs/sql/2026-09-18-web-widget-section-key.sql`, `docs/sql/2026-09-18-delete-sam-cms-widgets.sql` แล้วใส่ widget 3 กลุ่ม × 6 (`seed-ref-home-widgets.sql` — ตรวจ `box_layout` หลังรัน) · ใส่ข้อมูล Asset Plus ของ 6 เมนู (ผ่านหลังบ้าน หรือดัดแปลงจาก `seed-ref-*.sql`) · สิทธิ์ `2026_web_admin_module` ให้ตรง (`HomePopUp can_move = 1` ฯลฯ)
2. **รูป**: อัปโหลด `core_admin/wwwroot/Files/Site0/1/{home,home/tiles,intro_page,pop_up,header,footer,widget_icons/assetplus}/` ขึ้นเซิร์ฟเวอร์ admin
3. **admin `appsettings.json` บนเซิร์ฟเวอร์**: `DBConnection`, `RootURL`, **`FrontURL` = โดเมน front-end จริง**, **`GoogleReCaptcha:SiteKey/SecretKey` = คีย์จริงของโดเมน** (ไม่มี = login ไม่ได้ · คีย์ทดสอบ = ไม่กันบอท) · รายการคีย์อื่นดู CLAUDE.md admin หัวข้อ "ต้องตั้งบนเซิร์ฟเวอร์เอง"
4. **front-end `appsettings.json` บนเซิร์ฟเวอร์**: `ConnectionStrings:DBConnection`, **`AdminURL` = โดเมน admin จริง** (ใช้ทั้งต่อ path รูปและ CSP `frame-ancestors` ของ Preview), `DBCacheTime` (ผู้ใช้สั่ง 0 — ถามก่อนเปลี่ยน)
5. เซิร์ฟเวอร์ admin ต้องออกอินเทอร์เน็ตไป `https://www.google.com/recaptcha/api/siteverify` ได้ (ไม่งั้น login ไม่ได้)
7. **page builder**: เบราว์เซอร์ของแอดมินต้องโหลด CSS/ฟอนต์จากโดเมน front-end (`FrontURL`) ได้ — front-end ส่ง `Access-Control-Allow-Origin: <AdminURL>` ให้ไฟล์ฟอนต์อยู่แล้ว (`Program.cs`) แค่ตั้ง `AdminURL` ให้ตรงโดเมน admin จริง
6. `ASPNETCORE_ENVIRONMENT` ห้ามเป็น `Development`

---

## 14. session 3 (18 ก.ย. 2569) — page builder หน้าแรก (`CMSPage`) + ข้อมูล Widget

> หัวข้อนี้เขียนตอนปิด session 3 (context ใกล้เต็ม) ให้ session ถัดไปทำงานต่อได้โดยไม่ต้องย้อนอ่าน transcript · สเปกระดับคอลัมน์อยู่ใน `core_admin/CLAUDE.md` หัวข้อ "เมนูที่ทำตาม playbook แล้ว" → บล็อก **`CMSPage`**

### 14.1 ความต้องการของผู้ใช้ (คำสั่งจริง + คำตอบ 6 ข้อ)

- เพิ่มเมนู **"หน้าเว็บไซต์ > จัดการเมนูเว็บไซต์" (`CMSPage`)** โดยยึดแนวทาง back-end SAM แต่ **ตาราง `web_cms_page` เหลือแถวเดียว id 1 (หน้าแรก)** → เพิ่ม/ลบ/เปิดปิด/จัดเรียงไม่ได้ แก้ไขได้อย่างเดียว · `/Admin/CMSPage/Edit/1` ต้องเข้าหน้า **drop-down widget** (แท็บ "ตกแต่งเพจ") ทันที มีฟังก์ชันเหมือน SAM
- **"ข้อมูล Widget"** เก็บใน MSSQL แบบ SAM · ลบ widget ของ SAM ทิ้ง (ทำเป็นขั้นสุดท้าย) แล้วใส่ widget ของ Asset Plus โดย **1 `<section>` ของหน้า `/salepage` = 1 widget**
- คำตอบผู้ใช้: (1) **3 กลุ่ม × 6 = 18 widget** — กลุ่ม `DEFAULT` = Version 1, `MODERN` = Version 2, `CLASSIC` = Version 3 ของ `/salepage` (2) **ข้อความในแต่ละ section ยัง hardcode** ไม่มี "เลือกเวอร์ชัน section" (เลือกจาก 18 widget แทน) (3) **hero เป็น widget ธรรมดา** ลบ/ย้ายได้ (4) ลบข้อมูล SAM ได้ แต่ **ลบเป็นขั้นสุดท้าย** (5) กลุ่มเมนู "Widget" อยู่ **ถัดจาก "ผู้ดูแลระบบ"** (6) ต้องมี **ไอคอน Preview ลำดับฉบับร่าง** ที่หน้า list แถว id 1
- ทุกข้อลงท้าย: *ทดสอบอย่างละเอียด หลายเงื่อนไข หลายแบบ ครบถ้วน + ฝั่ง browser*

### 14.2 กลไกที่ทำ (ภาพรวม — ต่างจาก SAM ตรงไหน)

```
หลังบ้าน  /Admin/CMSPage (list 1 แถว: แก้ไข · Approve · [Preview])
          /Admin/CMSPage/Edit/1  = แท็บ "ตกแต่งเพจ" (is_home = 1 → ซ่อนแท็บ "ข้อมูลเพจ")
             ซ้าย  พาเลตต์ accordion 3 กลุ่ม (web_widget_group id 4/5/6) → การ์ด widget (web_widget cat_id) ลาก SortableJS (clone)
             ขวา   แคนวาส: การ์ดแต่ละใบโหลดตัวอย่างจาก /Admin/WidgetAjax?id=<web_widget.id> (HTML pb_info + token)
             Save  → box_layout = "wg_28,wg_29,…" (ฉบับร่าง, pb_status 0) → Approve → pb_box_layout
front-end SqlHomeLayoutService: pb_box_layout → split wg_<id> → SELECT pb_section_key FROM [2026_web_widget] WHERE id IN (…)
          → Index.cshtml: foreach key → <partial name="Partials/_<key>"> (Hero, NavPricesV2, DistributorsV3, …)
          /_preview/page/CMSPage/1 → PreviewState(web_cms_page, NoModule) → อ่าน box_layout (ฉบับร่าง) แทน
```

- SAM: หน้าบ้านเทียบ `"wg_2"` แบบ hardcode ใน view 1,500 บรรทัด และ**ไม่ได้อ่าน `web_widget`** — ของเรา data-driven ผ่านคอลัมน์ใหม่ **`web_widget.section_key` / `pb_section_key`** (= ชื่อ partial) · SAM พรีวิวการจัดเรียงฉบับร่างไม่ได้ (อ่าน `pb_box_layout` เสมอ) — ของเราทำได้ (โหมด `page`)
- `mod_name` ยังใช้ความหมายเดิมของ SAM = โมดูลข้อมูลที่ `WidgetAjax` ใช้เติม token `|||pb_xxx|||` ในตัวอย่าง (hero = `HomeImageSlide` + `|||REPEAT|||…|||/REPEAT|||` เติมสไลด์จริง) · widget ที่ไม่มี `mod_name` ไม่มีปุ่มดินสอ

### 14.3 ไฟล์ที่แก้ (admin — อยู่ใน commit `f175199` ของผู้ใช้แล้ว ยกเว้นเอกสารที่แก้ทีหลัง)

| ไฟล์ | สิ่งที่ทำ |
|---|---|
| `Areas/Admin/Helpers/AdminMenu.cs` | `Menu()`: เปิดบรรทัด `CMSPage` ในกลุ่ม "หน้าเว็บไซต์" (บรรทัดแรก) · เปิด `#region Widget` เฉพาะกลุ่ม "Widget" (กลุ่ม Widget / Widget ทั้งหมด) — `Widget2` ยัง `/* */` · `AllModule()`: `CMSPage` → `CanAdd/Delete/Move/Status = false`, `TextBreadcrumb` แก้ typo "หน้าเว้บไซต์" · `Widget` → เพิ่ม `"section_key"` ใน FieldCreate/Update/Approve + คอลัมน์ `ListData` "Section Key" |
| `Areas/Admin/Helpers/PreviewMenu.cs` | `CMSPage`: `"cms"` → `"page"` · แก้ `= ANY(@box)` (PostgreSQL) เป็น `IN (@box0,…)` ในกิ่ง cms (ยังใช้กับ `CMSPageFooter1/2` ที่ปิดอยู่) |
| `Areas/Admin/Views/CMSPage/Edit.cshtml` | CSS/Swiper ในแท็บ builder โหลดจาก `FrontURL` (`css/vendor.min.css`, `css/bootstrap-icons.min.css`, `css/main.min.css`, `vendors/swiper/*`) แทน `css_home` ของ SAM · `initWidgetSwipers()` เริ่ม Swiper เฉพาะ `.js-hero-slider` หลัง `.load()` · แก้บั๊ก SAM: hidden `name="box_data2"` → `box_data`, `substring(36,39)` → `dataset.id`, SQL ต่อ id → parameter, ปุ่มเลื่อนขึ้น/ลง delegate ครั้งเดียว, ซ่อนการ์ดพาเลตต์ด้วย `querySelector('#accordion_widget …')`, thumbnail การ์ด = `pb_img1` · ดินสอเฉพาะ widget ที่มี `pb_mod_name` |
| `Areas/Admin/Views/Shared/_PartialToolWidget.cshtml` | query ลูก widget แบบ parameter · ดินสอเฉพาะ `pb_mod_name` ไม่ว่าง |
| `Areas/Admin/Controllers/WidgetAjaxController.cs` | `Manage()` เขียนใหม่: อ่าน `pb_mod_name` → โมดูลตารางเดี่ยว (`*_single`/`web_home_*`) เปิด `Edit/<id แรก>`, อื่น ๆ เปิด list · ไม่มี = view แสดง "ไม่มีข้อมูลให้แก้ไข" (`Views/WidgetAjax/Manage.cshtml`) |
| `Areas/Admin/Views/Widget/Create.cshtml`, `Edit.cshtml` | ช่อง **Section Key** (+ คำอธิบายช่อง Mod Name) |
| `docs/sql/2026-09-18-web-widget-section-key.sql` | ALTER `web_widget` ADD `section_key`, `pb_section_key` nvarchar(100) + สิทธิ์ `CMSPage` ของ access_id 1 → add/delete/move/status = 0 (idempotent) — **deploy ต้องรัน** |
| `docs/sql/2026-09-18-delete-sam-cms-widgets.sql` | ลบ `web_cms_page` ที่ไม่ใช่ id 1, `web_widget` ที่ `section_key` ว่าง, กลุ่มที่ไม่มีลูก, `widget2`/`group2` ทั้งหมด (idempotent) — **deploy ต้องรัน** · dump ก่อนลบอยู่ `docs/backup-sam-widgets/*.txt` (เฉพาะเครื่อง dev ไม่ track) |
| `docs/sql/seed-ref-home-widgets.sql` | seed 3 กลุ่ม + 18 widget (HTML snapshot 426 KB) + `box_layout` ของ id 1 — **อ้างอิง / ใช้ตอน deploy** (id จะไม่ตรง dev; script คำนวณ `box_layout` เอง) · สร้างจาก `scratchpad/gen_seed.py` ซึ่งอ่าน `scratchpad/widgets/<Key>.html` ที่ตัดจาก `curl http://localhost:5310/salepage` (ตัด id/aria-labelledby ออก) |
| `wwwroot/Files/Site0/1/widget_icons/assetplus/` | **ไอคอน glyph PNG 128×128** `icon-<SectionKey>.png` ×6 (ขาว) + `icon-group-{default,modern,classic}.png` ×3 (น้ำเงิน) สร้างด้วย ComfyUI 17 ก.ย. 2569 (สคริปต์ `docs/comfyui-icons.py` + `docs/comfyui-icons.jobs.json`) · ไฟล์ screenshot เดิม `<Key>.jpg`/`group-v*.jpg` ยังอยู่แต่ไม่ถูกอ้าง — **ไม่ไปกับ git/publish ต้องอัปโหลดเอง** |
| `CLAUDE.md`, `docs/backend-menu-status.html` (เปิด 35 / ปิด 144), `docs/preview-spec.md`, ไฟล์นี้ | เอกสาร |

### 14.4 ไฟล์ที่แก้ (front-end — commit `3f4d86f` แล้ว)

| ไฟล์ | สิ่งที่ทำ |
|---|---|
| `Services/IHomeLayoutService.cs` (ใหม่) | `IReadOnlyList<string> GetSectionKeys()` |
| `Services/SqlHomeLayoutService.cs` (ใหม่) | อ่าน `web_cms_page` id 1 (`preview.Cols(…, "pb_box_layout")`) → token `wg_<n>` → `web_widget.pb_section_key` → กรองด้วย `KnownKeys` (18 key) · key ซ้ำ/ไม่รู้จัก/id ไม่มี = ข้าม (log warning) · layout ว่าง = `[]` · **อ่านแถวไม่ได้ (DB ล้ม) = `DefaultSections` (Version 1)** |
| `Models/Home/HomeViewModel.cs` | `Sections` |
| `Controllers/HomeController.cs` | inject `IHomeLayoutService` · `BuildHomeViewModel` ใส่ `Sections` |
| `Views/Home/Index.cshtml` | `foreach (var sectionKey in Model.Sections)` → `baseKey` (ตัด V2/V3) → เลือก model → `<partial name="Partials/_{sectionKey}">` (NavSummary null = ข้าม) |
| `Helpers/PreviewMap.cs` | `new("CMSPage", "web_cms_page", NoModule, BoxDataHome)` |
| `Program.cs` | ลงทะเบียน `SqlHomeLayoutService` · middleware ส่ง `Access-Control-Allow-Origin: <AdminURL>` ให้ `.woff2/.woff/.ttf/.otf` (แท็บ builder ฝั่ง admin โหลด CSS ของเว็บนี้ข้าม origin — ไม่มีจะฟอนต์/ไอคอนเพี้ยน) |
| `CLAUDE.md` | ตารางสถานะ + รายชื่อไฟล์ที่ต่างจาก static |

### 14.5 ข้อมูลใน DB ตอนนี้

- `[2026_web_widget_group]`: id 4 `DEFAULT` sort 10 · id 5 `MODERN` sort 20 · id 6 `CLASSIC` sort 30 · รูป `widget_icons/assetplus/icon-group-{default,modern,classic}.png` (17 ก.ย. ผู้ใช้สั่งตัด "(Version n)" + เปลี่ยนเป็นไอคอน)
- `[2026_web_widget]` (cat_id 4/5/6 · sort 10–60 · `section_key`):
  28 Hero · 29 NavPrices · 30 FeaturedFunds · 31 ExploreThemes · 32 Insights · 33 Distributors ·
  34–39 = `…V2` เรียงเดียวกัน · 40–45 = `…V3` · `mod_name` = `HomeImageSlide` เฉพาะ hero 3 ตัว (28/34/40) · `info`/`pb_info` = HTML `<section>` snapshot (hero = template REPEAT)
  · **`title` = ชื่อ section ไทยล้วน** (แบนเนอร์หน้าแรก / มูลค่าหน่วยลงทุน / กองทุนแนะนำประจำเดือน / เปิดมุมมองลงทุนตามเทรนด์ / บทความ / กิจกรรม / ข่าวประกาศ / ตัวแทนขาย — เหมือนกันทั้ง 3 เวอร์ชัน, 17 ก.ย. ผู้ใช้สั่งตัด "(Hero) — DEFAULT") · `img1` = `widget_icons/assetplus/icon-<Key ไม่มี V2/V3>.png` (เวอร์ชันเดียวกันใช้ไอคอนเดียวกัน)
- `[2026_web_cms_page]` id 1: `is_home 1`, `page_type 3`, `pb_status 1`, `box_layout = pb_box_layout = wg_28,wg_29,wg_30,wg_31,wg_33,wg_32` (**ผู้ใช้จัดเองตอน 16:33** — ตัวแทนขายก่อนบทความ ห้ามคืนเป็นลำดับเดิม)
- สิทธิ์ `[2026_web_admin_module]`: `CMSPage` can_edit/approve = 1 อื่น 0 · `Widget`/`WidgetGroup` = 1 ทั้งหมด

### 14.6 วิธีเพิ่ม/แก้ widget ในอนาคต (สูตร)

1. **เพิ่ม section ใหม่บนหน้าแรก**: สร้าง partial `Views/Home/Partials/_<Key>.cshtml` (+ `V2`/`V3` ถ้ามี) → เพิ่ม `Key` ใน `SqlHomeLayoutService.KnownKeys` + กิ่ง `baseKey switch` ใน `Index.cshtml` (เลือก model) → เพิ่มแถว `web_widget` ผ่านเมนู "Widget ทั้งหมด" (กลุ่ม, หัวข้อ, ICON, Section Key = `<Key>`, HTML Code = `<section>` ตัวอย่าง) → Approve → ลากใน builder
2. **widget ที่ตัวอย่างต้องดึงข้อมูลจริง**: ใส่ `Mod Name` = ชื่อโมดูลใน `AllModule()` แล้วเขียน HTML Code ด้วย token `|||pb_title|||` (แถวแรก) หรือ `|||REPEAT|||…|||/REPEAT|||` (ทุกแถว `show_front=1`) หรือ `|||Module:pb_col|||` / `|||REPEAT:Module|||` (โมดูลอื่น) — ดู `WidgetAjaxController.ReplaceTokenByModule()` · path รูปใน DB `Files/...` ให้เขียน `/|||pb_img1|||`
3. **ข้อความใน section ให้แก้จากหลังบ้าน** (ยังไม่ทำ — ผู้ใช้เลือก hardcode): เปิด `HomeSamText2–6` (`web_core_single` module 2–6) ตั้งชื่อ/ฟอร์มใหม่แบบ Header/Footer → front-end service อ่าน `pb_t*` ไปแทน hardcode ใน partial → ใส่ `mod_name` ใน widget ให้ตัวอย่างในหลังบ้านเติมค่าเอง
4. HTML ตัวอย่างมี path `/media/...` ของ front-end ได้เลย (`RewriteFrontAssetUrl()` ชี้ไป FrontURL ตอนแสดง) แต่รูปที่มาจากหลังบ้านให้ใช้ `/Files/...`

### 14.7 ผลทดสอบ regression 7 เมนู (18 ก.ย. หลังกู้ไฟล์ — ผ่านทั้งหมด ไม่พบบั๊ก)

ทำผ่าน Playwright (login จริง + reCAPTCHA) แถวทดสอบ `[TEST-…]` ลบผ่าน UI ตอนจบ · ตรวจ front-end ด้วย browser context ใหม่ทุกครั้ง + curl + DB · **snapshot 9 ตารางก่อน/หลัง เท่ากันทุกตาราง** (ต่างแค่ `updated_at`)

| เมนู | ที่ผ่าน |
|---|---|
| CMSPage | list 1 แถว ปุ่มถูก · `/Create` โดน access_denied · ลบ widget หมด → save/preview ว่าง/approve หน้าจริงว่าง · คละ 3 เวอร์ชัน → preview/approve ตรง · Unapprove · ลาก/ลบ/เลื่อน/ลากบนแคนวาส · พาเลตต์ซ่อน-คืนการ์ด |
| HomeIntroPage | validation · สร้าง/preview/approve/redirect/คุกกี้ · gate วันที่ 3 กรณี · status · แก้ไข → ฉบับร่าง · ลบ |
| HomePopUp | สร้าง 2 ใบ · ลิงก์ `_blank rel=noopener` / ไม่มี URL ไม่มี `<a>` · เด้ง/next/prev/dot/"ไม่ต้องแสดงอีก" คุกกี้ · preview ไม่สนคุกกี้ เปิดใบที่แก้ · Move มีผลทันที · status · แก้ไข · ลบ |
| HomeHeader | validation · ลบรูปมี swal · แก้ alt+โลโก้ → preview th/en (EN fallback) → approve ทุกหน้า → คืนค่า |
| HomeFooter | validation URL/อีเมล · แก้ 12 ช่อง → preview/approve หน้าแรก+salepage → คืนค่า |
| HomeSEO | ไม่มี preview (404) · title/og/keywords/โค้ดฝัง head+body ทุกหน้า · ฟอร์มไม่ล็อกตอนรออนุมัติ (SAM) |
| HomeImageSlide | validation TH/EN/วันที่ · ช่วงวันที่อนาคต → ซ่อน · ปักหมุดขึ้นแรก · status · Move · ลบรูป Mobile → fallback PC · ลบ → sort คืน 10/20/30 |

### 14.8 เหตุการณ์ที่ต้องรู้: `AdminMenu.cs` เคยหายแล้วกู้คืน

- ผมสั่ง `git checkout -- Areas/Admin/Helpers/AdminMenu.cs` โดยพลาด → งานค้าง commit ของ session 1–2 ในไฟล์นั้นหาย · กู้คืนโดย **replay ทุก Edit/Bash จาก transcript** (`~/.claude/projects/d--Project-admin-assetfund-co-th-2026-core-admin/{4baeaceb,bee81cc1}*.jsonl`) ลงบน `git show HEAD:…` ตามลำดับเวลา 16 ขั้น → ยืนยันด้วยเลขบรรทัด 6 จุด (`#region ผู้ดูแลระบบ` 628, `#region Widget` 643, `Name = "CMSPage"` 1031, `HomeSamText` 1228, `Widget / CMS Group` 5494, `Widget2` 5610) ตรงกับที่จดไว้ก่อนเกิดเหตุทุกจุด + build ผ่าน + regression 7 เมนูผ่าน → ถือว่ากู้ครบ
- กฎ: **ห้าม `git checkout --` / `git restore` / `git stash` ใน repo admin** (บันทึกใน memory `never-git-checkout-uncommitted` แล้ว)

### 14.9 เทคนิค/กับดักใหม่ของ session นี้ (นอกเหนือจาก §7.4b, §8 ข้อ 20–24)

- Bash tool: heredoc ที่มี python `'''` พัง → เขียนสคริปต์ด้วย Write แล้ว `python file.py` · `sqlcmd` ห้ามใช้ `-W` คู่ `-y 0`
- PowerShell: `$b -notmatch "..."` บน array คืน array → `if` เป็นจริงเสมอ; เช็ค `$LASTEXITCODE` · build front-end ไปที่ `%TEMP%\assetfund-7310-bin` ต้อง**ฆ่า process 7310 ของเราก่อน** ไม่งั้น MSB3027 (dll ถูกล็อก)
- datepicker (XDSoft) ในฟอร์มรับค่าจากการพิมพ์ (`fill` + Tab) เท่านั้น — ตั้ง `.val()` ผ่าน jQuery จะข้าม validation ช่วงวันที่ · ปุ่ม Save ถูก datepicker บัง → เรียก `sConfirmCustom(document.getElementById('fm_cms'))` ตรง ๆ ได้
- front-end encode ไทยเป็น `&#xE01;` — marker ทดสอบใช้ ASCII (`[TEST-…]`) · `ctx.request` ใช้คุกกี้ร่วมกับ context → ทดสอบ redirect intro ต้องใช้ context ใหม่
- หน้า builder: ห้าม `networkidle` · ลากจากพาเลตต์ต้องยิง synthetic DnD (§7.4b) · วางท้ายแคนวาส = dispatch ที่การ์ดสุดท้ายตำแหน่ง `bottom-4` (ถ้าแคนวาสว่างใช้ container)

### 14.10 แผนงานต่อไป (เรียงตามที่ผู้ใช้น่าจะสั่ง — รายละเอียดใน §12)

1. **ข้อความในแต่ละ section ของหน้าแรกให้แก้ได้** (§14.6 ข้อ 3) — ต่อยอดจาก widget โดยตรง
2. **เมนู footer 5 คอลัมน์ + ลิงก์นโยบาย** (§12.1) — ผู้ใช้บอกไว้ตอน Footer ว่าจะทำเมนูแยก · `CMSPageFooter1/2` ของ SAM ใช้ `web_cms_page_footer1/2` (ยังปิด, ยังมีข้อมูล SAM) — ต้องคุยโครงสร้างก่อน
3. **เมนูหลัก header** — ผู้ใช้ตัดสินใจแล้วว่า `web_cms_page` ไม่ใช้เป็นเมนูเว็บ (เหลือหน้าแรก) → ถ้าทำต้องออกแบบใหม่
4. **ต่อข้อมูลกองทุน/ข่าว/ประกาศเข้า front-end** (§12.2) — เมนู `Ap*` มีครบ, front-end ยัง mock · Preview ของตาราง `tb_*` ยังไม่มีกลไก
5. commit ฝั่ง admin — ผู้ใช้ทำเองแล้ว (`f175199`) เหลือเอกสาร 2 ไฟล์ · **reCAPTCHA คีย์จริง** · งาน deploy §13
