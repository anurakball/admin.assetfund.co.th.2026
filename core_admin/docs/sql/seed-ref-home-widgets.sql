-- อ้างอิงเท่านั้น (สร้างจาก scratchpad/gen_seed.py 18 ก.ย. 2569): ข้อมูล widget 3 กลุ่ม x 6 section ของ Asset Plus + box_layout ของหน้าแรก — บน dev รันแล้ว ห้ามรันซ้ำ (จะได้แถวซ้ำ)
-- Asset Plus home page widgets (18 ก.ย. 2569 · ชื่อ+ไอคอนปรับ 17 ก.ย. 2569: ตัด (Version n)/(Key) — GROUP ออก, ไอคอนเป็น PNG glyph สร้างด้วย ComfyUI แทน screenshot) — 3 groups (DEFAULT/MODERN/CLASSIC = Version 1/2/3 ของ /salepage) x 6 sections
-- info/pb_info = HTML ของ <section> จากหน้า /salepage (ตัวอย่างในหลังบ้าน) · hero ใช้ |||REPEAT||| กับ HomeImageSlide
-- section_key = ชื่อ partial ฝั่ง front-end (Views/Home/Partials/_<key>.cshtml) — front-end อ่านคอลัมน์นี้เพื่อเรนเดอร์
SET XACT_ABORT ON;
BEGIN TRAN;
DECLARE @g TABLE (n int, id bigint);
DECLARE @gid bigint;
INSERT INTO [2026_web_widget_group] (created_at, updated_at, created_by, updated_by, sort, status, pb_status, approve_by, show_front, title, img1, pb_title, pb_img1, web_id) VALUES (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'user', N'user', 10, 1, 1, N'user', 1, N'DEFAULT', N'Files/Site0/1/widget_icons/assetplus/icon-group-default.png', N'DEFAULT', N'Files/Site0/1/widget_icons/assetplus/icon-group-default.png', 0);
SET @gid = SCOPE_IDENTITY(); INSERT INTO @g (n, id) VALUES (1, @gid);
INSERT INTO [2026_web_widget] (created_at, updated_at, created_by, updated_by, sort, status, pb_status, approve_by, show_front, cat_id, title, img1, mod_name, info, section_key, pb_cat_id, pb_title, pb_img1, pb_mod_name, pb_info, pb_section_key, web_id) VALUES (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'user', N'user', 10, 1, 1, N'user', 1, @gid, N'แบนเนอร์หน้าแรก', N'Files/Site0/1/widget_icons/assetplus/icon-Hero.png', N'HomeImageSlide', N'<section class="section hero" aria-label="แบนเนอร์ไฮไลต์">
<div class="container-fluid">
<div class="swiper hero__slider js-hero-slider" id="heroSlider" data-initial-slide="0">
<div class="swiper-wrapper">
|||REPEAT|||<div class="swiper-slide hero__slide" data-slide-id="|||id|||"><a href="|||pb_url|||" class="hero__link" target="_top"><picture><source media="(max-width: 767.98px)" srcset="/|||pb_img1_icon|||" /><img src="/|||pb_img1|||" alt="|||pb_title|||" class="hero__image" loading="lazy" decoding="async" /></picture></a></div>|||/REPEAT|||
</div>
<div class="hero__controls">
<div class="hero__pagination slider-dots js-hero-pagination" id="heroPagination"></div>
<div class="hero__progress" id="heroProgress">
<svg class="hero__progress-ring" viewBox="0 0 40 40" aria-hidden="true">
<circle class="hero__progress-track" cx="20" cy="20" r="18" />
<circle class="hero__progress-bar js-hero-progress-bar" cx="20" cy="20" r="18" />
</svg>
<button type="button" class="hero__toggle js-hero-toggle" id="heroToggle" aria-pressed="false" aria-label="หยุดสไลด์ชั่วคราว"><i class="bi bi-play-fill" aria-hidden="true"></i></button>
</div>
</div>
</div>
</div>
</section>', N'Hero', CAST(@gid AS nvarchar(20)), N'แบนเนอร์หน้าแรก', N'Files/Site0/1/widget_icons/assetplus/icon-Hero.png', N'HomeImageSlide', N'<section class="section hero" aria-label="แบนเนอร์ไฮไลต์">
<div class="container-fluid">
<div class="swiper hero__slider js-hero-slider" id="heroSlider" data-initial-slide="0">
<div class="swiper-wrapper">
|||REPEAT|||<div class="swiper-slide hero__slide" data-slide-id="|||id|||"><a href="|||pb_url|||" class="hero__link" target="_top"><picture><source media="(max-width: 767.98px)" srcset="/|||pb_img1_icon|||" /><img src="/|||pb_img1|||" alt="|||pb_title|||" class="hero__image" loading="lazy" decoding="async" /></picture></a></div>|||/REPEAT|||
</div>
<div class="hero__controls">
<div class="hero__pagination slider-dots js-hero-pagination" id="heroPagination"></div>
<div class="hero__progress" id="heroProgress">
<svg class="hero__progress-ring" viewBox="0 0 40 40" aria-hidden="true">
<circle class="hero__progress-track" cx="20" cy="20" r="18" />
<circle class="hero__progress-bar js-hero-progress-bar" cx="20" cy="20" r="18" />
</svg>
<button type="button" class="hero__toggle js-hero-toggle" id="heroToggle" aria-pressed="false" aria-label="หยุดสไลด์ชั่วคราว"><i class="bi bi-play-fill" aria-hidden="true"></i></button>
</div>
</div>
</div>
</div>
</section>', N'Hero', 0);
INSERT INTO [2026_web_widget] (created_at, updated_at, created_by, updated_by, sort, status, pb_status, approve_by, show_front, cat_id, title, img1, mod_name, info, section_key, pb_cat_id, pb_title, pb_img1, pb_mod_name, pb_info, pb_section_key, web_id) VALUES (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'user', N'user', 20, 1, 1, N'user', 1, @gid, N'มูลค่าหน่วยลงทุน', N'Files/Site0/1/widget_icons/assetplus/icon-NavPrices.png', N'', N'<section class="section nav-prices">
    <div class="container">
        <div class="row g-4">
            <!-- ตาราง NAV ย่อ — 5 กองทุนล่าสุด -->
            <div class="col-12 col-lg-6">
                <div class="nav-prices__card">
                    <div class="section-head">
                        <h2 class="section-head__title">มูลค่าหน่วยลงทุน</h2>
                        <a href="/funds/nav" class="btn btn-split btn-light" aria-label="ดูมูลค่าหน่วยลงทุนทั้งหมด">
                            <span>
                                ดูทั้งหมด
                            </span>
                            <span class="btn-split__icon">
                                <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                            </span>
                        </a>
                    </div>

                    <div class="nav-prices__table-wrap">
                        <table class="nav-table js-row-links">
                            <caption class="visually-hidden">
                                มูลค่าหน่วยลงทุนของกองทุนแนะนำ ข้อมูล ณ วันที่ 16 &#xE01;&#xE31;&#xE19;&#xE22;&#xE32;&#xE22;&#xE19; 2569
                            </caption>
                            <thead>
                                <tr>
                                    <th scope="col">กองทุน</th>
                                    <th scope="col">NAV (บาท)</th>
                                    <th scope="col" class="text-end">เปลี่ยนแปลง</th>
                                </tr>
                            </thead>
                            <tbody>
                                    <tr class="nav-table__row js-row-link" data-href="/funds/a-humanoid">
                                        <th scope="row" class="nav-table__code">
                                            <a href="/funds/a-humanoid" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2E;&#xE34;&#xE27;&#xE41;&#xE21;&#xE19;&#xE19;&#xE2D;&#xE22;&#xE14;&#xE4C;">A-HUMANOID</a>
                                        </th>
                                        <td class="nav-table__nav">11.5594</td>
                                        <td class="nav-table__change is-up">
                                            <i class="bi bi-caret-up-fill" aria-hidden="true"></i>
                                            <span>&#x2B;1.18%</span>
                                        </td>
                                    </tr>
                                    <tr class="nav-table__row js-row-link" data-href="/funds/a-grid">
                                        <th scope="row" class="nav-table__code">
                                            <a href="/funds/a-grid" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE01;&#xE23;&#xE34;&#xE14; &#xE2D;&#xE34;&#xE19;&#xE1F;&#xE23;&#xE32;&#xE2A;&#xE15;&#xE23;&#xE31;&#xE04;&#xE40;&#xE08;&#xE2D;&#xE23;&#xE4C;">A-GRID</a>
                                        </th>
                                        <td class="nav-table__nav">12.4180</td>
                                        <td class="nav-table__change is-up">
                                            <i class="bi bi-caret-up-fill" aria-hidden="true"></i>
                                            <span>&#x2B;0.50%</span>
                                        </td>
                                    </tr>
                                    <tr class="nav-table__row js-row-link" data-href="/funds/a-asemi">
                                        <th scope="row" class="nav-table__code">
                                            <a href="/funds/a-asemi" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE40;&#xE2D;&#xE40;&#xE0A;&#xE35;&#xE22; &#xE40;&#xE0B;&#xE21;&#xE34;&#xE04;&#xE2D;&#xE19;&#xE14;&#xE31;&#xE01;&#xE40;&#xE15;&#xE2D;&#xE23;&#xE4C;">A-ASEMI</a>
                                        </th>
                                        <td class="nav-table__nav">14.2075</td>
                                        <td class="nav-table__change is-up">
                                            <i class="bi bi-caret-up-fill" aria-hidden="true"></i>
                                            <span>&#x2B;1.38%</span>
                                        </td>
                                    </tr>
                                    <tr class="nav-table__row js-row-link" data-href="/funds/a-jedi">
                                        <th scope="row" class="nav-table__code">
                                            <a href="/funds/a-jedi" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2A;&#xE40;&#xE1B;&#xE0B; &#xE2D;&#xE35;&#xE42;&#xE04;&#xE42;&#xE19;&#xE21;&#xE35;">A-JEDI</a>
                                        </th>
                                        <td class="nav-table__nav">9.8742</td>
                                        <td class="nav-table__change is-down">
                                            <i class="bi bi-caret-down-fill" aria-hidden="true"></i>
                                            <span>-0.46%</span>
                                        </td>
                                    </tr>
                                    <tr class="nav-table__row js-row-link" data-href="/funds/asp-crypto">
                                        <th scope="row" class="nav-table__code">
                                            <a href="/funds/asp-crypto" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE14;&#xE34;&#xE08;&#xE34;&#xE17;&#xE31;&#xE25; &#xE1A;&#xE25;&#xE47;&#xE2D;&#xE01;&#xE40;&#xE0A;&#xE19;">ASP-CRYPTO</a>
                                        </th>
                                        <td class="nav-table__nav">15.8410</td>
                                        <td class="nav-table__change is-up">
                                            <i class="bi bi-caret-up-fill" aria-hidden="true"></i>
                                            <span>&#x2B;1.55%</span>
                                        </td>
                                    </tr>
                            </tbody>
                        </table>
                    </div>

                    <p class="nav-prices__asof">
                        ข้อมูล ณ วันที่ 16 &#xE01;&#xE31;&#xE19;&#xE22;&#xE32;&#xE22;&#xE19; 2569
                        เวลา 18:00 น.
                    </p>
                </div>
            </div>

            <!-- Quick tiles -->
            <div class="col-12 col-lg-6">
                <div class="quick-tiles">
                    <a href="/funds/performance" class="quick-tile quick-tile--feature">
                        <img src="/media/images/home/tiles/performance.jpg" alt="" class="quick-tile__bg" loading="lazy"
                             decoding="async" />
                        <span class="quick-tile__body">
                            <span class="quick-tile__title">ผลการดำเนินงานทั้งหมด</span>
                            <span class="quick-tile__text">เปรียบเทียบผลตอบแทนย้อนหลังของแต่ละกองทุน</span>
                        </span>
                    </a>

                    <a href="/funds/nav" class="quick-tile quick-tile--accent">
                        <img src="/media/images/home/tiles/nav.jpg" alt="" class="quick-tile__bg" loading="lazy"
                             decoding="async" />
                        <span class="quick-tile__body">
                            <span class="quick-tile__title">มูลค่าหน่วยลงทุนทั้งหมด</span>
                            <span class="quick-tile__text">ค้นหามูลค่าหน่วยลงทุน (NAV) ย้อนหลังของทุกกองทุน</span>
                        </span>
                    </a>

                    <a href="/funds/calendar" class="quick-tile quick-tile--muted">
                        <img src="/media/images/home/tiles/calendar.jpg" alt="" class="quick-tile__bg" loading="lazy"
                             decoding="async" />
                        <span class="quick-tile__body">
                            <span class="quick-tile__title">ปฏิทินกองทุน</span>
                            <span class="quick-tile__text">ตารางวันทำการซื้อขายและวันหยุดของแต่ละกองทุน</span>
                        </span>
                    </a>
                </div>
            </div>
        </div>
    </div>
</section>', N'NavPrices', CAST(@gid AS nvarchar(20)), N'มูลค่าหน่วยลงทุน', N'Files/Site0/1/widget_icons/assetplus/icon-NavPrices.png', N'', N'<section class="section nav-prices">
    <div class="container">
        <div class="row g-4">
            <!-- ตาราง NAV ย่อ — 5 กองทุนล่าสุด -->
            <div class="col-12 col-lg-6">
                <div class="nav-prices__card">
                    <div class="section-head">
                        <h2 class="section-head__title">มูลค่าหน่วยลงทุน</h2>
                        <a href="/funds/nav" class="btn btn-split btn-light" aria-label="ดูมูลค่าหน่วยลงทุนทั้งหมด">
                            <span>
                                ดูทั้งหมด
                            </span>
                            <span class="btn-split__icon">
                                <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                            </span>
                        </a>
                    </div>

                    <div class="nav-prices__table-wrap">
                        <table class="nav-table js-row-links">
                            <caption class="visually-hidden">
                                มูลค่าหน่วยลงทุนของกองทุนแนะนำ ข้อมูล ณ วันที่ 16 &#xE01;&#xE31;&#xE19;&#xE22;&#xE32;&#xE22;&#xE19; 2569
                            </caption>
                            <thead>
                                <tr>
                                    <th scope="col">กองทุน</th>
                                    <th scope="col">NAV (บาท)</th>
                                    <th scope="col" class="text-end">เปลี่ยนแปลง</th>
                                </tr>
                            </thead>
                            <tbody>
                                    <tr class="nav-table__row js-row-link" data-href="/funds/a-humanoid">
                                        <th scope="row" class="nav-table__code">
                                            <a href="/funds/a-humanoid" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2E;&#xE34;&#xE27;&#xE41;&#xE21;&#xE19;&#xE19;&#xE2D;&#xE22;&#xE14;&#xE4C;">A-HUMANOID</a>
                                        </th>
                                        <td class="nav-table__nav">11.5594</td>
                                        <td class="nav-table__change is-up">
                                            <i class="bi bi-caret-up-fill" aria-hidden="true"></i>
                                            <span>&#x2B;1.18%</span>
                                        </td>
                                    </tr>
                                    <tr class="nav-table__row js-row-link" data-href="/funds/a-grid">
                                        <th scope="row" class="nav-table__code">
                                            <a href="/funds/a-grid" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE01;&#xE23;&#xE34;&#xE14; &#xE2D;&#xE34;&#xE19;&#xE1F;&#xE23;&#xE32;&#xE2A;&#xE15;&#xE23;&#xE31;&#xE04;&#xE40;&#xE08;&#xE2D;&#xE23;&#xE4C;">A-GRID</a>
                                        </th>
                                        <td class="nav-table__nav">12.4180</td>
                                        <td class="nav-table__change is-up">
                                            <i class="bi bi-caret-up-fill" aria-hidden="true"></i>
                                            <span>&#x2B;0.50%</span>
                                        </td>
                                    </tr>
                                    <tr class="nav-table__row js-row-link" data-href="/funds/a-asemi">
                                        <th scope="row" class="nav-table__code">
                                            <a href="/funds/a-asemi" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE40;&#xE2D;&#xE40;&#xE0A;&#xE35;&#xE22; &#xE40;&#xE0B;&#xE21;&#xE34;&#xE04;&#xE2D;&#xE19;&#xE14;&#xE31;&#xE01;&#xE40;&#xE15;&#xE2D;&#xE23;&#xE4C;">A-ASEMI</a>
                                        </th>
                                        <td class="nav-table__nav">14.2075</td>
                                        <td class="nav-table__change is-up">
                                            <i class="bi bi-caret-up-fill" aria-hidden="true"></i>
                                            <span>&#x2B;1.38%</span>
                                        </td>
                                    </tr>
                                    <tr class="nav-table__row js-row-link" data-href="/funds/a-jedi">
                                        <th scope="row" class="nav-table__code">
                                            <a href="/funds/a-jedi" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2A;&#xE40;&#xE1B;&#xE0B; &#xE2D;&#xE35;&#xE42;&#xE04;&#xE42;&#xE19;&#xE21;&#xE35;">A-JEDI</a>
                                        </th>
                                        <td class="nav-table__nav">9.8742</td>
                                        <td class="nav-table__change is-down">
                                            <i class="bi bi-caret-down-fill" aria-hidden="true"></i>
                                            <span>-0.46%</span>
                                        </td>
                                    </tr>
                                    <tr class="nav-table__row js-row-link" data-href="/funds/asp-crypto">
                                        <th scope="row" class="nav-table__code">
                                            <a href="/funds/asp-crypto" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE14;&#xE34;&#xE08;&#xE34;&#xE17;&#xE31;&#xE25; &#xE1A;&#xE25;&#xE47;&#xE2D;&#xE01;&#xE40;&#xE0A;&#xE19;">ASP-CRYPTO</a>
                                        </th>
                                        <td class="nav-table__nav">15.8410</td>
                                        <td class="nav-table__change is-up">
                                            <i class="bi bi-caret-up-fill" aria-hidden="true"></i>
                                            <span>&#x2B;1.55%</span>
                                        </td>
                                    </tr>
                            </tbody>
                        </table>
                    </div>

                    <p class="nav-prices__asof">
                        ข้อมูล ณ วันที่ 16 &#xE01;&#xE31;&#xE19;&#xE22;&#xE32;&#xE22;&#xE19; 2569
                        เวลา 18:00 น.
                    </p>
                </div>
            </div>

            <!-- Quick tiles -->
            <div class="col-12 col-lg-6">
                <div class="quick-tiles">
                    <a href="/funds/performance" class="quick-tile quick-tile--feature">
                        <img src="/media/images/home/tiles/performance.jpg" alt="" class="quick-tile__bg" loading="lazy"
                             decoding="async" />
                        <span class="quick-tile__body">
                            <span class="quick-tile__title">ผลการดำเนินงานทั้งหมด</span>
                            <span class="quick-tile__text">เปรียบเทียบผลตอบแทนย้อนหลังของแต่ละกองทุน</span>
                        </span>
                    </a>

                    <a href="/funds/nav" class="quick-tile quick-tile--accent">
                        <img src="/media/images/home/tiles/nav.jpg" alt="" class="quick-tile__bg" loading="lazy"
                             decoding="async" />
                        <span class="quick-tile__body">
                            <span class="quick-tile__title">มูลค่าหน่วยลงทุนทั้งหมด</span>
                            <span class="quick-tile__text">ค้นหามูลค่าหน่วยลงทุน (NAV) ย้อนหลังของทุกกองทุน</span>
                        </span>
                    </a>

                    <a href="/funds/calendar" class="quick-tile quick-tile--muted">
                        <img src="/media/images/home/tiles/calendar.jpg" alt="" class="quick-tile__bg" loading="lazy"
                             decoding="async" />
                        <span class="quick-tile__body">
                            <span class="quick-tile__title">ปฏิทินกองทุน</span>
                            <span class="quick-tile__text">ตารางวันทำการซื้อขายและวันหยุดของแต่ละกองทุน</span>
                        </span>
                    </a>
                </div>
            </div>
        </div>
    </div>
</section>', N'NavPrices', 0);
INSERT INTO [2026_web_widget] (created_at, updated_at, created_by, updated_by, sort, status, pb_status, approve_by, show_front, cat_id, title, img1, mod_name, info, section_key, pb_cat_id, pb_title, pb_img1, pb_mod_name, pb_info, pb_section_key, web_id) VALUES (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'user', N'user', 30, 1, 1, N'user', 1, @gid, N'กองทุนแนะนำประจำเดือน', N'Files/Site0/1/widget_icons/assetplus/icon-FeaturedFunds.png', N'', N'<section class="section featured-funds">
    <div class="container">
        <div class="section-head">
            <h2 class="section-head__title">กองทุนแนะนำประจำเดือน</h2>
            <a href="/funds/featured" class="btn btn-split btn-light" aria-label="ดูกองทุนแนะนำทั้งหมด">
                <span>ดูทั้งหมด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>

        <div class="featured-funds__carousel swiper js-featured-swiper">
            <div class="swiper-wrapper card-deck card-deck--cards-1 card-deck--cards-md-2 card-deck--cards-lg-3">
                    <div class="swiper-slide">
                        
<article class="card card--fund">
    <a href="/funds/a-humanoid" class="card__image" tabindex="-1" aria-hidden="true">
        <img src="/media/images/home/fund/a-humanoid.jpg" alt="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2E;&#xE34;&#xE27;&#xE41;&#xE21;&#xE19;&#xE19;&#xE2D;&#xE22;&#xE14;&#xE4C;" loading="lazy" decoding="async" />
    </a>


    <div class="card__body">
        <a href="/funds/a-humanoid" class="card__heading" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2E;&#xE34;&#xE27;&#xE41;&#xE21;&#xE19;&#xE19;&#xE2D;&#xE22;&#xE14;&#xE4C;">
            <h3 class="card__title card__code">A-HUMANOID</h3>
            <span class="card__meta card__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;/&#xE2B;&#xE38;&#xE49;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
        </a>

        <p class="card__text card__summary">&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE19;&#xE18;&#xE38;&#xE23;&#xE01;&#xE34;&#xE08;&#xE2B;&#xE38;&#xE48;&#xE19;&#xE22;&#xE19;&#xE15;&#xE4C;&#xE2E;&#xE34;&#xE27;&#xE41;&#xE21;&#xE19;&#xE19;&#xE2D;&#xE22;&#xE14;&#xE4C;&#xE41;&#xE25;&#xE30; AI &#xE23;&#xE30;&#xE14;&#xE31;&#xE1A;&#xE42;&#xE25;&#xE01; &#xE17;&#xE35;&#xE48;&#xE01;&#xE33;&#xE25;&#xE31;&#xE07;&#xE40;&#xE1B;&#xE25;&#xE35;&#xE48;&#xE22;&#xE19;&#xE42;&#xE09;&#xE21;&#xE20;&#xE32;&#xE04;&#xE01;&#xE32;&#xE23;&#xE1C;&#xE25;&#xE34;&#xE15;&#xE41;&#xE25;&#xE30;&#xE1A;&#xE23;&#xE34;&#xE01;&#xE32;&#xE23;</p>

            
    <span class="rating">
        <img src="/media/images/morningstar/5-star.png" class="rating__image" alt="Morningstar Rating 5 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

    </div>

    <div class="card__footer">
        <div class="card__actions">
            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--7">7</span>
</span>


            <a href="/funds/a-humanoid" class="btn btn-split" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2E;&#xE34;&#xE27;&#xE41;&#xE21;&#xE19;&#xE19;&#xE2D;&#xE22;&#xE14;&#xE4C;">
                <span>ดูรายละเอียด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>

    </div>
</article>

                    </div>
                    <div class="swiper-slide">
                        
<article class="card card--fund">
    <a href="/funds/a-grid" class="card__image" tabindex="-1" aria-hidden="true">
        <img src="/media/images/home/fund/a-grid.jpg" alt="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE01;&#xE23;&#xE34;&#xE14; &#xE2D;&#xE34;&#xE19;&#xE1F;&#xE23;&#xE32;&#xE2A;&#xE15;&#xE23;&#xE31;&#xE04;&#xE40;&#xE08;&#xE2D;&#xE23;&#xE4C;" loading="lazy" decoding="async" />
    </a>


    <div class="card__body">
        <a href="/funds/a-grid" class="card__heading" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE01;&#xE23;&#xE34;&#xE14; &#xE2D;&#xE34;&#xE19;&#xE1F;&#xE23;&#xE32;&#xE2A;&#xE15;&#xE23;&#xE31;&#xE04;&#xE40;&#xE08;&#xE2D;&#xE23;&#xE4C;">
            <h3 class="card__title card__code">A-GRID</h3>
            <span class="card__meta card__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;/&#xE2B;&#xE38;&#xE49;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
        </a>

        <p class="card__text card__summary">&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE19;&#xE42;&#xE04;&#xE23;&#xE07;&#xE2A;&#xE23;&#xE49;&#xE32;&#xE07;&#xE1E;&#xE37;&#xE49;&#xE19;&#xE10;&#xE32;&#xE19;&#xE14;&#xE49;&#xE32;&#xE19;&#xE1E;&#xE25;&#xE31;&#xE07;&#xE07;&#xE32;&#xE19;&#xE41;&#xE25;&#xE30;&#xE23;&#xE30;&#xE1A;&#xE1A; Smart Grid &#xE17;&#xE35;&#xE48;&#xE40;&#xE1B;&#xE47;&#xE19;&#xE2B;&#xE31;&#xE27;&#xE43;&#xE08;&#xE02;&#xE2D;&#xE07;&#xE42;&#xE25;&#xE01;&#xE2D;&#xE19;&#xE32;&#xE04;&#xE15;</p>

            
    <span class="rating">
        <img src="/media/images/morningstar/4-star.png" class="rating__image" alt="Morningstar Rating 4 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

    </div>

    <div class="card__footer">
        <div class="card__actions">
            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--3">3</span>
</span>


            <a href="/funds/a-grid" class="btn btn-split" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE01;&#xE23;&#xE34;&#xE14; &#xE2D;&#xE34;&#xE19;&#xE1F;&#xE23;&#xE32;&#xE2A;&#xE15;&#xE23;&#xE31;&#xE04;&#xE40;&#xE08;&#xE2D;&#xE23;&#xE4C;">
                <span>ดูรายละเอียด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>

    </div>
</article>

                    </div>
                    <div class="swiper-slide">
                        
<article class="card card--fund">
    <a href="/funds/a-asemi" class="card__image" tabindex="-1" aria-hidden="true">
        <img src="/media/images/home/fund/a-asemi.jpg" alt="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE40;&#xE2D;&#xE40;&#xE0A;&#xE35;&#xE22; &#xE40;&#xE0B;&#xE21;&#xE34;&#xE04;&#xE2D;&#xE19;&#xE14;&#xE31;&#xE01;&#xE40;&#xE15;&#xE2D;&#xE23;&#xE4C;" loading="lazy" decoding="async" />
    </a>


    <div class="card__body">
        <a href="/funds/a-asemi" class="card__heading" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE40;&#xE2D;&#xE40;&#xE0A;&#xE35;&#xE22; &#xE40;&#xE0B;&#xE21;&#xE34;&#xE04;&#xE2D;&#xE19;&#xE14;&#xE31;&#xE01;&#xE40;&#xE15;&#xE2D;&#xE23;&#xE4C;">
            <h3 class="card__title card__code">A-ASEMI</h3>
            <span class="card__meta card__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;/&#xE2B;&#xE38;&#xE49;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
        </a>

        <p class="card__text card__summary">&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE19;&#xE2B;&#xE48;&#xE27;&#xE07;&#xE42;&#xE0B;&#xE48;&#xE01;&#xE32;&#xE23;&#xE1C;&#xE25;&#xE34;&#xE15;&#xE40;&#xE0B;&#xE21;&#xE34;&#xE04;&#xE2D;&#xE19;&#xE14;&#xE31;&#xE01;&#xE40;&#xE15;&#xE2D;&#xE23;&#xE4C;&#xE41;&#xE2B;&#xE48;&#xE07;&#xE40;&#xE2D;&#xE40;&#xE0A;&#xE35;&#xE22; &#xE15;&#xE31;&#xE49;&#xE07;&#xE41;&#xE15;&#xE48;&#xE15;&#xE49;&#xE19;&#xE19;&#xE49;&#xE33;&#xE16;&#xE36;&#xE07;&#xE1B;&#xE25;&#xE32;&#xE22;&#xE19;&#xE49;&#xE33;</p>

            
    <span class="rating">
        <img src="/media/images/morningstar/4-star.png" class="rating__image" alt="Morningstar Rating 4 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

    </div>

    <div class="card__footer">
        <div class="card__actions">
            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--7">7</span>
</span>


            <a href="/funds/a-asemi" class="btn btn-split" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE40;&#xE2D;&#xE40;&#xE0A;&#xE35;&#xE22; &#xE40;&#xE0B;&#xE21;&#xE34;&#xE04;&#xE2D;&#xE19;&#xE14;&#xE31;&#xE01;&#xE40;&#xE15;&#xE2D;&#xE23;&#xE4C;">
                <span>ดูรายละเอียด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>

    </div>
</article>

                    </div>
                    <div class="swiper-slide">
                        
<article class="card card--fund">
    <a href="/funds/a-jedi" class="card__image" tabindex="-1" aria-hidden="true">
        <img src="/media/images/home/fund/a-jedi.jpg" alt="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2A;&#xE40;&#xE1B;&#xE0B; &#xE2D;&#xE35;&#xE42;&#xE04;&#xE42;&#xE19;&#xE21;&#xE35;" loading="lazy" decoding="async" />
    </a>


    <div class="card__body">
        <a href="/funds/a-jedi" class="card__heading" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2A;&#xE40;&#xE1B;&#xE0B; &#xE2D;&#xE35;&#xE42;&#xE04;&#xE42;&#xE19;&#xE21;&#xE35;">
            <h3 class="card__title card__code">A-JEDI</h3>
            <span class="card__meta card__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;/&#xE2B;&#xE38;&#xE49;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
        </a>

        <p class="card__text card__summary">&#xE42;&#xE2D;&#xE01;&#xE32;&#xE2A;&#xE40;&#xE15;&#xE34;&#xE1A;&#xE42;&#xE15;&#xE08;&#xE32;&#xE01;&#xE2D;&#xE38;&#xE15;&#xE2A;&#xE32;&#xE2B;&#xE01;&#xE23;&#xE23;&#xE21;&#xE2D;&#xE27;&#xE01;&#xE32;&#xE28;&#xE41;&#xE25;&#xE30;&#xE14;&#xE32;&#xE27;&#xE40;&#xE17;&#xE35;&#xE22;&#xE21;&#xE17;&#xE35;&#xE48;&#xE02;&#xE22;&#xE32;&#xE22;&#xE15;&#xE31;&#xE27;&#xE15;&#xE48;&#xE2D;&#xE40;&#xE19;&#xE37;&#xE48;&#xE2D;&#xE07;</p>

            
    <span class="rating">
        <img src="/media/images/morningstar/5-star.png" class="rating__image" alt="Morningstar Rating 5 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

    </div>

    <div class="card__footer">
        <div class="card__actions">
            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--7">7</span>
</span>


            <a href="/funds/a-jedi" class="btn btn-split" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2A;&#xE40;&#xE1B;&#xE0B; &#xE2D;&#xE35;&#xE42;&#xE04;&#xE42;&#xE19;&#xE21;&#xE35;">
                <span>ดูรายละเอียด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>

    </div>
</article>

                    </div>
                    <div class="swiper-slide">
                        
<article class="card card--fund">
    <a href="/funds/asp-crypto" class="card__image" tabindex="-1" aria-hidden="true">
        <img src="/media/images/home/fund/asp-crypto.jpg" alt="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE14;&#xE34;&#xE08;&#xE34;&#xE17;&#xE31;&#xE25; &#xE1A;&#xE25;&#xE47;&#xE2D;&#xE01;&#xE40;&#xE0A;&#xE19;" loading="lazy" decoding="async" />
    </a>


    <div class="card__body">
        <a href="/funds/asp-crypto" class="card__heading" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE14;&#xE34;&#xE08;&#xE34;&#xE17;&#xE31;&#xE25; &#xE1A;&#xE25;&#xE47;&#xE2D;&#xE01;&#xE40;&#xE0A;&#xE19;">
            <h3 class="card__title card__code">ASP-CRYPTO</h3>
            <span class="card__meta card__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;/&#xE2B;&#xE38;&#xE49;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
        </a>

        <p class="card__text card__summary">&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE19;&#xE18;&#xE38;&#xE23;&#xE01;&#xE34;&#xE08;&#xE41;&#xE1E;&#xE25;&#xE15;&#xE1F;&#xE2D;&#xE23;&#xE4C;&#xE21;&#xE14;&#xE34;&#xE08;&#xE34;&#xE17;&#xE31;&#xE25; &#xE41;&#xE25;&#xE30;&#xE40;&#xE17;&#xE04;&#xE42;&#xE19;&#xE42;&#xE25;&#xE22;&#xE35;&#xE1A;&#xE25;&#xE47;&#xE2D;&#xE01;&#xE40;&#xE0A;&#xE19;&#xE17;&#xE31;&#xE48;&#xE27;&#xE42;&#xE25;&#xE01;</p>

            
    <span class="rating">
        <img src="/media/images/morningstar/5-star.png" class="rating__image" alt="Morningstar Rating 5 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

    </div>

    <div class="card__footer">
        <div class="card__actions">
            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--8">8&#x2B;</span>
</span>


            <a href="/funds/asp-crypto" class="btn btn-split" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE14;&#xE34;&#xE08;&#xE34;&#xE17;&#xE31;&#xE25; &#xE1A;&#xE25;&#xE47;&#xE2D;&#xE01;&#xE40;&#xE0A;&#xE19;">
                <span>ดูรายละเอียด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>

    </div>
</article>

                    </div>
                    <div class="swiper-slide">
                        
<article class="card card--fund">
    <a href="/funds/a-ring" class="card__image" tabindex="-1" aria-hidden="true">
        <img src="/media/images/home/fund/a-ring.jpg" alt="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE42;&#xE01;&#xE25;&#xE14;&#xE4C;" loading="lazy" decoding="async" />
    </a>


    <div class="card__body">
        <a href="/funds/a-ring" class="card__heading" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE42;&#xE01;&#xE25;&#xE14;&#xE4C;">
            <h3 class="card__title card__code">A-RING</h3>
            <span class="card__meta card__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C;&#xE2A;&#xE34;&#xE19;&#xE17;&#xE32;&#xE07;&#xE40;&#xE25;&#xE37;&#xE2D;&#xE01;</span>
        </a>

        <p class="card__text card__summary">&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE19;&#xE17;&#xE2D;&#xE07;&#xE04;&#xE33; &#xE1B;&#xE49;&#xE2D;&#xE07;&#xE01;&#xE31;&#xE19;&#xE04;&#xE27;&#xE32;&#xE21;&#xE40;&#xE2A;&#xE35;&#xE48;&#xE22;&#xE07; &#xE2A;&#xE23;&#xE49;&#xE32;&#xE07;&#xE2A;&#xE21;&#xE14;&#xE38;&#xE25;&#xE1E;&#xE2D;&#xE23;&#xE4C;&#xE15;&#xE01;&#xE32;&#xE23;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;</p>

            
    <span class="rating">
        <img src="/media/images/morningstar/4-star.png" class="rating__image" alt="Morningstar Rating 4 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

    </div>

    <div class="card__footer">
        <div class="card__actions">
            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--8">8</span>
</span>


            <a href="/funds/a-ring" class="btn btn-split" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE42;&#xE01;&#xE25;&#xE14;&#xE4C;">
                <span>ดูรายละเอียด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>

    </div>
</article>

                    </div>
            </div>

            <div class="featured-funds__controls">
                <div class="slider-dots js-featured-pagination"></div>
            </div>
        </div>
    </div>
</section>', N'FeaturedFunds', CAST(@gid AS nvarchar(20)), N'กองทุนแนะนำประจำเดือน', N'Files/Site0/1/widget_icons/assetplus/icon-FeaturedFunds.png', N'', N'<section class="section featured-funds">
    <div class="container">
        <div class="section-head">
            <h2 class="section-head__title">กองทุนแนะนำประจำเดือน</h2>
            <a href="/funds/featured" class="btn btn-split btn-light" aria-label="ดูกองทุนแนะนำทั้งหมด">
                <span>ดูทั้งหมด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>

        <div class="featured-funds__carousel swiper js-featured-swiper">
            <div class="swiper-wrapper card-deck card-deck--cards-1 card-deck--cards-md-2 card-deck--cards-lg-3">
                    <div class="swiper-slide">
                        
<article class="card card--fund">
    <a href="/funds/a-humanoid" class="card__image" tabindex="-1" aria-hidden="true">
        <img src="/media/images/home/fund/a-humanoid.jpg" alt="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2E;&#xE34;&#xE27;&#xE41;&#xE21;&#xE19;&#xE19;&#xE2D;&#xE22;&#xE14;&#xE4C;" loading="lazy" decoding="async" />
    </a>


    <div class="card__body">
        <a href="/funds/a-humanoid" class="card__heading" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2E;&#xE34;&#xE27;&#xE41;&#xE21;&#xE19;&#xE19;&#xE2D;&#xE22;&#xE14;&#xE4C;">
            <h3 class="card__title card__code">A-HUMANOID</h3>
            <span class="card__meta card__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;/&#xE2B;&#xE38;&#xE49;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
        </a>

        <p class="card__text card__summary">&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE19;&#xE18;&#xE38;&#xE23;&#xE01;&#xE34;&#xE08;&#xE2B;&#xE38;&#xE48;&#xE19;&#xE22;&#xE19;&#xE15;&#xE4C;&#xE2E;&#xE34;&#xE27;&#xE41;&#xE21;&#xE19;&#xE19;&#xE2D;&#xE22;&#xE14;&#xE4C;&#xE41;&#xE25;&#xE30; AI &#xE23;&#xE30;&#xE14;&#xE31;&#xE1A;&#xE42;&#xE25;&#xE01; &#xE17;&#xE35;&#xE48;&#xE01;&#xE33;&#xE25;&#xE31;&#xE07;&#xE40;&#xE1B;&#xE25;&#xE35;&#xE48;&#xE22;&#xE19;&#xE42;&#xE09;&#xE21;&#xE20;&#xE32;&#xE04;&#xE01;&#xE32;&#xE23;&#xE1C;&#xE25;&#xE34;&#xE15;&#xE41;&#xE25;&#xE30;&#xE1A;&#xE23;&#xE34;&#xE01;&#xE32;&#xE23;</p>

            
    <span class="rating">
        <img src="/media/images/morningstar/5-star.png" class="rating__image" alt="Morningstar Rating 5 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

    </div>

    <div class="card__footer">
        <div class="card__actions">
            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--7">7</span>
</span>


            <a href="/funds/a-humanoid" class="btn btn-split" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2E;&#xE34;&#xE27;&#xE41;&#xE21;&#xE19;&#xE19;&#xE2D;&#xE22;&#xE14;&#xE4C;">
                <span>ดูรายละเอียด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>

    </div>
</article>

                    </div>
                    <div class="swiper-slide">
                        
<article class="card card--fund">
    <a href="/funds/a-grid" class="card__image" tabindex="-1" aria-hidden="true">
        <img src="/media/images/home/fund/a-grid.jpg" alt="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE01;&#xE23;&#xE34;&#xE14; &#xE2D;&#xE34;&#xE19;&#xE1F;&#xE23;&#xE32;&#xE2A;&#xE15;&#xE23;&#xE31;&#xE04;&#xE40;&#xE08;&#xE2D;&#xE23;&#xE4C;" loading="lazy" decoding="async" />
    </a>


    <div class="card__body">
        <a href="/funds/a-grid" class="card__heading" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE01;&#xE23;&#xE34;&#xE14; &#xE2D;&#xE34;&#xE19;&#xE1F;&#xE23;&#xE32;&#xE2A;&#xE15;&#xE23;&#xE31;&#xE04;&#xE40;&#xE08;&#xE2D;&#xE23;&#xE4C;">
            <h3 class="card__title card__code">A-GRID</h3>
            <span class="card__meta card__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;/&#xE2B;&#xE38;&#xE49;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
        </a>

        <p class="card__text card__summary">&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE19;&#xE42;&#xE04;&#xE23;&#xE07;&#xE2A;&#xE23;&#xE49;&#xE32;&#xE07;&#xE1E;&#xE37;&#xE49;&#xE19;&#xE10;&#xE32;&#xE19;&#xE14;&#xE49;&#xE32;&#xE19;&#xE1E;&#xE25;&#xE31;&#xE07;&#xE07;&#xE32;&#xE19;&#xE41;&#xE25;&#xE30;&#xE23;&#xE30;&#xE1A;&#xE1A; Smart Grid &#xE17;&#xE35;&#xE48;&#xE40;&#xE1B;&#xE47;&#xE19;&#xE2B;&#xE31;&#xE27;&#xE43;&#xE08;&#xE02;&#xE2D;&#xE07;&#xE42;&#xE25;&#xE01;&#xE2D;&#xE19;&#xE32;&#xE04;&#xE15;</p>

            
    <span class="rating">
        <img src="/media/images/morningstar/4-star.png" class="rating__image" alt="Morningstar Rating 4 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

    </div>

    <div class="card__footer">
        <div class="card__actions">
            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--3">3</span>
</span>


            <a href="/funds/a-grid" class="btn btn-split" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE01;&#xE23;&#xE34;&#xE14; &#xE2D;&#xE34;&#xE19;&#xE1F;&#xE23;&#xE32;&#xE2A;&#xE15;&#xE23;&#xE31;&#xE04;&#xE40;&#xE08;&#xE2D;&#xE23;&#xE4C;">
                <span>ดูรายละเอียด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>

    </div>
</article>

                    </div>
                    <div class="swiper-slide">
                        
<article class="card card--fund">
    <a href="/funds/a-asemi" class="card__image" tabindex="-1" aria-hidden="true">
        <img src="/media/images/home/fund/a-asemi.jpg" alt="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE40;&#xE2D;&#xE40;&#xE0A;&#xE35;&#xE22; &#xE40;&#xE0B;&#xE21;&#xE34;&#xE04;&#xE2D;&#xE19;&#xE14;&#xE31;&#xE01;&#xE40;&#xE15;&#xE2D;&#xE23;&#xE4C;" loading="lazy" decoding="async" />
    </a>


    <div class="card__body">
        <a href="/funds/a-asemi" class="card__heading" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE40;&#xE2D;&#xE40;&#xE0A;&#xE35;&#xE22; &#xE40;&#xE0B;&#xE21;&#xE34;&#xE04;&#xE2D;&#xE19;&#xE14;&#xE31;&#xE01;&#xE40;&#xE15;&#xE2D;&#xE23;&#xE4C;">
            <h3 class="card__title card__code">A-ASEMI</h3>
            <span class="card__meta card__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;/&#xE2B;&#xE38;&#xE49;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
        </a>

        <p class="card__text card__summary">&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE19;&#xE2B;&#xE48;&#xE27;&#xE07;&#xE42;&#xE0B;&#xE48;&#xE01;&#xE32;&#xE23;&#xE1C;&#xE25;&#xE34;&#xE15;&#xE40;&#xE0B;&#xE21;&#xE34;&#xE04;&#xE2D;&#xE19;&#xE14;&#xE31;&#xE01;&#xE40;&#xE15;&#xE2D;&#xE23;&#xE4C;&#xE41;&#xE2B;&#xE48;&#xE07;&#xE40;&#xE2D;&#xE40;&#xE0A;&#xE35;&#xE22; &#xE15;&#xE31;&#xE49;&#xE07;&#xE41;&#xE15;&#xE48;&#xE15;&#xE49;&#xE19;&#xE19;&#xE49;&#xE33;&#xE16;&#xE36;&#xE07;&#xE1B;&#xE25;&#xE32;&#xE22;&#xE19;&#xE49;&#xE33;</p>

            
    <span class="rating">
        <img src="/media/images/morningstar/4-star.png" class="rating__image" alt="Morningstar Rating 4 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

    </div>

    <div class="card__footer">
        <div class="card__actions">
            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--7">7</span>
</span>


            <a href="/funds/a-asemi" class="btn btn-split" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE40;&#xE2D;&#xE40;&#xE0A;&#xE35;&#xE22; &#xE40;&#xE0B;&#xE21;&#xE34;&#xE04;&#xE2D;&#xE19;&#xE14;&#xE31;&#xE01;&#xE40;&#xE15;&#xE2D;&#xE23;&#xE4C;">
                <span>ดูรายละเอียด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>

    </div>
</article>

                    </div>
                    <div class="swiper-slide">
                        
<article class="card card--fund">
    <a href="/funds/a-jedi" class="card__image" tabindex="-1" aria-hidden="true">
        <img src="/media/images/home/fund/a-jedi.jpg" alt="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2A;&#xE40;&#xE1B;&#xE0B; &#xE2D;&#xE35;&#xE42;&#xE04;&#xE42;&#xE19;&#xE21;&#xE35;" loading="lazy" decoding="async" />
    </a>


    <div class="card__body">
        <a href="/funds/a-jedi" class="card__heading" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2A;&#xE40;&#xE1B;&#xE0B; &#xE2D;&#xE35;&#xE42;&#xE04;&#xE42;&#xE19;&#xE21;&#xE35;">
            <h3 class="card__title card__code">A-JEDI</h3>
            <span class="card__meta card__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;/&#xE2B;&#xE38;&#xE49;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
        </a>

        <p class="card__text card__summary">&#xE42;&#xE2D;&#xE01;&#xE32;&#xE2A;&#xE40;&#xE15;&#xE34;&#xE1A;&#xE42;&#xE15;&#xE08;&#xE32;&#xE01;&#xE2D;&#xE38;&#xE15;&#xE2A;&#xE32;&#xE2B;&#xE01;&#xE23;&#xE23;&#xE21;&#xE2D;&#xE27;&#xE01;&#xE32;&#xE28;&#xE41;&#xE25;&#xE30;&#xE14;&#xE32;&#xE27;&#xE40;&#xE17;&#xE35;&#xE22;&#xE21;&#xE17;&#xE35;&#xE48;&#xE02;&#xE22;&#xE32;&#xE22;&#xE15;&#xE31;&#xE27;&#xE15;&#xE48;&#xE2D;&#xE40;&#xE19;&#xE37;&#xE48;&#xE2D;&#xE07;</p>

            
    <span class="rating">
        <img src="/media/images/morningstar/5-star.png" class="rating__image" alt="Morningstar Rating 5 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

    </div>

    <div class="card__footer">
        <div class="card__actions">
            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--7">7</span>
</span>


            <a href="/funds/a-jedi" class="btn btn-split" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2A;&#xE40;&#xE1B;&#xE0B; &#xE2D;&#xE35;&#xE42;&#xE04;&#xE42;&#xE19;&#xE21;&#xE35;">
                <span>ดูรายละเอียด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>

    </div>
</article>

                    </div>
                    <div class="swiper-slide">
                        
<article class="card card--fund">
    <a href="/funds/asp-crypto" class="card__image" tabindex="-1" aria-hidden="true">
        <img src="/media/images/home/fund/asp-crypto.jpg" alt="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE14;&#xE34;&#xE08;&#xE34;&#xE17;&#xE31;&#xE25; &#xE1A;&#xE25;&#xE47;&#xE2D;&#xE01;&#xE40;&#xE0A;&#xE19;" loading="lazy" decoding="async" />
    </a>


    <div class="card__body">
        <a href="/funds/asp-crypto" class="card__heading" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE14;&#xE34;&#xE08;&#xE34;&#xE17;&#xE31;&#xE25; &#xE1A;&#xE25;&#xE47;&#xE2D;&#xE01;&#xE40;&#xE0A;&#xE19;">
            <h3 class="card__title card__code">ASP-CRYPTO</h3>
            <span class="card__meta card__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;/&#xE2B;&#xE38;&#xE49;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
        </a>

        <p class="card__text card__summary">&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE19;&#xE18;&#xE38;&#xE23;&#xE01;&#xE34;&#xE08;&#xE41;&#xE1E;&#xE25;&#xE15;&#xE1F;&#xE2D;&#xE23;&#xE4C;&#xE21;&#xE14;&#xE34;&#xE08;&#xE34;&#xE17;&#xE31;&#xE25; &#xE41;&#xE25;&#xE30;&#xE40;&#xE17;&#xE04;&#xE42;&#xE19;&#xE42;&#xE25;&#xE22;&#xE35;&#xE1A;&#xE25;&#xE47;&#xE2D;&#xE01;&#xE40;&#xE0A;&#xE19;&#xE17;&#xE31;&#xE48;&#xE27;&#xE42;&#xE25;&#xE01;</p>

            
    <span class="rating">
        <img src="/media/images/morningstar/5-star.png" class="rating__image" alt="Morningstar Rating 5 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

    </div>

    <div class="card__footer">
        <div class="card__actions">
            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--8">8&#x2B;</span>
</span>


            <a href="/funds/asp-crypto" class="btn btn-split" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE14;&#xE34;&#xE08;&#xE34;&#xE17;&#xE31;&#xE25; &#xE1A;&#xE25;&#xE47;&#xE2D;&#xE01;&#xE40;&#xE0A;&#xE19;">
                <span>ดูรายละเอียด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>

    </div>
</article>

                    </div>
                    <div class="swiper-slide">
                        
<article class="card card--fund">
    <a href="/funds/a-ring" class="card__image" tabindex="-1" aria-hidden="true">
        <img src="/media/images/home/fund/a-ring.jpg" alt="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE42;&#xE01;&#xE25;&#xE14;&#xE4C;" loading="lazy" decoding="async" />
    </a>


    <div class="card__body">
        <a href="/funds/a-ring" class="card__heading" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE42;&#xE01;&#xE25;&#xE14;&#xE4C;">
            <h3 class="card__title card__code">A-RING</h3>
            <span class="card__meta card__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C;&#xE2A;&#xE34;&#xE19;&#xE17;&#xE32;&#xE07;&#xE40;&#xE25;&#xE37;&#xE2D;&#xE01;</span>
        </a>

        <p class="card__text card__summary">&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE19;&#xE17;&#xE2D;&#xE07;&#xE04;&#xE33; &#xE1B;&#xE49;&#xE2D;&#xE07;&#xE01;&#xE31;&#xE19;&#xE04;&#xE27;&#xE32;&#xE21;&#xE40;&#xE2A;&#xE35;&#xE48;&#xE22;&#xE07; &#xE2A;&#xE23;&#xE49;&#xE32;&#xE07;&#xE2A;&#xE21;&#xE14;&#xE38;&#xE25;&#xE1E;&#xE2D;&#xE23;&#xE4C;&#xE15;&#xE01;&#xE32;&#xE23;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;</p>

            
    <span class="rating">
        <img src="/media/images/morningstar/4-star.png" class="rating__image" alt="Morningstar Rating 4 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

    </div>

    <div class="card__footer">
        <div class="card__actions">
            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--8">8</span>
</span>


            <a href="/funds/a-ring" class="btn btn-split" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE42;&#xE01;&#xE25;&#xE14;&#xE4C;">
                <span>ดูรายละเอียด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>

    </div>
</article>

                    </div>
            </div>

            <div class="featured-funds__controls">
                <div class="slider-dots js-featured-pagination"></div>
            </div>
        </div>
    </div>
</section>', N'FeaturedFunds', 0);
INSERT INTO [2026_web_widget] (created_at, updated_at, created_by, updated_by, sort, status, pb_status, approve_by, show_front, cat_id, title, img1, mod_name, info, section_key, pb_cat_id, pb_title, pb_img1, pb_mod_name, pb_info, pb_section_key, web_id) VALUES (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'user', N'user', 40, 1, 1, N'user', 1, @gid, N'เปิดมุมมองลงทุนตามเทรนด์', N'Files/Site0/1/widget_icons/assetplus/icon-ExploreThemes.png', N'', N'<section class="section">
    <div class="container-fluid">
        <div class="explore-themes">
            <div class="container">
                <div class="explore-themes__heading">
                    <span class="explore-themes__watermark" aria-hidden="true">Explore by Theme</span>
                    <h2 class="explore-themes__title">เปิดมุมมองลงทุนตามเทรนด์</h2>
                </div>

                <div class="theme-grid">
                        <div class="theme-grid__item">
                            <a href="/funds/themes?theme=ai-robotics" class="theme-tile">
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-robot"></i>
</span>

                                <span class="theme-tile__name">AI &amp; Robotics</span>
                            </a>
                        </div>
                        <div class="theme-grid__item">
                            <a href="/funds/themes?theme=semiconductor" class="theme-tile">
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-cpu"></i>
</span>

                                <span class="theme-tile__name">Semiconductor</span>
                            </a>
                        </div>
                        <div class="theme-grid__item">
                            <a href="/funds/themes?theme=digital-assets" class="theme-tile">
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-currency-bitcoin"></i>
</span>

                                <span class="theme-tile__name">Digital Assets</span>
                            </a>
                        </div>
                        <div class="theme-grid__item">
                            <a href="/funds/themes?theme=clean-energy" class="theme-tile">
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-lightning-charge"></i>
</span>

                                <span class="theme-tile__name">Clean Energy</span>
                            </a>
                        </div>
                        <div class="theme-grid__item">
                            <a href="/funds/themes?theme=aerospace-defense" class="theme-tile">
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-rocket-takeoff"></i>
</span>

                                <span class="theme-tile__name">Aerospace &amp; Defense</span>
                            </a>
                        </div>
                        <div class="theme-grid__item">
                            <a href="/funds/themes?theme=esg" class="theme-tile">
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-tree"></i>
</span>

                                <span class="theme-tile__name">ESG &amp; Sustainability</span>
                            </a>
                        </div>
                </div>

                <div class="explore-themes__footer">
                    <a href="/funds/themes" class="btn btn-split btn-light" aria-label="ดูธีมการลงทุนทั้งหมด">
                        <span>ดูทั้งหมด</span>
                        <span class="btn-split__icon">
                            <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                        </span>
                    </a>
                </div>
            </div>
        </div>
    </div>
</section>', N'ExploreThemes', CAST(@gid AS nvarchar(20)), N'เปิดมุมมองลงทุนตามเทรนด์', N'Files/Site0/1/widget_icons/assetplus/icon-ExploreThemes.png', N'', N'<section class="section">
    <div class="container-fluid">
        <div class="explore-themes">
            <div class="container">
                <div class="explore-themes__heading">
                    <span class="explore-themes__watermark" aria-hidden="true">Explore by Theme</span>
                    <h2 class="explore-themes__title">เปิดมุมมองลงทุนตามเทรนด์</h2>
                </div>

                <div class="theme-grid">
                        <div class="theme-grid__item">
                            <a href="/funds/themes?theme=ai-robotics" class="theme-tile">
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-robot"></i>
</span>

                                <span class="theme-tile__name">AI &amp; Robotics</span>
                            </a>
                        </div>
                        <div class="theme-grid__item">
                            <a href="/funds/themes?theme=semiconductor" class="theme-tile">
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-cpu"></i>
</span>

                                <span class="theme-tile__name">Semiconductor</span>
                            </a>
                        </div>
                        <div class="theme-grid__item">
                            <a href="/funds/themes?theme=digital-assets" class="theme-tile">
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-currency-bitcoin"></i>
</span>

                                <span class="theme-tile__name">Digital Assets</span>
                            </a>
                        </div>
                        <div class="theme-grid__item">
                            <a href="/funds/themes?theme=clean-energy" class="theme-tile">
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-lightning-charge"></i>
</span>

                                <span class="theme-tile__name">Clean Energy</span>
                            </a>
                        </div>
                        <div class="theme-grid__item">
                            <a href="/funds/themes?theme=aerospace-defense" class="theme-tile">
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-rocket-takeoff"></i>
</span>

                                <span class="theme-tile__name">Aerospace &amp; Defense</span>
                            </a>
                        </div>
                        <div class="theme-grid__item">
                            <a href="/funds/themes?theme=esg" class="theme-tile">
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-tree"></i>
</span>

                                <span class="theme-tile__name">ESG &amp; Sustainability</span>
                            </a>
                        </div>
                </div>

                <div class="explore-themes__footer">
                    <a href="/funds/themes" class="btn btn-split btn-light" aria-label="ดูธีมการลงทุนทั้งหมด">
                        <span>ดูทั้งหมด</span>
                        <span class="btn-split__icon">
                            <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                        </span>
                    </a>
                </div>
            </div>
        </div>
    </div>
</section>', N'ExploreThemes', 0);
INSERT INTO [2026_web_widget] (created_at, updated_at, created_by, updated_by, sort, status, pb_status, approve_by, show_front, cat_id, title, img1, mod_name, info, section_key, pb_cat_id, pb_title, pb_img1, pb_mod_name, pb_info, pb_section_key, web_id) VALUES (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'user', N'user', 50, 1, 1, N'user', 1, @gid, N'บทความ / กิจกรรม / ข่าวประกาศ', N'Files/Site0/1/widget_icons/assetplus/icon-Insights.png', N'', N'<section class="section insights" aria-label="บทความ ข่าวสาร และประกาศ">
    <div class="container">
        <div class="card-deck card-deck--cards-1 card-deck--cards-md-2 card-deck--cards-lg-3">
            
<section class="insight-col">
    <div class="section-head">
        <h2 class="section-head__title">&#xE1A;&#xE17;&#xE04;&#xE27;&#xE32;&#xE21;</h2>
    </div>

    <div class="insight-col__body">
            
<a href="/articles/ai-revolution-2026" class="insight-feature">
    <span class="insight-feature__image card__image">
        <img src="/media/images/home/insight/ai-revolution-2026.jpg" alt="" loading="lazy" decoding="async" />
    </span>
    <span class="insight-feature__title">AI Revolution: &#xE42;&#xE2D;&#xE01;&#xE32;&#xE2A;&#xE01;&#xE32;&#xE23;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE17;&#xE35;&#xE48;&#xE2D;&#xE22;&#xE39;&#xE48;&#xE43;&#xE01;&#xE25;&#xE49;&#xE01;&#xE27;&#xE48;&#xE32;&#xE17;&#xE35;&#xE48;&#xE04;&#xE34;&#xE14;</span>
    <time class="insight-feature__date" datetime="2569-07-09">
        9 &#xE01;.&#xE04;. 2569
    </time>
</a>


            <div class="insight-rows">
                    
<a href="/articles/semiconductor-humanoid-h2-2026" class="insight-row">
    <span class="insight-row__thumb">
        <img src="/media/images/home/insight/asset-plus-investment-forum-2026.jpg" alt="" loading="lazy" decoding="async" />
    </span>
    <span class="insight-row__body">
        <span class="insight-row__title">&#xE2A;&#xE48;&#xE2D;&#xE07;&#xE42;&#xE2D;&#xE01;&#xE32;&#xE2A; Semiconductor &amp; Humanoid &#xE04;&#xE23;&#xE36;&#xE48;&#xE07;&#xE1B;&#xE35;&#xE2B;&#xE25;&#xE31;&#xE07; 2026</span>
        <time class="insight-row__date" datetime="2569-07-03">
            3 &#xE01;.&#xE04;. 2569
        </time>
    </span>
</a>

                    
<a href="/articles/semiconductor-cycle-restart" class="insight-row">
    <span class="insight-row__thumb">
        <img src="/media/images/home/insight/semiconductor-humanoid-h2-2026.jpg" alt="" loading="lazy" decoding="async" />
    </span>
    <span class="insight-row__body">
        <span class="insight-row__title">Semiconductor Cycle &#xE23;&#xE2D;&#xE1A;&#xE43;&#xE2B;&#xE21;&#xE48;&#xE40;&#xE23;&#xE34;&#xE48;&#xE21;&#xE41;&#xE25;&#xE49;&#xE27;&#xE2B;&#xE23;&#xE37;&#xE2D;&#xE22;&#xE31;&#xE07;</span>
        <time class="insight-row__date" datetime="2569-06-27">
            27 &#xE21;&#xE34;.&#xE22;. 2569
        </time>
    </span>
</a>

            </div>
    </div>

    <div class="insight-col__footer">
        <a href="/articles" class="btn btn-split btn-light" aria-label="ดู&#xE1A;&#xE17;&#xE04;&#xE27;&#xE32;&#xE21;ทั้งหมด">
            <span>ดูทั้งหมด</span>
            <span class="btn-split__icon">
                <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
            </span>
        </a>
    </div>
</section>

            
<section class="insight-col">
    <div class="section-head">
        <h2 class="section-head__title">&#xE01;&#xE34;&#xE08;&#xE01;&#xE23;&#xE23;&#xE21;</h2>
    </div>

    <div class="insight-col__body">
            
<a href="/events/money-banking-awards-2026" class="insight-feature">
    <span class="insight-feature__image card__image">
        <img src="/media/images/home/insight/semiconductor-humanoid-h2-2026.jpg" alt="" loading="lazy" decoding="async" />
    </span>
    <span class="insight-feature__title">&#xE1A;&#xE25;&#xE08;. &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE04;&#xE27;&#xE49;&#xE32;&#xE23;&#xE32;&#xE07;&#xE27;&#xE31;&#xE25; Money &amp; Banking Awards 2026 &#xE08;&#xE32;&#xE01;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19; ASP-NGF</span>
    <time class="insight-feature__date" datetime="2569-07-09">
        9 &#xE01;.&#xE04;. 2569
    </time>
</a>


            <div class="insight-rows">
                    
<a href="/events/thailand-gold-summit-2026" class="insight-row">
    <span class="insight-row__thumb">
        <img src="/media/images/home/insight/semiconductor-cycle-restart.jpg" alt="" loading="lazy" decoding="async" />
    </span>
    <span class="insight-row__body">
        <span class="insight-row__title">&#xE1A;&#xE25;&#xE08;. &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE0A;&#xE35;&#xE49;&#xE42;&#xE25;&#xE01;&#xE01;&#xE32;&#xE23;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE22;&#xE38;&#xE04; Uncertainty &#xE15;&#xE49;&#xE2D;&#xE07;&#xE01;&#xE23;&#xE30;&#xE08;&#xE32;&#xE22;&#xE1E;&#xE2D;&#xE23;&#xE4C;&#xE15;&#xE14;&#xE49;&#xE27;&#xE22;&#xE2A;&#xE34;&#xE19;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C;&#xE40;&#xE0A;&#xE34;&#xE07;&#xE01;&#xE25;&#xE22;&#xE38;&#xE17;&#xE18;&#xE4C;</span>
        <time class="insight-row__date" datetime="2569-06-30">
            30 &#xE21;&#xE34;.&#xE22;. 2569
        </time>
    </span>
</a>

                    
<a href="/events/gsb-the-selected" class="insight-row">
    <span class="insight-row__thumb">
        <img src="/media/images/home/insight/a-humanoid-ipo.jpg" alt="" loading="lazy" decoding="async" />
    </span>
    <span class="insight-row__body">
        <span class="insight-row__title">&#xE1A;&#xE25;&#xE08;. &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE23;&#xE48;&#xE27;&#xE21;&#xE40;&#xE1B;&#xE47;&#xE19;&#xE1E;&#xE31;&#xE19;&#xE18;&#xE21;&#xE34;&#xE15;&#xE23;&#xE01;&#xE31;&#xE1A;&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE2D;&#xE2D;&#xE21;&#xE2A;&#xE34;&#xE19; &#xE43;&#xE19;&#xE42;&#xE04;&#xE23;&#xE07;&#xE01;&#xE32;&#xE23; &#x201C;&#xE2D;&#xE2D;&#xE21;&#xE2A;&#xE34;&#xE19; The Selected&#x201D;</span>
        <time class="insight-row__date" datetime="2569-06-21">
            21 &#xE21;&#xE34;.&#xE22;. 2569
        </time>
    </span>
</a>

            </div>
    </div>

    <div class="insight-col__footer">
        <a href="/events" class="btn btn-split btn-light" aria-label="ดู&#xE01;&#xE34;&#xE08;&#xE01;&#xE23;&#xE23;&#xE21;ทั้งหมด">
            <span>ดูทั้งหมด</span>
            <span class="btn-split__icon">
                <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
            </span>
        </a>
    </div>
</section>

            
<section class="insight-col">
    <div class="section-head">
        <h2 class="section-head__title">&#xE02;&#xE48;&#xE32;&#xE27;&#xE2A;&#xE32;&#xE23;&#xE41;&#xE25;&#xE30;&#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28; &#xE1A;&#xE25;&#xE08;.</h2>
    </div>

    <ul class="announcement-list insight-col__body">
            <li>
                <a href="/media/documents/sample.pdf" class="announcement-item" target="_blank" rel="noopener"
                   aria-label="&#xE1C;&#xE25;&#xE01;&#xE32;&#xE23;&#xE14;&#xE33;&#xE40;&#xE19;&#xE34;&#xE19;&#xE07;&#xE32;&#xE19;: &#xE23;&#xE32;&#xE22;&#xE07;&#xE32;&#xE19;&#xE1C;&#xE25;&#xE01;&#xE32;&#xE23;&#xE14;&#xE33;&#xE40;&#xE19;&#xE34;&#xE19;&#xE07;&#xE32;&#xE19;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE20;&#xE32;&#xE22;&#xE43;&#xE15;&#xE49;&#xE01;&#xE32;&#xE23;&#xE08;&#xE31;&#xE14;&#xE01;&#xE32;&#xE23; &#xE1B;&#xE23;&#xE30;&#xE08;&#xE33;&#xE44;&#xE15;&#xE23;&#xE21;&#xE32;&#xE2A; 2/2569 (ไฟล์ PDF, เปิดในแท็บใหม่)">
                    <span class="announcement-item__icon" aria-hidden="true">
                        <i class="bi bi-graph-up-arrow"></i>
                    </span>
                    <span class="announcement-item__body">
                        <span class="announcement-item__title">&#xE23;&#xE32;&#xE22;&#xE07;&#xE32;&#xE19;&#xE1C;&#xE25;&#xE01;&#xE32;&#xE23;&#xE14;&#xE33;&#xE40;&#xE19;&#xE34;&#xE19;&#xE07;&#xE32;&#xE19;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE20;&#xE32;&#xE22;&#xE43;&#xE15;&#xE49;&#xE01;&#xE32;&#xE23;&#xE08;&#xE31;&#xE14;&#xE01;&#xE32;&#xE23; &#xE1B;&#xE23;&#xE30;&#xE08;&#xE33;&#xE44;&#xE15;&#xE23;&#xE21;&#xE32;&#xE2A; 2/2569</span>
                        <time class="announcement-item__date" datetime="2569-05-28">
                            28 &#xE1E;.&#xE04;. 2569
                        </time>
                    </span>
                </a>
            </li>
            <li>
                <a href="/media/documents/sample.pdf" class="announcement-item" target="_blank" rel="noopener"
                   aria-label="&#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE27;&#xE31;&#xE19;&#xE2B;&#xE22;&#xE38;&#xE14;&#xE0B;&#xE37;&#xE49;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;: &#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE27;&#xE31;&#xE19;&#xE2B;&#xE22;&#xE38;&#xE14;&#xE17;&#xE33;&#xE01;&#xE32;&#xE23;&#xE0B;&#xE37;&#xE49;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19; &#xE1B;&#xE23;&#xE30;&#xE08;&#xE33;&#xE40;&#xE14;&#xE37;&#xE2D;&#xE19;&#xE01;&#xE23;&#xE01;&#xE0E;&#xE32;&#xE04;&#xE21; 2569 (ไฟล์ PDF, เปิดในแท็บใหม่)">
                    <span class="announcement-item__icon" aria-hidden="true">
                        <i class="bi bi-calendar3"></i>
                    </span>
                    <span class="announcement-item__body">
                        <span class="announcement-item__title">&#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE27;&#xE31;&#xE19;&#xE2B;&#xE22;&#xE38;&#xE14;&#xE17;&#xE33;&#xE01;&#xE32;&#xE23;&#xE0B;&#xE37;&#xE49;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19; &#xE1B;&#xE23;&#xE30;&#xE08;&#xE33;&#xE40;&#xE14;&#xE37;&#xE2D;&#xE19;&#xE01;&#xE23;&#xE01;&#xE0E;&#xE32;&#xE04;&#xE21; 2569</span>
                        <time class="announcement-item__date" datetime="2569-05-28">
                            28 &#xE1E;.&#xE04;. 2569
                        </time>
                    </span>
                </a>
            </li>
            <li>
                <a href="/media/documents/sample.pdf" class="announcement-item" target="_blank" rel="noopener"
                   aria-label="&#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE41;&#xE08;&#xE49;&#xE07;&#xE1B;&#xE31;&#xE19;&#xE1C;&#xE25;: &#xE41;&#xE08;&#xE49;&#xE07;&#xE01;&#xE32;&#xE23;&#xE08;&#xE48;&#xE32;&#xE22;&#xE40;&#xE07;&#xE34;&#xE19;&#xE1B;&#xE31;&#xE19;&#xE1C;&#xE25;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19; ASP-DIGITAL (ไฟล์ PDF, เปิดในแท็บใหม่)">
                    <span class="announcement-item__icon" aria-hidden="true">
                        <i class="bi bi-cash-coin"></i>
                    </span>
                    <span class="announcement-item__body">
                        <span class="announcement-item__title">&#xE41;&#xE08;&#xE49;&#xE07;&#xE01;&#xE32;&#xE23;&#xE08;&#xE48;&#xE32;&#xE22;&#xE40;&#xE07;&#xE34;&#xE19;&#xE1B;&#xE31;&#xE19;&#xE1C;&#xE25;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19; ASP-DIGITAL</span>
                        <time class="announcement-item__date" datetime="2569-05-23">
                            23 &#xE1E;.&#xE04;. 2569
                        </time>
                    </span>
                </a>
            </li>
            <li>
                <a href="/media/documents/sample.pdf" class="announcement-item" target="_blank" rel="noopener"
                   aria-label="&#xE23;&#xE32;&#xE07;&#xE27;&#xE31;&#xE25;&#xE41;&#xE25;&#xE30;&#xE04;&#xE27;&#xE32;&#xE21;&#xE2A;&#xE33;&#xE40;&#xE23;&#xE47;&#xE08;: &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE04;&#xE27;&#xE49;&#xE32;&#xE23;&#xE32;&#xE07;&#xE27;&#xE31;&#xE25;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE22;&#xE2D;&#xE14;&#xE40;&#xE22;&#xE35;&#xE48;&#xE22;&#xE21;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE20;&#xE17;&#xE15;&#xE23;&#xE32;&#xE2A;&#xE32;&#xE23;&#xE17;&#xE38;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28; (ไฟล์ PDF, เปิดในแท็บใหม่)">
                    <span class="announcement-item__icon" aria-hidden="true">
                        <i class="bi bi-trophy"></i>
                    </span>
                    <span class="announcement-item__body">
                        <span class="announcement-item__title">&#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE04;&#xE27;&#xE49;&#xE32;&#xE23;&#xE32;&#xE07;&#xE27;&#xE31;&#xE25;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE22;&#xE2D;&#xE14;&#xE40;&#xE22;&#xE35;&#xE48;&#xE22;&#xE21;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE20;&#xE17;&#xE15;&#xE23;&#xE32;&#xE2A;&#xE32;&#xE23;&#xE17;&#xE38;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
                        <time class="announcement-item__date" datetime="2569-05-20">
                            20 &#xE1E;.&#xE04;. 2569
                        </time>
                    </span>
                </a>
            </li>
            <li>
                <a href="/media/documents/sample.pdf" class="announcement-item" target="_blank" rel="noopener"
                   aria-label="&#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE40;&#xE1B;&#xE34;&#xE14;&#xE40;&#xE2A;&#xE19;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;: &#xE40;&#xE1B;&#xE34;&#xE14;&#xE40;&#xE2A;&#xE19;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE2B;&#xE21;&#xE48; A-HUMANOID (ไฟล์ PDF, เปิดในแท็บใหม่)">
                    <span class="announcement-item__icon" aria-hidden="true">
                        <i class="bi bi-bar-chart-line"></i>
                    </span>
                    <span class="announcement-item__body">
                        <span class="announcement-item__title">&#xE40;&#xE1B;&#xE34;&#xE14;&#xE40;&#xE2A;&#xE19;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE2B;&#xE21;&#xE48; A-HUMANOID</span>
                        <time class="announcement-item__date" datetime="2569-05-18">
                            18 &#xE1E;.&#xE04;. 2569
                        </time>
                    </span>
                </a>
            </li>
    </ul>

    <div class="insight-col__footer">
        <a href="/news-announcements" class="btn btn-split btn-light" aria-label="ดูประกาศทั้งหมด">
            <span>ดูทั้งหมด</span>
            <span class="btn-split__icon">
                <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
            </span>
        </a>
    </div>
</section>

        </div>
    </div>
</section>', N'Insights', CAST(@gid AS nvarchar(20)), N'บทความ / กิจกรรม / ข่าวประกาศ', N'Files/Site0/1/widget_icons/assetplus/icon-Insights.png', N'', N'<section class="section insights" aria-label="บทความ ข่าวสาร และประกาศ">
    <div class="container">
        <div class="card-deck card-deck--cards-1 card-deck--cards-md-2 card-deck--cards-lg-3">
            
<section class="insight-col">
    <div class="section-head">
        <h2 class="section-head__title">&#xE1A;&#xE17;&#xE04;&#xE27;&#xE32;&#xE21;</h2>
    </div>

    <div class="insight-col__body">
            
<a href="/articles/ai-revolution-2026" class="insight-feature">
    <span class="insight-feature__image card__image">
        <img src="/media/images/home/insight/ai-revolution-2026.jpg" alt="" loading="lazy" decoding="async" />
    </span>
    <span class="insight-feature__title">AI Revolution: &#xE42;&#xE2D;&#xE01;&#xE32;&#xE2A;&#xE01;&#xE32;&#xE23;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE17;&#xE35;&#xE48;&#xE2D;&#xE22;&#xE39;&#xE48;&#xE43;&#xE01;&#xE25;&#xE49;&#xE01;&#xE27;&#xE48;&#xE32;&#xE17;&#xE35;&#xE48;&#xE04;&#xE34;&#xE14;</span>
    <time class="insight-feature__date" datetime="2569-07-09">
        9 &#xE01;.&#xE04;. 2569
    </time>
</a>


            <div class="insight-rows">
                    
<a href="/articles/semiconductor-humanoid-h2-2026" class="insight-row">
    <span class="insight-row__thumb">
        <img src="/media/images/home/insight/asset-plus-investment-forum-2026.jpg" alt="" loading="lazy" decoding="async" />
    </span>
    <span class="insight-row__body">
        <span class="insight-row__title">&#xE2A;&#xE48;&#xE2D;&#xE07;&#xE42;&#xE2D;&#xE01;&#xE32;&#xE2A; Semiconductor &amp; Humanoid &#xE04;&#xE23;&#xE36;&#xE48;&#xE07;&#xE1B;&#xE35;&#xE2B;&#xE25;&#xE31;&#xE07; 2026</span>
        <time class="insight-row__date" datetime="2569-07-03">
            3 &#xE01;.&#xE04;. 2569
        </time>
    </span>
</a>

                    
<a href="/articles/semiconductor-cycle-restart" class="insight-row">
    <span class="insight-row__thumb">
        <img src="/media/images/home/insight/semiconductor-humanoid-h2-2026.jpg" alt="" loading="lazy" decoding="async" />
    </span>
    <span class="insight-row__body">
        <span class="insight-row__title">Semiconductor Cycle &#xE23;&#xE2D;&#xE1A;&#xE43;&#xE2B;&#xE21;&#xE48;&#xE40;&#xE23;&#xE34;&#xE48;&#xE21;&#xE41;&#xE25;&#xE49;&#xE27;&#xE2B;&#xE23;&#xE37;&#xE2D;&#xE22;&#xE31;&#xE07;</span>
        <time class="insight-row__date" datetime="2569-06-27">
            27 &#xE21;&#xE34;.&#xE22;. 2569
        </time>
    </span>
</a>

            </div>
    </div>

    <div class="insight-col__footer">
        <a href="/articles" class="btn btn-split btn-light" aria-label="ดู&#xE1A;&#xE17;&#xE04;&#xE27;&#xE32;&#xE21;ทั้งหมด">
            <span>ดูทั้งหมด</span>
            <span class="btn-split__icon">
                <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
            </span>
        </a>
    </div>
</section>

            
<section class="insight-col">
    <div class="section-head">
        <h2 class="section-head__title">&#xE01;&#xE34;&#xE08;&#xE01;&#xE23;&#xE23;&#xE21;</h2>
    </div>

    <div class="insight-col__body">
            
<a href="/events/money-banking-awards-2026" class="insight-feature">
    <span class="insight-feature__image card__image">
        <img src="/media/images/home/insight/semiconductor-humanoid-h2-2026.jpg" alt="" loading="lazy" decoding="async" />
    </span>
    <span class="insight-feature__title">&#xE1A;&#xE25;&#xE08;. &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE04;&#xE27;&#xE49;&#xE32;&#xE23;&#xE32;&#xE07;&#xE27;&#xE31;&#xE25; Money &amp; Banking Awards 2026 &#xE08;&#xE32;&#xE01;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19; ASP-NGF</span>
    <time class="insight-feature__date" datetime="2569-07-09">
        9 &#xE01;.&#xE04;. 2569
    </time>
</a>


            <div class="insight-rows">
                    
<a href="/events/thailand-gold-summit-2026" class="insight-row">
    <span class="insight-row__thumb">
        <img src="/media/images/home/insight/semiconductor-cycle-restart.jpg" alt="" loading="lazy" decoding="async" />
    </span>
    <span class="insight-row__body">
        <span class="insight-row__title">&#xE1A;&#xE25;&#xE08;. &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE0A;&#xE35;&#xE49;&#xE42;&#xE25;&#xE01;&#xE01;&#xE32;&#xE23;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE22;&#xE38;&#xE04; Uncertainty &#xE15;&#xE49;&#xE2D;&#xE07;&#xE01;&#xE23;&#xE30;&#xE08;&#xE32;&#xE22;&#xE1E;&#xE2D;&#xE23;&#xE4C;&#xE15;&#xE14;&#xE49;&#xE27;&#xE22;&#xE2A;&#xE34;&#xE19;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C;&#xE40;&#xE0A;&#xE34;&#xE07;&#xE01;&#xE25;&#xE22;&#xE38;&#xE17;&#xE18;&#xE4C;</span>
        <time class="insight-row__date" datetime="2569-06-30">
            30 &#xE21;&#xE34;.&#xE22;. 2569
        </time>
    </span>
</a>

                    
<a href="/events/gsb-the-selected" class="insight-row">
    <span class="insight-row__thumb">
        <img src="/media/images/home/insight/a-humanoid-ipo.jpg" alt="" loading="lazy" decoding="async" />
    </span>
    <span class="insight-row__body">
        <span class="insight-row__title">&#xE1A;&#xE25;&#xE08;. &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE23;&#xE48;&#xE27;&#xE21;&#xE40;&#xE1B;&#xE47;&#xE19;&#xE1E;&#xE31;&#xE19;&#xE18;&#xE21;&#xE34;&#xE15;&#xE23;&#xE01;&#xE31;&#xE1A;&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE2D;&#xE2D;&#xE21;&#xE2A;&#xE34;&#xE19; &#xE43;&#xE19;&#xE42;&#xE04;&#xE23;&#xE07;&#xE01;&#xE32;&#xE23; &#x201C;&#xE2D;&#xE2D;&#xE21;&#xE2A;&#xE34;&#xE19; The Selected&#x201D;</span>
        <time class="insight-row__date" datetime="2569-06-21">
            21 &#xE21;&#xE34;.&#xE22;. 2569
        </time>
    </span>
</a>

            </div>
    </div>

    <div class="insight-col__footer">
        <a href="/events" class="btn btn-split btn-light" aria-label="ดู&#xE01;&#xE34;&#xE08;&#xE01;&#xE23;&#xE23;&#xE21;ทั้งหมด">
            <span>ดูทั้งหมด</span>
            <span class="btn-split__icon">
                <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
            </span>
        </a>
    </div>
</section>

            
<section class="insight-col">
    <div class="section-head">
        <h2 class="section-head__title">&#xE02;&#xE48;&#xE32;&#xE27;&#xE2A;&#xE32;&#xE23;&#xE41;&#xE25;&#xE30;&#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28; &#xE1A;&#xE25;&#xE08;.</h2>
    </div>

    <ul class="announcement-list insight-col__body">
            <li>
                <a href="/media/documents/sample.pdf" class="announcement-item" target="_blank" rel="noopener"
                   aria-label="&#xE1C;&#xE25;&#xE01;&#xE32;&#xE23;&#xE14;&#xE33;&#xE40;&#xE19;&#xE34;&#xE19;&#xE07;&#xE32;&#xE19;: &#xE23;&#xE32;&#xE22;&#xE07;&#xE32;&#xE19;&#xE1C;&#xE25;&#xE01;&#xE32;&#xE23;&#xE14;&#xE33;&#xE40;&#xE19;&#xE34;&#xE19;&#xE07;&#xE32;&#xE19;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE20;&#xE32;&#xE22;&#xE43;&#xE15;&#xE49;&#xE01;&#xE32;&#xE23;&#xE08;&#xE31;&#xE14;&#xE01;&#xE32;&#xE23; &#xE1B;&#xE23;&#xE30;&#xE08;&#xE33;&#xE44;&#xE15;&#xE23;&#xE21;&#xE32;&#xE2A; 2/2569 (ไฟล์ PDF, เปิดในแท็บใหม่)">
                    <span class="announcement-item__icon" aria-hidden="true">
                        <i class="bi bi-graph-up-arrow"></i>
                    </span>
                    <span class="announcement-item__body">
                        <span class="announcement-item__title">&#xE23;&#xE32;&#xE22;&#xE07;&#xE32;&#xE19;&#xE1C;&#xE25;&#xE01;&#xE32;&#xE23;&#xE14;&#xE33;&#xE40;&#xE19;&#xE34;&#xE19;&#xE07;&#xE32;&#xE19;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE20;&#xE32;&#xE22;&#xE43;&#xE15;&#xE49;&#xE01;&#xE32;&#xE23;&#xE08;&#xE31;&#xE14;&#xE01;&#xE32;&#xE23; &#xE1B;&#xE23;&#xE30;&#xE08;&#xE33;&#xE44;&#xE15;&#xE23;&#xE21;&#xE32;&#xE2A; 2/2569</span>
                        <time class="announcement-item__date" datetime="2569-05-28">
                            28 &#xE1E;.&#xE04;. 2569
                        </time>
                    </span>
                </a>
            </li>
            <li>
                <a href="/media/documents/sample.pdf" class="announcement-item" target="_blank" rel="noopener"
                   aria-label="&#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE27;&#xE31;&#xE19;&#xE2B;&#xE22;&#xE38;&#xE14;&#xE0B;&#xE37;&#xE49;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;: &#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE27;&#xE31;&#xE19;&#xE2B;&#xE22;&#xE38;&#xE14;&#xE17;&#xE33;&#xE01;&#xE32;&#xE23;&#xE0B;&#xE37;&#xE49;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19; &#xE1B;&#xE23;&#xE30;&#xE08;&#xE33;&#xE40;&#xE14;&#xE37;&#xE2D;&#xE19;&#xE01;&#xE23;&#xE01;&#xE0E;&#xE32;&#xE04;&#xE21; 2569 (ไฟล์ PDF, เปิดในแท็บใหม่)">
                    <span class="announcement-item__icon" aria-hidden="true">
                        <i class="bi bi-calendar3"></i>
                    </span>
                    <span class="announcement-item__body">
                        <span class="announcement-item__title">&#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE27;&#xE31;&#xE19;&#xE2B;&#xE22;&#xE38;&#xE14;&#xE17;&#xE33;&#xE01;&#xE32;&#xE23;&#xE0B;&#xE37;&#xE49;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19; &#xE1B;&#xE23;&#xE30;&#xE08;&#xE33;&#xE40;&#xE14;&#xE37;&#xE2D;&#xE19;&#xE01;&#xE23;&#xE01;&#xE0E;&#xE32;&#xE04;&#xE21; 2569</span>
                        <time class="announcement-item__date" datetime="2569-05-28">
                            28 &#xE1E;.&#xE04;. 2569
                        </time>
                    </span>
                </a>
            </li>
            <li>
                <a href="/media/documents/sample.pdf" class="announcement-item" target="_blank" rel="noopener"
                   aria-label="&#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE41;&#xE08;&#xE49;&#xE07;&#xE1B;&#xE31;&#xE19;&#xE1C;&#xE25;: &#xE41;&#xE08;&#xE49;&#xE07;&#xE01;&#xE32;&#xE23;&#xE08;&#xE48;&#xE32;&#xE22;&#xE40;&#xE07;&#xE34;&#xE19;&#xE1B;&#xE31;&#xE19;&#xE1C;&#xE25;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19; ASP-DIGITAL (ไฟล์ PDF, เปิดในแท็บใหม่)">
                    <span class="announcement-item__icon" aria-hidden="true">
                        <i class="bi bi-cash-coin"></i>
                    </span>
                    <span class="announcement-item__body">
                        <span class="announcement-item__title">&#xE41;&#xE08;&#xE49;&#xE07;&#xE01;&#xE32;&#xE23;&#xE08;&#xE48;&#xE32;&#xE22;&#xE40;&#xE07;&#xE34;&#xE19;&#xE1B;&#xE31;&#xE19;&#xE1C;&#xE25;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19; ASP-DIGITAL</span>
                        <time class="announcement-item__date" datetime="2569-05-23">
                            23 &#xE1E;.&#xE04;. 2569
                        </time>
                    </span>
                </a>
            </li>
            <li>
                <a href="/media/documents/sample.pdf" class="announcement-item" target="_blank" rel="noopener"
                   aria-label="&#xE23;&#xE32;&#xE07;&#xE27;&#xE31;&#xE25;&#xE41;&#xE25;&#xE30;&#xE04;&#xE27;&#xE32;&#xE21;&#xE2A;&#xE33;&#xE40;&#xE23;&#xE47;&#xE08;: &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE04;&#xE27;&#xE49;&#xE32;&#xE23;&#xE32;&#xE07;&#xE27;&#xE31;&#xE25;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE22;&#xE2D;&#xE14;&#xE40;&#xE22;&#xE35;&#xE48;&#xE22;&#xE21;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE20;&#xE17;&#xE15;&#xE23;&#xE32;&#xE2A;&#xE32;&#xE23;&#xE17;&#xE38;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28; (ไฟล์ PDF, เปิดในแท็บใหม่)">
                    <span class="announcement-item__icon" aria-hidden="true">
                        <i class="bi bi-trophy"></i>
                    </span>
                    <span class="announcement-item__body">
                        <span class="announcement-item__title">&#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE04;&#xE27;&#xE49;&#xE32;&#xE23;&#xE32;&#xE07;&#xE27;&#xE31;&#xE25;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE22;&#xE2D;&#xE14;&#xE40;&#xE22;&#xE35;&#xE48;&#xE22;&#xE21;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE20;&#xE17;&#xE15;&#xE23;&#xE32;&#xE2A;&#xE32;&#xE23;&#xE17;&#xE38;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
                        <time class="announcement-item__date" datetime="2569-05-20">
                            20 &#xE1E;.&#xE04;. 2569
                        </time>
                    </span>
                </a>
            </li>
            <li>
                <a href="/media/documents/sample.pdf" class="announcement-item" target="_blank" rel="noopener"
                   aria-label="&#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE40;&#xE1B;&#xE34;&#xE14;&#xE40;&#xE2A;&#xE19;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;: &#xE40;&#xE1B;&#xE34;&#xE14;&#xE40;&#xE2A;&#xE19;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE2B;&#xE21;&#xE48; A-HUMANOID (ไฟล์ PDF, เปิดในแท็บใหม่)">
                    <span class="announcement-item__icon" aria-hidden="true">
                        <i class="bi bi-bar-chart-line"></i>
                    </span>
                    <span class="announcement-item__body">
                        <span class="announcement-item__title">&#xE40;&#xE1B;&#xE34;&#xE14;&#xE40;&#xE2A;&#xE19;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE2B;&#xE21;&#xE48; A-HUMANOID</span>
                        <time class="announcement-item__date" datetime="2569-05-18">
                            18 &#xE1E;.&#xE04;. 2569
                        </time>
                    </span>
                </a>
            </li>
    </ul>

    <div class="insight-col__footer">
        <a href="/news-announcements" class="btn btn-split btn-light" aria-label="ดูประกาศทั้งหมด">
            <span>ดูทั้งหมด</span>
            <span class="btn-split__icon">
                <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
            </span>
        </a>
    </div>
</section>

        </div>
    </div>
</section>', N'Insights', 0);
INSERT INTO [2026_web_widget] (created_at, updated_at, created_by, updated_by, sort, status, pb_status, approve_by, show_front, cat_id, title, img1, mod_name, info, section_key, pb_cat_id, pb_title, pb_img1, pb_mod_name, pb_info, pb_section_key, web_id) VALUES (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'user', N'user', 60, 1, 1, N'user', 1, @gid, N'ตัวแทนขาย', N'Files/Site0/1/widget_icons/assetplus/icon-Distributors.png', N'', N'<section class="section distributors">
    <div class="container-fluid">
        <div class="distributors__head">
            <h2 class="distributors__title">
                ซื้อกองทุน <strong class="distributors__brand">Asset Plus</strong> ได้ผ่านตัวแทนขายชั้นนำ
            </h2>
        </div>

        <ul class="distributor-grid">
                <li class="distributor-grid__item">
                        <a href="https://www.scb.co.th" class="distributor" target="_blank" rel="noopener"
                           aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE44;&#xE17;&#xE22;&#xE1E;&#xE32;&#xE13;&#xE34;&#xE0A;&#xE22;&#xE4C; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                            <span class="distributor__logo">
                                <img class="distributor__img" src="/media/images/agent/Thumb-0.jpg" alt="" loading="lazy" decoding="async" />
                            </span>
                            <span class="distributor__name">SCB</span>
                        </a>
                </li>
                <li class="distributor-grid__item">
                        <a href="https://www.kasikornbank.com" class="distributor" target="_blank" rel="noopener"
                           aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE01;&#xE2A;&#xE34;&#xE01;&#xE23;&#xE44;&#xE17;&#xE22; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                            <span class="distributor__logo">
                                <img class="distributor__img" src="/media/images/agent/Thumb-1.jpg" alt="" loading="lazy" decoding="async" />
                            </span>
                            <span class="distributor__name">KBank</span>
                        </a>
                </li>
                <li class="distributor-grid__item">
                        <a href="https://www.gsb.or.th" class="distributor" target="_blank" rel="noopener"
                           aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE2D;&#xE2D;&#xE21;&#xE2A;&#xE34;&#xE19; (เปิดในแท็บใหม่)">
                            <span class="distributor__logo">
                                <img class="distributor__img" src="/media/images/agent/Thumb-2.jpg" alt="" loading="lazy" decoding="async" />
                            </span>
                            <span class="distributor__name">Government Savings Bank</span>
                        </a>
                </li>
                <li class="distributor-grid__item">
                        <a href="https://www.krungsri.com" class="distributor" target="_blank" rel="noopener"
                           aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE01;&#xE23;&#xE38;&#xE07;&#xE28;&#xE23;&#xE35;&#xE2D;&#xE22;&#xE38;&#xE18;&#xE22;&#xE32; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                            <span class="distributor__logo">
                                <img class="distributor__img" src="/media/images/agent/Thumb-3.jpg" alt="" loading="lazy" decoding="async" />
                            </span>
                            <span class="distributor__name">krungsri</span>
                        </a>
                </li>
                <li class="distributor-grid__item">
                        <a href="https://www.bangkokbank.com" class="distributor" target="_blank" rel="noopener"
                           aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE01;&#xE23;&#xE38;&#xE07;&#xE40;&#xE17;&#xE1E; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                            <span class="distributor__logo">
                                <img class="distributor__img" src="/media/images/agent/Thumb-4.jpg" alt="" loading="lazy" decoding="async" />
                            </span>
                            <span class="distributor__name">Bangkok Bank</span>
                        </a>
                </li>
                <li class="distributor-grid__item">
                        <a href="https://www.ttbbank.com" class="distributor" target="_blank" rel="noopener"
                           aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE17;&#xE2B;&#xE32;&#xE23;&#xE44;&#xE17;&#xE22;&#xE18;&#xE19;&#xE0A;&#xE32;&#xE15; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                            <span class="distributor__logo">
                                <img class="distributor__img" src="/media/images/agent/Thumb-15.jpg" alt="" loading="lazy" decoding="async" />
                            </span>
                            <span class="distributor__name">ttb</span>
                        </a>
                </li>
                <li class="distributor-grid__item">
                        <a href="https://www.cimbthai.com" class="distributor" target="_blank" rel="noopener"
                           aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE0B;&#xE35;&#xE44;&#xE2D;&#xE40;&#xE2D;&#xE47;&#xE21;&#xE1A;&#xE35; &#xE44;&#xE17;&#xE22; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                            <span class="distributor__logo">
                                <img class="distributor__img" src="/media/images/agent/Thumb-5.jpg" alt="" loading="lazy" decoding="async" />
                            </span>
                            <span class="distributor__name">CIMB</span>
                        </a>
                </li>
                <li class="distributor-grid__item">
                        <a href="https://www.uob.co.th" class="distributor" target="_blank" rel="noopener"
                           aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE22;&#xE39;&#xE42;&#xE2D;&#xE1A;&#xE35; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                            <span class="distributor__logo">
                                <img class="distributor__img" src="/media/images/agent/Thumb-6.jpg" alt="" loading="lazy" decoding="async" />
                            </span>
                            <span class="distributor__name">UOB</span>
                        </a>
                </li>
                <li class="distributor-grid__item">
                        <a href="https://www.lhbank.co.th" class="distributor" target="_blank" rel="noopener"
                           aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE41;&#xE25;&#xE19;&#xE14;&#xE4C; &#xE41;&#xE2D;&#xE19;&#xE14;&#xE4C; &#xE40;&#xE2E;&#xE49;&#xE32;&#xE2A;&#xE4C; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                            <span class="distributor__logo">
                                <img class="distributor__img" src="/media/images/agent/Thumb-7.jpg" alt="" loading="lazy" decoding="async" />
                            </span>
                            <span class="distributor__name">LH Bank</span>
                        </a>
                </li>
                <li class="distributor-grid__item">
                        <a href="https://bank.kkpfg.com" class="distributor" target="_blank" rel="noopener"
                           aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE40;&#xE01;&#xE35;&#xE22;&#xE23;&#xE15;&#xE34;&#xE19;&#xE32;&#xE04;&#xE34;&#xE19;&#xE20;&#xE31;&#xE17;&#xE23; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                            <span class="distributor__logo">
                                <img class="distributor__img" src="/media/images/agent/Thumb-8.jpg" alt="" loading="lazy" decoding="async" />
                            </span>
                            <span class="distributor__name">KKP</span>
                        </a>
                </li>
                <li class="distributor-grid__item">
                        <a href="https://www.fnsyrus.com" class="distributor" target="_blank" rel="noopener"
                           aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE1F;&#xE34;&#xE19;&#xE31;&#xE19;&#xE40;&#xE0B;&#xE35;&#xE22; &#xE44;&#xE0B;&#xE23;&#xE31;&#xE2A; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                            <span class="distributor__logo">
                                <img class="distributor__img" src="/media/images/agent/Thumb-9.jpg" alt="" loading="lazy" decoding="async" />
                            </span>
                            <span class="distributor__name">FINANSIA</span>
                        </a>
                </li>
                <li class="distributor-grid__item">
                        <a href="https://www.kgieworld.co.th" class="distributor" target="_blank" rel="noopener"
                           aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE40;&#xE04;&#xE08;&#xE35;&#xE44;&#xE2D; (&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;&#xE44;&#xE17;&#xE22;) &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                            <span class="distributor__logo">
                                <img class="distributor__img" src="/media/images/agent/Thumb-10.jpg" alt="" loading="lazy" decoding="async" />
                            </span>
                            <span class="distributor__name">KGI</span>
                        </a>
                </li>
                <li class="distributor-grid__item">
                        <a href="https://www.aira.co.th" class="distributor" target="_blank" rel="noopener"
                           aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE44;&#xE2D;&#xE23;&#xE48;&#xE32; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                            <span class="distributor__logo">
                                <img class="distributor__img" src="/media/images/agent/Thumb-11.jpg" alt="" loading="lazy" decoding="async" />
                            </span>
                            <span class="distributor__name">AIRA</span>
                        </a>
                </li>
                <li class="distributor-grid__item">
                        <a href="https://www.tisco.co.th" class="distributor" target="_blank" rel="noopener"
                           aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE17;&#xE34;&#xE2A;&#xE42;&#xE01;&#xE49; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (เปิดในแท็บใหม่)">
                            <span class="distributor__logo">
                                <img class="distributor__img" src="/media/images/agent/Thumb-12.jpg" alt="" loading="lazy" decoding="async" />
                            </span>
                            <span class="distributor__name">TISCO</span>
                        </a>
                </li>
                <li class="distributor-grid__item">
                        <a href="https://www.asiaplus.co.th" class="distributor" target="_blank" rel="noopener"
                           aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE40;&#xE2D;&#xE40;&#xE0B;&#xE35;&#xE22; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (เปิดในแท็บใหม่)">
                            <span class="distributor__logo">
                                <img class="distributor__img" src="/media/images/agent/Thumb-13.jpg" alt="" loading="lazy" decoding="async" />
                            </span>
                            <span class="distributor__name">ASIA PLUS Security</span>
                        </a>
                </li>
                <li class="distributor-grid__item">
                        <a href="https://www.dime.co.th" class="distributor" target="_blank" rel="noopener"
                           aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE40;&#xE14;&#xE1F; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (Dime!) (เปิดในแท็บใหม่)">
                            <span class="distributor__logo">
                                <img class="distributor__img" src="/media/images/agent/Thumb-14.jpg" alt="" loading="lazy" decoding="async" />
                            </span>
                            <span class="distributor__name">Dime</span>
                        </a>
                </li>
        </ul>

        <div class="distributors__footer">
            <a href="/services/distributors" class="btn btn-split btn-light" aria-label="ดูตัวแทนขายทั้งหมด">
                <span>ดูทั้งหมด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>
    </div>
</section>', N'Distributors', CAST(@gid AS nvarchar(20)), N'ตัวแทนขาย', N'Files/Site0/1/widget_icons/assetplus/icon-Distributors.png', N'', N'<section class="section distributors">
    <div class="container-fluid">
        <div class="distributors__head">
            <h2 class="distributors__title">
                ซื้อกองทุน <strong class="distributors__brand">Asset Plus</strong> ได้ผ่านตัวแทนขายชั้นนำ
            </h2>
        </div>

        <ul class="distributor-grid">
                <li class="distributor-grid__item">
                        <a href="https://www.scb.co.th" class="distributor" target="_blank" rel="noopener"
                           aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE44;&#xE17;&#xE22;&#xE1E;&#xE32;&#xE13;&#xE34;&#xE0A;&#xE22;&#xE4C; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                            <span class="distributor__logo">
                                <img class="distributor__img" src="/media/images/agent/Thumb-0.jpg" alt="" loading="lazy" decoding="async" />
                            </span>
                            <span class="distributor__name">SCB</span>
                        </a>
                </li>
                <li class="distributor-grid__item">
                        <a href="https://www.kasikornbank.com" class="distributor" target="_blank" rel="noopener"
                           aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE01;&#xE2A;&#xE34;&#xE01;&#xE23;&#xE44;&#xE17;&#xE22; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                            <span class="distributor__logo">
                                <img class="distributor__img" src="/media/images/agent/Thumb-1.jpg" alt="" loading="lazy" decoding="async" />
                            </span>
                            <span class="distributor__name">KBank</span>
                        </a>
                </li>
                <li class="distributor-grid__item">
                        <a href="https://www.gsb.or.th" class="distributor" target="_blank" rel="noopener"
                           aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE2D;&#xE2D;&#xE21;&#xE2A;&#xE34;&#xE19; (เปิดในแท็บใหม่)">
                            <span class="distributor__logo">
                                <img class="distributor__img" src="/media/images/agent/Thumb-2.jpg" alt="" loading="lazy" decoding="async" />
                            </span>
                            <span class="distributor__name">Government Savings Bank</span>
                        </a>
                </li>
                <li class="distributor-grid__item">
                        <a href="https://www.krungsri.com" class="distributor" target="_blank" rel="noopener"
                           aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE01;&#xE23;&#xE38;&#xE07;&#xE28;&#xE23;&#xE35;&#xE2D;&#xE22;&#xE38;&#xE18;&#xE22;&#xE32; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                            <span class="distributor__logo">
                                <img class="distributor__img" src="/media/images/agent/Thumb-3.jpg" alt="" loading="lazy" decoding="async" />
                            </span>
                            <span class="distributor__name">krungsri</span>
                        </a>
                </li>
                <li class="distributor-grid__item">
                        <a href="https://www.bangkokbank.com" class="distributor" target="_blank" rel="noopener"
                           aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE01;&#xE23;&#xE38;&#xE07;&#xE40;&#xE17;&#xE1E; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                            <span class="distributor__logo">
                                <img class="distributor__img" src="/media/images/agent/Thumb-4.jpg" alt="" loading="lazy" decoding="async" />
                            </span>
                            <span class="distributor__name">Bangkok Bank</span>
                        </a>
                </li>
                <li class="distributor-grid__item">
                        <a href="https://www.ttbbank.com" class="distributor" target="_blank" rel="noopener"
                           aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE17;&#xE2B;&#xE32;&#xE23;&#xE44;&#xE17;&#xE22;&#xE18;&#xE19;&#xE0A;&#xE32;&#xE15; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                            <span class="distributor__logo">
                                <img class="distributor__img" src="/media/images/agent/Thumb-15.jpg" alt="" loading="lazy" decoding="async" />
                            </span>
                            <span class="distributor__name">ttb</span>
                        </a>
                </li>
                <li class="distributor-grid__item">
                        <a href="https://www.cimbthai.com" class="distributor" target="_blank" rel="noopener"
                           aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE0B;&#xE35;&#xE44;&#xE2D;&#xE40;&#xE2D;&#xE47;&#xE21;&#xE1A;&#xE35; &#xE44;&#xE17;&#xE22; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                            <span class="distributor__logo">
                                <img class="distributor__img" src="/media/images/agent/Thumb-5.jpg" alt="" loading="lazy" decoding="async" />
                            </span>
                            <span class="distributor__name">CIMB</span>
                        </a>
                </li>
                <li class="distributor-grid__item">
                        <a href="https://www.uob.co.th" class="distributor" target="_blank" rel="noopener"
                           aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE22;&#xE39;&#xE42;&#xE2D;&#xE1A;&#xE35; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                            <span class="distributor__logo">
                                <img class="distributor__img" src="/media/images/agent/Thumb-6.jpg" alt="" loading="lazy" decoding="async" />
                            </span>
                            <span class="distributor__name">UOB</span>
                        </a>
                </li>
                <li class="distributor-grid__item">
                        <a href="https://www.lhbank.co.th" class="distributor" target="_blank" rel="noopener"
                           aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE41;&#xE25;&#xE19;&#xE14;&#xE4C; &#xE41;&#xE2D;&#xE19;&#xE14;&#xE4C; &#xE40;&#xE2E;&#xE49;&#xE32;&#xE2A;&#xE4C; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                            <span class="distributor__logo">
                                <img class="distributor__img" src="/media/images/agent/Thumb-7.jpg" alt="" loading="lazy" decoding="async" />
                            </span>
                            <span class="distributor__name">LH Bank</span>
                        </a>
                </li>
                <li class="distributor-grid__item">
                        <a href="https://bank.kkpfg.com" class="distributor" target="_blank" rel="noopener"
                           aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE40;&#xE01;&#xE35;&#xE22;&#xE23;&#xE15;&#xE34;&#xE19;&#xE32;&#xE04;&#xE34;&#xE19;&#xE20;&#xE31;&#xE17;&#xE23; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                            <span class="distributor__logo">
                                <img class="distributor__img" src="/media/images/agent/Thumb-8.jpg" alt="" loading="lazy" decoding="async" />
                            </span>
                            <span class="distributor__name">KKP</span>
                        </a>
                </li>
                <li class="distributor-grid__item">
                        <a href="https://www.fnsyrus.com" class="distributor" target="_blank" rel="noopener"
                           aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE1F;&#xE34;&#xE19;&#xE31;&#xE19;&#xE40;&#xE0B;&#xE35;&#xE22; &#xE44;&#xE0B;&#xE23;&#xE31;&#xE2A; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                            <span class="distributor__logo">
                                <img class="distributor__img" src="/media/images/agent/Thumb-9.jpg" alt="" loading="lazy" decoding="async" />
                            </span>
                            <span class="distributor__name">FINANSIA</span>
                        </a>
                </li>
                <li class="distributor-grid__item">
                        <a href="https://www.kgieworld.co.th" class="distributor" target="_blank" rel="noopener"
                           aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE40;&#xE04;&#xE08;&#xE35;&#xE44;&#xE2D; (&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;&#xE44;&#xE17;&#xE22;) &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                            <span class="distributor__logo">
                                <img class="distributor__img" src="/media/images/agent/Thumb-10.jpg" alt="" loading="lazy" decoding="async" />
                            </span>
                            <span class="distributor__name">KGI</span>
                        </a>
                </li>
                <li class="distributor-grid__item">
                        <a href="https://www.aira.co.th" class="distributor" target="_blank" rel="noopener"
                           aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE44;&#xE2D;&#xE23;&#xE48;&#xE32; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                            <span class="distributor__logo">
                                <img class="distributor__img" src="/media/images/agent/Thumb-11.jpg" alt="" loading="lazy" decoding="async" />
                            </span>
                            <span class="distributor__name">AIRA</span>
                        </a>
                </li>
                <li class="distributor-grid__item">
                        <a href="https://www.tisco.co.th" class="distributor" target="_blank" rel="noopener"
                           aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE17;&#xE34;&#xE2A;&#xE42;&#xE01;&#xE49; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (เปิดในแท็บใหม่)">
                            <span class="distributor__logo">
                                <img class="distributor__img" src="/media/images/agent/Thumb-12.jpg" alt="" loading="lazy" decoding="async" />
                            </span>
                            <span class="distributor__name">TISCO</span>
                        </a>
                </li>
                <li class="distributor-grid__item">
                        <a href="https://www.asiaplus.co.th" class="distributor" target="_blank" rel="noopener"
                           aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE40;&#xE2D;&#xE40;&#xE0B;&#xE35;&#xE22; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (เปิดในแท็บใหม่)">
                            <span class="distributor__logo">
                                <img class="distributor__img" src="/media/images/agent/Thumb-13.jpg" alt="" loading="lazy" decoding="async" />
                            </span>
                            <span class="distributor__name">ASIA PLUS Security</span>
                        </a>
                </li>
                <li class="distributor-grid__item">
                        <a href="https://www.dime.co.th" class="distributor" target="_blank" rel="noopener"
                           aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE40;&#xE14;&#xE1F; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (Dime!) (เปิดในแท็บใหม่)">
                            <span class="distributor__logo">
                                <img class="distributor__img" src="/media/images/agent/Thumb-14.jpg" alt="" loading="lazy" decoding="async" />
                            </span>
                            <span class="distributor__name">Dime</span>
                        </a>
                </li>
        </ul>

        <div class="distributors__footer">
            <a href="/services/distributors" class="btn btn-split btn-light" aria-label="ดูตัวแทนขายทั้งหมด">
                <span>ดูทั้งหมด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>
    </div>
</section>', N'Distributors', 0);
INSERT INTO [2026_web_widget_group] (created_at, updated_at, created_by, updated_by, sort, status, pb_status, approve_by, show_front, title, img1, pb_title, pb_img1, web_id) VALUES (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'user', N'user', 20, 1, 1, N'user', 1, N'MODERN', N'Files/Site0/1/widget_icons/assetplus/icon-group-modern.png', N'MODERN', N'Files/Site0/1/widget_icons/assetplus/icon-group-modern.png', 0);
SET @gid = SCOPE_IDENTITY(); INSERT INTO @g (n, id) VALUES (2, @gid);
INSERT INTO [2026_web_widget] (created_at, updated_at, created_by, updated_by, sort, status, pb_status, approve_by, show_front, cat_id, title, img1, mod_name, info, section_key, pb_cat_id, pb_title, pb_img1, pb_mod_name, pb_info, pb_section_key, web_id) VALUES (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'user', N'user', 10, 1, 1, N'user', 1, @gid, N'แบนเนอร์หน้าแรก', N'Files/Site0/1/widget_icons/assetplus/icon-Hero.png', N'HomeImageSlide', N'<section class="section hero hero--v2" aria-label="แบนเนอร์ไฮไลต์">
<div class="container-fluid">
<div class="swiper hero__slider js-hero-slider">
<div class="swiper-wrapper">
|||REPEAT|||<div class="swiper-slide hero__slide" data-slide-id="|||id|||"><a href="|||pb_url|||" class="hero__link" target="_top"><picture><source media="(max-width: 767.98px)" srcset="/|||pb_img1_icon|||" /><img src="/|||pb_img1|||" alt="|||pb_title|||" class="hero__image" loading="lazy" decoding="async" /></picture></a></div>|||/REPEAT|||
</div>
<div class="hero__controls hero__controls--overlay">
<div class="hero__tabs js-hero-pagination"></div>
<button type="button" class="hero__mini-toggle js-hero-toggle" aria-pressed="false" aria-label="หยุดสไลด์ชั่วคราว"><i class="bi bi-play-fill" aria-hidden="true"></i></button>
</div>
</div>
</div>
</section>', N'HeroV2', CAST(@gid AS nvarchar(20)), N'แบนเนอร์หน้าแรก', N'Files/Site0/1/widget_icons/assetplus/icon-Hero.png', N'HomeImageSlide', N'<section class="section hero hero--v2" aria-label="แบนเนอร์ไฮไลต์">
<div class="container-fluid">
<div class="swiper hero__slider js-hero-slider">
<div class="swiper-wrapper">
|||REPEAT|||<div class="swiper-slide hero__slide" data-slide-id="|||id|||"><a href="|||pb_url|||" class="hero__link" target="_top"><picture><source media="(max-width: 767.98px)" srcset="/|||pb_img1_icon|||" /><img src="/|||pb_img1|||" alt="|||pb_title|||" class="hero__image" loading="lazy" decoding="async" /></picture></a></div>|||/REPEAT|||
</div>
<div class="hero__controls hero__controls--overlay">
<div class="hero__tabs js-hero-pagination"></div>
<button type="button" class="hero__mini-toggle js-hero-toggle" aria-pressed="false" aria-label="หยุดสไลด์ชั่วคราว"><i class="bi bi-play-fill" aria-hidden="true"></i></button>
</div>
</div>
</div>
</section>', N'HeroV2', 0);
INSERT INTO [2026_web_widget] (created_at, updated_at, created_by, updated_by, sort, status, pb_status, approve_by, show_front, cat_id, title, img1, mod_name, info, section_key, pb_cat_id, pb_title, pb_img1, pb_mod_name, pb_info, pb_section_key, web_id) VALUES (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'user', N'user', 20, 1, 1, N'user', 1, @gid, N'มูลค่าหน่วยลงทุน', N'Files/Site0/1/widget_icons/assetplus/icon-NavPrices.png', N'', N'<section class="section nav-prices nav-prices--v2">
    <div class="container">
        <div class="row g-4">
            <!-- ตาราง NAV ย่อ — รูปแบบเดียวกับ v1 -->
            <div class="col-12 col-lg-6">
                <div class="nav-prices__card">
                    <div class="section-head">
                        <h2 class="section-head__title">มูลค่าหน่วยลงทุน</h2>
                        <a href="/funds/nav" class="btn btn-split btn-light" aria-label="ดูมูลค่าหน่วยลงทุนทั้งหมด">
                            <span>ดูทั้งหมด</span>
                            <span class="btn-split__icon">
                                <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                            </span>
                        </a>
                    </div>

                    <div class="nav-prices__table-wrap">
                        <table class="nav-table js-row-links">
                            <caption class="visually-hidden">
                                มูลค่าหน่วยลงทุนของกองทุนแนะนำ ข้อมูล ณ วันที่ 16 &#xE01;&#xE31;&#xE19;&#xE22;&#xE32;&#xE22;&#xE19; 2569
                            </caption>
                            <thead>
                                <tr>
                                    <th scope="col">กองทุน</th>
                                    <th scope="col">NAV (บาท)</th>
                                    <th scope="col" class="text-end">เปลี่ยนแปลง</th>
                                </tr>
                            </thead>
                            <tbody>
                                    <tr class="nav-table__row js-row-link" data-href="/funds/a-humanoid">
                                        <th scope="row" class="nav-table__code">
                                            <a href="/funds/a-humanoid" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2E;&#xE34;&#xE27;&#xE41;&#xE21;&#xE19;&#xE19;&#xE2D;&#xE22;&#xE14;&#xE4C;">A-HUMANOID</a>
                                        </th>
                                        <td class="nav-table__nav">11.5594</td>
                                        <td class="nav-table__change is-up">
                                            <i class="bi bi-caret-up-fill" aria-hidden="true"></i>
                                            <span>&#x2B;1.18%</span>
                                        </td>
                                    </tr>
                                    <tr class="nav-table__row js-row-link" data-href="/funds/a-grid">
                                        <th scope="row" class="nav-table__code">
                                            <a href="/funds/a-grid" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE01;&#xE23;&#xE34;&#xE14; &#xE2D;&#xE34;&#xE19;&#xE1F;&#xE23;&#xE32;&#xE2A;&#xE15;&#xE23;&#xE31;&#xE04;&#xE40;&#xE08;&#xE2D;&#xE23;&#xE4C;">A-GRID</a>
                                        </th>
                                        <td class="nav-table__nav">12.4180</td>
                                        <td class="nav-table__change is-up">
                                            <i class="bi bi-caret-up-fill" aria-hidden="true"></i>
                                            <span>&#x2B;0.50%</span>
                                        </td>
                                    </tr>
                                    <tr class="nav-table__row js-row-link" data-href="/funds/a-asemi">
                                        <th scope="row" class="nav-table__code">
                                            <a href="/funds/a-asemi" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE40;&#xE2D;&#xE40;&#xE0A;&#xE35;&#xE22; &#xE40;&#xE0B;&#xE21;&#xE34;&#xE04;&#xE2D;&#xE19;&#xE14;&#xE31;&#xE01;&#xE40;&#xE15;&#xE2D;&#xE23;&#xE4C;">A-ASEMI</a>
                                        </th>
                                        <td class="nav-table__nav">14.2075</td>
                                        <td class="nav-table__change is-up">
                                            <i class="bi bi-caret-up-fill" aria-hidden="true"></i>
                                            <span>&#x2B;1.38%</span>
                                        </td>
                                    </tr>
                                    <tr class="nav-table__row js-row-link" data-href="/funds/a-jedi">
                                        <th scope="row" class="nav-table__code">
                                            <a href="/funds/a-jedi" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2A;&#xE40;&#xE1B;&#xE0B; &#xE2D;&#xE35;&#xE42;&#xE04;&#xE42;&#xE19;&#xE21;&#xE35;">A-JEDI</a>
                                        </th>
                                        <td class="nav-table__nav">9.8742</td>
                                        <td class="nav-table__change is-down">
                                            <i class="bi bi-caret-down-fill" aria-hidden="true"></i>
                                            <span>-0.46%</span>
                                        </td>
                                    </tr>
                                    <tr class="nav-table__row js-row-link" data-href="/funds/asp-crypto">
                                        <th scope="row" class="nav-table__code">
                                            <a href="/funds/asp-crypto" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE14;&#xE34;&#xE08;&#xE34;&#xE17;&#xE31;&#xE25; &#xE1A;&#xE25;&#xE47;&#xE2D;&#xE01;&#xE40;&#xE0A;&#xE19;">ASP-CRYPTO</a>
                                        </th>
                                        <td class="nav-table__nav">15.8410</td>
                                        <td class="nav-table__change is-up">
                                            <i class="bi bi-caret-up-fill" aria-hidden="true"></i>
                                            <span>&#x2B;1.55%</span>
                                        </td>
                                    </tr>
                            </tbody>
                        </table>
                    </div>

                    <p class="nav-prices__asof">
                        ข้อมูล ณ วันที่ 16 &#xE01;&#xE31;&#xE19;&#xE22;&#xE32;&#xE22;&#xE19; 2569
                        เวลา 18:00 น.
                    </p>
                </div>
            </div>
            <div class="col-12 col-lg-6">
                <!-- Quick tiles — แถบแนวนอน 3 อัน เรียงลงมา -->
                <div class="quick-bars">
                    <a href="/funds/performance" class="quick-bar">
                        <span class="quick-bar__thumb">
                            <img src="/media/images/home/tiles/performance.jpg" alt="" loading="lazy"
                                 decoding="async" />
                        </span>
                        <span class="quick-bar__body">
                            <span class="quick-bar__title">ผลการดำเนินงานทั้งหมด</span>
                            <span class="quick-bar__text">เปรียบเทียบผลตอบแทนย้อนหลังของแต่ละกองทุน</span>
                        </span>
                        <i class="bi bi-arrow-right-short quick-bar__icon" aria-hidden="true"></i>
                    </a>

                    <a href="/funds/nav" class="quick-bar">
                        <span class="quick-bar__thumb">
                            <img src="/media/images/home/tiles/nav.jpg" alt="" loading="lazy" decoding="async" />
                        </span>
                        <span class="quick-bar__body">
                            <span class="quick-bar__title">มูลค่าหน่วยลงทุนทั้งหมด</span>
                            <span class="quick-bar__text">ค้นหามูลค่าหน่วยลงทุน (NAV) ย้อนหลังของทุกกองทุน</span>
                        </span>
                        <i class="bi bi-arrow-right-short quick-bar__icon" aria-hidden="true"></i>
                    </a>

                    <a href="/funds/calendar" class="quick-bar">
                        <span class="quick-bar__thumb">
                            <img src="/media/images/home/tiles/calendar.jpg" alt="" loading="lazy" decoding="async" />
                        </span>
                        <span class="quick-bar__body">
                            <span class="quick-bar__title">ปฏิทินกองทุน</span>
                            <span class="quick-bar__text">ตารางวันทำการซื้อขายและวันหยุดของแต่ละกองทุน</span>
                        </span>
                        <i class="bi bi-arrow-right-short quick-bar__icon" aria-hidden="true"></i>
                    </a>
                </div>
            </div>
        </div>
    </div>
</section>', N'NavPricesV2', CAST(@gid AS nvarchar(20)), N'มูลค่าหน่วยลงทุน', N'Files/Site0/1/widget_icons/assetplus/icon-NavPrices.png', N'', N'<section class="section nav-prices nav-prices--v2">
    <div class="container">
        <div class="row g-4">
            <!-- ตาราง NAV ย่อ — รูปแบบเดียวกับ v1 -->
            <div class="col-12 col-lg-6">
                <div class="nav-prices__card">
                    <div class="section-head">
                        <h2 class="section-head__title">มูลค่าหน่วยลงทุน</h2>
                        <a href="/funds/nav" class="btn btn-split btn-light" aria-label="ดูมูลค่าหน่วยลงทุนทั้งหมด">
                            <span>ดูทั้งหมด</span>
                            <span class="btn-split__icon">
                                <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                            </span>
                        </a>
                    </div>

                    <div class="nav-prices__table-wrap">
                        <table class="nav-table js-row-links">
                            <caption class="visually-hidden">
                                มูลค่าหน่วยลงทุนของกองทุนแนะนำ ข้อมูล ณ วันที่ 16 &#xE01;&#xE31;&#xE19;&#xE22;&#xE32;&#xE22;&#xE19; 2569
                            </caption>
                            <thead>
                                <tr>
                                    <th scope="col">กองทุน</th>
                                    <th scope="col">NAV (บาท)</th>
                                    <th scope="col" class="text-end">เปลี่ยนแปลง</th>
                                </tr>
                            </thead>
                            <tbody>
                                    <tr class="nav-table__row js-row-link" data-href="/funds/a-humanoid">
                                        <th scope="row" class="nav-table__code">
                                            <a href="/funds/a-humanoid" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2E;&#xE34;&#xE27;&#xE41;&#xE21;&#xE19;&#xE19;&#xE2D;&#xE22;&#xE14;&#xE4C;">A-HUMANOID</a>
                                        </th>
                                        <td class="nav-table__nav">11.5594</td>
                                        <td class="nav-table__change is-up">
                                            <i class="bi bi-caret-up-fill" aria-hidden="true"></i>
                                            <span>&#x2B;1.18%</span>
                                        </td>
                                    </tr>
                                    <tr class="nav-table__row js-row-link" data-href="/funds/a-grid">
                                        <th scope="row" class="nav-table__code">
                                            <a href="/funds/a-grid" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE01;&#xE23;&#xE34;&#xE14; &#xE2D;&#xE34;&#xE19;&#xE1F;&#xE23;&#xE32;&#xE2A;&#xE15;&#xE23;&#xE31;&#xE04;&#xE40;&#xE08;&#xE2D;&#xE23;&#xE4C;">A-GRID</a>
                                        </th>
                                        <td class="nav-table__nav">12.4180</td>
                                        <td class="nav-table__change is-up">
                                            <i class="bi bi-caret-up-fill" aria-hidden="true"></i>
                                            <span>&#x2B;0.50%</span>
                                        </td>
                                    </tr>
                                    <tr class="nav-table__row js-row-link" data-href="/funds/a-asemi">
                                        <th scope="row" class="nav-table__code">
                                            <a href="/funds/a-asemi" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE40;&#xE2D;&#xE40;&#xE0A;&#xE35;&#xE22; &#xE40;&#xE0B;&#xE21;&#xE34;&#xE04;&#xE2D;&#xE19;&#xE14;&#xE31;&#xE01;&#xE40;&#xE15;&#xE2D;&#xE23;&#xE4C;">A-ASEMI</a>
                                        </th>
                                        <td class="nav-table__nav">14.2075</td>
                                        <td class="nav-table__change is-up">
                                            <i class="bi bi-caret-up-fill" aria-hidden="true"></i>
                                            <span>&#x2B;1.38%</span>
                                        </td>
                                    </tr>
                                    <tr class="nav-table__row js-row-link" data-href="/funds/a-jedi">
                                        <th scope="row" class="nav-table__code">
                                            <a href="/funds/a-jedi" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2A;&#xE40;&#xE1B;&#xE0B; &#xE2D;&#xE35;&#xE42;&#xE04;&#xE42;&#xE19;&#xE21;&#xE35;">A-JEDI</a>
                                        </th>
                                        <td class="nav-table__nav">9.8742</td>
                                        <td class="nav-table__change is-down">
                                            <i class="bi bi-caret-down-fill" aria-hidden="true"></i>
                                            <span>-0.46%</span>
                                        </td>
                                    </tr>
                                    <tr class="nav-table__row js-row-link" data-href="/funds/asp-crypto">
                                        <th scope="row" class="nav-table__code">
                                            <a href="/funds/asp-crypto" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE14;&#xE34;&#xE08;&#xE34;&#xE17;&#xE31;&#xE25; &#xE1A;&#xE25;&#xE47;&#xE2D;&#xE01;&#xE40;&#xE0A;&#xE19;">ASP-CRYPTO</a>
                                        </th>
                                        <td class="nav-table__nav">15.8410</td>
                                        <td class="nav-table__change is-up">
                                            <i class="bi bi-caret-up-fill" aria-hidden="true"></i>
                                            <span>&#x2B;1.55%</span>
                                        </td>
                                    </tr>
                            </tbody>
                        </table>
                    </div>

                    <p class="nav-prices__asof">
                        ข้อมูล ณ วันที่ 16 &#xE01;&#xE31;&#xE19;&#xE22;&#xE32;&#xE22;&#xE19; 2569
                        เวลา 18:00 น.
                    </p>
                </div>
            </div>
            <div class="col-12 col-lg-6">
                <!-- Quick tiles — แถบแนวนอน 3 อัน เรียงลงมา -->
                <div class="quick-bars">
                    <a href="/funds/performance" class="quick-bar">
                        <span class="quick-bar__thumb">
                            <img src="/media/images/home/tiles/performance.jpg" alt="" loading="lazy"
                                 decoding="async" />
                        </span>
                        <span class="quick-bar__body">
                            <span class="quick-bar__title">ผลการดำเนินงานทั้งหมด</span>
                            <span class="quick-bar__text">เปรียบเทียบผลตอบแทนย้อนหลังของแต่ละกองทุน</span>
                        </span>
                        <i class="bi bi-arrow-right-short quick-bar__icon" aria-hidden="true"></i>
                    </a>

                    <a href="/funds/nav" class="quick-bar">
                        <span class="quick-bar__thumb">
                            <img src="/media/images/home/tiles/nav.jpg" alt="" loading="lazy" decoding="async" />
                        </span>
                        <span class="quick-bar__body">
                            <span class="quick-bar__title">มูลค่าหน่วยลงทุนทั้งหมด</span>
                            <span class="quick-bar__text">ค้นหามูลค่าหน่วยลงทุน (NAV) ย้อนหลังของทุกกองทุน</span>
                        </span>
                        <i class="bi bi-arrow-right-short quick-bar__icon" aria-hidden="true"></i>
                    </a>

                    <a href="/funds/calendar" class="quick-bar">
                        <span class="quick-bar__thumb">
                            <img src="/media/images/home/tiles/calendar.jpg" alt="" loading="lazy" decoding="async" />
                        </span>
                        <span class="quick-bar__body">
                            <span class="quick-bar__title">ปฏิทินกองทุน</span>
                            <span class="quick-bar__text">ตารางวันทำการซื้อขายและวันหยุดของแต่ละกองทุน</span>
                        </span>
                        <i class="bi bi-arrow-right-short quick-bar__icon" aria-hidden="true"></i>
                    </a>
                </div>
            </div>
        </div>
    </div>
</section>', N'NavPricesV2', 0);
INSERT INTO [2026_web_widget] (created_at, updated_at, created_by, updated_by, sort, status, pb_status, approve_by, show_front, cat_id, title, img1, mod_name, info, section_key, pb_cat_id, pb_title, pb_img1, pb_mod_name, pb_info, pb_section_key, web_id) VALUES (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'user', N'user', 30, 1, 1, N'user', 1, @gid, N'กองทุนแนะนำประจำเดือน', N'Files/Site0/1/widget_icons/assetplus/icon-FeaturedFunds.png', N'', N'<section class="section featured-funds featured-funds--v2">
    <div class="container">
        <div class="section-head">
            <h2 class="section-head__title">กองทุนแนะนำประจำเดือน</h2>
            <a href="/funds/featured" class="btn btn-split btn-light" aria-label="ดูกองทุนแนะนำทั้งหมด">
                <span>ดูทั้งหมด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>

        <ul class="fund-bento">
                <li class="fund-bento__cell">
                    <a href="/funds/a-humanoid" class="fund-bento__link" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2E;&#xE34;&#xE27;&#xE41;&#xE21;&#xE19;&#xE19;&#xE2D;&#xE22;&#xE14;&#xE4C;">
                        <span class="fund-bento__head">
                            <span class="fund-bento__code">A-HUMANOID</span>
                            <span class="fund-bento__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;/&#xE2B;&#xE38;&#xE49;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
                        </span>

                        <span class="fund-bento__summary">&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE19;&#xE18;&#xE38;&#xE23;&#xE01;&#xE34;&#xE08;&#xE2B;&#xE38;&#xE48;&#xE19;&#xE22;&#xE19;&#xE15;&#xE4C;&#xE2E;&#xE34;&#xE27;&#xE41;&#xE21;&#xE19;&#xE19;&#xE2D;&#xE22;&#xE14;&#xE4C;&#xE41;&#xE25;&#xE30; AI &#xE23;&#xE30;&#xE14;&#xE31;&#xE1A;&#xE42;&#xE25;&#xE01; &#xE17;&#xE35;&#xE48;&#xE01;&#xE33;&#xE25;&#xE31;&#xE07;&#xE40;&#xE1B;&#xE25;&#xE35;&#xE48;&#xE22;&#xE19;&#xE42;&#xE09;&#xE21;&#xE20;&#xE32;&#xE04;&#xE01;&#xE32;&#xE23;&#xE1C;&#xE25;&#xE34;&#xE15;&#xE41;&#xE25;&#xE30;&#xE1A;&#xE23;&#xE34;&#xE01;&#xE32;&#xE23;</span>

                        <span class="fund-bento__meta">
                            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--7">7</span>
</span>

                                
    <span class="rating">
        <img src="/media/images/morningstar/5-star.png" class="rating__image" alt="Morningstar Rating 5 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

                        </span>

                        <span class="btn btn-split btn-light">
                            <span>ดูรายละเอียด</span>
                            <span class="btn-split__icon">
                                <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                            </span>
                        </span>
                    </a>
                </li>
                <li class="fund-bento__cell">
                    <a href="/funds/a-grid" class="fund-bento__link" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE01;&#xE23;&#xE34;&#xE14; &#xE2D;&#xE34;&#xE19;&#xE1F;&#xE23;&#xE32;&#xE2A;&#xE15;&#xE23;&#xE31;&#xE04;&#xE40;&#xE08;&#xE2D;&#xE23;&#xE4C;">
                        <span class="fund-bento__head">
                            <span class="fund-bento__code">A-GRID</span>
                            <span class="fund-bento__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;/&#xE2B;&#xE38;&#xE49;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
                        </span>

                        <span class="fund-bento__summary">&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE19;&#xE42;&#xE04;&#xE23;&#xE07;&#xE2A;&#xE23;&#xE49;&#xE32;&#xE07;&#xE1E;&#xE37;&#xE49;&#xE19;&#xE10;&#xE32;&#xE19;&#xE14;&#xE49;&#xE32;&#xE19;&#xE1E;&#xE25;&#xE31;&#xE07;&#xE07;&#xE32;&#xE19;&#xE41;&#xE25;&#xE30;&#xE23;&#xE30;&#xE1A;&#xE1A; Smart Grid &#xE17;&#xE35;&#xE48;&#xE40;&#xE1B;&#xE47;&#xE19;&#xE2B;&#xE31;&#xE27;&#xE43;&#xE08;&#xE02;&#xE2D;&#xE07;&#xE42;&#xE25;&#xE01;&#xE2D;&#xE19;&#xE32;&#xE04;&#xE15;</span>

                        <span class="fund-bento__meta">
                            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--3">3</span>
</span>

                                
    <span class="rating">
        <img src="/media/images/morningstar/4-star.png" class="rating__image" alt="Morningstar Rating 4 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

                        </span>

                        <span class="btn btn-split btn-light">
                            <span>ดูรายละเอียด</span>
                            <span class="btn-split__icon">
                                <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                            </span>
                        </span>
                    </a>
                </li>
                <li class="fund-bento__cell">
                    <a href="/funds/a-asemi" class="fund-bento__link" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE40;&#xE2D;&#xE40;&#xE0A;&#xE35;&#xE22; &#xE40;&#xE0B;&#xE21;&#xE34;&#xE04;&#xE2D;&#xE19;&#xE14;&#xE31;&#xE01;&#xE40;&#xE15;&#xE2D;&#xE23;&#xE4C;">
                        <span class="fund-bento__head">
                            <span class="fund-bento__code">A-ASEMI</span>
                            <span class="fund-bento__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;/&#xE2B;&#xE38;&#xE49;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
                        </span>

                        <span class="fund-bento__summary">&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE19;&#xE2B;&#xE48;&#xE27;&#xE07;&#xE42;&#xE0B;&#xE48;&#xE01;&#xE32;&#xE23;&#xE1C;&#xE25;&#xE34;&#xE15;&#xE40;&#xE0B;&#xE21;&#xE34;&#xE04;&#xE2D;&#xE19;&#xE14;&#xE31;&#xE01;&#xE40;&#xE15;&#xE2D;&#xE23;&#xE4C;&#xE41;&#xE2B;&#xE48;&#xE07;&#xE40;&#xE2D;&#xE40;&#xE0A;&#xE35;&#xE22; &#xE15;&#xE31;&#xE49;&#xE07;&#xE41;&#xE15;&#xE48;&#xE15;&#xE49;&#xE19;&#xE19;&#xE49;&#xE33;&#xE16;&#xE36;&#xE07;&#xE1B;&#xE25;&#xE32;&#xE22;&#xE19;&#xE49;&#xE33;</span>

                        <span class="fund-bento__meta">
                            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--7">7</span>
</span>

                                
    <span class="rating">
        <img src="/media/images/morningstar/4-star.png" class="rating__image" alt="Morningstar Rating 4 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

                        </span>

                        <span class="btn btn-split btn-light">
                            <span>ดูรายละเอียด</span>
                            <span class="btn-split__icon">
                                <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                            </span>
                        </span>
                    </a>
                </li>
                <li class="fund-bento__cell">
                    <a href="/funds/a-jedi" class="fund-bento__link" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2A;&#xE40;&#xE1B;&#xE0B; &#xE2D;&#xE35;&#xE42;&#xE04;&#xE42;&#xE19;&#xE21;&#xE35;">
                        <span class="fund-bento__head">
                            <span class="fund-bento__code">A-JEDI</span>
                            <span class="fund-bento__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;/&#xE2B;&#xE38;&#xE49;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
                        </span>

                        <span class="fund-bento__summary">&#xE42;&#xE2D;&#xE01;&#xE32;&#xE2A;&#xE40;&#xE15;&#xE34;&#xE1A;&#xE42;&#xE15;&#xE08;&#xE32;&#xE01;&#xE2D;&#xE38;&#xE15;&#xE2A;&#xE32;&#xE2B;&#xE01;&#xE23;&#xE23;&#xE21;&#xE2D;&#xE27;&#xE01;&#xE32;&#xE28;&#xE41;&#xE25;&#xE30;&#xE14;&#xE32;&#xE27;&#xE40;&#xE17;&#xE35;&#xE22;&#xE21;&#xE17;&#xE35;&#xE48;&#xE02;&#xE22;&#xE32;&#xE22;&#xE15;&#xE31;&#xE27;&#xE15;&#xE48;&#xE2D;&#xE40;&#xE19;&#xE37;&#xE48;&#xE2D;&#xE07;</span>

                        <span class="fund-bento__meta">
                            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--7">7</span>
</span>

                                
    <span class="rating">
        <img src="/media/images/morningstar/5-star.png" class="rating__image" alt="Morningstar Rating 5 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

                        </span>

                        <span class="btn btn-split btn-light">
                            <span>ดูรายละเอียด</span>
                            <span class="btn-split__icon">
                                <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                            </span>
                        </span>
                    </a>
                </li>
                <li class="fund-bento__cell">
                    <a href="/funds/asp-crypto" class="fund-bento__link" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE14;&#xE34;&#xE08;&#xE34;&#xE17;&#xE31;&#xE25; &#xE1A;&#xE25;&#xE47;&#xE2D;&#xE01;&#xE40;&#xE0A;&#xE19;">
                        <span class="fund-bento__head">
                            <span class="fund-bento__code">ASP-CRYPTO</span>
                            <span class="fund-bento__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;/&#xE2B;&#xE38;&#xE49;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
                        </span>

                        <span class="fund-bento__summary">&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE19;&#xE18;&#xE38;&#xE23;&#xE01;&#xE34;&#xE08;&#xE41;&#xE1E;&#xE25;&#xE15;&#xE1F;&#xE2D;&#xE23;&#xE4C;&#xE21;&#xE14;&#xE34;&#xE08;&#xE34;&#xE17;&#xE31;&#xE25; &#xE41;&#xE25;&#xE30;&#xE40;&#xE17;&#xE04;&#xE42;&#xE19;&#xE42;&#xE25;&#xE22;&#xE35;&#xE1A;&#xE25;&#xE47;&#xE2D;&#xE01;&#xE40;&#xE0A;&#xE19;&#xE17;&#xE31;&#xE48;&#xE27;&#xE42;&#xE25;&#xE01;</span>

                        <span class="fund-bento__meta">
                            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--8">8&#x2B;</span>
</span>

                                
    <span class="rating">
        <img src="/media/images/morningstar/5-star.png" class="rating__image" alt="Morningstar Rating 5 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

                        </span>

                        <span class="btn btn-split btn-light">
                            <span>ดูรายละเอียด</span>
                            <span class="btn-split__icon">
                                <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                            </span>
                        </span>
                    </a>
                </li>
                <li class="fund-bento__cell">
                    <a href="/funds/a-ring" class="fund-bento__link" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE42;&#xE01;&#xE25;&#xE14;&#xE4C;">
                        <span class="fund-bento__head">
                            <span class="fund-bento__code">A-RING</span>
                            <span class="fund-bento__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C;&#xE2A;&#xE34;&#xE19;&#xE17;&#xE32;&#xE07;&#xE40;&#xE25;&#xE37;&#xE2D;&#xE01;</span>
                        </span>

                        <span class="fund-bento__summary">&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE19;&#xE17;&#xE2D;&#xE07;&#xE04;&#xE33; &#xE1B;&#xE49;&#xE2D;&#xE07;&#xE01;&#xE31;&#xE19;&#xE04;&#xE27;&#xE32;&#xE21;&#xE40;&#xE2A;&#xE35;&#xE48;&#xE22;&#xE07; &#xE2A;&#xE23;&#xE49;&#xE32;&#xE07;&#xE2A;&#xE21;&#xE14;&#xE38;&#xE25;&#xE1E;&#xE2D;&#xE23;&#xE4C;&#xE15;&#xE01;&#xE32;&#xE23;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;</span>

                        <span class="fund-bento__meta">
                            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--8">8</span>
</span>

                                
    <span class="rating">
        <img src="/media/images/morningstar/4-star.png" class="rating__image" alt="Morningstar Rating 4 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

                        </span>

                        <span class="btn btn-split btn-light">
                            <span>ดูรายละเอียด</span>
                            <span class="btn-split__icon">
                                <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                            </span>
                        </span>
                    </a>
                </li>
        </ul>
    </div>
</section>', N'FeaturedFundsV2', CAST(@gid AS nvarchar(20)), N'กองทุนแนะนำประจำเดือน', N'Files/Site0/1/widget_icons/assetplus/icon-FeaturedFunds.png', N'', N'<section class="section featured-funds featured-funds--v2">
    <div class="container">
        <div class="section-head">
            <h2 class="section-head__title">กองทุนแนะนำประจำเดือน</h2>
            <a href="/funds/featured" class="btn btn-split btn-light" aria-label="ดูกองทุนแนะนำทั้งหมด">
                <span>ดูทั้งหมด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>

        <ul class="fund-bento">
                <li class="fund-bento__cell">
                    <a href="/funds/a-humanoid" class="fund-bento__link" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2E;&#xE34;&#xE27;&#xE41;&#xE21;&#xE19;&#xE19;&#xE2D;&#xE22;&#xE14;&#xE4C;">
                        <span class="fund-bento__head">
                            <span class="fund-bento__code">A-HUMANOID</span>
                            <span class="fund-bento__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;/&#xE2B;&#xE38;&#xE49;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
                        </span>

                        <span class="fund-bento__summary">&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE19;&#xE18;&#xE38;&#xE23;&#xE01;&#xE34;&#xE08;&#xE2B;&#xE38;&#xE48;&#xE19;&#xE22;&#xE19;&#xE15;&#xE4C;&#xE2E;&#xE34;&#xE27;&#xE41;&#xE21;&#xE19;&#xE19;&#xE2D;&#xE22;&#xE14;&#xE4C;&#xE41;&#xE25;&#xE30; AI &#xE23;&#xE30;&#xE14;&#xE31;&#xE1A;&#xE42;&#xE25;&#xE01; &#xE17;&#xE35;&#xE48;&#xE01;&#xE33;&#xE25;&#xE31;&#xE07;&#xE40;&#xE1B;&#xE25;&#xE35;&#xE48;&#xE22;&#xE19;&#xE42;&#xE09;&#xE21;&#xE20;&#xE32;&#xE04;&#xE01;&#xE32;&#xE23;&#xE1C;&#xE25;&#xE34;&#xE15;&#xE41;&#xE25;&#xE30;&#xE1A;&#xE23;&#xE34;&#xE01;&#xE32;&#xE23;</span>

                        <span class="fund-bento__meta">
                            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--7">7</span>
</span>

                                
    <span class="rating">
        <img src="/media/images/morningstar/5-star.png" class="rating__image" alt="Morningstar Rating 5 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

                        </span>

                        <span class="btn btn-split btn-light">
                            <span>ดูรายละเอียด</span>
                            <span class="btn-split__icon">
                                <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                            </span>
                        </span>
                    </a>
                </li>
                <li class="fund-bento__cell">
                    <a href="/funds/a-grid" class="fund-bento__link" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE01;&#xE23;&#xE34;&#xE14; &#xE2D;&#xE34;&#xE19;&#xE1F;&#xE23;&#xE32;&#xE2A;&#xE15;&#xE23;&#xE31;&#xE04;&#xE40;&#xE08;&#xE2D;&#xE23;&#xE4C;">
                        <span class="fund-bento__head">
                            <span class="fund-bento__code">A-GRID</span>
                            <span class="fund-bento__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;/&#xE2B;&#xE38;&#xE49;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
                        </span>

                        <span class="fund-bento__summary">&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE19;&#xE42;&#xE04;&#xE23;&#xE07;&#xE2A;&#xE23;&#xE49;&#xE32;&#xE07;&#xE1E;&#xE37;&#xE49;&#xE19;&#xE10;&#xE32;&#xE19;&#xE14;&#xE49;&#xE32;&#xE19;&#xE1E;&#xE25;&#xE31;&#xE07;&#xE07;&#xE32;&#xE19;&#xE41;&#xE25;&#xE30;&#xE23;&#xE30;&#xE1A;&#xE1A; Smart Grid &#xE17;&#xE35;&#xE48;&#xE40;&#xE1B;&#xE47;&#xE19;&#xE2B;&#xE31;&#xE27;&#xE43;&#xE08;&#xE02;&#xE2D;&#xE07;&#xE42;&#xE25;&#xE01;&#xE2D;&#xE19;&#xE32;&#xE04;&#xE15;</span>

                        <span class="fund-bento__meta">
                            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--3">3</span>
</span>

                                
    <span class="rating">
        <img src="/media/images/morningstar/4-star.png" class="rating__image" alt="Morningstar Rating 4 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

                        </span>

                        <span class="btn btn-split btn-light">
                            <span>ดูรายละเอียด</span>
                            <span class="btn-split__icon">
                                <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                            </span>
                        </span>
                    </a>
                </li>
                <li class="fund-bento__cell">
                    <a href="/funds/a-asemi" class="fund-bento__link" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE40;&#xE2D;&#xE40;&#xE0A;&#xE35;&#xE22; &#xE40;&#xE0B;&#xE21;&#xE34;&#xE04;&#xE2D;&#xE19;&#xE14;&#xE31;&#xE01;&#xE40;&#xE15;&#xE2D;&#xE23;&#xE4C;">
                        <span class="fund-bento__head">
                            <span class="fund-bento__code">A-ASEMI</span>
                            <span class="fund-bento__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;/&#xE2B;&#xE38;&#xE49;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
                        </span>

                        <span class="fund-bento__summary">&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE19;&#xE2B;&#xE48;&#xE27;&#xE07;&#xE42;&#xE0B;&#xE48;&#xE01;&#xE32;&#xE23;&#xE1C;&#xE25;&#xE34;&#xE15;&#xE40;&#xE0B;&#xE21;&#xE34;&#xE04;&#xE2D;&#xE19;&#xE14;&#xE31;&#xE01;&#xE40;&#xE15;&#xE2D;&#xE23;&#xE4C;&#xE41;&#xE2B;&#xE48;&#xE07;&#xE40;&#xE2D;&#xE40;&#xE0A;&#xE35;&#xE22; &#xE15;&#xE31;&#xE49;&#xE07;&#xE41;&#xE15;&#xE48;&#xE15;&#xE49;&#xE19;&#xE19;&#xE49;&#xE33;&#xE16;&#xE36;&#xE07;&#xE1B;&#xE25;&#xE32;&#xE22;&#xE19;&#xE49;&#xE33;</span>

                        <span class="fund-bento__meta">
                            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--7">7</span>
</span>

                                
    <span class="rating">
        <img src="/media/images/morningstar/4-star.png" class="rating__image" alt="Morningstar Rating 4 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

                        </span>

                        <span class="btn btn-split btn-light">
                            <span>ดูรายละเอียด</span>
                            <span class="btn-split__icon">
                                <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                            </span>
                        </span>
                    </a>
                </li>
                <li class="fund-bento__cell">
                    <a href="/funds/a-jedi" class="fund-bento__link" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2A;&#xE40;&#xE1B;&#xE0B; &#xE2D;&#xE35;&#xE42;&#xE04;&#xE42;&#xE19;&#xE21;&#xE35;">
                        <span class="fund-bento__head">
                            <span class="fund-bento__code">A-JEDI</span>
                            <span class="fund-bento__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;/&#xE2B;&#xE38;&#xE49;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
                        </span>

                        <span class="fund-bento__summary">&#xE42;&#xE2D;&#xE01;&#xE32;&#xE2A;&#xE40;&#xE15;&#xE34;&#xE1A;&#xE42;&#xE15;&#xE08;&#xE32;&#xE01;&#xE2D;&#xE38;&#xE15;&#xE2A;&#xE32;&#xE2B;&#xE01;&#xE23;&#xE23;&#xE21;&#xE2D;&#xE27;&#xE01;&#xE32;&#xE28;&#xE41;&#xE25;&#xE30;&#xE14;&#xE32;&#xE27;&#xE40;&#xE17;&#xE35;&#xE22;&#xE21;&#xE17;&#xE35;&#xE48;&#xE02;&#xE22;&#xE32;&#xE22;&#xE15;&#xE31;&#xE27;&#xE15;&#xE48;&#xE2D;&#xE40;&#xE19;&#xE37;&#xE48;&#xE2D;&#xE07;</span>

                        <span class="fund-bento__meta">
                            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--7">7</span>
</span>

                                
    <span class="rating">
        <img src="/media/images/morningstar/5-star.png" class="rating__image" alt="Morningstar Rating 5 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

                        </span>

                        <span class="btn btn-split btn-light">
                            <span>ดูรายละเอียด</span>
                            <span class="btn-split__icon">
                                <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                            </span>
                        </span>
                    </a>
                </li>
                <li class="fund-bento__cell">
                    <a href="/funds/asp-crypto" class="fund-bento__link" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE14;&#xE34;&#xE08;&#xE34;&#xE17;&#xE31;&#xE25; &#xE1A;&#xE25;&#xE47;&#xE2D;&#xE01;&#xE40;&#xE0A;&#xE19;">
                        <span class="fund-bento__head">
                            <span class="fund-bento__code">ASP-CRYPTO</span>
                            <span class="fund-bento__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;/&#xE2B;&#xE38;&#xE49;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
                        </span>

                        <span class="fund-bento__summary">&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE19;&#xE18;&#xE38;&#xE23;&#xE01;&#xE34;&#xE08;&#xE41;&#xE1E;&#xE25;&#xE15;&#xE1F;&#xE2D;&#xE23;&#xE4C;&#xE21;&#xE14;&#xE34;&#xE08;&#xE34;&#xE17;&#xE31;&#xE25; &#xE41;&#xE25;&#xE30;&#xE40;&#xE17;&#xE04;&#xE42;&#xE19;&#xE42;&#xE25;&#xE22;&#xE35;&#xE1A;&#xE25;&#xE47;&#xE2D;&#xE01;&#xE40;&#xE0A;&#xE19;&#xE17;&#xE31;&#xE48;&#xE27;&#xE42;&#xE25;&#xE01;</span>

                        <span class="fund-bento__meta">
                            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--8">8&#x2B;</span>
</span>

                                
    <span class="rating">
        <img src="/media/images/morningstar/5-star.png" class="rating__image" alt="Morningstar Rating 5 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

                        </span>

                        <span class="btn btn-split btn-light">
                            <span>ดูรายละเอียด</span>
                            <span class="btn-split__icon">
                                <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                            </span>
                        </span>
                    </a>
                </li>
                <li class="fund-bento__cell">
                    <a href="/funds/a-ring" class="fund-bento__link" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE42;&#xE01;&#xE25;&#xE14;&#xE4C;">
                        <span class="fund-bento__head">
                            <span class="fund-bento__code">A-RING</span>
                            <span class="fund-bento__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C;&#xE2A;&#xE34;&#xE19;&#xE17;&#xE32;&#xE07;&#xE40;&#xE25;&#xE37;&#xE2D;&#xE01;</span>
                        </span>

                        <span class="fund-bento__summary">&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE19;&#xE17;&#xE2D;&#xE07;&#xE04;&#xE33; &#xE1B;&#xE49;&#xE2D;&#xE07;&#xE01;&#xE31;&#xE19;&#xE04;&#xE27;&#xE32;&#xE21;&#xE40;&#xE2A;&#xE35;&#xE48;&#xE22;&#xE07; &#xE2A;&#xE23;&#xE49;&#xE32;&#xE07;&#xE2A;&#xE21;&#xE14;&#xE38;&#xE25;&#xE1E;&#xE2D;&#xE23;&#xE4C;&#xE15;&#xE01;&#xE32;&#xE23;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;</span>

                        <span class="fund-bento__meta">
                            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--8">8</span>
</span>

                                
    <span class="rating">
        <img src="/media/images/morningstar/4-star.png" class="rating__image" alt="Morningstar Rating 4 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

                        </span>

                        <span class="btn btn-split btn-light">
                            <span>ดูรายละเอียด</span>
                            <span class="btn-split__icon">
                                <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                            </span>
                        </span>
                    </a>
                </li>
        </ul>
    </div>
</section>', N'FeaturedFundsV2', 0);
INSERT INTO [2026_web_widget] (created_at, updated_at, created_by, updated_by, sort, status, pb_status, approve_by, show_front, cat_id, title, img1, mod_name, info, section_key, pb_cat_id, pb_title, pb_img1, pb_mod_name, pb_info, pb_section_key, web_id) VALUES (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'user', N'user', 40, 1, 1, N'user', 1, @gid, N'เปิดมุมมองลงทุนตามเทรนด์', N'Files/Site0/1/widget_icons/assetplus/icon-ExploreThemes.png', N'', N'<section class="section">
    <div class="container-fluid">
        <div class="explore-themes explore-themes--v2">
            <div class="container">
                <div class="explore-themes__heading">
                    <span class="explore-themes__watermark" aria-hidden="true">Explore by Theme</span>
                    <h2 class="explore-themes__title">เปิดมุมมองลงทุนตามเทรนด์</h2>
                </div>

                <ul class="theme-pills">
                        <li>
                            <a href="/funds/themes?theme=ai-robotics" class="theme-pill">
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-robot"></i>
</span>

                                <span class="theme-pill__name">AI &amp; Robotics</span>
                            </a>
                        </li>
                        <li>
                            <a href="/funds/themes?theme=semiconductor" class="theme-pill">
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-cpu"></i>
</span>

                                <span class="theme-pill__name">Semiconductor</span>
                            </a>
                        </li>
                        <li>
                            <a href="/funds/themes?theme=digital-assets" class="theme-pill">
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-currency-bitcoin"></i>
</span>

                                <span class="theme-pill__name">Digital Assets</span>
                            </a>
                        </li>
                        <li>
                            <a href="/funds/themes?theme=clean-energy" class="theme-pill">
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-lightning-charge"></i>
</span>

                                <span class="theme-pill__name">Clean Energy</span>
                            </a>
                        </li>
                        <li>
                            <a href="/funds/themes?theme=aerospace-defense" class="theme-pill">
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-rocket-takeoff"></i>
</span>

                                <span class="theme-pill__name">Aerospace &amp; Defense</span>
                            </a>
                        </li>
                        <li>
                            <a href="/funds/themes?theme=esg" class="theme-pill">
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-tree"></i>
</span>

                                <span class="theme-pill__name">ESG &amp; Sustainability</span>
                            </a>
                        </li>
                </ul>

                <div class="explore-themes__footer">
                    <a href="/funds/themes" class="btn btn-split btn-light" aria-label="ดูธีมการลงทุนทั้งหมด">
                        <span>ดูทั้งหมด</span>
                        <span class="btn-split__icon">
                            <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                        </span>
                    </a>
                </div>
            </div>
        </div>
    </div>
</section>', N'ExploreThemesV2', CAST(@gid AS nvarchar(20)), N'เปิดมุมมองลงทุนตามเทรนด์', N'Files/Site0/1/widget_icons/assetplus/icon-ExploreThemes.png', N'', N'<section class="section">
    <div class="container-fluid">
        <div class="explore-themes explore-themes--v2">
            <div class="container">
                <div class="explore-themes__heading">
                    <span class="explore-themes__watermark" aria-hidden="true">Explore by Theme</span>
                    <h2 class="explore-themes__title">เปิดมุมมองลงทุนตามเทรนด์</h2>
                </div>

                <ul class="theme-pills">
                        <li>
                            <a href="/funds/themes?theme=ai-robotics" class="theme-pill">
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-robot"></i>
</span>

                                <span class="theme-pill__name">AI &amp; Robotics</span>
                            </a>
                        </li>
                        <li>
                            <a href="/funds/themes?theme=semiconductor" class="theme-pill">
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-cpu"></i>
</span>

                                <span class="theme-pill__name">Semiconductor</span>
                            </a>
                        </li>
                        <li>
                            <a href="/funds/themes?theme=digital-assets" class="theme-pill">
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-currency-bitcoin"></i>
</span>

                                <span class="theme-pill__name">Digital Assets</span>
                            </a>
                        </li>
                        <li>
                            <a href="/funds/themes?theme=clean-energy" class="theme-pill">
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-lightning-charge"></i>
</span>

                                <span class="theme-pill__name">Clean Energy</span>
                            </a>
                        </li>
                        <li>
                            <a href="/funds/themes?theme=aerospace-defense" class="theme-pill">
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-rocket-takeoff"></i>
</span>

                                <span class="theme-pill__name">Aerospace &amp; Defense</span>
                            </a>
                        </li>
                        <li>
                            <a href="/funds/themes?theme=esg" class="theme-pill">
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-tree"></i>
</span>

                                <span class="theme-pill__name">ESG &amp; Sustainability</span>
                            </a>
                        </li>
                </ul>

                <div class="explore-themes__footer">
                    <a href="/funds/themes" class="btn btn-split btn-light" aria-label="ดูธีมการลงทุนทั้งหมด">
                        <span>ดูทั้งหมด</span>
                        <span class="btn-split__icon">
                            <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                        </span>
                    </a>
                </div>
            </div>
        </div>
    </div>
</section>', N'ExploreThemesV2', 0);
INSERT INTO [2026_web_widget] (created_at, updated_at, created_by, updated_by, sort, status, pb_status, approve_by, show_front, cat_id, title, img1, mod_name, info, section_key, pb_cat_id, pb_title, pb_img1, pb_mod_name, pb_info, pb_section_key, web_id) VALUES (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'user', N'user', 50, 1, 1, N'user', 1, @gid, N'บทความ / กิจกรรม / ข่าวประกาศ', N'Files/Site0/1/widget_icons/assetplus/icon-Insights.png', N'', N'<section class="section insights insights--v2">
    <div class="container">
        <h2 class="visually-hidden">บทความ ข่าวสาร และประกาศ</h2>

        <ul class="insight-tabs nav" role="tablist">
                <li class="nav-item" role="presentation">
                    <button class="insight-tabs__link active"
                            data-bs-toggle="tab" data-bs-target="#insightPane-articles" type="button" role="tab"
                            aria-controls="insightPane-articles" aria-selected="true">
                        &#xE1A;&#xE17;&#xE04;&#xE27;&#xE32;&#xE21;
                    </button>
                </li>
                <li class="nav-item" role="presentation">
                    <button class="insight-tabs__link"
                            data-bs-toggle="tab" data-bs-target="#insightPane-events" type="button" role="tab"
                            aria-controls="insightPane-events" aria-selected="false">
                        &#xE01;&#xE34;&#xE08;&#xE01;&#xE23;&#xE23;&#xE21;
                    </button>
                </li>

            <li class="nav-item" role="presentation">
                <button class="insight-tabs__link" data-bs-toggle="tab"
                        data-bs-target="#insightPane-announcements" type="button" role="tab"
                        aria-controls="insightPane-announcements" aria-selected="false">
                    ข่าวสารและประกาศ บลจ.
                </button>
            </li>
        </ul>

        <div class="tab-content insight-panes">
                <div class="tab-pane fade show active"
                     role="tabpanel" tabindex="0">
                    <div class="insight-cards">
                            
<a href="/articles/ai-revolution-2026" class="insight-feature">
    <span class="insight-feature__image card__image">
        <img src="/media/images/home/insight/ai-revolution-2026.jpg" alt="" loading="lazy" decoding="async" />
    </span>
    <span class="insight-feature__title">AI Revolution: &#xE42;&#xE2D;&#xE01;&#xE32;&#xE2A;&#xE01;&#xE32;&#xE23;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE17;&#xE35;&#xE48;&#xE2D;&#xE22;&#xE39;&#xE48;&#xE43;&#xE01;&#xE25;&#xE49;&#xE01;&#xE27;&#xE48;&#xE32;&#xE17;&#xE35;&#xE48;&#xE04;&#xE34;&#xE14;</span>
    <time class="insight-feature__date" datetime="2569-07-09">
        9 &#xE01;.&#xE04;. 2569
    </time>
</a>

                            
<a href="/articles/semiconductor-humanoid-h2-2026" class="insight-feature">
    <span class="insight-feature__image card__image">
        <img src="/media/images/home/insight/asset-plus-investment-forum-2026.jpg" alt="" loading="lazy" decoding="async" />
    </span>
    <span class="insight-feature__title">&#xE2A;&#xE48;&#xE2D;&#xE07;&#xE42;&#xE2D;&#xE01;&#xE32;&#xE2A; Semiconductor &amp; Humanoid &#xE04;&#xE23;&#xE36;&#xE48;&#xE07;&#xE1B;&#xE35;&#xE2B;&#xE25;&#xE31;&#xE07; 2026</span>
    <time class="insight-feature__date" datetime="2569-07-03">
        3 &#xE01;.&#xE04;. 2569
    </time>
</a>

                            
<a href="/articles/semiconductor-cycle-restart" class="insight-feature">
    <span class="insight-feature__image card__image">
        <img src="/media/images/home/insight/semiconductor-humanoid-h2-2026.jpg" alt="" loading="lazy" decoding="async" />
    </span>
    <span class="insight-feature__title">Semiconductor Cycle &#xE23;&#xE2D;&#xE1A;&#xE43;&#xE2B;&#xE21;&#xE48;&#xE40;&#xE23;&#xE34;&#xE48;&#xE21;&#xE41;&#xE25;&#xE49;&#xE27;&#xE2B;&#xE23;&#xE37;&#xE2D;&#xE22;&#xE31;&#xE07;</span>
    <time class="insight-feature__date" datetime="2569-06-27">
        27 &#xE21;&#xE34;.&#xE22;. 2569
    </time>
</a>

                    </div>

                    <div class="insight-panes__footer">
                        <a href="/articles" class="btn btn-split btn-light" aria-label="ดู&#xE1A;&#xE17;&#xE04;&#xE27;&#xE32;&#xE21;ทั้งหมด">
                            <span>ดูทั้งหมด</span>
                            <span class="btn-split__icon">
                                <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                            </span>
                        </a>
                    </div>
                </div>
                <div class="tab-pane fade"
                     role="tabpanel" tabindex="0">
                    <div class="insight-cards">
                            
<a href="/events/money-banking-awards-2026" class="insight-feature">
    <span class="insight-feature__image card__image">
        <img src="/media/images/home/insight/semiconductor-humanoid-h2-2026.jpg" alt="" loading="lazy" decoding="async" />
    </span>
    <span class="insight-feature__title">&#xE1A;&#xE25;&#xE08;. &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE04;&#xE27;&#xE49;&#xE32;&#xE23;&#xE32;&#xE07;&#xE27;&#xE31;&#xE25; Money &amp; Banking Awards 2026 &#xE08;&#xE32;&#xE01;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19; ASP-NGF</span>
    <time class="insight-feature__date" datetime="2569-07-09">
        9 &#xE01;.&#xE04;. 2569
    </time>
</a>

                            
<a href="/events/thailand-gold-summit-2026" class="insight-feature">
    <span class="insight-feature__image card__image">
        <img src="/media/images/home/insight/semiconductor-cycle-restart.jpg" alt="" loading="lazy" decoding="async" />
    </span>
    <span class="insight-feature__title">&#xE1A;&#xE25;&#xE08;. &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE0A;&#xE35;&#xE49;&#xE42;&#xE25;&#xE01;&#xE01;&#xE32;&#xE23;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE22;&#xE38;&#xE04; Uncertainty &#xE15;&#xE49;&#xE2D;&#xE07;&#xE01;&#xE23;&#xE30;&#xE08;&#xE32;&#xE22;&#xE1E;&#xE2D;&#xE23;&#xE4C;&#xE15;&#xE14;&#xE49;&#xE27;&#xE22;&#xE2A;&#xE34;&#xE19;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C;&#xE40;&#xE0A;&#xE34;&#xE07;&#xE01;&#xE25;&#xE22;&#xE38;&#xE17;&#xE18;&#xE4C;</span>
    <time class="insight-feature__date" datetime="2569-06-30">
        30 &#xE21;&#xE34;.&#xE22;. 2569
    </time>
</a>

                            
<a href="/events/gsb-the-selected" class="insight-feature">
    <span class="insight-feature__image card__image">
        <img src="/media/images/home/insight/a-humanoid-ipo.jpg" alt="" loading="lazy" decoding="async" />
    </span>
    <span class="insight-feature__title">&#xE1A;&#xE25;&#xE08;. &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE23;&#xE48;&#xE27;&#xE21;&#xE40;&#xE1B;&#xE47;&#xE19;&#xE1E;&#xE31;&#xE19;&#xE18;&#xE21;&#xE34;&#xE15;&#xE23;&#xE01;&#xE31;&#xE1A;&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE2D;&#xE2D;&#xE21;&#xE2A;&#xE34;&#xE19; &#xE43;&#xE19;&#xE42;&#xE04;&#xE23;&#xE07;&#xE01;&#xE32;&#xE23; &#x201C;&#xE2D;&#xE2D;&#xE21;&#xE2A;&#xE34;&#xE19; The Selected&#x201D;</span>
    <time class="insight-feature__date" datetime="2569-06-21">
        21 &#xE21;&#xE34;.&#xE22;. 2569
    </time>
</a>

                    </div>

                    <div class="insight-panes__footer">
                        <a href="/events" class="btn btn-split btn-light" aria-label="ดู&#xE01;&#xE34;&#xE08;&#xE01;&#xE23;&#xE23;&#xE21;ทั้งหมด">
                            <span>ดูทั้งหมด</span>
                            <span class="btn-split__icon">
                                <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                            </span>
                        </a>
                    </div>
                </div>

            <div class="tab-pane fade" role="tabpanel" tabindex="0">
                <ul class="announcement-list insight-cards insight-cards--stack">
                        <li>
                            <a href="/media/documents/sample.pdf" class="announcement-item" target="_blank" rel="noopener"
                               aria-label="&#xE1C;&#xE25;&#xE01;&#xE32;&#xE23;&#xE14;&#xE33;&#xE40;&#xE19;&#xE34;&#xE19;&#xE07;&#xE32;&#xE19;: &#xE23;&#xE32;&#xE22;&#xE07;&#xE32;&#xE19;&#xE1C;&#xE25;&#xE01;&#xE32;&#xE23;&#xE14;&#xE33;&#xE40;&#xE19;&#xE34;&#xE19;&#xE07;&#xE32;&#xE19;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE20;&#xE32;&#xE22;&#xE43;&#xE15;&#xE49;&#xE01;&#xE32;&#xE23;&#xE08;&#xE31;&#xE14;&#xE01;&#xE32;&#xE23; &#xE1B;&#xE23;&#xE30;&#xE08;&#xE33;&#xE44;&#xE15;&#xE23;&#xE21;&#xE32;&#xE2A; 2/2569 (ไฟล์ PDF, เปิดในแท็บใหม่)">
                                <span class="announcement-item__icon" aria-hidden="true">
                                    <i class="bi bi-graph-up-arrow"></i>
                                </span>
                                <span class="announcement-item__body">
                                    <span class="announcement-item__title">&#xE23;&#xE32;&#xE22;&#xE07;&#xE32;&#xE19;&#xE1C;&#xE25;&#xE01;&#xE32;&#xE23;&#xE14;&#xE33;&#xE40;&#xE19;&#xE34;&#xE19;&#xE07;&#xE32;&#xE19;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE20;&#xE32;&#xE22;&#xE43;&#xE15;&#xE49;&#xE01;&#xE32;&#xE23;&#xE08;&#xE31;&#xE14;&#xE01;&#xE32;&#xE23; &#xE1B;&#xE23;&#xE30;&#xE08;&#xE33;&#xE44;&#xE15;&#xE23;&#xE21;&#xE32;&#xE2A; 2/2569</span>
                                    <time class="announcement-item__date"
                                          datetime="2569-05-28">
                                        28 &#xE1E;.&#xE04;. 2569
                                    </time>
                                </span>
                            </a>
                        </li>
                        <li>
                            <a href="/media/documents/sample.pdf" class="announcement-item" target="_blank" rel="noopener"
                               aria-label="&#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE27;&#xE31;&#xE19;&#xE2B;&#xE22;&#xE38;&#xE14;&#xE0B;&#xE37;&#xE49;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;: &#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE27;&#xE31;&#xE19;&#xE2B;&#xE22;&#xE38;&#xE14;&#xE17;&#xE33;&#xE01;&#xE32;&#xE23;&#xE0B;&#xE37;&#xE49;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19; &#xE1B;&#xE23;&#xE30;&#xE08;&#xE33;&#xE40;&#xE14;&#xE37;&#xE2D;&#xE19;&#xE01;&#xE23;&#xE01;&#xE0E;&#xE32;&#xE04;&#xE21; 2569 (ไฟล์ PDF, เปิดในแท็บใหม่)">
                                <span class="announcement-item__icon" aria-hidden="true">
                                    <i class="bi bi-calendar3"></i>
                                </span>
                                <span class="announcement-item__body">
                                    <span class="announcement-item__title">&#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE27;&#xE31;&#xE19;&#xE2B;&#xE22;&#xE38;&#xE14;&#xE17;&#xE33;&#xE01;&#xE32;&#xE23;&#xE0B;&#xE37;&#xE49;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19; &#xE1B;&#xE23;&#xE30;&#xE08;&#xE33;&#xE40;&#xE14;&#xE37;&#xE2D;&#xE19;&#xE01;&#xE23;&#xE01;&#xE0E;&#xE32;&#xE04;&#xE21; 2569</span>
                                    <time class="announcement-item__date"
                                          datetime="2569-05-28">
                                        28 &#xE1E;.&#xE04;. 2569
                                    </time>
                                </span>
                            </a>
                        </li>
                        <li>
                            <a href="/media/documents/sample.pdf" class="announcement-item" target="_blank" rel="noopener"
                               aria-label="&#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE41;&#xE08;&#xE49;&#xE07;&#xE1B;&#xE31;&#xE19;&#xE1C;&#xE25;: &#xE41;&#xE08;&#xE49;&#xE07;&#xE01;&#xE32;&#xE23;&#xE08;&#xE48;&#xE32;&#xE22;&#xE40;&#xE07;&#xE34;&#xE19;&#xE1B;&#xE31;&#xE19;&#xE1C;&#xE25;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19; ASP-DIGITAL (ไฟล์ PDF, เปิดในแท็บใหม่)">
                                <span class="announcement-item__icon" aria-hidden="true">
                                    <i class="bi bi-cash-coin"></i>
                                </span>
                                <span class="announcement-item__body">
                                    <span class="announcement-item__title">&#xE41;&#xE08;&#xE49;&#xE07;&#xE01;&#xE32;&#xE23;&#xE08;&#xE48;&#xE32;&#xE22;&#xE40;&#xE07;&#xE34;&#xE19;&#xE1B;&#xE31;&#xE19;&#xE1C;&#xE25;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19; ASP-DIGITAL</span>
                                    <time class="announcement-item__date"
                                          datetime="2569-05-23">
                                        23 &#xE1E;.&#xE04;. 2569
                                    </time>
                                </span>
                            </a>
                        </li>
                        <li>
                            <a href="/media/documents/sample.pdf" class="announcement-item" target="_blank" rel="noopener"
                               aria-label="&#xE23;&#xE32;&#xE07;&#xE27;&#xE31;&#xE25;&#xE41;&#xE25;&#xE30;&#xE04;&#xE27;&#xE32;&#xE21;&#xE2A;&#xE33;&#xE40;&#xE23;&#xE47;&#xE08;: &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE04;&#xE27;&#xE49;&#xE32;&#xE23;&#xE32;&#xE07;&#xE27;&#xE31;&#xE25;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE22;&#xE2D;&#xE14;&#xE40;&#xE22;&#xE35;&#xE48;&#xE22;&#xE21;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE20;&#xE17;&#xE15;&#xE23;&#xE32;&#xE2A;&#xE32;&#xE23;&#xE17;&#xE38;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28; (ไฟล์ PDF, เปิดในแท็บใหม่)">
                                <span class="announcement-item__icon" aria-hidden="true">
                                    <i class="bi bi-trophy"></i>
                                </span>
                                <span class="announcement-item__body">
                                    <span class="announcement-item__title">&#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE04;&#xE27;&#xE49;&#xE32;&#xE23;&#xE32;&#xE07;&#xE27;&#xE31;&#xE25;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE22;&#xE2D;&#xE14;&#xE40;&#xE22;&#xE35;&#xE48;&#xE22;&#xE21;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE20;&#xE17;&#xE15;&#xE23;&#xE32;&#xE2A;&#xE32;&#xE23;&#xE17;&#xE38;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
                                    <time class="announcement-item__date"
                                          datetime="2569-05-20">
                                        20 &#xE1E;.&#xE04;. 2569
                                    </time>
                                </span>
                            </a>
                        </li>
                        <li>
                            <a href="/media/documents/sample.pdf" class="announcement-item" target="_blank" rel="noopener"
                               aria-label="&#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE40;&#xE1B;&#xE34;&#xE14;&#xE40;&#xE2A;&#xE19;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;: &#xE40;&#xE1B;&#xE34;&#xE14;&#xE40;&#xE2A;&#xE19;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE2B;&#xE21;&#xE48; A-HUMANOID (ไฟล์ PDF, เปิดในแท็บใหม่)">
                                <span class="announcement-item__icon" aria-hidden="true">
                                    <i class="bi bi-bar-chart-line"></i>
                                </span>
                                <span class="announcement-item__body">
                                    <span class="announcement-item__title">&#xE40;&#xE1B;&#xE34;&#xE14;&#xE40;&#xE2A;&#xE19;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE2B;&#xE21;&#xE48; A-HUMANOID</span>
                                    <time class="announcement-item__date"
                                          datetime="2569-05-18">
                                        18 &#xE1E;.&#xE04;. 2569
                                    </time>
                                </span>
                            </a>
                        </li>
                </ul>

                <div class="insight-panes__footer">
                    <a href="/news-announcements" class="btn btn-split btn-light" aria-label="ดูข่าวสารและประกาศทั้งหมด">
                        <span>ดูทั้งหมด</span>
                        <span class="btn-split__icon">
                            <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                        </span>
                    </a>
                </div>
            </div>
        </div>
    </div>
</section>', N'InsightsV2', CAST(@gid AS nvarchar(20)), N'บทความ / กิจกรรม / ข่าวประกาศ', N'Files/Site0/1/widget_icons/assetplus/icon-Insights.png', N'', N'<section class="section insights insights--v2">
    <div class="container">
        <h2 class="visually-hidden">บทความ ข่าวสาร และประกาศ</h2>

        <ul class="insight-tabs nav" role="tablist">
                <li class="nav-item" role="presentation">
                    <button class="insight-tabs__link active"
                            data-bs-toggle="tab" data-bs-target="#insightPane-articles" type="button" role="tab"
                            aria-controls="insightPane-articles" aria-selected="true">
                        &#xE1A;&#xE17;&#xE04;&#xE27;&#xE32;&#xE21;
                    </button>
                </li>
                <li class="nav-item" role="presentation">
                    <button class="insight-tabs__link"
                            data-bs-toggle="tab" data-bs-target="#insightPane-events" type="button" role="tab"
                            aria-controls="insightPane-events" aria-selected="false">
                        &#xE01;&#xE34;&#xE08;&#xE01;&#xE23;&#xE23;&#xE21;
                    </button>
                </li>

            <li class="nav-item" role="presentation">
                <button class="insight-tabs__link" data-bs-toggle="tab"
                        data-bs-target="#insightPane-announcements" type="button" role="tab"
                        aria-controls="insightPane-announcements" aria-selected="false">
                    ข่าวสารและประกาศ บลจ.
                </button>
            </li>
        </ul>

        <div class="tab-content insight-panes">
                <div class="tab-pane fade show active"
                     role="tabpanel" tabindex="0">
                    <div class="insight-cards">
                            
<a href="/articles/ai-revolution-2026" class="insight-feature">
    <span class="insight-feature__image card__image">
        <img src="/media/images/home/insight/ai-revolution-2026.jpg" alt="" loading="lazy" decoding="async" />
    </span>
    <span class="insight-feature__title">AI Revolution: &#xE42;&#xE2D;&#xE01;&#xE32;&#xE2A;&#xE01;&#xE32;&#xE23;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE17;&#xE35;&#xE48;&#xE2D;&#xE22;&#xE39;&#xE48;&#xE43;&#xE01;&#xE25;&#xE49;&#xE01;&#xE27;&#xE48;&#xE32;&#xE17;&#xE35;&#xE48;&#xE04;&#xE34;&#xE14;</span>
    <time class="insight-feature__date" datetime="2569-07-09">
        9 &#xE01;.&#xE04;. 2569
    </time>
</a>

                            
<a href="/articles/semiconductor-humanoid-h2-2026" class="insight-feature">
    <span class="insight-feature__image card__image">
        <img src="/media/images/home/insight/asset-plus-investment-forum-2026.jpg" alt="" loading="lazy" decoding="async" />
    </span>
    <span class="insight-feature__title">&#xE2A;&#xE48;&#xE2D;&#xE07;&#xE42;&#xE2D;&#xE01;&#xE32;&#xE2A; Semiconductor &amp; Humanoid &#xE04;&#xE23;&#xE36;&#xE48;&#xE07;&#xE1B;&#xE35;&#xE2B;&#xE25;&#xE31;&#xE07; 2026</span>
    <time class="insight-feature__date" datetime="2569-07-03">
        3 &#xE01;.&#xE04;. 2569
    </time>
</a>

                            
<a href="/articles/semiconductor-cycle-restart" class="insight-feature">
    <span class="insight-feature__image card__image">
        <img src="/media/images/home/insight/semiconductor-humanoid-h2-2026.jpg" alt="" loading="lazy" decoding="async" />
    </span>
    <span class="insight-feature__title">Semiconductor Cycle &#xE23;&#xE2D;&#xE1A;&#xE43;&#xE2B;&#xE21;&#xE48;&#xE40;&#xE23;&#xE34;&#xE48;&#xE21;&#xE41;&#xE25;&#xE49;&#xE27;&#xE2B;&#xE23;&#xE37;&#xE2D;&#xE22;&#xE31;&#xE07;</span>
    <time class="insight-feature__date" datetime="2569-06-27">
        27 &#xE21;&#xE34;.&#xE22;. 2569
    </time>
</a>

                    </div>

                    <div class="insight-panes__footer">
                        <a href="/articles" class="btn btn-split btn-light" aria-label="ดู&#xE1A;&#xE17;&#xE04;&#xE27;&#xE32;&#xE21;ทั้งหมด">
                            <span>ดูทั้งหมด</span>
                            <span class="btn-split__icon">
                                <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                            </span>
                        </a>
                    </div>
                </div>
                <div class="tab-pane fade"
                     role="tabpanel" tabindex="0">
                    <div class="insight-cards">
                            
<a href="/events/money-banking-awards-2026" class="insight-feature">
    <span class="insight-feature__image card__image">
        <img src="/media/images/home/insight/semiconductor-humanoid-h2-2026.jpg" alt="" loading="lazy" decoding="async" />
    </span>
    <span class="insight-feature__title">&#xE1A;&#xE25;&#xE08;. &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE04;&#xE27;&#xE49;&#xE32;&#xE23;&#xE32;&#xE07;&#xE27;&#xE31;&#xE25; Money &amp; Banking Awards 2026 &#xE08;&#xE32;&#xE01;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19; ASP-NGF</span>
    <time class="insight-feature__date" datetime="2569-07-09">
        9 &#xE01;.&#xE04;. 2569
    </time>
</a>

                            
<a href="/events/thailand-gold-summit-2026" class="insight-feature">
    <span class="insight-feature__image card__image">
        <img src="/media/images/home/insight/semiconductor-cycle-restart.jpg" alt="" loading="lazy" decoding="async" />
    </span>
    <span class="insight-feature__title">&#xE1A;&#xE25;&#xE08;. &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE0A;&#xE35;&#xE49;&#xE42;&#xE25;&#xE01;&#xE01;&#xE32;&#xE23;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE22;&#xE38;&#xE04; Uncertainty &#xE15;&#xE49;&#xE2D;&#xE07;&#xE01;&#xE23;&#xE30;&#xE08;&#xE32;&#xE22;&#xE1E;&#xE2D;&#xE23;&#xE4C;&#xE15;&#xE14;&#xE49;&#xE27;&#xE22;&#xE2A;&#xE34;&#xE19;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C;&#xE40;&#xE0A;&#xE34;&#xE07;&#xE01;&#xE25;&#xE22;&#xE38;&#xE17;&#xE18;&#xE4C;</span>
    <time class="insight-feature__date" datetime="2569-06-30">
        30 &#xE21;&#xE34;.&#xE22;. 2569
    </time>
</a>

                            
<a href="/events/gsb-the-selected" class="insight-feature">
    <span class="insight-feature__image card__image">
        <img src="/media/images/home/insight/a-humanoid-ipo.jpg" alt="" loading="lazy" decoding="async" />
    </span>
    <span class="insight-feature__title">&#xE1A;&#xE25;&#xE08;. &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE23;&#xE48;&#xE27;&#xE21;&#xE40;&#xE1B;&#xE47;&#xE19;&#xE1E;&#xE31;&#xE19;&#xE18;&#xE21;&#xE34;&#xE15;&#xE23;&#xE01;&#xE31;&#xE1A;&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE2D;&#xE2D;&#xE21;&#xE2A;&#xE34;&#xE19; &#xE43;&#xE19;&#xE42;&#xE04;&#xE23;&#xE07;&#xE01;&#xE32;&#xE23; &#x201C;&#xE2D;&#xE2D;&#xE21;&#xE2A;&#xE34;&#xE19; The Selected&#x201D;</span>
    <time class="insight-feature__date" datetime="2569-06-21">
        21 &#xE21;&#xE34;.&#xE22;. 2569
    </time>
</a>

                    </div>

                    <div class="insight-panes__footer">
                        <a href="/events" class="btn btn-split btn-light" aria-label="ดู&#xE01;&#xE34;&#xE08;&#xE01;&#xE23;&#xE23;&#xE21;ทั้งหมด">
                            <span>ดูทั้งหมด</span>
                            <span class="btn-split__icon">
                                <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                            </span>
                        </a>
                    </div>
                </div>

            <div class="tab-pane fade" role="tabpanel" tabindex="0">
                <ul class="announcement-list insight-cards insight-cards--stack">
                        <li>
                            <a href="/media/documents/sample.pdf" class="announcement-item" target="_blank" rel="noopener"
                               aria-label="&#xE1C;&#xE25;&#xE01;&#xE32;&#xE23;&#xE14;&#xE33;&#xE40;&#xE19;&#xE34;&#xE19;&#xE07;&#xE32;&#xE19;: &#xE23;&#xE32;&#xE22;&#xE07;&#xE32;&#xE19;&#xE1C;&#xE25;&#xE01;&#xE32;&#xE23;&#xE14;&#xE33;&#xE40;&#xE19;&#xE34;&#xE19;&#xE07;&#xE32;&#xE19;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE20;&#xE32;&#xE22;&#xE43;&#xE15;&#xE49;&#xE01;&#xE32;&#xE23;&#xE08;&#xE31;&#xE14;&#xE01;&#xE32;&#xE23; &#xE1B;&#xE23;&#xE30;&#xE08;&#xE33;&#xE44;&#xE15;&#xE23;&#xE21;&#xE32;&#xE2A; 2/2569 (ไฟล์ PDF, เปิดในแท็บใหม่)">
                                <span class="announcement-item__icon" aria-hidden="true">
                                    <i class="bi bi-graph-up-arrow"></i>
                                </span>
                                <span class="announcement-item__body">
                                    <span class="announcement-item__title">&#xE23;&#xE32;&#xE22;&#xE07;&#xE32;&#xE19;&#xE1C;&#xE25;&#xE01;&#xE32;&#xE23;&#xE14;&#xE33;&#xE40;&#xE19;&#xE34;&#xE19;&#xE07;&#xE32;&#xE19;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE20;&#xE32;&#xE22;&#xE43;&#xE15;&#xE49;&#xE01;&#xE32;&#xE23;&#xE08;&#xE31;&#xE14;&#xE01;&#xE32;&#xE23; &#xE1B;&#xE23;&#xE30;&#xE08;&#xE33;&#xE44;&#xE15;&#xE23;&#xE21;&#xE32;&#xE2A; 2/2569</span>
                                    <time class="announcement-item__date"
                                          datetime="2569-05-28">
                                        28 &#xE1E;.&#xE04;. 2569
                                    </time>
                                </span>
                            </a>
                        </li>
                        <li>
                            <a href="/media/documents/sample.pdf" class="announcement-item" target="_blank" rel="noopener"
                               aria-label="&#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE27;&#xE31;&#xE19;&#xE2B;&#xE22;&#xE38;&#xE14;&#xE0B;&#xE37;&#xE49;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;: &#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE27;&#xE31;&#xE19;&#xE2B;&#xE22;&#xE38;&#xE14;&#xE17;&#xE33;&#xE01;&#xE32;&#xE23;&#xE0B;&#xE37;&#xE49;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19; &#xE1B;&#xE23;&#xE30;&#xE08;&#xE33;&#xE40;&#xE14;&#xE37;&#xE2D;&#xE19;&#xE01;&#xE23;&#xE01;&#xE0E;&#xE32;&#xE04;&#xE21; 2569 (ไฟล์ PDF, เปิดในแท็บใหม่)">
                                <span class="announcement-item__icon" aria-hidden="true">
                                    <i class="bi bi-calendar3"></i>
                                </span>
                                <span class="announcement-item__body">
                                    <span class="announcement-item__title">&#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE27;&#xE31;&#xE19;&#xE2B;&#xE22;&#xE38;&#xE14;&#xE17;&#xE33;&#xE01;&#xE32;&#xE23;&#xE0B;&#xE37;&#xE49;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19; &#xE1B;&#xE23;&#xE30;&#xE08;&#xE33;&#xE40;&#xE14;&#xE37;&#xE2D;&#xE19;&#xE01;&#xE23;&#xE01;&#xE0E;&#xE32;&#xE04;&#xE21; 2569</span>
                                    <time class="announcement-item__date"
                                          datetime="2569-05-28">
                                        28 &#xE1E;.&#xE04;. 2569
                                    </time>
                                </span>
                            </a>
                        </li>
                        <li>
                            <a href="/media/documents/sample.pdf" class="announcement-item" target="_blank" rel="noopener"
                               aria-label="&#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE41;&#xE08;&#xE49;&#xE07;&#xE1B;&#xE31;&#xE19;&#xE1C;&#xE25;: &#xE41;&#xE08;&#xE49;&#xE07;&#xE01;&#xE32;&#xE23;&#xE08;&#xE48;&#xE32;&#xE22;&#xE40;&#xE07;&#xE34;&#xE19;&#xE1B;&#xE31;&#xE19;&#xE1C;&#xE25;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19; ASP-DIGITAL (ไฟล์ PDF, เปิดในแท็บใหม่)">
                                <span class="announcement-item__icon" aria-hidden="true">
                                    <i class="bi bi-cash-coin"></i>
                                </span>
                                <span class="announcement-item__body">
                                    <span class="announcement-item__title">&#xE41;&#xE08;&#xE49;&#xE07;&#xE01;&#xE32;&#xE23;&#xE08;&#xE48;&#xE32;&#xE22;&#xE40;&#xE07;&#xE34;&#xE19;&#xE1B;&#xE31;&#xE19;&#xE1C;&#xE25;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19; ASP-DIGITAL</span>
                                    <time class="announcement-item__date"
                                          datetime="2569-05-23">
                                        23 &#xE1E;.&#xE04;. 2569
                                    </time>
                                </span>
                            </a>
                        </li>
                        <li>
                            <a href="/media/documents/sample.pdf" class="announcement-item" target="_blank" rel="noopener"
                               aria-label="&#xE23;&#xE32;&#xE07;&#xE27;&#xE31;&#xE25;&#xE41;&#xE25;&#xE30;&#xE04;&#xE27;&#xE32;&#xE21;&#xE2A;&#xE33;&#xE40;&#xE23;&#xE47;&#xE08;: &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE04;&#xE27;&#xE49;&#xE32;&#xE23;&#xE32;&#xE07;&#xE27;&#xE31;&#xE25;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE22;&#xE2D;&#xE14;&#xE40;&#xE22;&#xE35;&#xE48;&#xE22;&#xE21;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE20;&#xE17;&#xE15;&#xE23;&#xE32;&#xE2A;&#xE32;&#xE23;&#xE17;&#xE38;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28; (ไฟล์ PDF, เปิดในแท็บใหม่)">
                                <span class="announcement-item__icon" aria-hidden="true">
                                    <i class="bi bi-trophy"></i>
                                </span>
                                <span class="announcement-item__body">
                                    <span class="announcement-item__title">&#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE04;&#xE27;&#xE49;&#xE32;&#xE23;&#xE32;&#xE07;&#xE27;&#xE31;&#xE25;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE22;&#xE2D;&#xE14;&#xE40;&#xE22;&#xE35;&#xE48;&#xE22;&#xE21;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE20;&#xE17;&#xE15;&#xE23;&#xE32;&#xE2A;&#xE32;&#xE23;&#xE17;&#xE38;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
                                    <time class="announcement-item__date"
                                          datetime="2569-05-20">
                                        20 &#xE1E;.&#xE04;. 2569
                                    </time>
                                </span>
                            </a>
                        </li>
                        <li>
                            <a href="/media/documents/sample.pdf" class="announcement-item" target="_blank" rel="noopener"
                               aria-label="&#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE40;&#xE1B;&#xE34;&#xE14;&#xE40;&#xE2A;&#xE19;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;: &#xE40;&#xE1B;&#xE34;&#xE14;&#xE40;&#xE2A;&#xE19;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE2B;&#xE21;&#xE48; A-HUMANOID (ไฟล์ PDF, เปิดในแท็บใหม่)">
                                <span class="announcement-item__icon" aria-hidden="true">
                                    <i class="bi bi-bar-chart-line"></i>
                                </span>
                                <span class="announcement-item__body">
                                    <span class="announcement-item__title">&#xE40;&#xE1B;&#xE34;&#xE14;&#xE40;&#xE2A;&#xE19;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE2B;&#xE21;&#xE48; A-HUMANOID</span>
                                    <time class="announcement-item__date"
                                          datetime="2569-05-18">
                                        18 &#xE1E;.&#xE04;. 2569
                                    </time>
                                </span>
                            </a>
                        </li>
                </ul>

                <div class="insight-panes__footer">
                    <a href="/news-announcements" class="btn btn-split btn-light" aria-label="ดูข่าวสารและประกาศทั้งหมด">
                        <span>ดูทั้งหมด</span>
                        <span class="btn-split__icon">
                            <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                        </span>
                    </a>
                </div>
            </div>
        </div>
    </div>
</section>', N'InsightsV2', 0);
INSERT INTO [2026_web_widget] (created_at, updated_at, created_by, updated_by, sort, status, pb_status, approve_by, show_front, cat_id, title, img1, mod_name, info, section_key, pb_cat_id, pb_title, pb_img1, pb_mod_name, pb_info, pb_section_key, web_id) VALUES (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'user', N'user', 60, 1, 1, N'user', 1, @gid, N'ตัวแทนขาย', N'Files/Site0/1/widget_icons/assetplus/icon-Distributors.png', N'', N'<section class="section distributors distributors--v2">
    <div class="container">
        <div class="distributors__head">
            <h2 class="distributors__title">
                ซื้อกองทุน <strong class="distributors__brand">Asset Plus</strong> ได้ผ่านตัวแทนขายชั้นนำ
            </h2>
        </div>
    </div>

    <div class="logo-marquee">
        <div class="logo-marquee__track">
                <ul class="logo-marquee__set">
                        <li class="logo-marquee__item">
                                <a href="https://www.scb.co.th" class="distributor" target="_blank" rel="noopener" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE44;&#xE17;&#xE22;&#xE1E;&#xE32;&#xE13;&#xE34;&#xE0A;&#xE22;&#xE4C; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-0.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">SCB</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.kasikornbank.com" class="distributor" target="_blank" rel="noopener" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE01;&#xE2A;&#xE34;&#xE01;&#xE23;&#xE44;&#xE17;&#xE22; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-1.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">KBank</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.gsb.or.th" class="distributor" target="_blank" rel="noopener" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE2D;&#xE2D;&#xE21;&#xE2A;&#xE34;&#xE19; (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-2.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">Government Savings Bank</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.krungsri.com" class="distributor" target="_blank" rel="noopener" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE01;&#xE23;&#xE38;&#xE07;&#xE28;&#xE23;&#xE35;&#xE2D;&#xE22;&#xE38;&#xE18;&#xE22;&#xE32; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-3.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">krungsri</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.bangkokbank.com" class="distributor" target="_blank" rel="noopener" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE01;&#xE23;&#xE38;&#xE07;&#xE40;&#xE17;&#xE1E; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-4.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">Bangkok Bank</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.ttbbank.com" class="distributor" target="_blank" rel="noopener" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE17;&#xE2B;&#xE32;&#xE23;&#xE44;&#xE17;&#xE22;&#xE18;&#xE19;&#xE0A;&#xE32;&#xE15; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-15.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">ttb</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.cimbthai.com" class="distributor" target="_blank" rel="noopener" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE0B;&#xE35;&#xE44;&#xE2D;&#xE40;&#xE2D;&#xE47;&#xE21;&#xE1A;&#xE35; &#xE44;&#xE17;&#xE22; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-5.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">CIMB</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.uob.co.th" class="distributor" target="_blank" rel="noopener" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE22;&#xE39;&#xE42;&#xE2D;&#xE1A;&#xE35; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-6.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">UOB</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.lhbank.co.th" class="distributor" target="_blank" rel="noopener" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE41;&#xE25;&#xE19;&#xE14;&#xE4C; &#xE41;&#xE2D;&#xE19;&#xE14;&#xE4C; &#xE40;&#xE2E;&#xE49;&#xE32;&#xE2A;&#xE4C; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-7.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">LH Bank</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://bank.kkpfg.com" class="distributor" target="_blank" rel="noopener" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE40;&#xE01;&#xE35;&#xE22;&#xE23;&#xE15;&#xE34;&#xE19;&#xE32;&#xE04;&#xE34;&#xE19;&#xE20;&#xE31;&#xE17;&#xE23; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-8.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">KKP</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.fnsyrus.com" class="distributor" target="_blank" rel="noopener" aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE1F;&#xE34;&#xE19;&#xE31;&#xE19;&#xE40;&#xE0B;&#xE35;&#xE22; &#xE44;&#xE0B;&#xE23;&#xE31;&#xE2A; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-9.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">FINANSIA</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.kgieworld.co.th" class="distributor" target="_blank" rel="noopener" aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE40;&#xE04;&#xE08;&#xE35;&#xE44;&#xE2D; (&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;&#xE44;&#xE17;&#xE22;) &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-10.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">KGI</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.aira.co.th" class="distributor" target="_blank" rel="noopener" aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE44;&#xE2D;&#xE23;&#xE48;&#xE32; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-11.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">AIRA</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.tisco.co.th" class="distributor" target="_blank" rel="noopener" aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE17;&#xE34;&#xE2A;&#xE42;&#xE01;&#xE49; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-12.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">TISCO</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.asiaplus.co.th" class="distributor" target="_blank" rel="noopener" aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE40;&#xE2D;&#xE40;&#xE0B;&#xE35;&#xE22; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-13.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">ASIA PLUS Security</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.dime.co.th" class="distributor" target="_blank" rel="noopener" aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE40;&#xE14;&#xE1F; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (Dime!) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-14.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">Dime</span>
                                </a>
                        </li>
                </ul>
                <ul class="logo-marquee__set" aria-hidden="true">
                        <li class="logo-marquee__item">
                                <a href="https://www.scb.co.th" class="distributor" target="_blank" rel="noopener"
                                   tabindex="-1" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE44;&#xE17;&#xE22;&#xE1E;&#xE32;&#xE13;&#xE34;&#xE0A;&#xE22;&#xE4C; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-0.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">SCB</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.kasikornbank.com" class="distributor" target="_blank" rel="noopener"
                                   tabindex="-1" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE01;&#xE2A;&#xE34;&#xE01;&#xE23;&#xE44;&#xE17;&#xE22; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-1.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">KBank</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.gsb.or.th" class="distributor" target="_blank" rel="noopener"
                                   tabindex="-1" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE2D;&#xE2D;&#xE21;&#xE2A;&#xE34;&#xE19; (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-2.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">Government Savings Bank</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.krungsri.com" class="distributor" target="_blank" rel="noopener"
                                   tabindex="-1" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE01;&#xE23;&#xE38;&#xE07;&#xE28;&#xE23;&#xE35;&#xE2D;&#xE22;&#xE38;&#xE18;&#xE22;&#xE32; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-3.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">krungsri</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.bangkokbank.com" class="distributor" target="_blank" rel="noopener"
                                   tabindex="-1" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE01;&#xE23;&#xE38;&#xE07;&#xE40;&#xE17;&#xE1E; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-4.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">Bangkok Bank</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.ttbbank.com" class="distributor" target="_blank" rel="noopener"
                                   tabindex="-1" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE17;&#xE2B;&#xE32;&#xE23;&#xE44;&#xE17;&#xE22;&#xE18;&#xE19;&#xE0A;&#xE32;&#xE15; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-15.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">ttb</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.cimbthai.com" class="distributor" target="_blank" rel="noopener"
                                   tabindex="-1" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE0B;&#xE35;&#xE44;&#xE2D;&#xE40;&#xE2D;&#xE47;&#xE21;&#xE1A;&#xE35; &#xE44;&#xE17;&#xE22; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-5.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">CIMB</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.uob.co.th" class="distributor" target="_blank" rel="noopener"
                                   tabindex="-1" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE22;&#xE39;&#xE42;&#xE2D;&#xE1A;&#xE35; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-6.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">UOB</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.lhbank.co.th" class="distributor" target="_blank" rel="noopener"
                                   tabindex="-1" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE41;&#xE25;&#xE19;&#xE14;&#xE4C; &#xE41;&#xE2D;&#xE19;&#xE14;&#xE4C; &#xE40;&#xE2E;&#xE49;&#xE32;&#xE2A;&#xE4C; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-7.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">LH Bank</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://bank.kkpfg.com" class="distributor" target="_blank" rel="noopener"
                                   tabindex="-1" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE40;&#xE01;&#xE35;&#xE22;&#xE23;&#xE15;&#xE34;&#xE19;&#xE32;&#xE04;&#xE34;&#xE19;&#xE20;&#xE31;&#xE17;&#xE23; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-8.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">KKP</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.fnsyrus.com" class="distributor" target="_blank" rel="noopener"
                                   tabindex="-1" aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE1F;&#xE34;&#xE19;&#xE31;&#xE19;&#xE40;&#xE0B;&#xE35;&#xE22; &#xE44;&#xE0B;&#xE23;&#xE31;&#xE2A; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-9.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">FINANSIA</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.kgieworld.co.th" class="distributor" target="_blank" rel="noopener"
                                   tabindex="-1" aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE40;&#xE04;&#xE08;&#xE35;&#xE44;&#xE2D; (&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;&#xE44;&#xE17;&#xE22;) &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-10.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">KGI</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.aira.co.th" class="distributor" target="_blank" rel="noopener"
                                   tabindex="-1" aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE44;&#xE2D;&#xE23;&#xE48;&#xE32; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-11.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">AIRA</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.tisco.co.th" class="distributor" target="_blank" rel="noopener"
                                   tabindex="-1" aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE17;&#xE34;&#xE2A;&#xE42;&#xE01;&#xE49; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-12.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">TISCO</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.asiaplus.co.th" class="distributor" target="_blank" rel="noopener"
                                   tabindex="-1" aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE40;&#xE2D;&#xE40;&#xE0B;&#xE35;&#xE22; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-13.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">ASIA PLUS Security</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.dime.co.th" class="distributor" target="_blank" rel="noopener"
                                   tabindex="-1" aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE40;&#xE14;&#xE1F; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (Dime!) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-14.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">Dime</span>
                                </a>
                        </li>
                </ul>
        </div>
    </div>

    <div class="container">
        <div class="distributors__footer">
            <a href="/services/distributors" class="btn btn-split btn-light" aria-label="ดูตัวแทนขายทั้งหมด">
                <span>ดูทั้งหมด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>
    </div>
</section>', N'DistributorsV2', CAST(@gid AS nvarchar(20)), N'ตัวแทนขาย', N'Files/Site0/1/widget_icons/assetplus/icon-Distributors.png', N'', N'<section class="section distributors distributors--v2">
    <div class="container">
        <div class="distributors__head">
            <h2 class="distributors__title">
                ซื้อกองทุน <strong class="distributors__brand">Asset Plus</strong> ได้ผ่านตัวแทนขายชั้นนำ
            </h2>
        </div>
    </div>

    <div class="logo-marquee">
        <div class="logo-marquee__track">
                <ul class="logo-marquee__set">
                        <li class="logo-marquee__item">
                                <a href="https://www.scb.co.th" class="distributor" target="_blank" rel="noopener" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE44;&#xE17;&#xE22;&#xE1E;&#xE32;&#xE13;&#xE34;&#xE0A;&#xE22;&#xE4C; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-0.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">SCB</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.kasikornbank.com" class="distributor" target="_blank" rel="noopener" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE01;&#xE2A;&#xE34;&#xE01;&#xE23;&#xE44;&#xE17;&#xE22; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-1.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">KBank</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.gsb.or.th" class="distributor" target="_blank" rel="noopener" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE2D;&#xE2D;&#xE21;&#xE2A;&#xE34;&#xE19; (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-2.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">Government Savings Bank</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.krungsri.com" class="distributor" target="_blank" rel="noopener" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE01;&#xE23;&#xE38;&#xE07;&#xE28;&#xE23;&#xE35;&#xE2D;&#xE22;&#xE38;&#xE18;&#xE22;&#xE32; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-3.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">krungsri</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.bangkokbank.com" class="distributor" target="_blank" rel="noopener" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE01;&#xE23;&#xE38;&#xE07;&#xE40;&#xE17;&#xE1E; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-4.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">Bangkok Bank</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.ttbbank.com" class="distributor" target="_blank" rel="noopener" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE17;&#xE2B;&#xE32;&#xE23;&#xE44;&#xE17;&#xE22;&#xE18;&#xE19;&#xE0A;&#xE32;&#xE15; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-15.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">ttb</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.cimbthai.com" class="distributor" target="_blank" rel="noopener" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE0B;&#xE35;&#xE44;&#xE2D;&#xE40;&#xE2D;&#xE47;&#xE21;&#xE1A;&#xE35; &#xE44;&#xE17;&#xE22; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-5.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">CIMB</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.uob.co.th" class="distributor" target="_blank" rel="noopener" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE22;&#xE39;&#xE42;&#xE2D;&#xE1A;&#xE35; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-6.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">UOB</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.lhbank.co.th" class="distributor" target="_blank" rel="noopener" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE41;&#xE25;&#xE19;&#xE14;&#xE4C; &#xE41;&#xE2D;&#xE19;&#xE14;&#xE4C; &#xE40;&#xE2E;&#xE49;&#xE32;&#xE2A;&#xE4C; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-7.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">LH Bank</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://bank.kkpfg.com" class="distributor" target="_blank" rel="noopener" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE40;&#xE01;&#xE35;&#xE22;&#xE23;&#xE15;&#xE34;&#xE19;&#xE32;&#xE04;&#xE34;&#xE19;&#xE20;&#xE31;&#xE17;&#xE23; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-8.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">KKP</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.fnsyrus.com" class="distributor" target="_blank" rel="noopener" aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE1F;&#xE34;&#xE19;&#xE31;&#xE19;&#xE40;&#xE0B;&#xE35;&#xE22; &#xE44;&#xE0B;&#xE23;&#xE31;&#xE2A; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-9.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">FINANSIA</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.kgieworld.co.th" class="distributor" target="_blank" rel="noopener" aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE40;&#xE04;&#xE08;&#xE35;&#xE44;&#xE2D; (&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;&#xE44;&#xE17;&#xE22;) &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-10.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">KGI</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.aira.co.th" class="distributor" target="_blank" rel="noopener" aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE44;&#xE2D;&#xE23;&#xE48;&#xE32; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-11.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">AIRA</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.tisco.co.th" class="distributor" target="_blank" rel="noopener" aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE17;&#xE34;&#xE2A;&#xE42;&#xE01;&#xE49; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-12.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">TISCO</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.asiaplus.co.th" class="distributor" target="_blank" rel="noopener" aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE40;&#xE2D;&#xE40;&#xE0B;&#xE35;&#xE22; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-13.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">ASIA PLUS Security</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.dime.co.th" class="distributor" target="_blank" rel="noopener" aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE40;&#xE14;&#xE1F; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (Dime!) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-14.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">Dime</span>
                                </a>
                        </li>
                </ul>
                <ul class="logo-marquee__set" aria-hidden="true">
                        <li class="logo-marquee__item">
                                <a href="https://www.scb.co.th" class="distributor" target="_blank" rel="noopener"
                                   tabindex="-1" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE44;&#xE17;&#xE22;&#xE1E;&#xE32;&#xE13;&#xE34;&#xE0A;&#xE22;&#xE4C; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-0.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">SCB</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.kasikornbank.com" class="distributor" target="_blank" rel="noopener"
                                   tabindex="-1" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE01;&#xE2A;&#xE34;&#xE01;&#xE23;&#xE44;&#xE17;&#xE22; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-1.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">KBank</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.gsb.or.th" class="distributor" target="_blank" rel="noopener"
                                   tabindex="-1" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE2D;&#xE2D;&#xE21;&#xE2A;&#xE34;&#xE19; (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-2.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">Government Savings Bank</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.krungsri.com" class="distributor" target="_blank" rel="noopener"
                                   tabindex="-1" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE01;&#xE23;&#xE38;&#xE07;&#xE28;&#xE23;&#xE35;&#xE2D;&#xE22;&#xE38;&#xE18;&#xE22;&#xE32; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-3.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">krungsri</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.bangkokbank.com" class="distributor" target="_blank" rel="noopener"
                                   tabindex="-1" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE01;&#xE23;&#xE38;&#xE07;&#xE40;&#xE17;&#xE1E; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-4.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">Bangkok Bank</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.ttbbank.com" class="distributor" target="_blank" rel="noopener"
                                   tabindex="-1" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE17;&#xE2B;&#xE32;&#xE23;&#xE44;&#xE17;&#xE22;&#xE18;&#xE19;&#xE0A;&#xE32;&#xE15; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-15.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">ttb</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.cimbthai.com" class="distributor" target="_blank" rel="noopener"
                                   tabindex="-1" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE0B;&#xE35;&#xE44;&#xE2D;&#xE40;&#xE2D;&#xE47;&#xE21;&#xE1A;&#xE35; &#xE44;&#xE17;&#xE22; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-5.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">CIMB</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.uob.co.th" class="distributor" target="_blank" rel="noopener"
                                   tabindex="-1" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE22;&#xE39;&#xE42;&#xE2D;&#xE1A;&#xE35; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-6.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">UOB</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.lhbank.co.th" class="distributor" target="_blank" rel="noopener"
                                   tabindex="-1" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE41;&#xE25;&#xE19;&#xE14;&#xE4C; &#xE41;&#xE2D;&#xE19;&#xE14;&#xE4C; &#xE40;&#xE2E;&#xE49;&#xE32;&#xE2A;&#xE4C; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-7.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">LH Bank</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://bank.kkpfg.com" class="distributor" target="_blank" rel="noopener"
                                   tabindex="-1" aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE40;&#xE01;&#xE35;&#xE22;&#xE23;&#xE15;&#xE34;&#xE19;&#xE32;&#xE04;&#xE34;&#xE19;&#xE20;&#xE31;&#xE17;&#xE23; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-8.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">KKP</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.fnsyrus.com" class="distributor" target="_blank" rel="noopener"
                                   tabindex="-1" aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE1F;&#xE34;&#xE19;&#xE31;&#xE19;&#xE40;&#xE0B;&#xE35;&#xE22; &#xE44;&#xE0B;&#xE23;&#xE31;&#xE2A; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-9.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">FINANSIA</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.kgieworld.co.th" class="distributor" target="_blank" rel="noopener"
                                   tabindex="-1" aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE40;&#xE04;&#xE08;&#xE35;&#xE44;&#xE2D; (&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;&#xE44;&#xE17;&#xE22;) &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-10.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">KGI</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.aira.co.th" class="distributor" target="_blank" rel="noopener"
                                   tabindex="-1" aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE44;&#xE2D;&#xE23;&#xE48;&#xE32; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-11.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">AIRA</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.tisco.co.th" class="distributor" target="_blank" rel="noopener"
                                   tabindex="-1" aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE17;&#xE34;&#xE2A;&#xE42;&#xE01;&#xE49; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-12.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">TISCO</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.asiaplus.co.th" class="distributor" target="_blank" rel="noopener"
                                   tabindex="-1" aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE40;&#xE2D;&#xE40;&#xE0B;&#xE35;&#xE22; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-13.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">ASIA PLUS Security</span>
                                </a>
                        </li>
                        <li class="logo-marquee__item">
                                <a href="https://www.dime.co.th" class="distributor" target="_blank" rel="noopener"
                                   tabindex="-1" aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE40;&#xE14;&#xE1F; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (Dime!) (เปิดในแท็บใหม่)">
                                    <span class="distributor__logo">
                                        <img class="distributor__img" src="/media/images/agent/Thumb-14.jpg" alt="" loading="lazy"
                                             decoding="async" />
                                    </span>
                                    <span class="distributor__name">Dime</span>
                                </a>
                        </li>
                </ul>
        </div>
    </div>

    <div class="container">
        <div class="distributors__footer">
            <a href="/services/distributors" class="btn btn-split btn-light" aria-label="ดูตัวแทนขายทั้งหมด">
                <span>ดูทั้งหมด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>
    </div>
</section>', N'DistributorsV2', 0);
INSERT INTO [2026_web_widget_group] (created_at, updated_at, created_by, updated_by, sort, status, pb_status, approve_by, show_front, title, img1, pb_title, pb_img1, web_id) VALUES (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'user', N'user', 30, 1, 1, N'user', 1, N'CLASSIC', N'Files/Site0/1/widget_icons/assetplus/icon-group-classic.png', N'CLASSIC', N'Files/Site0/1/widget_icons/assetplus/icon-group-classic.png', 0);
SET @gid = SCOPE_IDENTITY(); INSERT INTO @g (n, id) VALUES (3, @gid);
INSERT INTO [2026_web_widget] (created_at, updated_at, created_by, updated_by, sort, status, pb_status, approve_by, show_front, cat_id, title, img1, mod_name, info, section_key, pb_cat_id, pb_title, pb_img1, pb_mod_name, pb_info, pb_section_key, web_id) VALUES (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'user', N'user', 10, 1, 1, N'user', 1, @gid, N'แบนเนอร์หน้าแรก', N'Files/Site0/1/widget_icons/assetplus/icon-Hero.png', N'HomeImageSlide', N'<section class="section hero hero--v3" aria-label="แบนเนอร์ไฮไลต์">
<div class="container-fluid">
<div class="swiper hero__slider js-hero-slider" data-slides-per-view="1.08" data-space-between="16">
<div class="swiper-wrapper">
|||REPEAT|||<div class="swiper-slide hero__slide" data-slide-id="|||id|||"><a href="|||pb_url|||" class="hero__link" target="_top"><picture><source media="(max-width: 767.98px)" srcset="/|||pb_img1_icon|||" /><img src="/|||pb_img1|||" alt="|||pb_title|||" class="hero__image" loading="lazy" decoding="async" /></picture></a></div>|||/REPEAT|||
</div>
</div>
<div class="hero__meta">
<p class="hero__index" aria-hidden="true"><span class="hero__index-current js-hero-counter">01</span><span class="hero__index-sep">/</span><span class="hero__index-total">03</span></p>
<div class="hero__line"><span class="hero__line-bar js-hero-progress-line"></span></div>
<button type="button" class="hero__mini-toggle js-hero-toggle" aria-pressed="false" aria-label="หยุดสไลด์ชั่วคราว"><i class="bi bi-play-fill" aria-hidden="true"></i></button>
</div>
</div>
</section>', N'HeroV3', CAST(@gid AS nvarchar(20)), N'แบนเนอร์หน้าแรก', N'Files/Site0/1/widget_icons/assetplus/icon-Hero.png', N'HomeImageSlide', N'<section class="section hero hero--v3" aria-label="แบนเนอร์ไฮไลต์">
<div class="container-fluid">
<div class="swiper hero__slider js-hero-slider" data-slides-per-view="1.08" data-space-between="16">
<div class="swiper-wrapper">
|||REPEAT|||<div class="swiper-slide hero__slide" data-slide-id="|||id|||"><a href="|||pb_url|||" class="hero__link" target="_top"><picture><source media="(max-width: 767.98px)" srcset="/|||pb_img1_icon|||" /><img src="/|||pb_img1|||" alt="|||pb_title|||" class="hero__image" loading="lazy" decoding="async" /></picture></a></div>|||/REPEAT|||
</div>
</div>
<div class="hero__meta">
<p class="hero__index" aria-hidden="true"><span class="hero__index-current js-hero-counter">01</span><span class="hero__index-sep">/</span><span class="hero__index-total">03</span></p>
<div class="hero__line"><span class="hero__line-bar js-hero-progress-line"></span></div>
<button type="button" class="hero__mini-toggle js-hero-toggle" aria-pressed="false" aria-label="หยุดสไลด์ชั่วคราว"><i class="bi bi-play-fill" aria-hidden="true"></i></button>
</div>
</div>
</section>', N'HeroV3', 0);
INSERT INTO [2026_web_widget] (created_at, updated_at, created_by, updated_by, sort, status, pb_status, approve_by, show_front, cat_id, title, img1, mod_name, info, section_key, pb_cat_id, pb_title, pb_img1, pb_mod_name, pb_info, pb_section_key, web_id) VALUES (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'user', N'user', 20, 1, 1, N'user', 1, @gid, N'มูลค่าหน่วยลงทุน', N'Files/Site0/1/widget_icons/assetplus/icon-NavPrices.png', N'', N'<section class="section nav-prices nav-prices--v3">
    <div class="container">
        <div class="row g-4">
            <!-- ตาราง NAV ย่อ — รูปแบบเดียวกับ v1 -->
            <div class="col-12 col-lg-6">
                <div class="nav-prices__card">
                    <div class="section-head">
                        <h2 class="section-head__title">มูลค่าหน่วยลงทุน</h2>
                        <a href="/funds/nav" class="btn btn-split btn-light" aria-label="ดูมูลค่าหน่วยลงทุนทั้งหมด">
                            <span>ดูทั้งหมด</span>
                            <span class="btn-split__icon">
                                <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                            </span>
                        </a>
                    </div>

                    <div class="nav-prices__table-wrap">
                        <table class="nav-table js-row-links">
                            <caption class="visually-hidden">
                                มูลค่าหน่วยลงทุนของกองทุนแนะนำ ข้อมูล ณ วันที่ 16 &#xE01;&#xE31;&#xE19;&#xE22;&#xE32;&#xE22;&#xE19; 2569
                            </caption>
                            <thead>
                                <tr>
                                    <th scope="col">กองทุน</th>
                                    <th scope="col">NAV (บาท)</th>
                                    <th scope="col" class="text-end">เปลี่ยนแปลง</th>
                                </tr>
                            </thead>
                            <tbody>
                                    <tr class="nav-table__row js-row-link" data-href="/funds/a-humanoid">
                                        <th scope="row" class="nav-table__code">
                                            <a href="/funds/a-humanoid" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2E;&#xE34;&#xE27;&#xE41;&#xE21;&#xE19;&#xE19;&#xE2D;&#xE22;&#xE14;&#xE4C;">A-HUMANOID</a>
                                        </th>
                                        <td class="nav-table__nav">11.5594</td>
                                        <td class="nav-table__change is-up">
                                            <i class="bi bi-caret-up-fill" aria-hidden="true"></i>
                                            <span>&#x2B;1.18%</span>
                                        </td>
                                    </tr>
                                    <tr class="nav-table__row js-row-link" data-href="/funds/a-grid">
                                        <th scope="row" class="nav-table__code">
                                            <a href="/funds/a-grid" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE01;&#xE23;&#xE34;&#xE14; &#xE2D;&#xE34;&#xE19;&#xE1F;&#xE23;&#xE32;&#xE2A;&#xE15;&#xE23;&#xE31;&#xE04;&#xE40;&#xE08;&#xE2D;&#xE23;&#xE4C;">A-GRID</a>
                                        </th>
                                        <td class="nav-table__nav">12.4180</td>
                                        <td class="nav-table__change is-up">
                                            <i class="bi bi-caret-up-fill" aria-hidden="true"></i>
                                            <span>&#x2B;0.50%</span>
                                        </td>
                                    </tr>
                                    <tr class="nav-table__row js-row-link" data-href="/funds/a-asemi">
                                        <th scope="row" class="nav-table__code">
                                            <a href="/funds/a-asemi" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE40;&#xE2D;&#xE40;&#xE0A;&#xE35;&#xE22; &#xE40;&#xE0B;&#xE21;&#xE34;&#xE04;&#xE2D;&#xE19;&#xE14;&#xE31;&#xE01;&#xE40;&#xE15;&#xE2D;&#xE23;&#xE4C;">A-ASEMI</a>
                                        </th>
                                        <td class="nav-table__nav">14.2075</td>
                                        <td class="nav-table__change is-up">
                                            <i class="bi bi-caret-up-fill" aria-hidden="true"></i>
                                            <span>&#x2B;1.38%</span>
                                        </td>
                                    </tr>
                                    <tr class="nav-table__row js-row-link" data-href="/funds/a-jedi">
                                        <th scope="row" class="nav-table__code">
                                            <a href="/funds/a-jedi" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2A;&#xE40;&#xE1B;&#xE0B; &#xE2D;&#xE35;&#xE42;&#xE04;&#xE42;&#xE19;&#xE21;&#xE35;">A-JEDI</a>
                                        </th>
                                        <td class="nav-table__nav">9.8742</td>
                                        <td class="nav-table__change is-down">
                                            <i class="bi bi-caret-down-fill" aria-hidden="true"></i>
                                            <span>-0.46%</span>
                                        </td>
                                    </tr>
                                    <tr class="nav-table__row js-row-link" data-href="/funds/asp-crypto">
                                        <th scope="row" class="nav-table__code">
                                            <a href="/funds/asp-crypto" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE14;&#xE34;&#xE08;&#xE34;&#xE17;&#xE31;&#xE25; &#xE1A;&#xE25;&#xE47;&#xE2D;&#xE01;&#xE40;&#xE0A;&#xE19;">ASP-CRYPTO</a>
                                        </th>
                                        <td class="nav-table__nav">15.8410</td>
                                        <td class="nav-table__change is-up">
                                            <i class="bi bi-caret-up-fill" aria-hidden="true"></i>
                                            <span>&#x2B;1.55%</span>
                                        </td>
                                    </tr>
                            </tbody>
                        </table>
                    </div>

                    <p class="nav-prices__asof">
                        ข้อมูล ณ วันที่ 16 &#xE01;&#xE31;&#xE19;&#xE22;&#xE32;&#xE22;&#xE19; 2569
                        เวลา 18:00 น.
                    </p>
                </div>
            </div>

            <!-- Quick tiles — การ์ดพื้นสีทึบ + ไอคอน -->
            <div class="col-12 col-lg-6">
                <div class="tile-stack">
                    <a href="/funds/performance" class="tile-stack__item tile-stack__item--primary">
                        <span class="tile-stack__icon">
                            <i class="bi bi-graph-up-arrow" aria-hidden="true"></i>
                        </span>
                        <span class="tile-stack__body">
                            <span class="tile-stack__title">ผลการดำเนินงานทั้งหมด</span>
                            <span class="tile-stack__text">เปรียบเทียบผลตอบแทนย้อนหลังของแต่ละกองทุน</span>
                        </span>
                        <i class="bi bi-arrow-right tile-stack__arrow" aria-hidden="true"></i>
                    </a>

                    <a href="/funds/nav" class="tile-stack__item tile-stack__item--primary">
                        <span class="tile-stack__icon">
                            <i class="bi bi-currency-exchange" aria-hidden="true"></i>
                        </span>
                        <span class="tile-stack__body">
                            <span class="tile-stack__title">มูลค่าหน่วยลงทุนทั้งหมด</span>
                            <span class="tile-stack__text">ค้นหามูลค่าหน่วยลงทุน (NAV) ย้อนหลังของทุกกองทุน</span>
                        </span>
                        <i class="bi bi-arrow-right tile-stack__arrow" aria-hidden="true"></i>
                    </a>

                    <a href="/funds/calendar" class="tile-stack__item tile-stack__item--primary">
                        <span class="tile-stack__icon">
                            <i class="bi bi-calendar3" aria-hidden="true"></i>
                        </span>
                        <span class="tile-stack__body">
                            <span class="tile-stack__title">ปฏิทินกองทุน</span>
                            <span class="tile-stack__text">ตารางวันทำการซื้อขายและวันหยุดของแต่ละกองทุน</span>
                        </span>
                        <i class="bi bi-arrow-right tile-stack__arrow" aria-hidden="true"></i>
                    </a>
                </div>
            </div>
        </div>
    </div>
</section>', N'NavPricesV3', CAST(@gid AS nvarchar(20)), N'มูลค่าหน่วยลงทุน', N'Files/Site0/1/widget_icons/assetplus/icon-NavPrices.png', N'', N'<section class="section nav-prices nav-prices--v3">
    <div class="container">
        <div class="row g-4">
            <!-- ตาราง NAV ย่อ — รูปแบบเดียวกับ v1 -->
            <div class="col-12 col-lg-6">
                <div class="nav-prices__card">
                    <div class="section-head">
                        <h2 class="section-head__title">มูลค่าหน่วยลงทุน</h2>
                        <a href="/funds/nav" class="btn btn-split btn-light" aria-label="ดูมูลค่าหน่วยลงทุนทั้งหมด">
                            <span>ดูทั้งหมด</span>
                            <span class="btn-split__icon">
                                <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                            </span>
                        </a>
                    </div>

                    <div class="nav-prices__table-wrap">
                        <table class="nav-table js-row-links">
                            <caption class="visually-hidden">
                                มูลค่าหน่วยลงทุนของกองทุนแนะนำ ข้อมูล ณ วันที่ 16 &#xE01;&#xE31;&#xE19;&#xE22;&#xE32;&#xE22;&#xE19; 2569
                            </caption>
                            <thead>
                                <tr>
                                    <th scope="col">กองทุน</th>
                                    <th scope="col">NAV (บาท)</th>
                                    <th scope="col" class="text-end">เปลี่ยนแปลง</th>
                                </tr>
                            </thead>
                            <tbody>
                                    <tr class="nav-table__row js-row-link" data-href="/funds/a-humanoid">
                                        <th scope="row" class="nav-table__code">
                                            <a href="/funds/a-humanoid" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2E;&#xE34;&#xE27;&#xE41;&#xE21;&#xE19;&#xE19;&#xE2D;&#xE22;&#xE14;&#xE4C;">A-HUMANOID</a>
                                        </th>
                                        <td class="nav-table__nav">11.5594</td>
                                        <td class="nav-table__change is-up">
                                            <i class="bi bi-caret-up-fill" aria-hidden="true"></i>
                                            <span>&#x2B;1.18%</span>
                                        </td>
                                    </tr>
                                    <tr class="nav-table__row js-row-link" data-href="/funds/a-grid">
                                        <th scope="row" class="nav-table__code">
                                            <a href="/funds/a-grid" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE01;&#xE23;&#xE34;&#xE14; &#xE2D;&#xE34;&#xE19;&#xE1F;&#xE23;&#xE32;&#xE2A;&#xE15;&#xE23;&#xE31;&#xE04;&#xE40;&#xE08;&#xE2D;&#xE23;&#xE4C;">A-GRID</a>
                                        </th>
                                        <td class="nav-table__nav">12.4180</td>
                                        <td class="nav-table__change is-up">
                                            <i class="bi bi-caret-up-fill" aria-hidden="true"></i>
                                            <span>&#x2B;0.50%</span>
                                        </td>
                                    </tr>
                                    <tr class="nav-table__row js-row-link" data-href="/funds/a-asemi">
                                        <th scope="row" class="nav-table__code">
                                            <a href="/funds/a-asemi" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE40;&#xE2D;&#xE40;&#xE0A;&#xE35;&#xE22; &#xE40;&#xE0B;&#xE21;&#xE34;&#xE04;&#xE2D;&#xE19;&#xE14;&#xE31;&#xE01;&#xE40;&#xE15;&#xE2D;&#xE23;&#xE4C;">A-ASEMI</a>
                                        </th>
                                        <td class="nav-table__nav">14.2075</td>
                                        <td class="nav-table__change is-up">
                                            <i class="bi bi-caret-up-fill" aria-hidden="true"></i>
                                            <span>&#x2B;1.38%</span>
                                        </td>
                                    </tr>
                                    <tr class="nav-table__row js-row-link" data-href="/funds/a-jedi">
                                        <th scope="row" class="nav-table__code">
                                            <a href="/funds/a-jedi" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2A;&#xE40;&#xE1B;&#xE0B; &#xE2D;&#xE35;&#xE42;&#xE04;&#xE42;&#xE19;&#xE21;&#xE35;">A-JEDI</a>
                                        </th>
                                        <td class="nav-table__nav">9.8742</td>
                                        <td class="nav-table__change is-down">
                                            <i class="bi bi-caret-down-fill" aria-hidden="true"></i>
                                            <span>-0.46%</span>
                                        </td>
                                    </tr>
                                    <tr class="nav-table__row js-row-link" data-href="/funds/asp-crypto">
                                        <th scope="row" class="nav-table__code">
                                            <a href="/funds/asp-crypto" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE14;&#xE34;&#xE08;&#xE34;&#xE17;&#xE31;&#xE25; &#xE1A;&#xE25;&#xE47;&#xE2D;&#xE01;&#xE40;&#xE0A;&#xE19;">ASP-CRYPTO</a>
                                        </th>
                                        <td class="nav-table__nav">15.8410</td>
                                        <td class="nav-table__change is-up">
                                            <i class="bi bi-caret-up-fill" aria-hidden="true"></i>
                                            <span>&#x2B;1.55%</span>
                                        </td>
                                    </tr>
                            </tbody>
                        </table>
                    </div>

                    <p class="nav-prices__asof">
                        ข้อมูล ณ วันที่ 16 &#xE01;&#xE31;&#xE19;&#xE22;&#xE32;&#xE22;&#xE19; 2569
                        เวลา 18:00 น.
                    </p>
                </div>
            </div>

            <!-- Quick tiles — การ์ดพื้นสีทึบ + ไอคอน -->
            <div class="col-12 col-lg-6">
                <div class="tile-stack">
                    <a href="/funds/performance" class="tile-stack__item tile-stack__item--primary">
                        <span class="tile-stack__icon">
                            <i class="bi bi-graph-up-arrow" aria-hidden="true"></i>
                        </span>
                        <span class="tile-stack__body">
                            <span class="tile-stack__title">ผลการดำเนินงานทั้งหมด</span>
                            <span class="tile-stack__text">เปรียบเทียบผลตอบแทนย้อนหลังของแต่ละกองทุน</span>
                        </span>
                        <i class="bi bi-arrow-right tile-stack__arrow" aria-hidden="true"></i>
                    </a>

                    <a href="/funds/nav" class="tile-stack__item tile-stack__item--primary">
                        <span class="tile-stack__icon">
                            <i class="bi bi-currency-exchange" aria-hidden="true"></i>
                        </span>
                        <span class="tile-stack__body">
                            <span class="tile-stack__title">มูลค่าหน่วยลงทุนทั้งหมด</span>
                            <span class="tile-stack__text">ค้นหามูลค่าหน่วยลงทุน (NAV) ย้อนหลังของทุกกองทุน</span>
                        </span>
                        <i class="bi bi-arrow-right tile-stack__arrow" aria-hidden="true"></i>
                    </a>

                    <a href="/funds/calendar" class="tile-stack__item tile-stack__item--primary">
                        <span class="tile-stack__icon">
                            <i class="bi bi-calendar3" aria-hidden="true"></i>
                        </span>
                        <span class="tile-stack__body">
                            <span class="tile-stack__title">ปฏิทินกองทุน</span>
                            <span class="tile-stack__text">ตารางวันทำการซื้อขายและวันหยุดของแต่ละกองทุน</span>
                        </span>
                        <i class="bi bi-arrow-right tile-stack__arrow" aria-hidden="true"></i>
                    </a>
                </div>
            </div>
        </div>
    </div>
</section>', N'NavPricesV3', 0);
INSERT INTO [2026_web_widget] (created_at, updated_at, created_by, updated_by, sort, status, pb_status, approve_by, show_front, cat_id, title, img1, mod_name, info, section_key, pb_cat_id, pb_title, pb_img1, pb_mod_name, pb_info, pb_section_key, web_id) VALUES (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'user', N'user', 30, 1, 1, N'user', 1, @gid, N'กองทุนแนะนำประจำเดือน', N'Files/Site0/1/widget_icons/assetplus/icon-FeaturedFunds.png', N'', N'<section class="section featured-funds featured-funds--v3">
    <div class="container">
        <div class="section-head">
            <h2 class="section-head__title">กองทุนแนะนำประจำเดือน</h2>
            <a href="/funds/featured" class="btn btn-split btn-light" aria-label="ดูกองทุนแนะนำทั้งหมด">
                <span>ดูทั้งหมด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>

        <div class="featured-funds__carousel swiper js-featured-swiper">
            <div class="swiper-wrapper card-deck card-deck--cards-1 card-deck--cards-md-2 card-deck--cards-lg-3">
                    <div class="swiper-slide">
                        
<article class="card card--fund">
    <a href="/funds/a-humanoid" class="card__image" tabindex="-1" aria-hidden="true">
        <img src="/media/images/home/fund/a-humanoid.jpg" alt="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2E;&#xE34;&#xE27;&#xE41;&#xE21;&#xE19;&#xE19;&#xE2D;&#xE22;&#xE14;&#xE4C;" loading="lazy" decoding="async" />
    </a>


    <div class="card__body">
        <a href="/funds/a-humanoid" class="card__heading" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2E;&#xE34;&#xE27;&#xE41;&#xE21;&#xE19;&#xE19;&#xE2D;&#xE22;&#xE14;&#xE4C;">
            <h3 class="card__title card__code">A-HUMANOID</h3>
            <span class="card__meta card__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;/&#xE2B;&#xE38;&#xE49;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
        </a>

        <p class="card__text card__summary">&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE19;&#xE18;&#xE38;&#xE23;&#xE01;&#xE34;&#xE08;&#xE2B;&#xE38;&#xE48;&#xE19;&#xE22;&#xE19;&#xE15;&#xE4C;&#xE2E;&#xE34;&#xE27;&#xE41;&#xE21;&#xE19;&#xE19;&#xE2D;&#xE22;&#xE14;&#xE4C;&#xE41;&#xE25;&#xE30; AI &#xE23;&#xE30;&#xE14;&#xE31;&#xE1A;&#xE42;&#xE25;&#xE01; &#xE17;&#xE35;&#xE48;&#xE01;&#xE33;&#xE25;&#xE31;&#xE07;&#xE40;&#xE1B;&#xE25;&#xE35;&#xE48;&#xE22;&#xE19;&#xE42;&#xE09;&#xE21;&#xE20;&#xE32;&#xE04;&#xE01;&#xE32;&#xE23;&#xE1C;&#xE25;&#xE34;&#xE15;&#xE41;&#xE25;&#xE30;&#xE1A;&#xE23;&#xE34;&#xE01;&#xE32;&#xE23;</p>

            
    <span class="rating">
        <img src="/media/images/morningstar/5-star.png" class="rating__image" alt="Morningstar Rating 5 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

    </div>

    <div class="card__footer">
        <div class="card__actions">
            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--7">7</span>
</span>


            <a href="/funds/a-humanoid" class="btn btn-split" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2E;&#xE34;&#xE27;&#xE41;&#xE21;&#xE19;&#xE19;&#xE2D;&#xE22;&#xE14;&#xE4C;">
                <span>ดูรายละเอียด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>

    </div>
</article>

                    </div>
                    <div class="swiper-slide">
                        
<article class="card card--fund">
    <a href="/funds/a-grid" class="card__image" tabindex="-1" aria-hidden="true">
        <img src="/media/images/home/fund/a-grid.jpg" alt="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE01;&#xE23;&#xE34;&#xE14; &#xE2D;&#xE34;&#xE19;&#xE1F;&#xE23;&#xE32;&#xE2A;&#xE15;&#xE23;&#xE31;&#xE04;&#xE40;&#xE08;&#xE2D;&#xE23;&#xE4C;" loading="lazy" decoding="async" />
    </a>


    <div class="card__body">
        <a href="/funds/a-grid" class="card__heading" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE01;&#xE23;&#xE34;&#xE14; &#xE2D;&#xE34;&#xE19;&#xE1F;&#xE23;&#xE32;&#xE2A;&#xE15;&#xE23;&#xE31;&#xE04;&#xE40;&#xE08;&#xE2D;&#xE23;&#xE4C;">
            <h3 class="card__title card__code">A-GRID</h3>
            <span class="card__meta card__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;/&#xE2B;&#xE38;&#xE49;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
        </a>

        <p class="card__text card__summary">&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE19;&#xE42;&#xE04;&#xE23;&#xE07;&#xE2A;&#xE23;&#xE49;&#xE32;&#xE07;&#xE1E;&#xE37;&#xE49;&#xE19;&#xE10;&#xE32;&#xE19;&#xE14;&#xE49;&#xE32;&#xE19;&#xE1E;&#xE25;&#xE31;&#xE07;&#xE07;&#xE32;&#xE19;&#xE41;&#xE25;&#xE30;&#xE23;&#xE30;&#xE1A;&#xE1A; Smart Grid &#xE17;&#xE35;&#xE48;&#xE40;&#xE1B;&#xE47;&#xE19;&#xE2B;&#xE31;&#xE27;&#xE43;&#xE08;&#xE02;&#xE2D;&#xE07;&#xE42;&#xE25;&#xE01;&#xE2D;&#xE19;&#xE32;&#xE04;&#xE15;</p>

            
    <span class="rating">
        <img src="/media/images/morningstar/4-star.png" class="rating__image" alt="Morningstar Rating 4 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

    </div>

    <div class="card__footer">
        <div class="card__actions">
            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--3">3</span>
</span>


            <a href="/funds/a-grid" class="btn btn-split" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE01;&#xE23;&#xE34;&#xE14; &#xE2D;&#xE34;&#xE19;&#xE1F;&#xE23;&#xE32;&#xE2A;&#xE15;&#xE23;&#xE31;&#xE04;&#xE40;&#xE08;&#xE2D;&#xE23;&#xE4C;">
                <span>ดูรายละเอียด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>

    </div>
</article>

                    </div>
                    <div class="swiper-slide">
                        
<article class="card card--fund">
    <a href="/funds/a-asemi" class="card__image" tabindex="-1" aria-hidden="true">
        <img src="/media/images/home/fund/a-asemi.jpg" alt="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE40;&#xE2D;&#xE40;&#xE0A;&#xE35;&#xE22; &#xE40;&#xE0B;&#xE21;&#xE34;&#xE04;&#xE2D;&#xE19;&#xE14;&#xE31;&#xE01;&#xE40;&#xE15;&#xE2D;&#xE23;&#xE4C;" loading="lazy" decoding="async" />
    </a>


    <div class="card__body">
        <a href="/funds/a-asemi" class="card__heading" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE40;&#xE2D;&#xE40;&#xE0A;&#xE35;&#xE22; &#xE40;&#xE0B;&#xE21;&#xE34;&#xE04;&#xE2D;&#xE19;&#xE14;&#xE31;&#xE01;&#xE40;&#xE15;&#xE2D;&#xE23;&#xE4C;">
            <h3 class="card__title card__code">A-ASEMI</h3>
            <span class="card__meta card__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;/&#xE2B;&#xE38;&#xE49;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
        </a>

        <p class="card__text card__summary">&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE19;&#xE2B;&#xE48;&#xE27;&#xE07;&#xE42;&#xE0B;&#xE48;&#xE01;&#xE32;&#xE23;&#xE1C;&#xE25;&#xE34;&#xE15;&#xE40;&#xE0B;&#xE21;&#xE34;&#xE04;&#xE2D;&#xE19;&#xE14;&#xE31;&#xE01;&#xE40;&#xE15;&#xE2D;&#xE23;&#xE4C;&#xE41;&#xE2B;&#xE48;&#xE07;&#xE40;&#xE2D;&#xE40;&#xE0A;&#xE35;&#xE22; &#xE15;&#xE31;&#xE49;&#xE07;&#xE41;&#xE15;&#xE48;&#xE15;&#xE49;&#xE19;&#xE19;&#xE49;&#xE33;&#xE16;&#xE36;&#xE07;&#xE1B;&#xE25;&#xE32;&#xE22;&#xE19;&#xE49;&#xE33;</p>

            
    <span class="rating">
        <img src="/media/images/morningstar/4-star.png" class="rating__image" alt="Morningstar Rating 4 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

    </div>

    <div class="card__footer">
        <div class="card__actions">
            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--7">7</span>
</span>


            <a href="/funds/a-asemi" class="btn btn-split" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE40;&#xE2D;&#xE40;&#xE0A;&#xE35;&#xE22; &#xE40;&#xE0B;&#xE21;&#xE34;&#xE04;&#xE2D;&#xE19;&#xE14;&#xE31;&#xE01;&#xE40;&#xE15;&#xE2D;&#xE23;&#xE4C;">
                <span>ดูรายละเอียด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>

    </div>
</article>

                    </div>
                    <div class="swiper-slide">
                        
<article class="card card--fund">
    <a href="/funds/a-jedi" class="card__image" tabindex="-1" aria-hidden="true">
        <img src="/media/images/home/fund/a-jedi.jpg" alt="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2A;&#xE40;&#xE1B;&#xE0B; &#xE2D;&#xE35;&#xE42;&#xE04;&#xE42;&#xE19;&#xE21;&#xE35;" loading="lazy" decoding="async" />
    </a>


    <div class="card__body">
        <a href="/funds/a-jedi" class="card__heading" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2A;&#xE40;&#xE1B;&#xE0B; &#xE2D;&#xE35;&#xE42;&#xE04;&#xE42;&#xE19;&#xE21;&#xE35;">
            <h3 class="card__title card__code">A-JEDI</h3>
            <span class="card__meta card__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;/&#xE2B;&#xE38;&#xE49;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
        </a>

        <p class="card__text card__summary">&#xE42;&#xE2D;&#xE01;&#xE32;&#xE2A;&#xE40;&#xE15;&#xE34;&#xE1A;&#xE42;&#xE15;&#xE08;&#xE32;&#xE01;&#xE2D;&#xE38;&#xE15;&#xE2A;&#xE32;&#xE2B;&#xE01;&#xE23;&#xE23;&#xE21;&#xE2D;&#xE27;&#xE01;&#xE32;&#xE28;&#xE41;&#xE25;&#xE30;&#xE14;&#xE32;&#xE27;&#xE40;&#xE17;&#xE35;&#xE22;&#xE21;&#xE17;&#xE35;&#xE48;&#xE02;&#xE22;&#xE32;&#xE22;&#xE15;&#xE31;&#xE27;&#xE15;&#xE48;&#xE2D;&#xE40;&#xE19;&#xE37;&#xE48;&#xE2D;&#xE07;</p>

            
    <span class="rating">
        <img src="/media/images/morningstar/5-star.png" class="rating__image" alt="Morningstar Rating 5 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

    </div>

    <div class="card__footer">
        <div class="card__actions">
            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--7">7</span>
</span>


            <a href="/funds/a-jedi" class="btn btn-split" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2A;&#xE40;&#xE1B;&#xE0B; &#xE2D;&#xE35;&#xE42;&#xE04;&#xE42;&#xE19;&#xE21;&#xE35;">
                <span>ดูรายละเอียด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>

    </div>
</article>

                    </div>
                    <div class="swiper-slide">
                        
<article class="card card--fund">
    <a href="/funds/asp-crypto" class="card__image" tabindex="-1" aria-hidden="true">
        <img src="/media/images/home/fund/asp-crypto.jpg" alt="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE14;&#xE34;&#xE08;&#xE34;&#xE17;&#xE31;&#xE25; &#xE1A;&#xE25;&#xE47;&#xE2D;&#xE01;&#xE40;&#xE0A;&#xE19;" loading="lazy" decoding="async" />
    </a>


    <div class="card__body">
        <a href="/funds/asp-crypto" class="card__heading" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE14;&#xE34;&#xE08;&#xE34;&#xE17;&#xE31;&#xE25; &#xE1A;&#xE25;&#xE47;&#xE2D;&#xE01;&#xE40;&#xE0A;&#xE19;">
            <h3 class="card__title card__code">ASP-CRYPTO</h3>
            <span class="card__meta card__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;/&#xE2B;&#xE38;&#xE49;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
        </a>

        <p class="card__text card__summary">&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE19;&#xE18;&#xE38;&#xE23;&#xE01;&#xE34;&#xE08;&#xE41;&#xE1E;&#xE25;&#xE15;&#xE1F;&#xE2D;&#xE23;&#xE4C;&#xE21;&#xE14;&#xE34;&#xE08;&#xE34;&#xE17;&#xE31;&#xE25; &#xE41;&#xE25;&#xE30;&#xE40;&#xE17;&#xE04;&#xE42;&#xE19;&#xE42;&#xE25;&#xE22;&#xE35;&#xE1A;&#xE25;&#xE47;&#xE2D;&#xE01;&#xE40;&#xE0A;&#xE19;&#xE17;&#xE31;&#xE48;&#xE27;&#xE42;&#xE25;&#xE01;</p>

            
    <span class="rating">
        <img src="/media/images/morningstar/5-star.png" class="rating__image" alt="Morningstar Rating 5 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

    </div>

    <div class="card__footer">
        <div class="card__actions">
            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--8">8&#x2B;</span>
</span>


            <a href="/funds/asp-crypto" class="btn btn-split" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE14;&#xE34;&#xE08;&#xE34;&#xE17;&#xE31;&#xE25; &#xE1A;&#xE25;&#xE47;&#xE2D;&#xE01;&#xE40;&#xE0A;&#xE19;">
                <span>ดูรายละเอียด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>

    </div>
</article>

                    </div>
                    <div class="swiper-slide">
                        
<article class="card card--fund">
    <a href="/funds/a-ring" class="card__image" tabindex="-1" aria-hidden="true">
        <img src="/media/images/home/fund/a-ring.jpg" alt="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE42;&#xE01;&#xE25;&#xE14;&#xE4C;" loading="lazy" decoding="async" />
    </a>


    <div class="card__body">
        <a href="/funds/a-ring" class="card__heading" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE42;&#xE01;&#xE25;&#xE14;&#xE4C;">
            <h3 class="card__title card__code">A-RING</h3>
            <span class="card__meta card__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C;&#xE2A;&#xE34;&#xE19;&#xE17;&#xE32;&#xE07;&#xE40;&#xE25;&#xE37;&#xE2D;&#xE01;</span>
        </a>

        <p class="card__text card__summary">&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE19;&#xE17;&#xE2D;&#xE07;&#xE04;&#xE33; &#xE1B;&#xE49;&#xE2D;&#xE07;&#xE01;&#xE31;&#xE19;&#xE04;&#xE27;&#xE32;&#xE21;&#xE40;&#xE2A;&#xE35;&#xE48;&#xE22;&#xE07; &#xE2A;&#xE23;&#xE49;&#xE32;&#xE07;&#xE2A;&#xE21;&#xE14;&#xE38;&#xE25;&#xE1E;&#xE2D;&#xE23;&#xE4C;&#xE15;&#xE01;&#xE32;&#xE23;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;</p>

            
    <span class="rating">
        <img src="/media/images/morningstar/4-star.png" class="rating__image" alt="Morningstar Rating 4 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

    </div>

    <div class="card__footer">
        <div class="card__actions">
            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--8">8</span>
</span>


            <a href="/funds/a-ring" class="btn btn-split" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE42;&#xE01;&#xE25;&#xE14;&#xE4C;">
                <span>ดูรายละเอียด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>

    </div>
</article>

                    </div>
            </div>

            <div class="featured-funds__controls">
                <div class="slider-dots js-featured-pagination"></div>
            </div>
        </div>
    </div>
</section>', N'FeaturedFundsV3', CAST(@gid AS nvarchar(20)), N'กองทุนแนะนำประจำเดือน', N'Files/Site0/1/widget_icons/assetplus/icon-FeaturedFunds.png', N'', N'<section class="section featured-funds featured-funds--v3">
    <div class="container">
        <div class="section-head">
            <h2 class="section-head__title">กองทุนแนะนำประจำเดือน</h2>
            <a href="/funds/featured" class="btn btn-split btn-light" aria-label="ดูกองทุนแนะนำทั้งหมด">
                <span>ดูทั้งหมด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>

        <div class="featured-funds__carousel swiper js-featured-swiper">
            <div class="swiper-wrapper card-deck card-deck--cards-1 card-deck--cards-md-2 card-deck--cards-lg-3">
                    <div class="swiper-slide">
                        
<article class="card card--fund">
    <a href="/funds/a-humanoid" class="card__image" tabindex="-1" aria-hidden="true">
        <img src="/media/images/home/fund/a-humanoid.jpg" alt="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2E;&#xE34;&#xE27;&#xE41;&#xE21;&#xE19;&#xE19;&#xE2D;&#xE22;&#xE14;&#xE4C;" loading="lazy" decoding="async" />
    </a>


    <div class="card__body">
        <a href="/funds/a-humanoid" class="card__heading" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2E;&#xE34;&#xE27;&#xE41;&#xE21;&#xE19;&#xE19;&#xE2D;&#xE22;&#xE14;&#xE4C;">
            <h3 class="card__title card__code">A-HUMANOID</h3>
            <span class="card__meta card__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;/&#xE2B;&#xE38;&#xE49;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
        </a>

        <p class="card__text card__summary">&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE19;&#xE18;&#xE38;&#xE23;&#xE01;&#xE34;&#xE08;&#xE2B;&#xE38;&#xE48;&#xE19;&#xE22;&#xE19;&#xE15;&#xE4C;&#xE2E;&#xE34;&#xE27;&#xE41;&#xE21;&#xE19;&#xE19;&#xE2D;&#xE22;&#xE14;&#xE4C;&#xE41;&#xE25;&#xE30; AI &#xE23;&#xE30;&#xE14;&#xE31;&#xE1A;&#xE42;&#xE25;&#xE01; &#xE17;&#xE35;&#xE48;&#xE01;&#xE33;&#xE25;&#xE31;&#xE07;&#xE40;&#xE1B;&#xE25;&#xE35;&#xE48;&#xE22;&#xE19;&#xE42;&#xE09;&#xE21;&#xE20;&#xE32;&#xE04;&#xE01;&#xE32;&#xE23;&#xE1C;&#xE25;&#xE34;&#xE15;&#xE41;&#xE25;&#xE30;&#xE1A;&#xE23;&#xE34;&#xE01;&#xE32;&#xE23;</p>

            
    <span class="rating">
        <img src="/media/images/morningstar/5-star.png" class="rating__image" alt="Morningstar Rating 5 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

    </div>

    <div class="card__footer">
        <div class="card__actions">
            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--7">7</span>
</span>


            <a href="/funds/a-humanoid" class="btn btn-split" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2E;&#xE34;&#xE27;&#xE41;&#xE21;&#xE19;&#xE19;&#xE2D;&#xE22;&#xE14;&#xE4C;">
                <span>ดูรายละเอียด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>

    </div>
</article>

                    </div>
                    <div class="swiper-slide">
                        
<article class="card card--fund">
    <a href="/funds/a-grid" class="card__image" tabindex="-1" aria-hidden="true">
        <img src="/media/images/home/fund/a-grid.jpg" alt="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE01;&#xE23;&#xE34;&#xE14; &#xE2D;&#xE34;&#xE19;&#xE1F;&#xE23;&#xE32;&#xE2A;&#xE15;&#xE23;&#xE31;&#xE04;&#xE40;&#xE08;&#xE2D;&#xE23;&#xE4C;" loading="lazy" decoding="async" />
    </a>


    <div class="card__body">
        <a href="/funds/a-grid" class="card__heading" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE01;&#xE23;&#xE34;&#xE14; &#xE2D;&#xE34;&#xE19;&#xE1F;&#xE23;&#xE32;&#xE2A;&#xE15;&#xE23;&#xE31;&#xE04;&#xE40;&#xE08;&#xE2D;&#xE23;&#xE4C;">
            <h3 class="card__title card__code">A-GRID</h3>
            <span class="card__meta card__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;/&#xE2B;&#xE38;&#xE49;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
        </a>

        <p class="card__text card__summary">&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE19;&#xE42;&#xE04;&#xE23;&#xE07;&#xE2A;&#xE23;&#xE49;&#xE32;&#xE07;&#xE1E;&#xE37;&#xE49;&#xE19;&#xE10;&#xE32;&#xE19;&#xE14;&#xE49;&#xE32;&#xE19;&#xE1E;&#xE25;&#xE31;&#xE07;&#xE07;&#xE32;&#xE19;&#xE41;&#xE25;&#xE30;&#xE23;&#xE30;&#xE1A;&#xE1A; Smart Grid &#xE17;&#xE35;&#xE48;&#xE40;&#xE1B;&#xE47;&#xE19;&#xE2B;&#xE31;&#xE27;&#xE43;&#xE08;&#xE02;&#xE2D;&#xE07;&#xE42;&#xE25;&#xE01;&#xE2D;&#xE19;&#xE32;&#xE04;&#xE15;</p>

            
    <span class="rating">
        <img src="/media/images/morningstar/4-star.png" class="rating__image" alt="Morningstar Rating 4 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

    </div>

    <div class="card__footer">
        <div class="card__actions">
            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--3">3</span>
</span>


            <a href="/funds/a-grid" class="btn btn-split" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE01;&#xE23;&#xE34;&#xE14; &#xE2D;&#xE34;&#xE19;&#xE1F;&#xE23;&#xE32;&#xE2A;&#xE15;&#xE23;&#xE31;&#xE04;&#xE40;&#xE08;&#xE2D;&#xE23;&#xE4C;">
                <span>ดูรายละเอียด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>

    </div>
</article>

                    </div>
                    <div class="swiper-slide">
                        
<article class="card card--fund">
    <a href="/funds/a-asemi" class="card__image" tabindex="-1" aria-hidden="true">
        <img src="/media/images/home/fund/a-asemi.jpg" alt="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE40;&#xE2D;&#xE40;&#xE0A;&#xE35;&#xE22; &#xE40;&#xE0B;&#xE21;&#xE34;&#xE04;&#xE2D;&#xE19;&#xE14;&#xE31;&#xE01;&#xE40;&#xE15;&#xE2D;&#xE23;&#xE4C;" loading="lazy" decoding="async" />
    </a>


    <div class="card__body">
        <a href="/funds/a-asemi" class="card__heading" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE40;&#xE2D;&#xE40;&#xE0A;&#xE35;&#xE22; &#xE40;&#xE0B;&#xE21;&#xE34;&#xE04;&#xE2D;&#xE19;&#xE14;&#xE31;&#xE01;&#xE40;&#xE15;&#xE2D;&#xE23;&#xE4C;">
            <h3 class="card__title card__code">A-ASEMI</h3>
            <span class="card__meta card__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;/&#xE2B;&#xE38;&#xE49;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
        </a>

        <p class="card__text card__summary">&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE19;&#xE2B;&#xE48;&#xE27;&#xE07;&#xE42;&#xE0B;&#xE48;&#xE01;&#xE32;&#xE23;&#xE1C;&#xE25;&#xE34;&#xE15;&#xE40;&#xE0B;&#xE21;&#xE34;&#xE04;&#xE2D;&#xE19;&#xE14;&#xE31;&#xE01;&#xE40;&#xE15;&#xE2D;&#xE23;&#xE4C;&#xE41;&#xE2B;&#xE48;&#xE07;&#xE40;&#xE2D;&#xE40;&#xE0A;&#xE35;&#xE22; &#xE15;&#xE31;&#xE49;&#xE07;&#xE41;&#xE15;&#xE48;&#xE15;&#xE49;&#xE19;&#xE19;&#xE49;&#xE33;&#xE16;&#xE36;&#xE07;&#xE1B;&#xE25;&#xE32;&#xE22;&#xE19;&#xE49;&#xE33;</p>

            
    <span class="rating">
        <img src="/media/images/morningstar/4-star.png" class="rating__image" alt="Morningstar Rating 4 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

    </div>

    <div class="card__footer">
        <div class="card__actions">
            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--7">7</span>
</span>


            <a href="/funds/a-asemi" class="btn btn-split" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE40;&#xE2D;&#xE40;&#xE0A;&#xE35;&#xE22; &#xE40;&#xE0B;&#xE21;&#xE34;&#xE04;&#xE2D;&#xE19;&#xE14;&#xE31;&#xE01;&#xE40;&#xE15;&#xE2D;&#xE23;&#xE4C;">
                <span>ดูรายละเอียด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>

    </div>
</article>

                    </div>
                    <div class="swiper-slide">
                        
<article class="card card--fund">
    <a href="/funds/a-jedi" class="card__image" tabindex="-1" aria-hidden="true">
        <img src="/media/images/home/fund/a-jedi.jpg" alt="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2A;&#xE40;&#xE1B;&#xE0B; &#xE2D;&#xE35;&#xE42;&#xE04;&#xE42;&#xE19;&#xE21;&#xE35;" loading="lazy" decoding="async" />
    </a>


    <div class="card__body">
        <a href="/funds/a-jedi" class="card__heading" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2A;&#xE40;&#xE1B;&#xE0B; &#xE2D;&#xE35;&#xE42;&#xE04;&#xE42;&#xE19;&#xE21;&#xE35;">
            <h3 class="card__title card__code">A-JEDI</h3>
            <span class="card__meta card__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;/&#xE2B;&#xE38;&#xE49;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
        </a>

        <p class="card__text card__summary">&#xE42;&#xE2D;&#xE01;&#xE32;&#xE2A;&#xE40;&#xE15;&#xE34;&#xE1A;&#xE42;&#xE15;&#xE08;&#xE32;&#xE01;&#xE2D;&#xE38;&#xE15;&#xE2A;&#xE32;&#xE2B;&#xE01;&#xE23;&#xE23;&#xE21;&#xE2D;&#xE27;&#xE01;&#xE32;&#xE28;&#xE41;&#xE25;&#xE30;&#xE14;&#xE32;&#xE27;&#xE40;&#xE17;&#xE35;&#xE22;&#xE21;&#xE17;&#xE35;&#xE48;&#xE02;&#xE22;&#xE32;&#xE22;&#xE15;&#xE31;&#xE27;&#xE15;&#xE48;&#xE2D;&#xE40;&#xE19;&#xE37;&#xE48;&#xE2D;&#xE07;</p>

            
    <span class="rating">
        <img src="/media/images/morningstar/5-star.png" class="rating__image" alt="Morningstar Rating 5 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

    </div>

    <div class="card__footer">
        <div class="card__actions">
            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--7">7</span>
</span>


            <a href="/funds/a-jedi" class="btn btn-split" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE2A;&#xE40;&#xE1B;&#xE0B; &#xE2D;&#xE35;&#xE42;&#xE04;&#xE42;&#xE19;&#xE21;&#xE35;">
                <span>ดูรายละเอียด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>

    </div>
</article>

                    </div>
                    <div class="swiper-slide">
                        
<article class="card card--fund">
    <a href="/funds/asp-crypto" class="card__image" tabindex="-1" aria-hidden="true">
        <img src="/media/images/home/fund/asp-crypto.jpg" alt="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE14;&#xE34;&#xE08;&#xE34;&#xE17;&#xE31;&#xE25; &#xE1A;&#xE25;&#xE47;&#xE2D;&#xE01;&#xE40;&#xE0A;&#xE19;" loading="lazy" decoding="async" />
    </a>


    <div class="card__body">
        <a href="/funds/asp-crypto" class="card__heading" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE14;&#xE34;&#xE08;&#xE34;&#xE17;&#xE31;&#xE25; &#xE1A;&#xE25;&#xE47;&#xE2D;&#xE01;&#xE40;&#xE0A;&#xE19;">
            <h3 class="card__title card__code">ASP-CRYPTO</h3>
            <span class="card__meta card__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;/&#xE2B;&#xE38;&#xE49;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
        </a>

        <p class="card__text card__summary">&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE19;&#xE18;&#xE38;&#xE23;&#xE01;&#xE34;&#xE08;&#xE41;&#xE1E;&#xE25;&#xE15;&#xE1F;&#xE2D;&#xE23;&#xE4C;&#xE21;&#xE14;&#xE34;&#xE08;&#xE34;&#xE17;&#xE31;&#xE25; &#xE41;&#xE25;&#xE30;&#xE40;&#xE17;&#xE04;&#xE42;&#xE19;&#xE42;&#xE25;&#xE22;&#xE35;&#xE1A;&#xE25;&#xE47;&#xE2D;&#xE01;&#xE40;&#xE0A;&#xE19;&#xE17;&#xE31;&#xE48;&#xE27;&#xE42;&#xE25;&#xE01;</p>

            
    <span class="rating">
        <img src="/media/images/morningstar/5-star.png" class="rating__image" alt="Morningstar Rating 5 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

    </div>

    <div class="card__footer">
        <div class="card__actions">
            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--8">8&#x2B;</span>
</span>


            <a href="/funds/asp-crypto" class="btn btn-split" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE14;&#xE34;&#xE08;&#xE34;&#xE17;&#xE31;&#xE25; &#xE1A;&#xE25;&#xE47;&#xE2D;&#xE01;&#xE40;&#xE0A;&#xE19;">
                <span>ดูรายละเอียด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>

    </div>
</article>

                    </div>
                    <div class="swiper-slide">
                        
<article class="card card--fund">
    <a href="/funds/a-ring" class="card__image" tabindex="-1" aria-hidden="true">
        <img src="/media/images/home/fund/a-ring.jpg" alt="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE42;&#xE01;&#xE25;&#xE14;&#xE4C;" loading="lazy" decoding="async" />
    </a>


    <div class="card__body">
        <a href="/funds/a-ring" class="card__heading" title="&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE42;&#xE01;&#xE25;&#xE14;&#xE4C;">
            <h3 class="card__title card__code">A-RING</h3>
            <span class="card__meta card__category">&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C;&#xE2A;&#xE34;&#xE19;&#xE17;&#xE32;&#xE07;&#xE40;&#xE25;&#xE37;&#xE2D;&#xE01;</span>
        </a>

        <p class="card__text card__summary">&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE19;&#xE17;&#xE2D;&#xE07;&#xE04;&#xE33; &#xE1B;&#xE49;&#xE2D;&#xE07;&#xE01;&#xE31;&#xE19;&#xE04;&#xE27;&#xE32;&#xE21;&#xE40;&#xE2A;&#xE35;&#xE48;&#xE22;&#xE07; &#xE2A;&#xE23;&#xE49;&#xE32;&#xE07;&#xE2A;&#xE21;&#xE14;&#xE38;&#xE25;&#xE1E;&#xE2D;&#xE23;&#xE4C;&#xE15;&#xE01;&#xE32;&#xE23;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;</p>

            
    <span class="rating">
        <img src="/media/images/morningstar/4-star.png" class="rating__image" alt="Morningstar Rating 4 ดาว จาก 5 ดาว" width="834" height="417" loading="lazy" decoding="async" />
    </span>

    </div>

    <div class="card__footer">
        <div class="card__actions">
            
<span class="risk-level">
    <span class="risk-level__label">ระดับความเสี่ยง</span>
    <span class="risk-level__value risk-level__value--8">8</span>
</span>


            <a href="/funds/a-ring" class="btn btn-split" aria-label="ดูรายละเอียด &#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE40;&#xE1B;&#xE34;&#xE14; &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17;&#xE1E;&#xE25;&#xE31;&#xE2A; &#xE42;&#xE01;&#xE25;&#xE14;&#xE4C;">
                <span>ดูรายละเอียด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>

    </div>
</article>

                    </div>
            </div>

            <div class="featured-funds__controls">
                <div class="slider-dots js-featured-pagination"></div>
            </div>
        </div>
    </div>
</section>', N'FeaturedFundsV3', 0);
INSERT INTO [2026_web_widget] (created_at, updated_at, created_by, updated_by, sort, status, pb_status, approve_by, show_front, cat_id, title, img1, mod_name, info, section_key, pb_cat_id, pb_title, pb_img1, pb_mod_name, pb_info, pb_section_key, web_id) VALUES (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'user', N'user', 40, 1, 1, N'user', 1, @gid, N'เปิดมุมมองลงทุนตามเทรนด์', N'Files/Site0/1/widget_icons/assetplus/icon-ExploreThemes.png', N'', N'<section class="section">
    <div class="container-fluid">
        <div class="explore-themes explore-themes--v3">
            <div class="container">
                <div class="explore-themes__heading">
                    <span class="explore-themes__watermark" aria-hidden="true">Explore by Theme</span>
                    <h2 class="explore-themes__title">เปิดมุมมองลงทุนตามเทรนด์</h2>
                </div>

                <ul class="theme-cells">
                        <li class="theme-cells__item">
                            <a href="/funds/themes?theme=ai-robotics" class="theme-cell">
                                <span class="theme-cell__index" aria-hidden="true">01</span>
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-robot"></i>
</span>

                                <span class="theme-cell__name">AI &amp; Robotics</span>
                            </a>
                        </li>
                        <li class="theme-cells__item">
                            <a href="/funds/themes?theme=semiconductor" class="theme-cell">
                                <span class="theme-cell__index" aria-hidden="true">02</span>
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-cpu"></i>
</span>

                                <span class="theme-cell__name">Semiconductor</span>
                            </a>
                        </li>
                        <li class="theme-cells__item">
                            <a href="/funds/themes?theme=digital-assets" class="theme-cell">
                                <span class="theme-cell__index" aria-hidden="true">03</span>
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-currency-bitcoin"></i>
</span>

                                <span class="theme-cell__name">Digital Assets</span>
                            </a>
                        </li>
                        <li class="theme-cells__item">
                            <a href="/funds/themes?theme=clean-energy" class="theme-cell">
                                <span class="theme-cell__index" aria-hidden="true">04</span>
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-lightning-charge"></i>
</span>

                                <span class="theme-cell__name">Clean Energy</span>
                            </a>
                        </li>
                        <li class="theme-cells__item">
                            <a href="/funds/themes?theme=aerospace-defense" class="theme-cell">
                                <span class="theme-cell__index" aria-hidden="true">05</span>
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-rocket-takeoff"></i>
</span>

                                <span class="theme-cell__name">Aerospace &amp; Defense</span>
                            </a>
                        </li>
                        <li class="theme-cells__item">
                            <a href="/funds/themes?theme=esg" class="theme-cell">
                                <span class="theme-cell__index" aria-hidden="true">06</span>
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-tree"></i>
</span>

                                <span class="theme-cell__name">ESG &amp; Sustainability</span>
                            </a>
                        </li>
                </ul>

                <div class="explore-themes__footer">
                    <a href="/funds/themes" class="btn btn-split btn-light" aria-label="ดูธีมการลงทุนทั้งหมด">
                        <span>ดูทั้งหมด</span>
                        <span class="btn-split__icon">
                            <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                        </span>
                    </a>
                </div>
            </div>
        </div>
    </div>
</section>', N'ExploreThemesV3', CAST(@gid AS nvarchar(20)), N'เปิดมุมมองลงทุนตามเทรนด์', N'Files/Site0/1/widget_icons/assetplus/icon-ExploreThemes.png', N'', N'<section class="section">
    <div class="container-fluid">
        <div class="explore-themes explore-themes--v3">
            <div class="container">
                <div class="explore-themes__heading">
                    <span class="explore-themes__watermark" aria-hidden="true">Explore by Theme</span>
                    <h2 class="explore-themes__title">เปิดมุมมองลงทุนตามเทรนด์</h2>
                </div>

                <ul class="theme-cells">
                        <li class="theme-cells__item">
                            <a href="/funds/themes?theme=ai-robotics" class="theme-cell">
                                <span class="theme-cell__index" aria-hidden="true">01</span>
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-robot"></i>
</span>

                                <span class="theme-cell__name">AI &amp; Robotics</span>
                            </a>
                        </li>
                        <li class="theme-cells__item">
                            <a href="/funds/themes?theme=semiconductor" class="theme-cell">
                                <span class="theme-cell__index" aria-hidden="true">02</span>
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-cpu"></i>
</span>

                                <span class="theme-cell__name">Semiconductor</span>
                            </a>
                        </li>
                        <li class="theme-cells__item">
                            <a href="/funds/themes?theme=digital-assets" class="theme-cell">
                                <span class="theme-cell__index" aria-hidden="true">03</span>
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-currency-bitcoin"></i>
</span>

                                <span class="theme-cell__name">Digital Assets</span>
                            </a>
                        </li>
                        <li class="theme-cells__item">
                            <a href="/funds/themes?theme=clean-energy" class="theme-cell">
                                <span class="theme-cell__index" aria-hidden="true">04</span>
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-lightning-charge"></i>
</span>

                                <span class="theme-cell__name">Clean Energy</span>
                            </a>
                        </li>
                        <li class="theme-cells__item">
                            <a href="/funds/themes?theme=aerospace-defense" class="theme-cell">
                                <span class="theme-cell__index" aria-hidden="true">05</span>
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-rocket-takeoff"></i>
</span>

                                <span class="theme-cell__name">Aerospace &amp; Defense</span>
                            </a>
                        </li>
                        <li class="theme-cells__item">
                            <a href="/funds/themes?theme=esg" class="theme-cell">
                                <span class="theme-cell__index" aria-hidden="true">06</span>
                                <span class="theme-icon" aria-hidden="true">
        <i class="bi bi-tree"></i>
</span>

                                <span class="theme-cell__name">ESG &amp; Sustainability</span>
                            </a>
                        </li>
                </ul>

                <div class="explore-themes__footer">
                    <a href="/funds/themes" class="btn btn-split btn-light" aria-label="ดูธีมการลงทุนทั้งหมด">
                        <span>ดูทั้งหมด</span>
                        <span class="btn-split__icon">
                            <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                        </span>
                    </a>
                </div>
            </div>
        </div>
    </div>
</section>', N'ExploreThemesV3', 0);
INSERT INTO [2026_web_widget] (created_at, updated_at, created_by, updated_by, sort, status, pb_status, approve_by, show_front, cat_id, title, img1, mod_name, info, section_key, pb_cat_id, pb_title, pb_img1, pb_mod_name, pb_info, pb_section_key, web_id) VALUES (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'user', N'user', 50, 1, 1, N'user', 1, @gid, N'บทความ / กิจกรรม / ข่าวประกาศ', N'Files/Site0/1/widget_icons/assetplus/icon-Insights.png', N'', N'<section class="section insights insights--v3" aria-label="บทความ ข่าวสาร และประกาศ">
    <div class="container">
        <div class="card-deck card-deck--cards-1 card-deck--cards-md-2">
                <section class="insight-col">
                    <div class="section-head">
                        <h2 class="section-head__title">&#xE1A;&#xE17;&#xE04;&#xE27;&#xE32;&#xE21;</h2>
                    </div>

                    <div class="insight-col__body">
                            <a href="/articles/ai-revolution-2026" class="insight-date-row">
                                <span class="insight-date-row__date" aria-hidden="true">
                                    <span class="insight-date-row__day">09</span>
                                    <span class="insight-date-row__month">
                                        &#xE01;.&#xE04;.
                                    </span>
                                </span>
                                <span class="insight-date-row__body">
                                    <span class="insight-date-row__title">AI Revolution: &#xE42;&#xE2D;&#xE01;&#xE32;&#xE2A;&#xE01;&#xE32;&#xE23;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE17;&#xE35;&#xE48;&#xE2D;&#xE22;&#xE39;&#xE48;&#xE43;&#xE01;&#xE25;&#xE49;&#xE01;&#xE27;&#xE48;&#xE32;&#xE17;&#xE35;&#xE48;&#xE04;&#xE34;&#xE14;</span>
                                    <time class="insight-date-row__meta"
                                          datetime="2569-07-09">
                                        9 &#xE01;&#xE23;&#xE01;&#xE0E;&#xE32;&#xE04;&#xE21; 2569
                                    </time>
                                </span>
                            </a>
                            <a href="/articles/semiconductor-humanoid-h2-2026" class="insight-date-row">
                                <span class="insight-date-row__date" aria-hidden="true">
                                    <span class="insight-date-row__day">03</span>
                                    <span class="insight-date-row__month">
                                        &#xE01;.&#xE04;.
                                    </span>
                                </span>
                                <span class="insight-date-row__body">
                                    <span class="insight-date-row__title">&#xE2A;&#xE48;&#xE2D;&#xE07;&#xE42;&#xE2D;&#xE01;&#xE32;&#xE2A; Semiconductor &amp; Humanoid &#xE04;&#xE23;&#xE36;&#xE48;&#xE07;&#xE1B;&#xE35;&#xE2B;&#xE25;&#xE31;&#xE07; 2026</span>
                                    <time class="insight-date-row__meta"
                                          datetime="2569-07-03">
                                        3 &#xE01;&#xE23;&#xE01;&#xE0E;&#xE32;&#xE04;&#xE21; 2569
                                    </time>
                                </span>
                            </a>
                            <a href="/articles/semiconductor-cycle-restart" class="insight-date-row">
                                <span class="insight-date-row__date" aria-hidden="true">
                                    <span class="insight-date-row__day">27</span>
                                    <span class="insight-date-row__month">
                                        &#xE21;&#xE34;.&#xE22;.
                                    </span>
                                </span>
                                <span class="insight-date-row__body">
                                    <span class="insight-date-row__title">Semiconductor Cycle &#xE23;&#xE2D;&#xE1A;&#xE43;&#xE2B;&#xE21;&#xE48;&#xE40;&#xE23;&#xE34;&#xE48;&#xE21;&#xE41;&#xE25;&#xE49;&#xE27;&#xE2B;&#xE23;&#xE37;&#xE2D;&#xE22;&#xE31;&#xE07;</span>
                                    <time class="insight-date-row__meta"
                                          datetime="2569-06-27">
                                        27 &#xE21;&#xE34;&#xE16;&#xE38;&#xE19;&#xE32;&#xE22;&#xE19; 2569
                                    </time>
                                </span>
                            </a>
                    </div>

                    <div class="insight-col__footer">
                        <a href="/articles" class="btn btn-split btn-light" aria-label="ดู&#xE1A;&#xE17;&#xE04;&#xE27;&#xE32;&#xE21;ทั้งหมด">
                            <span>ดูทั้งหมด</span>
                            <span class="btn-split__icon">
                                <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                            </span>
                        </a>
                    </div>
                </section>
                <section class="insight-col">
                    <div class="section-head">
                        <h2 class="section-head__title">&#xE01;&#xE34;&#xE08;&#xE01;&#xE23;&#xE23;&#xE21;</h2>
                    </div>

                    <div class="insight-col__body">
                            <a href="/events/money-banking-awards-2026" class="insight-date-row">
                                <span class="insight-date-row__date" aria-hidden="true">
                                    <span class="insight-date-row__day">09</span>
                                    <span class="insight-date-row__month">
                                        &#xE01;.&#xE04;.
                                    </span>
                                </span>
                                <span class="insight-date-row__body">
                                    <span class="insight-date-row__title">&#xE1A;&#xE25;&#xE08;. &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE04;&#xE27;&#xE49;&#xE32;&#xE23;&#xE32;&#xE07;&#xE27;&#xE31;&#xE25; Money &amp; Banking Awards 2026 &#xE08;&#xE32;&#xE01;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19; ASP-NGF</span>
                                    <time class="insight-date-row__meta"
                                          datetime="2569-07-09">
                                        9 &#xE01;&#xE23;&#xE01;&#xE0E;&#xE32;&#xE04;&#xE21; 2569
                                    </time>
                                </span>
                            </a>
                            <a href="/events/thailand-gold-summit-2026" class="insight-date-row">
                                <span class="insight-date-row__date" aria-hidden="true">
                                    <span class="insight-date-row__day">30</span>
                                    <span class="insight-date-row__month">
                                        &#xE21;&#xE34;.&#xE22;.
                                    </span>
                                </span>
                                <span class="insight-date-row__body">
                                    <span class="insight-date-row__title">&#xE1A;&#xE25;&#xE08;. &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE0A;&#xE35;&#xE49;&#xE42;&#xE25;&#xE01;&#xE01;&#xE32;&#xE23;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE22;&#xE38;&#xE04; Uncertainty &#xE15;&#xE49;&#xE2D;&#xE07;&#xE01;&#xE23;&#xE30;&#xE08;&#xE32;&#xE22;&#xE1E;&#xE2D;&#xE23;&#xE4C;&#xE15;&#xE14;&#xE49;&#xE27;&#xE22;&#xE2A;&#xE34;&#xE19;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C;&#xE40;&#xE0A;&#xE34;&#xE07;&#xE01;&#xE25;&#xE22;&#xE38;&#xE17;&#xE18;&#xE4C;</span>
                                    <time class="insight-date-row__meta"
                                          datetime="2569-06-30">
                                        30 &#xE21;&#xE34;&#xE16;&#xE38;&#xE19;&#xE32;&#xE22;&#xE19; 2569
                                    </time>
                                </span>
                            </a>
                            <a href="/events/gsb-the-selected" class="insight-date-row">
                                <span class="insight-date-row__date" aria-hidden="true">
                                    <span class="insight-date-row__day">21</span>
                                    <span class="insight-date-row__month">
                                        &#xE21;&#xE34;.&#xE22;.
                                    </span>
                                </span>
                                <span class="insight-date-row__body">
                                    <span class="insight-date-row__title">&#xE1A;&#xE25;&#xE08;. &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE23;&#xE48;&#xE27;&#xE21;&#xE40;&#xE1B;&#xE47;&#xE19;&#xE1E;&#xE31;&#xE19;&#xE18;&#xE21;&#xE34;&#xE15;&#xE23;&#xE01;&#xE31;&#xE1A;&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE2D;&#xE2D;&#xE21;&#xE2A;&#xE34;&#xE19; &#xE43;&#xE19;&#xE42;&#xE04;&#xE23;&#xE07;&#xE01;&#xE32;&#xE23; &#x201C;&#xE2D;&#xE2D;&#xE21;&#xE2A;&#xE34;&#xE19; The Selected&#x201D;</span>
                                    <time class="insight-date-row__meta"
                                          datetime="2569-06-21">
                                        21 &#xE21;&#xE34;&#xE16;&#xE38;&#xE19;&#xE32;&#xE22;&#xE19; 2569
                                    </time>
                                </span>
                            </a>
                    </div>

                    <div class="insight-col__footer">
                        <a href="/events" class="btn btn-split btn-light" aria-label="ดู&#xE01;&#xE34;&#xE08;&#xE01;&#xE23;&#xE23;&#xE21;ทั้งหมด">
                            <span>ดูทั้งหมด</span>
                            <span class="btn-split__icon">
                                <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                            </span>
                        </a>
                    </div>
                </section>
        </div>

        <!-- ข่าวสารและประกาศ — เต็มความกว้างแถวล่าง -->
        <section class="insight-col insight-col--wide">
            <div class="section-head">
                <h2 class="section-head__title">ข่าวสารและประกาศ บลจ.</h2>
            </div>

            <ul class="announcement-list insight-col__body">
                    <li>
                        <a href="/media/documents/sample.pdf" class="insight-date-row" target="_blank" rel="noopener"
                           aria-label="&#xE1C;&#xE25;&#xE01;&#xE32;&#xE23;&#xE14;&#xE33;&#xE40;&#xE19;&#xE34;&#xE19;&#xE07;&#xE32;&#xE19;: &#xE23;&#xE32;&#xE22;&#xE07;&#xE32;&#xE19;&#xE1C;&#xE25;&#xE01;&#xE32;&#xE23;&#xE14;&#xE33;&#xE40;&#xE19;&#xE34;&#xE19;&#xE07;&#xE32;&#xE19;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE20;&#xE32;&#xE22;&#xE43;&#xE15;&#xE49;&#xE01;&#xE32;&#xE23;&#xE08;&#xE31;&#xE14;&#xE01;&#xE32;&#xE23; &#xE1B;&#xE23;&#xE30;&#xE08;&#xE33;&#xE44;&#xE15;&#xE23;&#xE21;&#xE32;&#xE2A; 2/2569 (ไฟล์ PDF, เปิดในแท็บใหม่)">
                            <span class="insight-date-row__date" aria-hidden="true">
                                <span class="insight-date-row__day">28</span>
                                <span class="insight-date-row__month">
                                    &#xE1E;.&#xE04;.
                                </span>
                            </span>
                            <span class="insight-date-row__body">
                                <span class="insight-date-row__title">&#xE23;&#xE32;&#xE22;&#xE07;&#xE32;&#xE19;&#xE1C;&#xE25;&#xE01;&#xE32;&#xE23;&#xE14;&#xE33;&#xE40;&#xE19;&#xE34;&#xE19;&#xE07;&#xE32;&#xE19;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE20;&#xE32;&#xE22;&#xE43;&#xE15;&#xE49;&#xE01;&#xE32;&#xE23;&#xE08;&#xE31;&#xE14;&#xE01;&#xE32;&#xE23; &#xE1B;&#xE23;&#xE30;&#xE08;&#xE33;&#xE44;&#xE15;&#xE23;&#xE21;&#xE32;&#xE2A; 2/2569</span>
                                <span class="insight-date-row__meta">&#xE1C;&#xE25;&#xE01;&#xE32;&#xE23;&#xE14;&#xE33;&#xE40;&#xE19;&#xE34;&#xE19;&#xE07;&#xE32;&#xE19;</span>
                            </span>
                        </a>
                    </li>
                    <li>
                        <a href="/media/documents/sample.pdf" class="insight-date-row" target="_blank" rel="noopener"
                           aria-label="&#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE27;&#xE31;&#xE19;&#xE2B;&#xE22;&#xE38;&#xE14;&#xE0B;&#xE37;&#xE49;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;: &#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE27;&#xE31;&#xE19;&#xE2B;&#xE22;&#xE38;&#xE14;&#xE17;&#xE33;&#xE01;&#xE32;&#xE23;&#xE0B;&#xE37;&#xE49;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19; &#xE1B;&#xE23;&#xE30;&#xE08;&#xE33;&#xE40;&#xE14;&#xE37;&#xE2D;&#xE19;&#xE01;&#xE23;&#xE01;&#xE0E;&#xE32;&#xE04;&#xE21; 2569 (ไฟล์ PDF, เปิดในแท็บใหม่)">
                            <span class="insight-date-row__date" aria-hidden="true">
                                <span class="insight-date-row__day">28</span>
                                <span class="insight-date-row__month">
                                    &#xE1E;.&#xE04;.
                                </span>
                            </span>
                            <span class="insight-date-row__body">
                                <span class="insight-date-row__title">&#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE27;&#xE31;&#xE19;&#xE2B;&#xE22;&#xE38;&#xE14;&#xE17;&#xE33;&#xE01;&#xE32;&#xE23;&#xE0B;&#xE37;&#xE49;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19; &#xE1B;&#xE23;&#xE30;&#xE08;&#xE33;&#xE40;&#xE14;&#xE37;&#xE2D;&#xE19;&#xE01;&#xE23;&#xE01;&#xE0E;&#xE32;&#xE04;&#xE21; 2569</span>
                                <span class="insight-date-row__meta">&#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE27;&#xE31;&#xE19;&#xE2B;&#xE22;&#xE38;&#xE14;&#xE0B;&#xE37;&#xE49;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;</span>
                            </span>
                        </a>
                    </li>
                    <li>
                        <a href="/media/documents/sample.pdf" class="insight-date-row" target="_blank" rel="noopener"
                           aria-label="&#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE41;&#xE08;&#xE49;&#xE07;&#xE1B;&#xE31;&#xE19;&#xE1C;&#xE25;: &#xE41;&#xE08;&#xE49;&#xE07;&#xE01;&#xE32;&#xE23;&#xE08;&#xE48;&#xE32;&#xE22;&#xE40;&#xE07;&#xE34;&#xE19;&#xE1B;&#xE31;&#xE19;&#xE1C;&#xE25;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19; ASP-DIGITAL (ไฟล์ PDF, เปิดในแท็บใหม่)">
                            <span class="insight-date-row__date" aria-hidden="true">
                                <span class="insight-date-row__day">23</span>
                                <span class="insight-date-row__month">
                                    &#xE1E;.&#xE04;.
                                </span>
                            </span>
                            <span class="insight-date-row__body">
                                <span class="insight-date-row__title">&#xE41;&#xE08;&#xE49;&#xE07;&#xE01;&#xE32;&#xE23;&#xE08;&#xE48;&#xE32;&#xE22;&#xE40;&#xE07;&#xE34;&#xE19;&#xE1B;&#xE31;&#xE19;&#xE1C;&#xE25;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19; ASP-DIGITAL</span>
                                <span class="insight-date-row__meta">&#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE41;&#xE08;&#xE49;&#xE07;&#xE1B;&#xE31;&#xE19;&#xE1C;&#xE25;</span>
                            </span>
                        </a>
                    </li>
                    <li>
                        <a href="/media/documents/sample.pdf" class="insight-date-row" target="_blank" rel="noopener"
                           aria-label="&#xE23;&#xE32;&#xE07;&#xE27;&#xE31;&#xE25;&#xE41;&#xE25;&#xE30;&#xE04;&#xE27;&#xE32;&#xE21;&#xE2A;&#xE33;&#xE40;&#xE23;&#xE47;&#xE08;: &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE04;&#xE27;&#xE49;&#xE32;&#xE23;&#xE32;&#xE07;&#xE27;&#xE31;&#xE25;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE22;&#xE2D;&#xE14;&#xE40;&#xE22;&#xE35;&#xE48;&#xE22;&#xE21;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE20;&#xE17;&#xE15;&#xE23;&#xE32;&#xE2A;&#xE32;&#xE23;&#xE17;&#xE38;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28; (ไฟล์ PDF, เปิดในแท็บใหม่)">
                            <span class="insight-date-row__date" aria-hidden="true">
                                <span class="insight-date-row__day">20</span>
                                <span class="insight-date-row__month">
                                    &#xE1E;.&#xE04;.
                                </span>
                            </span>
                            <span class="insight-date-row__body">
                                <span class="insight-date-row__title">&#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE04;&#xE27;&#xE49;&#xE32;&#xE23;&#xE32;&#xE07;&#xE27;&#xE31;&#xE25;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE22;&#xE2D;&#xE14;&#xE40;&#xE22;&#xE35;&#xE48;&#xE22;&#xE21;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE20;&#xE17;&#xE15;&#xE23;&#xE32;&#xE2A;&#xE32;&#xE23;&#xE17;&#xE38;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
                                <span class="insight-date-row__meta">&#xE23;&#xE32;&#xE07;&#xE27;&#xE31;&#xE25;&#xE41;&#xE25;&#xE30;&#xE04;&#xE27;&#xE32;&#xE21;&#xE2A;&#xE33;&#xE40;&#xE23;&#xE47;&#xE08;</span>
                            </span>
                        </a>
                    </li>
                    <li>
                        <a href="/media/documents/sample.pdf" class="insight-date-row" target="_blank" rel="noopener"
                           aria-label="&#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE40;&#xE1B;&#xE34;&#xE14;&#xE40;&#xE2A;&#xE19;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;: &#xE40;&#xE1B;&#xE34;&#xE14;&#xE40;&#xE2A;&#xE19;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE2B;&#xE21;&#xE48; A-HUMANOID (ไฟล์ PDF, เปิดในแท็บใหม่)">
                            <span class="insight-date-row__date" aria-hidden="true">
                                <span class="insight-date-row__day">18</span>
                                <span class="insight-date-row__month">
                                    &#xE1E;.&#xE04;.
                                </span>
                            </span>
                            <span class="insight-date-row__body">
                                <span class="insight-date-row__title">&#xE40;&#xE1B;&#xE34;&#xE14;&#xE40;&#xE2A;&#xE19;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE2B;&#xE21;&#xE48; A-HUMANOID</span>
                                <span class="insight-date-row__meta">&#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE40;&#xE1B;&#xE34;&#xE14;&#xE40;&#xE2A;&#xE19;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;</span>
                            </span>
                        </a>
                    </li>
            </ul>

            <div class="insight-col__footer">
                <a href="/news-announcements" class="btn btn-split btn-light" aria-label="ดูข่าวสารและประกาศทั้งหมด">
                    <span>ดูทั้งหมด</span>
                    <span class="btn-split__icon">
                        <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                    </span>
                </a>
            </div>
        </section>
    </div>
</section>', N'InsightsV3', CAST(@gid AS nvarchar(20)), N'บทความ / กิจกรรม / ข่าวประกาศ', N'Files/Site0/1/widget_icons/assetplus/icon-Insights.png', N'', N'<section class="section insights insights--v3" aria-label="บทความ ข่าวสาร และประกาศ">
    <div class="container">
        <div class="card-deck card-deck--cards-1 card-deck--cards-md-2">
                <section class="insight-col">
                    <div class="section-head">
                        <h2 class="section-head__title">&#xE1A;&#xE17;&#xE04;&#xE27;&#xE32;&#xE21;</h2>
                    </div>

                    <div class="insight-col__body">
                            <a href="/articles/ai-revolution-2026" class="insight-date-row">
                                <span class="insight-date-row__date" aria-hidden="true">
                                    <span class="insight-date-row__day">09</span>
                                    <span class="insight-date-row__month">
                                        &#xE01;.&#xE04;.
                                    </span>
                                </span>
                                <span class="insight-date-row__body">
                                    <span class="insight-date-row__title">AI Revolution: &#xE42;&#xE2D;&#xE01;&#xE32;&#xE2A;&#xE01;&#xE32;&#xE23;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE17;&#xE35;&#xE48;&#xE2D;&#xE22;&#xE39;&#xE48;&#xE43;&#xE01;&#xE25;&#xE49;&#xE01;&#xE27;&#xE48;&#xE32;&#xE17;&#xE35;&#xE48;&#xE04;&#xE34;&#xE14;</span>
                                    <time class="insight-date-row__meta"
                                          datetime="2569-07-09">
                                        9 &#xE01;&#xE23;&#xE01;&#xE0E;&#xE32;&#xE04;&#xE21; 2569
                                    </time>
                                </span>
                            </a>
                            <a href="/articles/semiconductor-humanoid-h2-2026" class="insight-date-row">
                                <span class="insight-date-row__date" aria-hidden="true">
                                    <span class="insight-date-row__day">03</span>
                                    <span class="insight-date-row__month">
                                        &#xE01;.&#xE04;.
                                    </span>
                                </span>
                                <span class="insight-date-row__body">
                                    <span class="insight-date-row__title">&#xE2A;&#xE48;&#xE2D;&#xE07;&#xE42;&#xE2D;&#xE01;&#xE32;&#xE2A; Semiconductor &amp; Humanoid &#xE04;&#xE23;&#xE36;&#xE48;&#xE07;&#xE1B;&#xE35;&#xE2B;&#xE25;&#xE31;&#xE07; 2026</span>
                                    <time class="insight-date-row__meta"
                                          datetime="2569-07-03">
                                        3 &#xE01;&#xE23;&#xE01;&#xE0E;&#xE32;&#xE04;&#xE21; 2569
                                    </time>
                                </span>
                            </a>
                            <a href="/articles/semiconductor-cycle-restart" class="insight-date-row">
                                <span class="insight-date-row__date" aria-hidden="true">
                                    <span class="insight-date-row__day">27</span>
                                    <span class="insight-date-row__month">
                                        &#xE21;&#xE34;.&#xE22;.
                                    </span>
                                </span>
                                <span class="insight-date-row__body">
                                    <span class="insight-date-row__title">Semiconductor Cycle &#xE23;&#xE2D;&#xE1A;&#xE43;&#xE2B;&#xE21;&#xE48;&#xE40;&#xE23;&#xE34;&#xE48;&#xE21;&#xE41;&#xE25;&#xE49;&#xE27;&#xE2B;&#xE23;&#xE37;&#xE2D;&#xE22;&#xE31;&#xE07;</span>
                                    <time class="insight-date-row__meta"
                                          datetime="2569-06-27">
                                        27 &#xE21;&#xE34;&#xE16;&#xE38;&#xE19;&#xE32;&#xE22;&#xE19; 2569
                                    </time>
                                </span>
                            </a>
                    </div>

                    <div class="insight-col__footer">
                        <a href="/articles" class="btn btn-split btn-light" aria-label="ดู&#xE1A;&#xE17;&#xE04;&#xE27;&#xE32;&#xE21;ทั้งหมด">
                            <span>ดูทั้งหมด</span>
                            <span class="btn-split__icon">
                                <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                            </span>
                        </a>
                    </div>
                </section>
                <section class="insight-col">
                    <div class="section-head">
                        <h2 class="section-head__title">&#xE01;&#xE34;&#xE08;&#xE01;&#xE23;&#xE23;&#xE21;</h2>
                    </div>

                    <div class="insight-col__body">
                            <a href="/events/money-banking-awards-2026" class="insight-date-row">
                                <span class="insight-date-row__date" aria-hidden="true">
                                    <span class="insight-date-row__day">09</span>
                                    <span class="insight-date-row__month">
                                        &#xE01;.&#xE04;.
                                    </span>
                                </span>
                                <span class="insight-date-row__body">
                                    <span class="insight-date-row__title">&#xE1A;&#xE25;&#xE08;. &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE04;&#xE27;&#xE49;&#xE32;&#xE23;&#xE32;&#xE07;&#xE27;&#xE31;&#xE25; Money &amp; Banking Awards 2026 &#xE08;&#xE32;&#xE01;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19; ASP-NGF</span>
                                    <time class="insight-date-row__meta"
                                          datetime="2569-07-09">
                                        9 &#xE01;&#xE23;&#xE01;&#xE0E;&#xE32;&#xE04;&#xE21; 2569
                                    </time>
                                </span>
                            </a>
                            <a href="/events/thailand-gold-summit-2026" class="insight-date-row">
                                <span class="insight-date-row__date" aria-hidden="true">
                                    <span class="insight-date-row__day">30</span>
                                    <span class="insight-date-row__month">
                                        &#xE21;&#xE34;.&#xE22;.
                                    </span>
                                </span>
                                <span class="insight-date-row__body">
                                    <span class="insight-date-row__title">&#xE1A;&#xE25;&#xE08;. &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE0A;&#xE35;&#xE49;&#xE42;&#xE25;&#xE01;&#xE01;&#xE32;&#xE23;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19;&#xE22;&#xE38;&#xE04; Uncertainty &#xE15;&#xE49;&#xE2D;&#xE07;&#xE01;&#xE23;&#xE30;&#xE08;&#xE32;&#xE22;&#xE1E;&#xE2D;&#xE23;&#xE4C;&#xE15;&#xE14;&#xE49;&#xE27;&#xE22;&#xE2A;&#xE34;&#xE19;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C;&#xE40;&#xE0A;&#xE34;&#xE07;&#xE01;&#xE25;&#xE22;&#xE38;&#xE17;&#xE18;&#xE4C;</span>
                                    <time class="insight-date-row__meta"
                                          datetime="2569-06-30">
                                        30 &#xE21;&#xE34;&#xE16;&#xE38;&#xE19;&#xE32;&#xE22;&#xE19; 2569
                                    </time>
                                </span>
                            </a>
                            <a href="/events/gsb-the-selected" class="insight-date-row">
                                <span class="insight-date-row__date" aria-hidden="true">
                                    <span class="insight-date-row__day">21</span>
                                    <span class="insight-date-row__month">
                                        &#xE21;&#xE34;.&#xE22;.
                                    </span>
                                </span>
                                <span class="insight-date-row__body">
                                    <span class="insight-date-row__title">&#xE1A;&#xE25;&#xE08;. &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE23;&#xE48;&#xE27;&#xE21;&#xE40;&#xE1B;&#xE47;&#xE19;&#xE1E;&#xE31;&#xE19;&#xE18;&#xE21;&#xE34;&#xE15;&#xE23;&#xE01;&#xE31;&#xE1A;&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE2D;&#xE2D;&#xE21;&#xE2A;&#xE34;&#xE19; &#xE43;&#xE19;&#xE42;&#xE04;&#xE23;&#xE07;&#xE01;&#xE32;&#xE23; &#x201C;&#xE2D;&#xE2D;&#xE21;&#xE2A;&#xE34;&#xE19; The Selected&#x201D;</span>
                                    <time class="insight-date-row__meta"
                                          datetime="2569-06-21">
                                        21 &#xE21;&#xE34;&#xE16;&#xE38;&#xE19;&#xE32;&#xE22;&#xE19; 2569
                                    </time>
                                </span>
                            </a>
                    </div>

                    <div class="insight-col__footer">
                        <a href="/events" class="btn btn-split btn-light" aria-label="ดู&#xE01;&#xE34;&#xE08;&#xE01;&#xE23;&#xE23;&#xE21;ทั้งหมด">
                            <span>ดูทั้งหมด</span>
                            <span class="btn-split__icon">
                                <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                            </span>
                        </a>
                    </div>
                </section>
        </div>

        <!-- ข่าวสารและประกาศ — เต็มความกว้างแถวล่าง -->
        <section class="insight-col insight-col--wide">
            <div class="section-head">
                <h2 class="section-head__title">ข่าวสารและประกาศ บลจ.</h2>
            </div>

            <ul class="announcement-list insight-col__body">
                    <li>
                        <a href="/media/documents/sample.pdf" class="insight-date-row" target="_blank" rel="noopener"
                           aria-label="&#xE1C;&#xE25;&#xE01;&#xE32;&#xE23;&#xE14;&#xE33;&#xE40;&#xE19;&#xE34;&#xE19;&#xE07;&#xE32;&#xE19;: &#xE23;&#xE32;&#xE22;&#xE07;&#xE32;&#xE19;&#xE1C;&#xE25;&#xE01;&#xE32;&#xE23;&#xE14;&#xE33;&#xE40;&#xE19;&#xE34;&#xE19;&#xE07;&#xE32;&#xE19;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE20;&#xE32;&#xE22;&#xE43;&#xE15;&#xE49;&#xE01;&#xE32;&#xE23;&#xE08;&#xE31;&#xE14;&#xE01;&#xE32;&#xE23; &#xE1B;&#xE23;&#xE30;&#xE08;&#xE33;&#xE44;&#xE15;&#xE23;&#xE21;&#xE32;&#xE2A; 2/2569 (ไฟล์ PDF, เปิดในแท็บใหม่)">
                            <span class="insight-date-row__date" aria-hidden="true">
                                <span class="insight-date-row__day">28</span>
                                <span class="insight-date-row__month">
                                    &#xE1E;.&#xE04;.
                                </span>
                            </span>
                            <span class="insight-date-row__body">
                                <span class="insight-date-row__title">&#xE23;&#xE32;&#xE22;&#xE07;&#xE32;&#xE19;&#xE1C;&#xE25;&#xE01;&#xE32;&#xE23;&#xE14;&#xE33;&#xE40;&#xE19;&#xE34;&#xE19;&#xE07;&#xE32;&#xE19;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE20;&#xE32;&#xE22;&#xE43;&#xE15;&#xE49;&#xE01;&#xE32;&#xE23;&#xE08;&#xE31;&#xE14;&#xE01;&#xE32;&#xE23; &#xE1B;&#xE23;&#xE30;&#xE08;&#xE33;&#xE44;&#xE15;&#xE23;&#xE21;&#xE32;&#xE2A; 2/2569</span>
                                <span class="insight-date-row__meta">&#xE1C;&#xE25;&#xE01;&#xE32;&#xE23;&#xE14;&#xE33;&#xE40;&#xE19;&#xE34;&#xE19;&#xE07;&#xE32;&#xE19;</span>
                            </span>
                        </a>
                    </li>
                    <li>
                        <a href="/media/documents/sample.pdf" class="insight-date-row" target="_blank" rel="noopener"
                           aria-label="&#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE27;&#xE31;&#xE19;&#xE2B;&#xE22;&#xE38;&#xE14;&#xE0B;&#xE37;&#xE49;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;: &#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE27;&#xE31;&#xE19;&#xE2B;&#xE22;&#xE38;&#xE14;&#xE17;&#xE33;&#xE01;&#xE32;&#xE23;&#xE0B;&#xE37;&#xE49;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19; &#xE1B;&#xE23;&#xE30;&#xE08;&#xE33;&#xE40;&#xE14;&#xE37;&#xE2D;&#xE19;&#xE01;&#xE23;&#xE01;&#xE0E;&#xE32;&#xE04;&#xE21; 2569 (ไฟล์ PDF, เปิดในแท็บใหม่)">
                            <span class="insight-date-row__date" aria-hidden="true">
                                <span class="insight-date-row__day">28</span>
                                <span class="insight-date-row__month">
                                    &#xE1E;.&#xE04;.
                                </span>
                            </span>
                            <span class="insight-date-row__body">
                                <span class="insight-date-row__title">&#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE27;&#xE31;&#xE19;&#xE2B;&#xE22;&#xE38;&#xE14;&#xE17;&#xE33;&#xE01;&#xE32;&#xE23;&#xE0B;&#xE37;&#xE49;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE2B;&#xE19;&#xE48;&#xE27;&#xE22;&#xE25;&#xE07;&#xE17;&#xE38;&#xE19; &#xE1B;&#xE23;&#xE30;&#xE08;&#xE33;&#xE40;&#xE14;&#xE37;&#xE2D;&#xE19;&#xE01;&#xE23;&#xE01;&#xE0E;&#xE32;&#xE04;&#xE21; 2569</span>
                                <span class="insight-date-row__meta">&#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE27;&#xE31;&#xE19;&#xE2B;&#xE22;&#xE38;&#xE14;&#xE0B;&#xE37;&#xE49;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;</span>
                            </span>
                        </a>
                    </li>
                    <li>
                        <a href="/media/documents/sample.pdf" class="insight-date-row" target="_blank" rel="noopener"
                           aria-label="&#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE41;&#xE08;&#xE49;&#xE07;&#xE1B;&#xE31;&#xE19;&#xE1C;&#xE25;: &#xE41;&#xE08;&#xE49;&#xE07;&#xE01;&#xE32;&#xE23;&#xE08;&#xE48;&#xE32;&#xE22;&#xE40;&#xE07;&#xE34;&#xE19;&#xE1B;&#xE31;&#xE19;&#xE1C;&#xE25;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19; ASP-DIGITAL (ไฟล์ PDF, เปิดในแท็บใหม่)">
                            <span class="insight-date-row__date" aria-hidden="true">
                                <span class="insight-date-row__day">23</span>
                                <span class="insight-date-row__month">
                                    &#xE1E;.&#xE04;.
                                </span>
                            </span>
                            <span class="insight-date-row__body">
                                <span class="insight-date-row__title">&#xE41;&#xE08;&#xE49;&#xE07;&#xE01;&#xE32;&#xE23;&#xE08;&#xE48;&#xE32;&#xE22;&#xE40;&#xE07;&#xE34;&#xE19;&#xE1B;&#xE31;&#xE19;&#xE1C;&#xE25;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19; ASP-DIGITAL</span>
                                <span class="insight-date-row__meta">&#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE41;&#xE08;&#xE49;&#xE07;&#xE1B;&#xE31;&#xE19;&#xE1C;&#xE25;</span>
                            </span>
                        </a>
                    </li>
                    <li>
                        <a href="/media/documents/sample.pdf" class="insight-date-row" target="_blank" rel="noopener"
                           aria-label="&#xE23;&#xE32;&#xE07;&#xE27;&#xE31;&#xE25;&#xE41;&#xE25;&#xE30;&#xE04;&#xE27;&#xE32;&#xE21;&#xE2A;&#xE33;&#xE40;&#xE23;&#xE47;&#xE08;: &#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE04;&#xE27;&#xE49;&#xE32;&#xE23;&#xE32;&#xE07;&#xE27;&#xE31;&#xE25;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE22;&#xE2D;&#xE14;&#xE40;&#xE22;&#xE35;&#xE48;&#xE22;&#xE21;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE20;&#xE17;&#xE15;&#xE23;&#xE32;&#xE2A;&#xE32;&#xE23;&#xE17;&#xE38;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28; (ไฟล์ PDF, เปิดในแท็บใหม่)">
                            <span class="insight-date-row__date" aria-hidden="true">
                                <span class="insight-date-row__day">20</span>
                                <span class="insight-date-row__month">
                                    &#xE1E;.&#xE04;.
                                </span>
                            </span>
                            <span class="insight-date-row__body">
                                <span class="insight-date-row__title">&#xE41;&#xE2D;&#xE2A;&#xE40;&#xE0B;&#xE17; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE04;&#xE27;&#xE49;&#xE32;&#xE23;&#xE32;&#xE07;&#xE27;&#xE31;&#xE25;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE22;&#xE2D;&#xE14;&#xE40;&#xE22;&#xE35;&#xE48;&#xE22;&#xE21;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE20;&#xE17;&#xE15;&#xE23;&#xE32;&#xE2A;&#xE32;&#xE23;&#xE17;&#xE38;&#xE19;&#xE15;&#xE48;&#xE32;&#xE07;&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;</span>
                                <span class="insight-date-row__meta">&#xE23;&#xE32;&#xE07;&#xE27;&#xE31;&#xE25;&#xE41;&#xE25;&#xE30;&#xE04;&#xE27;&#xE32;&#xE21;&#xE2A;&#xE33;&#xE40;&#xE23;&#xE47;&#xE08;</span>
                            </span>
                        </a>
                    </li>
                    <li>
                        <a href="/media/documents/sample.pdf" class="insight-date-row" target="_blank" rel="noopener"
                           aria-label="&#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE40;&#xE1B;&#xE34;&#xE14;&#xE40;&#xE2A;&#xE19;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;: &#xE40;&#xE1B;&#xE34;&#xE14;&#xE40;&#xE2A;&#xE19;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE2B;&#xE21;&#xE48; A-HUMANOID (ไฟล์ PDF, เปิดในแท็บใหม่)">
                            <span class="insight-date-row__date" aria-hidden="true">
                                <span class="insight-date-row__day">18</span>
                                <span class="insight-date-row__month">
                                    &#xE1E;.&#xE04;.
                                </span>
                            </span>
                            <span class="insight-date-row__body">
                                <span class="insight-date-row__title">&#xE40;&#xE1B;&#xE34;&#xE14;&#xE40;&#xE2A;&#xE19;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;&#xE43;&#xE2B;&#xE21;&#xE48; A-HUMANOID</span>
                                <span class="insight-date-row__meta">&#xE1B;&#xE23;&#xE30;&#xE01;&#xE32;&#xE28;&#xE40;&#xE1B;&#xE34;&#xE14;&#xE40;&#xE2A;&#xE19;&#xE2D;&#xE02;&#xE32;&#xE22;&#xE01;&#xE2D;&#xE07;&#xE17;&#xE38;&#xE19;</span>
                            </span>
                        </a>
                    </li>
            </ul>

            <div class="insight-col__footer">
                <a href="/news-announcements" class="btn btn-split btn-light" aria-label="ดูข่าวสารและประกาศทั้งหมด">
                    <span>ดูทั้งหมด</span>
                    <span class="btn-split__icon">
                        <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                    </span>
                </a>
            </div>
        </section>
    </div>
</section>', N'InsightsV3', 0);
INSERT INTO [2026_web_widget] (created_at, updated_at, created_by, updated_by, sort, status, pb_status, approve_by, show_front, cat_id, title, img1, mod_name, info, section_key, pb_cat_id, pb_title, pb_img1, pb_mod_name, pb_info, pb_section_key, web_id) VALUES (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'user', N'user', 60, 1, 1, N'user', 1, @gid, N'ตัวแทนขาย', N'Files/Site0/1/widget_icons/assetplus/icon-Distributors.png', N'', N'<section class="section distributors distributors--v3">
    <div class="container">
        <div class="distributors__head">
            <h2 class="distributors__title">
                ซื้อกองทุน <strong class="distributors__brand">Asset Plus</strong> ได้ผ่านตัวแทนขายชั้นนำ
            </h2>
        </div>

        <div class="logo-board">
            <ul class="logo-board__grid">
                    <li class="logo-board__cell">
                            <a href="https://www.scb.co.th" class="logo-board__link" target="_blank" rel="noopener"
                               aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE44;&#xE17;&#xE22;&#xE1E;&#xE32;&#xE13;&#xE34;&#xE0A;&#xE22;&#xE4C; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                <span class="logo-board__logo">
                                    <img src="/media/images/agent/Thumb-0.jpg" alt="" loading="lazy" decoding="async" />
                                </span>
                                <span class="logo-board__name">SCB</span>
                            </a>
                    </li>
                    <li class="logo-board__cell">
                            <a href="https://www.kasikornbank.com" class="logo-board__link" target="_blank" rel="noopener"
                               aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE01;&#xE2A;&#xE34;&#xE01;&#xE23;&#xE44;&#xE17;&#xE22; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                <span class="logo-board__logo">
                                    <img src="/media/images/agent/Thumb-1.jpg" alt="" loading="lazy" decoding="async" />
                                </span>
                                <span class="logo-board__name">KBank</span>
                            </a>
                    </li>
                    <li class="logo-board__cell">
                            <a href="https://www.gsb.or.th" class="logo-board__link" target="_blank" rel="noopener"
                               aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE2D;&#xE2D;&#xE21;&#xE2A;&#xE34;&#xE19; (เปิดในแท็บใหม่)">
                                <span class="logo-board__logo">
                                    <img src="/media/images/agent/Thumb-2.jpg" alt="" loading="lazy" decoding="async" />
                                </span>
                                <span class="logo-board__name">Government Savings Bank</span>
                            </a>
                    </li>
                    <li class="logo-board__cell">
                            <a href="https://www.krungsri.com" class="logo-board__link" target="_blank" rel="noopener"
                               aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE01;&#xE23;&#xE38;&#xE07;&#xE28;&#xE23;&#xE35;&#xE2D;&#xE22;&#xE38;&#xE18;&#xE22;&#xE32; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                <span class="logo-board__logo">
                                    <img src="/media/images/agent/Thumb-3.jpg" alt="" loading="lazy" decoding="async" />
                                </span>
                                <span class="logo-board__name">krungsri</span>
                            </a>
                    </li>
                    <li class="logo-board__cell">
                            <a href="https://www.bangkokbank.com" class="logo-board__link" target="_blank" rel="noopener"
                               aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE01;&#xE23;&#xE38;&#xE07;&#xE40;&#xE17;&#xE1E; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                <span class="logo-board__logo">
                                    <img src="/media/images/agent/Thumb-4.jpg" alt="" loading="lazy" decoding="async" />
                                </span>
                                <span class="logo-board__name">Bangkok Bank</span>
                            </a>
                    </li>
                    <li class="logo-board__cell">
                            <a href="https://www.ttbbank.com" class="logo-board__link" target="_blank" rel="noopener"
                               aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE17;&#xE2B;&#xE32;&#xE23;&#xE44;&#xE17;&#xE22;&#xE18;&#xE19;&#xE0A;&#xE32;&#xE15; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                <span class="logo-board__logo">
                                    <img src="/media/images/agent/Thumb-15.jpg" alt="" loading="lazy" decoding="async" />
                                </span>
                                <span class="logo-board__name">ttb</span>
                            </a>
                    </li>
                    <li class="logo-board__cell">
                            <a href="https://www.cimbthai.com" class="logo-board__link" target="_blank" rel="noopener"
                               aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE0B;&#xE35;&#xE44;&#xE2D;&#xE40;&#xE2D;&#xE47;&#xE21;&#xE1A;&#xE35; &#xE44;&#xE17;&#xE22; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                <span class="logo-board__logo">
                                    <img src="/media/images/agent/Thumb-5.jpg" alt="" loading="lazy" decoding="async" />
                                </span>
                                <span class="logo-board__name">CIMB</span>
                            </a>
                    </li>
                    <li class="logo-board__cell">
                            <a href="https://www.uob.co.th" class="logo-board__link" target="_blank" rel="noopener"
                               aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE22;&#xE39;&#xE42;&#xE2D;&#xE1A;&#xE35; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                <span class="logo-board__logo">
                                    <img src="/media/images/agent/Thumb-6.jpg" alt="" loading="lazy" decoding="async" />
                                </span>
                                <span class="logo-board__name">UOB</span>
                            </a>
                    </li>
                    <li class="logo-board__cell">
                            <a href="https://www.lhbank.co.th" class="logo-board__link" target="_blank" rel="noopener"
                               aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE41;&#xE25;&#xE19;&#xE14;&#xE4C; &#xE41;&#xE2D;&#xE19;&#xE14;&#xE4C; &#xE40;&#xE2E;&#xE49;&#xE32;&#xE2A;&#xE4C; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                <span class="logo-board__logo">
                                    <img src="/media/images/agent/Thumb-7.jpg" alt="" loading="lazy" decoding="async" />
                                </span>
                                <span class="logo-board__name">LH Bank</span>
                            </a>
                    </li>
                    <li class="logo-board__cell">
                            <a href="https://bank.kkpfg.com" class="logo-board__link" target="_blank" rel="noopener"
                               aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE40;&#xE01;&#xE35;&#xE22;&#xE23;&#xE15;&#xE34;&#xE19;&#xE32;&#xE04;&#xE34;&#xE19;&#xE20;&#xE31;&#xE17;&#xE23; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                <span class="logo-board__logo">
                                    <img src="/media/images/agent/Thumb-8.jpg" alt="" loading="lazy" decoding="async" />
                                </span>
                                <span class="logo-board__name">KKP</span>
                            </a>
                    </li>
                    <li class="logo-board__cell">
                            <a href="https://www.fnsyrus.com" class="logo-board__link" target="_blank" rel="noopener"
                               aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE1F;&#xE34;&#xE19;&#xE31;&#xE19;&#xE40;&#xE0B;&#xE35;&#xE22; &#xE44;&#xE0B;&#xE23;&#xE31;&#xE2A; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                <span class="logo-board__logo">
                                    <img src="/media/images/agent/Thumb-9.jpg" alt="" loading="lazy" decoding="async" />
                                </span>
                                <span class="logo-board__name">FINANSIA</span>
                            </a>
                    </li>
                    <li class="logo-board__cell">
                            <a href="https://www.kgieworld.co.th" class="logo-board__link" target="_blank" rel="noopener"
                               aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE40;&#xE04;&#xE08;&#xE35;&#xE44;&#xE2D; (&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;&#xE44;&#xE17;&#xE22;) &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                <span class="logo-board__logo">
                                    <img src="/media/images/agent/Thumb-10.jpg" alt="" loading="lazy" decoding="async" />
                                </span>
                                <span class="logo-board__name">KGI</span>
                            </a>
                    </li>
                    <li class="logo-board__cell">
                            <a href="https://www.aira.co.th" class="logo-board__link" target="_blank" rel="noopener"
                               aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE44;&#xE2D;&#xE23;&#xE48;&#xE32; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                <span class="logo-board__logo">
                                    <img src="/media/images/agent/Thumb-11.jpg" alt="" loading="lazy" decoding="async" />
                                </span>
                                <span class="logo-board__name">AIRA</span>
                            </a>
                    </li>
                    <li class="logo-board__cell">
                            <a href="https://www.tisco.co.th" class="logo-board__link" target="_blank" rel="noopener"
                               aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE17;&#xE34;&#xE2A;&#xE42;&#xE01;&#xE49; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (เปิดในแท็บใหม่)">
                                <span class="logo-board__logo">
                                    <img src="/media/images/agent/Thumb-12.jpg" alt="" loading="lazy" decoding="async" />
                                </span>
                                <span class="logo-board__name">TISCO</span>
                            </a>
                    </li>
                    <li class="logo-board__cell">
                            <a href="https://www.asiaplus.co.th" class="logo-board__link" target="_blank" rel="noopener"
                               aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE40;&#xE2D;&#xE40;&#xE0B;&#xE35;&#xE22; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (เปิดในแท็บใหม่)">
                                <span class="logo-board__logo">
                                    <img src="/media/images/agent/Thumb-13.jpg" alt="" loading="lazy" decoding="async" />
                                </span>
                                <span class="logo-board__name">ASIA PLUS Security</span>
                            </a>
                    </li>
                    <li class="logo-board__cell">
                            <a href="https://www.dime.co.th" class="logo-board__link" target="_blank" rel="noopener"
                               aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE40;&#xE14;&#xE1F; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (Dime!) (เปิดในแท็บใหม่)">
                                <span class="logo-board__logo">
                                    <img src="/media/images/agent/Thumb-14.jpg" alt="" loading="lazy" decoding="async" />
                                </span>
                                <span class="logo-board__name">Dime</span>
                            </a>
                    </li>
            </ul>
        </div>

        <div class="distributors__footer">
            <a href="/services/distributors" class="btn btn-split btn-light" aria-label="ดูตัวแทนขายทั้งหมด">
                <span>ดูทั้งหมด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>
    </div>
</section>', N'DistributorsV3', CAST(@gid AS nvarchar(20)), N'ตัวแทนขาย', N'Files/Site0/1/widget_icons/assetplus/icon-Distributors.png', N'', N'<section class="section distributors distributors--v3">
    <div class="container">
        <div class="distributors__head">
            <h2 class="distributors__title">
                ซื้อกองทุน <strong class="distributors__brand">Asset Plus</strong> ได้ผ่านตัวแทนขายชั้นนำ
            </h2>
        </div>

        <div class="logo-board">
            <ul class="logo-board__grid">
                    <li class="logo-board__cell">
                            <a href="https://www.scb.co.th" class="logo-board__link" target="_blank" rel="noopener"
                               aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE44;&#xE17;&#xE22;&#xE1E;&#xE32;&#xE13;&#xE34;&#xE0A;&#xE22;&#xE4C; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                <span class="logo-board__logo">
                                    <img src="/media/images/agent/Thumb-0.jpg" alt="" loading="lazy" decoding="async" />
                                </span>
                                <span class="logo-board__name">SCB</span>
                            </a>
                    </li>
                    <li class="logo-board__cell">
                            <a href="https://www.kasikornbank.com" class="logo-board__link" target="_blank" rel="noopener"
                               aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE01;&#xE2A;&#xE34;&#xE01;&#xE23;&#xE44;&#xE17;&#xE22; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                <span class="logo-board__logo">
                                    <img src="/media/images/agent/Thumb-1.jpg" alt="" loading="lazy" decoding="async" />
                                </span>
                                <span class="logo-board__name">KBank</span>
                            </a>
                    </li>
                    <li class="logo-board__cell">
                            <a href="https://www.gsb.or.th" class="logo-board__link" target="_blank" rel="noopener"
                               aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE2D;&#xE2D;&#xE21;&#xE2A;&#xE34;&#xE19; (เปิดในแท็บใหม่)">
                                <span class="logo-board__logo">
                                    <img src="/media/images/agent/Thumb-2.jpg" alt="" loading="lazy" decoding="async" />
                                </span>
                                <span class="logo-board__name">Government Savings Bank</span>
                            </a>
                    </li>
                    <li class="logo-board__cell">
                            <a href="https://www.krungsri.com" class="logo-board__link" target="_blank" rel="noopener"
                               aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE01;&#xE23;&#xE38;&#xE07;&#xE28;&#xE23;&#xE35;&#xE2D;&#xE22;&#xE38;&#xE18;&#xE22;&#xE32; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                <span class="logo-board__logo">
                                    <img src="/media/images/agent/Thumb-3.jpg" alt="" loading="lazy" decoding="async" />
                                </span>
                                <span class="logo-board__name">krungsri</span>
                            </a>
                    </li>
                    <li class="logo-board__cell">
                            <a href="https://www.bangkokbank.com" class="logo-board__link" target="_blank" rel="noopener"
                               aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE01;&#xE23;&#xE38;&#xE07;&#xE40;&#xE17;&#xE1E; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                <span class="logo-board__logo">
                                    <img src="/media/images/agent/Thumb-4.jpg" alt="" loading="lazy" decoding="async" />
                                </span>
                                <span class="logo-board__name">Bangkok Bank</span>
                            </a>
                    </li>
                    <li class="logo-board__cell">
                            <a href="https://www.ttbbank.com" class="logo-board__link" target="_blank" rel="noopener"
                               aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE17;&#xE2B;&#xE32;&#xE23;&#xE44;&#xE17;&#xE22;&#xE18;&#xE19;&#xE0A;&#xE32;&#xE15; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                <span class="logo-board__logo">
                                    <img src="/media/images/agent/Thumb-15.jpg" alt="" loading="lazy" decoding="async" />
                                </span>
                                <span class="logo-board__name">ttb</span>
                            </a>
                    </li>
                    <li class="logo-board__cell">
                            <a href="https://www.cimbthai.com" class="logo-board__link" target="_blank" rel="noopener"
                               aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE0B;&#xE35;&#xE44;&#xE2D;&#xE40;&#xE2D;&#xE47;&#xE21;&#xE1A;&#xE35; &#xE44;&#xE17;&#xE22; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                <span class="logo-board__logo">
                                    <img src="/media/images/agent/Thumb-5.jpg" alt="" loading="lazy" decoding="async" />
                                </span>
                                <span class="logo-board__name">CIMB</span>
                            </a>
                    </li>
                    <li class="logo-board__cell">
                            <a href="https://www.uob.co.th" class="logo-board__link" target="_blank" rel="noopener"
                               aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE22;&#xE39;&#xE42;&#xE2D;&#xE1A;&#xE35; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                <span class="logo-board__logo">
                                    <img src="/media/images/agent/Thumb-6.jpg" alt="" loading="lazy" decoding="async" />
                                </span>
                                <span class="logo-board__name">UOB</span>
                            </a>
                    </li>
                    <li class="logo-board__cell">
                            <a href="https://www.lhbank.co.th" class="logo-board__link" target="_blank" rel="noopener"
                               aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE41;&#xE25;&#xE19;&#xE14;&#xE4C; &#xE41;&#xE2D;&#xE19;&#xE14;&#xE4C; &#xE40;&#xE2E;&#xE49;&#xE32;&#xE2A;&#xE4C; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                <span class="logo-board__logo">
                                    <img src="/media/images/agent/Thumb-7.jpg" alt="" loading="lazy" decoding="async" />
                                </span>
                                <span class="logo-board__name">LH Bank</span>
                            </a>
                    </li>
                    <li class="logo-board__cell">
                            <a href="https://bank.kkpfg.com" class="logo-board__link" target="_blank" rel="noopener"
                               aria-label="&#xE18;&#xE19;&#xE32;&#xE04;&#xE32;&#xE23;&#xE40;&#xE01;&#xE35;&#xE22;&#xE23;&#xE15;&#xE34;&#xE19;&#xE32;&#xE04;&#xE34;&#xE19;&#xE20;&#xE31;&#xE17;&#xE23; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                <span class="logo-board__logo">
                                    <img src="/media/images/agent/Thumb-8.jpg" alt="" loading="lazy" decoding="async" />
                                </span>
                                <span class="logo-board__name">KKP</span>
                            </a>
                    </li>
                    <li class="logo-board__cell">
                            <a href="https://www.fnsyrus.com" class="logo-board__link" target="_blank" rel="noopener"
                               aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE1F;&#xE34;&#xE19;&#xE31;&#xE19;&#xE40;&#xE0B;&#xE35;&#xE22; &#xE44;&#xE0B;&#xE23;&#xE31;&#xE2A; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                <span class="logo-board__logo">
                                    <img src="/media/images/agent/Thumb-9.jpg" alt="" loading="lazy" decoding="async" />
                                </span>
                                <span class="logo-board__name">FINANSIA</span>
                            </a>
                    </li>
                    <li class="logo-board__cell">
                            <a href="https://www.kgieworld.co.th" class="logo-board__link" target="_blank" rel="noopener"
                               aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE40;&#xE04;&#xE08;&#xE35;&#xE44;&#xE2D; (&#xE1B;&#xE23;&#xE30;&#xE40;&#xE17;&#xE28;&#xE44;&#xE17;&#xE22;) &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                <span class="logo-board__logo">
                                    <img src="/media/images/agent/Thumb-10.jpg" alt="" loading="lazy" decoding="async" />
                                </span>
                                <span class="logo-board__name">KGI</span>
                            </a>
                    </li>
                    <li class="logo-board__cell">
                            <a href="https://www.aira.co.th" class="logo-board__link" target="_blank" rel="noopener"
                               aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE44;&#xE2D;&#xE23;&#xE48;&#xE32; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (&#xE21;&#xE2B;&#xE32;&#xE0A;&#xE19;) (เปิดในแท็บใหม่)">
                                <span class="logo-board__logo">
                                    <img src="/media/images/agent/Thumb-11.jpg" alt="" loading="lazy" decoding="async" />
                                </span>
                                <span class="logo-board__name">AIRA</span>
                            </a>
                    </li>
                    <li class="logo-board__cell">
                            <a href="https://www.tisco.co.th" class="logo-board__link" target="_blank" rel="noopener"
                               aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE17;&#xE34;&#xE2A;&#xE42;&#xE01;&#xE49; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (เปิดในแท็บใหม่)">
                                <span class="logo-board__logo">
                                    <img src="/media/images/agent/Thumb-12.jpg" alt="" loading="lazy" decoding="async" />
                                </span>
                                <span class="logo-board__name">TISCO</span>
                            </a>
                    </li>
                    <li class="logo-board__cell">
                            <a href="https://www.asiaplus.co.th" class="logo-board__link" target="_blank" rel="noopener"
                               aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE40;&#xE2D;&#xE40;&#xE0B;&#xE35;&#xE22; &#xE1E;&#xE25;&#xE31;&#xE2A; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (เปิดในแท็บใหม่)">
                                <span class="logo-board__logo">
                                    <img src="/media/images/agent/Thumb-13.jpg" alt="" loading="lazy" decoding="async" />
                                </span>
                                <span class="logo-board__name">ASIA PLUS Security</span>
                            </a>
                    </li>
                    <li class="logo-board__cell">
                            <a href="https://www.dime.co.th" class="logo-board__link" target="_blank" rel="noopener"
                               aria-label="&#xE1A;&#xE23;&#xE34;&#xE29;&#xE31;&#xE17;&#xE2B;&#xE25;&#xE31;&#xE01;&#xE17;&#xE23;&#xE31;&#xE1E;&#xE22;&#xE4C; &#xE40;&#xE14;&#xE1F; &#xE08;&#xE33;&#xE01;&#xE31;&#xE14; (Dime!) (เปิดในแท็บใหม่)">
                                <span class="logo-board__logo">
                                    <img src="/media/images/agent/Thumb-14.jpg" alt="" loading="lazy" decoding="async" />
                                </span>
                                <span class="logo-board__name">Dime</span>
                            </a>
                    </li>
            </ul>
        </div>

        <div class="distributors__footer">
            <a href="/services/distributors" class="btn btn-split btn-light" aria-label="ดูตัวแทนขายทั้งหมด">
                <span>ดูทั้งหมด</span>
                <span class="btn-split__icon">
                    <i class="bi bi-arrow-right-short" aria-hidden="true"></i>
                </span>
            </a>
        </div>
    </div>
</section>', N'DistributorsV3', 0);
DECLARE @g1 bigint = (SELECT id FROM @g WHERE n = 1);
DECLARE @layout nvarchar(500) = '';
SET @layout = @layout + CASE WHEN @layout = '' THEN '' ELSE ',' END + 'wg_' + CAST((SELECT id FROM [2026_web_widget] WHERE cat_id = @g1 AND section_key = N'Hero') AS nvarchar(20));
SET @layout = @layout + CASE WHEN @layout = '' THEN '' ELSE ',' END + 'wg_' + CAST((SELECT id FROM [2026_web_widget] WHERE cat_id = @g1 AND section_key = N'NavPrices') AS nvarchar(20));
SET @layout = @layout + CASE WHEN @layout = '' THEN '' ELSE ',' END + 'wg_' + CAST((SELECT id FROM [2026_web_widget] WHERE cat_id = @g1 AND section_key = N'FeaturedFunds') AS nvarchar(20));
SET @layout = @layout + CASE WHEN @layout = '' THEN '' ELSE ',' END + 'wg_' + CAST((SELECT id FROM [2026_web_widget] WHERE cat_id = @g1 AND section_key = N'ExploreThemes') AS nvarchar(20));
SET @layout = @layout + CASE WHEN @layout = '' THEN '' ELSE ',' END + 'wg_' + CAST((SELECT id FROM [2026_web_widget] WHERE cat_id = @g1 AND section_key = N'Insights') AS nvarchar(20));
SET @layout = @layout + CASE WHEN @layout = '' THEN '' ELSE ',' END + 'wg_' + CAST((SELECT id FROM [2026_web_widget] WHERE cat_id = @g1 AND section_key = N'Distributors') AS nvarchar(20));
UPDATE [2026_web_cms_page] SET box_layout = @layout, pb_box_layout = @layout, updated_at = SYSDATETIMEOFFSET(), updated_by = N'user' WHERE id = 1 AND is_home = 1;
SELECT id, title, section_key, cat_id, sort, LEN(info) info_len FROM [2026_web_widget] WHERE section_key IS NOT NULL ORDER BY cat_id, sort;
SELECT id, box_layout, pb_box_layout FROM [2026_web_cms_page] WHERE id = 1;
COMMIT;
