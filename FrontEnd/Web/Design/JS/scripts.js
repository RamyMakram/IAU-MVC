
////////////////////////////////////////////////////get cookie of lang /////////////////////////////////
var language = GetCookie();

function GetCookie() {
    let name = "lang=";
    let cookieValue = null;
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            cookieValue = c.substring(name.length, c.length);
        }
    }
    return cookieValue;
}
$('img').on('dragstart', function (event) { event.preventDefault(); });
////////////////////////////////////////////////////get cookie of lang /////////////////////////////////
let isAffilate = 0;
let uploadfiles = [];
let FileNames = [];
var fileData = new FormData();
let CurrentPage = 1;
let enterPersonnel = false,
    enterDocuments = false,
    enterDocumentOther = false;
let inquiry = false;
let Redirect = false;
let RedirectReqType = null;
var isSaudi = 1
var CityComponentSelect = ""
var Cities = []
var RegionComponentSelect = ""
let subservice = []
let mainservice = []
let nationalty = []
let Countries = []
let type = []
let titles = []
let doctype = []
var Answer = [];
let $AppType = []
$(document).ready(function () {
    CountryState()
    let data = null;
    let cook = document.cookie.split(';');
    cook.forEach(i => {
        let keyandval = i.split('=');
        if (keyandval[0].trim().replace(' ', '') == "us") {
            data = i.replace(' ', '').split("us=")[1]
        }
    })
    if (data == '')
        data = null;
    let mst = localStorage.getItem("mst");
    let ret = localStorage.getItem("ret");
    if (data != null && data != "" && mst != null && mst != "" && ret != null && ret != "") {
        $(".loading").addClass("callApi");
        $.ajax({
            url: `https://outres.iau.edu.sa/commondata/api/v1/userinfo`,
            data: {
                userName: data,
                lang: language
            },
            type: "GET",
            success: function (da) {
                var res = da
                Redirect = true;
                document.cookie = "";
                window.history.pushState({}, document.title, "/");
                $('#NewRequest').click();
                $(`[data-mainserviceid='${mst}']`).addClass("active").click()
                RedirectReqType = ret;
                localStorage.removeItem('mst')
                localStorage.removeItem('ret')
                $("#FirstName").val(res["firstName"]).attr("disabled", "")
                $("#MiddelName").val(res["middleName"]).attr("disabled", "")
                $("#FamilyName").val(res["familyName"]).attr("disabled", "")
                $('#IAUID').val(res["iauIdNumber"]).attr("disabled", "")
                isAffilate = 1;
                $("#Affiliated option:selected").text("yes")
                $("#Affiliated").attr("disabled", "")
                $('#Email').val(res["email"]).attr("disabled", "")
                $('#Mobile').val(res["mobile"]).attr("disabled", "")
                if ($(`#Nationality_ID option`).length == 1) {
                    let timeout = setInterval(function () {
                        if ($(`#Nationality_ID option`).length != 1) {
                            $(`#Nationality_ID option:contains("${res["nationality"]}")`).attr('selected', true);
                            $("#Nationality_ID").attr("disabled", "")
                            clearInterval(timeout)
                        }
                    }, 500)
                } else {
                    $(`#Nationality_ID option:contains("${res["nationality"]}")`).attr('selected', true);
                    $("#Nationality_ID").attr("disabled", "")
                }
                if ($AppType.length == 0) {
                    let timeout = setInterval(function () {
                        if ($AppType.length != 0) {
                            $('#Affiliated').val("1");
                            FilterAppType(true);
                            clearInterval(timeout)
                        }
                    }, 500)
                }
                else {
                    $('#Affiliated').val("1");
                    FilterAppType(true);
                }

                localStorage.removeItem('mst')
                localStorage.removeItem('ret')

                setTimeout(e => { $(".loading").removeClass("callApi"); }, 500)
            },
            error: function () {
                localStorage.removeItem('mst')
                localStorage.removeItem('ret')
                document.cookie = "";
                location.href = location.href.split("?")[0]
            },
        })
    } else {
        localStorage.removeItem('mst')
        localStorage.removeItem('ret')
        document.cookie = "";
        if (data != null && data != "")
            location.href = location.href.split("?")[0]
        setTimeout(e => { $(".loading").removeClass("callApi"); }, 500)

    }
    if (document.body.offsetWidth < 767) {
        $("#Service_Type_Id img").removeAttr("data-bs-toggle");
        $("#Service_Type_Id img").removeAttr("data-bs-trigger");
        $("#Service_Type_Id img").removeAttr("data-bs-original-title");
        $("#Service_Type_Id img").removeAttr("aria-label");
    }
});
function reIntializeReType() {
    $('.requesttype').click(function () {
        let ID = $('.mainservice.active').attr('data-mainserviceid');
        $(".requesttype").removeClass("active");
        $(this).addClass("active");
        $(".loading").addClass("active");

        $.ajax({
            url: "/Home/GetApplicantData?ServiceID=" + ID + "&RequestType=" + $(this).attr("data-requesttypeid"), method: "Post", data: {
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            }, success: function (data) {
                $AppType = JSON.parse(data)
                if (!Redirect)
                    $('#Affiliated').val("0").trigger("change");
                $("#right-arrow").click();
                setTimeout(e => { $(".loading").removeClass("active"); }, 500)
            }
        })
        if (this.getAttribute("data-requesttypenameEN").toLowerCase().includes("inquiry") || this.getAttribute("data-requesttypenameEN").toLowerCase().includes("سؤال")) {
            inquiry = true;
            uploadfiles = [];
            FileNames = [];
            $('#filesName').html("");
        }
        else {
            inquiry = false;
            uploadfiles = [];
            FileNames = [];
            $('#filesNameOther').html("");
        }
    })
    $(".stick").click(function (event) {
        if ($(event.currentTarget).hasClass("follow")) {
            window.location.href = "/Follow/Index";
            return;
        }
        else if ($(event.currentTarget).hasClass("submit")) {
            let value = $(event.currentTarget.parentNode.parentNode).attr("id");
            $("#" + value + " .stick").removeClass("active");
            $(event.currentTarget).addClass("active");
            CurrentPage = 1;

            $(".nav-fill .nav-item:nth-of-type(" + CurrentPage + ") .nav-link").addClass("active")
            $(".nav-fill").attr("style", "");
            $(`.containt > .row`).attr("style", "display:none;");
            $(`.containt > .row[data-PageIndex='${CurrentPage}']`).attr("style", "display:flex;");
        }
    });
}
$("#right-arrow").click(function () {
    if (CurrentPage == 1) {//Will Enter Reqtype
        CurrentPage++;
        $(".nav-fill .nav-link").removeClass("active");
        $(".nav-fill .nav-item:nth-of-type(" + CurrentPage + ") .nav-link").addClass("active")
        $('.containt > .row').attr("style", "display:none;");
        $(`.containt > .row[data-PageIndex='${CurrentPage}']`).attr("style", "display:flex;");
    }
    else if (CurrentPage == 2) {//Will Enter Personal Data
        if ($("#Request_Type_Id .active ").attr("data-requesttypenameEN").toLowerCase() == "inquiry" || $("#Request_Type_Id .active ").attr("data-requesttypenameEN").toLowerCase() == "سؤال") {
            $("#Documents-Inquery").attr("data-PageIndex", "4");
            $("#Documents-Other").attr("data-PageIndex", "");
            $(".nav-fill .nav-item:nth-of-type(" + 4 + ") .nav-link").attr({
                "data-slide-to": "Documents-Inquery"
            });

        } else {
            $("#Documents-Inquery").attr("data-PageIndex", "");
            $("#Documents-Other").attr("data-PageIndex", "4");
            $(".nav-fill .nav-item:nth-of-type(" + 4 + ") .nav-link").attr({
                "data-slide-to": "Documents-Other"
            });
        }
        CurrentPage++;
        $(".nav-fill .nav-link").removeClass("active");
        $(".nav-fill .nav-item:nth-of-type(" + CurrentPage + ") .nav-link").addClass("active")
        $('.containt > .row').attr("style", "display:none;");
        $(`.containt > .row[data-PageIndex='${CurrentPage}']`).attr("style", "display:flex;");
    }
    else if (CurrentPage == 3) {//Will Enter ReqInfo
        ////////validate PersonalData//////////////
        let controls = $('.required-field div:nth-of-type(2)').children('select,input');
        let affilte = document.getElementById('Affiliated');
        let error = false;
        ([...controls]).forEach(e => {
            if (e.value == "" || e.value == null || e.value == "null") {
                $(e).css({ 'border': '2px solid red', 'background': '#ffafaf' });
                if (e.id == "IAUID" && affilte.value == 0) {
                    $(e).css({ 'border': 'none', 'background': '#646e85' })
                }
                else {
                    error = true;
                    return;
                }
            }
            else {
                $(e).css({ 'border': 'none', 'background': 'white' })
            }
        });
        if (error)
            return;
        let e = $('#spanMobile'), z = $("#Mobile");
        if (z.val() == "" || z.val() == null || z.val() == "null" || z.val().length != 9) {
            $(e).css({ 'border': '2px solid red', 'background': '#ffafaf' });
            z.css({ 'background': '#ffafaf' });
            error = true;
            return;
        }
        else {
            $(e).css({ 'border': 'none', 'background': 'white' })
        }
        e = $('#idNumber')
        if (e.val() == "" || e.val() == null || e.val() == "null" || e.val().length != 10 || !document.getElementById('idNumber').checkValidity() || !/^\d*$/.test($('#idNumber').val())) {
            $(e).css({ 'border': '2px solid red', 'background': '#ffafaf' });
            error = true;
            return;
        }
        else {
            $(e).css({ 'border': 'none', 'background': 'white' })
        }
        var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/
        let em = $('#Email')
        error = !(regex.test(em.val()) && document.getElementById('Email').checkValidity())//not valid
        if (error) {
            em.css({ 'border': '2px solid red', 'background': '#ffafaf' });
            return
        }
        else {
            em.css({ 'border': 'none', 'background': 'white' })
        }
        error = !(document.getElementById('Mobile').checkValidity() && /^\d*$/.test($('#Mobile').val()));
        if (error) {
            $('#Mobile').css({ 'border': '2px solid red', 'background': '#ffafaf' });
            return
        }
        else {
            $('#Mobile').css({ 'border': 'none', 'background': 'white' })
        }
        if (!error) {
            CurrentPage++;
            $(".nav-fill .nav-link").removeClass("active");
            $(".nav-fill .nav-item:nth-of-type(" + CurrentPage + ") .nav-link").addClass("active")
            $('.containt > .row').attr("style", "display:none;");
            $(`.containt > .row[data-PageIndex='${CurrentPage}']`).attr("style", "display:flex;");
        }
    }
    else if (CurrentPage == 4) {//Enter Confirmation
        let error = false;
        if ($("#Request_Type_Id .active ").attr("data-requesttypenameEN").toLowerCase() == "inquiry" || $("#Request_Type_Id .active ").attr("data-requesttypenameEN").toLowerCase() == "سؤال") {
            let controls = $('#provider,#Sub_Services_ID,#Main_Services_ID,#Required_Fields_Notes');
            ([...controls]).forEach(e => {
                if (e.value == "" || e.value == null || e.value == "null" || e.value.replace(' ', '').length == 0) {
                    $(e).css({ 'border': '2px solid red', 'background': '#ffafaf' });
                    error = true;
                    return;
                }
                else {
                    $(e).css({ 'border': 'none', 'background': 'white' })
                }
            });
            ///required files
            if (supporteddocs.length > uploadfiles.length) {
                let e = $('#upload-area');
                e.css({ 'border': '2px solid red', 'background': '#ffafaf' });
                error = true;
            }
            else {
                let e = $('#upload-area');
                e.css({ 'border': 'none', 'background': supporteddocs.length != 0 ? 'white' : 'rgba(239, 239, 239, 0.57)' })
            }

            //eforms
            let formscount = $('.eform-btn').length;
            if ((formscount != 0 && $('.eform-btn.filled').length != formscount) || (formscount != 0 && Answer.length == 0)) {
                let e = $('#EFormsView').parent();
                e.css({ 'border': '2px solid red', 'background': '#ffafaf' });
                error = true;
            }
            else {
                let e = $('#EFormsView').parent();
                e.css({ 'border': 'none', 'background': formscount != 0 ? 'white' : 'rgba(239, 239, 239, 0.57)' })
            }

        }
        else {
            ([...$('#providerOther,#Required_Fields_Notes_Other')]).forEach(e => {
                if (e.value == "" || e.value == null || e.value == "null" || e.value.replace(' ', '').length == 0) {
                    $(e).css({ 'border': '2px solid red', 'background': '#ffafaf' });
                    error = true;
                    return;
                }
                else {
                    $(e).css({ 'border': 'none', 'background': 'white' })
                }
            });
        }
        if (error)
            return;
        else {
            CurrentPage++;
            $(".nav-fill .nav-link").removeClass("active");
            $(".nav-fill .nav-item:nth-of-type(" + CurrentPage + ") .nav-link").addClass("active")
            $('.containt > .row').attr("style", "display:none;");
            $(`.containt > .row[data-PageIndex='${CurrentPage}']`).attr("style", "display:flex;");
        }
        $('body').css({ 'overflow-y': 'auto !important' })

    }
    else {
        return;
    }
    if (CurrentPage > 0) {
        $("#left-arrow").attr("style", "visibility:visiable");
        $(".nav-fill").removeAttr("style");
    }

    if (CurrentPage >= $(".containt > .row").length || CurrentPage == 5) {
        $("#right-arrow").attr("style", "visibility:hidden");
    } else {
        if (CurrentPage > 2 && CurrentPage < 5) {
            $("#right-arrow").attr("style", "visibility:visiable");
        }
    }
    if ($(".containt > .row:nth-of-type(" + CurrentPage + ")").attr("id") == "personel-data-m"
    ) {
        enterPersonnel = true
    }
    else if (($(`.containt > .row[data-PageIndex='${CurrentPage}']`).attr("id") == "Documents-Inquery"
        && enterDocuments == false)) {
        enterDocuments = true;
        CurrentPage++;

    } else if (($(`.containt > .row[data-PageIndex='${CurrentPage}']`).attr("id") == "Documents-Inquery"
        && enterDocuments == true)) {
        CurrentPage++;

    } else if (($(`.containt > .row[data-PageIndex='${CurrentPage}']`).attr("id") == "Documents-Other"
        && enterDocumentOther == false)) {
        enterDocumentOther = true;
    }
    else if ($(`.containt > .row[data-PageIndex='${CurrentPage}']`).attr("id") == "Confirmation-m") {
        GeneratePdfData();
        // enterpdfpreview = true;
    }
});
$("#Mobile").change(function () {
    $("#Mobile").css({ 'border': 'none', 'background': 'white' })
    $("#spanMobile").css({ 'border': 'none', 'background': 'white' })
})
$("#left-arrow").click(function () {
    CurrentPage--;
    $(".nav-fill .nav-link").removeClass("active");
    $(".nav-fill .nav-item:nth-of-type(" + CurrentPage + ") .nav-link").addClass("active")
    $('.containt > .row').attr("style", "display:none;");
    $(`.containt > .row[data-PageIndex='${CurrentPage}']`).attr("style", "display:flex;");
    if (CurrentPage <= 1) {
        $("#left-arrow").attr("style", "visibility:hidden");
        if (CurrentPage == 0)
            $(".nav-fill").attr("style", "display:none");
    }

    if (CurrentPage > 2) {
        $("#right-arrow").attr("style", "visibility:visiable");
    }
    if (CurrentPage <= 2) {
        $("#right-arrow").attr("style", "visibility:hidden");
    }

});
$(".stick").click(function (event) {
    if ($(event.currentTarget).hasClass("follow")) {
        window.location.href = "/Follow/Index";
        return;
    }
    else if ($(event.currentTarget).hasClass("submit")) {
        let value = $(event.currentTarget.parentNode.parentNode).attr("id");
        $("#" + value + " .stick").removeClass("active");
        $(event.currentTarget).addClass("active");
        CurrentPage = 1;

        $(".nav-fill .nav-item:nth-of-type(" + CurrentPage + ") .nav-link").addClass("active")
        $(".nav-fill").attr("style", "");
        $(`.containt > .row`).attr("style", "display:none;");
        $(`.containt > .row[data-PageIndex='${CurrentPage}']`).attr("style", "display:flex;");
    }
});
$(".nav-fill .nav-link").click(function () {////navbar btn click
    let index = $(this).attr("data-counter");
    if (index > CurrentPage) {
        alert("you can't select step that you don't reach yet but you can select previous steps");
    } else {
        CurrentPage = index;
        $(".containt > .row").attr("style", "display:none;");
        $("#" + $(this).attr("data-slide-to")).attr("style", "display:flex;")
        $(".nav-fill .nav-link").removeClass("active");
        $(this).addClass("active");
        if (CurrentPage <= 2) {
            $("#right-arrow").attr("style", "visibility:hidden");
        } else if (CurrentPage > 2) {
            $("#right-arrow").attr("style", "visibility:visable");
        }
    }
});
///////////////////////////////////ServiceType////////////////////////////////////////////
$('.mainservice').click(function (e) {//service type
    let ID = $(this).attr('data-mainserviceid');
    $(".loading").addClass("active");
    $.ajax({
        url: "/Home/GetRequest?ServiceID=" + ID, method: "Post", data: {
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        }, success: function (data) {
            $("#Request_Type_Id").html(`
				<div class="col-md-4 col-lg-3 icon-text-logo" style="padding-left: 23px; margin-top:45px">
					${(language == "ar" ? '<img src="/Design/img/ArServiceType.png" style="width:100%" />'
                    : '<img src="/Design/img/EnRequestType.png" style="width:100%"/>')} 
				</div>`);
            let serverpath = $('#serverpath').html()
            let toogle = `${window.screen.width <= 700 ? "" : (`data-bs-toggle="tooltip" data-bs-placement="top"
										 data-bs-custom-class="beautifier"
										 title="Please include your IAU ID number, whether it is the student ID number or your ID job number, following by the Password."`)}`;
            JSON.parse(data).forEach(request => {
                let data_bind = `
							<div class="col-md-4 col-lg-3">
								<div class="stick requesttype" data-requesttypeid="${request.ID}" data-requesttypenameEN="${request.Name_EN}" data-requesttypename="${(language == "ar" ? request.Name_AR : request.Name_EN)}">
									<img src="${(serverpath + "/" + request.Image_Path)}" class="Toogle-TootipScreen" ${toogle} />
									<p>${(language == "ar" ? request.Name_AR : request.Name_EN)}</p >
								</div >
							</div >
					`
                console.log(data_bind)
                $("#Request_Type_Id").append(data_bind)
            })

            $("body").tooltip({ selector: '[data-bs-toggle=tooltip]' });
            $(`.mainservice`).removeClass('active')
            $(`.mainservice[data-mainserviceid="${ID}"]`).addClass('active');
            reIntializeReType();
            if (Redirect)
                $(`[data-requesttypeid='${RedirectReqType}']`).addClass("active").click()
            $("#right-arrow").click();
            setTimeout(e => { $(".loading").removeClass("active"); }, 500)
        }
    })
})

