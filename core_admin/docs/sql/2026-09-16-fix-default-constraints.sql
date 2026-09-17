-- แก้ DEFAULT constraint วันที่ที่เป็น literal '...+07' (SQL Server แปลงไม่ได้) ใน 2026_* ทุกตาราง
-- คัดลอกจาก scratchpad ของ session ที่ทำงานจริงเมื่อ 16 ก.ย. 2569 เพื่อเก็บถาวร (scratchpad เป็นโฟลเดอร์ชั่วคราว)
-- ⚠ ต้องรันบน DB เซิร์ฟเวอร์จริงก่อนใช้หลังบ้าน (dev รันแล้ว) · idempotent: ไม่เจอแถวที่ต้องแก้ก็ไม่ทำอะไร · ตรวจผล still_bad ต้อง = 0

SET NOCOUNT ON; SET XACT_ABORT ON;
BEGIN TRAN;
DECLARE @sql nvarchar(max) = N'';
SELECT @sql = @sql +
  N'ALTER TABLE [' + t.name + N'] DROP CONSTRAINT [' + d.name + N']; ' +
  N'ALTER TABLE [' + t.name + N'] ADD CONSTRAINT [' + d.name + N'] DEFAULT (' +
      REPLACE(d.definition, N'+07''', N' +07:00''') + N') FOR [' + c.name + N']; '
FROM sys.default_constraints d
JOIN sys.tables t ON t.object_id = d.parent_object_id
JOIN sys.columns c ON c.object_id = d.parent_object_id AND c.column_id = d.parent_column_id
WHERE t.name LIKE '2026[_]%' AND d.definition LIKE '%+07'')';
EXEC sp_executesql @sql;
COMMIT;
SELECT COUNT(*) still_bad FROM sys.default_constraints d JOIN sys.tables t ON t.object_id=d.parent_object_id WHERE t.name LIKE '2026[_]%' AND d.definition LIKE '%+07'')';
SELECT name, definition FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID('[2026_web_home_intro_page]') AND definition LIKE '%2023%';
