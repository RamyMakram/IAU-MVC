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

$(".containt2 input.search").keyup(function (element) {
    $(".containt2 input.search").each(element => {
        if (!$(".containt2 input.search:eq(" + element + ")").val()) {
            $(".containt2 input.search:eq(" + element + ")").focus();
            return false;
        }
    });

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
        $(".containt2 .row .col-md-12:nth-of-type(3)").css("display", "block");
        //search for the request number
        $.ajax({
            url: "/Follow/FollowRequest",
            data:
            {
                requestCode
            },
            method: "Post",
            success: function (result) {
                if (result.Result == 'OK') {

                    result = result.Options;
                    if (result.requestid == 0) {
                        alert("You enter Invalid Requesd Number!")
                        $("#div_Follow").css("display", "none");
                    }
                    else {
                        $("#div_Follow").css("display", "block");

                        $("#spn_location").html(result.location);
                        $("#spn_status").html(result.status);
                        //  $("#spn_status").html(result.statusId);
                        $("#spn_deliverydate").html(result.deliverydate);
                    }
                }
                else {
                    alert("SomeThing Went Wrong!");
                }
            }
        });
    }
}