///////////////////////////////////////////////////////////////////////////////
let dropArea = document.querySelectorAll('#drop-area,#drop-area-other');
let Supportedfilename = "";
$("#drop-area-other p").click(function () {
    $("#filelistOther").click();
});
$("#drop-area p").click(function () {
    $("#filelistSupportive").click();
});
['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
    dropArea.forEach(i => {
        i.addEventListener(eventName, preventDefaults, false)
    })
});

dropArea.forEach(i => {
    console.log(i)
    i.addEventListener('drop', handleDrop, false);
})

function preventDefaults(e) {
    e.preventDefault()
    e.stopPropagation()
}

function handleDrop(e) {
    let dt = e.dataTransfer
    let files = dt.files
    HandelDragAndDrop(files)
}

function handleFiles(files) {//Supportded Files
    if (inquiry) {
        let index = uploadfiles.findIndex(q => q.ID == Current_Supportedfilename);
        if (index == -1) {
            uploadfiles.push({ ID: Current_Supportedfilename, File: files[0] });
            FileNames.push(files[0].name)
        } else {
            uploadfiles[index].File = files[0]
            FileNames[index] = files[0].name
        }

        $('#fileUpload' + Current_Supportedfilename).css("color", "green");
        document.getElementById('fileRemove' + Current_Supportedfilename).style.display = "inline";
    }
}
let DropedFile = []
counter = 0;
function HandelDragAndDrop(files) {
    let sum = 0;
    if (Math.floor((sum / 1024) / 1024 > 20)) {
        alert("max files size 20MB")
    } else {
        ([...files]).forEach(function (file) {
            console.log(file)

            if (FileNames.findIndex(q => q == file.name) == -1) {
                DropedFile.push(file)
                counter++;
                FileNames.push(file.name);
                $(inquiry ? '#filesNameDrop' : '#filesNameDropOther').append("<div class='col-md-6 fileshow' data-toggle='tooltip' data-placement='bottom' title='" + file.name + "' data-filename='" + file.name + "'  id='support-doc" + counter + "'>" + file.name.slice(0, 7) + ".. \t (" + Math.ceil(file.size / 1024) + " kb) <meter min=1 max=10 value=10></meter> <i class='far fa-times-circle' " + `onclick="deleteFileSupport('support-doc${counter}',this)"></i></div>`)
            }
        });
    }

}
function deleteFileSupport(id, _this) {
    event.stopPropagation();
    event.preventDefault();
    let filename = $(_this).parent().data("filename")
    console.log(filename)
    $('#filesNameDropOther #' + id).remove();
    $('#filesNameDrop #' + id).remove();
    FileNames.pop(filename)
    DropedFile.pop(DropedFile.find(q => q.name == filename))
}
var saverequest_Clicked = false;
let data = null;
function sendSMS() {
    document.getElementById('verficationFeedback').style.display = 'none';
    document.getElementById('GenralError').style.display = 'none';
    $('.verification-input input').val("")
    $('.verification-input input:nth-child(1)').focus()
    $("#saveRequestBTN").html("<img src='././Design/img/spinner1.gif' style='width: 53px;'/>");
    data = serialiazeForm();
    $(".loading").addClass("active");

    $.ajax({
        url: `/Home/SendVerification?to=${data.Personel_Data.Mobile}&email=${data.Personel_Data.Email}`,
        type: "Post", data: {
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        },
        success: function (result) {
            var myModal = new bootstrap.Modal(document.getElementById('FourMessage'), {
                keyboard: false,
                backdrop: 'static'
            })
            myModal.show();
            setTimeout(e => { $(".loading").removeClass("active"); }, 500)
        },
        complete: function () {
            if (language == "ar") {
                $("#saveRequestBTN").html("إرسال")

            } else {
                $("#saveRequestBTN").html("Submit")
            }
        },
    })
}
let supporteddocs = []

