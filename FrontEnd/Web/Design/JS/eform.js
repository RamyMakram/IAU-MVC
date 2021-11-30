let Form_Answare = [];
function ReIntalizeEformListener() {
    Answare = [];
    $(document).on("click", ".eform-btn", function () {
        $(".loading").addClass("active");
        let filled = $(this).hasClass("filled");
        $.ajax({
            url: `/Home/GetEform/${$(this).data("id")}`, method: "Post", data: {
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            }, success: function (dd) {
                $('#Eform').html(dd)
                var AutoFill = $("[data-id^='E']", $('#Eform'));
                ([...AutoFill]).forEach(i => {
                    let ref = $('div label', $(i))
                    if ($('#' + ref.data('ref')).prop('nodeName') == 'SELECT')
                        ref.text($('#' + ref.data('ref') +" option:selected").text())
                    else
                        ref.text($('#' + ref.data('ref')).val())
                })
                $('#Eform').modal('toggle');
                if (filled) {
                    let formid = $('.modal-title', $('#Eform')).data('id');
                    var data = Answer.filter(q => q.EFromID == formid);
                    var QTY = $('[data-id]', $('#Eform'));
                    $('[data-id] label', $('#Eform')).css({ 'color': '#212529' });
                    ([...QTY]).forEach(s => {
                        var frm_qt = $(s).data('id').split('-')
                        let T = frm_qt[0];
                        let QTyID = frm_qt[1];
                        switch (T) {
                            case "C":
                            case "R":
                                var item = data.find(q => q.Question_ID == QTyID)
                                var values = JSON.parse(item.Value_En);
                                ([...$('input', $(s))]).forEach(is => {
                                    var val = $(is).data("value").split('-')
                                    if (values.findIndex(q => q == val[1]) != -1)
                                        $(is).attr('checked', 'checked');
                                })
                                break;
                            case "I":
                            case "D":
                                $(' input', $(s)).val(data.find(q => q.Question_ID == QTyID).Value_En)
                                break;
                            default:
                        }
                    })
                }
                setTimeout(e => { $(".loading").removeClass("active"); }, 500)
                $('.saveeform').click(w => {
                    let formid = $('.modal-title', $('#Eform')).data('id');
                    var QTY = $('[data-id]', $('#Eform'));
                    $('[data-id] label', $('#Eform')).css({ 'color': '#212529' });
                    let Error = false;
                    ([...QTY]).forEach(s => {
                        var frm_qt = $(s).data('id')
                        var name = $(s).data('name').split('-')
                        var req = $(s).data('required') == "True"
                        let T = frm_qt.split('-')[0];
                        switch (T) {
                            case 'E':
                                var val = $(' label', $(s)).text()
                                AddOrUpdate({ EFromID: formid, Question_ID: frm_qt.split('-')[1], T, Value: val, Value_En: val, Name: name[0], Name_En: name[1] });
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
                                    $('label', $(s)).css({ 'color': 'red' })
                                    Error = true;
                                    return;
                                }
                                AddOrUpdate({ EFromID: formid, Question_ID: frm_qt.split('-')[1], T, Value: JSON.stringify(values), Value_En: JSON.stringify(values_En), Name: name[0], Name_En: name[1] });
                                break;
                            case "I":
                            case "D":
                                var val = $(' input', $(s)).val()
                                if (req && val.length == 0) {
                                    $(' label', $(s)).css({ 'color': 'red' })
                                    Error = true;
                                    return;
                                }
                                AddOrUpdate({ EFromID: formid, Question_ID: frm_qt.split('-')[1], T, Value: val, Value_En: val, Name: name[0], Name_En: name[1] });
                                break;
                            default:
                        }
                    })
                    if (!Error) {
                        $(`.eform-btn[data-id="${formid}"]`).addClass("filled")
                        Form_Answare.forEach(i => {
                            AddOrUpdateMain(i)
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
        Form_Answare.push(model);
    else {
        Form_Answare[index].Value = model.Value
        Form_Answare[index].Value_En = model.Value_En
        Form_Answare[index].Name = model.Name
        Form_Answare[index].Name_En = model.Name_En
    }
}
function AddOrUpdateMain(model) {
    let index = Answer.findIndex(q => q.EFromID == model.EFromID && q.Question_ID == model.Question_ID)
    if (index == -1)
        Answer.push(model);
    else {
        Answer[index].Value = model.Value
        Answer[index].Value_En = model.Value_En
        Answer[index].Name = model.Name
        Answer[index].Name_En = model.Name_En
    }
}