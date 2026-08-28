using thaicredit_hr_admin.Areas.Admin.Models;

namespace thaicredit_hr_admin.Areas.Admin.Helpers
{
    public class AdminMenu
    {
        public List<AdminMenuModel>? Menu()
        {
            var listMenu = new List<AdminMenuModel>();

            #region หน้าเว็บไซต์
            listMenu.Add(new()
            {
                Title = "หน้าเว็บไซต์",
                Icon = "fa-solid fa-house",
                SubMenu = new()
                {
                    new(){Title = "จัดการเมนูเว็บไซต์",ModuleName = "CMSPage",Link = "CMSPage",Icon = "fa-solid fa-sitemap"},
                    new(){Title = "หน้า Intro Page",ModuleName = "HomeIntroPage",Link = "HomeIntroPage",Icon = "fa-regular fa-window-restore"},
                    new(){Title = "หน้า Pop-Up",ModuleName = "HomePopUp",Link = "HomePopUp",Icon = "fa-solid fa-clone"},
                    new(){Title = "โลโก้ / Template",ModuleName = "HomeHeader",Link = "HomeHeader",Icon = "fa-solid fa-book-atlas"},
                    new(){Title = "จัดการเมนู Footer 1",ModuleName = "CMSPageFooter1",Link = "CMSPageFooter1",Icon = "fa-solid fa-sitemap"},
                    new(){Title = "จัดการเมนู Footer 2",ModuleName = "CMSPageFooter2",Link = "CMSPageFooter2",Icon = "fa-solid fa-sitemap"},
                    new(){Title = "ปรับแต่ง Footer",ModuleName = "HomeFooter",Link = "HomeFooter",Icon = "fa-regular fa-copyright"},
                    new(){Title = "SEO & Code",ModuleName = "HomeSEO",Link = "HomeSEO",Icon = "fa-solid fa-sliders"}
                }
            });
            #endregion

            #region ข้อมูลหน้าแรก
            listMenu.Add(new()
            {
                Title = "ข้อมูลหน้าแรก",
                Icon = "fa-solid fa-book-open",
                SubMenu = new()
                {
                    new(){Title = "รูปสไลด์หน้าแรก",ModuleName = "HomeImageSlide",Link = "HomeImageSlide",Icon = "fa-regular fa-images"},
                    new(){Title = "ตั้งค่ารูปสไลด์",ModuleName = "HomeImageConf",Link = "HomeImageConf",Icon = "fa-solid fa-sliders"},
                    new(){Title = "SAM ใส่ใจ",ModuleName = "HomeSamText",Link = "HomeSamText",Icon = "fa-solid fa-list-ol"},
                    new(){Title = "ภาพรวมการบริหารหนี้",ModuleName = "HomeSamText2",Link = "HomeSamText2",Icon = "fa-regular fa-note-sticky"},
                    new(){Title = "ปิดหนี้ไว ไปต่อได้",ModuleName = "HomeSamText3",Link = "HomeSamText3",Icon = "fa-solid fa-percent"},
                    new(){Title = "ทรัพย์เด่นและน่าสนใจ",ModuleName = "HomeSamText4",Link = "HomeSamText4",Icon = "fa-solid fa-building-circle-check"},
                    new(){Title = "คุณกำลังมองหาอะไร",ModuleName = "HomeSamText5",Link = "HomeSamText5",Icon = "fa-solid fa-magnifying-glass-location"},
                    new(){Title = "บ้านเด่นทำเลดี",ModuleName = "HomeSamText6",Link = "HomeSamText6",Icon = "fa-solid fa-map-location-dot"},
                    new(){Title = "วิสัยทัศน์",ModuleName = "HomeSamText7",Link = "HomeSamText7",Icon = "fa-solid fa-align-center"}
                }
            });
            #endregion

            #region เนื้อหาหน้าแรก
            listMenu.Add(new()
            {
                Title = "เนื้อหาหน้าแรก",
                Icon = "fa-solid fa-bookmark",
                SubMenu = new()
                {
                    new(){Title = "Hero Banner",ModuleName = "HeroBanner",Link = "HeroBanner",Icon = "fa-regular fa-image"},
                    new(){Title = "บทนำ",ModuleName = "MicrositeIntro",Link = "MicrositeIntro",Icon = "fa-solid fa-align-left"},
                    new(){Title = "บทบาทของเรา",ModuleName = "MicrositeRole",Link = "MicrositeRole",Icon = "fa-solid fa-icons"},
                    new(){Title = "แบนเนอร์ครึ่งจอ",ModuleName = "MicrositeBannerHalf",Link = "MicrositeBannerHalf",Icon = "fa-solid fa-panorama"},
                    new(){Title = "เอกสารที่เกี่ยวข้อง",ModuleName = "MicrositeDoc",Link = "MicrositeDoc",Icon = "fa-solid fa-file-pdf"},
                    new(){Title = "สิทธิพิเศษ",ModuleName = "MicrositeBenefit",Link = "MicrositeBenefit",Icon = "fa-solid fa-circle-check"},
                    new(){Title = "ทรัพย์ที่ร่วมรายการ",ModuleName = "MicrositeAssetHead",Link = "MicrositeAssetHead",Icon = "fa-solid fa-heading"},
                    new(){Title = "ทรัพย์ที่ร่วมรายการ",ModuleName = "MicrositeAsset",Link = "MicrositeAsset",Icon = "fa-solid fa-building"},
                    new(){Title = "เงื่อนไขโครงการ",ModuleName = "MicrositeTerm",Link = "MicrositeTerm",Icon = "fa-solid fa-table"},
                    new(){Title = "จุดเด่นโครงการ",ModuleName = "MicrositeHighlight",Link = "MicrositeHighlight",Icon = "fa-solid fa-star"},
                    new(){Title = "สอบถามเพิ่มเติม (QR)",ModuleName = "MicrositeContactQr",Link = "MicrositeContactQr",Icon = "fa-solid fa-qrcode"},
                    new(){Title = "ตัวเลขความสำเร็จ",ModuleName = "MicrositeCounter",Link = "MicrositeCounter",Icon = "fa-solid fa-chart-simple"},
                    new(){Title = "แนวทางที่เราช่วยได้",ModuleName = "MicrositeHelp",Link = "MicrositeHelp",Icon = "fa-solid fa-hands-holding-circle"},
                    new(){Title = "เอกสารที่ต้องเตรียม",ModuleName = "MicrositePrepare",Link = "MicrositePrepare",Icon = "fa-solid fa-clipboard-list"},
                    new(){Title = "ข้อมูลติดต่อกลับ",ModuleName = "MicrositeLead",Link = "MicrositeLead",Icon = "fa-solid fa-address-card"},
                    new(){Title = "คำถามที่พบบ่อย",ModuleName = "MicrositeFaq",Link = "MicrositeFaq",Icon = "fa-solid fa-circle-question"}
                }
            });
            #endregion

            #region เกี่ยวกับเรา
            listMenu.Add(new()
            {
                Title = "เกี่ยวกับเรา",
                Icon = "fa-solid fa-store",
                SubMenu = new()
                {
                    new(){Title = "วิสัยทัศน์/พันธกิจ",ModuleName = "AboutVision",Link = "AboutVision",Icon = "fa-solid fa-braille"},
                    new(){Title = "ประวัติความเป็นมา",ModuleName = "AboutHistory",Link = "AboutHistory",Icon = "fa-solid fa-landmark"},
                    new(){Title = "โครงสร้างองค์กร",ModuleName = "AboutStructure",Link = "AboutStructure",Icon = "fa-solid fa-timeline"},
                    new(){Title = "รายงานประจำปี",ModuleName = "AboutAnnual",Link = "AboutAnnual",Icon = "fa-solid fa-scroll"},
                    new(){Title = "รายงานการเงิน",ModuleName = "AboutFin",Link = "AboutFin",Icon = "fa-solid fa-hand-holding-dollar"},
                    new(){Title = "คณะกรรมการบริษัท",ModuleName = "AboutBoard1",Link = "AboutBoard1",Icon = "fa-solid fa-people-line"},
                    new(){Title = "คณะกรรมการอื่นๆ (กลุ่ม)",ModuleName = "AboutBoard2cat",Link = "AboutBoard2cat",Icon = "fa-solid fa-people-line"},
                    new(){Title = "คณะกรรมการอื่นๆ",ModuleName = "AboutBoard2",Link = "AboutBoard2",Icon = "fa-solid fa-people-line"},
                    new(){Title = "คณะผู้บริหารระดับสูง (กลุ่ม)",ModuleName = "AboutBoard3cat",Link = "AboutBoard3cat",Icon = "fa-solid fa-people-line"},
                    new(){Title = "คณะผู้บริหารระดับสูง",ModuleName = "AboutBoard3",Link = "AboutBoard3",Icon = "fa-solid fa-people-line"},
                    new(){Title = "คณะผู้บริหารฝ่าย (สายงาน)",ModuleName = "AboutBoard4cat",Link = "AboutBoard4cat",Icon = "fa-solid fa-people-line"},
                    new(){Title = "คณะผู้บริหารฝ่าย (กลุ่ม)",ModuleName = "AboutBoard4sub",Link = "AboutBoard4sub",Icon = "fa-solid fa-people-line"},
                    new(){Title = "คณะผู้บริหารฝ่าย",ModuleName = "AboutBoard4",Link = "AboutBoard4",Icon = "fa-solid fa-people-line"},
                    new(){Title = "บริหารสินทรัพย์ด้อยคุณภาพ",ModuleName = "AboutText1",Link = "AboutText1",Icon = "fa-solid fa-tree-city"},
                    new(){Title = "บริหารทรัพย์สินรอการขาย",ModuleName = "AboutText2",Link = "AboutText2",Icon = "fa-solid fa-building-user"},
                    new(){Title = "สร้างโอกาสทางธุรกิจ บสส.",ModuleName = "AboutText3",Link = "AboutText3",Icon = "fa-solid fa-user-tie"}
                }
            });
            #endregion

            #region ทรัพย์สินรอการขาย
            listMenu.Add(new()
            {
                Title = "ทรัพย์สินรอการขาย",
                Icon = "fa-solid fa-building-user",
                SubMenu = new()
                {
                    new(){Title = "ขั้นตอนการซื้อทรัพย์",ModuleName = "NpaText1",Link = "NpaText1",Icon = "fa-solid fa-building-user"}
                }
            });
            #endregion

            #region บริหารหนี้ด้อยคุณภาพ
            listMenu.Add(new()
            {
                Title = "บริหารหนี้ด้อยคุณภาพ",
                Icon = "fa-solid fa-list-check",
                SubMenu = new()
                {
                    new() { Title = "ภาพรวมการบริหารหนี้", ModuleName = "DebtText1", Link = "DebtText1", Icon = "fa-solid fa-tachograph-digital" },
                    new() { Title = "ขั้นตอนการปรับโครงสร้างหนี้", ModuleName = "DebtText2", Link = "DebtText2", Icon = "fa-solid fa-stairs" },
                    new() { Title = "สิทธิประโยชน์ลูกหนี้", ModuleName = "DebtText3", Link = "DebtText3", Icon = "fa-regular fa-id-card" },
                    /*new() { Title = "ดาวน์โหลดแบบฟอร์ม", ModuleName = "DebtText4", Link = "DebtText4", Icon = "fa-solid fa-file-arrow-down" },*/
                    new() { Title = "ลงทะเบียนปรับโครงสร้างหนี้", ModuleName = "DebtText5", Link = "DebtText5", Icon = "fa-solid fa-pen-to-square" },
                    new() { Title = "คลินิกแก้หนี้", ModuleName = "DebtText6", Link = "DebtText6", Icon = "fa-solid fa-house-medical" } 
                }
            });
            #endregion

            #region ข่าวประชาสัมพันธ์
            listMenu.Add(new()
            {
                Title = "ข่าวประชาสัมพันธ์",
                Icon = "fa-regular fa-newspaper",
                SubMenu = new()
                {
                    new() { Title = "ข่าวประชาสัมพันธ์ (กลุ่ม)", ModuleName = "NewsGroup", Link = "NewsGroup", Icon = "fa-regular fa-folder-open" },
                    new() { Title = "ข่าวประชาสัมพันธ์", ModuleName = "News", Link = "News", Icon = "fa-regular fa-newspaper" }
                }
            });
            #endregion

            #region บทความและวารสาร
            listMenu.Add(new()
            {
                Title = "บทความและวารสาร",
                Icon = "fa-solid fa-newspaper",
                SubMenu = new()
                {
                    new() { Title = "บทความและวารสาร (กลุ่ม)", ModuleName = "ArticleGroup", Link = "ArticleGroup", Icon = "fa-regular fa-folder-open" },
                    new() { Title = "บทความและวารสาร", ModuleName = "Article", Link = "Article", Icon = "fa-solid fa-newspaper" }
                }
            });
            #endregion

            #region วิดีโอ/สื่อประชาสัมพันธ์
            listMenu.Add(new()
            {
                Title = "วิดีโอ/สื่อประชาสัมพันธ์",
                Icon = "fa-solid fa-video",
                SubMenu = new()
                {
                    new() { Title = "รายการ (กลุ่ม)", ModuleName = "VDOGroup", Link = "VDOGroup", Icon = "fa-regular fa-folder-open" },
                    new() { Title = "รายการ", ModuleName = "VDOGroupsub", Link = "VDOGroupsub", Icon = "fa-solid fa-file-video" },
                    new() { Title = "วิดีโอ/สื่อประชาสัมพันธ์", ModuleName = "VDO", Link = "VDO", Icon = "fa-solid fa-video" }
                }
            });
            #endregion

            #region ประกาศ
            listMenu.Add(new()
            {
                Title = "ประกาศ",
                Icon = "fa-solid fa-bullhorn",
                SubMenu = new()
                {
                    new() { Title = "ประกาศอัตราดอกเบี้ย", ModuleName = "AnnounceRate", Link = "AnnounceRate", Icon = "fa-solid fa-percent" },
                    new() { Title = "ประกาศจาก NPL/NPA", ModuleName = "AnnounceNPA", Link = "AnnounceNPA", Icon = "fa-solid fa-city" }, 
                    new() { Title = "ประกาศทั่วไป (กลุ่ม)", ModuleName = "AnnounceOtherGroup", Link = "AnnounceOtherGroup", Icon = "fa-regular fa-folder-open" },
                    new() { Title = "ประกาศทั่วไป", ModuleName = "AnnounceOther", Link = "AnnounceOther", Icon = "fa-regular fa-file-lines" }
                }
            });
            #endregion

            #region ประกาศจัดซื้อจัดจ้าง
            listMenu.Add(new()
            {
                Title = "ประกาศจัดซื้อจัดจ้าง",
                Icon = "fa-regular fa-comment-dots",
                SubMenu = new()
                {
                    new() { Title = "ประกาศจัดซื้อจัดจ้าง", ModuleName = "AnnouncePro", Link = "AnnouncePro", Icon = "fa-regular fa-comment-dots" },
                    new() { Title = "ประเภท", ModuleName = "AnnounceProGroup", Link = "AnnounceProGroup", Icon = "fa-regular fa-folder-open" },
                    new() { Title = "วิธีการจัดซื้อ", ModuleName = "AnnounceProType", Link = "AnnounceProType", Icon = "fa-regular fa-message" },
                    new() { Title = "รายชื่อผู้ลงทะเบียน", ModuleName = "AnnounceSubmit", Link = "AnnounceSubmit", Icon = "fa-solid fa-user-check" }
                }
            });
            #endregion

            #region ร่วมงานกับเรา
            listMenu.Add(new()
            {
                Title = "ร่วมงานกับเรา",
                Icon = "fa-solid fa-briefcase",
                SubMenu = new()
                {
                    new() { Title = "ตำแหน่งงาน", ModuleName = "AnnounceJob", Link = "AnnounceJob", Icon = "fa-solid fa-briefcase" },
                    //new() { Title = "รายชื่อผู้สมัครงาน", ModuleName = "AnnounceJobSubmit", Link = "AnnounceJobSubmit", Icon = "fa-solid fa-user-check" },
                    new() { Title = "รายชื่อผู้สมัครงาน", ModuleName = "AnnounceJobSubmitFull", Link = "AnnounceJobSubmitFull", Icon = "fa-solid fa-clipboard-user" },
                    new() { Title = "รูปประกอบ", ModuleName = "JobCMS", Link = "JobCMS", Icon = "fa-solid fa-people-roof" }
                }
            });
            #endregion

            #region ติดต่อเรา
            listMenu.Add(new()
            {
                Title = "ติดต่อเรา",
                Icon = "fa-solid fa-headset",
                SubMenu = new()
                {
                    new() { Title = "สำนักงานใหญ่/สาขา", ModuleName = "ContactOffice", Link = "ContactOffice", Icon = "fa-solid fa-building" },
                    new() { Title = "ช่องทางร้องเรียน/ข้อเสนอแนะ", ModuleName = "ContactCMS", Link = "ContactCMS", Icon = "fa-solid fa-bullhorn" },
                    new() { Title = "รายชื่อผู้ติดต่อ", ModuleName = "ContactSubmit", Link = "ContactSubmit", Icon = "fa-solid fa-user-check" },
                    
                    
                }
            });
            #endregion
              
            #region ดาวน์โหลด
            listMenu.Add(new()
            {
                Title = "ดาวน์โหลด",
                Icon = "fa-solid fa-file-arrow-down",
                SubMenu = new()
                {
                    new() { Title = "ดาวน์โหลด (กลุ่ม)", ModuleName = "DownloadGroup", Link = "DownloadGroup", Icon = "fa-regular fa-folder-open" },
                    new() { Title = "ดาวน์โหลด", ModuleName = "Download", Link = "Download", Icon = "fa-solid fa-file-arrow-down" },
                    //new() { Title = "รายชื่อผู้กรอกข้อมูล", ModuleName = "DownloadSubmit", Link = "DownloadSubmit", Icon = "fa-solid fa-user-check" },
                }
            });
            #endregion

            #region คำถามที่พบบ่อย
            listMenu.Add(new()
            {
                Title = "คำถามที่พบบ่อย",
                Icon = "fa-regular fa-circle-question",
                SubMenu = new()
                {
                    new() { Title = "คำถามที่พบบ่อย (กลุ่ม)", ModuleName = "FAQGroup", Link = "FAQGroup", Icon = "fa-regular fa-folder-open" },
                    new() { Title = "คำถามที่พบบ่อย", ModuleName = "FAQ", Link = "FAQ", Icon = "fa-regular fa-circle-question" },
                }
            });
            #endregion

            #region ลิงค์หน่วยงาน
            listMenu.Add(new()
            {
                Title = "ลิงค์หน่วยงาน",
                Icon = "fa-solid fa-link",
                SubMenu = new()
                {
                    new() { Title = "ลิงค์หน่วยงาน (กลุ่ม)", ModuleName = "LinkGroup", Link = "LinkGroup", Icon = "fa-regular fa-folder-open" },
                    new() { Title = "ลิงค์หน่วยงาน", ModuleName = "Link", Link = "Link", Icon = "fa-solid fa-link" },
                    new() { Title = "แลกเปลี่ยนลิงค์", ModuleName = "LinkCMS", Link = "LinkCMS", Icon = "fa-brands fa-ubuntu" },
                    new() { Title = "รายชื่อผู้แลกเปลี่ยน", ModuleName = "LinkSubmit", Link = "LinkSubmit", Icon = "fa-solid fa-user-check" },
                }
            });
            #endregion

            #region โปรโมชั่น
            listMenu.Add(new()
            {
                Title = "โปรโมชั่น",
                Icon = "fa-solid fa-tags",
                SubMenu = new()
                {
                    new() { Title = "โปรโมชั่น (กลุ่ม)", ModuleName = "PromotionGroup", Link = "PromotionGroup", Icon = "fa-regular fa-folder-open" }, 
                    new() { Title = "โปรโมชั่น", ModuleName = "Promotion", Link = "Promotion", Icon = "fa-solid fa-tags" }
                }
            });
            #endregion

            #region Banner
            /*
            listMenu.Add(new()
            {
                Title = "Banner",
                Icon = "fa-regular fa-note-sticky",
                SubMenu = new()
                {
                    new() { Title = "กลุ่ม Banner", ModuleName = "BannerGroup", Link = "BannerGroup", Icon = "fa-regular fa-folder-open" }, 
                    new() { Title = "Banner ทั้งหมด", ModuleName = "Banner", Link = "Banner", Icon = "fa-regular fa-note-sticky" }
                }
            }); 
            */
            #endregion
             
            #region E-Form
            listMenu.Add(new()
            {
                Title = "E-Form",
                Icon = "fa-brands fa-wpforms",
                SubMenu = new()
                {
                    new() { Title = "E-Form (กลุ่ม)", ModuleName = "EFormGroup", Link = "EFormGroup", Icon = "fa-regular fa-folder-open" },
                    new() { Title = "E-Form", ModuleName = "EForm", Link = "EForm", Icon = "fa-solid fa-clipboard-list" },
                    new() { Title = "รายชื่อผู้กรอกข้อมูล", ModuleName = "EFormSubmit", Link = "EFormSubmit", Icon = "fa-solid fa-user-pen" },
                    /*new() { Title = "อีเมลแจ้งเตือน", ModuleName = "EFormEmail", Link = "EFormEmail", Icon = "fa-regular fa-envelope" }*/
                }
            });
            #endregion

            #region เมนู Footer
            listMenu.Add(new()
            {
                Title = "เมนู Footer",
                Icon = "fa-regular fa-window-maximize",
                SubMenu = new()
                {
                    new() { Title = "แบบสำรวจ", ModuleName = "FooterPoll", Link = "FooterPoll", Icon = "fa-solid fa-square-poll-vertical" },
                    new() { Title = "ประกาศความเป็นส่วนตัว", ModuleName = "FooterPrivacy1", Link = "FooterPrivacy1", Icon = "fa-solid fa-shield-halved" },
                    new() { Title = "รายชื่อผู้ยินยอม", ModuleName = "FooterPrivacy1Submit", Link = "FooterPrivacy1Submit", Icon = "fa-solid fa-shield-halved" },
                    new() { Title = "ข้อตกลงและเงื่อนไข", ModuleName = "FooterPrivacy2", Link = "FooterPrivacy2", Icon = "fa-regular fa-square-check" },
                    new() { Title = "รายชื่อผู้ยินยอม", ModuleName = "FooterPrivacy2Submit", Link = "FooterPrivacy2Submit", Icon = "fa-regular fa-square-check" },
                    new() { Title = "เปิดเผยข้อมูลความโปร่งใส", ModuleName = "FooterLink", Link = "FooterLink", Icon = "fa-brands fa-readme" }
                }
            });
            #endregion

            #region สมาชิก
            listMenu.Add(new()
            {
                Title = "สมาชิก",
                Icon = "fa-solid fa-users",
                SubMenu = new()
                {
                    new() { Title = "จัดการสมาชิก", ModuleName = "MemberList", Link = "MemberList", Icon = "fa-solid fa-users" }, 
                    new() { Title = "รายงานสมาชิก", ModuleName = "MemberReport", Link = "MemberReport", Icon = "fa-solid fa-chart-bar" }, 
                    new() { Title = "อีเมลเจ้าหน้าที่แจ้งเตือน", ModuleName = "MemberEmail", Link = "MemberEmail", Icon = "fa-regular fa-envelope" }, 
                    new() { Title = "ส่งข่าวสารสมาชิก", ModuleName = "MemberNews", Link = "MemberNews", Icon = "fa-regular fa-newspaper" }
                }
            });
            #endregion
            
            #region ตัวแทนขายทรัพย์ (Agent)
            listMenu.Add(new()
            {
                Title = "ตัวแทนขายทรัพย์ (Agent)",
                Icon = "fa-solid fa-users-line",
                SubMenu = new()
                {
                    new() { Title = "รายชื่อผู้สมัครตัวแทน", ModuleName = "AgentList", Link = "AgentList", Icon = "fa-regular fa-address-card" }, 
                    new() { Title = "นำเข้าข้อมูลผู้สมัคร", ModuleName = "AgentImport", Link = "AgentImport", Icon = "fa-solid fa-file-csv" }, 
                    new() { Title = "รายงานตัวแทน", ModuleName = "AgentReport", Link = "AgentReport", Icon = "fa-solid fa-newspaper" }, 
                    new() { Title = "อีเมลเจ้าหน้าที่แจ้งเตือน", ModuleName = "AgentEmail", Link = "AgentEmail", Icon = "fa-solid fa-envelope" },
                    new() { Title = "ส่งข่าวสารเอเจนต์", ModuleName = "AgentNews", Link = "AgentNews", Icon = "fa-solid fa-envelope" }
                }
            });
            #endregion
            
            #region ข้อมูลทรัพย์สิน (NPA)
            listMenu.Add(new()
            {
                Title = "ข้อมูลทรัพย์สิน (NPA)",
                Icon = "bi-house-door",
                SubMenu = new()
                {
                    new() { Title = "กำหนดสถานะทรัพย์สิน", ModuleName = "NpaStatus", Link = "NpaStatus", Icon = "fa-solid fa-house-circle-check" }, 
                    new() { Title = "รายชื่อทรัพย์ Wishlist", ModuleName = "NpaFav", Link = "NpaFav", Icon = "fa-solid fa-city" }, 
                    new() { Title = "รายการ ย่าน/ทำเล", ModuleName = "NpaLocate", Link = "NpaLocate", Icon = "fa-solid fa-tree-city" }, 
                    new() { Title = "รายการ Facility", ModuleName = "NpaFacility", Link = "NpaFacility", Icon = "fa-regular fa-hospital" },
                    new() { Title = "ยื่นข้อเสนอซื้อ", ModuleName = "NpaOffer", Link = "NpaOffer", Icon = "fa-solid fa-hand-holding-dollar" },
                    new() { Title = "อีเมลเจ้าหน้าที่แจ้งเตือน", ModuleName = "NpaOfferEmail", Link = "NpaOfferEmail", Icon = "fa-regular fa-envelope" },
                    //new() { Title = "รายงานการค้นหาข้อมูลทรัพย์สิน", ModuleName = "NpaReport", Link = "NpaReport", Icon = "fa-solid fa-chart-bar" }
                }
            });
            #endregion

            #region ลูกค้าปรับโครงสร้างหนี้
            listMenu.Add(new()
            {
                Title = "ลูกค้าปรับโครงสร้างหนี้",
                Icon = "fa-solid fa-house-user",
                SubMenu = new()
                {
                    //new() { Title = "แก้ไขรายละเอียดโครงการ", ModuleName = "NplWeb", Link = "NplWeb", Icon = "fa-solid fa-file-invoice" }, 
                    new() { Title = "รายชื่อผู้ลงทะเบียน (NPL)", ModuleName = "NplRegister", Link = "NplRegister", Icon = "fa-solid fa-list" }, 
                    new() { Title = "อีเมลเจ้าหน้าที่แจ้งเตือน", ModuleName = "NplEmail", Link = "NplEmail", Icon = "fa-regular fa-envelope" }, 
                    //new() { Title = "ข้อมูลลูกหนี้", ModuleName = "NplCustomer", Link = "NplCustomer", Icon = "fa-solid fa-user-group" }
                }
            });
            #endregion

            /* 
            //มีแล้วอยู่ด้านบน
            #region โครงการคลินิกแก้หนี้
            listMenu.Add(new()
            {
                Title = "โครงการคลินิกแก้หนี้",
                Icon = "fa-solid fa-building-circle-check",
                SubMenu = new()
                {
                    new() { Title = "แก้ไขรายละเอียดโครงการ", ModuleName = "CliWeb", Link = "CliWeb", Icon = "fa-brands fa-readme" }
                }
            });
            #endregion
            */

            #region แจ้งความประสงค์ซื้อทรัพย์
            listMenu.Add(new()
            {
                Title = "แจ้งความประสงค์ซื้อทรัพย์",
                Icon = "fa-solid fa-house-laptop",
                SubMenu = new()
                {
                    new() { Title = "รายการผู้สนใจซื้อทรัพย์", ModuleName = "BuyNPA", Link = "BuyNPA", Icon = "fa-solid fa-user-check" }, 
                    //new() { Title = "รายงานผู้สนใจซื้อทรัพย์", ModuleName = "BuyNPAReport", Link = "BuyNPAReport", Icon = "fa-regular fa-file-lines" }, 
                    new() { Title = "อีเมลเจ้าหน้าที่แจ้งเตือน", ModuleName = "BuyNPAEmail", Link = "BuyNPAEmail", Icon = "fa-regular fa-envelope" }
                }
            });
            #endregion
             
            #region นัดหมายชมทรัพย์
            /*listMenu.Add(new()
            {
                Title = "นัดหมายชมทรัพย์",
                Icon = "fa-regular fa-calendar-days",
                SubMenu = new()
                {
                    new() { Title = "รายการนัดหมายชมทรัพย์", ModuleName = "MeetNPA", Link = "MeetNPA", Icon = "fa-regular fa-calendar-check" }, 
                    new() { Title = "อีเมลเจ้าหน้าที่แจ้งเตือน", ModuleName = "MeetNPAEmail", Link = "MeetNPAEmail", Icon = "fa-regular fa-envelope" }
                }
            });*/
            #endregion
                    
            #region Subscription
            listMenu.Add(new()
            {
                Title = "Subscription",
                Icon = "fa-solid fa-envelope",
                SubMenu = new()
                {
                    new() { Title = "รายชื่อผู้ลงทะเบียน", ModuleName = "Subscription", Link = "Subscription", Icon = "fa-solid fa-user-check" }, new() { Title = "ส่งข่าวสารทางอีเมล", ModuleName = "SubscriptionEmail", Link = "SubscriptionEmail", Icon = "fa-solid fa-envelopes-bulk" }, new() { Title = "ข่าวสารทางอีเมลย้อนหลัง", ModuleName = "SubscriptionNews", Link = "SubscriptionNews", Icon = "fa-solid fa-clock-rotate-left" }
                }
            });
            #endregion
             
            #region Monitor
            listMenu.Add(new()
            {
                Title = "Monitor",
                Icon = "fa-solid fa-display",
                SubMenu = new()
                {
                    new() { Title = "Server Health", ModuleName = "ServerStatus", Link = "ServerStatus", Icon = "fa-solid fa-bars-progress" },  
                    new() { Title = "Error Logs", ModuleName = "ErrorLogs", Link = "ErrorLogs", Icon = "fa-solid fa-triangle-exclamation" },
                    new() { Title = "Google Analytics", ModuleName = "GoogleAnalytics", Link = "GoogleAnalytics", Icon = "fa-brands fa-google" },
                }
            });
            #endregion
             
            #region Setting
            listMenu.Add(new()
            {
                Title = "Setting",
                Icon = "fa-solid fa-gear",
                SubMenu = new()
                {
                    new() { Title = "File Manager", ModuleName = "FileManager", Link = "FileManager", Icon = "fa-solid fa-box-archive" }, 
                    new() { Title = "Google Map Key", ModuleName = "GoogleKey", Link = "GoogleKey", Icon = "fa-regular fa-map" }, 
                    new() { Title = "AI Settings", ModuleName = "AISetting", Link = "AISetting", Icon = "fa-brands fa-openai" }, 
                    //new() { Title = "LDAP Azure AD", ModuleName = "LDAPSetting", Link = "LDAPSetting", Icon = "fa-brands fa-microsoft" }
                }
            });
            #endregion

            #region Landing / Microsite
            listMenu.Add(new()
            {
                Title = "Landing / Microsite",
                Icon = "fa-solid fa-sitemap",
                SubMenu = new()
                {
                    new() { Title = "Microsite", ModuleName = "Microsite", Link = "Microsite", Icon = "fa-solid fa-sitemap" } 
                    /*
                    new() { Title = "Lead Form", ModuleName = "MicrositeForm", Link = "MicrositeForm", Icon = "fa-regular fa-file-lines" }, 
                    new() { Title = "รายชื่อผู้กรอกข้อมูล", ModuleName = "MicrositeSubmit", Link = "MicrositeSubmit", Icon = "fa-solid fa-user-check" }
                    */
                }
            });
            #endregion

            #region Logs
            listMenu.Add(new()
            {
                Title = "Logs",
                Icon = "fa-solid fa-magnifying-glass",
                SubMenu = new()
                {
                    new() { Title = "Activities สมาชิก", ModuleName = "LogsMember", Link = "LogsMember", Icon = "fa-solid fa-user-clock" }, 
                    new() { Title = "ให้ความยินยอม PDPA", ModuleName = "LogsPDPA", Link = "LogsPDPA", Icon = "fa-solid fa-shield-halved" },
                    new() { Title = "AI Services API", ModuleName = "LogsAI", Link = "LogsAI", Icon = "fa-solid fa-shield-halved" },
                    new() { Title = "CRM Data API", ModuleName = "LogsCRM", Link = "LogsCRM", Icon = "fa-solid fa-shield-halved" },
                    new() { Title = "LINE LIFF Data API", ModuleName = "LogsLINE", Link = "LogsLINE", Icon = "fa-solid fa-shield-halved" },
                    //new() { Title = "AD/LDAP Login API", ModuleName = "LogsLDAP", Link = "LogsLDAP", Icon = "fa-solid fa-shield-halved" },
                    //new() { Title = "จับคู่ทรัพย์สินอัตโนมัติ API", ModuleName = "LogsMatchAPI", Link = "LogsMatchAPI", Icon = "fa-solid fa-building" }, 
                    new() { Title = "Google Map API", ModuleName = "LogsGoogleMap", Link = "LogsGoogleMap", Icon = "fa-regular fa-map" },
                    new() { Title = "ค้นหาทรัพย์ NPA", ModuleName = "LogsNpaSearch", Link = "LogsNpaSearch", Icon = "fa-solid fa-magnifying-glass-location" }
                }
            }); 
            #endregion
             
            #region ผู้ดูแลระบบ
            listMenu.Add(new()
            {
                Title = "ผู้ดูแลระบบ",
                Icon = "bi-shield-lock",
                SubMenu = new()
                {
                    new() { Title = "สิทธิ์การใช้", ModuleName = "AdminAccess", Link = "AdminAccess", Icon = "bi-shield-lock" }, 
                    new() { Title = "ผู้ดูแลระบบ", ModuleName = "AdminUser", Link = "AdminUser", Icon = "bi-people" }, 
                    new() { Title = "Admin Logs", ModuleName = "AdminLog", Link = "AdminLog", Icon = "fa-solid fa-user-clock" }, 
                    new() { Title = "ผู้ใช้ที่ไม่เข้าใช้งาน", ModuleName = "AdminUserNotLogin", Link = "AdminUserNotLogin", Icon = "bi-person-x" }
                }
            });
            #endregion

            #region Widget
            /*
            listMenu.Add(new()
            {
                Title = "Widget",
                Icon = "bi bi-collection",
                SubMenu = new()
                {
                    new() { Title = "กลุ่ม Widget", ModuleName = "WidgetGroup", Link = "WidgetGroup", Icon = "bi bi-folder2" }, 
                    new() { Title = "Widget ทั้งหมด", ModuleName = "Widget", Link = "Widget", Icon = "bi bi-collection" }
                }
            });
            listMenu.Add(new()
            {
                Title = "Widget2",
                Icon = "bi bi-collection",
                SubMenu = new()
                {
                    new() { Title = "กลุ่ม Widget2", ModuleName = "WidgetGroup2", Link = "WidgetGroup2", Icon = "bi bi-folder2" }, 
                    new() { Title = "Widget2 ทั้งหมด", ModuleName = "Widget2", Link = "Widget2", Icon = "bi bi-collection" }
                }
            });
            */
            #endregion
             
            return listMenu;
        }