function PrepareFiles() {
    let lengthSuppor = supporteddocs.length;
    let lengthupload = uploadfiles.length;

    for (var i = 0; i < lengthSuppor; i++) {
        for (var j = 0; j < lengthupload; j++) {
            if (supporteddocs[i].ID == uploadfiles[j].ID) {
                fileData.append(uploadfiles[j].File.name, uploadfiles[j].File)
                //console.log(uploadfiles[j].File.name)
                break;
            }
        }
    }
}

function saveRequest() {
    $(".loading").addClass("active");
    let requestCode = "";
    $(".modal-body .verification-input input").each(element => {
        requestCode = requestCode + $(".modal-body .verification-input input:eq(" + element + ")").val();
    });
    $("#Comfirm-Digits").html("<img src='././Design/img/spinner1.gif' style='width:40px'/>");
    if (!saverequest_Clicked) {
        saverequest_Clicked = true;
        fileData = new FormData();
        if (inquiry)
            PrepareFiles();
        [...DropedFile].forEach(e => {
            fileData.append(e.name, e)
        })
        fileData.append('request_Data', JSON.stringify(data));
        fileData.append('code', requestCode);
        fileData.append('__RequestVerificationToken', $('input[name="__RequestVerificationToken"]').val());
        $.ajax({
            url: "/Home/saveApplicantDataAndRequest",
            type: "POST",
            data: fileData,
            processData: false,
            contentType: false,
            dataType: "text json",
            enctype: 'multipart/form-data',
            success: function (result) {
                if (result.success) {
                    $('#FourMessage').modal('hide');
                    $("#MainBody").addClass("mainbody");
                    $("#right-arrow").attr("style", "visibility:hidden");
                    $("#left-arrow").attr("style", "visibility:hidden");
                    document.getElementById('MainBody').innerHTML = language == "ar" ?
                        `<div class= "row" >
										<div class="success">
											<span>لقد تم ارسال طلبك بنجاح، </span><br />
											<span> وسيتم إرسال كود الطلب الخاص بكم لمتابعة طلبكم عن طريق الرسائل النصيه او البريد الإلكتروني</span>
										</div>
										<div class="col-md-4" style="padding: 25px; text-align: center;width: 100%;">
											<a href="" class="btn" id="Okaybutton">موافق</a>
										</div>
									</div > `
                        : `<div class= "row" >
										<div class="success">
											<span>Your request has been sent successfully. You will soon receive the </span><br />
											<span>TRACKING NUMBER and the related link to follow your request via SMS or Email </span>
										</div>
										<div class="col-md-4" style="padding: 25px; text-align: center;width: 100%;">
											<a href="" class="btn" id="Okaybutton">Ok</a>
										</div>
									</div > `
                    $('#FourMessage').modal('hide');
                }
                else {
                    if (result.result == "ErrorInCode")
                        document.getElementById('verficationFeedback').style.display = 'block';
                    else
                        document.getElementById('GenralError').style.display = 'block';
                    saverequest_Clicked = false
                }
                setTimeout(e => { $(".loading").removeClass("active"); }, 500)

            },
            error: function (err) {
                //console.log(err)
                saverequest_Clicked = false
                setTimeout(e => { $(".loading").removeClass("active"); }, 500)

            }, complete: function () {
                if (language == "ar") {
                    $("#Comfirm-Digits").html("تأكيد");
                } else {
                    $("#Comfirm-Digits").html("Confirm");
                }
                setTimeout(e => { $(".loading").removeClass("active"); }, 500)

            }
        });
    }
    else {
        $('#FourMessage').modal('hide');
    }
}

