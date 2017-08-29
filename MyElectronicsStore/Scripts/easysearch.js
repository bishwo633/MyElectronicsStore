$("#simplesearch").click(function () {
    $(".advanced").hide();
    $(".simple").show();
});
$("#advancedsearch").click(function () {
    $(".simple").hide();
    $(".advanced").show();
});
//load simple search by default
$("#simplesearch").click();