function ReIntalizeEformReadOnlyListener() {
	$(document).on("click", ".eform-readonly-btn", function () {
		$(".loading").addClass("active");
		$.ajax({
			url: `/Home/GetEformSummary/${$(this).data("id")}`, method: "Post", data: {
				__RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
			}, success: function (dd) {
				$('#EformReadOnly').html(dd)
				$("#Eform-UnitName", $('#EformReadOnly')).text($('#provider option:selected').text())
				var AutoFill = $("tr[data-id^='E']", $('#EformReadOnly'));
				([...AutoFill]).forEach(i => {
					let ref = $('td label', $(i))
					ref.text($('#' + ref.data('ref')).val())
				})
				Answer.forEach(i => {
					if (i["T"] == "C" || i["T"] == "R") {
						let data = JSON.parse(language == "ar" ? i["Value"] : i["Value_En"])
						let html = ''
						data.forEach(s => {
							html += `<label>${s}</label>`
						})
						$(`tr[data-id='${i["T"]}-${i["Question_ID"]}'] td`, $('#EformReadOnly')).html(html)
					}
					else
						$(`tr[data-id='${i["T"]}-${i["Question_ID"]}'] td`, $('#EformReadOnly')).text(language == "ar" ? i["Value"] : i["Value_En"]);
				});
				$('#eform-ServType', $('#EformReadOnly')).text($('.active.mainservice p').text())
				$('#eform-reqType', $('#EformReadOnly')).text($('.active.requesttype p').text())
				$('#EformReadOnly').modal('toggle')
				setTimeout(e => { $(".loading").removeClass("active"); }, 500)
			}
		})
	});
}