function AssignCity(data) {
    data.forEach(function (element) {
        CityComponentSelect += `<option ${(element.City_ID == 310 ? 'selected' : '')} value="${element.City_ID}">${(language == "ar" ? element.City_Name_AR : element.City_Name_EN)}</option>`
    });
    CityComponentSelect += "</select>"
}

$("#Required_Fields_Notes_Other").keyup(function () {
    $("#text-area-counter_Other").text($(this).val().length + "/400")
    if ($(this).val()) {
        $(".insideTextArea").hide();
        $(".insideTextAreaCounter").show()
    } else if (!$(this).val()) {
        $(".insideTextArea").show();
        $(".insideTextAreaCounter").hide()
    }
});

$("#Required_Fields_Notes").keyup(function () {
    $("#text-area-counter").text($(this).val().length + "/400")
    if ($(this).val()) {
        $(".insideTextArea").hide();
        $(".insideTextAreaCounter").show()
    } else if (!$(this).val()) {
        $(".insideTextArea").show();
        $(".insideTextAreaCounter").hide()
    }
});

function LoadApiDocumentsData() {
    $(".loading").addClass("active");

    let RID = document.getElementsByClassName('stick active requesttype')[0].getAttribute('data-requesttypeid');
    let SID = document.getElementsByClassName('stick active mainservice')[0].getAttribute('data-mainserviceid');
    let APPID = $('#Applicant_Type_ID  option:selected').val();
    $.ajax({
        url: `/Home/GetProviders?RID=${RID}&SID=${SID}&AID=${APPID}`,
        method: "Post",
        data: {
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        },
        success: function (result) {
            if (result != null) {
                let data = JSON.parse(result)
                $(inquiry ? "#provider" : "#providerOther").html(language == "ar" ? "<option disabled selected value='null'>اختر-----------------</option>" : "<option disabled selected value='null'>Select-----------------</option>")
                data.forEach(function (element) {
                    $(inquiry ? "#provider" : "#providerOther").append("<option value=" + element.ID + ">" + (language == "ar" ? element.Name_AR : element.Name_EN) + "</option>")
                });
            } else if (result == null) {
                $("#modelbody").append("No Data Added , try again later");
                $("#exampleModalCenter").modal("show");
            }
            setTimeout(e => { $(".loading").removeClass("active"); }, 500)

        }
    });
}
let SubServices = []
function GetMainServices(ID) {
    $(".loading").addClass("active");

    let SID = document.getElementsByClassName('stick active mainservice')[0].getAttribute('data-mainserviceid');
    let APPID = $('#Applicant_Type_ID  option:selected').val();
    $("#Main_Services_ID option").remove();
    $("#Main_Services_ID").append("<option disabled selected value='null'>Select ----------------</option>");

    $.ajax({
        url: `/Home/GetMainServices?ID=${ID}&SID=${SID}&AID=${APPID}`,
        method: "Post", data: {
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        },
        success: function (result) {
            if (result != null) {
                $("#Main_Services_ID").removeAttr("disabled");
                $("#Main_Services_ID").html(language == "ar" ? "<option disabled selected value='null'>اختر-----------------</option>" : "<option disabled selected value='null'>Select-----------------</option>")
                let data = JSON.parse(result)
                data.forEach(function (element) {
                    $("#Main_Services_ID").append("<option value=" + element.ID + ">" + (language == "ar" ? element.Name_AR : element.Name_EN) + "</option>")
                });
            } else if (result == null) {
                $("#modelbody").append("No Data Added , try again later");
                $("#exampleModalCenter").modal("show");
            }
            setTimeout(e => { $(".loading").removeClass("active"); }, 500)

        }
    });
}
function GetSubServices(ID) {
    $(".loading").addClass("active");

    $("#Sub_Services_ID option").remove();
    $("#Sub_Services_ID").append("<option disabled selected value='null'>Select ----------------</option>");

    $.ajax({
        url: `/Home/GetSub?ID=${ID}`,
        method: "Post", data: {
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        },
        success: function (result) {
            if (result != null) {
                let data = JSON.parse(result)
                uploadfiles = [];
                SubServices = data;
                $("#Sub_Services_ID").removeAttr("disabled");
                $("#Sub_Services_ID").html(language == "ar" ? "<option disabled selected value='null'>اختر-----------------</option>" : "<option disabled selected value='null'>Select-----------------</option>")

                data.forEach(function (element) {
                    $("#Sub_Services_ID").append("<option value=" + element.ID + ">" + (language == "ar" ? element.Name_AR : element.Name_EN) + "</option>")
                });
                $("#filesName").html("")
                supporteddocs = SubServices[0].Docs
                SubServices[0].Docs.forEach(function (element) {
                    let Name = language == "ar" ? element.Name_AR : element.Name_EN;
                    $("#filesName").append(`
										<div class= "col-lg-4 col-md-6 col-sm-6" style = "margin-bottom:10px;padding:0px;4px;" >
												<div style="background-color: #f6f6f6;display: inline; padding: 0px 3px;border-radius: 5px;">
													<span style="font-size:14px;display:inline-block;width:70%" title="${Name}" id="FileDefault${element.ID}">${Name.length > 17 ? Name.substring(0, 13) + "..." : Name}</span>
													<span style="display:none padding:2px 0px" id="FileName${element.ID}"></span>
													<input type="file" style="display: none;" id="uploadFileDialog${element.ID}" onchange="handleFiles(this.files)" />
													<i class="fa fa-upload" id="fileUpload${element.ID}" style="padding:0px 2px;color:gray" name="${element.ID}" data-RequiredName="${element.Name_EN}"></i>
													<i class="fa fa-trash" id="fileRemove${element.ID}" style="display:none" name="${element.ID}" data-RequiredName="${element.Name_EN}"></i>
												</div>
                                        </div >
				`)
                });
                if (SubServices[0].Docs.length == 0)
                    $('#upload-area').css('background-color', 'rgb(239 239 239 / 57%)')
                else
                    $('#upload-area').css('background-color', 'white')
                $(".fa-upload").click(function () {
                    Current_Supportedfilename = this.getAttribute("name");
                    Supportedfilename = this.getAttribute("data-RequiredName")
                    $("#uploadFileDialog" + Current_Supportedfilename).click();
                });

                $(".fa-trash").click(function () {
                    let RemoveCurrent_Supportedfilename = this.getAttribute("name");
                    uploadfiles.splice(uploadfiles.findIndex(q => q.ID == RemoveCurrent_Supportedfilename), 1)
                    document.getElementById('FileDefault' + RemoveCurrent_Supportedfilename).style.display = "inline-block";
                    document.getElementById('FileName' + RemoveCurrent_Supportedfilename).style.display = "none";
                    this.style.display = "none";
                    FileNames.splice(FileNames.indexOf(this.getAttribute("data-RequiredName")), 1)
                    document.getElementById("uploadFileDialog" + RemoveCurrent_Supportedfilename).value = "";
                    $('#fileUpload' + RemoveCurrent_Supportedfilename).css("color", "gray");
                });
            } else if (result == null) {
                $("#modelbody").append("No Data Added , try again later");
                $("#exampleModalCenter").modal("show");
            }
            setTimeout(e => { $(".loading").removeClass("active"); }, 500)

        }
    });
}
function GetEfroms(ID) {
    $(".loading").addClass("active");

    let Docs = SubServices.find(q => q.ID == ID)
    $("#filesName").html("")
    supporteddocs = Docs.Docs

    if (supporteddocs.length == 0)
        $('#upload-area').css('background-color', 'rgb(239 239 239 / 57%)')
    else
        $('#upload-area').css('background-color', 'white')

    Docs.Docs.forEach(function (element) {
        let Name = language == "ar" ? element.Name_AR : element.Name_EN
        $("#filesName").append(`
										<div class= "col-lg-4 col-md-6 col-sm-6" style = "margin-bottom:10px;padding:0px;4px;" >
											<div style="background-color: #f6f6f6;display: inline; padding: 0px 3px;border-radius: 5px;">
												<span style="font-size:14px;display:inline-block;width:70%" title="${Name}" id="FileDefault${element.ID}">${Name.length > 17 ? Name.substring(0, 13) + "..." : Name}</span>
												<span style="display:none padding:2px 0px" id="FileName${element.ID}"></span>
												<input type="file" style="display: none;" id="uploadFileDialog${element.ID}" onchange="handleFiles(this.files)" />
												<i class="fa fa-upload" id="fileUpload${element.ID}" style="padding:0px 2px;color:gray" name="${element.ID}" data-RequiredName="${element.Name_EN}"></i>
												<i class="fa fa-trash" id="fileRemove${element.ID}" style="display:none" name="${element.ID}" data-RequiredName="${element.Name_EN}"></i>
											</div>
                                        </div>
					`)
    });
    $(".fa-upload").click(function () {
        Current_Supportedfilename = this.getAttribute("name");
        Supportedfilename = this.getAttribute("data-RequiredName")
        $("#uploadFileDialog" + Current_Supportedfilename).click();
    });

    $(".fa-trash").click(function () {
        let RemoveCurrent_Supportedfilename = this.getAttribute("name");
        uploadfiles.splice(uploadfiles.findIndex(q => q.ID == RemoveCurrent_Supportedfilename), 1)
        document.getElementById('FileDefault' + RemoveCurrent_Supportedfilename).style.display = "inline-block";
        document.getElementById('FileName' + RemoveCurrent_Supportedfilename).style.display = "none";
        this.style.display = "none";
        FileNames.splice(FileNames.indexOf(this.getAttribute("data-RequiredName")), 1)
        document.getElementById("uploadFileDialog" + RemoveCurrent_Supportedfilename).value = "";
        $('#fileUpload' + RemoveCurrent_Supportedfilename).css("color", "gray");
    });

    $.ajax({
        url: `/Home/GetEforms?ID=${ID}`,
        method: "Post", data: {
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        },
        success: function (result) {
            $("#EFormsView").html("");
            $('#Eform-read-summary').html("");
            if (result != null) {
                let data = JSON.parse(result)
                if (data.length != 0) {
                    $('#Eform-read-summary').parent().css('display', 'block');
                    $('#E-forms').css('background-color', 'white')

                    data.forEach(function (element) {
                        $("#EFormsView").append(`
									<div class="col-lg-6 col-md-6 col-sm-6" style = "margin-bottom:5px" >
										<div class="row icon-container" style="padding:0px;margin:0px">
												<a style="padding:0px" class="btn btn-outline eform-btn" data-id="${element.ID}"><i class="fas fa-passport" style="font-size: 31px;color: #4f693a;padding:0 5px"></i>${(language == 'ar' ? element.Name : element.Name_EN)}</a>
										</div>
									</div>
				`
                        )

                        $('#Eform-read-summary').append(
                            `<div class="row icon-container" style="padding:0px;margin:0px">
						    <a style="padding:0px;color: #4f693a !important;" class="btn btn-outline eform-readonly-btn" data-id="${element.ID}">
                                <i class="fas fa-passport" style="font-size: 31px;padding:0 5px;"></i>
                                <span>${(language == 'ar' ? element.Name : element.Name_EN)}</span>
                            </a>
						</div>
                        `
                        )
                    });
                }
                else {
                    $('#Eform-read-summary').parent().css('display', 'none');
                    $('#E-forms').css('background-color', 'rgb(239 239 239 / 57%)')
                }
                ReIntalizeEformListener();
                ReIntalizeEformReadOnlyListener()
            }
            setTimeout(e => { $(".loading").removeClass("active"); }, 500)

        }
    });

}

