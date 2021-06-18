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
		$.ajax({
			url: "/Follow/FollowRequest",
			data:
			{
				requestCode
			},
			method: "Post",
			success: function (result) {
				console.log(result)
				let data = JSON.parse(result);
				if (data.success) {
					if (data.result == null) {
						alert("You enter Invalid Requesd Number!")
						$("#div_Follow").css("display", "none");
					}
					else {
						let isAr = $('#lang').text() == "ar"
						let request = data.result;
						$('.state').removeClass('progress-sucess-state')
						$('.state-' + request.Request.Request_State_ID).addClass('progress-sucess-state')
						for (var i = 1; i < request.Request.Request_State_ID; i++) {
							$('.state-' + i).addClass('progress-sucess-state')
						}
						
						$('#spn_location').text((isAr ? request.State.Units.Units_Name_AR : request.State.Units.Units_Name_EN).slice(0, 12))
						$('#spn_location').attr('title',(isAr ? request.State.Units.Units_Name_AR : request.State.Units.Units_Name_EN))
						$('#spn_status').text((isAr ? request.Request.Request_State.StateName_AR : request.Request.Request_State.StateName_EN).slice(0, 12))
						$('#spn_status').attr('title',isAr ? request.Request.Request_State.StateName_AR : request.Request.Request_State.StateName_EN)
						$('#spn_deliverydate').text(new Date(request.State.ExpireDays).toDateString())

						$("#div_Follow").css("display", "block");
					}
				}
				else {
					alert("SomeThing Went Wrong!");
				}
			}
		});
	}
}