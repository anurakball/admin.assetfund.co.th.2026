var webID = $("meta[name='web-id']").prop('content');

function preload(showHide) {
    if (showHide.toLowerCase() == 'show') {
        $(".loading").fadeIn();
    } else if (showHide.toLowerCase() == 'hide') {
        $(".loading").fadeOut();
    } else {
        $(".loading").fadeToggle();
    }
}

function logOut() {
    Swal.fire({
        title: 'ออกจากระบบ ?',
        text: "คุณต้องการออกจากระบบ ?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#0047B6',
        cancelButtonColor: '#BC1320',
        confirmButtonText: 'ใช่',
        cancelButtonText: 'ไม่ใช่',
        showClass: {
            popup: 'animate__animated animate__bounceIn'
        },
        hideClass: {
            popup: 'animate__animated animate__bounceOut'
        }
    }).then((result) => {
        if (result.isConfirmed) {
            $('<form method="post" action="/Admin/User/Logout"><input type="hidden" name="__RequestVerificationToken" value="' + $('meta[name=csrf-token]').prop('content') +'"></form>').appendTo('body').submit();
        }
    });
}

function activeMenu() {
    var currentPath = window.location.pathname.toLocaleLowerCase();
    $(".nav.pcoded-inner-navbar li a").each(function (i, el) {

        //console.log('el',el);
        var menuLink = $(this).prop('href').toLocaleLowerCase();
        menuLink = menuLink.replace(window.location.origin, '');
        //console.log('menuLink', menuLink);
        //console.log('currentPath', currentPath);
        //console.log('if', (menuLink.split('/').length >= 3 && currentPath.indexOf(menuLink) == 0));
        if (menuLink.split('/').length >= 3 && (currentPath == menuLink || currentPath.indexOf(menuLink + "/") == 0) /*currentPath.indexOf(menuLink) == 0*/) {
            $(this).closest("li").addClass("active");
            if ($(this).closest("ul.pcoded-submenu").length > 0) {
                $(this).closest("ul.pcoded-submenu").closest("li").addClass("pcoded-trigger");
                $(this).closest("ul.pcoded-submenu").show();
            }
        }
    });

    if (typeof ($(".nav.pcoded-inner-navbar").find('li.active').offset()) != 'undefined') {
        $($(".navbar-content.scroll-div")).animate({
            scrollTop: $(".nav.pcoded-inner-navbar").find('li.active').offset().top - 130
        }, 0);
    }
}

function sConfrim(form) {
    Swal.fire({
        title: 'ยืนยัน ?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#0047B6',
        cancelButtonColor: '#BC1320',
        confirmButtonText: 'ใช่',
        cancelButtonText: 'ปิด',
        showClass: {
            popup: 'animate__animated animate__bounceIn'
        },
        hideClass: {
            popup: 'animate__animated animate__bounceOut'
        }
    }).then((result) => {
        if (result.isConfirmed) {
            form.submit();
        } else {
            return false;
        }
    });
    return false;
}

function sAlert(title, html = "", icon = "") {
    return Swal.fire({
        title: title,
        html: html,
        icon: icon,
        confirmButtonText: 'ปิด',
        confirmButtonColor: '#BC1320',
        showClass: {
            popup: 'animate__animated animate__bounceIn'
        },
        hideClass: {
            popup: 'animate__animated animate__bounceOut'
        }
    });
}

function alertModal(txt, icon = 'info', thenFn) {
    Swal.fire({
        title: ("แจ้งเตือน"),
        text: txt,
        icon: icon == '' ? 'info' : icon,
        confirmButtonColor: '#BC1320',
        confirmButtonText: 'ปิด',
        showClass: {
            popup: 'animate__animated animate__bounceIn'
        },
        hideClass: {
            //popup: 'animate__animated animate__bounceOut'
        }
    }).then((result) => {
        if (typeof (thenFn) === 'function') {
            setTimeout(function () {
                thenFn(result);
            }, 500);
        }
    });
}

function confirmModal(txt, icon = 'info', thenFn) {
    Swal.fire({
        title: ("แจ้งเตือน"),
        text: txt,
        icon: icon == '' ? 'info' : icon,
        showCancelButton: true,
        confirmButtonColor: '#0047B6',
        cancelButtonColor: '#BC1320',
        confirmButtonText: 'ใช่',
        cancelButtonText: 'ปิด',
        showClass: {
            popup: 'animate__animated animate__bounceIn'
        },
        hideClass: {
            popup: 'animate__animated animate__bounceOut'
        }
    }).then((result) => {
        if (typeof (thenFn) === 'function') {
            setTimeout(function () {
                thenFn(result);
            }, 500);
        }
    });
}

function searchStringInArray(str, strArray) {
    for (var j = 0; j < strArray.length; j++) {
        if (str.indexOf(strArray[j]) > -1) {
            return str.indexOf(strArray[j]);
        }
    }
    return -1;
} 

function updateQueryStringParameter(uri, key, value) {
    uri = uri.split('#')[0];
    var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
    var separator = uri.indexOf('?') !== -1 ? "&" : "?";
    if (uri.match(re)) {
        return uri.replace(re, '$1' + key + "=" + value + '$2');
    }
    else {
        return uri + separator + key + "=" + value;
    }
}

function deleteMultiple(mod_name) {
    if ($(":input[name=_id]:checked").length == 0) {
        sAlert('เลือกรายการที่ต้องลบ');
    } else {
        var valArray = [];
        $("input:checkbox[name=_id]:checked").each(function () {
            valArray.push($(this).val());
        });

        deleteRow(mod_name, valArray.join(), getCustomAlertTxt(mod_name, valArray.join()));
    }
}

function deleteMultipleUser(mod_name) {
    if ($(":input[name=_id]:checked").length == 0) {
        sAlert('เลือกรายการที่ต้องลบ');
    } else {
        var valArray = [];
        $("input:checkbox[name=_id]:checked").each(function () {
            valArray.push($(this).val());
        });

        deleteRow(mod_name, valArray.join(), getCustomAlertTxtUser(mod_name, valArray.join()));
    }
}

function deleteRow(mod_name, id, otherTxt = "") {
    let no_item = '1';
    //console.log(id.split(','));
    if (id.split(',').length > 0) {
        no_item = id.split(',').length;
    }

    //------ check child before delete -----//
    var checkDeleteTitle = "";
    var checkDeleteText = "";
    $.each(id.split(','), function (i, v) {
        let _child = $("table.table.table-striped-custom tbody tr[data-id='" + v + "']").find("td[data-child-count]");
        //console.log(_child);
        //console.log(_child.data());
        //console.log('condition', (_child.data() != null && _child.data().childCount != "" && parseInt(_child.data().childCount) > 0));
        if (_child.data() != null && _child.data().childCount != "" && parseInt(_child.data().childCount) > 0) {
            checkDeleteTitle = 'ไม่สามารถลบรายการได้';
            checkDeleteText = 'เนื่องจาก <i><u>' + _child.data().title + '</u></i> มีรายการย่อย <span class="text-danger fw-bolder">' + _child.data().childCount + ' รายการ</span>';
            return;
        } 
    });

    if (checkDeleteTitle != "") {
        sAlert(checkDeleteTitle, checkDeleteText,"warning");
        return;
    }
    //------ end check child before delete -----//

    let alertText = "คุณต้องการลบข้อมูล " + no_item + " รายการ ?";
    if (otherTxt != "") {
        alertText = otherTxt + "<br/>" + alertText;
    }

    Swal.fire({
        title: 'ลบข้อมูล ?',
        //text: "คุณต้องการลบข้อมูล " + no_item + " รายการ ?",
        html: alertText,
        icon: 'error',
        showCancelButton: true,
        confirmButtonColor: '#0047B6',
        cancelButtonColor: '#BC1320',
        confirmButtonText: 'ใช่',
        cancelButtonText: 'ไม่ใช่',
        showClass: {
            popup: 'animate__animated animate__bounceIn'
        },
        hideClass: {
            popup: 'animate__animated animate__bounceOut'
        }
    }).then((result) => {
        if (result.isConfirmed) {
            $('<form method="post" action="/Admin/' + mod_name + '/Delete"><input type="hidden" name="id" value="' + id + '"><input type="hidden" name="__RequestVerificationToken" value="' + $('meta[name=csrf-token]').prop('content') + '"></form>').appendTo('body').submit();
        }
    });
}