function serialiazeForm() {
    let Data = {
        Unit_ID: $((inquiry ? "#provider" : "#providerOther") + '  option:selected').val() == "null" ? null : $((inquiry ? "#provider" : "#providerOther") + '  option:selected').val(),
        Sub_Services_ID: $('#Sub_Services_ID  option:selected').val() == "null" ? null : $('#Sub_Services_ID  option:selected').val(),
        Required_Fields_Notes: inquiry ? ($('#Required_Fields_Notes').val() == "null" ? null : $('#Required_Fields_Notes').val()) : ($('#Required_Fields_Notes_Other').val() == "null" ? null : $('#Required_Fields_Notes_Other').val()),
        Service_Type_ID: document.getElementsByClassName('stick active mainservice')[0].getAttribute('data-mainserviceid'),
        Request_Type_ID: document.getElementsByClassName('stick active requesttype')[0].getAttribute('data-requesttypeid'),
        Personel_Data: {
            IAU_ID_Number: $('#IAUID').val(),
            Applicant_Type_ID: $('#Applicant_Type_ID  option:selected').val() == "null" ? null : $('#Applicant_Type_ID  option:selected').val(),
            Title_Middle_Names_ID: $('#title  option:selected').val() == "null" ? null : $('#title  option:selected').val(),
            First_Name: $("#FirstName").val(),
            Middle_Name: $("#MiddelName").val(),
            Last_Name: $("#FamilyName").val(),
            Nationality_ID: $('#Nationality_ID  option:selected').val() == "null" ? null : $('#Nationality_ID  option:selected').val(),
            ID_Document: $('#ID_Document  option:selected').val() == "null" ? null : $('#ID_Document  option:selected').val(),
            Country_ID: $('#Country_ID  option:selected').val() == "null" ? null : $('#Country_ID  option:selected').val(),
            ID_Number: $('#idNumber').val(),
            Address_CountryID: $('#City_Country_2').val(),
            Address_CityID: isSaudi ? $('#City_Country_1').val() : null,
            Adress_RegionID: isSaudi ? $('#Region_Postal_Code_1').val() : null,
            Address_City: isSaudi ? null : $('#City_Country_1').val(),
            Adress_Region: isSaudi ? null : $('#Region_Postal_Code_1').val(),
            Postal_Code: $('#Region_Postal_Code_2').val(),
            Email: $('#Email').val(),
            Mobile: "966" + $('#Mobile').val(),
            E_Forms_Answer: Answer
        },
        Affiliated: isAffilate,
        ID_Document_Name: $('#ID_Document  option:selected').text(),
        title_Middle_Names: $('#title  option:selected').text(),
        Nationality_Name: $('#Nationality_ID  option:selected').text(),
        Country_Name: isSaudi ? $('#City_Country_2 option:selected').text() : $('#City_Country_2').val(),
        Region_Postal_Code_1: isSaudi ? $('#Region_Postal_Code_1 option:selected').text() : $('#Region_Postal_Code_1').val(),
        Region_Postal_Code_2: $('#Region_Postal_Code_2').val(),
        Applicant_Type_Name: $('#Applicant_Type_ID  option:selected').text(),
        title: $('#title  option:selected').val() == "null" ? null : $('#title  option:selected').val(),
        Main_Services_ID: $('#Main_Services_ID  option:selected').val() == "null" ? null : $('#Main_Services_ID  option:selected').val(),
        Main_Services_Name: $('#Main_Services_ID  option:selected').text(),
        Name: `${($("#title  option:selected").text())} ${$("#FirstName").val()} ${$("#MiddelName").val()} ${$("#FamilyName").val()}`,
        Address: "",
        file_names: FileNames
    }
    //console.log(Data);
    return Data;
}