        #region Field_Config
        private static readonly List<KeyValuePair<string, List<string>>> FieldSearch_Default = new() { new("text", new() { "title", "en_title" }) };
        private static readonly List<KeyValuePair<string, List<string>>> FieldSearch_DefaultCat = new() { new("text", new() { "title", "en_title" }), new("cat_id", new() { "cat_id" }) };
        
        private static readonly List<KeyValuePair<string, string>> ListData_Default = new() { new("title", "หัวข้อ"), new("pb_status", "สถานะ"), new("updated_at", "วันที่แก้ไข") };
        private static readonly List<KeyValuePair<string, string>> ListData_DefaultImg = new() { new("title", "หัวข้อ"), new("img1", "ภาพ"), new("pb_status", "สถานะ"), new("updated_at", "วันที่แก้ไข") };
        private static readonly List<KeyValuePair<string, string>> ListData_DefaultVDO = new() { new("title", "หัวข้อ"), new("url", "VDO"), new("pb_status", "สถานะ"), new("updated_at", "วันที่แก้ไข") };
        private static readonly List<KeyValuePair<string, string>> ListData_MemberEmail = new() { new("t2", "อีเมล"), new("pb_status", "สถานะ"), new("updated_at", "วันที่แก้ไข") };

        // ---------- Export ของ "รายชื่อผู้สมัครงาน(ครบ)" (web_job_submit_full) ----------
        // export ทุกคอลัมน์ในตาราง + คลี่ข้อมูลใน form_json (jsonb) ออกมาเป็นคอลัมน์อ่านง่าย
        // และแปลงค่ารหัส/ตัวย่อ (cat_id, job_lang, certify, status ฯลฯ) ให้เป็นข้อความที่มีความหมาย
        // หมายเหตุ: Key ของแต่ละรายการถูกฝังลงใน SQL เป็น select expression โดยตรง (ดู Export() ใน AdminCoreController)
        // จำนวนแถว "ประวัติการศึกษา" ในใบสมัคร — ต้องตรงกับ EduRowCount ใน Views/Career/Apply.cshtml ของ front-end
        public const int JobEduRowCount = 5;

        private static List<KeyValuePair<string, string>> BuildJobSubmitFullExport()
        {
            var list = new List<KeyValuePair<string, string>>();
            void Add(string expr, string label) => list.Add(new(expr, label));
            // ดึงค่าจาก form_json ด้วย ->> (คืน text)
            void Fj(string key, string label) => list.Add(new($"form_json->>'{key}'", label));

            #region คอลัมน์จริงในตาราง (แปลงค่ารหัสให้เป็นข้อความ)
            Add("id", "รหัสใบสมัคร");
            Add("TO_CHAR(submitted_at, 'DD-MM-YYYY HH24:MI:SS')", "วันที่ส่งใบสมัคร");
            Add("TO_CHAR(created_at, 'DD-MM-YYYY HH24:MI:SS')", "วันที่บันทึก");
            Add("TO_CHAR(updated_at, 'DD-MM-YYYY HH24:MI:SS')", "วันที่แก้ไขล่าสุด");
            Add("created_by", "สร้างโดย");
            Add("updated_by", "แก้ไขโดย");
            Add("CASE WHEN status = 1 THEN 'ใช้งาน' WHEN status = 0 THEN 'ปกติ' ELSE status::text END", "สถานะข้อมูล");
            Add("CASE WHEN pb_status = 1 THEN 'เผยแพร่แล้ว' ELSE 'รออนุมัติ' END", "สถานะเผยแพร่");
            Add("CASE WHEN show_front = 1 THEN 'แสดง' ELSE 'ไม่แสดง' END", "แสดงหน้าเว็บ");
            Add("cat_id", "รหัสตำแหน่ง (cat_id)");
            Add("COALESCE(NULLIF(job_title, ''), (SELECT title FROM web_core_news wcn WHERE wcn.id = web_job_submit_full.cat_id LIMIT 1))", "ตำแหน่งที่สมัคร");
            // ตำแหน่งที่สมัครลำดับที่ 2/3 — ผู้สมัครเลือกก็ได้ ไม่เลือกก็ได้ (ว่าง = ไม่ได้เลือก)
            Add("NULLIF(cat_id2, 0)", "รหัสตำแหน่งลำดับที่ 2 (cat_id2)");
            Add("job_title2", "ตำแหน่งที่สมัคร (ลำดับที่ 2)");
            Add("NULLIF(cat_id3, 0)", "รหัสตำแหน่งลำดับที่ 3 (cat_id3)");
            Add("job_title3", "ตำแหน่งที่สมัคร (ลำดับที่ 3)");
            Add("CASE WHEN LOWER(job_lang) = 'en' THEN 'อังกฤษ' ELSE 'ไทย' END", "ภาษาที่กรอกใบสมัคร");
            Add("expected_salary", "เงินเดือนที่ต้องการ");
            Add("available_date", "วันที่เริ่มงานได้");
            Add("apply_date", "วันที่สมัคร");
            Add("photo_file", "ไฟล์รูปถ่าย");
            Add("prefix", "คำนำหน้า");
            Add("prefix_other", "คำนำหน้า (อื่นๆ)");
            Add("firstname_th", "ชื่อ (ไทย)");
            Add("lastname_th", "นามสกุล (ไทย)");
            Add("firstname_en", "ชื่อ (อังกฤษ)");
            Add("lastname_en", "นามสกุล (อังกฤษ)");
            Add("birth_date", "วันเกิด");
            Add("age", "อายุ");
            Add("nationality", "สัญชาติ");
            Add("height", "ส่วนสูง (ซม.)");
            Add("weight", "น้ำหนัก (กก.)");
            Add("nickname", "ชื่อเล่น");
            Add("mobile", "โทรศัพท์มือถือ");
            Add("email", "อีเมล");
            Add("current_address", "ที่อยู่ปัจจุบัน");
            Add("current_zip", "รหัสไปรษณีย์ (ปัจจุบัน)");
            Add("current_phone", "โทรศัพท์ (ปัจจุบัน)");
            Add("registered_address", "ที่อยู่ตามทะเบียนบ้าน");
            Add("registered_zip", "รหัสไปรษณีย์ (ทะเบียนบ้าน)");
            Add("registered_phone", "โทรศัพท์ (ทะเบียนบ้าน)");
            Add("military_status", "สถานภาพทางทหาร");
            Add("military_status_other", "สถานภาพทางทหาร (อื่นๆ)");
            Add("idcard", "เลขบัตรประชาชน");
            Add("id_issue_date", "วันออกบัตร");
            Add("id_expiry_date", "วันหมดอายุบัตร");
            Add("id_issue_place", "สถานที่ออกบัตร");
            Add("marital_status", "สถานภาพสมรส");
            Add("special_skill", "ความสามารถพิเศษ");
            Add("work_area", "พื้นที่ที่ปฏิบัติงานได้");
            Add("work_area_province", "จังหวัดที่ระบุ");
            Add("travel_frequency", "ความถี่ที่สามารถเดินทางไปปฏิบัติงานต่างจังหวัด");
            Add("license_name", "ใบอนุญาตประกอบวิชาชีพ");
            Add("license_no", "เลขที่ใบอนุญาต");
            Add("license_expiry", "วันหมดอายุใบอนุญาต");
            Add("news_source", "แหล่งข่าวการสมัคร");
            Add("source_detail", "รายละเอียดแหล่งข่าว");
            Add("has_relative_in_sam", "มีญาติ/คนรู้จักใน บสส.");
            Add("relative_name", "ชื่อ-นามสกุล (ญาติ)");
            Add("relative_relation", "ความสัมพันธ์ (ญาติ)");
            Add("CASE WHEN certify = 1 THEN 'ยอมรับ' ELSE 'ไม่ยอมรับ' END", "ยอมรับคำรับรอง");
            Add("signature", "ลายมือชื่อผู้สมัคร");
            Add("sign_date", "วันที่ลงชื่อ");
            Add("sensitive_consent", "ความยินยอมข้อมูลอ่อนไหว");
            Add("consent_signature", "ลายมือชื่อผู้ให้ความยินยอม");
            Add("consent_date", "วันที่ให้ความยินยอม");
            Add("resume_file", "ไฟล์เรซูเม่");
            Add("translate(COALESCE(document_files, ''), '[]\"', '')", "ไฟล์เอกสารแนบ");
            Add("ip_address", "IP Address");
            Add("referer", "Referer");
            Add("user_agent", "User Agent");
            #endregion

            #region ครอบครัว (จาก form_json)
            Fj("FatherName", "บิดา: ชื่อ-นามสกุล"); Fj("FatherAge", "บิดา: อายุ"); Fj("FatherJob", "บิดา: อาชีพ");
            Fj("FatherStatus", "บิดา: สถานะ"); Fj("FatherAddress", "บิดา: ที่อยู่"); Fj("FatherPhone", "บิดา: โทรศัพท์");
            Fj("MotherName", "มารดา: ชื่อ-นามสกุล"); Fj("MotherAge", "มารดา: อายุ"); Fj("MotherJob", "มารดา: อาชีพ");
            Fj("MotherStatus", "มารดา: สถานะ"); Fj("MotherAddress", "มารดา: ที่อยู่"); Fj("MotherPhone", "มารดา: โทรศัพท์");
            Fj("SiblingCount", "จำนวนพี่น้อง"); Fj("SiblingOrder", "เป็นบุตรคนที่");
            for (int i = 0; i < 3; i++)
            {
                int n = i + 1;
                Fj($"Sibling[{i}].Name", $"พี่น้อง {n}: ชื่อ-สกุล"); Fj($"Sibling[{i}].Age", $"พี่น้อง {n}: อายุ");
                Fj($"Sibling[{i}].Job", $"พี่น้อง {n}: อาชีพ"); Fj($"Sibling[{i}].Place", $"พี่น้อง {n}: สถานที่");
                Fj($"Sibling[{i}].Phone", $"พี่น้อง {n}: โทรศัพท์");
            }
            Fj("SpouseName", "คู่สมรส: ชื่อ-นามสกุล"); Fj("SpouseAge", "คู่สมรส: อายุ"); Fj("SpouseJob", "คู่สมรส: อาชีพ");
            Fj("SpousePosition", "คู่สมรส: ตำแหน่ง"); Fj("SpouseWorkplace", "คู่สมรส: สถานที่ทำงาน"); Fj("SpousePhone", "คู่สมรส: โทรศัพท์");
            Fj("ChildCount", "จำนวนบุตร"); Fj("ChildMale", "บุตรชาย"); Fj("ChildFemale", "บุตรหญิง");
            for (int i = 0; i < 3; i++)
            {
                int n = i + 1;
                Fj($"Child[{i}].Name", $"บุตร {n}: ชื่อ-สกุล"); Fj($"Child[{i}].Age", $"บุตร {n}: อายุ");
                Fj($"Child[{i}].Job", $"บุตร {n}: อาชีพ"); Fj($"Child[{i}].Place", $"บุตร {n}: สถานที่");
                Fj($"Child[{i}].Phone", $"บุตร {n}: โทรศัพท์");
            }
            #endregion

            #region การศึกษา (จาก form_json)
            // ผู้สมัครเลือกระดับการศึกษาเองรายแถว (เดิม fix เป็น Master/Bachelor/Secondary/OtherEdu)
            // → key เปลี่ยนเป็น Education[i].* จำนวน JobEduRowCount แถว
            for (int i = 0; i < JobEduRowCount; i++)
            {
                int n = i + 1;
                Fj($"Education[{i}].Level", $"การศึกษา {n}: ระดับ"); Fj($"Education[{i}].Institute", $"การศึกษา {n}: สถาบัน");
                Fj($"Education[{i}].Major", $"การศึกษา {n}: คณะ/สาขา"); Fj($"Education[{i}].Gpa", $"การศึกษา {n}: เกรดเฉลี่ย");
                Fj($"Education[{i}].GraduatedYear", $"การศึกษา {n}: ปีที่จบ");
            }
            #endregion

            #region ประวัติการทำงาน (จาก form_json)
            for (int i = 0; i < 3; i++)
            {
                int n = i + 1;
                Fj($"Work[{i}].Company", $"งาน {n}: สถานประกอบการ"); Fj($"Work[{i}].BusinessType", $"งาน {n}: ประเภทกิจการ");
                Fj($"Work[{i}].Location", $"งาน {n}: ที่ตั้ง"); Fj($"Work[{i}].Phone", $"งาน {n}: โทรศัพท์");
                Fj($"Work[{i}].Department", $"งาน {n}: สังกัด"); Fj($"Work[{i}].StartDate", $"งาน {n}: วันเริ่มงาน");
                Fj($"Work[{i}].EndDate", $"งาน {n}: วันสิ้นสุด"); Fj($"Work[{i}].Years", $"งาน {n}: อายุงาน(ปี)");
                Fj($"Work[{i}].Bonus", $"งาน {n}: โบนัส"); Fj($"Work[{i}].StartPosition", $"งาน {n}: ตำแหน่งเริ่มต้น");
                Fj($"Work[{i}].LastPosition", $"งาน {n}: ตำแหน่งสุดท้าย"); Fj($"Work[{i}].StartSalary", $"งาน {n}: เงินเดือนเริ่มต้น");
                Fj($"Work[{i}].LastSalary", $"งาน {n}: เงินเดือนสุดท้าย"); Fj($"Work[{i}].OtherIncome", $"งาน {n}: รายได้อื่น");
                Fj($"Work[{i}].JobDescription", $"งาน {n}: ลักษณะงาน"); Fj($"Work[{i}].ResignReason", $"งาน {n}: สาเหตุที่ลาออก");
            }
            #endregion

            #region การฝึกอบรม (จาก form_json)
            for (int i = 0; i < 3; i++)
            {
                int n = i + 1;
                Fj($"Training[{i}].Course", $"อบรม {n}: หลักสูตร"); Fj($"Training[{i}].Institute", $"อบรม {n}: สถาบัน");
                Fj($"Training[{i}].Date", $"อบรม {n}: วันที่"); Fj($"Training[{i}].Duration", $"อบรม {n}: ระยะเวลา");
            }
            #endregion

            #region ภาษา + คอมพิวเตอร์ (จาก form_json)
            for (int i = 0; i < 3; i++)
            {
                int n = i + 1;
                Fj($"Language[{i}].Name", $"ภาษา {n}: ชื่อภาษา"); Fj($"Language[{i}].Listening", $"ภาษา {n}: ฟัง");
                Fj($"Language[{i}].Speaking", $"ภาษา {n}: พูด"); Fj($"Language[{i}].Reading", $"ภาษา {n}: อ่าน");
                Fj($"Language[{i}].Writing", $"ภาษา {n}: เขียน");
            }
            Fj("Computer.Word", "คอมพิวเตอร์: Word"); Fj("Computer.Excel", "คอมพิวเตอร์: Excel");
            Fj("Computer.PowerPoint", "คอมพิวเตอร์: PowerPoint"); Fj("Computer.AITools", "คอมพิวเตอร์: การใช้งานเครื่องมือ AI");
            // แถวที่ผู้สมัครกดปุ่ม "เพิ่มโปรแกรมอื่น" เพิ่มเอง
            for (int i = 0; i < 3; i++)
            {
                int n = i + 1;
                Fj($"ComputerExtra[{i}].Name", $"โปรแกรมเพิ่มเติม {n}: ชื่อโปรแกรม");
                Fj($"ComputerExtra[{i}].Level", $"โปรแกรมเพิ่มเติม {n}: ระดับ");
            }
            Fj("ComputerOther", "คอมพิวเตอร์: โปรแกรมอื่นๆ");
            #endregion

            #region บุคคลอ้างอิง + ติดต่อฉุกเฉิน (จาก form_json)
            Fj("ReferenceName", "อ้างอิง: ชื่อ-สกุล"); Fj("ReferenceRelation", "อ้างอิง: ความสัมพันธ์"); Fj("ReferenceJob", "อ้างอิง: อาชีพ");
            Fj("ReferenceWorkplace", "อ้างอิง: สถานที่ทำงาน"); Fj("ReferencePosition", "อ้างอิง: ตำแหน่ง"); Fj("ReferenceMobile", "อ้างอิง: โทรศัพท์");
            Fj("EmergencyName", "ฉุกเฉิน: ชื่อ-สกุล"); Fj("EmergencyRelation", "ฉุกเฉิน: ความสัมพันธ์");
            Fj("EmergencyHomePhone", "ฉุกเฉิน: โทร.บ้าน"); Fj("EmergencyWorkPhone", "ฉุกเฉิน: โทร.ที่ทำงาน"); Fj("EmergencyMobile", "ฉุกเฉิน: โทร.มือถือ");
            #endregion

            #region ข้อมูลอ่อนไหว (จาก form_json)
            // หมายเหตุ: ศาสนา / กรุ๊ปเลือด / ข้อมูลสุขภาพ ถูกถอดออกจากใบสมัครแล้ว จึงไม่มีคอลัมน์เหล่านี้ใน export
            var legalItems = new[]
            {
                ("Disciplinary", "เคยถูกสอบสวนทางวินัย"), ("Terminated", "เคยถูกเลิกจ้าง/ไล่ออก"),
                ("LaborDispute", "เคยมีคดีด้านแรงงาน"), ("DebtLawsuit", "เคยถูกฟ้องชำระหนี้"),
                ("Imprisoned", "เคยรับโทษจำคุก"), ("DrunkDriving", "เคยถูกฟ้องเมาแล้วขับ"),
                ("CriminalRecord", "เคยมีประวัติอาชญากรรม"), ("Bankrupt", "เคยล้มละลาย"),
            };
            foreach (var (k, lbl) in legalItems)
            {
                Fj($"Legal.{k}", $"กฎหมาย: {lbl}");
                Fj($"Legal.{k}Detail", $"กฎหมาย: {lbl} (รายละเอียด)");
            }
            #endregion

            // เก็บข้อมูลดิบทั้งก้อนไว้ท้ายสุด เผื่อมีรายการเกินจำนวนคอลัมน์ที่คลี่ไว้ (เช่น พี่น้อง/งาน เกิน 3)
            Add("form_json::text", "ข้อมูลดิบทั้งหมด (JSON)");

            return list;
        }
        private static readonly List<KeyValuePair<string, string>> ExportData_JobSubmitFull = BuildJobSubmitFullExport();