function statusRow(mod_name, id, status_row) {
    //var _status_txt = status_row == "1" ? "เปิด" : "ปิด";

    var _status_txt = "";
    var _status_txt_2 = "";

    if (status_row == "2")
    { 
        _status_txt = "ปักหมุดการแสดงผล ?";
        _status_txt_2 = "คุณต้องการปักหมุดการแสดงผล ?";
    }
    else if (status_row == "1")
    {
        _status_txt = "กำหนดการแสดงผลปกติ ?";
        _status_txt_2 = "คุณต้องการกำหนดการแสดงผลปกติ ?";
    }
    else
    {
        _status_txt = "ซ่อนการแสดงผล ?";
        _status_txt_2 = "คุณต้องการซ่อนการแสดงผล ?";
    }

    var _status_icon = "";
    if (status_row == "2") {
        _status_icon = "warning";
    }
    else if (status_row == "1") {
        _status_icon = "warning";
    }
    else {
        _status_icon = "error";
    }
    Swal.fire({
        title: _status_txt,
        text: _status_txt_2,
        icon: _status_icon,
        showCancelButton: true,
        confirmButtonColor: '#0047B6',
        cancelButtonColor: '#BC1320',
        confirmButtonText: 'ใช่',
        cancelButtonText: 'ไม่ใช่',
        showClass: {
            popup: 'animate__animated animate__bounceIn'
        },
        hideClass: {
            popup: 'animate__animated animate__bounceOut'
        }
    }).then((result) => {
        if (result.isConfirmed) {
            $('<form method="post" action="/Admin/' + mod_name + '/Status"><input type="hidden" name="id" value="' + id + '"><input type="hidden" name="status" value="' + status_row + '"><input type="hidden" name="__RequestVerificationToken" value="' + $('meta[name=csrf-token]').prop('content') + '"></form>').appendTo('body').submit();
        }
    });
}

function approveRow(mod_name, id, approve_status) {
    var _status_txt = approve_status == "1" ? "อนุมัติ" : "ไม่อนุมัติ";
    Swal.fire({
        title: _status_txt + ' รายการ ?',
        text: "คุณต้องการ" + _status_txt + " ใช่หรือไม่ ?",
        icon: approve_status == "1" ? 'warning' : 'error',
        showCancelButton: true,
        confirmButtonColor: '#0047B6',
        cancelButtonColor: '#BC1320',
        confirmButtonText: 'ใช่',
        cancelButtonText: 'ไม่ใช่',
        showClass: {
            popup: 'animate__animated animate__bounceIn'
        },
        hideClass: {
            popup: 'animate__animated animate__bounceOut'
        }
    }).then((result) => {
        if (result.isConfirmed) {
            $('<form method="post" action="/Admin/' + mod_name + '/Approve"><input type="hidden" name="id" value="' + id + '"><input type="hidden" name="approve" value="' + approve_status + '"><input type="hidden" name="__RequestVerificationToken" value="' + $('meta[name=csrf-token]').prop('content') + '"></form>').appendTo('body').submit();
        }
    });
}

function moveRow(mod_name, id, move, step, draggableUI, originSort) {
    /*var _move_txt = move == "up" ? "ขึ้น" : "ลง";
    Swal.fire({
        title: 'เลื่อน' + _move_txt + ' ?',
        text: "คุณต้องการเลื่อน" + _move_txt + " ?",
        icon: 'info',
        showCancelButton: true,
        confirmButtonColor: '#0047B6',
        cancelButtonColor: '#BC1320',
        confirmButtonText: 'ใช่',
        cancelButtonText: 'ไม่ใช่',
        showClass: {
            popup: 'animate__animated animate__bounceIn'
        },
        hideClass: {
            popup: 'animate__animated animate__bounceOut'
        }
    }).then((result) => {
        if (result.isConfirmed) {
            $('<form method="post" action="/Admin/' + mod_name + '/Move"><input type="hidden" name="id" value="' + id + '"><input type="hidden" name="move" value="' + move + '"><input type="hidden" name="__RequestVerificationToken" value="' + $('meta[name=csrf-token]').prop('content') + '"></form>').appendTo('body').submit();
        }
    });*/

    var _move_txt = move == "up" ? "ขึ้น" : "ลง";
    step = (step == null) ? 1 : step;
    Swal.fire({
        title: 'เลื่อน' + _move_txt + ' ?',
        text: "คุณต้องการเลื่อน" + _move_txt + " ?",
        icon: 'info',
        showCancelButton: true,
        confirmButtonColor: '#0047B6',
        cancelButtonColor: '#BC1320',
        confirmButtonText: 'ใช่',
        cancelButtonText: 'ไม่ใช่',
        showClass: {
            popup: 'animate__animated animate__bounceIn'
        },
        hideClass: {
            popup: 'animate__animated animate__bounceOut'
        }
    }).then((result) => {
        if (result.isConfirmed) {
            
            var $new_form = $('<form>', {
                'action': '/Admin/' + mod_name + '/Move',
                'method': 'POST'
            }).append($('<input>', {
                'name': '__RequestVerificationToken',
                'value': $('meta[name=csrf-token]').prop('content'),
                'type': 'hidden'
            })).append($('<input>', {
                'name': 'id',
                'value': id,
                'type': 'hidden'
            })).append($('<input>', {
                'name': 'move',
                'value': move,
                'type': 'hidden'
            })).append($('<input>', {
                'name': 'step',
                'value': step,
                'type': 'hidden'
            }));
            //console.log($new_form);
            // send form
            $new_form.appendTo('body').submit();
        } else if (draggableUI != null && originSort != null) {
            try {
                console.log('undo sort');

                draggableUI.sort(originSort, true);
            } catch (error) {
                
            }
        }
    });


    

    //new Sortable(document.querySelector("table.table.table-striped-custom tbody"), {
    //    animation: 150,
    //    onEnd: function (/**Event*/evt) {
    //        console.log(evt);
    //        var itemEl = evt.item;  // dragged HTMLElement
    //        console.log('evt.to', evt.to);    // target list
    //        console.log('evt.from', evt.from);  // previous list
    //        console.log('evt.oldIndex', evt.oldIndex);  // element's old index within old parent
    //        console.log('evt.newIndex', evt.newIndex);  // element's new index within new parent
    //        console.log('evt.oldDraggableIndex', evt.oldDraggableIndex); // element's old index within old parent, only counting draggable elements
    //        console.log('evt.newDraggableIndex', evt.newDraggableIndex); // element's new index within new parent, only counting draggable elements
    //        console.log('evt.clone', evt.clone); // the clone element
    //        console.log('evt.pullMode', evt.pullMode);  // when item is in another sortable: `"clone"` if cloning, `true` if moving
    //    },
    //});
}

function difference(a, b) { return Math.abs(a - b); }

function dragRow(mod_name) {
    var originSort;
    var sortUI = new Sortable(document.querySelector("table.table.table-striped-custom tbody"), {
        handle: '.move_handle',
        animation: 150,
        onStart: function (/**Event*/evt) {
		    //evt.oldIndex;  // element index within parent
            originSort = sortUI.toArray();
            console.log('onStart [originSort]', originSort);
	    },
        onEnd: function (/**Event*/evt) {
            //console.log(evt);
            var itemEl = evt.item;  // dragged HTMLElement
            //console.log('evt.to', evt.to);    // target list
            //console.log('evt.from', evt.from);  // previous list
            console.log('evt.oldIndex', evt.oldIndex);  // element's old index within old parent
            console.log('evt.newIndex', evt.newIndex);  // element's new index within new parent
            //console.log('evt.oldDraggableIndex', evt.oldDraggableIndex); // element's old index within old parent, only counting draggable elements
            //console.log('evt.newDraggableIndex', evt.newDraggableIndex); // element's new index within new parent, only counting draggable elements
            //console.log('evt.clone', evt.clone); // the clone element
            //console.log('evt.pullMode', evt.pullMode);  // when item is in another sortable: `"clone"` if cloning, `true` if moving

            console.log('mod_name', mod_name);
            console.log('id', itemEl.dataset.id);
            console.log('move', (evt.oldIndex > evt.newIndex ? 'up' : 'down'));
            console.log('step', difference(evt.oldIndex, evt.newIndex));
            console.log('draggableUI', sortUI);
            console.log('originSort', originSort);

            if (difference(evt.oldIndex, evt.newIndex) > 0) {
                moveRow(
                    mod_name,
                    itemEl.dataset.id,
                    (evt.oldIndex > evt.newIndex ? 'up' : 'down'),
                    difference(evt.oldIndex, evt.newIndex),
                    sortUI,
                    originSort
                )
            }
        },
    });
}