function SerializeGenratePDF() {
    let Data = {
        Affiliated: $("#Affiliated option:selected").text(),
        provider_Name: $((inquiry ? "#provider" : "#providerOther") + '  option:selected').text(),
        Sub_Services_Name: $('#Sub_Services_ID  option:selected').text(),
        Service_Type_Name: document.getElementsByClassName('stick active mainservice')[0].getAttribute('data-mainservicename'),
        first_Name: $("#FirstName").val(),
        middle_Name: $("#MiddelName").val(),
        last_Name: $("#FamilyName").val(),
        Request_Type_Name: document.getElementsByClassName('stick active requesttype')[0].getAttribute('data-requesttypename'),
        Applicant_Type_Name: $('#Applicant_Type_ID  option:selected').text(),
        Nationality_Name: $('#Nationality_ID  option:selected').text(),
        Country_Name: $('#Country_ID option:selected').text(),
        ID_Document_Name: $('#ID_Document  option:selected').text(),
        Document_Number: $('#idNumber').val(),
        Region_Postal_Code_1: $('#City_Country_2 option:selected').text(),
        Region_Postal_Code_2: isSaudi ? $('#Region_Postal_Code_1 option:selected').text() : $('#Region_Postal_Code_1').val(),
        Region: isSaudi ? $('#Region_Postal_Code_1 option:selected').text() : $('#Region_Postal_Code_1').val(),
        CityAndRegion: isSaudi ? $('#City_Country_1 option:selected').text() : $('#City_Country_1').val(),
        postal: $('#Region_Postal_Code_2').val(),
        Email: $('#Email').val(),
        Mobile: "966" + $('#Mobile').val(),
        Required_Fields_Notes: inquiry ? ($('#Required_Fields_Notes').val() == "null" ? null : $('#Required_Fields_Notes').val()) : ($('#Required_Fields_Notes_Other').val() == "null" ? null : $('#Required_Fields_Notes_Other').val()),

        Personel_Data: {
            IAU_ID_Number: $('#IAUID').val(),
            Applicant_Type_ID: $('#Applicant_Type_ID  option:selected').val() == "null" ? null : $('#Applicant_Type_ID  option:selected').val(),
            Title_Middle_Names_ID: $('#title  option:selected').val() == "null" ? null : $('#title  option:selected').val(),
            Nationality_ID: $('#Nationality_ID  option:selected').val() == "null" ? null : $('#Nationality_ID  option:selected').val(),
            ID_Document: $('#ID_Document  option:selected').val() == "null" ? null : $('#ID_Document  option:selected').val(),
            Country_ID: document.querySelector('#Country_ID').nodeName == "SELECT" ? $('#Country_ID  option:selected').val() : null,
            Address_CountryID: isSaudi ? $('#City_Country_2 option:selected').text() : $('#City_Country_2').val(),
            City_Country_1: $('#City_Country_1').val(),
            Adress_RegionID: $('#Region_Postal_Code_1').val(),
            Address_City: $('#City_Country_1').val(),
            Adress_Region: $('#Region_Postal_Code_1').val(),
            Postal_Code: $('#Region_Postal_Code_2').val(),
            Email: $('#Email').val(),
            Mobile: "966" + $('#Mobile').val(),
        },
        title_Middle_Names: $('#title  option:selected').text(),
        title: $('#title  option:selected').val() == "null" ? null : $('#title  option:selected').val(),
        Main_Services_ID: $('#Main_Services_ID  option:selected').val() == "null" ? null : $('#Main_Services_ID  option:selected').val(),
        Main_Services_Name: $('#Main_Services_ID  option:selected').text(),
        Name: `${($("#title  option:selected").text())} ${$("#FirstName").val()} ${$("#MiddelName").val()} ${$("#FamilyName").val()}`,
        Address: "",
        file_names: [...FileNames]
    }
    return Data;
}

