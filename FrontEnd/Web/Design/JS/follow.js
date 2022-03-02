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

$(".containt2 input.search").click(function (element) {
    if (!$(element.currentTarget).val()) {
        $(".containt2 input.search").each(element => {
            if (!$(".containt2 input.search:eq(" + element + ")").val()) {
                $(".containt2 input.search:eq(" + element + ")").focus();
                return false;
            }
        });
    }
});
let inputLocatin;
$(".containt2 input.search").keyup(function (element) {

    $(".containt2 input.search").each(elementCount => {
        if (!$(".containt2 input.search:eq(" + elementCount + ")").val()) {
            $(".containt2 input.search:eq(" + elementCount + ")").focus();
            inputLocatin = elementCount;
            return false;
        }
    });

    if (element.key == "Backspace") {
        if (inputLocatin > 0) {
            $(".containt2 input.search:eq(" + inputLocatin-- + ")").val("");
            $(".containt2 input.search:eq(" + inputLocatin-- + ")").focus();
        }
    }

    if (element.key == "Enter") {
        show();
    }
});

function showState() {
    show();
}

function searchByPhone() {
    var myModal = new bootstrap.Modal(document.getElementById('phoneSearch'), {
        keyboard: false
    })
    myModal.show()
}

function show() {
    var requestCode = "";
    $(".containt2 input.search").each(element => {
        requestCode = requestCode + $(".containt2 input.search:eq(" + element + ")").val();
    });
    if (requestCode.trim().length == 13) {
        $.ajax({
            url: "/Follow/FollowRequest", data: {
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(),
                requestCode
            },
            method: "Post",
            success: function (result) {
                let data = JSON.parse(result);
                if (data.success) {
                    if (data.result == null) {
                        alert(language == "ar" ? "!كود خاطئ" : "Wrong Code!")
                        $("#div_Follow").css("display", "none");
                    }
                    else {
                        $(".containt2 .row .col-md-12:nth-of-type(3)").css("display", "block");
                        let isAr = $('#lang').text() == "ar"
                        let request = data.result;
                        $('.state').removeClass('progress-sucess-state')
                        $('.state-' + request.Request.Request_State_ID).addClass('progress-sucess-state')
                        for (var i = 1; i < request.Request.Request_State_ID; i++) {
                            $('.state-' + i).addClass('progress-sucess-state')
                        }

                        $('#spn_shipped').text((isAr ? (request.Request.IsTwasul_OC ? "تواصل" : "مستفيد") : (request.Request.IsTwasul_OC ? "Twasul" : "Mustafid")))
                        $('#spn_location').text((isAr ? request.State.Units.Units_Name_AR : request.State.Units.Units_Name_EN).slice(0, 12))
                        $('#spn_location').attr('title', (isAr ? request.State.Units.Units_Name_AR : request.State.Units.Units_Name_EN))
                        $('#spn_status').text((isAr ? request.Request.Request_State.StateName_AR : request.Request.Request_State.StateName_EN).slice(0, 12))
                        $('#spn_status').attr('title', isAr ? request.Request.Request_State.StateName_AR : request.Request.Request_State.StateName_EN)
                        $('#spn_deliverydate').text(new Date(request.State.ExpireDays).toDateString())

                        $("#div_Follow").css("display", "block");
                    }
                }
                else {
                    alert(language == "ar" ? "!كود خاطئ" : "Wrong Code!")

                }
            }
        });
    }
}