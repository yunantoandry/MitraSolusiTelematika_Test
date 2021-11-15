
$(function () {
    $('[id*=gv]').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
        "responsive": true,
        "sPaginationType": "full_numbers",
        "lengthMenu": [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]]
        //stateSave: true

    });
});
$(function () {
    $('.js-example-basic-single').select2();
});