function GeneratePdfData() {
    var ar = {
        't-summary': '   ملخص الطلب   ',
        't-serviceType': '   نوع الخدمة   ',
        't-reqtype': '   نوع الطلب   ',
        't-personalData': '   البيانات الشخصية   ',
        't-genralInfo': '   البيانات العامة   ',
        't-IAUAff': '   منسوب للجامعة   ',
        't-applicanttype': '   مقدم الطلب   ',
        't-firstname': '   الاسم الاول   ',
        't-middlename': '   الاسم الثاني   ',
        't-lastname': '   الاسم الاخير   ',
        't-nationalty': '   الجنسية   ',
        't-country': '   المدينة   ',
        't-iddoc': '   نوع الهوية   ',
        't-idnumber': '   رقم الهوية   ',
        't-address': '   العنوان   ',
        't-city': '   المدينة   ',
        't-region': '   المنطقة   ',
        't-country': '   الدولة   ',
        't-country2': '   مكان الاقامة   ',
        't-postal': '   الرقم البريدي   ',
        't-contactinfo': '   معلومات الإتصال   ',
        't-email': '   البريد الإلكتروني   ',
        't-phone': '   رقم الهاتف   ',
        't-attachment': '    المرفقات    ',
        't-pdf-note': '    ملاحظات    ',
    }
    var en = {
        't-summary': 'Summary Of Request',
        't-serviceType': 'Service Type',
        't-reqtype': 'Request Type',
        't-personalData': 'Personal Data',
        't-genralInfo': 'General Information',
        't-IAUAff': 'IAU Affiliated',
        't-applicanttype': 'Applicant Type',
        't-firstname': 'First Name',
        't-middlename': 'Middle Name',
        't-lastname': 'Last Name',
        't-nationalty': 'Nationality',
        't-country': 'Country',
        't-iddoc': 'ID Document',
        't-idnumber': 'ID Number',
        't-address': 'Address Information',
        't-city': 'City',
        't-region': 'Region',
        't-country': 'Country',
        't-country2': 'Country of residence',
        't-postal': 'Postal Code',
        't-contactinfo': 'Contact Information',
        't-email': 'Email',
        't-phone': 'Mobile Phone',
        't-attachment': 'Attachments',
        't-pdf-note': 'Request Notes',
    }
    let Form = SerializeGenratePDF();
    let FilesDiv = ""
    Form["file_names"].forEach(e => {
        let checkif_reqdoc = uploadfiles.find(q => q.File.name == e);
        let Name = ""
        if (checkif_reqdoc != null) {
            let element = supporteddocs.find(q => q.ID == checkif_reqdoc.ID)
            Name = language == "ar" ? element.Name_AR : element.Name_EN;
        }
        FilesDiv += (e == "," ? "" : `<p style='margin:0 6px;color:green !important;width:200px !important;white-space: nowrap;' data-toggle='tooltip' data-placement='bottom' title='${Name != "" ? Name : e}'><i class='fa fa-paperclip' aria-hidden='true'></i> ${(Name != "" ? Name.slice(0, 20) + (Name.length > 20 ? " ..." : "") : e.slice(0, 20) + (e.length > 20 ? " ..." : ""))}</p>`)
    })
    document.getElementById('padf').innerHTML = `
	<div style = "padding: 15px;display: inline-flex;justify-content: space-between;width: 100%;flex-wrap: wrap;" class="header-of-pdf" >
	<img src="../Design/img/mustafid (2).png">
		<img src="../Design/img/VisionLogo.png" style="padding-top: 10px;margin-bottom: 8px;">
												</div>
		<table class='container' id="PDFTable" style="width: 100%;padding: 15px;">
			<tbody>
				<tr>
					<th class="boldtitle" key="t-summary"></th>
				</tr>
				<tr class="row">
					<th class="col-4" key="t-serviceType"></th>
					<td class="col-8">${Form["Service_Type_Name"]}</th>
				</tr>
				<tr class="row">
					<th class="col-4" key="t-reqtype"></th>
					<td class="col-8">${Form["Request_Type_Name"]}</th>
				</tr>
				<tr class="row">
					<th class="boldtitle" key="t-personalData"></th>
				</tr>
				<tr class="row">
					<th class="boldtitle" key="t-genralInfo"></th>
				</tr>
				<tr class="row">
					<th class="col-4" key="t-IAUAff"></th>
					<td class="col-8">${Form["Affiliated"]}</th>
				</tr>
				<tr class="row">
					<th class="col-4" key="t-applicanttype"></th>
					<td class="col-8">${Form["Applicant_Type_Name"]}</th>
				</tr>
				<tr class="row">
					<th class="col-4" key="t-firstname"></th>
					<td class="col-8">${Form["first_Name"]}</th>
				</tr>
				<tr class="row">
					<th class="col-4" key="t-middlename"></th>
					<td class="col-8">${Form["middle_Name"]}</th>
				</tr>
				<tr class="row">
					<th class="col-4" key="t-lastname"></th>
					<td class="col-8">${Form["last_Name"]}</th>
				</tr>
				<tr class="row">
					<th class="boldtitle" key="t-nationalty"></th>
				</tr>
				<tr class="row">
					<th class="col-4" key="t-nationalty"></th>
					<td class="col-8">${Form["Nationality_Name"]}</th>
				</tr>
				<tr class="row">
					<th class="col-4" key="t-country2"></th>
					<td class="col-8">${Form["Country_Name"]}</th>
				</tr>
				<tr class="row">
					<th class="col-4" key="t-iddoc"></th>
					<td class="col-8">${Form["ID_Document_Name"]}</th>
				</tr>
				<tr class="row">
					<th class="col-4" key="t-idnumber"></th>
					<td class="col-8">${Form["Document_Number"]}</th>
				</tr>
				<tr class="row">
					<th class="boldtitle" key="t-address"></th>
				</tr>
				<tr class="row">
					<th class="col-4" key="t-country"></th>
					<td class="col-8">${Form["Region_Postal_Code_1"]}</th>
				</tr>
				<tr class="row">
					<th class="col-4" key="t-region"></th>
					<td class="col-8">${Form["Region_Postal_Code_2"]}</th>
				</tr>
				<tr class="row">
					<th class="col-4" key="t-city"></th>
					<td class="col-8">${Form["CityAndRegion"]}</th>
				</tr>
				<tr class="row">
					<th class="col-4" key="t-postal"></th>
					<td class="col-8">${Form["postal"]}</th>
				</tr>
				<tr>
					<th class="boldtitle" key="t-contactinfo"></th>
				</tr>
				<tr class="row">
					<th class="col-4" key="t-email"></th>
					<td class="col-8">${Form["Email"]}</th>
				</tr>
				<tr class="row">
					<th class="col-4" key="t-phone"></th>
					<td class="col-8">${Form["Mobile"]}</th>
				</tr>
				<tr class="row">
					<th class="col-4" key="t-pdf-note"></th>
					<td class="col-8">${Form["Required_Fields_Notes"]}</th>
				</tr>
                ${(Form["file_names"].length == 0) ? "" :
            `<tr>
					<th class="boldtitle" key="t-attachment"></th>
				</tr>
				<tr class="row" style="display:inline-flex">
					<th class="col-12 FilesINPDF">
                        <div class="row" style="display:flex;flex-wrap:wrap;justify-content: center;">${FilesDiv}</div>
                    </th>
				</tr>`
        }
			</tbody>
		</table>

		`


    let data = null;
    if (language == 'ar') {
        data = ar;
    }
    else {
        data = en;
    }
    for (var key in data) {
        $(`th[key='${key}']`).text(data[key])
        //$(`th[key='${key}']`).css({ 'width': '50%' })
    }
}

function review() {
    $("#left-arrow").attr("style", "visibility:hidden");
    $("#ReviewMaster").removeAttr("style");
    $("#ReviewMaster2").removeAttr("style");
    for (var i = 2; i < 6; i++) {
        $(".containt > .row:nth-of-type(" + i + ")").attr("style", "display:flex; margin-bottom:40px");
    }
    $(".containt > .row:nth-of-type(" + 6 + ")").attr("style", "display:none");
}

