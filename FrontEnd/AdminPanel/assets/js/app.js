/*
Template Name: Skote - Responsive Bootstrap 4 Admin Dashboard
Author: Themesbrand
Version: 2.0
Website: https://themesbrand.com/
Contact: themesbrand@gmail.com
File: Main Js File
*/
'use strict';

var language = "";

function setLanguage(lang) {
    if (lang == 'en') {
        $("#bootstrap-style").attr('href', '/assets/css/bootstrap.min.css');
        $("#app-style").attr('href', '/assets/css/app.min.css');
        sessionStorage.setItem("is_visited", "light-mode-switch");
    } else if (lang == 'ar') {
        $("#bootstrap-style").attr('href', '/assets/css/bootstrap.min.css');
        $("#app-style").attr('href', '/assets/css/app-rtl.min.css');
        sessionStorage.setItem("is_visited", "rtl-mode-switch");
    }
    localStorage.setItem('lang', lang);
    language = localStorage.getItem('lang');
    getLanguage();
}

function getLanguage() {
    if (language == null || language == "")
        try {
            let cook = document.cookie.split(';');
            cook.forEach(i => {
                let keyandval = i.split('=');
                if (keyandval[0].trim().replace(' ', '') == "lang") {
                    language = i.split("lang=")[1].replace(' ', '')
                }
            })
        } catch (e) {
            location.href = "/Home/ChangeLanguage?lang=ar"
        }
    if (language != "" && language != null) {
        $.getJSON('/assets/lang/' + language + '.json', function (lang) {
            $('html').attr('lang', language);
            $.each(lang, function (index, val) {

                (index === 'head') ? $(document).attr("title", val['title']) : false;
                $("[key='" + index + "']").text(val);
            });
        })
    }
    else
        location.href = "/Home/ChangeLanguage?lang=ar"

};

function initLeftMenuCollapse() {
    try {
        $('#vertical-menu-btn').on('click', function (event) {
            event.preventDefault();
            $('body').toggleClass('sidebar-enable');
            if ($(window).width() >= 992) {
                $('body').toggleClass('vertical-collpsed');
            } else {
                $('body').removeClass('vertical-collpsed');
            }
        });
    } catch (e) {
        console.log(e)
    }
}

function initActiveMenu() {
    try {
        // === following js will activate the menu in left side bar based on url ====
        $("#sidebar-menu a").each(function (e) {
            //console.log(window.location.href.split(/[?#]/)[0])
            var pageUrl = window.location.pathname.split('/')[1];
            //console.log(this.id, pageUrl)
            var ElementRefs = this.id.split(',');
            if (ElementRefs.length > 0)
                (ElementRefs).forEach(i => {
                    //console.log(i)
                    if (i == pageUrl) {
                        $(this).addClass("active");
                        $(this).parent().addClass("mm-active"); // add active to li of the current link
                        $(this).parent().parent().addClass("mm-show");
                        $(this).parent().parent().prev().addClass("mm-active"); // add active class to an anchor
                        $(this).parent().parent().parent().addClass("mm-active");
                        $(this).parent().parent().parent().parent().addClass("mm-show"); // add active to li of the current link
                        $(this).parent().parent().parent().parent().parent().addClass("mm-active");
                    }
                })

        });
    } catch (e) {
        console.log(e)

    }

}

function initMenuItemScroll() {
    try {
        $(document).ready(function () {
            if ($("#sidebar-menu").length > 0 && $("#sidebar-menu .mm-active .active").length > 0) {
                var activeMenu = $("#sidebar-menu .mm-active .active").offset().top;
                if (activeMenu > 300) {
                    activeMenu = activeMenu - 300;
                    $(".simplebar-content-wrapper").animate({ scrollTop: activeMenu }, "slow");
                }
            }
        });
    } catch (e) {
        console.log(e)

    }
    // focus active menu in left sidebar

}

