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
    toastr.options = {
      "closeButton": false,
      "debug": false,
      "newestOnTop": false,
      "progressBar": false,
      "positionClass": "toast-top-right",
      "preventDuplicates": false,
      "onclick": null,
      "showDuration": "300",
      "hideDuration": "1000",
      "timeOut": "2000",
      "extendedTimeOut": "1000",
      "showEasing": "swing",
      "hideEasing": "linear",
      "showMethod": "fadeIn",
      "hideMethod": "fadeOut"
    }
    if (!$("#ReceiveEmailReminder").is(":checked")) {
        $('#ReceiveDaysBefore').prop('disabled', true);
    }
    $("input[type='checkbox']").change(function() {
        $('#ReceiveDaysBefore').prop('disabled', true);
        if(this.checked) {
            $('#ReceiveDaysBefore').prop('disabled', false);
        }
    });    
});

$(document).ready(function() {
    $("#periodsTbl tr").each(function (index) {
        if (index == 1) {
            $(this).find(".take").attr('readonly', true);
            $(this).find(".take").addClass('input-disabled');
        }
    });
});

$(document).ready(function() {
    $("#periodsTbl td:nth-child(3)").each(function () {
        if (parseFloat($(this).find(".take").val()) > 0) {
            $(this).find(".take").addClass('takeOff');
        }
    });

    $("#periodsTbl td:nth-child(5)").each(function () {
        if (parseFloat($(this).text()) > 0) {
            $(this).addClass('forfeitWarn');
        }
    });
});

function formatTblCells() {
    $("#periodsTbl td:nth-child(3)").each(function () {
        if (parseFloat($(this).find(".take").val()) > 0) {
            $(this).find(".take").addClass('takeOff');
        }
        else {
            $(this).find(".take").removeClass('takeOff');
        }
    });
    $("#periodsTbl td:nth-child(5)").each(function () {
        if (parseFloat($(this).text()) > 0) {
            $(this).addClass('forfeitWarn');
        }
        else {
            $(this).removeClass('forfeitWarn');
        }
    });
}