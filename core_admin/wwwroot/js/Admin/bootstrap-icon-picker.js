/* =========================================================================
 * Bootstrap Icon Picker — โมดอลเลือกไอคอนจาก Bootstrap Icons
 * ใช้คู่กับ AdminHelpers.InputBootstrapIcon และไฟล์ข้อมูล bootstrap-icons-list.js
 *
 *  - openBsIconPicker(inputId)   : เปิดโมดอลเพื่อเลือกไอคอนให้ input ที่ระบุ
 *  - clearBsIcon(inputId)        : ล้างค่าไอคอน
 *  - refreshBsIconPreview(inputId): อัปเดตรูปตัวอย่างจากค่าใน input
 * ค่าที่เก็บใน input จะอยู่ในรูป "bi bi-telephone"
 * ========================================================================= */
(function () {
    "use strict";

    var ICON_PREFIX = "bi bi-";      // ค่าที่บันทึก เช่น "bi bi-telephone"
    var targetId = null;             // id ของ input ที่กำลังเลือกอยู่
    var modalEl = null;              // element โมดอล (สร้างครั้งเดียว)
    var gridEl, searchEl, countEl;
    var filtered = [];

    function icons() {
        return (typeof BOOTSTRAP_ICONS !== "undefined" && BOOTSTRAP_ICONS) ? BOOTSTRAP_ICONS : [];
    }

    // แปลงค่าใน input ("bi bi-telephone") ให้เหลือชื่อไอคอน ("telephone")
    function nameFromValue(val) {
        if (!val) return "";
        var parts = ("" + val).trim().split(/\s+/);
        for (var i = 0; i < parts.length; i++) {
            if (parts[i].indexOf("bi-") === 0) return parts[i].substring(3);
        }
        return "";
    }

    function buildModal() {
        if (modalEl) return;

        modalEl = document.createElement("div");
        modalEl.className = "bsicon-modal-overlay";
        modalEl.innerHTML =
            '<div class="bsicon-modal">' +
                '<div class="bsicon-modal-head">' +
                    '<span class="bsicon-modal-title"><i class="bi bi-grid-3x3-gap-fill"></i> เลือกไอคอน (Bootstrap Icons)</span>' +
                    '<button type="button" class="bsicon-modal-close" aria-label="ปิด">&times;</button>' +
                '</div>' +
                '<div class="bsicon-modal-search">' +
                    '<i class="bi bi-search"></i>' +
                    '<input type="text" class="form-control" placeholder="ค้นหาไอคอน เช่น telephone, facebook, envelope...">' +
                    '<span class="bsicon-count"></span>' +
                '</div>' +
                '<div class="bsicon-grid"></div>' +
            '</div>';
        document.body.appendChild(modalEl);

        gridEl = modalEl.querySelector(".bsicon-grid");
        searchEl = modalEl.querySelector(".bsicon-modal-search input");
        countEl = modalEl.querySelector(".bsicon-count");

        // ปิดโมดอล
        modalEl.querySelector(".bsicon-modal-close").addEventListener("click", closeModal);
        modalEl.addEventListener("click", function (e) {
            if (e.target === modalEl) closeModal();
        });
        document.addEventListener("keydown", function (e) {
            if (e.key === "Escape" && modalEl.classList.contains("open")) closeModal();
        });

        // ค้นหา
        var t = null;
        searchEl.addEventListener("input", function () {
            clearTimeout(t);
            t = setTimeout(applyFilter, 120);
        });

        // เลือกไอคอน (event delegation)
        gridEl.addEventListener("click", function (e) {
            var cell = e.target.closest(".bsicon-cell");
            if (cell) pickIcon(cell.getAttribute("data-name"));
        });
    }

    function applyFilter() {
        var q = (searchEl.value || "").trim().toLowerCase();
        var all = icons();
        filtered = q ? all.filter(function (n) { return n.indexOf(q) !== -1; }) : all.slice();

        // เรนเดอร์ไอคอนทั้งหมดในครั้งเดียว (รายการเบา ไม่ต้อง lazy load)
        var html = "";
        for (var i = 0; i < filtered.length; i++) {
            var n = filtered[i];
            html += '<button type="button" class="bsicon-cell" data-name="' + n + '" title="' + n + '">' +
                        '<i class="bi bi-' + n + '"></i>' +
                        '<span>' + n + '</span>' +
                    '</button>';
        }
        gridEl.innerHTML = filtered.length
            ? html
            : '<div class="bsicon-empty">ไม่พบไอคอนที่ตรงกับคำค้นหา</div>';
        gridEl.scrollTop = 0;
        countEl.textContent = filtered.length.toLocaleString() + " ไอคอน";
    }

    function highlightCurrent() {
        var input = document.getElementById(targetId);
        var cur = input ? nameFromValue(input.value) : "";
        var cells = gridEl.querySelectorAll(".bsicon-cell.active");
        for (var i = 0; i < cells.length; i++) cells[i].classList.remove("active");
        if (cur) {
            var el = gridEl.querySelector('.bsicon-cell[data-name="' + cur + '"]');
            if (el) {
                el.classList.add("active");
                el.scrollIntoView({ block: "center" });
            }
        }
    }

    function pickIcon(name) {
        var input = document.getElementById(targetId);
        if (input) {
            input.value = ICON_PREFIX + name;
            input.dispatchEvent(new Event("input", { bubbles: true }));
            input.dispatchEvent(new Event("change", { bubbles: true }));
        }
        refreshBsIconPreview(targetId);
        closeModal();
    }

    function closeModal() {
        if (modalEl) modalEl.classList.remove("open");
        document.body.style.overflow = "";
    }

    // ---------- API สาธารณะ ----------
    window.openBsIconPicker = function (inputId) {
        targetId = inputId;
        buildModal();
        searchEl.value = "";
        applyFilter();
        modalEl.classList.add("open");
        document.body.style.overflow = "hidden";
        highlightCurrent();
        setTimeout(function () { searchEl.focus(); }, 50);
    };

    window.clearBsIcon = function (inputId) {
        var input = document.getElementById(inputId);
        if (input) {
            input.value = "";
            input.dispatchEvent(new Event("input", { bubbles: true }));
            input.dispatchEvent(new Event("change", { bubbles: true }));
        }
        refreshBsIconPreview(inputId);
    };

    window.refreshBsIconPreview = function (inputId) {
        var input = document.getElementById(inputId);
        if (!input) return;
        var prev = document.getElementById(inputId + "_preview");
        var del = document.getElementById(inputId + "_del");
        var name = nameFromValue(input.value);
        if (prev) prev.className = name ? ("bi bi-" + name) : "bi bi-emoji-neutral text-muted";
        if (del) del.style.display = name ? "" : "none";
    };
})();