function initHoriMenuActive() {
    try {
        $(".navbar-nav a").each(function () {
            var pageUrl = window.location.href.split(/[?#]/)[0];
            if (this.href == pageUrl) {
                $(this).addClass("active");
                $(this).parent().addClass("active");
                $(this).parent().parent().addClass("active");
                $(this).parent().parent().parent().addClass("active");
                $(this).parent().parent().parent().parent().addClass("active");
                $(this).parent().parent().parent().parent().parent().addClass("active");
            }
        });
    } catch (e) {
        console.log(e)

    }

}

function initFullScreen() {
    try {
        $('[data-toggle="fullscreen"]').on("click", function (e) {
            e.preventDefault();
            $('body').toggleClass('fullscreen-enable');
            if (!document.fullscreenElement && /* alternative standard method */ !document.mozFullScreenElement && !document.webkitFullscreenElement) {  // current working methods
                if (document.documentElement.requestFullscreen) {
                    document.documentElement.requestFullscreen();
                } else if (document.documentElement.mozRequestFullScreen) {
                    document.documentElement.mozRequestFullScreen();
                } else if (document.documentElement.webkitRequestFullscreen) {
                    document.documentElement.webkitRequestFullscreen(Element.ALLOW_KEYBOARD_INPUT);
                }
            } else {
                if (document.cancelFullScreen) {
                    document.cancelFullScreen();
                } else if (document.mozCancelFullScreen) {
                    document.mozCancelFullScreen();
                } else if (document.webkitCancelFullScreen) {
                    document.webkitCancelFullScreen();
                }
            }
        });
        document.addEventListener('fullscreenchange', exitHandler);
        document.addEventListener("webkitfullscreenchange", exitHandler);
        document.addEventListener("mozfullscreenchange", exitHandler);
        function exitHandler() {
            if (!document.webkitIsFullScreen && !document.mozFullScreen && !document.msFullscreenElement) {
                $('body').removeClass('fullscreen-enable');
            }
        }

    } catch (e) {
        console.log(e)

    }

}

function initRightSidebar() {
    try {
        $('.right-bar-toggle').on('click', function (e) {
            $('body').toggleClass('right-bar-enabled');
        });

        $(document).on('click', 'body', function (e) {
            if ($(e.target).closest('.right-bar-toggle, .right-bar').length > 0) {
                return;
            }

            $('body').removeClass('right-bar-enabled');
            return;
        });
    } catch (e) {
        console.log(e)

    }
    // right side-bar toggle

}

function initComponents() {
    try {
        $(function () {
            try {
                $('[data-toggle="tooltip"]').tooltip()
            } catch (e) {
                console.log(e)

            }
        })

        $(function () {
            try {
                $('[data-toggle="popover"]').popover()
            } catch (e) {
                console.log(e)

            }
        })
    } catch (e) {
        console.log(e)

    }

}

function initPreloader() {
    $(window).on('load', function () {
        $('#status').fadeOut();
        $('#preloader').delay(350).fadeOut('slow');
    });

}

function initSettings() {
    if (window.sessionStorage) {
        var alreadyVisited = sessionStorage.getItem("is_visited");
        if (!alreadyVisited) {
            sessionStorage.setItem("is_visited", "light-mode-switch");
        } else {
            $(".right-bar input:checkbox").prop('checked', false);
            $("#" + alreadyVisited).prop('checked', true);
            //updateThemeSetting(alreadyVisited);
        }
    }

    $("#light-mode-switch, #dark-mode-switch, #rtl-mode-switch").on("change", function (e) {
        //updateThemeSetting(e.target.id);
    });

}

function initLanguage() {
    // Auto Loader

    setLanguage(language == null ? "ar" : language);
    //$('.language').on('click', function (e) {
    //	setLanguage($(this).attr('data-lang'));
    //});
}
var User = "";
$(document).ajaxSend(function (event, jqxhr, settings) {
    if (settings.url != "/assets/lang/ar.json")
        document.getElementById('LoadingDiv').style.display = 'flex';
});
$(document).ajaxComplete(function () {
    setTimeout(function () {
        document.getElementById('LoadingDiv').style.display = 'none';
    }, 1000)
});
(function ($) {

    init();

    function init() {
        try {
            let cook = document.cookie.split(';');
            cook.forEach(i => {
                let keyandval = i.split('=');
                if (keyandval[0].trim().replace(' ', '') == "lang") {
                    language = i.split("lang=")[1].replace(' ', '')
                }
                else if (keyandval[0].trim().replace(' ', '') == "u") {
                    User = i.split("u=")[1].replace(' ', '')
                    if (User == "") {
                        document.cookie = "";
                        location.href = "/"
                    }
                }
            })
        } catch (e) {
            location.href = "/Home/ChangeLanguage?lang=ar"
        }
        $('[required]').attr('oninvalid', `this.setCustomValidity('${language == "ar" ? "هذا الحقل الزامي" : "This Field Required"}')`).attr("oninput", "setCustomValidity('')");

        initLanguage();
        initLeftMenuCollapse();
        initActiveMenu();
        initMenuItemScroll();
        initHoriMenuActive();
        initFullScreen();
        //initRightSidebar();
        try {

            $("#side-menu").metisMenu();
        } catch (e) {
            console.log(e)

        }
        //initDropdownMenu();
        initComponents();
        initSettings();
        initPreloader();
        try {

            Waves.init();
        } catch (e) {
            console.log(e)

        }
        $('.dropdown-menu a.dropdown-toggle').on('click', function (e) {
            if (!$(this).next().hasClass('show')) {
                $(this).parents('.dropdown-menu').first().find('.show').removeClass("show");
            }
            var $subMenu = $(this).next(".dropdown-menu");
            $subMenu.toggleClass('show');

            return false;
        });
        $.ajax({
            url: "/Home/GetOrderCount?ID=" + User, method: "Get", success: function (x) {
                //console.log(x)
                let data = JSON.parse(x)
                //console.log(x, data["result"])
                var count = document.getElementById('NotficationsCount')
                if (data["result"] != 0) {
                    count.innerText = data["result"];
                    count.style.display = "inline-flex";
                }
                else
                    count.style.display = "none";
                WebSocketTest();
                setTimeout(function () {
                    document.getElementById('LoadingDiv').style.display = 'none';
                }, 1000)
            }
        })

        function WebSocketTest() {
            if ("WebSocket" in window) {

                //var ws = new WebSocket("wss://be-mustafid.iau.edu.sa/WSHandler.ashx?Name=" + User);
                //var ws = new WebSocket("wss://localhost:44344/WSHandler.ashx?Name=" + User);
                var ws = new WebSocket("wss://mm.iau-bsc.com/WSHandler.ashx?Name=" + User);

                ws.onopen = function () {
                };

                ws.onmessage = function (evt) {
                    if (evt.data == "Out") {
                        location.href = "";
                        return;
                    }
                    var sound;
                    sound = new Howl({
                        src: ['data:audio/mp3;base64,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']
                    });
                    sound.play();
                    var count = document.getElementById('NotficationsCount')
                    count.innerText = parseInt(count.innerText == "" ? "0" : count.innerText) + 1
                    count.style.display = "inline-flex";
                    if (location.pathname == '/Email') {
                        var received_msg = JSON.parse(evt.data);
                        if (document.querySelector('.dataTables_empty') != null)
                            document.querySelector('.dataTables_empty').style.display = "none";

                        if (window.location.pathname.split('/')[1] == "Email") {
                            var mailList = document.getElementById('MailList')
                            var html = mailList.innerHTML;
                            var dateOptions = { month: '2-digit', day: '2-digit' };
                            var timeOptions = { hour12: false, hour: '2-digit', minute: '2-digit' };
                            $('.dataTables_empty').css({ 'display': 'none' });
                            mailList.innerHTML = `
						<tr onclick="Preview('/Email/Preview/${received_msg["Request_Data_ID"]}')" class="noreaded">
								<td class="sorting_1 dtr-control"><input type="checkbox" class="selectbox-request" id="${received_msg["Request_Data_ID"]}"></td>
								<td>${received_msg["Request_Data_ID"]}</td>
								<td>${(language == "ar" ? received_msg["Service_Type"]["Service_Type_Name_AR"] : received_msg["Service_Type"]["Service_Type_Name_EN"])}</td>
								<td>${(language == "ar" ? received_msg["Request_Type"]["Request_Type_Name_AR"] : received_msg["Request_Type"]["Request_Type_Name_EN"])}</td>
								<td><span>${document.querySelector(`option[key='${(received_msg["Personel_Data"]["IAU_ID_Number"] == "" ? "t-no" : "t-yes")}']`).innerText}</span></td>
								<td class="en-font">${received_msg["Personel_Data"]["First_Name"]}</td>
								<td class="en-font">${(new Date(received_msg["CreatedDate"]).toLocaleDateString([], dateOptions) + " - " + new Date(received_msg["CreatedDate"]).toLocaleTimeString([], timeOptions))}</td>
								<td>
                                        <div class="dropdown dropbtn">
											${(received_msg["Required_Fields_Notes"] == null ? "" : received_msg["Required_Fields_Notes"].substr(0, 15) + " ...")}
											<div class="dropdown-content">
												<div class="p-10">
													${(received_msg["Required_Fields_Notes"] == null ? "" : received_msg["Required_Fields_Notes"])}
												</div>
											</div>
										</div>

                                </td>
							</tr>
						`+ html
                        }
                    }
                };

                ws.onclose = function (ee) {
                    WebSocketTest();
                };
            } else {

                alert("WebSocket NOT supported by your Browser!");
            }
        }
    }

})(jQuery)