function confirm() {
    $("#ReviewMaster").attr("style", "display:none");
    $("#ReviewMaster2").attr("style", "display:none");
    for (var i = 2; i < 6; i++) {
        $(".containt > .row:nth-of-type(" + i + ")").attr("style", "display:none");
    }
    $(".containt > .row:nth-of-type(" + 6 + ")").removeAttr("style");
    $("#left-arrow").attr("style", "visibility:visable");
}

function FilterAppType(aff) {
    let $data = $AppType.filter(q => aff ? q.Affliated : !q.Affliated)
    $("#Applicant_Type_ID").html(language == "ar" ? '<option disabled selected value="null">اختر-----------------</option>' : '<option disabled selected value="null">Select-----------------</option>');
    $data.filter(q => !q.Affiliated).forEach(i => {
        $("#Applicant_Type_ID").append("<option value=" + i.Applicant_Type_ID + ">" + (language == "ar" ? i.Applicant_Type_Name_AR : i.Applicant_Type_Name_EN) + "</option>")
    })
}
function AffiliatedState() {
    isAffilate = parseInt($("#Affiliated option:selected").val());
    if (isAffilate == 1) {
        //$(".loading").addClass("active");
        $("#IAUID").removeAttr("disabled");
        if (!Redirect) {
            localStorage.setItem("ret", document.getElementsByClassName('stick active requesttype')[0].getAttribute('data-requesttypeid'))
            localStorage.setItem("mst", document.getElementsByClassName('stick active mainservice')[0].getAttribute('data-mainserviceid'))
            $("#RedirectBTN").click();
        }

    } else if (($("#Affiliated option:selected").text()).toLowerCase()) {
        $("#IAUID").attr("disabled", "disabled");
    }
    FilterAppType(isAffilate == 1)
}
$('#Nationality_ID').change(i => {
    $("#__IDNUMBind").text(language == "ar" ? "رقم الهوية الوطنية" : "National ID Number")
})
function CountryState() {
    $(".loading").addClass("active");

    let ID = $("#City_Country_2 option:selected").val()
    $.ajax({
        url: "/Home/GetCityRegion?CID=" + ID, method: "POST", data: {
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        }, success: function (datxa) {
            let data = JSON.parse(datxa)
            if (data.Regions.length != 0) {
                isSaudi = 1;
                Cities = data.City;
                let id = 5;
                CityComponentSelect = `<select id="City_Country_1" name="City_Country_1">`
                RegionComponentSelect = `<select id="Region_Postal_Code_1" name="Region_Postal_Code_1">`
                data.Regions.forEach(function (element) {
                    RegionComponentSelect += `<option ${(element.Region_ID == 5 ? ' selected' : '')} value="${element.Region_ID}"> ${(language == "ar" ? element.Region_Name_AR : element.Region_Name_EN)}</option>`
                });
                RegionComponentSelect += "</select>"
                AssignCity(Cities.filter(q => q.Region_ID == 5));
                var CityAttr = document.getElementById('City');
                if (CityComponentSelect != "") {
                    CityAttr.innerHTML = `
									<img />
									${CityComponentSelect}
									`
                }
                var RegionAttr = document.getElementById('Region');
                if (RegionComponentSelect != "") {
                    RegionAttr.innerHTML = `
									<img />
									${RegionComponentSelect}
									`
                    document.getElementById('Region_Postal_Code_1').addEventListener('change', function (e) {
                        let cities = Object.create(Cities).filter(q => q.Region_ID == this.value)
                        CityComponentSelect = `<select id="City_Country_1" name="City_Country_1">`
                        AssignCity(cities)
                        document.getElementById('City').innerHTML = `
									<img />
			${CityComponentSelect}
			`;
                    })
                }
            }
            else {
                isSaudi = 0;
                var RegionAttr = document.getElementById('Region');
                var CityAttr = document.getElementById('City');
                RegionAttr.innerHTML = `
								<img />
			<input type="text" id="Region_Postal_Code_1" name="Region_Postal_Code_1" />
			`
                CityAttr.innerHTML = `
								<img />
			<input type="text" id="City_Country_1" name="City_Country_1">
				`
            }
            setTimeout(e => { $(".loading").removeClass("active"); }, 500)

        }
    })
}
$("#Documents-Inquery #Documents #E-forms .icon-container").click(function () {
    var myModal = new bootstrap.Modal(document.getElementById('formModel'), {
        keyboard: false
    })
    myModal.show()
});

$("#Required_Fields_Notes").keyup(function () {
    $("#Required_Fields_Notes-counter").text($(this).val().length + "/400")
    if ($(this).val()) {
        $(".insideTextArea").hide();
        $(".insideTextAreaCounter").show()
    } else if (!$(this).val()) {
        $(".insideTextArea").show();
        $(".insideTextAreaCounter").hide()
    }
});

$("btnlogin").click(function () {
    alert("Login");
});

var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
    return new bootstrap.Tooltip(tooltipTriggerEl)
})

function loginCoordinator() {
    window.location.href = "../Dashboard/home.html"
}

$(".modal-body .verification-input input").click(function (element) {
    if (!$(element.currentTarget).val()) {
        $(".modal-body .verification-input input").each(element => {
            if (!$(".modal-body .verification-input input:eq(" + element + ")").val()) {
                $(".modal-body .verification-input input:eq(" + element + ")").focus();
                return false;
            }
        });
    }
});

$(".modal-body .verification-input input").keyup(function (element) {
    $(".modal-body .verification-input input").each(element => {
        if (!$(".modal-body .verification-input input:eq(" + element + ")").val()) {
            $(".modal-body .verification-input input:eq(" + element + ")").focus();
            return false;
        }
    });

    if (element.key == "Enter") {
        var requestCode = "";
        $(".modal-body .verification-input input").each(element => {
            requestCode = requestCode + $(".modal-body .verification-input input:eq(" + element + ")").val();
        });
        if (requestCode.trim().length == 4) {
            saveRequest(requestCode);
        } else {
            alert("have to enter verification code");
        }
    }
});
let AllowResend = true;
$("#ResendVerificationCode").click(function () {

    if (AllowResend) {
        $("#saveRequestBTN").html("<img src='/Design/img/spinner1.gif' style='width: 53px;'/>");
        let data = serialiazeForm();
        $(".loading").addClass("active");

        $.ajax({
            url: `/Home/SendVerification?to=${data.Personel_Data.Mobile}&email=${data.Personel_Data.Email}`,
            type: "Post", data: {
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            },
            success: function (result) {
                setTimeout(e => { $(".loading").removeClass("active"); }, 500)
                $("#ResendVerificationCode").attr("disabled", "disabled");
                if (language == "ar") {
                    $("#ResendVerificationCode").html("يمكنك اعادة ارسال الكود بعد <span id='downConter'>(30)</span> ثانية");
                } else {
                    $("#ResendVerificationCode").html("You Can Resend Code After <span id='downConter'>(30)</span> Second");
                }
                AllowResend = false;

                counter = 30;
                var x = setInterval(function () {
                    counter--;
                    document.getElementById("downConter").innerHTML = "(" + counter + ")";
                    if (counter == 0) {
                        AllowResend = true;
                        $("#ResendVerificationCode").removeAttr("disabled");
                        if (language == "ar") {
                            $("#ResendVerificationCode").html("أعد ارسال كود التحقق");
                        } else {
                            $("#ResendVerificationCode").html("Resend Verification Code");
                        }
                        clearInterval(x);
                    }
                }, 1000);
            }, error: function () {
                AllowResend = true;

                $("#saveRequestBTN").html(language == "ar" ? "إرسال" : "Submit")
                setTimeout(e => { $(".loading").removeClass("active"); }, 500)
            },
            complete: function () {
                AllowResend = true;

                $("#saveRequestBTN").html(language == "ar" ? "إرسال" : "Submit")
                setTimeout(e => { $(".loading").removeClass("active"); }, 500)

            },
        })
    }
});
function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}