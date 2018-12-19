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
            $(this).find(".take").addClass('takeHours');
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
            $(this).find(".take").addClass('takeHours');
        }
        else {
            $(this).find(".take").removeClass('takeHours');
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