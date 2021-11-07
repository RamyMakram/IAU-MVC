﻿let Form_Answare = [];
function ReIntalizeEformListener() {
    Answare = [];
    $(document).on("click", ".eform-btn", function () {
        if ($(this).hasClass("filled"))
            return;
        $(".loading").addClass("active");
        $.ajax({
            url: `/Home/GetEform/${$(this).data("id")}`, method: "Post", data: {
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            }, success: function (dd) {
                $('#Eform').html(dd)
                var AutoFill = $("tr[data-id^='E']", $('#Eform'));
                ([...AutoFill]).forEach(i => {
                    let ref = $('td label', $(i))
                    ref.text($('#' + ref.data('ref')).val())
                })
                new bootstrap.Modal(document.getElementById('Eform'), {
                    keyboard: false,
                    backdrop: 'static'
                }).show()
                setTimeout(e => { $(".loading").removeClass("active"); }, 500)
                $('.saveeform').click(w => {
                    let formid = $('.modal-title', $('#Eform')).data('id');
                    var QTY = $('tr[data-id]', $('#Eform'));
                    $('tr[data-id] th label', $('#Eform')).css({ 'color': '#212529' });
                    let Error = false;
                    ([...QTY]).forEach(s => {
                        var frm_qt = $(s).data('id')
                        var name = $(s).data('name').split('-')
                        var req = $(s).data('required') == "True"
                        switch (frm_qt.split('-')[0]) {
                            case 'E':
                                var val = $('td label', $(s)).text()
                                AddOrUpdate({ EFromID: formid, Question_ID: frm_qt.split('-')[1], Value: val, Value_En: val, Name: name[0], Name_En: name[1] });
                                break;
                            case "C":
                            case "R":
                                var values = [];
                                var values_En = [];
                                ([...$('input:checked', $(s))]).forEach(is => {
                                    var val = $(is).data("value").split('-')
                                    values.push(val[0])
                                    values_En.push(val[1])
                                })
                                if (req && (values.length == 0 || values_En.length == 0)) {
                                    $('th label', $(s)).css({ 'color': 'red' })
                                    Error = true;
                                    return;
                                }
                                AddOrUpdate({ EFromID: formid, Question_ID: frm_qt.split('-')[1], Value: JSON.stringify(values), Value_En: JSON.stringify(values_En), Name: name[0], Name_En: name[1] });
                                break;
                            case "I":
                            case "D":
                                var val = $('td input', $(s)).val()
                                if (req && val.length == 0) {
                                    $('th label', $(s)).css({ 'color': 'red' })
                                    Error = true;
                                    return;
                                }
                                AddOrUpdate({ EFromID: formid, Question_ID: frm_qt.split('-')[1], Value: val, Value_En: val, Name: name[0], Name_En: name[1] });
                                break;
                            default:
                        }
                    })
                    if (!Error) {
                        $(`.eform-btn[data-id="${formid}"]`).addClass("filled")
                        console.log(Form_Answare)
                        Form_Answare.forEach(i => {
                            Answer.push(i)
                        })
                        $('#Eform').modal('toggle');
                    }
                })
            }
        })
    });
}
function AddOrUpdate(model) {
    let index = Form_Answare.findIndex(q => q.EFromID == model.EFromID && q.Question_ID == model.Question_ID)
    if (index == -1)
        Form_Answare .push(model);
    else {
        Form_Answare[index].Value = model.Value
        Form_Answare[index].Value_En = model.Value_En
        Form_Answare[index].Name = model.Name
        Form_Answare[index].Name_En = model.Name_En
    }
}