function exportData(ele, mod_name, methodName = "Export") {
    var export_query = ele.data('query');
    var totalrows = ele.data('totalrows');
    var export_id = ele.data('exportid');
    if (export_id !== undefined) {
        if ($.trim(export_id) != "") {
            var export_id_Array = export_id.split(',');
            if (export_id_Array.length > 0) {
                totalrows = export_id_Array.length;
            }
        }
    }
    else {
        export_id = "";
    }
    if (totalrows == null || totalrows == "" || totalrows == "0") {
        sAlert("ไม่มีข้อมูลที่ต้อง export");
    } else {
        Swal.fire({
            title: 'Export ?',
            text: "คุณต้องการ Export ข้อมูล " + totalrows + " รายการ ?",
            icon: 'info',
            showCancelButton: true,
            confirmButtonColor: '#0047B6',
            cancelButtonColor: '#BC1320',
            confirmButtonText: 'ใช่',
            cancelButtonText: 'ไม่ใช่',
            showClass: {
                popup: 'animate__animated animate__bounceIn'
            },
            hideClass: {
                popup: 'animate__animated animate__bounceOut'
            }
        }).then((result) => {
            if (result.isConfirmed) {
                var $new_form = $('<form>', {
                    'action': '/Admin/' + mod_name + '/' + methodName,
                    'method': 'POST',
                    'target': '_top'
                }).append($('<input>', {
                    'name': '__RequestVerificationToken',
                    'value': $('meta[name=csrf-token]').prop('content'),
                    'type': 'hidden'
                })).append($('<input>', {
                    'name': 'export',
                    'value': export_query,
                    'type': 'hidden'
                })).append($('<input>', {
                    'name': 'export_id',
                    'value': export_id,
                    'type': 'hidden'
                }));
                //console.log($new_form);
                // send form
                $new_form.appendTo('body').submit();
            }
        });
    }
}

function getWidth() {
    return Math.max(
        document.body.scrollWidth,
        document.documentElement.scrollWidth,
        document.body.offsetWidth,
        document.documentElement.offsetWidth,
        document.documentElement.clientWidth
    );
}

function getHeight() {
    return Math.max(
        document.body.scrollHeight,
        document.documentElement.scrollHeight,
        document.body.offsetHeight,
        document.documentElement.offsetHeight,
        document.documentElement.clientHeight
    );
}

