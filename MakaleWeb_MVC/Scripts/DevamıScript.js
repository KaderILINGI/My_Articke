
$(function () {

    $('#modal_makale').on('show.bs.modal', function (e) {
        var btn = $(e.relatedTarget);
        mid = btn.data("makaleid");
        $("#modal_makale_body").load("/Makale/MakaleGoster/" + mid)

        /*console.log(mid)*/
    });
});
