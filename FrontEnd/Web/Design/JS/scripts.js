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
let incrementValue = 1;
let enterPersonnel = false,
	enterDocuments = false,
	enterDocumentOther = false;
let inquiry = false;
let Redirect = false;
let RedirectReqType = null;
var isSaudi = 0
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
$(document).ready(function () {
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
		$.ajax({
			url: `https://outres.iau.edu.sa/commondata/api/v1/userinfo?userName=${encodeURIComponent(data)}&lang=en`,
			type: "GET",
			crossDomain: false,
			dataType: 'json',
			success: function (res) {
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
				setTimeout(e => { $(".loading").removeClass("active"); }, 500)
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
		setTimeout(e => { $(".loading").removeClass("active"); }, 500)

	}
});
function reIntializeReType() {
	$('.requesttype').click(function () {
		let ID = $('.mainservice.active').attr('data-mainserviceid');
		$(".loading").addClass("active");
		$.ajax({
			url: "/Home/GetApplicantData?ServiceID=" + ID, method: "Get", success: function (data) {
				$("#Applicant_Type_ID").html(language == "ar" ? '<option disabled selected value="null">اختر-----------------</option>' : '<option disabled selected value="null">Select-----------------</option>');
				console.log(JSON.parse(data))
				JSON.parse(data).forEach(i => {
					$("#Applicant_Type_ID").append("<option value=" + i.Applicant_Type_ID + ">" + (language == "ar" ? i.Applicant_Type_Name_AR : i.Applicant_Type_Name_EN) + "</option>")
				})
				setTimeout(e => { $(".loading").removeClass("active"); }, 500)
			}
		})
		if (this.getAttribute("data-requesttypenameEN").toLowerCase().includes("inquiry") || this.getAttribute("data-requesttypenameEN").toLowerCase().includes("سؤال")) {
			inquiry = true;
			uploadfiles = [];
			FileNames = [];
		}
		else {
			inquiry = false;
			uploadfiles = [];
			FileNames = [];
			$('#filesNameOther').html("");
		}
	})
	$(".stick").click(function (event) {
		let value = $(event.currentTarget.parentNode.parentNode).attr("id");
		$("#" + value + " .stick").removeClass("active");
		$(event.currentTarget).addClass("active");
		if ($(event.currentTarget).hasClass("follow")) {
			window.location.href = "/Follow/Index";
			return;
		}
		$("#right-arrow").click();
	});
}
$("#right-arrow").click(function () {
	if (incrementValue == 4) {
		if ($(".containt > .row:nth-of-type(" + incrementValue + ")").attr("id") == "personel-data-m"
			&& enterPersonnel == true) {
			////////validate PersonalData//////////////
			let controls = $('.required-field div:nth-of-type(2)').children('select,input');
			let affilte = document.getElementById('Affiliated');
			let error = false;
			controls.push(affilte);
			([...controls]).forEach(e => {
				if (e.value == "" || e.value == null || e.value == "null") {
					$(e).css({ 'border': '2px solid red', 'background': '#ffafaf' });
					if (e.id == "IAUID" && affilte.value == 0) {
						$(e).css({ 'border': 'none', 'background': '#646e85' })
					}
					else
						error = true;
				}
				else {
					$(e).css({ 'border': 'none', 'background': 'white' })
				}
			});
			let e = $('#Mobile')
			if (e.val() == "" || e.val() == null || e.val() == "null" || e.val().length != 9) {
				$(e).css({ 'border': '2px solid red', 'background': '#ffafaf' });
				error = true;
				return;
			}
			else {
				$(e).css({ 'border': 'none', 'background': 'white' })
			}
			e = $('#idNumber')
			if (e.val() == "" || e.val() == null || e.val() == "null" || e.val().length != 10) {
				$(e).css({ 'border': '2px solid red', 'background': '#ffafaf' });
				error = true;
				return;
			}
			else {
				$(e).css({ 'border': 'none', 'background': 'white' })
			}
			if (!error) {
				$(".nav-fill .nav-link").removeClass("active");
				$(".nav-fill .nav-item:nth-of-type(" + incrementValue + ") .nav-link").addClass("active")
				incrementValue++;
			}
		}
	} else if (incrementValue == 6) {
		let error = false;
		if ($("#Request_Type_Id .active ").attr("data-requesttypenameEN").toLowerCase() == "inquiry" || $("#Request_Type_Id .active ").attr("data-requesttypenameEN").toLowerCase() == "سؤال") {
			let controls = $('#provider,#Sub_Services_ID,#Main_Services_ID');
			([...controls]).forEach(e => {
				if (e.value == "" || e.value == null || e.value == "null") {
					$(e).css({ 'border': '2px solid red', 'background': '#ffafaf' });
					error = true;
					return;
				}
				else {
					$(e).css({ 'border': 'none', 'background': 'white' })
				}
			});
			///required files
			if (supporteddocs.length != uploadfiles.length) {
				let e = $('#upload-area');
				e.css({ 'border': '2px solid red', 'background': '#ffafaf' });
				error = true;
			}
			else {
				let e = $('#upload-area');
				e.css({ 'border': 'none', 'background': 'white' })
			}

		}
		else {
			let e = $('#providerOther')
			if (e.val() == "" || e.val() == null || e.val() == "null") {
				$(e).css({ 'border': '2px solid red', 'background': '#ffafaf' });
				error = true;
				return;
			}
			else {
				$(e).css({ 'border': 'none', 'background': 'white' })
			}
		}
		if (error)
			return;
		else {
			$(".nav-fill .nav-link").removeClass("active");
			$(".nav-fill .nav-item:nth-of-type(" + incrementValue + ") .nav-link").addClass("active")
			incrementValue++;
		}


	} else {
		$(".nav-fill .nav-link").removeClass("active");
		$(".nav-fill .nav-item:nth-of-type(" + incrementValue + ") .nav-link").addClass("active")
		incrementValue++;
	}
	console.log(incrementValue);
	if (incrementValue >= 6) {
		$(".nav-fill .nav-item:nth-of-type(" + 5 + ") .nav-link").addClass("active")
	}
	$(".containt > .row").attr("style", "display:none;");
	if (incrementValue == 5) {
		if ($("#Request_Type_Id .active ").attr("data-requesttypenameEN").toLowerCase() == "inquiry" || $("#Request_Type_Id .active ").attr("data-requesttypenameEN").toLowerCase() == "سؤال") {
			$(".containt > .row:nth-of-type(" + incrementValue + ")").attr("style", "display:flex;");
			$(".nav-fill .nav-item:nth-of-type(" + 4 + ") .nav-link").attr({
				"data-slide-to": "Documents-Inquery",
				"data-counter": incrementValue
			});

		} else {
			$(".containt > .row:nth-of-type(" + ++incrementValue + ")").attr("style", "display:flex;");
			$(".nav-fill .nav-item:nth-of-type(" + 4 + ") .nav-link").attr({
				"data-slide-to": "Documents-Other",
				"data-counter": incrementValue
			});
		}

	} else {
		if (incrementValue == 6 && ($("#Request_Type_Id .active ").attr("data-requesttypenameEN").toLowerCase() == "inquiry" || $("#Request_Type_Id .active ").attr("data-requesttypenameEN").toLowerCase() == "سؤال")) {
			$(".containt > .row:nth-of-type(" + ++incrementValue + ")").attr("style", "display:flex;");
			$(".nav-fill .nav-item:nth-of-type(" + 4 + ") .nav-link").attr({
				"data-slide-to": "Documents-Inquery",
				"data-counter": incrementValue
			});

		} else {

			$(".containt > .row:nth-of-type(" + incrementValue + ")").attr("style", "display:flex;");
		}
	}
	if (incrementValue >= 0) {
		$("#left-arrow").attr("style", "visibility:visiable");
		$(".nav-fill").removeAttr("style");
	}

	if (incrementValue >= $(".containt > .row").length) {
		$("#right-arrow").attr("style", "visibility:hidden");
	} else {
		if (incrementValue > 3) {
			$("#right-arrow").attr("style", "visibility:visiable");
		}
	}
	if ($(".containt > .row:nth-of-type(" + incrementValue + ")").attr("id") == "personel-data-m"
	) {
		enterPersonnel = true
	}
	else if (($(".containt > .row:nth-of-type(" + incrementValue + ")").attr("id") == "Documents-Inquery"
		&& enterDocuments == false)) {
		enterDocuments = true;
		incrementValue++;

	} else if (($(".containt > .row:nth-of-type(" + incrementValue + ")").attr("id") == "Documents-Inquery"
		&& enterDocuments == true)) {
		incrementValue++;

	} else if (($(".containt > .row:nth-of-type(" + incrementValue + ")").attr("id") == "Documents-Other"
		&& enterDocumentOther == false)) {
		enterDocumentOther = true;
	}
	else if ($(".containt > .row:nth-of-type(" + incrementValue + ")").attr("id") == "Confirmation-m") {
		GeneratePdfData();
		// enterpdfpreview = true;
	}
});

$("#left-arrow").click(function () {
	incrementValue--;
	if (incrementValue == 6 || incrementValue == 5) {
		if ($("#Request_Type_Id .active ").attr("data-requesttypenameEN").toLowerCase() == "inquiry" || $("#Request_Type_Id .active ").attr("data-requesttypename").toLowerCase() == "سؤال") {
			incrementValue = incrementValue - 1;
		}
	}
	if (incrementValue == 5) {
		if ($("#Request_Type_Id .active ").attr("data-requesttypenameEN").toLowerCase() != "inquiry") {
			if ($("#Request_Type_Id .active ").attr("data-requesttypename").toLowerCase() != "سؤال") {
				incrementValue = incrementValue - 1;
			}
		}
	}
	if (incrementValue == 6 /*request data Not inquery == 6*/) {
		$(".nav-fill .nav-link").removeClass("active");
		$(".nav-fill .nav-item:nth-of-type(" + (incrementValue - 2) + ") .nav-link").addClass("active");
	} else {
		$(".nav-fill .nav-link").removeClass("active");
		$(".nav-fill .nav-item:nth-of-type(" + (incrementValue - 1) + ") .nav-link").addClass("active");
	}
	$(".containt > .row").attr("style", "display:none;");
	$(".containt > .row:nth-of-type(" + incrementValue + ")").attr("style", "display:flex;");
	if (incrementValue <= 1) {
		$("#left-arrow").attr("style", "visibility:hidden");
		$(".nav-fill").attr("style", "display:none");
	}
	if (incrementValue > 4) {
		$("#right-arrow").attr("style", "visibility:visiable");
	}
	if (incrementValue <= 3) {
		$("#right-arrow").attr("style", "visibility:hidden");
	}

});
$(".stick").click(function (event) {
	let value = $(event.currentTarget.parentNode.parentNode).attr("id");
	$("#" + value + " .stick").removeClass("active");
	$(event.currentTarget).addClass("active");
	if ($(event.currentTarget).hasClass("follow")) {
		window.location.href = "/Follow/Index";
		return;
	}
	$("#right-arrow").click();
});
$(".nav-fill .nav-link").click(function () {
	console.log($(this).attr("data-counter"));
	if ($(this).attr("data-counter") > incrementValue) {
		alert("you can't select step that you don't reach yet but you can select previous steps");
	} else if ($(this).attr("data-counter") < incrementValue) {
		incrementValue = $(this).attr("data-counter");
		$(".containt > .row").attr("style", "display:none;");
		$("#" + $(this).attr("data-slide-to")).attr("style", "display:flex;")
		$(".nav-fill .nav-link").removeClass("active");
		$(this).addClass("active");
		if (incrementValue <= 2) {
			$("#right-arrow").attr("style", "visibility:hidden");
		} else if (incrementValue > 2) {
			$("#right-arrow").attr("style", "visibility:visable");
		}
	}
});
///////////////////////////////////ServiceType////////////////////////////////////////////
$('.mainservice').click(function (e) {
	let ID = $(this).attr('data-mainserviceid');
	$(".loading").addClass("active");
	$.ajax({
		url: "/Home/GetApplicantData?ServiceID=" + ID, method: "Get", success: function (data) {
			$("#Applicant_Type_ID").html(language == "ar" ? '<option disabled selected value="null">اختر-----------------</option>' : '<option disabled selected value="null">Select-----------------</option>');
			console.log(JSON.parse(data))
			JSON.parse(data).forEach(i => {
				$("#Applicant_Type_ID").append("<option value=" + i.Applicant_Type_ID + ">" + (language == "ar" ? i.Applicant_Type_Name_AR : i.Applicant_Type_Name_EN) + "</option>")
			})
			setTimeout(e => { $(".loading").removeClass("active"); }, 500)
		}
	})
	$(".loading").addClass("active");

	$.ajax({
		url: "/Home/GetRequest?ServiceID=" + ID, method: "Get", success: function (data) {
			$("#Request_Type_Id").html(`
				<div class="col-md-4 col-lg-3 icon-text-logo" style="padding-left: 23px; margin-top:45px">
					${(language == "ar" ? '<img src="/Design/img/ArServiceType.png" style="width:100%" />'
					: '<img src="/Design/img/EnRequestType.png" style="width:100%"/>')} 
				</div>`);
			let serverpath = $('#serverpath').html()
			JSON.parse(data).forEach(request => {
				$("#Request_Type_Id").append(`
							<div class="col-md-4 col-lg-3">
								<div class="stick requesttype" data-requesttypeid="${request.ID}" data-requesttypenameEN="${request.Name_EN}" data-requesttypename="${(language == "ar" ? request.Name_AR : request.Name_EN)}">
									<img src=${(serverpath + "/" + request.Image_Path)} data-bs-toggle="tooltip" data-bs-placement="top"
										 data-bs-custom-class="beautifier"
										 title="Please include your IAU ID number, whether it is the student ID number or your ID job number, following by the Password.">
									<p>${(language == "ar" ? request.Name_AR : request.Name_EN)}</p >
								</div >
							</div >
					`)
			})

			reIntializeReType();
			if (Redirect)
				$(`[data-requesttypeid='${RedirectReqType}']`).addClass("active").click()
			setTimeout(e => { $(".loading").removeClass("active"); }, 500)
		}
	})
})

///////////////////////////////////////////////////////////////////////////////
let dropArea = document.getElementById('drop-area');
let Supportedfilename = "";
$("#drop-area-other").click(function () {
	$("#filelistOther").click();
});
$("#drop-area").click(function () {
	$("#filelistSupportive").click();
});
['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
	dropArea.addEventListener(eventName, preventDefaults, false)
});

dropArea.addEventListener('drop', handleDrop, false);

function preventDefaults(e) {
	e.preventDefault()
	e.stopPropagation()
}

function handleDrop(e) {
	let dt = e.dataTransfer
	let files = dt.files
	HandelDragAndDrop(files)
}

let uploadfiles = [];
let FileNames = [];
var fileData = new FormData();

function handleFiles(files) {
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
	if (inquiry) {
		let sum = 0;
		//$('#filesNameDrop').html("");
		DropedFile = files;
		if (Math.floor((sum / 1024) / 1024 > 20)) {
			alert("max files size 20MB")
		} else {
			([...files]).forEach(function (file) {
				DropedFile.push(file)
				counter++;
				FileNames.push(file.name);
				$('#filesNameDrop').append("<div class='col-md-6 fileshow' id='support-doc" + counter + "'>" + file.name.slice(0, 7) + ".. \t (" + Math.ceil(file.size / 1024) + " kb) <meter min=1 max=10 value=10></meter> <i class='far fa-times-circle' onclick='deleteFileSupport(\"support-doc" + counter + "\")'></i></div>")
			});
		}
	}
	else {
		let sum = 0;
		//$('#filesNameDropOther').html("");
		if (Math.floor((sum / 1024) / 1024 > 20)) {
			alert("max files size 20MB")
		} else {
			([...files]).forEach(function (file) {
				DropedFile.push(file)
				FileNames.push(file.name)
				$('#filesNameDropOther').append("<div class='col-md-6 fileshow' id='support-doc" + counter + "'>" + file.name.slice(0, 7) + ".. \t (" + Math.ceil(file.size / 1024) + " kb) <meter min=1 max=10 value=10></meter> <i class='far fa-times-circle' onclick='deleteFileSupport(\"support-doc" + counter + "\")'></i></div>")
			});
		}
		console.log(DropedFile)
	}
}
function deleteFileSupport(id) {
	$('#filesNameDropOther ' + id).remove();
}
var saverequest_Clicked = false;
let data = null;
function sendSMS() {
	$("#saveRequestBTN").html("<img src='././Design/img/spinner1.gif' style='width: 53px;'/>");
	data = serialiazeForm();
	$(".loading").addClass("active");

	$.ajax({
		url: `/Home/SendVerification?to=${data.Personel_Data.Mobile}`,
		type: "GET",
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
				console.log(uploadfiles[j].File.name)
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
	if (true) {
		if (!saverequest_Clicked) {
			saverequest_Clicked = true;
			fileData = new FormData();
			if (inquiry)
				PrepareFiles();
			[...DropedFile].forEach(e => {
				fileData.append(e.name, e)
			})
			fileData.append('request_Data', JSON.stringify(data));
			fileData.append('base64File', document.getElementById('SignaturePDF').innerHTML);
			fileData.append('code', requestCode);
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
						document.getElementById('MainBody').innerHTML = language == "ar" ?
							`<div class= "row" >
										<div class="success">
											<span>لقد تم ارسال طلبك بنجاح، </span><br />
											<span> وسيتم إرسال كود الطلب الخاص بكم لمتابعة طلبكم عن طريق الرسائل النصيه</span>
										</div>
										<div class="col-md-4" style="padding: 25px; text-align: center;width: 100%;">
											<a href="" class="btn" id="Okaybutton">موافق</a>
										</div>
									</div > `
							: `<div class= "row" >
										<div class="success">
											<span>Your request has been sent successfully. You will soon receive the </span><br />
											<span>TRACKING NUMBER and the related link to follow your request via SMS </span>
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
					console.log(err)
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
}

function AssignCity(data) {
	data.forEach(function (element) {
		CityComponentSelect += "<option value=" + element.City_ID + ">" + (language == "ar" ? element.City_Name_AR : element.City_Name_EN) + "</option>"
	});
	CityComponentSelect += "</select>"
}

$("#Required_Fields_Notes_Other").keyup(function () {
    $("#text-area-counter_Other").text($(this).val().length + "/300")
    if ($(this).val()) {
        $(".insideTextArea").hide();
        $(".insideTextAreaCounter").show()
    } else if (!$(this).val()) {
        $(".insideTextArea").show();
        $(".insideTextAreaCounter").hide()
    }
});

$("#Required_Fields_Notes").keyup(function () {
	$("#text-area-counter").text($(this).val().length + "/300")
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
		method: "Get",
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
		method: "Get",
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
		method: "Get",
		success: function (result) {
			if (result != null) {
				let data = JSON.parse(result)
				SubServices = data;
				$("#Sub_Services_ID").removeAttr("disabled");
				$("#Sub_Services_ID").html(language == "ar" ? "<option disabled selected value='null'>اختر-----------------</option>" : "<option disabled selected value='null'>Select-----------------</option>")

				data.forEach(function (element) {
					$("#Sub_Services_ID").append("<option value=" + element.ID + ">" + (language == "ar" ? element.Name_AR : element.Name_EN) + "</option>")
				});
				$("#filesName").html("")
				supporteddocs = SubServices[0].Docs
				SubServices[0].Docs.forEach(function (element) {
					let Name = language == "ar" ? element.Name_AR : element.Name_EN
					$("#filesName").append(`
										<div class= "col-lg-4 col-md-6 col-sm-6" style = "margin-bottom:10px;padding:0px;4px;" >
												<div style="background-color: #f6f6f6;display: inline; padding: 0px 3px;border-radius: 5px;">
													<span style="font-size:14px;display:inline-block;width:70%" title="${Name}" id="FileDefault${element.ID}">${Name.length > 17 ? Name.substring(0, 13) + "..." : Name}</span>
													<span style="display:none padding:2px 0px" id="FileName${element.ID}"></span>
													<input type="file" style="display: none;" id="uploadFileDialog${element.ID}" onchange="handleFiles(this.files)" />
													<i class="fa fa-upload" id="fileUpload${element.ID}" style="padding:0px 2px;color:gray" name="${element.ID}" data-RequiredName="${element.Name}"></i>
													<i class="fa fa-trash" id="fileRemove${element.ID}" style="display:none" name="${element.ID}" data-RequiredName="${element.Name}"></i>
												</div>
                                        </div >
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
	Docs.Docs.forEach(function (element) {
		let Name = language == "ar" ? element.Name_AR : element.Name_EN
		$("#filesName").append(`
										<div class= "col-lg-4 col-md-6 col-sm-6" style = "margin-bottom:10px;padding:0px;4px;" >
											<div style="background-color: #f6f6f6;display: inline; padding: 0px 3px;border-radius: 5px;">
												<span style="font-size:14px;display:inline-block;width:70%" title="${Name}" id="FileDefault${element.ID}">${Name.length > 17 ? Name.substring(0, 13) + "..." : Name}</span>
												<span style="display:none padding:2px 0px" id="FileName${element.ID}"></span>
												<input type="file" style="display: none;" id="uploadFileDialog${element.ID}" onchange="handleFiles(this.files)" />
												<i class="fa fa-upload" id="fileUpload${element.ID}" style="padding:0px 2px;color:gray" name="${element.ID}" data-RequiredName="${element.Name}"></i>
												<i class="fa fa-trash" id="fileRemove${element.ID}" style="display:none" name="${element.ID}" data-RequiredName="${element.Name}"></i>
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
		method: "Get",
		success: function (result) {
			$("#EFormsView").html("");
			if (result != null) {
				let data = JSON.parse(result)
				data.forEach(function (element) {
					$("#EFormsView").append(`
									<div class= "col-lg-4 col-md-6 col-sm-6" style = "margin-bottom:5px" >
										<div class="row icon-container" style="padding:0px;margin:0px">
											<div class="col-sm-4" style="padding:0px">
												<img src="/Design/img/reader.png" style="width:25px" />
											</div>
											<div class="col-sm-8">
												<a style="padding:0px" href="https://mm.iau-bsc.com/${element.Url}">${(language == 'ar' ? element.Name_AR : element.Name_EN)}</a>
											</div>
										</div>
									</div >
				`
					)
				});


			}
			setTimeout(e => { $(".loading").removeClass("active"); }, 500)

		}
	});

}

function serialiazeForm() {
	let Data = {
		Unit_ID: $('#provider  option:selected').val() == "null" ? null : $('#provider  option:selected').val(),
		provider_Name: $('#provider  option:selected').text(),
		Sub_Services_ID: $('#Sub_Services_ID  option:selected').val() == "null" ? null : $('#Sub_Services_ID  option:selected').val(),
		Sub_Services_Name: $('#Sub_Services_ID  option:selected').text(),
		Required_Fields_Notes: $('#Required_Fields_Notes').val() == "null" ? null : $('#Required_Fields_Notes').val(),
		Service_Type_ID: document.getElementsByClassName('stick active mainservice')[0].getAttribute('data-mainserviceid'),
		Service_Type_Name: document.getElementsByClassName('stick active mainservice')[0].getAttribute('data-mainservicename'),
		Request_Type_ID: document.getElementsByClassName('stick active requesttype')[0].getAttribute('data-requesttypeid'),
		Request_Type_Name: document.getElementsByClassName('stick active requesttype')[0].getAttribute('data-requesttypename'),
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
	return Data;
}

function SerializeGenratePDF() {
	let Data = {
		Affiliated: $("#Affiliated option:selected").text(),
		provider_Name: $('#provider  option:selected').text(),
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
		file_names: Object.create(FileNames)
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
		't-postal': '   الرقم البريدي   ',
		't-contactinfo': '   معلومات الاتصال   ',
		't-email': '   البريد الالكتروني   ',
		't-phone': '   رقم الهاتف   ',
		't-attachment': '    المرفقات    ',
	}
	var en = {
		't-summary': 'Summary Of Request',
		't-serviceType': 'Service Type',
		't-reqtype': 'Request Type',
		't-personalData': 'Personal Data',
		't-genralInfo': 'General Information',
		't-IAUAff': 'IAU Affliated',
		't-applicanttype': 'Applicant Type',
		't-firstname': 'First Name',
		't-middlename': 'Middle Name',
		't-lastname': 'Last Name',
		't-nationalty': 'Nationalty',
		't-country': 'Country',
		't-iddoc': 'ID Document',
		't-idnumber': 'ID Number',
		't-address': 'Address Information',
		't-city': 'City',
		't-region': 'Region',
		't-country': 'Country',
		't-postal': 'Postal Code',
		't-contactinfo': 'Conatct Information',
		't-email': 'Email',
		't-phone': 'Mobile Phone',
		't-attachment': 'Attachments',
	}
	let Form = SerializeGenratePDF();
	let FilesDiv = ""
	Form["file_names"].splice(',').forEach(e => { FilesDiv += e == "," ? "" : "<p style='margin:0 6px;direction:ltr;text-align:center;color:green !important'>" + e + "</p>" })
	document.getElementById('padf').innerHTML = `
	<div style = "padding: 15px;display: inline-flex;justify-content: space-between;width: 100%;" >
	<img src="../Design/img/MousLogo2.png">
		<img src="../Design/img/VisionLogo.png">
												</div>
		<table class='container' id="PDFTable" style="width: 100%;padding: 15px;">
			<tbody>
				<tr>
					<th class="boldtitle" key="t-summary"></th>
				</tr>
				<tr class="row">
					<th class="col-6" key="t-serviceType"></th>
					<th class="col-6">${Form["Service_Type_Name"]}</th>
				</tr>
				<tr class="row">
					<th class="col-6" key="t-reqtype"></th>
					<th class="col-6">${Form["Request_Type_Name"]}</th>
				</tr>
				<tr class="row">
					<th class="boldtitle" key="t-personalData"></th>
				</tr>
				<tr class="row">
					<th class="boldtitle" key="t-genralInfo"></th>
				</tr>
				<tr class="row">
					<th class="col-6" key="t-IAUAff"></th>
					<th class="col-6">${Form["Affiliated"]}</th>
				</tr>
				<tr class="row">
					<th class="col-6" key="t-applicanttype"></th>
					<th class="col-6">${Form["Applicant_Type_Name"]}</th>
				</tr>
				<tr class="row">
					<th class="col-6" key="t-firstname"></th>
					<th class="col-6">${Form["first_Name"]}</th>
				</tr>
				<tr class="row">
					<th class="col-6" key="t-middlename"></th>
					<th class="col-6">${Form["middle_Name"]}</th>
				</tr>
				<tr class="row">
					<th class="col-6" key="t-lastname"></th>
					<th class="col-6">${Form["last_Name"]}</th>
				</tr>
				<tr class="row">
					<th class="boldtitle" key="t-nationalty"></th>
				</tr>
				<tr class="row">
					<th class="col-6" key="t-nationalty"></th>
					<th class="col-6">${Form["Nationality_Name"]}</th>
				</tr>
				<tr class="row">
					<th class="col-6" key="t-country"></th>
					<th class="col-6">${Form["Country_Name"]}</th>
				</tr>
				<tr class="row">
					<th class="col-6" key="t-iddoc"></th>
					<th class="col-6">${Form["ID_Document_Name"]}</th>
				</tr>
				<tr class="row">
					<th class="col-6" key="t-idnumber"></th>
					<th class="col-6">${Form["Document_Number"]}</th>
				</tr>
				<tr class="row">
					<th class="boldtitle" key="t-address"></th>
				</tr>
				<tr class="row">
					<th class="col-6" key="t-city"></th>
					<th class="col-6">${Form["CityAndRegion"]}</th>
				</tr>
				<tr class="row">
					<th class="col-6" key="t-region"></th>
					<th class="col-6">${Form["Region_Postal_Code_2"]}</th>
				</tr>
				<tr class="row">
					<th class="col-6" key="t-country"></th>
					<th class="col-6">${Form["Region_Postal_Code_1"]}</th>
				</tr>
				<tr class="row">
					<th class="col-6" key="t-postal"></th>
					<th class="col-6">${Form["postal"]}</th>
				</tr>
				<tr>
					<th class="boldtitle" key="t-contactinfo"></th>
				</tr>
				<tr class="row">
					<th class="col-6" key="t-email"></th>
					<th class="col-6">${Form["Email"]}</th>
				</tr>
				<tr class="row">
					<th class="col-6" key="t-phone"></th>
					<th class="col-6">${Form["Mobile"]}</th>
				</tr>
				<tr>
					<th class="boldtitle" key="t-attachment"></th>
				</tr>
				<tr class="row" style="display:inline-flex">
					<th class="col-12 FilesINPDF">${FilesDiv}</th>
				</tr>
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
		$(`th[key='${key}']`).css({ 'width': '50%' })
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

let isAffilate = 0;

function AffiliatedState() {
	isAffilate = parseInt($("#Affiliated option:selected").val());
	if (isAffilate == 1) {
		$("#IAUID").removeAttr("disabled");
		if (!Redirect) {
			localStorage.setItem("ret", document.getElementsByClassName('stick active requesttype')[0].getAttribute('data-requesttypeid'))
			localStorage.setItem("mst", document.getElementsByClassName('stick active mainservice')[0].getAttribute('data-mainserviceid'))
			$("#RedirectBTN").click();
		}

	} else if (($("#Affiliated option:selected").text()).toLowerCase()) {
		$("#IAUID").attr("disabled", "disabled");
	}
}

function CountryState() {
	$(".loading").addClass("active");

	let ID = $("#City_Country_2 option:selected").val()
	$.ajax({
		url: "/Home/GetCityRegion?CID=" + ID, method: "Get", success: function (datxa) {
			let data = JSON.parse(datxa)
			if (data.Regions.length != 0) {
				isSaudi = 1;
				Cities = data.City;
				let id = data.Regions[0].ID;
				CityComponentSelect = `<select id="City_Country_1" name="City_Country_1">`
				RegionComponentSelect = `<select id="Region_Postal_Code_1" name="Region_Postal_Code_1">`
				data.Regions.forEach(function (element) {
					RegionComponentSelect += "<option value=" + element.Region_ID + ">" + (language == "ar" ? element.Region_Name_AR : element.Region_Name_EN) + "</option>"
				});
				RegionComponentSelect += "</select>"
				AssignCity(Cities.filter(q => q.SubID == id));
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
	$("#Required_Fields_Notes-counter").text($(this).val().length + "/300")
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

$("#ResendVerificationCode").click(function() {
    $("#ResendVerificatio   nCode").attr("disabled", "disabled");
    if (language == "ar") {
        $("#ResendVerificationCode").html("يمكنك اعادة ارسال الكود بعد <span id='downConter'>(30)</span> ثانية");
    } else {
        $("#ResendVerificationCode").html("You Can Resend Code After <span id='downConter'>(30)</span> Second");
    }
    counter = 30;
    var x = setInterval(function() {
        counter--;
        document.getElementById("downConter").innerHTML = "(" + counter + ")";
        if (counter == 0) {
            $("#ResendVerificationCode").removeAttr("disabled");
            if (language == "ar") {
                $("#ResendVerificationCode").html("أعد ارسال كود التحقق");
            } else {
                $("#ResendVerificationCode").html("Resend Verification Code");
            }
            clearInterval(x);
        }
    }, 1000);
});