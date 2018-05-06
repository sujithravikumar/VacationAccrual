$(function () {
    $("#MaxBalance").spinner({
        min: 0,
        max: 200
    });
    $("#Accrual").spinner({
        min: 0,
        max: 20,
        step: 0.01,
        numberFormat: "n"
    });
    $("#Balance").spinner({
        min: 0,
        max: 200,
        step: 0.01,
        numberFormat: "n"
    });
});

$(document).ready(function() {
    $('#periodsTbl').DataTable({searching: false, paging: false, ordering: false, info: false});
});

$(document).ready(function(){
    $("#periodsTbl tr").each(function (index) {
        if (index == 1) {
            $(this).find(".take").attr('readonly', true);
            $(this).find(".take").addClass('input-disabled');
        }
    });
});