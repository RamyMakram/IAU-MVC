function ReIntalizeEformReadOnlyListener() {
    $(document).on("click", ".eform-readonly-btn", function () {
        $(".loading").addClass("active");
        $.ajax({
            url: `/Home/GetEformSummary?ID=${$(this).data("id")}&uid=${$((inquiry ? "#provider" : "#providerOther") + '  option:selected').val() == "null" ? null : $((inquiry ? "#provider" : "#providerOther") + '  option:selected').val()}`, method: "Post", data: {
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            }, success: function (dd) {
                $('#EformReadOnly').html(dd)
                $("#Eform-UnitName", $('#EformReadOnly')).text($('#provider option:selected').text())
                var AutoFill = $("[data-id^='E']", $('#EformReadOnly'));
                ([...AutoFill]).forEach(i => {
                    let ref = $('div.form-control label', $(i))
                    if ($('#' + ref.data('ref')).prop('nodeName') == 'SELECT')
                        ref.text($('#' + ref.data('ref') + " option:selected").text())
                    else
                        ref.text($('#' + ref.data('ref')).val())
                })
                Answer.forEach(i => {
                    if (i["T"] == "C" || i["T"] == "R") {
                        let data = JSON.parse(language == "ar" ? i["Value"] : i["Value_En"])
                        let html = ''
                        let $coun = 0;
                        data.forEach(s => {
                            html += `<label class="mx-2">${($coun != 0 ? "," : "") + s}</label> `
                            $coun++;
                        })
                        $(`[data-id='${i["T"]}-${i["Question_ID"]}'] div.data-here`, $('#EformReadOnly')).html(html)
                    }
                    else
                        $(`[data-id='${i["T"]}-${i["Question_ID"]}'] label.data-here`, $('#EformReadOnly')).html(language == "ar" ? i["Value"] : i["Value_En"])
                });
                $('#eform-ServType', $('#EformReadOnly')).text($('.active.mainservice p').text())
                $('#eform-reqType', $('#EformReadOnly')).text($('.active.requesttype p').text())
                $('#EformReadOnly').modal('toggle')
                setTimeout(e => { $(".loading").removeClass("active"); }, 500)
            }
        })
    });
}