function innitCK(input_id) {
    //var uploadTargetHash = 'v1_RmlsZXM1';
    //var uploadTargetHash = getstartPathHash('v1_', 'Site' + webID) + '1';
    var uploadTargetHash = $('meta[name=elfinder-roots-hash]').prop('content');
     
    // elFinder connector URL
    var connectorUrl = window.location.origin + '/el-finder/file-system/connector';
    window['ckEditor_' + input_id] = ClassicEditor
        .create(document.querySelector('#' + input_id), {
            htmlSupport: {
                allow: [
                    {
                        name: /.*/,
                        attributes: true,
                        classes: true,
                        styles: true
                    }
                ],
                allowEmpty: ["i","span"]
            },
            fontFamily: {
                options: [
                    'default',
                    'Kanit',
                    'Arial, Helvetica, sans-serif',
                    'Courier New, Courier, monospace',
                    'Georgia, serif',
                    'Tahoma, Geneva, sans-serif',
                    'Times New Roman, Times, serif',
                    'Verdana, Geneva, sans-serif'
                ]
            },
            fontSize: {
                options: [
                    'default',
                    12,
                    14, 
                    16,
                    18,
                    20,
                    22,
                    24,
                    26,
                    28,
                    30,
                ]
            },
            toolbar: {
                items: [
                    'sourceEditing',
                    'undo',
                    'redo',
                    '|',
                    'bold',
                    'italic',
                    'underline',
                    'link',
                    'bulletedList',
                    'numberedList',
                    '|',
                    'fontFamily',
                    'fontColor',
                    'fontBackgroundColor',
                    'fontSize',
                    'highlight',
                    '|',
                    'alignment',
                    'outdent',
                    'indent',
                    'pageBreak',
                    '|',
                    /*'imageUpload',*/
                    'imageInsert',
                    'CKFinder',
                    'CKBox',
                    'mediaEmbed',
                    'insertTable',
                    '|', 
                    'findAndReplace',
                    'showBlocks',
                    'selectAll',
                    'blockQuote',
                    'code'
                ]
            },
            indentBlock: {
                classes: [
                    'custom-block-indent-a', // First step - smallest indentation.
                    'custom-block-indent-b',
                    'custom-block-indent-c'  // Last step - biggest indentation.
                ]
            },
            mediaEmbed: {
                previewsInData: true
            },
            /*htmlEmbed: {
                        showPreviews: true,
                sanitizeHtml: ( inputHtml ) => {
                    // Strip unsafe elements and attributes, e.g.:
                    // the `<script>` elements and `on*` attributes.
                    //https://www.npmjs.com/package/sanitize-html
                    const outputHtml = sanitizeHTML( inputHtml );

                    return {
                        html: outputHtml,
                        // true or false depending on whether the sanitizer stripped anything.
                        hasChanged: true
                    };
                }
            },*/
            language: 'en',
            image: {
                styles: [
                    'alignLeft', 'alignCenter', 'alignRight'/*, 'full', 'side'*/
                ],
                toolbar: [
                    'imageStyle:alignLeft', 'imageStyle:alignCenter', 'imageStyle:alignRight',
                    'imageTextAlternative',
                    /*'imageStyle:full',
                    'imageStyle:side',*/
                    'linkImage'
                ]
            },
            table: {
                contentToolbar: [
                    'tableColumn',
                    'tableRow',
                    'mergeTableCells',
                    'tableCellProperties',
                    'tableProperties'
                ]
            },
            link: {
                addTargetToExternalLinks: true,
            },
            list: {
                properties: {
                    styles: true,
                    startIndex: false,
                    reversed: false
                }
            },
            fontColor: {
				columns: 20,
				documentColors: 200,
				colors: [
				{color: 'hsl(6, 54%, 95%)', label:' '}, {color: 'hsl(6, 54%, 89%)', label:' '}, {color: 'hsl(6, 54%, 78%)', label:' '}, {color: 'hsl(6, 54%, 68%)', label:' '}, {color: 'hsl(6, 54%, 57%)', label:' '}, {color: 'hsl(6, 63%, 46%)', label:' '}, {color: 'hsl(6, 63%, 41%)', label:' '}, {color: 'hsl(6, 63%, 35%)', label:' '}, {color: 'hsl(6, 63%, 29%)', label:' '}, {color: 'hsl(6, 63%, 24%)', label:' '}, {color: 'hsl(6, 78%, 96%)', label:' '}, {color: 'hsl(6, 78%, 91%)', label:' '}, {color: 'hsl(6, 78%, 83%)', label:' '}, {color: 'hsl(6, 78%, 74%)', label:' '}, {color: 'hsl(6, 78%, 66%)', label:' '}, {color: 'hsl(6, 78%, 57%)', label:' '}, {color: 'hsl(6, 59%, 50%)', label:' '}, {color: 'hsl(6, 59%, 43%)', label:' '}, {color: 'hsl(6, 59%, 37%)', label:' '}, {color: 'hsl(6, 59%, 30%)', label:' '}, {color: 'hsl(283, 39%, 95%)', label:' '}, {color: 'hsl(283, 39%, 91%)', label:' '}, {color: 'hsl(283, 39%, 81%)', label:' '}, {color: 'hsl(283, 39%, 72%)', label:' '}, {color: 'hsl(283, 39%, 63%)', label:' '}, {color: 'hsl(283, 39%, 53%)', label:' '}, {color: 'hsl(283, 34%, 47%)', label:' '}, {color: 'hsl(283, 34%, 40%)', label:' '}, {color: 'hsl(283, 34%, 34%)', label:' '}, {color: 'hsl(283, 34%, 28%)', label:' '}, {color: 'hsl(282, 39%, 95%)', label:' '}, {color: 'hsl(282, 39%, 89%)', label:' '}, {color: 'hsl(282, 39%, 79%)', label:' '}, {color: 'hsl(282, 39%, 68%)', label:' '}, {color: 'hsl(282, 39%, 58%)', label:' '}, {color: 'hsl(282, 44%, 47%)', label:' '}, {color: 'hsl(282, 44%, 42%)', label:' '}, {color: 'hsl(282, 44%, 36%)', label:' '}, {color: 'hsl(282, 44%, 30%)', label:' '}, {color: 'hsl(282, 44%, 25%)', label:' '}, {color: 'hsl(204, 51%, 94%)', label:' '}, {color: 'hsl(204, 51%, 89%)', label:' '}, {color: 'hsl(204, 51%, 78%)', label:' '}, {color: 'hsl(204, 51%, 67%)', label:' '}, {color: 'hsl(204, 51%, 55%)', label:' '}, {color: 'hsl(204, 64%, 44%)', label:' '}, {color: 'hsl(204, 64%, 39%)', label:' '}, {color: 'hsl(204, 64%, 34%)', label:' '}, {color: 'hsl(204, 64%, 28%)', label:' '}, {color: 'hsl(204, 64%, 23%)', label:' '}, {color: 'hsl(204, 70%, 95%)', label:' '}, {color: 'hsl(204, 70%, 91%)', label:' '}, {color: 'hsl(204, 70%, 81%)', label:' '}, {color: 'hsl(204, 70%, 72%)', label:' '}, {color: 'hsl(204, 70%, 63%)', label:' '}, {color: 'hsl(204, 70%, 53%)', label:' '}, {color: 'hsl(204, 62%, 47%)', label:' '}, {color: 'hsl(204, 62%, 40%)', label:' '}, {color: 'hsl(204, 62%, 34%)', label:' '}, {color: 'hsl(204, 62%, 28%)', label:' '}, {color: 'hsl(168, 55%, 94%)', label:' '}, {color: 'hsl(168, 55%, 88%)', label:' '}, {color: 'hsl(168, 55%, 77%)', label:' '}, {color: 'hsl(168, 55%, 65%)', label:' '}, {color: 'hsl(168, 55%, 54%)', label:' '}, {color: 'hsl(168, 76%, 42%)', label:' '}, {color: 'hsl(168, 76%, 37%)', label:' '}, {color: 'hsl(168, 76%, 32%)', label:' '}, {color: 'hsl(168, 76%, 27%)', label:' '}, {color: 'hsl(168, 76%, 22%)', label:' '}, {color: 'hsl(168, 42%, 94%)', label:' '}, {color: 'hsl(168, 42%, 87%)', label:' '}, {color: 'hsl(168, 42%, 74%)', label:' '}, {color: 'hsl(168, 42%, 61%)', label:' '}, {color: 'hsl(168, 45%, 49%)', label:' '}, {color: 'hsl(168, 76%, 36%)', label:' '}, {color: 'hsl(168, 76%, 31%)', label:' '}, {color: 'hsl(168, 76%, 27%)', label:' '}, {color: 'hsl(168, 76%, 23%)', label:' '}, {color: 'hsl(168, 76%, 19%)', label:' '}, {color: 'hsl(145, 45%, 94%)', label:' '}, {color: 'hsl(145, 45%, 88%)', label:' '}, {color: 'hsl(145, 45%, 77%)', label:' '}, {color: 'hsl(145, 45%, 65%)', label:' '}, {color: 'hsl(145, 45%, 53%)', label:' '}, {color: 'hsl(145, 63%, 42%)', label:' '}, {color: 'hsl(145, 63%, 37%)', label:' '}, {color: 'hsl(145, 63%, 32%)', label:' '}, {color: 'hsl(145, 63%, 27%)', label:' '}, {color: 'hsl(145, 63%, 22%)', label:' '}, {color: 'hsl(145, 61%, 95%)', label:' '}, {color: 'hsl(145, 61%, 90%)', label:' '}, {color: 'hsl(145, 61%, 80%)', label:' '}, {color: 'hsl(145, 61%, 69%)', label:' '}, {color: 'hsl(145, 61%, 59%)', label:' '}, {color: 'hsl(145, 63%, 49%)', label:' '}, {color: 'hsl(145, 63%, 43%)', label:' '}, {color: 'hsl(145, 63%, 37%)', label:' '}, {color: 'hsl(145, 63%, 31%)', label:' '}, {color: 'hsl(145, 63%, 25%)', label:' '}, {color: 'hsl(48, 89%, 95%)', label:' '}, {color: 'hsl(48, 89%, 90%)', label:' '}, {color: 'hsl(48, 89%, 80%)', label:' '}, {color: 'hsl(48, 89%, 70%)', label:' '}, {color: 'hsl(48, 89%, 60%)', label:' '}, {color: 'hsl(48, 89%, 50%)', label:' '}, {color: 'hsl(48, 88%, 44%)', label:' '}, {color: 'hsl(48, 88%, 38%)', label:' '}, {color: 'hsl(48, 88%, 32%)', label:' '}, {color: 'hsl(48, 88%, 26%)', label:' '}, {color: 'hsl(37, 90%, 95%)', label:' '}, {color: 'hsl(37, 90%, 90%)', label:' '}, {color: 'hsl(37, 90%, 80%)', label:' '}, {color: 'hsl(37, 90%, 71%)', label:' '}, {color: 'hsl(37, 90%, 61%)', label:' '}, {color: 'hsl(37, 90%, 51%)', label:' '}, {color: 'hsl(37, 86%, 45%)', label:' '}, {color: 'hsl(37, 86%, 39%)', label:' '}, {color: 'hsl(37, 86%, 33%)', label:' '}, {color: 'hsl(37, 86%, 27%)', label:' '}, {color: 'hsl(28, 80%, 95%)', label:' '}, {color: 'hsl(28, 80%, 90%)', label:' '}, {color: 'hsl(28, 80%, 81%)', label:' '}, {color: 'hsl(28, 80%, 71%)', label:' '}, {color: 'hsl(28, 80%, 61%)', label:' '}, {color: 'hsl(28, 80%, 52%)', label:' '}, {color: 'hsl(28, 74%, 46%)', label:' '}, {color: 'hsl(28, 74%, 39%)', label:' '}, {color: 'hsl(28, 74%, 33%)', label:' '}, {color: 'hsl(28, 74%, 27%)', label:' '}, {color: 'hsl(24, 71%, 94%)', label:' '}, {color: 'hsl(24, 71%, 88%)', label:' '}, {color: 'hsl(24, 71%, 77%)', label:' '}, {color: 'hsl(24, 71%, 65%)', label:' '}, {color: 'hsl(24, 71%, 53%)', label:' '}, {color: 'hsl(24, 100%, 41%)', label:' '}, {color: 'hsl(24, 100%, 36%)', label:' '}, {color: 'hsl(24, 100%, 31%)', label:' '}, {color: 'hsl(24, 100%, 26%)', label:' '}, {color: 'hsl(24, 100%, 22%)', label:' '}, {color: 'hsl(192, 15%, 99%)', label:' '}, {color: 'hsl(192, 15%, 99%)', label:' '}, {color: 'hsl(192, 15%, 97%)', label:' '}, {color: 'hsl(192, 15%, 96%)', label:' '}, {color: 'hsl(192, 15%, 95%)', label:' '}, {color: 'hsl(192, 15%, 94%)', label:' '}, {color: 'hsl(192, 5%, 82%)', label:' '}, {color: 'hsl(192, 3%, 71%)', label:' '}, {color: 'hsl(192, 2%, 60%)', label:' '}, {color: 'hsl(192, 1%, 49%)', label:' '}, {color: 'hsl(204, 8%, 98%)', label:' '}, {color: 'hsl(204, 8%, 95%)', label:' '}, {color: 'hsl(204, 8%, 90%)', label:' '}, {color: 'hsl(204, 8%, 86%)', label:' '}, {color: 'hsl(204, 8%, 81%)', label:' '}, {color: 'hsl(204, 8%, 76%)', label:' '}, {color: 'hsl(204, 5%, 67%)', label:' '}, {color: 'hsl(204, 4%, 58%)', label:' '}, {color: 'hsl(204, 3%, 49%)', label:' '}, {color: 'hsl(204, 3%, 40%)', label:' '}, {color: 'hsl(184, 9%, 96%)', label:' '}, {color: 'hsl(184, 9%, 92%)', label:' '}, {color: 'hsl(184, 9%, 85%)', label:' '}, {color: 'hsl(184, 9%, 77%)', label:' '}, {color: 'hsl(184, 9%, 69%)', label:' '}, {color: 'hsl(184, 9%, 62%)', label:' '}, {color: 'hsl(184, 6%, 54%)', label:' '}, {color: 'hsl(184, 5%, 47%)', label:' '}, {color: 'hsl(184, 5%, 40%)', label:' '}, {color: 'hsl(184, 5%, 32%)', label:' '}, {color: 'hsl(184, 6%, 95%)', label:' '}, {color: 'hsl(184, 6%, 91%)', label:' '}, {color: 'hsl(184, 6%, 81%)', label:' '}, {color: 'hsl(184, 6%, 72%)', label:' '}, {color: 'hsl(184, 6%, 62%)', label:' '}, {color: 'hsl(184, 6%, 53%)', label:' '}, {color: 'hsl(184, 5%, 46%)', label:' '}, {color: 'hsl(184, 5%, 40%)', label:' '}, {color: 'hsl(184, 5%, 34%)', label:' '}, {color: 'hsl(184, 5%, 27%)', label:' '}, {color: 'hsl(210, 12%, 93%)', label:' '}, {color: 'hsl(210, 12%, 86%)', label:' '}, {color: 'hsl(210, 12%, 71%)', label:' '}, {color: 'hsl(210, 12%, 57%)', label:' '}, {color: 'hsl(210, 15%, 43%)', label:' '}, {color: 'hsl(210, 29%, 29%)', label:' '}, {color: 'hsl(210, 29%, 25%)', label:' '}, {color: 'hsl(210, 29%, 22%)', label:' '}, {color: 'hsl(210, 29%, 18%)', label:' '}, {color: 'hsl(210, 29%, 15%)', label:' '}, {color: 'hsl(210, 9%, 92%)', label:' '}, {color: 'hsl(210, 9%, 85%)', label:' '}, {color: 'hsl(210, 9%, 70%)', label:' '}, {color: 'hsl(210, 9%, 55%)', label:' '}, {color: 'hsl(210, 14%, 39%)', label:' '}, {color: 'hsl(210, 29%, 24%)', label:' '}, {color: 'hsl(210, 29%, 21%)', label:' '}, {color: 'hsl(210, 29%, 18%)', label:' '}, {color: 'hsl(210, 29%, 16%)', label:' '}, {color: 'hsl(210, 29%, 13%)', label:' '}, {color: 'hsl(219, 94%, 14%)', label:' '}
				]
			},
        })
        .then(editor => {
            //console.log('editor', editor);
            const ckf = editor.commands.get('ckfinder'),
                fileRepo = editor.plugins.get('FileRepository'),
                ntf = editor.plugins.get('Notification'),
                i18 = editor.locale.t,
                // Insert images to editor window
                insertImages = urls => {
                    const imgCmd = editor.commands.get('imageUpload');
                    if (!imgCmd.isEnabled) {
                        ntf.showWarning(i18('Could not insert image at the current position.'), {
                            title: i18('Inserting image failed'),
                            namespace: 'ckfinder'
                        });
                        return;
                    }
                    editor.execute('imageInsert', { source: urls });
                },
                // To get elFinder instance
                getfm = open => {
                    return new Promise((resolve, reject) => {
                        // Execute when the elFinder instance is created
                        const done = () => {
                            if (open) {
                                // request to open folder specify
                                if (!Object.keys(_fm.files()).length) {
                                    // when initial request
                                    _fm.one('open', () => {
                                        _fm.file(open) ? resolve(_fm) : reject(_fm, 'errFolderNotFound');
                                    });
                                } else {
                                    // elFinder has already been initialized
                                    new Promise((res, rej) => {
                                        if (_fm.file(open)) {
                                            res();
                                        } else {
                                            // To acquire target folder information
                                            _fm.request({ cmd: 'parents', target: open }).done(e => {
                                                _fm.file(open) ? res() : rej();
                                            }).fail(() => {
                                                rej();
                                            });
                                        }
                                    }).then(() => {
                                        // Open folder after folder information is acquired
                                        _fm.exec('open', open).done(() => {
                                            resolve(_fm);
                                        }).fail(err => {
                                            reject(_fm, err ? err : 'errFolderNotFound');
                                        });
                                    }).catch((err) => {
                                        reject(_fm, err ? err : 'errFolderNotFound');
                                    });
                                }
                            } else {
                                // show elFinder manager only
                                resolve(_fm);
                            }
                        };

                        // Check elFinder instance
                        if (_fm) {
                            // elFinder instance has already been created
                            done();
                        } else {
                            // To create elFinder instance
                            _fm = $('<div/>').dialogelfinder({
                                // dialog title
                                title: 'File Manager',
                                // connector URL
                                url: connectorUrl,
                                // start folder setting
                                startPathHash: open ? open : void (0),
                                // Set to do not use browser history to un-use location.hash
                                useBrowserHistory: false,
                                // Disable auto open
                                autoOpen: false,
                                // elFinder dialog width
                                width: getWidth() * 0.9,
                                height: screen.height * 0.7,
                                // set getfile command options
                                commandsOptions: {
                                    getfile: {
                                        oncomplete: 'close',
                                        multiple: true
                                    }
                                },
                                // Insert in CKEditor when choosing files
                                getFileCallback: (files, fm) => {
                                    let imgs = [];
                                    fm.getUI('cwd').trigger('unselectall');
                                    $.each(files, function (i, f) {
                                        if (f && f.mime.match(/^image\//i)) {
                                            imgs.push(fm.convAbsUrl(f.url));
                                        } else {
                                            editor.execute('link', fm.convAbsUrl(f.url));
                                        }
                                    });
                                    if (imgs.length) {
                                        insertImages(imgs);
                                    }
                                },
                                handlers : {
                                    init: function (event, elfinderInstance) {
                                        //console.log('init', elfinderInstance.roots);
                                        //console.log(event.data);
                                    },
                                    load: function (event, elfinderInstance) {
                                        //console.log('load', elfinderInstance.roots);
                                        //console.log('load', Object.values(elfinderInstance.roots)[0]);
                                        //console.log(event.data);
                                    }
                                }
                            }).elfinder('instance')
                            
                            done();
                        }
                    });
                };

            // elFinder instance
            let _fm;

            if (ckf) {
                // Take over ckfinder execute()
                ckf.execute = () => {
                    getfm().then(fm => {
                        fm.getUI().dialogelfinder('open');
                    });
                };
            }

            // Make uploader
            const uploder = function (loader) {
                //console.log('loader', loader);
                let upload = function (file, resolve, reject) {
                    //console.log('file', file);
                    //console.log('resolve', resolve);
                    //console.log('reject', reject);
                    //console.log('uploder');
                    getfm(uploadTargetHash).then(fm => {
                        let fmNode = fm.getUI();
                        fmNode.dialogelfinder('open');
                        //console.log('exec upload');
                        fm.exec('upload', { files: [file], target: uploadTargetHash }, void (0), uploadTargetHash)
                            .done(data => {
                                if (data.added && data.added.length) {
                                    fm.url(data.added[0].hash, { async: true }).done(function (url) {
                                        resolve({
                                            'default': fm.convAbsUrl(url)
                                        });
                                        fmNode.dialogelfinder('close');
                                    }).fail(function () {
                                        reject('errFileNotFound');
                                    });
                                } else {
                                    reject(fm.i18n(data.error ? data.error : 'errUpload'));
                                    fmNode.dialogelfinder('close');
                                }
                            })
                            .fail(err => {
                                const error = fm.parseError(err);
                                reject(fm.i18n(error ? (error === 'userabort' ? 'errAbort' : error) : 'errUploadNoFiles'));
                            });
                    }).catch((fm, err) => {
                        const error = fm.parseError(err);
                        reject(fm.i18n(error ? (error === 'userabort' ? 'errAbort' : error) : 'errUploadNoFiles'));
                    });
                };

                this.upload = function () {
                    return new Promise(function (resolve, reject) {
                        if (loader.file instanceof Promise || (loader.file && typeof loader.file.then === 'function')) {
                            loader.file.then(function (file) {
                                upload(file, resolve, reject);
                            });
                        } else {
                            upload(loader.file, resolve, reject);
                        }
                    });
                };
                this.abort = function () {
                    _fm && _fm.getUI().trigger('uploadabort');
                };
            };

            // Set up image uploader
            fileRepo.createUploadAdapter = loader => {
                return new uploder(loader);
            };

            /*const content = '<p>A paragraph with <a href="https://ckeditor.com">some link</a>.';
            const viewFragment = editor.data.processor.toView( content );
            const modelFragment = editor.data.toModel( viewFragment );

            editor.model.insertContent( modelFragment );*/
            window['ckEditor_' + input_id] = editor;
        })
        .catch(error => {
            console.error(error);
        });
} 
function openElFinder(inputID, ele) {
    //var pb_path = "../../";
    var pb_path = window.location.origin + "/";
    var connectorUrl = window.location.origin + '/el-finder/file-system/connector';
    var _elID = inputID;
    var onlyMimes = ele.data("only-mimes").split(",");
    onlyMimes = (onlyMimes.length > 0 && onlyMimes[0] == '') ? [] : onlyMimes;
    var hash = '';
    if (window['fm_' + _elID] != null) {
        window['fm_' + _elID].destroy();
    }
    window['fm_' + _elID] = $('<div/>').dialogelfinder({
        url: connectorUrl,
        lang: 'en',
        title: 'File Manager',
        startPathHash: '',//hash,
        useBrowserHistory: false,
        rememberLastDir: true,
        width: getWidth() * 0.9,
        height: screen.height * 0.7,
        destroyOnClose: true,
        onlyMimes: onlyMimes,
        getFileCallback: function (files, fm) {
            //console.log(fm);
            //console.log(files);
            $("#" + _elID +"_img_section").hide();
            $("#" + _elID +"_thumbnail").prop("src", "").hide();
            $("#" + _elID + "_thumbnail2").prop("src", "").hide();

            if (files.length > 0) {
                files = files[0];
            }

            var new_files_path = files.path.replace(/\\/g, "/").replace(admin_fm_path1, admin_fm_path2);
            if ((files.mime).indexOf("image/") > -1)
            { 
                $("input#" + _elID +"").val(new_files_path);
                $("#" + _elID +"_thumbnail").prop("src", pb_path + new_files_path).show();
                $("#" + _elID + "_del").show(); 
                $("#" + _elID + "_img_section").slideDown();
            }
            else
            {
                $("input#" + _elID + "").val(new_files_path);
                if ((files.mime).indexOf("video/mp4") > -1)
                {
                    $("#" + _elID +"_thumbnail2").prop("src", pb_path + new_files_path).show();
                    $("#" + _elID + "_img_section").slideDown();
                }
                $("#" + _elID +"_del").show();
            }
			
			var _elID_arr = _elID.split("_");
			var last_num = parseInt(_elID_arr[_elID_arr.length-1]);
			last_num = last_num+1;
			//alert('span_'+_elID_arr[_elID_arr.length-2]+"_"+last_num);
			try 
			{
				document.getElementById('span_'+_elID_arr[_elID_arr.length-2]+"_"+last_num).style.display = '';
			}
			catch{}
			 
        },
        commandsOptions: {
            getfile: {
                oncomplete: 'close',
                folders: false
            },
            edit: {
                extraOptions: {
                    // set API key to enable Creative Cloud image editor
                    // see https://console.adobe.io/
                    creativeCloudApiKey: '',
                    // browsing manager URL for CKEditor, TinyMCE
                    // uses self location with the empty value
                    managerUrl: ''
                }
            }
            , quicklook: {
                // to enable CAD-Files and 3D-Models preview with sharecad.org
                sharecadMimes: ['image/vnd.dwg', 'image/vnd.dxf', 'model/vnd.dwf', 'application/vnd.hp-hpgl', 'application/plt', 'application/step', 'model/iges', 'application/vnd.ms-pki.stl', 'application/sat', 'image/cgm', 'application/x-msmetafile'],
                // to enable preview with Google Docs Viewer
                googleDocsMimes: ['application/pdf', 'image/tiff', 'application/vnd.ms-office', 'application/msword', 'application/vnd.ms-word', 'application/vnd.ms-excel', 'application/vnd.ms-powerpoint', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document', 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet', 'application/vnd.openxmlformats-officedocument.presentationml.presentation', 'application/postscript', 'application/rtf'],
                // to enable preview with Microsoft Office Online Viewer
                // these MIME types override "googleDocsMimes"
                officeOnlineMimes: ['application/vnd.ms-office', 'application/msword', 'application/vnd.ms-word', 'application/vnd.ms-excel', 'application/vnd.ms-powerpoint', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document', 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet', 'application/vnd.openxmlformats-officedocument.presentationml.presentation', 'application/vnd.oasis.opendocument.text', 'application/vnd.oasis.opendocument.spreadsheet', 'application/vnd.oasis.opendocument.presentation']
            }
        }
    }).dialogelfinder('instance');
    
}

function openElFinderFM(inputID) {
    //var pb_path = "../../";
    var pb_path = window.location.origin + "/";
    var connectorUrl = window.location.origin + '/el-finder/file-system/connector';
    var _elID = inputID; 
    var hash = '';
    if (window['fm_' + _elID] != null) {
        window['fm_' + _elID].destroy();
    }
    window['fm_' + _elID] = $('<div/>').dialogelfinder({
        url: connectorUrl,
        lang: 'en',
        title: 'File Manager',
        startPathHash: '',//hash,
        useBrowserHistory: false,
        rememberLastDir: true,
        width: getWidth() * 0.95,
        height: screen.height * 0.75,
        destroyOnClose: true, 
        getFileCallback: function (files, fm) {
            //console.log(fm);
            console.log(files);
            $("#" + _elID + "_img_section").hide();
            $("#" + _elID + "_thumbnail").prop("src", "").hide();
            $("#" + _elID + "_thumbnail2").prop("src", "").hide();

            if (files.length > 0) {
                files = files[0];
            }
            var new_files_path = files.path.replace(/\\/g, "/").replace(admin_fm_path1, admin_fm_path2);
            if ((files.mime).indexOf("image/") > -1)
            {
                //console.log((files.path).replace(/\\/g, "/"));
                $("input#" + _elID + "").val(new_files_path);
                $("#" + _elID + "_thumbnail").prop("src", pb_path + new_files_path).show();
                $("#" + _elID + "_del").show();
                $("#" + _elID + "_img_section").slideDown();
            }
            else
            {
                $("input#" + _elID + "").val(new_files_path);
                if ((files.mime).indexOf("video/mp4") > -1)
                {
                    $("#" + _elID + "_thumbnail2").prop("src", pb_path + new_files_path).show();
                    $("#" + _elID + "_img_section").slideDown();
                }
                $("#" + _elID + "_del").show();
            }
        },
        commandsOptions: {
            getfile: {
                oncomplete: 'close',
                folders: false
            },
            edit: {
                extraOptions: {
                    // set API key to enable Creative Cloud image editor
                    // see https://console.adobe.io/
                    creativeCloudApiKey: '',
                    // browsing manager URL for CKEditor, TinyMCE
                    // uses self location with the empty value
                    managerUrl: ''
                }
            }
            , quicklook: {
                // to enable CAD-Files and 3D-Models preview with sharecad.org
                sharecadMimes: ['image/vnd.dwg', 'image/vnd.dxf', 'model/vnd.dwf', 'application/vnd.hp-hpgl', 'application/plt', 'application/step', 'model/iges', 'application/vnd.ms-pki.stl', 'application/sat', 'image/cgm', 'application/x-msmetafile'],
                // to enable preview with Google Docs Viewer
                googleDocsMimes: ['application/pdf', 'image/tiff', 'application/vnd.ms-office', 'application/msword', 'application/vnd.ms-word', 'application/vnd.ms-excel', 'application/vnd.ms-powerpoint', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document', 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet', 'application/vnd.openxmlformats-officedocument.presentationml.presentation', 'application/postscript', 'application/rtf'],
                // to enable preview with Microsoft Office Online Viewer
                // these MIME types override "googleDocsMimes"
                officeOnlineMimes: ['application/vnd.ms-office', 'application/msword', 'application/vnd.ms-word', 'application/vnd.ms-excel', 'application/vnd.ms-powerpoint', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document', 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet', 'application/vnd.openxmlformats-officedocument.presentationml.presentation', 'application/vnd.oasis.opendocument.text', 'application/vnd.oasis.opendocument.spreadsheet', 'application/vnd.oasis.opendocument.presentation']
            }
        }
    }).dialogelfinder('instance');

}

function removeFileFM(inputID, ele) {
    Swal.fire({
        title: 'ลบออกจากเนื้อหา ?',
        text: "ไฟล์จะยังอยู่ใน File maneger, ต้องการลบออกออกจากเนื้อหาหรือไม่ ?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#0047B6',
        cancelButtonColor: '#BC1320',
        confirmButtonText: 'ใช่',
        cancelButtonText: 'ไม่ใช่',
        showClass: {
            popup: 'animate__animated animate__bounceIn'
        },
        hideClass: {
            popup: 'animate__animated animate__bounceOut'
        }
    }).then((result) => {
        if (result.isConfirmed) {

            $("#" + inputID + "_thumbnail").prop('src', '').hide();
            ele.hide();
            $("input#" + inputID).val('');
            $("#" + inputID + "_img_section").slideUp();

        }
    });
}

function checkFileFM(inputID) {
	var pb_path = window.location.origin + "/";
    var _fileFM = $("#" + inputID).val();
    console.log("_fileFM", _fileFM);
    if (_fileFM != "") {

        var imgExt = ['png', 'tif', 'tiff', 'wbmp', 'ico', 'jng', 'bmp', 'svg', 'webp', 'jpg', 'jpeg','svg'];
        var videoExt = ['3gpp', '3gp', 'mpeg', 'mpg', 'mov', 'flv', 'mng', 'asx', 'asf', 'wmv', 'avi', 'm4v', 'mp4'];

        $("#" + inputID + "_del").show();

        var file_ar = (_fileFM).split('.')
        var ext = file_ar[file_ar.length - 1];
        if (ext != "") {

            ext = ext.toLowerCase();

            if (imgExt.indexOf(ext) > -1) {
                $("#" + inputID + "_thumbnail").prop("src", pb_path + _fileFM).show();
                $("#" + inputID + "_img_section").show();
            }
            else if (videoExt.indexOf(ext) > -1) {
                $("#" + inputID + "_thumbnail2").prop("src", pb_path + _fileFM).show();
                $("#" + inputID + "_img_section").show();
            }
        }
    }

}

function checkAccess(ele) {
    //console.log('name', ele.prop('name'));
    //console.log('checked', ele.is(':checked'));
    const _name = ele.prop('name');
    const _checked = ele.is(':checked');
    if (_name != null && _name != '') {
        $("form#fm_cms :input[type='checkbox'][name^=" + _name + "]:not(:disabled)").prop("checked", _checked);
    }
}

function checkAccess2(ele) {
    //console.log('name', ele.prop('name'));
    //console.log('checked', ele.is(':checked'));
    const _name = ele.prop('name');
    const _checked = ele.is(':checked');
    const _mod_name = _name.split('_can_');
    if (_mod_name.length > 0) {
        //console.log('mod_name', _mod_name);

        if (_checked && $("form#fm_cms :input[type='checkbox'][name=" + _mod_name[0] + "]").is(':checked') == false) {
            $("form#fm_cms :input[type='checkbox'][name=" + _mod_name[0] + "]").prop("checked", true);
        }
    }
}

function checkAccessAll(ele) {
    //console.log('checked', ele.is(':checked'));
    const _checked = ele.is(':checked');
    $("table#table_access :input[type='checkbox']:not(:disabled)").prop("checked", _checked);
}

function checkSort(ele, field) {

    let url = updateQueryStringParameter(window.location.href, 'orderby', 'sort');
    url = updateQueryStringParameter(url, 'sort', 'asc');

    if (field != "" && $("#id_" + field).length > 0) {
        var group_val = $("#id_" + field).val();
        if (group_val == null || group_val == "") {
            group_val = $("#id_" + field).find('option[value!=""]:eq(0)').attr("value");
        }
        url = updateQueryStringParameter(url, field, group_val);

        $('form#formSearch :input').not("#id_" + field).each(function (e) {
            if ($(this).prop('name') != '') {
                url = updateQueryStringParameter(url, $(this).prop('name'), '');
            }
        });
    } else {
        $('form#formSearch :input').each(function (e) {
            if ($(this).prop('name') != '') {
                url = updateQueryStringParameter(url, $(this).prop('name'), '');
            }
        });
    }

    window.location.href = url;
}

// jQuery formBuilder 3.8.3 calls .trim() on field/option `value`, assuming it is a string.
// A numeric value (e.g. from imported data or a custom input-set) throws "t.trim is not a function"
// and breaks the whole builder. Coerce every `value` to a string before loading data into formBuilder.
function fbSanitizeFields(fields) {
    if (!Array.isArray(fields)) return fields;
    fields.forEach(function (f) {
        if (f == null || typeof f !== 'object') return;
        if ('value' in f && f.value != null && typeof f.value !== 'string') {
            f.value = String(f.value);
        }
        if (Array.isArray(f.values)) {
            f.values.forEach(function (o) {
                if (o != null && typeof o === 'object' && 'value' in o && o.value != null && typeof o.value !== 'string') {
                    o.value = String(o.value);
                }
            });
        }
    });
    return fields;
}

// Normalize an entire formBuilder options object in place: its saved formData (a JSON string or
// array) and every custom inputSet's fields. Safe to call even when those are absent.
function fbSanitizeOptions(options) {
    if (options == null) return options;
    if (options.formData) {
        try {
            var parsed = (typeof options.formData === 'string') ? JSON.parse(options.formData) : options.formData;
            options.formData = JSON.stringify(fbSanitizeFields(parsed));
        } catch (e) { /* leave formData untouched if it is not parseable */ }
    }
    if (Array.isArray(options.inputSets)) {
        options.inputSets.forEach(function (s) {
            if (s && Array.isArray(s.fields)) fbSanitizeFields(s.fields);
        });
    }
    return options;
}

function checkLockedRow(modConfig, jsonRow) {
    if (
        modConfig != null &&
        jsonRow != null &&
        modConfig.CanApprove != null &&
        modConfig.CanApprove == true &&
        jsonRow.length > 0 &&
        jsonRow[0]["pb_status"] != null && 
        parseInt(jsonRow[0]["pb_status"]) == 0
    ) {
        console.log(jsonRow[0]);
        lockForm();
    }
}

function lockForm(lockFlags = true) {
    $("#fm_cms :input").each(function () {
        var input_name = $(this).prop("name");
        var input_onclick = $(this).prop("onclick");
        if ((input_name + "" != "" && input_name != "preview_content") || input_onclick != null) {
            $(this).prop("disabled", lockFlags);
            if (window['ckEditor_id_' + input_name] != null) {
                window['ckEditor_id_' + input_name].isReadOnly = lockFlags;
            }
        }
    });
    $("#section_btn :input").prop("disabled", lockFlags);
    if (lockFlags) {
        $('<div class="alert alert-primary" id="lock_row_alert" role="alert">🔒 รายการรอการอนุมัติ</div>').insertBefore("#fm_cms");
    } else {
        $("#lock_row_alert").remove();
    }
}

function previewModal(url, lang, size) {
    var rowLocked = false;
    if (
        typeof (modConfig) != 'undefined' &&
        modConfig != null &&
        jsonRow != null &&
        modConfig.CanApprove != null &&
        modConfig.CanApprove == true &&
        jsonRow.length > 0 &&
        jsonRow[0]["pb_status"] != null &&
        parseInt(jsonRow[0]["pb_status"]) == 0
     ) {
        rowLocked = true;
    }

    $("#previewModal .modal-dialog").removeClass("modal-xl").removeClass("modal-fullscreen");
    $("#previewModal .modal-body").empty();
    //----- default parameter -----//
    if (size == null) {
        size = "";
    } else {
        if (size == "xl") {
            size = "modal-xl";
        } else if (size == "full") {
            size = "modal-fullscreen";
        } else {
            size = "";
        }
    }
    lang = (lang == null) ? "th" : lang;
    if ($("#fm_cms :input[name='_preview']").length == 0) {
        $("<input type='hidden' name='_preview' value='1'>").appendTo($("#fm_cms"));
    }
    //--------------------------------------//

    $("#previewModal .modal-dialog").addClass(size);
    $("#previewModal .modal-body").append($("<iframe name='preview_iframe' id='preview_iframe' frameborder='0' vspace='0' hspace='0' style='width:100%; height:98%' src='about:blank'></iframe>"));
    $("#previewModal").modal('show');

    //----- set lang -----//
    if (url.indexOf("?") > -1) {
        url += "&lang=" + lang
    } else {
        url += "?lang=" + lang
    }



    //----- temp -----//
    let _method = document.getElementById("fm_cms").method;
    let _target = document.getElementById("fm_cms").target;
    let _action = document.getElementById("fm_cms").action;

    if (rowLocked) { lockForm(false); }
    document.getElementById("fm_cms").method = "post";
    document.getElementById("fm_cms").target = "preview_iframe";
    document.getElementById("fm_cms").action = url;
    document.getElementById("fm_cms").submit();

    setTimeout(function () {
        document.getElementById("fm_cms").method = _method;
        document.getElementById("fm_cms").target = _target;
        document.getElementById("fm_cms").action = _action;
        if (rowLocked) { lockForm(); }
    }, 200);
}

// ───── พรีวิวหน้าเว็บฝั่ง front-end (ปุ่ม [Preview] ของรายการที่รออนุมัติในหน้า list) ─────
// baseUrl = URL ของหน้าพรีวิวฝั่ง front-end เช่น https://localhost:7169/_preview/announce-pro/47
// เปิด modal เต็มจอแล้วโหลด URL ลง iframe (ค่าเริ่มต้นภาษาไทย, สลับได้จากปุ่มบน header)
function openFrontPreview(baseUrl) {
    var $modal = $("#frontPreviewModal");
    if ($modal.length == 0 || !baseUrl) { return; }

    $modal.data("preview-base", baseUrl);

    // reset ปุ่มเลือกภาษาให้กลับมาที่ "ไทย" ทุกครั้งที่เปิดรายการใหม่
    $modal.find("[data-preview-lang]").removeClass("active");
    $modal.find("[data-preview-lang='th']").addClass("active");

    loadFrontPreview("th");
    $modal.modal("show");
}

// โหลด (หรือโหลดซ้ำ) หน้าพรีวิวตามภาษาที่เลือก
var _frontPreviewTimer = null;
function loadFrontPreview(lang) {
    var $modal = $("#frontPreviewModal");
    var baseUrl = $modal.data("preview-base");
    if (!baseUrl) { return; }

    lang = (lang == "en") ? "en" : "th";
    var url = baseUrl + (baseUrl.indexOf("?") > -1 ? "&" : "?") + "lang=" + lang;

    $("#frontPreviewLoading").css("display", "flex");
    $("#frontPreviewFrame").attr("src", url);

    // fallback: บางหน้ามี resource ที่ค้าง (websocket/long-poll ตอน dev) ทำให้ event load ไม่ยิงสักที
    // ถ้าเกิน 5 วิ ให้เอา spinner ออกเอง — เนื้อหาหลักแสดงครบไปก่อนหน้านั้นแล้ว
    if (_frontPreviewTimer != null) { clearTimeout(_frontPreviewTimer); }
    _frontPreviewTimer = setTimeout(hideFrontPreviewLoading, 5000);
}

function hideFrontPreviewLoading() {
    if (_frontPreviewTimer != null) { clearTimeout(_frontPreviewTimer); _frontPreviewTimer = null; }
    $("#frontPreviewLoading").css("display", "none");
}

$(function () {
    var $modal = $("#frontPreviewModal");
    if ($modal.length == 0) { return; }

    // ปุ่มสลับภาษา
    $modal.on("click", "[data-preview-lang]", function () {
        var $btn = $(this);
        if ($btn.hasClass("active")) { return; }
        $modal.find("[data-preview-lang]").removeClass("active");
        $btn.addClass("active");
        loadFrontPreview($btn.attr("data-preview-lang"));
    });

    // ซ่อน spinner เมื่อ iframe โหลดเสร็จ (ข้ามครั้งแรกที่ src ยังเป็น about:blank)
    $("#frontPreviewFrame").on("load", function () {
        if ((this.getAttribute("src") || "about:blank") != "about:blank") {
            hideFrontPreviewLoading();
        }
    });

    // ปิด modal (ปุ่ม X / กด Esc) -> ล้าง src กันไม่ให้หน้า front-end ทำงานค้างอยู่เบื้องหลัง
    $modal.on("hidden.bs.modal", function () {
        if (_frontPreviewTimer != null) { clearTimeout(_frontPreviewTimer); _frontPreviewTimer = null; }
        $("#frontPreviewFrame").attr("src", "about:blank");
        $("#frontPreviewLoading").css("display", "flex");
    });
});

function getCustomAlertTxtUser(mod_name, id) {
    if (typeof(jsUser) != 'undefined' && jsUser != null && jsUser.length > 0 && mod_name != "" && id != "") {

        let arID = id.split(',');
        for(var i=0; i<arID.length;i++) arID[i] = parseInt(arID[i], 10);
        console.log('mod_name', mod_name);
        console.log('id', arID);
        
        let field = "";
        if (mod_name == "AdminAccess") {
            field = "access_id";
        }

        console.log('field', field);
        let findUser = jsUser.filter(function (item) {
            return arID.indexOf(item[field]) > -1;
        });
        console.log('findUser', findUser);
        if (findUser.length > 0) {
            let txt = "ข้อมูลที่ต้องการลบ ถูกใช้ใน ผู้ดูแลระบบ<hr/>";
            let txtList = "<ul class='text-danger'>";
            $.each(findUser, function (i, v) {
                if (i >= 9) {
                    return;
                }
                txtList += "<li class='text-start fw-light'>" + v['name'] + " " + v['surname'] + "</li>";
            });
            if (findUser.length > 10) {
                txtList += "<li class='text-start fw-light'>และอื่นๆ</li>";
            }
            txtList += "</ul>";

            return "<span class='text-danger fw-bolder'>" + txt + "</span>" + txtList;
        } else { 
            return "";
        }
    } else {
        return "";
    }
}

function getCustomAlertTxt(mod_name, id) {
    if (typeof(jsPosition) != 'undefined' && jsPosition != null && jsPosition.length > 0 && mod_name != "" && id != "") {

        let arID = id.split(',');
        for(var i=0; i<arID.length;i++) arID[i] = parseInt(arID[i], 10);
        console.log('mod_name', mod_name);
        console.log('id', arID);
        
        let field = "";
        let pb_field = "";
        if (mod_name == "JobArea") {
            field = "job_area_id";
            pb_field = "pb_" + field;
        } else if (mod_name == "JobType") {
            field = "job_type_id";
            pb_field = "pb_" + field;
        } else if (mod_name == "JobLocation") {
            field = "job_location_id";
            pb_field = "pb_" + field;
        }
        console.log('field', field);
        console.log('pb_field', pb_field);
        let findPosition = jsPosition.filter(function (item) {
            return arID.indexOf(item[field]) > -1 || arID.indexOf(item[pb_field]) > -1;
        });
        console.log('findPosition', findPosition);
        if (findPosition.length > 0) {
            let txt = "ข้อมูลที่ต้องการลบ ถูกใช้ในตำแหน่งงาน<hr/>";
            let txtList = "<ul class='text-danger'>";
            $.each(findPosition, function (i, v) {
                txtList += "<li class='text-start fw-light'>" + v['title'] + "</li>";
            });
            txtList += "</ul>";

            return "<span class='text-danger fw-bolder'>" + txt + "</span>" + txtList;
        } else { 
            return "";
        }
    } else {
        return "";
    }
}

function getstartPathHash(volumeId, path) {
    var hash = volumeId + btoa(path).replace(/\+/g, '-').replace(/\//g, '_').replace(/=/g, '.').replace(/\.+$/, '');
    return hash
}

function getPath(volumeId, pathHash) {
    // Remove the volumeId from the pathHash
    pathHash = pathHash.replace(volumeId, '');

    // Decode the pathHash by reversing the URL-safe replacements
    const decodedPath = atob(
        pathHash
            .replace(/-/g, '+')
            .replace(/_/g, '/')
            .replace(/\./g, '=')
    );

    const pathWithoutVolumeId = decodedPath;

    return pathWithoutVolumeId;
}

function elfinderInitHashRootPath() {
    if ($('meta[name=elfinder-roots-hash]').prop('content') == '') {
        elfinderGetHashRootPath(function (hash) {
            console.log('elfinderGetHashRootPath', hash);
            $('meta[name=elfinder-roots-hash]').prop('content', hash);
        });
    }
}

function elfinderGetHashRootPath(hash) {
    var connectorUrl = window.location.origin + '/el-finder/file-system/connector';
    var qCommand = '?cmd=open&init=1';
    $.getJSON({
        url: connectorUrl + qCommand,
        type: 'get',
        cache: false,
        success: function (res) {
            //console.log(typeof (res.cwd.hash));
            if (typeof (res.cwd.hash) == 'string') {
                elfinderSetHashRootPath(res.cwd.hash);
                hash(res.cwd.hash);
            } else {
                hash('');
            }
        },
        error: function(res){
            hash('');
        }
    });
}

function elfinderSetHashRootPath(hash) {
    var connectorUrl = window.location.origin + '/el-finder/file-system/set-session-hash-path?hash=' + hash;
    $.ajax({
        url: connectorUrl,
        type: 'get',
        cache: false,
        success: function (res) {
            console.log('elfinderSetHashRootPath(session)', res);
        },
        error: function (res) {
            
        }
    });
}

function replace_for_url(this_val)
{
	this_val = this_val.replaceAll(' ', '-');
	this_val = this_val.replaceAll('!', '');
	this_val = this_val.replaceAll('#', '');
	this_val = this_val.replaceAll('$', '');
	this_val = this_val.replaceAll('?', '');
	this_val = this_val.replaceAll('&', '');
	this_val = this_val.replaceAll('%', '');
	this_val = this_val.replaceAll("'", '');
	this_val = this_val.replaceAll('"', '');
	this_val = this_val.replaceAll('(', '');
	this_val = this_val.replaceAll(')', '');
	this_val = this_val.replaceAll('*', '');
	this_val = this_val.replaceAll('+', '');
	this_val = this_val.replaceAll(',', '');
	//this_val = this_val.replaceAll('/', '');
	this_val = this_val.replaceAll('\\', '');
	this_val = this_val.replaceAll(':', '');
	this_val = this_val.replaceAll(';', '');
	this_val = this_val.replaceAll('=', '');
	this_val = this_val.replaceAll('<', '');
	this_val = this_val.replaceAll('>', '');
	this_val = this_val.replaceAll('[', '');
	this_val = this_val.replaceAll(']', '');
	this_val = this_val.replaceAll('^', '');
	this_val = this_val.replaceAll('~', '');
	this_val = this_val.replaceAll('{', '');
	this_val = this_val.replaceAll('}', '');
	
	return this_val;
}

$(document).ready(function () {
    elfinderInitHashRootPath();

});