        #region ช่องรหัสทรัพย์ NPA ของตัวแทน (web_agent.npaid1..npaidN)
        // จำนวนช่องรหัสทรัพย์ที่ตัวแทนแจ้งได้ — เลขนี้ต้องอยู่ที่นี่ที่เดียวในโปรเจกต์ admin
        // ต้องตรงกับ UHelper.NpaSlotCount ของ front-end (d:\Project\sam.or.th\Helpers\Utility.cs) และคอลัมน์จริงใน DB
        // ⚠️ ถ้าเพิ่มค่านี้ ต้อง ALTER TABLE web_agent เพิ่ม npaid{n} varchar(200) NULL ให้ครบ "ก่อน" deploy เสมอ
        //    (Detail/Edit ของ admin อ่านจาก select * แล้ว index ด้วยชื่อคอลัมน์ → คอลัมน์ขาด = 500 ทั้งหน้า)
        public const int NpaSlotCount = 30;

        // ("npaid1","รหัส NPA 1") ... — ใช้ทั้ง ViewDetailData และ ExportData (ลำดับในลิสต์คือลำดับคอลัมน์ export)
        private static IEnumerable<KeyValuePair<string, string>> NpaPairs() =>
            Enumerable.Range(1, NpaSlotCount).Select(i => new KeyValuePair<string, string>("npaid" + i, "รหัส NPA " + i));

        // ชื่อคอลัมน์ล้วน — ใช้ใน FieldUpdate (whitelist ตอน save) และ AgentImportController (INSERT)
        public static readonly List<string> NpaFields =
            Enumerable.Range(1, NpaSlotCount).Select(i => "npaid" + i).ToList();

        // ⚠️ ลิสต์ 3 ตัวข้างล่างถูก "แชร์" ข้าม instance ของ Module (AllModule() คืน object ใหม่แต่ชี้ลิสต์เดิม)
        //    → ห้าม .Add()/.Remove() ภายหลัง ให้ reassign เป็นลิสต์ใหม่แทน
        private static List<KeyValuePair<string, string>> BuildAgentListViewDetail()
        {
            var list = new List<KeyValuePair<string, string>>
            {
                new ("id", "ไอดี"),
                new ("codeid", "รหัส"),
                new ("title", "คำนำหน้า"),
                new ("name", "ชื่อ"),
                new ("surname", "นามสกุล"),
                new ("idcard", "เลขบัตรประชาชน/เลขผู้เสียภาษี"),
                new ("cus_base_addr", "ที่อยู่ตามทะเบียนบ้าน"),
                new ("cus_base_moo", "หมู่ที่ (ทะเบียนบ้าน)"),
                new ("cus_base_soi", "ซอย (ทะเบียนบ้าน)"),
                new ("cus_base_road", "ถนน (ทะเบียนบ้าน)"),
                new ("cus_base_tambol", "ตำบล/แขวง (ทะเบียนบ้าน)"),
                new ("cus_base_amphur", "อำเภอ/เขต (ทะเบียนบ้าน)"),
                new ("cus_base_province", "จังหวัด (ทะเบียนบ้าน)"),
                new ("cus_base_zipcode", "รหัสไปรษณีย์ (ทะเบียนบ้าน)"),
                new ("cus_contact_arrr", "ที่อยู่ติดต่อ"),
                new ("cus_contact_moo", "หมู่ที่ (ที่อยู่ติดต่อ)"),
                new ("cus_contact_soi", "ซอย (ที่อยู่ติดต่อ)"),
                new ("cus_contact_road", "ถนน (ที่อยู่ติดต่อ)"),
                new ("cus_contact_tambol", "ตำบล/แขวง (ที่อยู่ติดต่อ)"),
                new ("cus_contact_amphur", "อำเภอ/เขต (ที่อยู่ติดต่อ)"),
                new ("cus_contact_province", "จังหวัด (ที่อยู่ติดต่อ)"),
                new ("cus_contact_zipcode", "รหัสไปรษณีย์ (ที่อยู่ติดต่อ)"),
                new ("cus_base_phone", "โทรศัพท์บ้าน"),
                new ("cus_contact_phone", "โทรศัพท์ติดต่อ"),
                new ("upfile1", "ไฟล์แนบ 1"),
                new ("upfile2", "ไฟล์แนบ 2"),
                new ("upfile3", "ไฟล์แนบ 3"),
                new ("upfile4", "ไฟล์แนบ 4"),
            };
            list.AddRange(NpaPairs());
            list.AddRange(new List<KeyValuePair<string, string>>
            {
                new ("adddate", "วันที่เป็นสมาชิก"),
                new ("approved", "สถานะ"),
                new ("created_at", "วันที่สมัครเอเจนซี่"),
                new ("updated_at", "วันที่แก้ไข"),
            });
            return list;
        }

        private static List<string> BuildAgentListFieldUpdate()
        {
            var list = new List<string>
            {
                "title", "name", "surname", "idcard",
                "cus_base_addr", "cus_base_moo", "cus_base_soi", "cus_base_road",
                "cus_base_tambol", "cus_base_amphur", "cus_base_province", "cus_base_zipcode",
                "cus_contact_arrr", "cus_contact_moo", "cus_contact_soi", "cus_contact_road",
                "cus_contact_tambol", "cus_contact_amphur", "cus_contact_province", "cus_contact_zipcode",
                "cus_base_phone", "cus_contact_phone",
            };
            list.AddRange(NpaFields);
            list.AddRange(new[] { "approved", "updated_at" });
            return list;
        }

        private static readonly List<KeyValuePair<string, string>> ViewDetailData_AgentList = BuildAgentListViewDetail();
        private static readonly List<string> FieldUpdate_AgentList                          = BuildAgentListFieldUpdate();
        #endregion

        public List<string> FieldCreate_CoreItem = new() { "cat_id", "date_news", "title", "en_title", "title_color", "des", "en_des", "des_color", "img1", "en_img1", "url", "en_url", "url_target", "title_url", "en_title_url", "img1_icon", "en_img1_icon", "issue_date_config", "issue_date", "expiry_date", "sort" };
        public List<string> FieldCreate_CoreGroup = new() { "cat_id", "title", "en_title", "t1", "en_t1", "t2", "en_t2", "t3", "en_t3", "t4", "en_t4", "t5", "en_t5", "t6", "en_t6", "t7", "en_t7", "t8", "en_t8", "t9", "en_t9", "t10", "en_t10", "t11", "en_t11", "t12", "en_t12", "t13", "en_t13", "t14", "en_t14", "t15", "en_t15", "t16", "en_t16", "t17", "en_t17", "t18", "en_t18", "t19", "en_t19", "t20", "en_t20", "t21", "en_t21", "t22", "en_t22", "t23", "en_t23", "t24", "en_t24", "t25", "en_t25", "t26", "en_t26", "t27", "en_t27", "t28", "en_t28", "t29", "en_t29", "t30", "en_t30" , "issue_date_config", "issue_date", "expiry_date" ,"sort"};
        public List<string> FieldCreate_CoreNews = new() { "cat_id", "title", "en_title", "date_news", "img1", "en_img1", "img2", "en_img2", "file1", "en_file1", "file2", "en_file2", "file3", "en_file3", "file4", "en_file4", "file5", "en_file5", "file6", "en_file6", "file1_title", "en_file1_title", "file2_title", "en_file2_title", "file3_title", "en_file3_title", "file4_title", "en_file4_title", "file5_title", "en_file5_title", "file6_title", "en_file6_title", "des", "en_des", "info", "en_info", "info2", "en_info2", "page_type", "url", "en_url", "url_target", "issue_date_config", "issue_date", "expiry_date", "seo_url","en_seo_url", "sort" };

        public List<string> FieldCreate_CoreSingle = new() { "date_news", "title", "en_title", "t1", "en_t1", "t2", "en_t2", "t3", "en_t3", "t4", "en_t4", "t5", "en_t5", "t6", "en_t6", "t7", "en_t7", "t8", "en_t8", "t9", "en_t9", "t10", "en_t10", "t11", "en_t11", "t12", "en_t12", "t13", "en_t13", "t14", "en_t14", "t15", "en_t15", "t16", "en_t16", "t17", "en_t17", "t18", "en_t18", "t19", "en_t19", "t20", "en_t20", "t21", "en_t21", "t22", "en_t22", "t23", "en_t23", "t24", "en_t24", "t25", "en_t25", "t26", "en_t26", "t27", "en_t27", "t28", "en_t28", "t29", "en_t29", "t30", "en_t30", "t31", "en_t31", "t32", "en_t32", "t33", "en_t33", "t34", "en_t34", "t35", "en_t35", "t36", "en_t36", "t37", "en_t37", "t38", "en_t38", "t39", "en_t39", "t40", "en_t40", "t41", "en_t41", "t42", "en_t42", "t43", "en_t43", "t44", "en_t44", "t45", "en_t45", "t46", "en_t46", "t47", "en_t47", "t48", "en_t48", "t49", "en_t49", "t50", "en_t50", "seo_url", "en_seo_url", "issue_date_config", "issue_date", "expiry_date", "sort" };
        // FooterPoll-only update list (= FieldUpdate_CoreSingle + date_news). Kept separate so the
        // shared FieldUpdate_CoreSingle (used by 35 edit-only modules) is not touched.
        public List<string> FieldUpdate_FooterPoll = new() { "date_news", "title", "en_title", "t1", "en_t1", "t2", "en_t2", "t3", "en_t3", "t4", "en_t4", "t5", "en_t5", "t6", "en_t6", "t7", "en_t7", "t8", "en_t8", "t9", "en_t9", "t10", "en_t10", "t11", "en_t11", "t12", "en_t12", "t13", "en_t13", "t14", "en_t14", "t15", "en_t15", "t16", "en_t16", "t17", "en_t17", "t18", "en_t18", "t19", "en_t19", "t20", "en_t20", "t21", "en_t21", "t22", "en_t22", "t23", "en_t23", "t24", "en_t24", "t25", "en_t25", "t26", "en_t26", "t27", "en_t27", "t28", "en_t28", "t29", "en_t29", "t30", "en_t30", "t31", "en_t31", "t32", "en_t32", "t33", "en_t33", "t34", "en_t34", "t35", "en_t35", "t36", "en_t36", "t37", "en_t37", "t38", "en_t38", "t39", "en_t39", "t40", "en_t40", "t41", "en_t41", "t42", "en_t42", "t43", "en_t43", "t44", "en_t44", "t45", "en_t45", "t46", "en_t46", "t47", "en_t47", "t48", "en_t48", "t49", "en_t49", "t50", "en_t50", "seo_url", "en_seo_url", "issue_date_config", "issue_date", "expiry_date" };
        public List<string> FieldUpdate_CoreItem = new() { "cat_id", "date_news", "title", "en_title", "title_color", "des", "en_des", "des_color", "img1", "en_img1", "url", "en_url", "url_target", "title_url", "en_title_url", "img1_icon", "en_img1_icon", "issue_date_config", "issue_date", "expiry_date" };
        public List<string> FieldUpdate_CoreSingle = new() { "title", "en_title", "t1", "en_t1", "t2", "en_t2", "t3", "en_t3", "t4", "en_t4", "t5", "en_t5", "t6", "en_t6", "t7", "en_t7", "t8", "en_t8", "t9", "en_t9", "t10", "en_t10", "t11", "en_t11", "t12", "en_t12", "t13", "en_t13", "t14", "en_t14", "t15", "en_t15", "t16", "en_t16", "t17", "en_t17", "t18", "en_t18", "t19", "en_t19", "t20", "en_t20", "t21", "en_t21", "t22", "en_t22", "t23", "en_t23", "t24", "en_t24", "t25", "en_t25", "t26", "en_t26", "t27", "en_t27", "t28", "en_t28", "t29", "en_t29", "t30", "en_t30", "t31", "en_t31", "t32", "en_t32", "t33", "en_t33", "t34", "en_t34", "t35", "en_t35", "t36", "en_t36", "t37", "en_t37", "t38", "en_t38", "t39", "en_t39", "t40", "en_t40", "t41", "en_t41", "t42", "en_t42", "t43", "en_t43", "t44", "en_t44", "t45", "en_t45", "t46", "en_t46", "t47", "en_t47", "t48", "en_t48", "t49", "en_t49", "t50", "en_t50" , "issue_date_config", "issue_date", "expiry_date" };
        public List<string> FieldUpdate_CoreGroup = new() { "cat_id", "title", "en_title", "t1", "en_t1", "t2", "en_t2", "t3", "en_t3", "t4", "en_t4", "t5", "en_t5", "t6", "en_t6", "t7", "en_t7", "t8", "en_t8", "t9", "en_t9", "t10", "en_t10", "t11", "en_t11", "t12", "en_t12", "t13", "en_t13", "t14", "en_t14", "t15", "en_t15", "t16", "en_t16", "t17", "en_t17", "t18", "en_t18", "t19", "en_t19", "t20", "en_t20", "t21", "en_t21", "t22", "en_t22", "t23", "en_t23", "t24", "en_t24", "t25", "en_t25", "t26", "en_t26", "t27", "en_t27", "t28", "en_t28", "t29", "en_t29", "t30", "en_t30","issue_date_config", "issue_date", "expiry_date" };
        public List<string> FieldUpdate_CoreNews = new() { "cat_id", "title", "en_title", "date_news", "img1", "en_img1", "img2", "en_img2", "file1", "en_file1", "file2", "en_file2", "file3", "en_file3", "file4", "en_file4", "file5", "en_file5", "file6", "en_file6", "file1_title", "en_file1_title", "file2_title", "en_file2_title", "file3_title", "en_file3_title", "file4_title", "en_file4_title", "file5_title", "en_file5_title", "file6_title", "en_file6_title", "des", "en_des", "info", "en_info", "info2", "en_info2", "page_type", "url", "en_url", "url_target", "issue_date_config", "issue_date", "expiry_date","seo_url","en_seo_url" };
         
        public List<string> FieldApprove_CoreItem = new() { "cat_id", "date_news", "title", "en_title", "title_color", "des", "en_des", "des_color", "img1", "en_img1", "url", "en_url", "url_target", "title_url", "en_title_url", "img1_icon", "en_img1_icon", "issue_date_config", "issue_date", "expiry_date" };
        public List<string> FieldApprove_CoreSingle = new() { "title", "en_title", "t1", "en_t1", "t2", "en_t2", "t3", "en_t3", "t4", "en_t4", "t5", "en_t5", "t6", "en_t6", "t7", "en_t7", "t8", "en_t8", "t9", "en_t9", "t10", "en_t10", "t11", "en_t11", "t12", "en_t12", "t13", "en_t13", "t14", "en_t14", "t15", "en_t15", "t16", "en_t16", "t17", "en_t17", "t18", "en_t18", "t19", "en_t19", "t20", "en_t20", "t21", "en_t21", "t22", "en_t22", "t23", "en_t23", "t24", "en_t24", "t25", "en_t25", "t26", "en_t26", "t27", "en_t27", "t28", "en_t28", "t29", "en_t29", "t30", "en_t30", "t31", "en_t31", "t32", "en_t32", "t33", "en_t33", "t34", "en_t34", "t35", "en_t35", "t36", "en_t36", "t37", "en_t37", "t38", "en_t38", "t39", "en_t39", "t40", "en_t40", "t41", "en_t41", "t42", "en_t42", "t43", "en_t43", "t44", "en_t44", "t45", "en_t45", "t46", "en_t46", "t47", "en_t47", "t48", "en_t48", "t49", "en_t49", "t50", "en_t50", "issue_date_config", "issue_date", "expiry_date" };
        // FooterPoll-only approve list (= FieldApprove_CoreSingle + date_news). Kept separate so the
        // shared FieldApprove_CoreSingle (used by other web_core_single modules) is not touched.
        // The Approve action copies pb_date_news = date_news (approve) / date_news = pb_date_news (revert).
        public List<string> FieldApprove_FooterPoll = new() { "date_news", "title", "en_title", "t1", "en_t1", "t2", "en_t2", "t3", "en_t3", "t4", "en_t4", "t5", "en_t5", "t6", "en_t6", "t7", "en_t7", "t8", "en_t8", "t9", "en_t9", "t10", "en_t10", "t11", "en_t11", "t12", "en_t12", "t13", "en_t13", "t14", "en_t14", "t15", "en_t15", "t16", "en_t16", "t17", "en_t17", "t18", "en_t18", "t19", "en_t19", "t20", "en_t20", "t21", "en_t21", "t22", "en_t22", "t23", "en_t23", "t24", "en_t24", "t25", "en_t25", "t26", "en_t26", "t27", "en_t27", "t28", "en_t28", "t29", "en_t29", "t30", "en_t30", "t31", "en_t31", "t32", "en_t32", "t33", "en_t33", "t34", "en_t34", "t35", "en_t35", "t36", "en_t36", "t37", "en_t37", "t38", "en_t38", "t39", "en_t39", "t40", "en_t40", "t41", "en_t41", "t42", "en_t42", "t43", "en_t43", "t44", "en_t44", "t45", "en_t45", "t46", "en_t46", "t47", "en_t47", "t48", "en_t48", "t49", "en_t49", "t50", "en_t50", "seo_url", "en_seo_url", "issue_date_config", "issue_date", "expiry_date" };
        public List<string> FieldApprove_CoreGroup = new() { "cat_id", "title", "en_title", "t1", "en_t1", "t2", "en_t2", "t3", "en_t3", "t4", "en_t4", "t5", "en_t5", "t6", "en_t6", "t7", "en_t7", "t8", "en_t8", "t9", "en_t9", "t10", "en_t10", "t11", "en_t11", "t12", "en_t12", "t13", "en_t13", "t14", "en_t14", "t15", "en_t15", "t16", "en_t16", "t17", "en_t17", "t18", "en_t18", "t19", "en_t19", "t20", "en_t20", "t21", "en_t21", "t22", "en_t22", "t23", "en_t23", "t24", "en_t24", "t25", "en_t25", "t26", "en_t26", "t27", "en_t27", "t28", "en_t28", "t29", "en_t29", "t30", "en_t30","issue_date_config", "issue_date", "expiry_date" };
        // เมนูที่ปิดส่วน "วันที่แสดงผล" (EnableIssueDate = false) — ไม่มี input issue_date / expiry_date ในฟอร์ม
        // จึงต้องตัด 2 ฟิลด์นี้ออกจากรายการ ไม่ให้ setFieldsUpdate() เขียนทับเป็น NULL ทุกครั้งที่บันทึก
        // ("issue_date_config" ยังอยู่ เพราะฟอร์มส่งค่า 1 = แสดงตลอดเวลา มาให้เป็น hidden input)
        public List<string> FieldCreate_CoreGroup_NoIssueDate  => FieldWithoutIssueDate(FieldCreate_CoreGroup);
        public List<string> FieldUpdate_CoreGroup_NoIssueDate  => FieldWithoutIssueDate(FieldUpdate_CoreGroup);
        public List<string> FieldApprove_CoreGroup_NoIssueDate => FieldWithoutIssueDate(FieldApprove_CoreGroup);
        private static List<string> FieldWithoutIssueDate(List<string> src) =>
            src.Where(f => f != "issue_date" && f != "expiry_date").ToList();

        // JobCMS (รูปประกอบ) — ฟอร์มแก้ไขเหลือเฉพาะรูปภาพ t1 / en_t1 (ช่องอื่น comment ไว้ใน Views/JobCMS/Edit.cshtml)
        // ต้องจำกัดรายการ update/approve ให้ตรงกับช่องที่ฟอร์มส่งมาจริง ไม่งั้น setFieldsUpdate()
        // จะเขียน NULL ทับ title/t2..t50 ที่ไม่มีใน ฟอร์ม  ⚠ เปิดช่องกลับเมื่อไร ให้กลับไปใช้ *_CoreSingle
        public List<string> FieldUpdate_JobCMS  = new() { "t1", "en_t1", "issue_date_config", "issue_date", "expiry_date" };
        public List<string> FieldApprove_JobCMS = new() { "t1", "en_t1", "issue_date_config", "issue_date", "expiry_date" };

        #region ----- HomeFooter (ปรับแต่ง Footer) -----
        // ฟิลด์ของเมนู "ปรับแต่ง Footer" — Create/Update/Approve ใช้ชุดเดียวกันทั้งหมด
        // (เดิมเขียนซ้ำ 3 ที่ ทำให้เพิ่มฟิลด์แล้วลืมแก้ครบได้ง่าย)
        // ปุ่มท้ายเว็บ 3 ปุ่ม (footer__actions) ปุ่มละ 7 ฟิลด์:
        //   btn{i}_text / en_btn{i}_text          ชื่อปุ่ม 2 ภาษา
        //   btn{i}_url  / en_btn{i}_url           URL 2 ภาษา ('#xxx' = เปิด modal id xxx)
        //   btn{i}_url_target                     _top / _blank
        //   btn{i}_bg_color / btn{i}_text_color   สีพื้นหลัง / สีตัวอักษร (ว่าง = ใช้สีธีมเดิม)
        // ทุกคอลัมน์มีคู่ pb_* ครบ (ดู Scripts/sql/2026-08-06-web-home-footer-buttons.sql)
        private static readonly List<string> Field_HomeFooter = BuildHomeFooterFields();
        private static List<string> BuildHomeFooterFields()
        {
            var fields = new List<string>
            {
                "title", "en_title", "tel", "email",
                "sc_fb", "sc_tw", "sc_ln", "sc_yt", "sc_tt", "sc_as", "sc_gp",
                "cr", "en_cr",
            };
            for (int i = 1; i <= HomeFooterButtonCount; i++)
            {
                fields.Add($"btn{i}_text");
                fields.Add($"en_btn{i}_text");
                fields.Add($"btn{i}_url");
                fields.Add($"en_btn{i}_url");
                fields.Add($"btn{i}_url_target");
                fields.Add($"btn{i}_bg_color");
                fields.Add($"btn{i}_text_color");
            }
            return fields;
        }
        // จำนวนปุ่มท้ายเว็บ — ต้องตรงกับคอลัมน์ใน web_home_footer, ฟอร์ม HomeFooter/Edit.cshtml
        // และ front-end (_PartialFooter.cshtml + layoutContentService.GetHomeFooter)
        public const int HomeFooterButtonCount = 3;
        #endregion

        public List<string> FieldApprove_CoreNews = new() { "cat_id", "title", "en_title", "date_news", "img1", "en_img1", "img2", "en_img2", "file1", "en_file1", "file2", "en_file2", "file3", "en_file3", "file4", "en_file4", "file5", "en_file5", "file6", "en_file6", "file1_title", "en_file1_title", "file2_title", "en_file2_title", "file3_title", "en_file3_title", "file4_title", "en_file4_title", "file5_title", "en_file5_title", "file6_title", "en_file6_title", "des", "en_des", "info", "en_info", "info2", "en_info2", "page_type", "url", "en_url", "url_target", "issue_date_config", "issue_date", "expiry_date","seo_url","en_seo_url" };
        #endregion

