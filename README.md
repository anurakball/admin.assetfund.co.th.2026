# admin.assetfund.co.th.2026

หลังบ้านของเว็บไซต์ ASSET PLUS - Fund Management (ASP.NET Core 10 MVC) — โค้ดทั้งหมดอยู่ใน `core_admin/`
รายละเอียดสถาปัตยกรรม/กฎการทำงานอยู่ที่ `core_admin/CLAUDE.md`

## ตั้งเครื่องใหม่หลัง clone

1. **ติดตั้ง** .NET 10 SDK และ SQL Server (MySQL `sam_npa` ใช้เฉพาะเมนู NPA ที่ยกมาจาก SAM — ไม่มีก็เปิดระบบได้)
2. **ฐานข้อมูล** — DB ไม่อยู่ใน git: restore `asset_plus_uat` จาก backup ของเครื่อง dev / เซิร์ฟเวอร์
   แล้วรันสคริปต์ใน `core_admin/docs/sql/` ที่ DB นั้นยังไม่มี (คำอธิบายแต่ละไฟล์อยู่ `core_admin/docs/CMS-MENU-HANDOFF.md`)
3. **ไฟล์ตั้งค่า** — ในโฟลเดอร์ `core_admin/`
   ```bash
   copy appsettings.Development.json.example appsettings.Development.json
   ```
   แก้ `DBConnection` ให้ตรง SQL Server ของเครื่อง (ค่าในไฟล์ example คือ `.` / `sa` / `sasa`)
   รหัสผ่านอื่น (`MailSettings`, `API`, `LINEAPI`) เว้นว่างได้ — ระบบเปิดได้ แค่ส่งอีเมล/เรียก API ไม่ได้
4. **รัน**
   ```bash
   cd core_admin
   dotnet run --launch-profile https     # https://localhost:7300
   ```
   ครั้งแรกถ้า browser ไม่เชื่อใบรับรอง dev ให้รัน `dotnet dev-certs https --trust`
5. **เข้าระบบ** ด้วยบัญชีใน `[2026_web_admin]` — บน localhost ข้าม reCAPTCHA ให้อัตโนมัติ (`GoogleReCaptcha:SkipOnLocalhost`)

## อะไรอยู่ / ไม่อยู่ใน git

| อยู่ใน git | ไม่อยู่ (ต้องหามาเอง) |
|---|---|
| โค้ด, view, `wwwroot/` (css/js/lib/โลโก้) | `appsettings.Development.json` (ความลับ — copy จาก `.example`) |
| รูปพื้นหลังหน้า Login `wwwroot/images/bg/bg0–9.jpg` | ฐานข้อมูล `asset_plus_uat` |
| รูปที่ข้อมูลตั้งต้นของเมนู CMS อ้างถึง (`wwwroot/Files/Site0/1/...` บางไฟล์) | ไฟล์อื่นที่ผู้ใช้อัปโหลดผ่าน elFinder ใน `wwwroot/Files/` |
| สคริปต์ SQL ที่ต้องรัน `core_admin/docs/sql/` | XML ของ ws_schedule (`App_Data/`) — ระบบสร้างให้เองตอนรัน |
| เอกสารงาน CMS / audit ใน `core_admin/docs/` | ไฟล์อัปโหลดของเมนู `tb_*` (อยู่ในโฟลเดอร์ของเว็บเดิม — ตั้ง `LegacyUpload:Path`) |

กฎ ignore ทั้งหมดอยู่ที่ `core_admin/.gitignore` (ตัวหลัก) — เพิ่มไฟล์ที่โค้ดต้องใช้ตอนรันเมื่อไร ต้องตรวจว่าไม่ถูก ignore
