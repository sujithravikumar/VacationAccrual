$(document).ready(function(){
    $("#periodsTbl td:nth-child(4)").each(function () {
        if (parseInt($(this).text()) >= 120) {
            $(this).addClass('red-yellow');
        }
    });
});

$(document).ready(function(){
    $("#periodsTbl input").change(function(){
        var accural;
        var take;
        var balance;
        $("#periodsTbl tr").each(function (index) {
            take = parseFloat($(this).find(".take").val());
            if(index > 0) {
                if(index == 1) {
                    accural = parseFloat($(this).find(".accural").text());
                    balance = parseFloat($(this).find(".balance").text());
                    balance = balance - take;
                }
                else {
                    balance = balance + accural - take;
                }
                $(this).find(".balance").text(balance.toFixed(2));
            }
        });
        $("#periodsTbl td:nth-child(4)").each(function () {
            if (parseInt($(this).text()) >= 120) {
                $(this).addClass('red-yellow');
            }
            else {
                $(this).removeClass('red-yellow');
            }
        });
    });
});