        public List<Module> AllModule()
        {
            return new List<Module>()
            { 
                #region หน้าเว็บไซต์
                new Module()
                {
                    Name = "CMSPage",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "จัดการเมนูเว็บไซต์",TextBreadcrumb = "หน้าเว้บไซต์/จัดการเมนูเว็บไซต์", Table = "web_cms_page",OrderBy = "sort",Sort = "asc", TableCate = "web_cms_page",TableCateField = "cat_id",TableCateOrderby = "title",TableCateSort = "asc",TableCateTitle = "title",TableCateLabel = "กลุ่ม", CanAdd = true,CanEdit = true,CanDelete = true,CanMove = true,CanStatus = true,CanApprove = true, UseViewCreateFrom = "CMSPage",UseViewEditFrom = "CMSPage", IsCMSPage = true, FieldCMSPage = "parent_id", 
                        FieldSearch = FieldSearch_Default,
                        ListData = new() { new("title", "หัวข้อ"), new("page_type", "ประเภท"), new("pb_status", "สถานะ"), new("updated_at", "วันที่แก้ไข") }, 
                        FieldCreate = new()
                        {
                            "page_type", "title", "en_title", "des", "en_des", "url", "en_url", "url_target", "issue_date", "expiry_date", "seo_kw", "seo_de", "script_head", "script_body", "box_layout", "box_data", "issue_date_config", "page_type_sub", "page_type_sub_core", "page_type_sub_core_data", "info", "en_info", "page_style", "box_data_sub", "seo_url", "en_seo_url","cat_id", "parent_id", "sort"
                        },
                        FieldUpdate = new()
                        {
                            "page_type", "title", "en_title", "des", "en_des", "url", "en_url", "url_target", "issue_date", "expiry_date", "seo_kw", "seo_de", "script_head", "script_body", "box_layout", "box_data", "issue_date_config", "page_type_sub", "page_type_sub_core", "page_type_sub_core_data", "info", "en_info", "page_style", "box_data_sub", "seo_url", "en_seo_url"
                        },
                        FieldApprove = new()
                        {
                            "page_type", "title", "en_title", "des", "en_des", "url", "en_url", "url_target", "issue_date", "expiry_date", "seo_kw", "seo_de", "script_head", "script_body", "box_layout", "box_data", "issue_date_config", "page_type_sub", "page_type_sub_core", "page_type_sub_core_data", "info", "en_info", "page_style", "box_data_sub", "seo_url", "en_seo_url","cat_id", "parent_id"
                        }}
                },
                new Module()
                {
                    Name = "HomeIntroPage",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "หน้า Intro Page", TextBreadcrumb = "หน้าเว็บไซต์/หน้า Intro Page", Table = "web_home_intro_page", OrderBy = "sort", Sort = "asc", CanAdd = true, CanEdit = true, CanDelete = true, CanMove = false, CanStatus = true, CanApprove = true, UseViewCreateFrom = "HomeIntroPage", UseViewEditFrom = "HomeIntroPage",
                        FieldSearch = FieldSearch_Default, ListData = ListData_DefaultImg,
                        FieldCreate = new()
                        {
                            "title", "en_title", "info", "en_info", "img1", "issue_date", "expiry_date", "main_color", "title_1_text", "title_1_color", "title_1_color_text", "title_1_url", "title_2_text", "title_2_color", "title_2_color_text", "title_2_url", "title_3_text", "title_3_color", "title_3_color_text", "title_3_url", "issue_date_config","title_1_url_target","title_2_url_target","title_3_url_target", "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title", "en_title", "info", "en_info", "img1", "issue_date", "expiry_date", "main_color", "title_1_text", "title_1_color", "title_1_color_text", "title_1_url", "title_2_text", "title_2_color", "title_2_color_text", "title_2_url", "title_3_text", "title_3_color", "title_3_color_text", "title_3_url", "issue_date_config","title_1_url_target","title_2_url_target","title_3_url_target"
                        },
                        FieldApprove = new()
                        {
                            "title", "en_title", "info", "en_info", "img1", "issue_date", "expiry_date", "main_color", "title_1_text", "title_1_color", "title_1_color_text", "title_1_url", "title_2_text", "title_2_color", "title_2_color_text", "title_2_url", "title_3_text", "title_3_color", "title_3_color_text", "title_3_url", "issue_date_config","title_1_url_target","title_2_url_target","title_3_url_target"
                        }
                    }
                },
                new Module()
                {
                    Name = "HomePopUp",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "หน้า Pop-Up", TextBreadcrumb = "หน้าเว็บไซต์/หน้า Pop-Up", Table = "web_home_pop_up", OrderBy = "sort", Sort = "asc", CanAdd = true, CanEdit = true, CanDelete = true, CanMove = false, CanStatus = true, CanApprove = true, UseViewCreateFrom = "HomePopUp", UseViewEditFrom = "HomePopUp",
                        FieldSearch = FieldSearch_Default, ListData = ListData_DefaultImg,
                        FieldCreate = new()
                        {
                            "title", "en_title", "info", "en_info", "img1", "url", "en_url", "url_target", "issue_date", "expiry_date", "main_color", "title_1_text", "title_1_color", "title_1_color_text", "title_1_url", "title_2_text", "title_2_color", "title_2_color_text", "title_2_url", "title_3_text", "title_3_color", "title_3_color_text", "title_3_url", "issue_date_config","title_1_url_target","title_2_url_target","title_3_url_target", "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title", "en_title", "info", "en_info", "img1", "url", "en_url", "url_target", "issue_date", "expiry_date", "main_color", "title_1_text", "title_1_color", "title_1_color_text", "title_1_url", "title_2_text", "title_2_color", "title_2_color_text", "title_2_url", "title_3_text", "title_3_color", "title_3_color_text", "title_3_url", "issue_date_config","title_1_url_target","title_2_url_target","title_3_url_target"
                        },
                        FieldApprove = new()
                        {
                            "title", "en_title", "info", "en_info", "img1", "url", "en_url", "url_target", "issue_date", "expiry_date", "main_color", "title_1_text", "title_1_color", "title_1_color_text", "title_1_url", "title_2_text", "title_2_color", "title_2_color_text", "title_2_url", "title_3_text", "title_3_color", "title_3_color_text", "title_3_url", "issue_date_config","title_1_url_target","title_2_url_target","title_3_url_target"
                        }
                    }
                },
                new Module()
                {
                    Name = "HomeHeader",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "โลโก้ / Template", TextBreadcrumb = "หน้าเว็บไซต์/โลโก้ / Template", Table = "web_home_header", OrderBy = "sort", Sort = "asc", CanAdd = false, CanEdit = true, CanDelete = false, CanMove = false, CanStatus = false, CanApprove = true, UseViewCreateFrom = "HomeHeader", UseViewEditFrom = "HomeHeader",
                        FieldSearch = FieldSearch_Default, ListData = ListData_DefaultImg,
                        FieldCreate = new()
                        {
                            "title","this_type","img1",
                        },
                        FieldUpdate = new()
                        {
                            "title","this_type","img1","des",
                        },
                        FieldApprove = new()
                        {
                            "title","this_type","img1","des",
                        }
                    }
                },
                new Module()
                {
                    Name = "HomeFooter",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ปรับแต่ง Footer", TextBreadcrumb = "หน้าเว็บไซต์/ปรับแต่ง Footer", Table = "web_home_footer", OrderBy = "sort", Sort = "asc", CanAdd = false, CanEdit = true, CanDelete = false, CanMove = false, CanStatus = false, CanApprove = true, UseViewCreateFrom = "HomeFooter", UseViewEditFrom = "HomeFooter",
                        FieldSearch = FieldSearch_Default, ListData = ListData_Default,
                        FieldCreate = new(Field_HomeFooter),
                        FieldUpdate = new(Field_HomeFooter),
                        FieldApprove = new(Field_HomeFooter)
                    }
                },
                new Module()
                {
                    Name = "HomeSEO",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "SEO & Code", TextBreadcrumb = "หน้าเว็บไซต์/SEO & Code", Table = "web_home_seo", OrderBy = "sort", Sort = "asc", CanAdd = false, CanEdit = true, CanDelete = false, CanMove = false, CanStatus = false, CanApprove = true, UseViewCreateFrom = "HomeSEO", UseViewEditFrom = "HomeSEO",
                        FieldSearch = FieldSearch_Default, ListData = ListData_Default,
                        FieldCreate = new()
                        {
                            "title", "en_title", "des", "en_des", "info", "en_info", "ga_embed_head", "ga_embed_body", "fb_embed_head", "fb_embed_body", "ot1_embed_head", "ot1_embed_body", "ot2_embed_head", "ot2_embed_body", "ot3_embed_head", "ot3_embed_body",
                        },
                        FieldUpdate = new()
                        {
                            "title", "en_title", "des", "en_des", "info", "en_info", "ga_embed_head", "ga_embed_body", "fb_embed_head", "fb_embed_body", "ot1_embed_head", "ot1_embed_body", "ot2_embed_head", "ot2_embed_body", "ot3_embed_head", "ot3_embed_body",
                        },
                        FieldApprove = new()
                        {
                            "title", "en_title", "des", "en_des", "info", "en_info", "ga_embed_head", "ga_embed_body", "fb_embed_head", "fb_embed_body", "ot1_embed_head", "ot1_embed_body", "ot2_embed_head", "ot2_embed_body", "ot3_embed_head", "ot3_embed_body",
                        }
                    }
                },
                new Module()
                {
                    Name = "CMSPageFooter1",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "จัดการเมนู Footer 1", TextBreadcrumb = "หน้าเว้บไซต์/จัดการเมนู Footer 1", Table = "web_cms_page_footer1", OrderBy = "sort", Sort = "asc",  TableCate = "web_cms_page_footer1", TableCateField = "cat_id", TableCateOrderby = "title", TableCateSort = "asc", TableCateTitle = "title", TableCateLabel = "กลุ่ม",  CanAdd = true, CanEdit = true, CanDelete = true, CanMove = true, CanStatus = true, CanApprove = true, UseViewCreateFrom = "CMSPage", UseViewEditFrom = "CMSPage", IsCMSPage = true, FieldCMSPage = "parent_id",
                        FieldSearch = FieldSearch_Default,
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),new ("page_type", "ประเภท"),new ("pb_status", "สถานะ"),new ("updated_at", "วันที่แก้ไข")
                        },
                        FieldCreate = new()
                        {
                            "cat_id", "page_type", "title", "en_title", "des", "en_des", "url", "en_url", "url_target", "issue_date", "expiry_date", "seo_kw", "seo_de", "script_head", "script_body", "box_layout", "box_data", "issue_date_config", "page_type_sub", "page_type_sub_core", "page_type_sub_core_data", "parent_id", "info", "en_info", "page_style", "box_data_sub", "seo_url", "en_seo_url", "sort"
                        },
                        FieldUpdate = new()
                        {
                            "page_type", "title", "en_title", "des", "en_des", "url", "en_url", "url_target", "issue_date", "expiry_date", "seo_kw", "seo_de", "script_head", "script_body", "box_layout", "box_data", "issue_date_config", "page_type_sub", "page_type_sub_core", "page_type_sub_core_data", "info", "en_info", "page_style", "box_data_sub", "seo_url", "en_seo_url"
                        },
                        FieldApprove = new()
                        {
                            "cat_id", "page_type", "title", "en_title", "des", "en_des", "url", "en_url", "url_target", "issue_date", "expiry_date", "seo_kw", "seo_de", "script_head", "script_body", "box_layout", "box_data", "issue_date_config", "page_type_sub", "page_type_sub_core", "page_type_sub_core_data", "parent_id", "info", "en_info", "page_style", "box_data_sub", "seo_url", "en_seo_url"
                        }
                    }
                },
                new Module()
                {
                    Name = "CMSPageFooter2",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "จัดการเมนู Footer 2", TextBreadcrumb = "หน้าเว้บไซต์/จัดการเมนู Footer 2", Table = "web_cms_page_footer2", OrderBy = "sort", Sort = "asc",  TableCate = "web_cms_page_footer2", TableCateField = "cat_id", TableCateOrderby = "title", TableCateSort = "asc", TableCateTitle = "title", TableCateLabel = "กลุ่ม",  CanAdd = true, CanEdit = true, CanDelete = true, CanMove = true, CanStatus = true, CanApprove = true, UseViewCreateFrom = "CMSPage", UseViewEditFrom = "CMSPage", IsCMSPage = true, FieldCMSPage = "parent_id",
                        FieldSearch = FieldSearch_Default,
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"), new ("page_type", "ประเภท"), new ("pb_status", "สถานะ"), new ("updated_at", "วันที่แก้ไข")
                        },
                        FieldCreate = new()
                        {
                            "cat_id", "page_type", "title", "en_title", "des", "en_des", "url", "en_url", "url_target", "issue_date", "expiry_date", "seo_kw", "seo_de", "script_head", "script_body", "box_layout", "box_data", "issue_date_config", "page_type_sub", "page_type_sub_core", "page_type_sub_core_data", "parent_id", "info", "en_info", "page_style", "box_data_sub", "sort", "seo_url", "en_seo_url"
                        },
                        FieldUpdate = new()
                        {
                            "page_type", "title", "en_title", "des", "en_des", "url", "en_url", "url_target", "issue_date", "expiry_date", "seo_kw", "seo_de", "script_head", "script_body", "box_layout", "box_data", "issue_date_config", "page_type_sub", "page_type_sub_core", "page_type_sub_core_data", "info", "en_info", "page_style", "box_data_sub", "seo_url", "en_seo_url"
                        },
                        FieldApprove = new()
                        {
                            "cat_id", "page_type", "title", "en_title", "des", "en_des", "url", "en_url", "url_target", "issue_date", "expiry_date", "seo_kw", "seo_de", "script_head", "script_body", "box_layout", "box_data", "issue_date_config", "page_type_sub", "page_type_sub_core", "page_type_sub_core_data", "parent_id", "info", "en_info", "page_style", "box_data_sub", "seo_url", "en_seo_url"
                        }
                    }
                },
                #endregion

                #region ข้อมูลหน้าแรก
                new Module()
                {
                    Name = "HomeImageSlide",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "รูปสไลด์หน้าแรก",TextBreadcrumb = "ข้อมูลหน้าแรก/รูปสไลด์หน้าแรก",
                        Table = "web_core_item",TableModuleID = 1,OrderBy = "sort",Sort = "asc",
                        CanAdd = true,CanEdit = true,CanDelete = true,CanMove = true,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "HomeImageSlide",UseViewEditFrom = "HomeImageSlide",
                        FieldSearch = FieldSearch_Default, ListData = ListData_DefaultImg,
                        FieldCreate = FieldCreate_CoreItem,
                        FieldUpdate = FieldUpdate_CoreItem,
                        FieldApprove = FieldApprove_CoreItem
                    }
                },
                new Module()
                {
                    Name = "HomeImageConf",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ตั้งค่ารูปสไลด์",TextBreadcrumb = "ข้อมูลหน้าแรก/ตั้งค่ารูปสไลด์",
                        Table = "web_home_image_conf",OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewEditFrom = "HomeImageConf",
                        FieldSearch = new(){},
                        ListData = new()
                        {
                            new ("effect", "ประเภทการเลื่อนวน"), new ("speed", "ความเร็ว (วินาที)"), new ("pb_status", "สถานะ"), new ("updated_at", "วันที่แก้ไข"), new ("updated_by", "แก้ไขโดย")
                        },
                        FieldUpdate = new(){ "effect", "speed" },
                        FieldApprove = new(){ "effect", "speed" }
                    }
                },
                new Module()
                {
                    Name = "HomeSamText",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "SAM ใส่ใจ",TextBreadcrumb = "ข้อมูลหน้าแรก/SAM ใส่ใจ",
                        Table = "web_core_single",TableModuleID = 1,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "HomeSamText",UseViewEditFrom = "HomeSamText",
                        FieldSearch = FieldSearch_Default,
                        ListData = new(){new ("title", "หัวข้อ"), new("t2", "รูปภาพ"), new ("pb_status", "สถานะ"),new ("updated_at", "วันที่แก้ไข")},
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "HomeSamText2",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ภาพรวมการบริหารหนี้",TextBreadcrumb = "ข้อมูลหน้าแรก/ภาพรวมการบริหารหนี้",
                        Table = "web_core_single",TableModuleID = 2,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "HomeSamText2",UseViewEditFrom = "HomeSamText2",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "HomeSamText3",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ปิดหนี้ไว ไปต่อได้",TextBreadcrumb = "ข้อมูลหน้าแรก/ปิดหนี้ไว ไปต่อได้",
                        Table = "web_core_single",TableModuleID = 3,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "HomeSamText3",UseViewEditFrom = "HomeSamText3",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "HomeSamText4",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ทรัพย์เด่นและน่าสนใจ",TextBreadcrumb = "ข้อมูลหน้าแรก/ทรัพย์เด่นและน่าสนใจ",
                        Table = "web_core_single",TableModuleID = 4,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "HomeSamText4",UseViewEditFrom = "HomeSamText4",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "HomeSamText5",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "คุณกำลังมองหาอะไร",TextBreadcrumb = "ข้อมูลหน้าแรก/คุณกำลังมองหาอะไร",
                        Table = "web_core_single",TableModuleID = 5,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "HomeSamText5",UseViewEditFrom = "HomeSamText5",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "HomeSamText6",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "บ้านเด่นทำเลดี",TextBreadcrumb = "ข้อมูลหน้าแรก/บ้านเด่นทำเลดี",
                        Table = "web_core_single",TableModuleID = 6,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "HomeSamText6",UseViewEditFrom = "HomeSamText6",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "HomeSamText7",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "วิสัยทัศน์",TextBreadcrumb = "ข้อมูลหน้าแรก/วิสัยทัศน์",
                        Table = "web_core_single",TableModuleID = 7,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "HomeSamText7",UseViewEditFrom = "HomeSamText7",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                #endregion

                #region ข้อมูลหน้าแรก
                new Module()
                {
                    Name = "HeroBanner",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "Hero Banner",TextBreadcrumb = "ข้อมูลหน้าแรก/Hero Banner",
                        Table = "web_core_single",TableModuleID = 36,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "HeroBanner",UseViewEditFrom = "HeroBanner",
                        FieldSearch = FieldSearch_Default,
                        ListData = new(){new ("title", "หัวข้อ"), new("t2", "รูปภาพ"), new ("pb_status", "สถานะ"),new ("updated_at", "วันที่แก้ไข")},
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "MicrositeIntro",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "บทนำ",TextBreadcrumb = "ข้อมูลหน้าแรก/บทนำ",
                        Table = "web_core_single",TableModuleID = 37,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "MicrositeIntro",UseViewEditFrom = "MicrositeIntro",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "MicrositeRole",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "บทบาทของเรา",TextBreadcrumb = "ข้อมูลหน้าแรก/บทบาทของเรา",
                        Table = "web_core_item",TableModuleID = 9,OrderBy = "sort",Sort = "asc",
                        CanAdd = true,CanEdit = true,CanDelete = true,CanMove = true,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "MicrositeRole",UseViewEditFrom = "MicrositeRole",
                        FieldSearch = FieldSearch_Default,
                        ListData = new(){new ("img1_icon", "ไอคอน"), new ("title", "หัวข้อ"), new ("pb_status", "สถานะ"),new ("updated_at", "วันที่แก้ไข")},
                        FieldCreate = FieldCreate_CoreItem,
                        FieldUpdate = FieldUpdate_CoreItem,
                        FieldApprove = FieldApprove_CoreItem
                    }
                },
                new Module()
                {
                    Name = "MicrositeBannerHalf",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "แบนเนอร์ครึ่งจอ",TextBreadcrumb = "ข้อมูลหน้าแรก/แบนเนอร์ครึ่งจอ",
                        Table = "web_core_single",TableModuleID = 38,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "MicrositeBannerHalf",UseViewEditFrom = "MicrositeBannerHalf",
                        FieldSearch = FieldSearch_Default,
                        ListData = new(){new ("title", "หัวข้อ"), new("t2", "รูปภาพ"), new ("pb_status", "สถานะ"),new ("updated_at", "วันที่แก้ไข")},
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "MicrositeDoc",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "เอกสารที่เกี่ยวข้อง",TextBreadcrumb = "ข้อมูลหน้าแรก/เอกสารที่เกี่ยวข้อง",
                        Table = "web_core_single",TableModuleID = 39,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "MicrositeDoc",UseViewEditFrom = "MicrositeDoc",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "MicrositeBenefit",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "สิทธิพิเศษ",TextBreadcrumb = "ข้อมูลหน้าแรก/สิทธิพิเศษ",
                        Table = "web_core_single",TableModuleID = 40,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "MicrositeBenefit",UseViewEditFrom = "MicrositeBenefit",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "MicrositeAssetHead",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ทรัพย์ที่ร่วมรายการ (หัวข้อ)",TextBreadcrumb = "ข้อมูลหน้าแรก/ทรัพย์ที่ร่วมรายการ (หัวข้อ)",
                        Table = "web_core_single",TableModuleID = 41,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "MicrositeAssetHead",UseViewEditFrom = "MicrositeAssetHead",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "MicrositeAsset",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ทรัพย์ที่ร่วมรายการ",TextBreadcrumb = "ข้อมูลหน้าแรก/ทรัพย์ที่ร่วมรายการ",
                        Table = "web_core_item",TableModuleID = 10,OrderBy = "sort",Sort = "asc",
                        CanAdd = true,CanEdit = true,CanDelete = true,CanMove = true,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "MicrositeAsset",UseViewEditFrom = "MicrositeAsset",
                        FieldSearch = FieldSearch_Default,
                        ListData = new(){new ("img1", "รูป"), new ("title", "ประเภท"), new ("des", "ทำเล"), new ("date_news", "ราคา"), new ("pb_status", "สถานะ"),new ("updated_at", "วันที่แก้ไข")},
                        FieldCreate = FieldCreate_CoreItem,
                        FieldUpdate = FieldUpdate_CoreItem,
                        FieldApprove = FieldApprove_CoreItem
                    }
                },
                new Module()
                {
                    Name = "MicrositeTerm",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "เงื่อนไขโครงการ",TextBreadcrumb = "ข้อมูลหน้าแรก/เงื่อนไขโครงการ",
                        Table = "web_core_single",TableModuleID = 42,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "MicrositeTerm",UseViewEditFrom = "MicrositeTerm",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "MicrositeHighlight",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "จุดเด่นโครงการ",TextBreadcrumb = "ข้อมูลหน้าแรก/จุดเด่นโครงการ",
                        Table = "web_core_single",TableModuleID = 43,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "MicrositeHighlight",UseViewEditFrom = "MicrositeHighlight",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "MicrositeContactQr",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "สอบถามเพิ่มเติม (QR)",TextBreadcrumb = "ข้อมูลหน้าแรก/สอบถามเพิ่มเติม (QR)",
                        Table = "web_core_single",TableModuleID = 44,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "MicrositeContactQr",UseViewEditFrom = "MicrositeContactQr",
                        FieldSearch = FieldSearch_Default,
                        ListData = new(){new ("title", "หัวข้อ"), new("t4", "QR Code"), new ("pb_status", "สถานะ"),new ("updated_at", "วันที่แก้ไข")},
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "MicrositeCounter",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ตัวเลขความสำเร็จ",TextBreadcrumb = "ข้อมูลหน้าแรก/ตัวเลขความสำเร็จ",
                        Table = "web_core_single",TableModuleID = 45,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "MicrositeCounter",UseViewEditFrom = "MicrositeCounter",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "MicrositeHelp",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "แนวทางที่เราช่วยได้",TextBreadcrumb = "ข้อมูลหน้าแรก/แนวทางที่เราช่วยได้",
                        Table = "web_core_single",TableModuleID = 46,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "MicrositeHelp",UseViewEditFrom = "MicrositeHelp",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "MicrositePrepare",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "เอกสารที่ต้องเตรียม",TextBreadcrumb = "ข้อมูลหน้าแรก/เอกสารที่ต้องเตรียม",
                        Table = "web_core_single",TableModuleID = 47,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "MicrositePrepare",UseViewEditFrom = "MicrositePrepare",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "MicrositeLead",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ข้อมูลติดต่อกลับ",TextBreadcrumb = "ข้อมูลหน้าแรก/ข้อมูลติดต่อกลับ",
                        Table = "web_core_single",TableModuleID = 48,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "MicrositeLead",UseViewEditFrom = "MicrositeLead",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "MicrositeFaq",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "คำถามที่พบบ่อย (ส่วนแสดงผล)",TextBreadcrumb = "ข้อมูลหน้าแรก/คำถามที่พบบ่อย (ส่วนแสดงผล)",
                        Table = "web_core_single",TableModuleID = 49,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "MicrositeFaq",UseViewEditFrom = "MicrositeFaq",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                #endregion

                #region ข้อมูลเกี่ยวกับเรา
                new Module()
                {
                    Name = "AboutVision",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "วิสัยทัศน์/พันธกิจ",TextBreadcrumb = "เกี่ยวกับเรา / วิสัยทัศน์/พันธกิจ",
                        Table = "web_core_single",TableModuleID = 8,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "AboutVision",UseViewEditFrom = "AboutVision",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "AboutHistory",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ประวัติความเป็นมา",TextBreadcrumb = "เกี่ยวกับเรา/ประวัติความเป็นมา",
                        Table = "web_core_single",TableModuleID = 9,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "AboutHistory",UseViewEditFrom = "AboutHistory",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "AboutStructure",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "โครงสร้างองค์กร",TextBreadcrumb = "เกี่ยวกับเรา/โครงสร้างองค์กร",
                        Table = "web_core_item",TableModuleID = 4,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "AboutStructure",UseViewEditFrom = "AboutStructure",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = FieldCreate_CoreItem,
                        FieldUpdate = FieldUpdate_CoreItem,
                        FieldApprove = FieldApprove_CoreItem
                    }
                },
                new Module()
                {
                    Name = "AboutAnnual",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "รายงานประจำปี",TextBreadcrumb = "เกี่ยวกับเรา/รายงานประจำปี",
                        Table = "web_core_item",TableModuleID = 2,OrderBy = "pb_title_color desc, id",Sort = "desc",
                        CanAdd = true,CanEdit = true,CanDelete = true,CanMove = false,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "AboutAnnual",UseViewEditFrom = "AboutAnnual",
                        FieldSearch = FieldSearch_Default,
                        ListData = new(){new ("title", "หัวข้อ"), new("title_color", "ปี"), new("img1", "รูปภาพ"), new ("pb_status", "สถานะ"),new ("updated_at", "วันที่แก้ไข")},
                        FieldCreate = FieldCreate_CoreItem,
                        FieldUpdate = FieldUpdate_CoreItem,
                        FieldApprove = FieldApprove_CoreItem
                    }
                },
                new Module()
                {
                    Name = "AboutFin",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "รายงานการเงิน",TextBreadcrumb = "เกี่ยวกับเรา/รายงานการเงิน",
                        Table = "web_core_item",TableModuleID = 3,OrderBy = "pb_title_color desc, id",Sort = "desc",
                        CanAdd = true,CanEdit = true,CanDelete = true,CanMove = false,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "AboutFin",UseViewEditFrom = "AboutFin",
                        FieldSearch = FieldSearch_Default,
                        ListData = new(){new ("title", "หัวข้อ"), new("title_color", "ปี"), new("img1", "รูปภาพ"), new ("pb_status", "สถานะ"),new ("updated_at", "วันที่แก้ไข")},
                        FieldCreate = FieldCreate_CoreItem,
                        FieldUpdate = FieldUpdate_CoreItem,
                        FieldApprove = FieldApprove_CoreItem
                    }
                },
                new Module()
                {
                    Name = "AboutBoard1",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "คณะกรรมการบริษัท",TextBreadcrumb = "เกี่ยวกับเรา/คณะกรรมการบริษัท",
                        Table = "web_core_item",TableModuleID = 5,OrderBy = "sort",Sort = "asc",
                        CanAdd = true,CanEdit = true,CanDelete = true,CanMove = true,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "AboutBoard1",UseViewEditFrom = "AboutBoard1",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_DefaultImg,
                        FieldCreate = FieldCreate_CoreItem,
                        FieldUpdate = FieldUpdate_CoreItem,
                        FieldApprove = FieldApprove_CoreItem
                    }
                },
                new Module()
                {
                    Name = "AboutBoard2",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "คณะกรรมการอื่นๆ",TextBreadcrumb = "เกี่ยวกับเรา/คณะกรรมการอื่นๆ",
                        Table = "web_core_item",TableModuleID = 6,OrderBy = "sort",Sort = "asc",
                        TableCate = "web_core_group", TableCateModuleID = "1", TableCateField = "cat_id", TableCateOrderby = "sort", TableCateSort = "asc", TableCateTitle = "title", TableCateLabel = "กลุ่ม", CanAdd = true,CanEdit = true,CanDelete = true,CanMove = true,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "AboutBoard2",UseViewEditFrom = "AboutBoard2",
                        FieldSearch = FieldSearch_DefaultCat,
                        ListData = ListData_Default,
                        FieldCreate = FieldCreate_CoreItem,
                        FieldUpdate = FieldUpdate_CoreItem,
                        FieldApprove = FieldApprove_CoreItem
                    }
                },
                new Module()
                {
                    Name = "AboutBoard2cat",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "คณะกรรมการอื่นๆ (กลุ่ม)",TextBreadcrumb = "เกี่ยวกับเรา/คณะกรรมการอื่นๆ (กลุ่ม)",
                        Table = "web_core_group",TableModuleID = 1,OrderBy = "sort",Sort = "asc",
                        CanAdd = true,CanEdit = true,CanDelete = true,CanMove = true,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "CoreGroup",UseViewEditFrom = "CoreGroup",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = FieldCreate_CoreGroup,
                        FieldUpdate = FieldUpdate_CoreGroup,
                        FieldApprove = FieldApprove_CoreGroup
                    }
                },
                new Module()
                {
                    Name = "AboutBoard3",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "คณะผู้บริหารระดับสูง",TextBreadcrumb = "เกี่ยวกับเรา/คณะผู้บริหารระดับสูง",
                        Table = "web_core_item",TableModuleID = 7,OrderBy = "sort",Sort = "asc",
                        TableCate = "web_core_group", TableCateModuleID = "2", TableCateField = "cat_id", TableCateOrderby = "sort", TableCateSort = "asc", TableCateTitle = "title", TableCateLabel = "กลุ่ม", CanAdd = true,CanEdit = true,CanDelete = true,CanMove = true,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "AboutBoard3",UseViewEditFrom = "AboutBoard3",
                        FieldSearch = FieldSearch_DefaultCat,
                        ListData = ListData_DefaultImg,
                        FieldCreate = FieldCreate_CoreItem,
                        FieldUpdate = FieldUpdate_CoreItem,
                        FieldApprove = FieldApprove_CoreItem
                    }
                },
                new Module()
                {
                    Name = "AboutBoard3cat",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "คณะผู้บริหารระดับสูง (กลุ่ม)",TextBreadcrumb = "เกี่ยวกับเรา/คณะผู้บริหารระดับสูง (กลุ่ม)",
                        Table = "web_core_group",TableModuleID = 2,OrderBy = "sort",Sort = "asc",
                        CanAdd = true,CanEdit = true,CanDelete = true,CanMove = true,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "CoreGroup",UseViewEditFrom = "CoreGroup",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = FieldCreate_CoreGroup,
                        FieldUpdate =FieldUpdate_CoreGroup,
                        FieldApprove = FieldApprove_CoreGroup
                    }
                }, 
                new Module()
                {
                    Name = "AboutBoard4cat",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "คณะผู้บริหารฝ่าย (สายงาน)",TextBreadcrumb = "เกี่ยวกับเรา/คณะผู้บริหารฝ่าย (สายงาน)",
                        Table = "web_core_group",TableModuleID = 3,OrderBy = "sort",Sort = "asc",
                        CanAdd = true,CanEdit = true,CanDelete = true,CanMove = true,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "CoreGroup",UseViewEditFrom = "CoreGroup",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = FieldCreate_CoreGroup,
                        FieldUpdate = FieldUpdate_CoreGroup,
                        FieldApprove = FieldApprove_CoreGroup
                    }
                },
                new Module()
                {
                     Name = "AboutBoard4sub",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "คณะผู้บริหารฝ่าย (กลุ่ม)",TextBreadcrumb = "เกี่ยวกับเรา/คณะผู้บริหารฝ่าย (กลุ่ม)",
                        Table = "web_core_group",TableModuleID = 4,OrderBy = "sort",Sort = "asc",
                        TableCate = "web_core_group", TableCateModuleID = "3", TableCateField = "cat_id", TableCateOrderby = "sort", TableCateSort = "asc", TableCateTitle = "title", TableCateLabel = "กลุ่ม", CanAdd = true,CanEdit = true,CanDelete = true,CanMove = true,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "AboutBoard4sub",UseViewEditFrom = "AboutBoard4sub",
                        FieldSearch = FieldSearch_DefaultCat,
                        ListData = ListData_Default,
                        FieldCreate = FieldCreate_CoreGroup,
                        FieldUpdate = FieldUpdate_CoreGroup,
                        FieldApprove = FieldApprove_CoreGroup
                    }
                },
                new Module()
                {
                    Name = "AboutBoard4",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "คณะผู้บริหารฝ่าย",TextBreadcrumb = "เกี่ยวกับเรา/คณะผู้บริหารฝ่าย",
                        Table = "web_core_item",TableModuleID = 8,OrderBy = "sort",Sort = "asc",
                        TableCate = "web_core_group", TableCateModuleID = "4", TableCateField = "cat_id", TableCateOrderby = "sort", TableCateSort = "asc", TableCateTitle = "title", TableCateLabel = "กลุ่ม", CanAdd = true,CanEdit = true,CanDelete = true,CanMove = true,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "AboutBoard4",UseViewEditFrom = "AboutBoard4",
                        FieldSearch = FieldSearch_DefaultCat,
                        ListData = ListData_DefaultImg,
                        FieldCreate = FieldCreate_CoreItem,
                        FieldUpdate = FieldUpdate_CoreItem,
                        FieldApprove = FieldApprove_CoreItem
                    }
                },
                new Module()
                {
                    Name = "AboutText1",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "การบริหารสินทรัพย์ด้อยคุณภาพ บสส.",TextBreadcrumb = "เกี่ยวกับเรา/การบริหารสินทรัพย์ด้อยคุณภาพ บสส.",
                        Table = "web_core_single",TableModuleID = 10,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "AboutText1",UseViewEditFrom = "AboutText1",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "AboutText2",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "บริหารทรัพย์สินรอการขาย",TextBreadcrumb = "เกี่ยวกับเรา/บริหารทรัพย์สินรอการขาย",
                        Table = "web_core_single",TableModuleID = 11,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "AboutText1",UseViewEditFrom = "AboutText1",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "AboutText3",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "สร้างโอกาสทางธุรกิจ บสส.",TextBreadcrumb = "เกี่ยวกับเรา/สร้างโอกาสทางธุรกิจ บสส.",
                        Table = "web_core_single",TableModuleID = 12,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "AboutText1",UseViewEditFrom = "AboutText1",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                #endregion

                #region ทรัพย์สินรอการขาย
                new Module()
                {
                    Name = "NpaText1",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ขั้นตอนการซื้อทรัพย์",TextBreadcrumb = "ทรัพย์สินรอการขาย/ขั้นตอนการซื้อทรัพย์",
                        Table = "web_core_single",TableModuleID = 13,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "NpaText1",UseViewEditFrom = "NpaText1",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                #endregion
                 
                #region บริหารหนี้ด้อยคุณภาพ
                new Module()
                {
                    Name = "DebtText1",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ภาพรวมการบริหารหนี้",TextBreadcrumb = "บริหารหนี้ด้อยคุณภาพ/ภาพรวมการบริหารหนี้",
                        Table = "web_core_single",TableModuleID = 14,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "DebtText1",UseViewEditFrom = "DebtText1",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "DebtText2",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ขั้นตอนการปรับโครงสร้างหนี้",TextBreadcrumb = "บริหารหนี้ด้อยคุณภาพ/ขั้นตอนการปรับโครงสร้างหนี้",
                        Table = "web_core_single",TableModuleID = 15,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "DebtText2",UseViewEditFrom = "DebtText2",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "DebtText3",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "สิทธิประโยชน์ลูกหนี้",TextBreadcrumb = "บริหารหนี้ด้อยคุณภาพ/สิทธิประโยชน์ลูกหนี้",
                        Table = "web_core_single",TableModuleID = 16,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "DebtText3",UseViewEditFrom = "DebtText3",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "DebtText4",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ดาวน์โหลดแบบฟอร์ม",TextBreadcrumb = "บริหารหนี้ด้อยคุณภาพ/ดาวน์โหลดแบบฟอร์ม",
                        Table = "web_core_single",TableModuleID = 17,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "AboutText1",UseViewEditFrom = "AboutText1",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "DebtText5",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ลงทะเบียนปรับโครงสร้างหนี้",TextBreadcrumb = "บริหารหนี้ด้อยคุณภาพ/ลงทะเบียนปรับโครงสร้างหนี้",
                        Table = "web_core_single",TableModuleID = 18,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "DebtText5",UseViewEditFrom = "DebtText5",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "DebtText6",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "คลินิกแก้หนี้",TextBreadcrumb = "บริหารหนี้ด้อยคุณภาพ/คลินิกแก้หนี้",
                        Table = "web_core_single",TableModuleID = 19,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "DebtText6",UseViewEditFrom = "DebtText6",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                #endregion

                #region ข่าวประชาสัมพันธ์
                new Module()
                {
                    Name = "NewsGroup",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ข่าวประชาสัมพันธ์ (กลุ่ม)", TextBreadcrumb = "ข่าวประชาสัมพันธ์/ข่าวประชาสัมพันธ์ (กลุ่ม)",
                        Table = "web_core_group",TableModuleID = 5,OrderBy = "sort",Sort = "asc",
                        CanAdd = true,CanEdit = true,CanDelete = true,CanMove = false,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "CoreGroup",UseViewEditFrom = "CoreGroup",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = FieldCreate_CoreGroup,
                        FieldUpdate = FieldUpdate_CoreGroup,
                        FieldApprove = FieldApprove_CoreGroup
                    }
                },
                new Module()
                {
                    Name = "News",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ข่าวประชาสัมพันธ์",
                        TextBreadcrumb = "ข่าวประชาสัมพันธ์/ข่าวประชาสัมพันธ์",
                        Table = "web_core_news",TableModuleID = 1,OrderBy = "pb_date_news desc, id",Sort = "desc",
                        TableCate = "web_core_group", TableCateModuleID = "5", TableCateField = "cat_id", TableCateOrderby = "sort", TableCateSort = "asc", TableCateTitle = "title", TableCateLabel = "กลุ่ม", CanAdd = true,CanEdit = true,CanDelete = true,CanMove = false,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "News",UseViewEditFrom = "News",
                        FieldSearch = FieldSearch_DefaultCat, ListData = ListData_DefaultImg,
                        FieldCreate = FieldCreate_CoreNews,
                        FieldUpdate = FieldUpdate_CoreNews,
                        FieldApprove = FieldApprove_CoreNews
                    }
                },
                #endregion

                #region บทความและวารสาร
                new Module()
                {
                    Name = "ArticleGroup",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "บทความและวารสาร (กลุ่ม)",
                        TextBreadcrumb = "บทความและวารสาร/บทความและวารสาร (กลุ่ม)",
                        Table = "web_core_group",TableModuleID = 6,OrderBy = "sort",Sort = "asc",
                        CanAdd = true,CanEdit = true,CanDelete = true,CanMove = true,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "CoreGroup",UseViewEditFrom = "CoreGroup",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = FieldCreate_CoreGroup,
                        FieldUpdate = FieldUpdate_CoreGroup,
                        FieldApprove = FieldApprove_CoreGroup
                    }
                },
                new Module()
                {
                    Name = "Article",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "บทความและวารสาร",
                        TextBreadcrumb = "บทความและวารสาร/บทความและวารสาร",
                        Table = "web_core_news",TableModuleID = 2,OrderBy = "pb_date_news desc, id",Sort = "desc",
                        TableCate = "web_core_group", TableCateModuleID = "6", TableCateField = "cat_id", TableCateOrderby = "sort", TableCateSort = "asc", TableCateTitle = "title", TableCateLabel = "กลุ่ม", CanAdd = true,CanEdit = true,CanDelete = true,CanMove = false,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "Article",UseViewEditFrom = "Article",
                        FieldSearch = FieldSearch_DefaultCat, ListData = ListData_DefaultImg,
                        FieldCreate = FieldCreate_CoreNews,
                        FieldUpdate = FieldUpdate_CoreNews,
                        FieldApprove = FieldApprove_CoreNews
                    }
                }, 
                #endregion

                #region วิดีโอ/สื่อประชาสัมพันธ์
                new Module()
                {
                    Name = "VDOGroup",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "รายการ (กลุ่ม)",
                        TextBreadcrumb = "วิดีโอ/สื่อประชาสัมพันธ์/รายการ (กลุ่ม)",
                        Table = "web_core_group",TableModuleID = 7,OrderBy = "sort",Sort = "asc",
                        CanAdd = true,CanEdit = true,CanDelete = true,CanMove = true,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "CoreGroup",UseViewEditFrom = "CoreGroup",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = FieldCreate_CoreGroup,
                        FieldUpdate = FieldUpdate_CoreGroup,
                        FieldApprove = FieldApprove_CoreGroup
                    }
                },
                new Module()
                {
                     Name = "VDOGroupsub",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "รายการ",TextBreadcrumb = "วิดีโอ/สื่อประชาสัมพันธ์/ รายการ",
                        Table = "web_core_group",TableModuleID = 8,OrderBy = "sort",Sort = "asc",
                        TableCate = "web_core_group", TableCateModuleID = "7", TableCateField = "cat_id", TableCateOrderby = "sort", TableCateSort = "asc", TableCateTitle = "title", TableCateLabel = "กลุ่ม", CanAdd = true,CanEdit = true,CanDelete = true,CanMove = false,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "VDOGroupsub",UseViewEditFrom = "VDOGroupsub",
                        FieldSearch = FieldSearch_DefaultCat,
                        ListData = ListData_Default,
                        FieldCreate = FieldCreate_CoreGroup,
                        FieldUpdate = FieldUpdate_CoreGroup,
                        FieldApprove = FieldApprove_CoreGroup
                    }
                },
                new Module()
                {
                    Name = "VDO",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "วิดีโอ/สื่อประชาสัมพันธ์",
                        TextBreadcrumb = "วิดีโอ/สื่อประชาสัมพันธ์/วิดีโอ/สื่อประชาสัมพันธ์",
                        Table = "web_core_news",TableModuleID = 3,OrderBy = "pb_date_news desc, id",Sort = "desc",
                        TableCate = "web_core_group", TableCateModuleID = "8", TableCateField = "cat_id", TableCateOrderby = "sort", TableCateSort = "asc", TableCateTitle = "title", TableCateLabel = "กลุ่ม", CanAdd = true,CanEdit = true,CanDelete = true,CanMove = false,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "VDO",UseViewEditFrom = "VDO",
                        FieldSearch = FieldSearch_DefaultCat, ListData = ListData_DefaultVDO,
                        FieldCreate = FieldCreate_CoreNews,
                        FieldUpdate = FieldUpdate_CoreNews,
                        FieldApprove = FieldApprove_CoreNews
                    }
                }, 
                #endregion

                #region สมาชิก
                new Module()
                {
                    Name = "MemberList",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "จัดการสมาชิก",
                        TextBreadcrumb = "สมาชิก/จัดการสมาชิก",
                        Table = "web_member",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = false,
                        CanEdit = true,
                        CanDelete = true,
                        CanMove = false,
                        CanStatus = false,
                        CanApprove = false,
                        CanExport = true,
                        UseViewCreateFrom = "MemberList",
                        UseViewEditFrom = "MemberList",
                        EnableViewDetail = true,
                        ViewDetailData = new()
                        {
                            new ("id", "ไอดี"),
                            new ("member_type", "ประเภทสมาชิก"),
                            new ("type", "ประเภท"),
                            new ("firstname", "ชื่อ"),
                            new ("surname", "นามสกุล"),
                            new ("username", "Username"),
                            new ("mobile", "มือถือ"),
                            new ("email", "อีเมล"),
                            new ("confirm_email", "ยืนยันอีเมล"),
                            new ("lineuid", "LINE UUID"),
                            new ("reg_date", "วันที่สมัคร"),
                            new ("updated_at", "วันที่แก้ไขข้อมูล"),
                            new ("last_login", "เข้าสู่ระบบล่าสุด"),
                        },
                        ExportData = new()
                        {
                            new ("id", "id"),
                            new ("CASE member_type WHEN 1 THEN 'สมาชิก' WHEN 2 THEN 'Investor' WHEN 3 THEN 'Agency' WHEN 4 THEN 'Sale/Executive' ELSE '' END", "ประเภทสมาชิก"),
                            new ("type", "ประเภท"),
                            new ("firstname", "ชื่อ"),
                            new ("surname", "นามสกุล"),
                            new ("username", "Username"),
                            new ("mobile", "มือถือ"),
                            new ("email", "อีเมล"),
                            new ("confirm_email", "ยืนยันอีเมล"),
                            new ("lineuid", "LINE UUID"),
                            new ("TO_CHAR(reg_date, 'DD-MM-YYYY HH24:MI:SS')", "วันที่สมัคร"),
                            new ("TO_CHAR(updated_at, 'DD-MM-YYYY HH24:MI:SS')", "วันที่แก้ไขข้อมูล"),
                            new ("TO_CHAR(last_login, 'DD-MM-YYYY HH24:MI:SS')", "เข้าสู่ระบบล่าสุด"),
                        },
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "firstname", "surname", "email", "mobile"}),
                            new ("member_type", new() { "member_type" })
                        },
                        FieldSearchIsEqual = new()
                        {
                            "member_type"
                        },
                        ListData = new()
                        {
                            new ("firstname", "ชื่อ-สกุล"),
                            new ("member_type", "ประเภทสมาชิก"),
                            new ("type", "ประเภท"),
                            new ("confirm_email", "สถานะ"),
                            new ("reg_date", "วันที่สมัคร")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "member_type",
                            "type",
                            "confirm_email",
                            "title",
                            "titleoth",
                            "firstname",
                            "surname",
                            "email",
                            "mobile",
                            "lineuid",
                            "password"
                        }
                    }
                },
                new Module()
                {
                    
                },
                new Module()
                {
                    Name = "MemberReport",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "รายงานสมาชิก",
                        TextBreadcrumb = "สมาชิก/รายงานสมาชิก",
                        Table = "web_admin",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        CanMove = false,
                        CanStatus = true,
                        CanApprove = true,
                        UseViewCreateFrom = "MemberReport",
                        UseViewEditFrom = "MemberReport",
                        UseViewListFrom = "MemberReport",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("pb_status", "สถานะ"),
                            new ("updated_at", "วันที่แก้ไข")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        }
                    }
                },
                new Module()
                {
                    Name = "MemberEmail",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "อีเมลเจ้าหน้าที่แจ้งเตือน",
                        TextBreadcrumb = "สมาชิก/อีเมลเจ้าหน้าที่แจ้งเตือน",
                        Table = "web_core_single",TableModuleID = 26,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "MemberEmail",UseViewEditFrom = "MemberEmail",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_MemberEmail,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "MemberNews",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ส่งข่าวสารสมาชิก",
                        TextBreadcrumb = "สมาชิก/ส่งข่าวสารสมาชิก",
                        Table = "web_member",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        CanMove = false,
                        CanStatus = true,
                        CanApprove = true,
                        UseViewCreateFrom = "SubscriptionEmail",
                        UseViewEditFrom = "SubscriptionEmail",
                        UseViewListFrom = "SubscriptionEmail",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("pb_status", "สถานะ"),
                            new ("updated_at", "วันที่แก้ไข")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        }
                    }
                },
                #endregion
                
                #region ตัวแทนขายทรัพย์ (Agent)
                new Module()
                {
                    Name = "AgentList",
                    Config = new Module.ModuleConfig()
                    { 
                        Text = "รายชื่อผู้สมัครตัวแทน",
                        TextBreadcrumb = "ตัวแทนขายทรัพย์ (Agent)/รายชื่อผู้สมัครตัวแทน", 
                        Table = "web_agent",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = false,
                        CanEdit = true,
                        CanDelete = true,
                        CanMove = false,
                        CanStatus = false,
                        CanApprove = false,
                        CanExport = true,
                        UseViewCreateFrom = "AgentList",
                        UseViewEditFrom = "AgentList",
                        EnableViewDetail = true,
                        // รายการฟิลด์ Detail/Update — สร้างจาก builder ด้านบน (ช่อง npaid ขับด้วย NpaSlotCount)
                        ViewDetailData = ViewDetailData_AgentList,
                        // ⚠️ เมนูนี้ไม่ใช้ ExportData — AgentListController.Export() เขียนไฟล์เองด้วย
                        //    Helpers/AgentImportSchema.cs ให้ได้ "รูปแบบเดียวกับไฟล์ Import" (นำกลับไป Import ต่อได้ทันที)
                        ExportData = new(),
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "name", "surname", "idcard", "cus_base_phone", "cus_contact_phone"})
                        },
                        ListData = new()
                        {
                            new ("codeid", "รหัส"),
                            new ("name", "ชื่อ"),
                            new ("surname", "นามสกุล"),
                            new ("idcard", "เลขบัตรประชนชน/เลขผู้เสียภาษี"),
                            new ("approved", "สถานะ"),
                            new ("created_at", "วันที่สมัครเอเจนซี่"),
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title",
                            "sort"
                        },
                        FieldUpdate = FieldUpdate_AgentList
                    }
                },
                new Module()
                {
                    Name = "AgentImport",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "นำเข้าข้อมูลผู้สมัคร",
                        TextBreadcrumb = "ตัวแทนขายทรัพย์ (Agent)/นำเข้าข้อมูลผู้สมัคร",
                        Table = "web_admin",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        CanMove = false,
                        CanStatus = true,
                        CanApprove = true,
                        UseViewCreateFrom = "AgentImport",
                        UseViewEditFrom = "AgentImport",
                        UseViewListFrom = "AgentImport",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("pb_status", "สถานะ"),
                            new ("updated_at", "วันที่แก้ไข")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        }
                    }
                },
                new Module()
                {
                    Name = "AgentReport",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "รายงานตัวแทน",
                        TextBreadcrumb = "ตัวแทนขายทรัพย์ (Agent)/รายงานตัวแทน",
                        Table = "web_admin",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        CanMove = false,
                        CanStatus = true,
                        CanApprove = true,
                        UseViewCreateFrom = "AgentReport",
                        UseViewEditFrom = "AgentReport",
                        UseViewListFrom = "AgentReport",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("pb_status", "สถานะ"),
                            new ("updated_at", "วันที่แก้ไข")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        }
                    }
                },
                new Module()
                {
                    Name = "AgentEmail",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "อีเมลเจ้าหน้าที่แจ้งเตือน",
                        TextBreadcrumb = "ตัวแทนขายทรัพย์ (Agent)/อีเมลเจ้าหน้าที่แจ้งเตือน",
                        Table = "web_core_single",TableModuleID = 27,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "MemberEmail",UseViewEditFrom = "MemberEmail",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_MemberEmail,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "AgentNews",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ส่งข่าวสารเอเจนต์",
                        TextBreadcrumb = "สมาชิก/ส่งข่าวสารเอเจนต์",
                        Table = "web_agent",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        CanMove = false,
                        CanStatus = true,
                        CanApprove = true,
                        UseViewCreateFrom = "SubscriptionEmail",
                        UseViewEditFrom = "SubscriptionEmail",
                        UseViewListFrom = "SubscriptionEmail",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("pb_status", "สถานะ"),
                            new ("updated_at", "วันที่แก้ไข")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        }
                    }
                },
                #endregion

                #region ข้อมูลทรัพย์สิน (NPA)
                new Module()
                {
                    Name = "NpaStatus",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "กำหนดสถานะทรัพย์สิน",
                        TextBreadcrumb = "ข้อมูลทรัพย์สิน (NPA)/กำหนดสถานะทรัพย์สิน",
                        Table = "web_npa_status",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        CanMove = false,
                        CanStatus = true,
                        CanApprove = true,
                        UseViewCreateFrom = "NpaStatus",
                        UseViewEditFrom = "NpaStatus",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("pb_status", "สถานะ"),
                            new ("updated_at", "วันที่แก้ไข")
                        },
                        FieldApprove = new()
                        {
                            "title",
                            "npa_id",
                            "group_id",
                            "facility_id",
                            "highlight_status",
                            "suggest_status"
                        },
                        FieldCreate = new()
                        {
                            "title",
                            "npa_id",
                            "group_id",
                            "facility_id",
                            "highlight_status",
                            "suggest_status",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title",
                            "npa_id",
                            "group_id",
                            "facility_id",
                            "highlight_status",
                            "suggest_status"
                        }
                    }
                },
                new Module()
                {
                    Name = "NpaFav",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "รายชื่อทรัพย์ Wishlist",
                        TextBreadcrumb = "ข้อมูลทรัพย์สิน (NPA)/รายชื่อทรัพย์ Wishlist",
                        Table = "web_npa_fav",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        CanMove = false,
                        CanStatus = true,
                        CanApprove = true,
                        CanExport = true,
                        UseViewCreateFrom = "NpaFav",
                        UseViewEditFrom = "NpaFav",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("title2", "สมาชิก"),
                            new ("pb_status", "สถานะ"),
                            new ("updated_at", "วันที่แก้ไข")
                        },
                        FieldApprove = new()
                        {
                            "title",
                            "npa_id",
                            "title2",
                            "member_id"
                        },
                        FieldCreate = new()
                        {
                            "title",
                            "npa_id",
                            "title2",
                            "member_id",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title",
                            "npa_id",
                            "title2",
                            "member_id"
                        },
                        ExportData = new()
                        {
                            new ("id", "id"),
                            new ("TO_CHAR(created_at, 'DD-MM-YYYY HH24:MI:SS')", "วันที่บันทึก"),
                            new ("TO_CHAR(updated_at, 'DD-MM-YYYY HH24:MI:SS')", "วันที่แก้ไข"),
                            new ("title", "ทรัพย์ที่บันทึก"),
                            new ("npa_id", "รหัสทรัพย์ (npa_id)"),
                            new ("member_id", "รหัสสมาชิก"),
                            new ("COALESCE(NULLIF(title2, ''), (SELECT TRIM(CONCAT(m.firstname, ' ', m.surname)) FROM web_member m WHERE m.id = web_npa_fav.member_id))", "สมาชิก"),
                            new ("(SELECT m.email FROM web_member m WHERE m.id = web_npa_fav.member_id)", "อีเมลสมาชิก"),
                            new ("(SELECT m.mobile FROM web_member m WHERE m.id = web_npa_fav.member_id)", "เบอร์โทรสมาชิก"),
                            new ("CASE pb_status WHEN 1 THEN 'อนุมัติ' ELSE 'รออนุมัติ' END", "สถานะ"),
                        }
                    }
                },
                new Module()
                {
                    Name = "NpaReport",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "รายงานการค้นหาข้อมูลทรัพย์สิน",
                        TextBreadcrumb = "ข้อมูลทรัพย์สิน (NPA)/รายงานการค้นหาข้อมูลทรัพย์สิน",
                        Table = "web_npa_report",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        CanMove = true,
                        CanStatus = true,
                        CanApprove = true,
                        UseViewCreateFrom = "NpaReport",
                        UseViewEditFrom = "NpaReport",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("pb_status", "สถานะ"),
                            new ("created_at", "วันที่สร้าง"),
                            new ("updated_at", "วันที่แก้ไข")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        }
                    }
                },
                new Module()
                {
                    Name = "NpaLocate",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "รายการ ย่าน/ทำเล",
                        TextBreadcrumb = "ข้อมูลทรัพย์สิน (NPA)/รายการ ย่าน/ทำเล",
                        Table = "web_core_group",TableModuleID = 18,OrderBy = "sort",Sort = "asc",
                        // CanMove / CanStatus = false และปิดส่วน "วันที่แสดงผล" — front-end มีระบบเลือกนำไปแสดงเองอยู่แล้ว
                        // (ย่าน/ทำเล ที่โผล่บนเว็บ = ที่ถูกเลือกไว้ในหน้าแรก t2..t11 ของ web_core_single)
                        CanAdd = true,CanEdit = true,CanDelete = true,CanMove = false,CanStatus = false,CanApprove = true,
                        EnableIssueDate = false,
                        UseViewCreateFrom = "CoreGroup",UseViewEditFrom = "CoreGroup",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = FieldCreate_CoreGroup_NoIssueDate,
                        FieldUpdate = FieldUpdate_CoreGroup_NoIssueDate,
                        FieldApprove = FieldApprove_CoreGroup_NoIssueDate
                    }
                },
                new Module()
                {
                    Name = "NpaFacility",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "รายการ Facility",
                        TextBreadcrumb = "ข้อมูลทรัพย์สิน (NPA)/รายการ Facility",
                        Table = "web_core_group",TableModuleID = 19,OrderBy = "sort",Sort = "asc",
                        // CanMove / CanStatus = false และปิดส่วน "วันที่แสดงผล" — front-end มีระบบเลือกนำไปแสดงเองอยู่แล้ว
                        // (Facility ที่โผล่บนเว็บ = ที่ถูกเลือกไว้ในหน้าแรก t12..t21 ของ web_core_single)
                        CanAdd = true,CanEdit = true,CanDelete = true,CanMove = false,CanStatus = false,CanApprove = true,
                        EnableIssueDate = false,
                        UseViewCreateFrom = "CoreGroup",UseViewEditFrom = "CoreGroup",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = FieldCreate_CoreGroup_NoIssueDate,
                        FieldUpdate = FieldUpdate_CoreGroup_NoIssueDate,
                        FieldApprove = FieldApprove_CoreGroup_NoIssueDate
                    }
                },
                new Module()
                {
                    Name = "NpaOffer",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ยื่นข้อเสนอซื้อ",
                        TextBreadcrumb = "ข้อมูลทรัพย์สิน (NPA)/ยื่นข้อเสนอซื้อ", Table = "web_npa_offer", OrderBy = "id", Sort = "desc",
                        // member_email = อีเมลของสมาชิกที่ล็อกอินอยู่ตอนยื่นข้อเสนอ (ว่าง = ยื่นแบบไม่ล็อกอิน)
                        // ใส่ในช่องค้นหาด้วย เพื่อให้เจ้าหน้าที่ค้นรายการของสมาชิกรายหนึ่งได้จากอีเมลสมาชิก
                        CanAdd = false,CanEdit = false,CanDelete = true,CanMove = false,CanStatus = false,CanApprove = false, UseViewCreateFrom = "AnnounceSubmit",UseViewEditFrom = "AnnounceSubmit",FieldSearch = new() { new("text", new() { "title", "product_code", "data_fname", "data_email", "member_email" }) },
                        CanExport = true,
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("created_at", "วันที่สร้าง"),
                        },
                        FieldApprove = new() { "title" }, FieldCreate = new() { "title", "sort" }, FieldUpdate = new() { "title" },
                        EnableViewDetail = true,
                        ViewDetailData = new()
                        {
                            new ("id", "ไอดี"),
                            new ("created_at", "วันที่สร้าง"),
                            new ("title", "หัวข้อ"),
                            new ("product_code", "รหัสทรัพย์"),
                            new ("data_fname", "ชื่อ-นามสกุล"),
                            new ("data_mobile", "เบอร์โทรศัพท์"),
                            new ("data_email", "อีเมล"),
                            new ("member_email", "อีเมลสมาชิก (เข้าสู่ระบบ)"),
                            new ("offer_price", "ราคาที่เสนอ"),
                            new ("doc_file", "เอกสารแนบ"),
                            new ("doc_desc", "รายละเอียดเพิ่มเติม"),
                        },
                        ExportData = new()
                        {
                            new ("id", "id"),
                            new ("TO_CHAR(created_at, 'DD-MM-YYYY HH24:MI:SS')", "create"),
                            new ("title", "หัวข้อ"),
                            new ("product_code", "รหัสทรัพย์"),
                            new ("data_fname", "ชื่อ-นามสกุล"),
                            new ("data_mobile", "เบอร์โทรศัพท์"),
                            new ("data_email", "อีเมล"),
                            new ("member_email", "อีเมลสมาชิก (เข้าสู่ระบบ)"),
                            new ("offer_price", "ราคาที่เสนอ"),
                            new ("doc_file", "เอกสารแนบ"),
                            new ("doc_desc", "รายละเอียดเพิ่มเติม"),
                        },
                    }
                },
                // อีเมลเจ้าหน้าที่ที่จะได้รับแจ้งเตือนเมื่อมีการยื่นข้อเสนอซื้อ (front-end /th/asset-offer)
                // เก็บคนละระเบียนกับเมนูอีเมลแจ้งเตือนอื่น ๆ — web_core_single module_id = 50
                new Module()
                {
                    Name = "NpaOfferEmail",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "อีเมลเจ้าหน้าที่แจ้งเตือน",
                        TextBreadcrumb = "ข้อมูลทรัพย์สิน (NPA)/อีเมลเจ้าหน้าที่แจ้งเตือน",
                        Table = "web_core_single",TableModuleID = 50,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "MemberEmail",UseViewEditFrom = "MemberEmail",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_MemberEmail,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                #endregion

                #region ลูกค้าปรับโครงสร้างหนี้
                new Module()
                {
                    Name = "NplWeb",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "แก้ไขรายละเอียดโครงการ",
                        TextBreadcrumb = "ลูกค้าปรับโครงสร้างหนี้/แก้ไขรายละเอียดโครงการ",
                        Table = "web_core_single",TableModuleID = 28,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "AboutText1",UseViewEditFrom = "AboutText1",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "NplRegister",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "รายชื่อผู้ลงทะเบียน (NPL)",
                        TextBreadcrumb = "ลูกค้าปรับโครงสร้างหนี้/รายชื่อผู้ลงทะเบียน (NPL)",
                        Table = "web_npl_register",
                        OrderBy = "id",
                        Sort = "desc",
                        CanAdd = false,
                        CanEdit = false,
                        CanDelete = true,
                        CanMove = false,
                        CanStatus = false,
                        CanApprove = false,
                        CanExport = true,
                        UseViewCreateFrom = "NplRegister",
                        UseViewEditFrom = "NplRegister",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("data_fname", "ชื่อ"),
                            new ("data_lname", "นามสกุล"),
                            new ("data_mobile", "เบอร์มือถือ"),
                            new ("data_email", "อีเมล"),
                            new ("created_at", "วันที่สมัคร")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        },
                        EnableViewDetail = true,
                        ViewDetailData = new()
                        {
                            new ("id", "ไอดี"),
                            new ("created_at", "วันที่สร้าง"), 
                            new ("data_fname", "ชื่อ"),
                            new ("data_lname", "นามสกุล"),
                            new ("data_mobile", "เบอร์มือถือ"),
                            new ("data_email", "อีเมล"),
                            new ("data_old_bank", "ธนาคารเจ้าหนี้ (ในอดีต)"),
                            new ("data_idcode", "เลขบัตรประชาชน (13 หลัก)"),
                            new ("data_cus_code", "รหัสลูกหนี้ (ถ้าทราบ)"),
                            new ("data_type_contact", "วัตถุประสงค์การติดต่อ"),
                            new ("data_type_contact_other", "(อื่นๆ)"),
                            new ("data_type_contact_time", "ช่วงเวลาที่สะดวกในการติดต่อ"),
                        },
                        ExportData = new()
                        {
                            new ("id", "id"),
                            new ("TO_CHAR(created_at, 'DD-MM-YYYY HH24:MI:SS')", "create"),
                            new ("data_fname", "ชื่อ"),
                            new ("data_lname", "นามสกุล"),
                            new ("data_mobile", "เบอร์มือถือ"),
                            new ("data_email", "อีเมล"),
                            new ("data_old_bank", "ธนาคารเจ้าหนี้ (ในอดีต)"),
                            new ("data_idcode", "เลขบัตรประชาชน (13 หลัก)"),
                            new ("data_cus_code", "รหัสลูกหนี้ (ถ้าทราบ)"),
                            new ("data_type_contact", "วัตถุประสงค์การติดต่อ"),
                            new ("data_type_contact_other", "(อื่นๆ)"),
                            new ("data_type_contact_time", "ช่วงเวลาที่สะดวกในการติดต่อ"),
                        },
                    }
                },
                new Module()
                {
                    Name = "NplEmail",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "อีเมลเจ้าหน้าที่แจ้งเตือน",
                        TextBreadcrumb = "ลูกค้าปรับโครงสร้างหนี้/อีเมลเจ้าหน้าที่แจ้งเตือน",
                        Table = "web_core_single",TableModuleID = 29,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "MemberEmail",UseViewEditFrom = "MemberEmail",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_MemberEmail,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "NplCustomer",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ข้อมูลลูกหนี้",
                        TextBreadcrumb = "ลูกค้าปรับโครงสร้างหนี้/ข้อมูลลูกหนี้",
                        Table = "web_npa_customer",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        CanMove = true,
                        CanStatus = true,
                        CanApprove = true,
                        UseViewCreateFrom = "NplCustomer",
                        UseViewEditFrom = "NplCustomer",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("pb_status", "สถานะ"),
                            new ("created_at", "วันที่สร้าง"),
                            new ("updated_at", "วันที่แก้ไข")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        }
                    }
                },
                #endregion

                #region โครงการคลินิกแก้หนี้
                new Module()
                {
                    Name = "CliWeb",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "แก้ไขรายละเอียดโครงการ",
                        TextBreadcrumb = "โครงการคลินิกแก้หนี้/แก้ไขรายละเอียดโครงการ",
                        Table = "web_core_single",TableModuleID = 30,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "AboutText1",UseViewEditFrom = "AboutText1",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                #endregion

                #region แจ้งความประสงค์ซื้อทรัพย์
                new Module()
                {
                    Name = "BuyNPA",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "รายการผู้สนใจซื้อทรัพย์",
                        TextBreadcrumb = "แจ้งความประสงค์ซื้อทรัพย์/รายการผู้สนใจซื้อทรัพย์",
                        Table = "web_npa_buy",
                        OrderBy = "id",
                        Sort = "desc",
                        CanAdd = false,
                        CanEdit = false,
                        CanDelete = true,
                        CanMove = false,
                        CanStatus = false,
                        CanApprove = false,
                        CanExport = true,
                        UseViewCreateFrom = "BuyNPA",
                        UseViewEditFrom = "BuyNPA",
                        FieldSearch = new()
                        {
                            new ("text", new() { "data_fname", "data_mobile", "data_email", "product_code" })
                        },
                        ListData = new()
                        {
                            new ("data_fname", "ชื่อ - นามสกุล"),
                            new ("data_mobile", "เบอร์โทรศัพท์"),
                            new ("data_email", "อีเมล"),
                            new ("product_code", "รหัสทรัพย์"),
                            new ("created_at", "วันที่แจ้ง")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        },
                        EnableViewDetail = true,
                        ViewDetailData = new()
                        {
                            new ("id", "ไอดี"),
                            new ("created_at", "วันที่แจ้ง"),
                            new ("product_code", "รหัสทรัพย์"),
                            new ("data_fname", "ชื่อ - นามสกุล"),
                            new ("data_mobile", "เบอร์โทรศัพท์"),
                            new ("data_email", "อีเมล"),
                            new ("data_ads", "สื่อโฆษณา"),
                            new ("data_info", "ข้อมูลที่ต้องการเพิ่มเติม"),
                            new ("appoint_date", "วันที่นัดหมาย"),
                            new ("disclose", "ความยินยอม PDPA (1=ยินยอม, 0=ไม่ยินยอม)"),
                        },
                        ExportData = new()
                        {
                            new ("id", "id"),
                            new ("TO_CHAR(created_at, 'DD-MM-YYYY HH24:MI:SS')", "วันที่แจ้ง"),
                            new ("product_code", "รหัสทรัพย์"),
                            new ("data_fname", "ชื่อ - นามสกุล"),
                            new ("data_mobile", "เบอร์โทรศัพท์"),
                            new ("data_email", "อีเมล"),
                            new ("data_ads", "สื่อโฆษณา"),
                            new ("data_info", "ข้อมูลที่ต้องการเพิ่มเติม"),
                            new ("appoint_date", "วันที่นัดหมาย"),
                            new ("disclose", "ความยินยอม PDPA"),
                        },
                    }
                },
                new Module()
                {
                    Name = "BuyNPAReport",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "รายงานผู้สนใจซื้อทรัพย์",
                        TextBreadcrumb = "แจ้งความประสงค์ซื้อทรัพย์/รายงานผู้สนใจซื้อทรัพย์",
                        Table = "web_npa_report",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        CanMove = true,
                        CanStatus = true,
                        CanApprove = true,
                        UseViewCreateFrom = "BuyNPAReport",
                        UseViewEditFrom = "BuyNPAReport",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("pb_status", "สถานะ"),
                            new ("created_at", "วันที่สร้าง"),
                            new ("updated_at", "วันที่แก้ไข")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        }
                    }
                },
                new Module()
                {
                    Name = "BuyNPAEmail",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "อีเมลเจ้าหน้าที่แจ้งเตือน",
                        TextBreadcrumb = "แจ้งความประสงค์ซื้อทรัพย์/อีเมลเจ้าหน้าที่แจ้งเตือน",
                        Table = "web_core_single",TableModuleID = 31,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "MemberEmail",UseViewEditFrom = "MemberEmail",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_MemberEmail,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                #endregion

                #region นัดหมายชมทรัพย์
                new Module()
                {
                    Name = "MeetNPA",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "รายการนัดหมายชมทรัพย์",
                        TextBreadcrumb = "นัดหมายชมทรัพย์/รายการนัดหมายชมทรัพย์",
                        Table = "web_meet_npa",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        CanMove = true,
                        CanStatus = true,
                        CanApprove = true,
                        UseViewCreateFrom = "MeetNPA",
                        UseViewEditFrom = "MeetNPA",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("pb_status", "สถานะ"),
                            new ("created_at", "วันที่สร้าง"),
                            new ("updated_at", "วันที่แก้ไข")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        }
                    }
                },
                new Module()
                {
                    Name = "MeetNPAEmail",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "อีเมลเจ้าหน้าที่แจ้งเตือน",
                        TextBreadcrumb = "นัดหมายชมทรัพย์/อีเมลเจ้าหน้าที่แจ้งเตือน",
                        Table = "web_core_single",TableModuleID = 32,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "MemberEmail",UseViewEditFrom = "MemberEmail",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_MemberEmail,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                }, 
                #endregion
                  
                #region E-Form
                new Module()
                {
                    Name = "EFormGroup",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "E-Form (กลุ่ม)",
                        TextBreadcrumb = "E-Form/E-Form (กลุ่ม)",
                        Table = "web_core_group",TableModuleID = 17,OrderBy = "sort",Sort = "asc",
                        CanAdd = true,CanEdit = true,CanDelete = true,CanMove = false,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "CoreGroup",UseViewEditFrom = "CoreGroup",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = FieldCreate_CoreGroup,
                        FieldUpdate = FieldUpdate_CoreGroup,
                        FieldApprove = FieldApprove_CoreGroup
                    }
                },
                new Module()
                {
                    Name = "EForm",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "E-Form",
                        TextBreadcrumb = "E-Form/E-Form",
                        Table = "web_eform",
                        OrderBy = "id",
                        Sort = "desc", 
                        TableCate = "web_core_group", TableCateModuleID = "17", TableCateField = "cat_id", TableCateOrderby = "sort", TableCateSort = "asc", TableCateTitle = "title", TableCateLabel = "กลุ่ม", CanAdd = true,CanEdit = true,CanDelete = true,CanMove = false,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "EForm",UseViewEditFrom = "EForm",
                        FieldSearch = FieldSearch_DefaultCat,
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("pb_status", "สถานะ"),
                            new ("updated_at", "วันที่แก้ไข")
                        },
                        FieldApprove = new()
                        {
                            "cat_id",
                            "title",
                            "en_title",
                            "des",
                            "en_des",
                            "issue_date",
                            "expiry_date",
                            "issue_date_config",
                            "input_element",
                            "info",
                            "en_info",
                            "info2",
                            "en_info2",
                            "url",
                            "en_url",
                            "url_target",
                        },
                        FieldCreate = new()
                        {
                            "cat_id",
                            "title",
                            "en_title",
                            "des",
                            "en_des",
                            "issue_date",
                            "expiry_date",
                            "issue_date_config",
                            "input_element",
                            "info",
                            "en_info",
                            "info2",
                            "en_info2",
                            "url",
                            "en_url",
                            "url_target",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "cat_id",
                            "title",
                            "en_title",
                            "des",
                            "en_des",
                            "issue_date",
                            "expiry_date",
                            "issue_date_config",
                            "input_element",
                            "info",
                            "en_info",
                            "info2",
                            "en_info2",
                            "url",
                            "en_url",
                            "url_target",
                        }
                    }
                },
                new Module()
                {
                    Name = "EFormSubmit",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "รายชื่อผู้กรอกแบบฟอร์ม",
                        TextBreadcrumb = "แบบฟอร์มออนไลน์/รายชื่อผู้กรอกแบบฟอร์ม",
                        Table = "web_eform_submit",
                        CanExport = true,
                        OrderBy = "created_at",
                        Sort = "desc",
                        TableCate = "web_eform",
                        TableCateField = "cat_id",
                        TableCateOrderby = "title",
                        TableCateSort = "asc",
                        TableCateTitle = "title",
                        TableCateLabel = "กลุ่ม",
                        CanAdd = false,
                        CanEdit = false,
                        CanDelete = true,
                        CanMove = false,
                        CanStatus = false,
                        CanApprove = false,
                        EnableDateSearch = true,
                        UseViewCreateFrom = "EFormSubmit",
                        UseViewEditFrom = "EFormSubmit",
                        UseViewDetailFrom = "EFormSubmit",
                        EnableViewDetail = true,
                        ViewDetailData = new()
                        {
                            new ("id", "ไอดี"),
                            new ("created_at", "วันที่-เวลา ส่งข้อมูล"),
                            new ("cat_id", "E-Form"),
                            new ("info", "Field"),
                            new ("en_info", "Value"),
                            new ("utm_id", "utm_id"),
                            new ("utm_source", "utm_source"),
                            new ("utm_medium", "utm_medium"),
                            new ("utm_campaign", "utm_campaign"),
                            new ("utm_content", "utm_content"),
                            new ("utm_term", "utm_term"),
                            new ("des", "IP Address"),
                        },
                        ExportData = new()
                        {
                            new ("id", "id"),
                            new ("TO_CHAR(created_at, 'DD-MM-YYYY HH24:MI:SS')", "create"),
                            new ("des", "ip"),
                            new ("utm_id", "utm_id"),
                            new ("utm_source", "utm_source"),
                            new ("utm_medium", "utm_medium"),
                            new ("utm_campaign", "utm_campaign"),
                            new ("utm_content", "utm_content"),
                            new ("utm_term", "utm_term"),
                            new ("info", "field"),
                            new ("en_info", "value")
                        },
                        FieldSearch = new()
                        {
                            new ("text", new() { "title","updated_by", "info", "en_info" }),
                            new ("cat_id", new() { "cat_id" })
                        },
                        FieldSearchIsEqual = new() { "cat_id" },
                        ListData = new()
                        {
                            new ("cat_id", "แบบฟอร์ม"),
                            new ("info", "ข้อมูลที่กรอก (สรุป)"),
                            new ("des", "IP Address"),
                            new ("created_at", "วันที่ส่งข้อมูล")
                        },
                        FieldApprove = new()
                        {
                            "title",
                            "en_title",
                            "des",
                            "en_des",
                            "img1",
                            "url",
                            "en_url",
                            "url_target",

                        },
                        FieldCreate = new()
                        {
                            "title",
                            "en_title",
                            "des",
                            "en_des",
                            "img1",
                            "url",
                            "en_url",
                            "url_target",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title",
                            "en_title",
                            "des",
                            "en_des",
                            "url",
                            "en_url",
                            "url_target",
                            "img1",
                        }
                    }
                }, 
                #endregion

                #region ดาวน์โหลดเอกสาร
                new Module()
                {
                    Name = "DownloadGroup",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ดาวน์โหลด (กลุ่ม)",
                        TextBreadcrumb = "ดาวน์โหลด/ดาวน์โหลด (กลุ่ม)",
                        Table = "web_core_group",TableModuleID = 13,OrderBy = "sort",Sort = "asc",
                        CanAdd = true,CanEdit = true,CanDelete = true,CanMove = false,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "CoreGroup",UseViewEditFrom = "CoreGroup",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = FieldCreate_CoreGroup,
                        FieldUpdate = FieldUpdate_CoreGroup,
                        FieldApprove = FieldApprove_CoreGroup
                    }
                },
                new Module()
                {
                    Name = "Download",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ดาวน์โหลด",TextBreadcrumb = "ดาวน์โหลด/ดาวน์โหลด",
                        Table = "web_core_news",TableModuleID = 10,OrderBy = "pb_date_news desc, id",Sort = "desc",
                        TableCate = "web_core_group", TableCateModuleID = "13", TableCateField = "cat_id", TableCateOrderby = "sort", TableCateSort = "asc", TableCateTitle = "title", TableCateLabel = "กลุ่ม", CanAdd = true,CanEdit = true,CanDelete = true,CanMove = false,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "Download",UseViewEditFrom = "Download",
                        FieldSearch = FieldSearch_DefaultCat, ListData = ListData_Default,
                        FieldCreate = FieldCreate_CoreNews,
                        FieldUpdate = FieldUpdate_CoreNews,
                        FieldApprove = FieldApprove_CoreNews
                    }
                },
                new Module()
                {
                    Name = "DownloadSubmit",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "รายชื่อผู้กรอกข้อมูล",
                        TextBreadcrumb = "ดาวน์โหลดเอกสาร/รายชื่อผู้กรอกข้อมูล",Table = "web_download_submit", OrderBy = "id", Sort = "desc",
                        TableCate = "web_core_news", TableCateModuleID = "10", TableCateField = "cat_id", TableCateOrderby = "id", TableCateSort = "desc", TableCateTitle = "title", TableCateLabel = "ไฟล์ดาวน์โหลด", CanAdd = false,CanEdit = false,CanDelete = true,CanMove = false,CanStatus = false,CanApprove = false, UseViewCreateFrom = "AnnounceSubmit",UseViewEditFrom = "AnnounceSubmit",FieldSearch = FieldSearch_DefaultCat,
                        CanExport = true,
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("created_at", "วันที่สร้าง"),
                        },
                        FieldApprove = new() { "title" }, FieldCreate = new() { "title", "sort" }, FieldUpdate = new() { "title" },
                        EnableViewDetail = true,
                        ViewDetailData = new()
                        {
                            new ("id", "ไอดี"),
                            new ("created_at", "วันที่สร้าง"),
                            new ("cat_id", "ไฟล์ดาวน์โหลด"),
                            new ("data_idcode", "เลขประจำตัวผู้เสียภาษีของสถานประกอบการ"),
                            new ("data_fname", "ชื่อ-นามสกุล"),
                            new ("data_company", "ชื่อสถานประกอบการ"),
                            new ("data_building", "ชื่ออาคาร"),
                            new ("data_roomid", "ห้องเลขที่"),
                            new ("data_floor", "ชั้นที่"),
                            new ("data_village", "ชื่อหมู่บ้าน"),
                            new ("data_addressid", "เลขที่"),
                            new ("data_moo", "หมู่ที่"),
                            new ("data_soi", "ตรอก/ซอย"),
                            new ("data_road", "ถนน"),
                            new ("data_ad_province", "จังหวัด"),
                            new ("data_base_amphur", "อำเภอ"),
                            new ("data_base_tambol", "ตำบล"),
                            new ("data_base_zipcode", "รหัสไปรษณีย์"),
                            new ("data_mobile", "หมายเลขโทรศัพท์"),
                            new ("data_email", "อีเมล"),
                        },
                        ExportData = new()
                        {
                            new ("id", "id"),
                            new ("TO_CHAR(created_at, 'DD-MM-YYYY HH24:MI:SS')", "create"),
                            new ("data_fname", "ชื่อ-นามสกุล"),
                            new ("data_company", "ชื่อสถานประกอบการ"),
                            new ("data_building", "ชื่ออาคาร"),
                            new ("data_roomid", "ห้องเลขที่"),
                            new ("data_floor", "ชั้นที่"),
                            new ("data_village", "ชื่อหมู่บ้าน"),
                            new ("data_addressid", "เลขที่"),
                            new ("data_moo", "หมู่ที่"),
                            new ("data_soi", "ตรอก/ซอย"),
                            new ("data_road", "ถนน"),
                            new ("data_ad_province", "จังหวัด"),
                            new ("data_base_amphur", "อำเภอ"),
                            new ("data_base_tambol", "ตำบล"),
                            new ("data_base_zipcode", "รหัสไปรษณีย์"),
                            new ("data_mobile", "หมายเลขโทรศัพท์"),
                            new ("data_email", "อีเมล"),
                        },
                    }
                },
                new Module()
                {
                    Name = "DownloadReport",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "รายงานการดาวน์โหลด",
                        TextBreadcrumb = "ดาวน์โหลดเอกสาร/รายงานการดาวน์โหลด",
                        Table = "web_download_report",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        CanMove = true,
                        CanStatus = true,
                        CanApprove = true,
                        UseViewCreateFrom = "DownloadReport",
                        UseViewEditFrom = "DownloadReport",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("pb_status", "สถานะ"),
                            new ("created_at", "วันที่สร้าง"),
                            new ("updated_at", "วันที่แก้ไข")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        }
                    }
                },
                #endregion

                #region ประกาศ
                new Module()
                {
                    Name = "AnnounceRate",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ประกาศอัตราดอกเบี้ย",
                        TextBreadcrumb = "ประกาศ/ประกาศอัตราดอกเบี้ย",
                        Table = "web_core_news",TableModuleID = 4,OrderBy = "pb_date_news desc, id",Sort = "desc",
                        CanAdd = true,CanEdit = true,CanDelete = true,CanMove = false,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "AnnounceRate",UseViewEditFrom = "AnnounceRate",
                        FieldSearch = FieldSearch_DefaultCat, ListData = ListData_Default,
                        FieldCreate = FieldCreate_CoreNews,
                        FieldUpdate = FieldUpdate_CoreNews,
                        FieldApprove = FieldApprove_CoreNews
                    }
                },
                new Module()
                {
                    Name = "AnnounceNPA",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ประกาศจาก NPL/NPA",
                        TextBreadcrumb = "ประกาศ/ประกาศจาก NPL/NPA",
                        Table = "web_core_news",TableModuleID = 5,OrderBy = "pb_date_news desc, id",Sort = "desc",
                        CanAdd = true,CanEdit = true,CanDelete = true,CanMove = false,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "AnnounceRate",UseViewEditFrom = "AnnounceRate",
                        FieldSearch = FieldSearch_DefaultCat, ListData = ListData_Default,
                        FieldCreate = FieldCreate_CoreNews,
                        FieldUpdate = FieldUpdate_CoreNews,
                        FieldApprove = FieldApprove_CoreNews
                    }
                },
                new Module()
                {
                    Name = "AnnounceOtherGroup",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ประกาศทั่วไป (กลุ่ม)",
                        TextBreadcrumb = "ประกาศ/ประกาศทั่วไป (กลุ่ม)",
                        Table = "web_core_group",TableModuleID = 9,OrderBy = "sort",Sort = "asc",
                        CanAdd = true,CanEdit = true,CanDelete = true,CanMove = false,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "CoreGroup",UseViewEditFrom = "CoreGroup",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = FieldCreate_CoreGroup,
                        FieldUpdate = FieldUpdate_CoreGroup,
                        FieldApprove = FieldApprove_CoreGroup
                    }
                },
                new Module()
                {
                    Name = "AnnounceOther",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ประกาศทั่วไป",
                        TextBreadcrumb = "ประกาศ/ประกาศทั่วไป",
                        Table = "web_core_news",TableModuleID = 6,OrderBy = "pb_date_news desc, id",Sort = "desc",
                        TableCate = "web_core_group", TableCateModuleID = "9", TableCateField = "cat_id", TableCateOrderby = "sort", TableCateSort = "asc", TableCateTitle = "title", TableCateLabel = "กลุ่ม", CanAdd = true,CanEdit = true,CanDelete = true,CanMove = false,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "AnnounceOther",UseViewEditFrom = "AnnounceOther",
                        FieldSearch = FieldSearch_DefaultCat, ListData = ListData_Default,
                        FieldCreate = FieldCreate_CoreNews,
                        FieldUpdate = FieldUpdate_CoreNews,
                        FieldApprove = FieldApprove_CoreNews
                    }
                }, 
                #endregion

                #region ประกาศจัดซื้อจัดจ้าง
                new Module()
                {
                    Name = "AnnouncePro",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ประกาศจัดซื้อจัดจ้าง",
                        TextBreadcrumb = "ประกาศจัดซื้อจัดจ้าง/ประกาศจัดซื้อจัดจ้าง", 
                        Table = "web_core_news",TableModuleID = 7,OrderBy = "pb_date_news desc, id",Sort = "desc",
                        TableCate = "web_core_group", TableCateModuleID = "10", TableCateField = "cat_id", TableCateOrderby = "sort", TableCateSort = "asc", TableCateTitle = "title", TableCateLabel = "ประเภท", CanAdd = true,CanEdit = true,CanDelete = true,CanMove = false,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "AnnouncePro",UseViewEditFrom = "AnnouncePro",
                        // เมนูนี้ไม่แสดงคอลัมน์ วันที่สร้าง/วันที่แก้ไข (ต่างจาก ListData_Default ที่เมนูอื่นใช้)
                        FieldSearch = FieldSearch_DefaultCat, ListData = new() { new("title", "หัวข้อ"), new("pb_status", "สถานะ") },
                        FieldCreate = FieldCreate_CoreNews,
                        FieldUpdate = FieldUpdate_CoreNews,
                        FieldApprove = FieldApprove_CoreNews
                    }
                },
                new Module()
                {
                    Name = "AnnounceJob",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ตำแหน่งงาน",
                        TextBreadcrumb = "ร่วมงานกับเรา/ตำแหน่งงาน",
                        Table = "web_core_news",TableModuleID = 8,OrderBy = "pb_date_news desc, id",Sort = "desc",
                        CanAdd = true,CanEdit = true,CanDelete = true,CanMove = false,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "AnnounceJob",UseViewEditFrom = "AnnounceJob",
                        FieldSearch = FieldSearch_DefaultCat, ListData = ListData_Default,
                        FieldCreate = FieldCreate_CoreNews,
                        FieldUpdate = FieldUpdate_CoreNews,
                        FieldApprove = FieldApprove_CoreNews
                    }
                },
                new Module()
                {
                    Name = "AnnounceProGroup",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ประเภท",
                        TextBreadcrumb = "ประกาศจัดซื้อจัดจ้าง/ประเภท",
                        Table = "web_core_group",TableModuleID = 10,OrderBy = "sort",Sort = "asc",
                        CanAdd = true,CanEdit = true,CanDelete = true,CanMove = true,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "CoreGroup",UseViewEditFrom = "CoreGroup",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = FieldCreate_CoreGroup,
                        FieldUpdate = FieldUpdate_CoreGroup,
                        FieldApprove = FieldApprove_CoreGroup
                    }
                },
                new Module()
                {
                    Name = "AnnounceProType",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "วิธีการจัดซื้อ",
                        TextBreadcrumb = "ประกาศจัดซื้อจัดจ้าง/วิธีการจัดซื้อ",
                        Table = "web_core_group",TableModuleID = 11,OrderBy = "sort",Sort = "asc",
                        CanAdd = true,CanEdit = true,CanDelete = true,CanMove = true,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "CoreGroup",UseViewEditFrom = "CoreGroup",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = FieldCreate_CoreGroup,
                        FieldUpdate = FieldUpdate_CoreGroup,
                        FieldApprove = FieldApprove_CoreGroup
                    }
                },
                new Module()
                {
                    Name = "AnnounceSubmit",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "รายชื่อผู้ลงทะเบียน", TextBreadcrumb = "ประกาศ/รายชื่อผู้ลงทะเบียน", Table = "web_announce_submit", OrderBy = "id", Sort = "desc",
                        TableCate = "web_core_news", TableCateModuleID = "7", TableCateField = "cat_id", TableCateOrderby = "id", TableCateSort = "desc", TableCateTitle = "title", TableCateLabel = "ประกาศ", CanAdd = false,CanEdit = false,CanDelete = true,CanMove = false,CanStatus = false,CanApprove = false, UseViewCreateFrom = "AnnounceSubmit",UseViewEditFrom = "AnnounceSubmit",FieldSearch = FieldSearch_DefaultCat,
                        // cat_id ต้องเทียบแบบตรงตัว ไม่ใช่ LIKE — ไม่งั้นเลือกประกาศ id 17 จะติดรายการของ id 171/173 มาด้วย
                        FieldSearchIsEqual = new() { "cat_id" },
                        CanExport = true,
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("file1", "ไฟล์อัพโหลด"),
                            new ("file_status", "สถานะไฟล์"),
                            new ("created_at", "วันที่สร้าง"),
                        },
                        FieldApprove = new() { "title" }, FieldCreate = new() { "title", "sort" }, FieldUpdate = new() { "title" },
                        EnableViewDetail = true,
                        ViewDetailData = new()
                        {
                            new ("id", "ไอดี"),
                            new ("created_at", "วันที่สร้าง"),
                            new ("cat_id", "ประกาศจัดซื้อจัดจ้าง"),
                            new ("data_idcode", "เลขประจำตัวผู้เสียภาษีของสถานประกอบการ"),
                            new ("data_fname", "ชื่อ-นามสกุล"),
                            new ("data_company", "ชื่อสถานประกอบการ"),
                            new ("data_building", "ชื่ออาคาร"),
                            new ("data_roomid", "ห้องเลขที่"),
                            new ("data_floor", "ชั้นที่"),
                            new ("data_village", "ชื่อหมู่บ้าน"),
                            new ("data_addressid", "เลขที่"),
                            new ("data_moo", "หมู่ที่"),
                            new ("data_soi", "ตรอก/ซอย"),
                            new ("data_road", "ถนน"),
                            new ("data_ad_province", "จังหวัด"),
                            new ("data_base_amphur", "อำเภอ"),
                            new ("data_base_tambol", "ตำบล"),
                            new ("data_base_zipcode", "รหัสไปรษณีย์"),
                            new ("data_mobile", "หมายเลขโทรศัพท์"),
                            new ("data_email", "อีเมล"),
                        },
                        ExportData = new()
                        {
                            new ("id", "id"),
                            new ("TO_CHAR(created_at, 'DD-MM-YYYY HH24:MI:SS')", "create"), 
                            new ("data_fname", "ชื่อ-นามสกุล"),
                            new ("data_company", "ชื่อสถานประกอบการ"),
                            new ("data_building", "ชื่ออาคาร"),
                            new ("data_roomid", "ห้องเลขที่"),
                            new ("data_floor", "ชั้นที่"),
                            new ("data_village", "ชื่อหมู่บ้าน"),
                            new ("data_addressid", "เลขที่"),
                            new ("data_moo", "หมู่ที่"),
                            new ("data_soi", "ตรอก/ซอย"),
                            new ("data_road", "ถนน"),
                            new ("data_ad_province", "จังหวัด"),
                            new ("data_base_amphur", "อำเภอ"),
                            new ("data_base_tambol", "ตำบล"),
                            new ("data_base_zipcode", "รหัสไปรษณีย์"),
                            new ("data_mobile", "หมายเลขโทรศัพท์"),
                            new ("data_email", "อีเมล"),
                        },
                    }
                },
                new Module()
                {
                    Name = "AnnounceJobSubmit",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "รายชื่อผู้สมัครงาน", TextBreadcrumb = "ร่วมงานกับเรา/รายชื่อผู้สมัครงาน", Table = "web_job_submit", OrderBy = "id", Sort = "desc",
                        TableCate = "web_core_news", TableCateModuleID = "8", TableCateField = "cat_id", TableCateOrderby = "id", TableCateSort = "desc", TableCateTitle = "title", TableCateLabel = "ตำแหน่ง", CanAdd = false,CanEdit = false,CanDelete = true,CanMove = false,CanStatus = false,CanApprove = false, UseViewCreateFrom = "AnnounceSubmit",UseViewEditFrom = "AnnounceSubmit",FieldSearch = FieldSearch_DefaultCat,
                        CanExport = true,
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("created_at", "วันที่สร้าง"),
                        },
                        FieldApprove = new() { "title" }, FieldCreate = new() { "title", "sort" }, FieldUpdate = new() { "title" },
                        EnableViewDetail = true,
                        ViewDetailData = new()
                        {
                            new ("id", "ไอดี"),
                            new ("created_at", "วันที่สร้าง"),
                            new ("cat_id", "ตำแหน่ง"),
                            new ("data_fname", "ชื่อ-นามสกุล"),
                            new ("data_age", "อายุ"),
                            new ("data_gender", "เพศ"),
                            new ("data_email", "Email"),
                            new ("data_mobile", "เบอร์โทรศัพท์"),
                            new ("data_doc", "ไฟล์แนบ"),
                            new ("data_remark", "ตำแหน่งนี้เหมาะกับคุณอย่างไร"),
                        },
                        ExportData = new()
                        {
                            new ("id", "id"),
                            new ("TO_CHAR(created_at, 'DD-MM-YYYY HH24:MI:SS')", "create"),
                            new ("cat_id", "ตำแหน่ง"),
                            new ("data_fname", "ชื่อ-นามสกุล"),
                            new ("data_age", "อายุ"),
                            new ("data_gender", "เพศ"),
                            new ("data_email", "Email"),
                            new ("data_mobile", "เบอร์โทรศัพท์"),
                            new ("data_doc", "ไฟล์แนบ"),
                            new ("data_remark", "ตำแหน่งนี้เหมาะกับคุณอย่างไร"),
                        },
                    }
                },
                new Module()
                {
                    Name = "AnnounceJobSubmitFull",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "รายชื่อผู้สมัครงาน(ครบ)", TextBreadcrumb = "ร่วมงานกับเรา/รายชื่อผู้สมัครงาน(ครบ)", Table = "web_job_submit_full", OrderBy = "id", Sort = "desc",
                        TableCate = "web_core_news", TableCateModuleID = "8", TableCateField = "cat_id", TableCateOrderby = "id", TableCateSort = "desc", TableCateTitle = "title", TableCateLabel = "ตำแหน่ง", CanAdd = false,CanEdit = false,CanDelete = true,CanMove = false,CanStatus = false,CanApprove = false, UseViewDetailFrom = "AnnounceJobSubmitFull",
                        FieldSearch = new() { new("text", new() { "job_title", "firstname_th", "lastname_th", "email", "mobile" }), new("cat_id", new() { "cat_id" }) },
                        CanExport = true,
                        ListData = new()
                        {
                            new ("job_title", "ตำแหน่ง"),
                            new ("firstname_th", "ชื่อ"),
                            new ("lastname_th", "นามสกุล"),
                            new ("email", "Email"),
                            new ("mobile", "เบอร์โทรศัพท์"),
                            new ("created_at", "วันที่สมัคร"),
                        },
                        EnableViewDetail = true,
                        ViewDetailData = new()
                        {
                            new ("id", "ไอดี"),
                        },
                        ExportData = ExportData_JobSubmitFull,
                    }
                },
                new Module()
                {
                    Name = "JobCMS",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "รูปประกอบ",TextBreadcrumb = "ร่วมงานกับเรา/รูปประกอบ",
                        Table = "web_core_single",TableModuleID = 20,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "JobCMS",UseViewEditFrom = "JobCMS",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_JobCMS,
                        FieldApprove = FieldApprove_JobCMS
                    }
                },
                #endregion

                #region ติดต่อเรา
                new Module()
                {
                    Name = "ContactOffice",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "สำนักงานใหญ่/สาขา",
                        TextBreadcrumb = "ติดต่อเรา / สำนักงานใหญ่/สาขา",
                        Table = "web_core_news",TableModuleID = 9,OrderBy = "pb_date_news desc, id",Sort = "desc",
                        CanAdd = true,CanEdit = true,CanDelete = true,CanMove = false,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "ContactOffice",UseViewEditFrom = "ContactOffice",
                        FieldSearch = FieldSearch_DefaultCat, ListData = ListData_Default,
                        FieldCreate = FieldCreate_CoreNews,
                        FieldUpdate = FieldUpdate_CoreNews,
                        FieldApprove = FieldApprove_CoreNews
                    }
                },
                new Module()
                {
                    Name = "ContactCMS",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ช่องทางร้องเรียน/ข้อเสนอแนะ",TextBreadcrumb = "ติดต่อเรา / ช่องทางร้องเรียน/ข้อเสนอแนะ",
                        Table = "web_core_single",TableModuleID = 21,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "ContactCMS",UseViewEditFrom = "ContactCMS",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "ContactSubmit",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "รายชื่อผู้ติดต่อ", TextBreadcrumb = "ติดต่อเรา/รายชื่อผู้ติดต่อ", Table = "web_contact_submit", OrderBy = "id", Sort = "desc",
                        TableCate = "web_core_group", TableCateModuleID = "12", TableCateField = "cat_id", TableCateOrderby = "id", TableCateSort = "desc", TableCateTitle = "title", TableCateLabel = "หัวข้อ", CanAdd = false,CanEdit = false,CanDelete = true,CanMove = false,CanStatus = false,CanApprove = false, UseViewCreateFrom = "AnnounceSubmit",UseViewEditFrom = "AnnounceSubmit",FieldSearch = FieldSearch_DefaultCat,
                        CanExport = true,
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("created_at", "วันที่สร้าง"),
                        },
                        FieldApprove = new() { "title" }, FieldCreate = new() { "title", "sort" }, FieldUpdate = new() { "title" },
                        EnableViewDetail = true,
                        ViewDetailData = new()
                        {
                            new ("id", "ไอดี"),
                            new ("created_at", "วันที่สร้าง"),
                            new ("cat_id", "หัวข้อ"),
                            new ("data_pre_name", "คำนำหน้า"),
                            new ("data_fname", "ชื่อ-นามสกุล"),
                            new ("data_mobile", "เบอร์โทรศัพท์"),
                            new ("data_email", "Email"),
                            new ("data_msg", "รายละเอียด"),
                        },
                        ExportData = new()
                        {
                            new ("id", "id"),
                            new ("TO_CHAR(created_at, 'DD-MM-YYYY HH24:MI:SS')", "create"),
                            new ("data_pre_name", "คำนำหน้า"),
                            new ("data_fname", "ชื่อ-นามสกุล"),
                            new ("data_mobile", "เบอร์โทรศัพท์"),
                            new ("data_email", "Email"),
                            new ("data_msg", "รายละเอียด"),
                        },
                    }
                },
                #endregion
                 
                #region โปรโมชั่น
                new Module()
                {
                    Name = "PromotionGroup",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "โปรโมชั่น (กลุ่ม)",
                        TextBreadcrumb = "โปรโมชั่น/โปรโมชั่น (กลุ่ม)",
                        Table = "web_core_group",TableModuleID = 16,OrderBy = "sort",Sort = "asc",
                        CanAdd = true,CanEdit = true,CanDelete = true,CanMove = false,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "CoreGroup",UseViewEditFrom = "CoreGroup",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = FieldCreate_CoreGroup,
                        FieldUpdate = FieldUpdate_CoreGroup,
                        FieldApprove = FieldApprove_CoreGroup
                    }
                },
                new Module()
                {
                    Name = "Promotion",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "โปรโมชั่น",
                        TextBreadcrumb = "โปรโมชั่น/โปรโมชั่น",
                        Table = "web_core_news",TableModuleID = 13,OrderBy = "pb_date_news desc, id",Sort = "desc",
                        TableCate = "web_core_group", TableCateModuleID = "16", TableCateField = "cat_id", TableCateOrderby = "sort", TableCateSort = "asc", TableCateTitle = "title", TableCateLabel = "กลุ่ม", CanAdd = true,CanEdit = true,CanDelete = true,CanMove = false,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "CoreNews",UseViewEditFrom = "CoreNews",
                        FieldSearch = FieldSearch_DefaultCat, ListData = ListData_Default,
                        FieldCreate = FieldCreate_CoreNews,
                        FieldUpdate = FieldUpdate_CoreNews,
                        FieldApprove = FieldApprove_CoreNews
                    }
                }, 
                #endregion

                #region Banner
                new Module()
                {
                    Name = "BannerGroup",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "Banner (กลุ่ม)",
                        TextBreadcrumb = "Banner/Banner (กลุ่ม)",
                        Table = "web_banner_group",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        CanMove = true,
                        CanStatus = true,
                        CanApprove = true,
                        UseViewCreateFrom = "BannerGroup",
                        UseViewEditFrom = "BannerGroup",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("pb_status", "สถานะ"),
                            new ("created_at", "วันที่สร้าง"),
                            new ("updated_at", "วันที่แก้ไข")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        }
                    }
                },
                new Module()
                {
                    Name = "Banner",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "Banner",
                        TextBreadcrumb = "Banner/Banner",
                        Table = "web_banner",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        CanMove = true,
                        CanStatus = true,
                        CanApprove = true,
                        UseViewCreateFrom = "Banner",
                        UseViewEditFrom = "Banner",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("pb_status", "สถานะ"),
                            new ("created_at", "วันที่สร้าง"),
                            new ("updated_at", "วันที่แก้ไข")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        }
                    }
                },
                #endregion

                #region FAQ
                new Module()
                {
                    Name = "FAQGroup",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "คำถามที่พบบ่อย (กลุ่ม)",
                        TextBreadcrumb = "คำถามที่พบบ่อย/คำถามที่พบบ่อย (กลุ่ม)",
                        Table = "web_core_group",TableModuleID = 14,OrderBy = "sort",Sort = "asc",
                        CanAdd = true,CanEdit = true,CanDelete = true,CanMove = true,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "CoreGroup",UseViewEditFrom = "CoreGroup",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = FieldCreate_CoreGroup,
                        FieldUpdate = FieldUpdate_CoreGroup,
                        FieldApprove = FieldApprove_CoreGroup
                    }
                },
                new Module()
                {
                    Name = "FAQ",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "คำถามที่พบบ่อย",
                        TextBreadcrumb = "คำถามที่พบบ่อย/คำถามที่พบบ่อย",
                        Table = "web_core_news",TableModuleID = 11,OrderBy = "sort",Sort = "asc",
                        TableCate = "web_core_group", TableCateModuleID = "14", TableCateField = "cat_id", TableCateOrderby = "sort", TableCateSort = "asc", TableCateTitle = "title", TableCateLabel = "กลุ่ม", CanAdd = true,CanEdit = true,CanDelete = true,CanMove = true,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "FAQ",UseViewEditFrom = "FAQ",
                        FieldSearch = FieldSearch_DefaultCat, ListData = ListData_Default,
                        FieldCreate = FieldCreate_CoreNews,
                        FieldUpdate = FieldUpdate_CoreNews,
                        FieldApprove = FieldApprove_CoreNews
                    }
                }, 
                #endregion

                #region ลิงค์หน่วยงาน
                new Module()
                {
                    Name = "LinkGroup",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ลิงค์หน่วยงาน (กลุ่ม)",
                        TextBreadcrumb = "ลิงค์หน่วยงาน/ลิงค์หน่วยงาน (กลุ่ม)",
                        Table = "web_core_group",TableModuleID = 15,OrderBy = "sort",Sort = "asc",
                        CanAdd = true,CanEdit = true,CanDelete = true,CanMove = true,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "CoreGroup",UseViewEditFrom = "CoreGroup",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = FieldCreate_CoreGroup,
                        FieldUpdate = FieldUpdate_CoreGroup,
                        FieldApprove = FieldApprove_CoreGroup
                    }
                },
                new Module()
                {
                    Name = "Link",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ลิงค์หน่วยงาน",
                        TextBreadcrumb = "ลิงค์หน่วยงาน/ลิงค์หน่วยงาน",
                        Table = "web_core_news",TableModuleID = 12,OrderBy = "pb_date_news desc, id",Sort = "desc",
                        TableCate = "web_core_group", TableCateModuleID = "15", TableCateField = "cat_id", TableCateOrderby = "sort", TableCateSort = "asc", TableCateTitle = "title", TableCateLabel = "กลุ่ม", CanAdd = true,CanEdit = true,CanDelete = true,CanMove = false,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "Link",UseViewEditFrom = "Link",
                        FieldSearch = FieldSearch_DefaultCat, ListData = ListData_Default,
                        FieldCreate = FieldCreate_CoreNews,
                        FieldUpdate = FieldUpdate_CoreNews,
                        FieldApprove = FieldApprove_CoreNews
                    }
                },
                new Module()
                {
                    Name = "LinkCMS",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "แลกเปลี่ยนลิงค์",TextBreadcrumb = "ลิงค์หน่วยงาน / แลกเปลี่ยนลิงค์",
                        Table = "web_core_single",TableModuleID = 22,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "LinkCMS",UseViewEditFrom = "LinkCMS",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "LinkSubmit",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "รายชื่อผู้แลกเปลี่ยน",
                        TextBreadcrumb = "ลิงค์หน่วยงาน/รายชื่อผู้แลกเปลี่ยน",Table = "web_link_submit", OrderBy = "id", Sort = "desc",
                        CanAdd = false,CanEdit = false,CanDelete = true,CanMove = false,CanStatus = false,CanApprove = false, UseViewCreateFrom = "AnnounceSubmit",UseViewEditFrom = "AnnounceSubmit",FieldSearch = FieldSearch_DefaultCat,
                        CanExport = true,
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("created_at", "วันที่สร้าง"),
                        },
                        FieldApprove = new() { "title" }, FieldCreate = new() { "title", "sort" }, FieldUpdate = new() { "title" },
                        EnableViewDetail = true,
                        ViewDetailData = new()
                        {
                            new ("id", "ไอดี"),
                            new ("created_at", "วันที่สร้าง"),
                            new ("data_fname", "ชื่อ-นามสกุล"),
                            new ("data_email", "อีเมล"),
                            new ("data_url", "URL"),
                            new ("data_file", "Banner"),
                        },
                        ExportData = new()
                        {
                            new ("id", "id"),
                            new ("TO_CHAR(created_at, 'DD-MM-YYYY HH24:MI:SS')", "create"),
                            new ("data_fname", "ชื่อ-นามสกุล"),
                            new ("data_email", "อีเมล"),
                            new ("data_url", "URL"),
                            new ("data_file", "Banner"),
                        },
                    }
                },
                #endregion

                #region Footer 
                new Module()
                {
                    Name = "FooterPoll",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "แบบสำรวจ",TextBreadcrumb = "เมนู Footer / แบบสำรวจ",
                        Table = "web_core_single",TableModuleID = 23,OrderBy = "pb_date_news desc, id",Sort = "desc",
                        CanAdd = true,CanEdit = true,CanDelete = true,CanMove = false,CanStatus = true,CanApprove = true,
                        UseViewCreateFrom = "PollVote",UseViewEditFrom = "PollVote",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = FieldCreate_CoreSingle,
                        FieldUpdate = FieldUpdate_FooterPoll,
                        FieldApprove = FieldApprove_FooterPoll
                    }
                },
                new Module()
                {
                    Name = "FooterPrivacy1",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ประกาศความเป็นส่วนตัว",TextBreadcrumb = "เมนู Footer / ประกาศความเป็นส่วนตัว",
                        Table = "web_core_single",TableModuleID = 24,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "AboutText1",UseViewEditFrom = "AboutText1",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "FooterPrivacy1Submit",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "รายชื่อผู้ยินยอม", TextBreadcrumb = "เมนู Footer/รายชื่อผู้ยินยอม", Table = "web_privacy1_submit", OrderBy = "id", Sort = "desc",
                        CanAdd = false,CanEdit = false,CanDelete = true,CanMove = false,CanStatus = false,CanApprove = false, UseViewCreateFrom = "AnnounceSubmit",UseViewEditFrom = "AnnounceSubmit",FieldSearch = FieldSearch_DefaultCat,
                        CanExport = true,
                        ListData = new()
                        {
                            new ("title", "Browser"),
                            new ("en_title", "IP Address"),
                            new ("created_at", "วันที่สร้าง"),
                        },
                        FieldApprove = new() { "title" }, FieldCreate = new() { "title", "sort" }, FieldUpdate = new() { "title" },
                        EnableViewDetail = true,
                        ViewDetailData = new()
                        {
                            new ("id", "ไอดี"),
                            new ("title", "Browser"),
                            new ("en_title", "IP Address"),
                            new ("created_at", "วันที่สร้าง")
                        },
                        ExportData = new()
                        {
                            new ("id", "id"),
                            new ("title", "Browser"),
                            new ("en_title", "IP Address"),
                            new ("TO_CHAR(created_at, 'DD-MM-YYYY HH24:MI:SS')", "create"),
                        },
                    }
                },
                new Module()
                {
                    Name = "FooterPrivacy2",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ข้อตกลงและเงื่อนไข",TextBreadcrumb = "เมนู Footer / ข้อตกลงและเงื่อนไข",
                        Table = "web_core_single",TableModuleID = 25,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "AboutText1",UseViewEditFrom = "AboutText1",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "FooterPrivacy2Submit",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "รายชื่อผู้ยินยอม", TextBreadcrumb = "เมนู Footer/รายชื่อผู้ยินยอม", Table = "web_privacy2_submit", OrderBy = "id", Sort = "desc",
                        CanAdd = false,CanEdit = false,CanDelete = true,CanMove = false,CanStatus = false,CanApprove = false, UseViewCreateFrom = "AnnounceSubmit",UseViewEditFrom = "AnnounceSubmit",FieldSearch = FieldSearch_DefaultCat,
                        CanExport = true,
                        ListData = new()
                        {
                            new ("title", "Browser"),
                            new ("en_title", "IP Address"),
                            new ("created_at", "วันที่สร้าง"),
                        },
                        FieldApprove = new() { "title" }, FieldCreate = new() { "title", "sort" }, FieldUpdate = new() { "title" },
                        EnableViewDetail = true,
                        ViewDetailData = new()
                        {
                            new ("id", "ไอดี"),
                            new ("title", "Browser"),
                            new ("en_title", "IP Address"),
                            new ("created_at", "วันที่สร้าง")
                        },
                        ExportData = new()
                        {
                            new ("id", "id"),
                            new ("title", "Browser"),
                            new ("en_title", "IP Address"),
                            new ("TO_CHAR(created_at, 'DD-MM-YYYY HH24:MI:SS')", "create"),
                        },
                    }
                },
                new Module()
                {
                    Name = "FooterLink",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "เปิดเผยข้อมูลความโปร่งใส", TextBreadcrumb = "เมนู Footer/เปิดเผยข้อมูลความโปร่งใส", Table = "web_footer_link", OrderBy = "sort", Sort = "asc",  TableCate = "web_footer_link", TableCateField = "cat_id", TableCateOrderby = "title", TableCateSort = "asc", TableCateTitle = "title", TableCateLabel = "กลุ่ม",  CanAdd = true, CanEdit = true, CanDelete = true, CanMove = true, CanStatus = true, CanApprove = true, UseViewCreateFrom = "CMSPage", UseViewEditFrom = "CMSPage", IsCMSPage = true, FieldCMSPage = "parent_id",
                        FieldSearch = FieldSearch_Default,
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),new ("page_type", "ประเภท"),new ("pb_status", "สถานะ"),new ("updated_at", "วันที่แก้ไข")
                        },
                        FieldCreate = new()
                        {
                            "cat_id", "page_type", "title", "en_title", "des", "en_des", "url", "en_url", "url_target", "issue_date", "expiry_date", "seo_kw", "seo_de", "script_head", "script_body", "box_layout", "box_data", "issue_date_config", "page_type_sub", "page_type_sub_core", "page_type_sub_core_data", "parent_id", "info", "en_info", "page_style", "box_data_sub", "seo_url", "en_seo_url", "sort"
                        },
                        FieldUpdate = new()
                        {
                            "page_type", "title", "en_title", "des", "en_des", "url", "en_url", "url_target", "issue_date", "expiry_date", "seo_kw", "seo_de", "script_head", "script_body", "box_layout", "box_data", "issue_date_config", "page_type_sub", "page_type_sub_core", "page_type_sub_core_data", "info", "en_info", "page_style", "box_data_sub", "seo_url", "en_seo_url"
                        },
                        FieldApprove = new()
                        {
                            "cat_id", "page_type", "title", "en_title", "des", "en_des", "url", "en_url", "url_target", "issue_date", "expiry_date", "seo_kw", "seo_de", "script_head", "script_body", "box_layout", "box_data", "issue_date_config", "page_type_sub", "page_type_sub_core", "page_type_sub_core_data", "parent_id", "info", "en_info", "page_style", "box_data_sub", "seo_url", "en_seo_url"
                        }
                    }
                },
                #endregion

                #region Landing-Microsite
                new Module()
                {
                    Name = "Microsite",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "Microsite",
                        TextBreadcrumb = "Microsite/Microsite",
                        Table = "web_microsite",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        CanMove = false, // ไม่มีการจัดเรียงในเมนูนี้
                        CanStatus = true,
                        CanApprove = true,
                        UseViewCreateFrom = "Microsite",
                        UseViewEditFrom = "Microsite",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "url", "pb_url", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("pb_url", "URL"),
                            new ("pb_status", "สถานะ"),
                            new ("updated_at", "วันที่แก้ไข")
                        },
                        // ลิงก์ไปหน้าจริง {FrontURL}/th/{pb_url}
                        // ต้องใช้ pb_url + เงื่อนไขชุดเดียวกับที่ front-end ค้นหา (HomeController: หา microsite
                        // จาก web_microsite where pb_url = slug and status=1 and pb_status=1 and show_front=1)
                        FrontLinkField = "pb_url",
                        FrontLinkPrefix = "th/",
                        FrontLinkRequire1Fields = new() { "status", "pb_status", "show_front" },
                        // Approve copies column -> pb_column, so every pb_* pair in
                        // web_microsite is listed here even when no form input exposes it yet.
                        // pb_issue_date / pb_expiry_date matter most: AdminHelpers.WebInfo()
                        // gates microsite access on now() between them.
                        FieldApprove = new()
                        {
                            "cat_id",
                            "this_type",
                            "title",
                            "en_title",
                            "des",
                            "en_des",
                            "info",
                            "en_info",
                            "img1",
                            "file1",
                            "en_file1",
                            "url",
                            "en_url",
                            "url_target",
                            "issue_date",
                            "expiry_date",
                            "param",
                            "en_param"
                        },
                        // FieldCreate / FieldUpdate must list only fields the Create/Edit
                        // forms actually post: setFieldsUpdate() writes DBNull for any
                        // listed field missing from the form, which would wipe the column.
                        FieldCreate = new()
                        {
                            "title",
                            "url",
                            "issue_date",
                            "expiry_date",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title",
                            "url",
                            "issue_date",
                            "expiry_date"
                        }
                    }
                },
                new Module()
                {
                    Name = "MicrositeForm",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "Lead Form",
                        TextBreadcrumb = "Microsite/Lead Form",
                        Table = "web_microsite_form",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        CanMove = true,
                        CanStatus = true,
                        CanApprove = true,
                        UseViewCreateFrom = "MicrositeForm",
                        UseViewEditFrom = "MicrositeForm",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("pb_status", "สถานะ"),
                            new ("created_at", "วันที่สร้าง"),
                            new ("updated_at", "วันที่แก้ไข")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        }
                    }
                },
                new Module()
                {
                    Name = "MicrositeSubmit",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "รายชื่อผู้กรอกข้อมูล",
                        TextBreadcrumb = "Microsite/รายชื่อผู้กรอกข้อมูล",
                        Table = "web_microsite_submit",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        CanMove = true,
                        CanStatus = true,
                        CanApprove = true,
                        UseViewCreateFrom = "MicrositeSubmit",
                        UseViewEditFrom = "MicrositeSubmit",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("pb_status", "สถานะ"),
                            new ("created_at", "วันที่สร้าง"),
                            new ("updated_at", "วันที่แก้ไข")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        }
                    }
                }, 
                #endregion

                #region Subscription
                new Module()
                {
                    Name = "Subscription",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "รายชื่อผู้ลงทะเบียน",
                        TextBreadcrumb = "Subscription/รายชื่อผู้ลงทะเบียน",
                        Table = "web_subscription",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = false,
                        CanEdit = false,
                        CanDelete = true,
                        CanMove = false,
                        CanStatus = true,
                        CanApprove = false,
                        UseViewCreateFrom = "Subscription",
                        UseViewEditFrom = "Subscription",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("email", "E-mail"),
                            new ("created_at", "วันที่สร้าง"),
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        }
                    }
                },
                new Module()
                {
                    Name = "SubscriptionEmail",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ส่งข่าวสารทางอีเมล",
                        TextBreadcrumb = "Subscription/ส่งข่าวสารทางอีเมล",
                        Table = "web_subscription",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        CanMove = false,
                        CanStatus = true,
                        CanApprove = true,
                        UseViewCreateFrom = "SubscriptionEmail",
                        UseViewEditFrom = "SubscriptionEmail",
                        UseViewListFrom = "SubscriptionEmail",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("pb_status", "สถานะ"),
                            new ("updated_at", "วันที่แก้ไข")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        }
                    }
                },
                new Module()
                {
                    Name = "SubscriptionNews",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ข่าวสารทางอีเมลย้อนหลัง",
                        TextBreadcrumb = "Subscription/ข่าวสารทางอีเมลย้อนหลัง",
                        Table = "web_subscription_news",
                        OrderBy = "id",
                        Sort = "desc",
                        CanAdd = false, CanEdit = false, CanDelete = true, CanMove = false, CanStatus = false, CanApprove = false,
                        FieldSearch = new()
                        {
                            new ("text", new() { "subject", "created_by" })
                        },
                        ListData = new()
                        {
                            new ("subject", "หัวเรื่อง"),
                            new ("recipient_type", "ผู้รับ"),
                            new ("total_count", "จำนวนผู้รับ"),
                            new ("success_count", "สำเร็จ"),
                            new ("created_at", "วันที่ส่ง"),
                        },
                        EnableViewDetail = true,
                        ViewDetailData = new()
                        {
                            new ("id", "ไอดี"),
                            new ("created_at", "วันที่ส่ง"),
                            new ("created_by", "ส่งโดย"),
                            new ("recipient_type", "ประเภทผู้รับ"),
                            new ("total_count", "จำนวนผู้รับทั้งหมด"),
                            new ("success_count", "ส่งสำเร็จ"),
                            new ("fail_count", "ส่งไม่สำเร็จ"),
                            new ("subject", "หัวเรื่อง (Subject)"),
                            new ("body", "เนื้อหาอีเมล (Message Body)"),
                            new ("file1", "ไฟล์แนบ 1"),
                            new ("file2", "ไฟล์แนบ 2"),
                            new ("file3", "ไฟล์แนบ 3"),
                        },
                    }
                },
                #endregion

                #region Tracking Code
                new Module()
                {
                    Name = "GoogleAnalytics",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "Google Analytics",
                        TextBreadcrumb = "Tracking Code/Google Analytics",
                        Table = "web_google_analytics",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        CanMove = false,
                        CanStatus = true,
                        CanApprove = true,
                        UseViewCreateFrom = "GoogleAnalytics",
                        UseViewEditFrom = "GoogleAnalytics",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("pb_status", "สถานะ"),
                            new ("updated_at", "วันที่แก้ไข")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        }
                    }
                },
                new Module()
                {
                    Name = "GoogleAds",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "Google Ads",
                        TextBreadcrumb = "Tracking Code/Google Ads",
                        Table = "web_google_ads",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        CanMove = true,
                        CanStatus = true,
                        CanApprove = true,
                        UseViewCreateFrom = "GoogleAds",
                        UseViewEditFrom = "GoogleAds",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("pb_status", "สถานะ"),
                            new ("created_at", "วันที่สร้าง"),
                            new ("updated_at", "วันที่แก้ไข")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        }
                    }
                },
                new Module()
                {
                    Name = "FacebookPixel",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "Facebook Pixel",
                        TextBreadcrumb = "Tracking Code/Facebook Pixel",
                        Table = "web_facebook_pixel",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        CanMove = true,
                        CanStatus = true,
                        CanApprove = true,
                        UseViewCreateFrom = "FacebookPixel",
                        UseViewEditFrom = "FacebookPixel",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("pb_status", "สถานะ"),
                            new ("created_at", "วันที่สร้าง"),
                            new ("updated_at", "วันที่แก้ไข")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        }
                    }
                }, 
                #endregion

                #region Monitor
                new Module()
                {
                    Name = "ServerStatus",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "Server Health",
                        TextBreadcrumb = "Monitor/Server Health",
                        Table = "web_admin",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        CanMove = false,
                        CanStatus = true,
                        CanApprove = true,
                        UseViewCreateFrom = "ServerStatus",
                        UseViewEditFrom = "ServerStatus",
                        UseViewListFrom = "ServerStatus",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("pb_status", "สถานะ"),
                            new ("updated_at", "วันที่แก้ไข")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        }
                    }
                }, 
                new Module()
                {
                    Name = "ErrorLogs",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "Error Logs",
                        TextBreadcrumb = "Monitor/Error Logs",
                        Table = "web_admin",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        CanMove = false,
                        CanStatus = true,
                        CanApprove = true,
                        UseViewCreateFrom = "ErrorLogs",
                        UseViewEditFrom = "ErrorLogs",
                        UseViewListFrom = "ErrorLogs",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("pb_status", "สถานะ"),
                            new ("updated_at", "วันที่แก้ไข")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        }
                    }
                },
                new Module()
                {
                    Name = "GoogleAnalyticsLogs",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "Google Analytics",
                        TextBreadcrumb = "Monitor/Google Analytics",
                        Table = "web_google_analytics_logs",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        CanMove = true,
                        CanStatus = true,
                        CanApprove = true,
                        UseViewCreateFrom = "GoogleAnalyticsLogs",
                        UseViewEditFrom = "GoogleAnalyticsLogs",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("pb_status", "สถานะ"),
                            new ("created_at", "วันที่สร้าง"),
                            new ("updated_at", "วันที่แก้ไข")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        }
                    }
                }, 
                #endregion

                #region Setting
                new Module()
                {
                    Name = "FileManager",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "File Manager",
                        TextBreadcrumb = "Setting/File Manager",
                        Table = "web_file_manager",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        CanMove = false,
                        CanStatus = true,
                        CanApprove = true,
                        UseViewCreateFrom = "FileManager",
                        UseViewEditFrom = "FileManager",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("pb_status", "สถานะ"),
                            new ("updated_at", "วันที่แก้ไข")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        }
                    }
                },
                new Module()
                {
                    Name = "GoogleKey",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "Google Map Key",
                        TextBreadcrumb = "Setting/Google Map Key",
                        Table = "web_core_single",TableModuleID = 33,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "GoogleKey",UseViewEditFrom = "GoogleKey",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "AISetting",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "AI Settings",
                        TextBreadcrumb = "Setting/AI Settings",
                        Table = "web_core_single",TableModuleID = 34,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "AISetting",UseViewEditFrom = "AISetting",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                },
                new Module()
                {
                    Name = "LDAPSetting",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "LDAP Azure AD",
                        TextBreadcrumb = "Setting/LDAP Azure AD",
                        Table = "web_core_single",TableModuleID = 35,OrderBy = "sort",Sort = "asc",
                        CanAdd = false,CanEdit = true,CanDelete = false,CanMove = false,CanStatus = false,CanApprove = true,
                        UseViewCreateFrom = "LDAPSetting",UseViewEditFrom = "LDAPSetting",
                        FieldSearch = FieldSearch_Default,
                        ListData = ListData_Default,
                        FieldCreate = new(){ },
                        FieldUpdate = FieldUpdate_CoreSingle,
                        FieldApprove = FieldApprove_CoreSingle
                    }
                }, 
                #endregion

                #region Logs
                new Module()
                {
                    Name = "LogsMember",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "Activities สมาชิก",
                        TextBreadcrumb = "Logs/Activities สมาชิก",
                        Table = "web_member",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = false,
                        CanEdit = false,
                        CanDelete = false,
                        CanMove = false,
                        CanStatus = false,
                        CanApprove = false,
                        UseViewListFrom = "LogsMember",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        }
                    }
                },
                new Module()
                {
                    Name = "LogsPDPA",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ให้ความยินยอม PDPA",
                        TextBreadcrumb = "Logs/ให้ความยินยอม PDPA",
                        Table = "web_member",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = false,
                        CanEdit = false,
                        CanDelete = false,
                        CanMove = false,
                        CanStatus = false,
                        CanApprove = false,
                        UseViewListFrom = "LogsPDPA",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        }
                    }
                },
                new Module()
                {
                    Name = "LogsAI",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "AI Services API",
                        TextBreadcrumb = "Logs/AI Services API",
                        Table = "web_member",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = false,
                        CanEdit = false,
                        CanDelete = false,
                        CanMove = false,
                        CanStatus = false,
                        CanApprove = false,
                        UseViewListFrom = "LogsAI",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        }
                    }
                },
                new Module()
                {
                    Name = "LogsCRM",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "CRM Data API",
                        TextBreadcrumb = "Logs/CRM Data API",
                        Table = "web_member",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = false,
                        CanEdit = false,
                        CanDelete = false,
                        CanMove = false,
                        CanStatus = false,
                        CanApprove = false,
                        UseViewListFrom = "LogsCRM",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        }
                    }
                },
                new Module()
                {
                    Name = "LogsLINE",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "LINE LIFF Data API",
                        TextBreadcrumb = "Logs/LINE LIFF Data API",
                        Table = "api_audit_log",
                        OrderBy = "log_date",
                        Sort = "desc",
                        CanAdd = false,
                        CanEdit = false,
                        CanDelete = false,
                        CanMove = false,
                        CanStatus = false,
                        CanApprove = false,
                        UseViewListFrom = "LogsLINE",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        }
                    }
                },new Module()
                {
                    Name = "LogsLDAP",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "AD/LDAP Login API",
                        TextBreadcrumb = "Logs/AD/LDAP Login API",
                        Table = "web_member",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = false,
                        CanEdit = false,
                        CanDelete = false,
                        CanMove = false,
                        CanStatus = false,
                        CanApprove = false,
                        UseViewListFrom = "LogsAI",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        }
                    }
                },
                new Module()
                {
                    Name = "LogsMatchAPI",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "จับคู่ทรัพย์สินอัตโนมัติ  API",
                        TextBreadcrumb = "Logs/จับคู่ทรัพย์สินอัตโนมัติ  API",
                        Table = "web_logs_match",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        CanMove = true,
                        CanStatus = true,
                        CanApprove = true,
                        UseViewCreateFrom = "LogsMatchAPI",
                        UseViewEditFrom = "LogsMatchAPI",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("pb_status", "สถานะ"),
                            new ("created_at", "วันที่สร้าง"),
                            new ("updated_at", "วันที่แก้ไข")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        }
                    }
                },
                new Module()
                {
                    Name = "LogsGoogleMap",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "Google Map API",
                        TextBreadcrumb = "Logs/Google Map API",
                        Table = "web_member",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = false,
                        CanEdit = false,
                        CanDelete = false,
                        CanMove = false,
                        CanStatus = false,
                        CanApprove = false,
                        UseViewListFrom = "LogsGoogleMap",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        }
                    }
                },
                new Module()
                {
                    //----- Logs/ค้นหาทรัพย์ NPA — หน้า list เขียน query เองทั้งหมดใน View (ตาราง api_npa_search_log)
                    //      Table ที่ตั้งไว้เป็นเพียงตารางหลอกให้ AdminCoreController.Index() ทำงานผ่าน (เหมือน LogsGoogleMap)
                    Name = "LogsNpaSearch",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "ค้นหาทรัพย์ NPA",
                        TextBreadcrumb = "Logs/ค้นหาทรัพย์ NPA",
                        Table = "web_member",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = false,
                        CanEdit = false,
                        CanDelete = false,
                        CanMove = false,
                        CanStatus = false,
                        CanApprove = false,
                        UseViewListFrom = "LogsNpaSearch",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ")
                        },
                        FieldApprove = new()
                        {
                            "title"
                        },
                        FieldCreate = new()
                        {
                            "title"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        }
                    }
                },
                #endregion

                #region Admin
                new Module()
                {
                    Name = "AdminAccess",
                    Config = new()
                    {
                        Text = "สิทธิ์การใช้",
                        TextBreadcrumb = "ผู้ดูแลระบบ/สิทธิ์การใช้",
                        Table = "web_admin_access",
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        //CanMove = false,
                        UseViewCreateFrom = "AdminAccess",
                        UseViewEditFrom = "AdminAccess",
                        UseViewListFrom = "AdminAccess",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title" })
                        },
                        OrderBy = "created_at",
                        Sort = "asc",
                        PerPage = 10,
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("updated_at", "วันที่แก้ไข"),
                            new ("created_by", "สร้างโดย"),
                            new ("updated_by", "แก้ไขโดย"),
                        },
                        ExportData = new(){},
                        FieldApprove = new(){},
                        FieldCreate = new()
                        {
                            "title",
                            "sort",
                            "created_at",
                            "updated_at",
                            "created_by",
                            "updated_by"
                        },
                        FieldUpdate = new()
                        {
                            "title",
                            "updated_at",
                            "updated_by"
                        }
                    }
                },
                new Module()
                {
                    Name = "AdminUser",
                    Config = new()
                    {
                        Text = "ผู้ดูแลระบบ",
                        TextBreadcrumb = "ผู้ดูแลระบบ/ผู้ดูแลระบบ",
                        Table = "web_admin",
                        OrderBy = "created_at",
                        Sort = "desc",
                        TableCate = "web_admin_access",
                        TableCateField = "access_id",
                        TableCateOrderby = "title",
                        TableCateSort = "asc",
                        TableCateTitle = "title",
                        TableCateLabel = "สิทธิ์การใช้",
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        CanStatus = true,
                        UseViewCreateFrom = "AdminUser",
                        UseViewEditFrom = "AdminUser",
                        FieldSearch = new()
                        {
                            new ("text", new() { "username", "name", "surname", "email", "created_by" }),
                            new ("access_id", new() { "access_id" })
                        },
                        FieldSearchIsEqual = new() { "access_id" },
                        ListData = new()
                        {
                            //----- ประเภทการ Login ไม่ทำเป็นคอลัมน์แยก แต่ต่อท้าย Username ว่า "พนักงาน" (ดู AdminCore/Index.cshtml)
                            new ("username", "Username"),
                            new ("name", "ชื่อ"),
                            new ("surname", "สกุล"),
                            new ("created_at", "วันที่สร้าง"),
                            new ("created_by", "สร้างโดย"),
                        },
                        FieldCreate = new()
                        {
                            "access_id", "username", "password", "name", "surname", "section", "email", "use_otp",
                            "created_at",
                            "updated_at",
                            "created_by",
                            "updated_by",
                            "status",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "access_id", "password", "name", "surname", "section", "email", "use_otp",
                            "updated_at",
                            "updated_by"
                        }
                    }
                },
                new Module()
                {
                    Name = "AdminLog",
                    Config = new()
                    {
                        Text = "Admin log",
                        TextBreadcrumb = "ผู้ดูแลระบบ/Admin log",
                        Table = "web_admin_log",
                        CanExport = true,
                        UseViewDetailFrom = "AdminLog",
                        UseViewListFrom = "AdminLog",
                        EnableDateSearch = true,
                        FieldSearch = new()
                        {
                            new ("text", new() { "action_info", "ip", "action_table" }),
                            new ("action", new() { "action" }),
                            new ("username", new() { "admin_username" }),
                        },
                        FieldSearchIsEqual = new() { /*"action",*/ "admin_username" },
                        OrderBy = "created_at",
                        Sort = "DESC",
                        PerPage = 10,
                        ListData = new()
                        {
                            new ("action", "Action"),
                            new ("action_info", "หัวข้อ"),
                            new ("admin_username", "Username"),
                            new ("ip", "ไอพี"),
                            new ("created_at", "วันที่สร้าง"),
                            new ("created_by", "สร้างโดย"),
                        },
                        ExportData = new()
                        {
                            new ("id", "id"),
                            new ("TO_CHAR(created_at, 'DD-MM-YYYY HH24:MI:SS')", "create"),
                            new ("admin_user_id", "user id"),
                            new ("admin_username", "username"),
                            new ("action", "action"),
                            new ("action_info", "detail"),
                            new ("action_url", "url"),
                            new ("action_table", "table"),
                            new ("ip", "ip"),
                            new ("old_value", "old value"),
                            new ("new_value", "new value"),

                        },
                        FieldApprove = new(){},
                        FieldCreate = new()
                        {
                            "title"
                        },
                        FieldUpdate = new()
                        {
                            "title"
                        },
                        EnableViewDetail = true,
                        ViewDetailData = new()
                        {
                            new ("id", "ไอดี"),
                            new ("created_at", "วันที่สร้าง"),
                            new ("admin_user_id", "User ID"),
                            new ("admin_username", "Username"),
                            new ("action", "Action"),
                            new ("action_info", "รายละเอียด"),
                            new ("action_url", "URL"),
                            new ("action_table", "ตาราง"),
                            new ("ip", "IP"),
                            new ("old_value", "old value"),
                            new ("new_value", "new value"),
                        },
                    }
                },
                new Module()
                {
                    Name = "AdminUserNotLogin",
                    Config = new()
                    {
                        Text = "ผู้ใช้ที่ไม่เข้าใช้งาน",
                        TextBreadcrumb = "ผู้ดูแลระบบ/ผู้ใช้ที่ไม่เข้าใช้งาน",
                        Table = "web_admin",
                        OrderBy = "name",
                        Sort = "desc",
                        UseViewListFrom = "AdminUserNotLogin",
                        FieldSearch = new() { },
                        EnableDateSearch = true,
                        ListData = new()
                        {
                            new ("username", "Username"),
                            new ("name", "ชื่อ"),
                            new ("surname", "สกุล"),
                            new ("email", "อีเมล"),
                            new ("created_by", "สร้างโดย"),
                        },
                        FieldCreate = new() { },
                        FieldUpdate = new() { },
                    }
                },
	            #endregion 

                #region Widget / CMS Group 
                new Module()
                {
                    Name = "WidgetGroup",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "กลุ่ม Widget",
                        TextBreadcrumb = "Widget/กลุ่ม Widget",
                        Table = "web_widget_group",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        CanMove = true,
                        CanStatus = true,
                        CanApprove = true,
                        UseViewCreateFrom = "WidgetGroup",
                        UseViewEditFrom = "WidgetGroup",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("img1", "ไอคอน"),
                            new ("pb_status", "สถานะ"),
                            new ("created_at", "วันที่สร้าง"),
                            new ("updated_at", "วันที่แก้ไข")
                        },
                        FieldApprove = new()
                        {
                            "title",
                            "img1"
                        },
                        FieldCreate = new()
                        {
                            "title",
                            "img1",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title",
                            "img1"
                        }
                    }
                },
                new Module()
                {
                    Name = "Widget",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "Widget ทั้งหมด",
                        TextBreadcrumb = "Widget/Widget ทั้งหมด",
                        Table = "web_widget",
                        OrderBy = "sort",
                        Sort = "asc",
                        TableCate = "web_widget_group",
                        TableCateField = "cat_id",
                        TableCateOrderby = "sort",
                        TableCateSort = "asc",
                        TableCateTitle = "title",
                        TableCateLabel = "กลุ่ม",
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        CanMove = true,
                        CanStatus = true,
                        CanApprove = true,
                        UseViewCreateFrom = "Widget",
                        UseViewEditFrom = "Widget",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" }),
                            new ("cat_id", new() { "cat_id" })
                        },
                        FieldSearchIsEqual = new() { "cat_id" },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("img1", "ไอคอน"),
                            new ("pb_status", "สถานะ"),
                            new ("created_at", "วันที่สร้าง"),
                            new ("updated_at", "วันที่แก้ไข")
                        },
                        FieldApprove = new()
                        {
                            "cat_id",
                            "title",
                            "img1",
                            "mod_name",
                            "info",
                        },
                        FieldCreate = new()
                        {
                            "cat_id",
                            "title",
                            "img1",
                            "mod_name",
                            "info",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "cat_id",
                            "title",
                            "img1",
                            "mod_name",
                            "info",
                        }
                    }
                },
                #endregion

                #region Widget2 / CMS Group
                new Module()
                {
                    Name = "WidgetGroup2",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "กลุ่ม Widget2",
                        TextBreadcrumb = "Widget2/กลุ่ม Widget2",
                        Table = "web_widget_group2",
                        OrderBy = "sort",
                        Sort = "asc",
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        CanMove = true,
                        CanStatus = true,
                        CanApprove = true,
                        UseViewCreateFrom = "WidgetGroup",
                        UseViewEditFrom = "WidgetGroup",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" })
                        },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("img1", "ไอคอน"),
                            new ("pb_status", "สถานะ"),
                            new ("created_at", "วันที่สร้าง"),
                            new ("updated_at", "วันที่แก้ไข")
                        },
                        FieldApprove = new()
                        {
                            "title",
                            "img1"
                        },
                        FieldCreate = new()
                        {
                            "title",
                            "img1",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "title",
                            "img1"
                        }
                    }
                },
                new Module()
                {
                    Name = "Widget2",
                    Config = new Module.ModuleConfig()
                    {
                        Text = "Widget2 ทั้งหมด",
                        TextBreadcrumb = "Widget2/Widget2 ทั้งหมด",
                        Table = "web_widget2",
                        OrderBy = "sort",
                        Sort = "asc",
                        TableCate = "web_widget_group2",
                        TableCateField = "cat_id",
                        TableCateOrderby = "sort",
                        TableCateSort = "asc",
                        TableCateTitle = "title",
                        TableCateLabel = "กลุ่ม",
                        CanAdd = true,
                        CanEdit = true,
                        CanDelete = true,
                        CanMove = true,
                        CanStatus = true,
                        CanApprove = true,
                        UseViewCreateFrom = "Widget",
                        UseViewEditFrom = "Widget",
                        FieldSearch = new()
                        {
                            new ("text", new() { "title", "updated_by" }),
                            new ("cat_id", new() { "cat_id" })
                        },
                        FieldSearchIsEqual = new() { "cat_id" },
                        ListData = new()
                        {
                            new ("title", "หัวข้อ"),
                            new ("img1", "ไอคอน"),
                            new ("pb_status", "สถานะ"),
                            new ("created_at", "วันที่สร้าง"),
                            new ("updated_at", "วันที่แก้ไข")
                        },
                        FieldApprove = new()
                        {
                            "cat_id",
                            "title",
                            "img1",
                            "mod_name",
                            "info",
                        },
                        FieldCreate = new()
                        {
                            "cat_id",
                            "title",
                            "img1",
                            "mod_name",
                            "info",
                            "sort"
                        },
                        FieldUpdate = new()
                        {
                            "cat_id",
                            "title",
                            "img1",
                            "mod_name",
                            "info",
                        }
                    }
                },
                #endregion
            };
        }
    }
}
 