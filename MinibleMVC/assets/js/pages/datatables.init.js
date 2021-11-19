$(document).ready(function () {
    $("#datatable").DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/TablesDatatable/Yogabuni",
            //"url": "/assets/js/prueba.json",
            "type": "POST",
            "datatype": "json"
        },
        "responsivePriority": 1,
        "data": null,
        "columns": [
            { "data": "Name", "name": "Name", "autoWidth": true },
            { "data": "Position", "name": "Position", "autoWidth": true },
            { "data": "Office", "name": "Office", "autoWidth": true },
            { "data": "Age", "name": "Age", "autoWidth": true },
            { "data": "StartDate", "name": "StartDate", "autoWidth": true },
            { "data": "Salary", "name": "Salary", "autoWidth": true },
        ]
    }),
    $("#datatable-buttons").DataTable({
        lengthChange: !1,
        buttons: ["copy", "excel", "pdf", "colvis"]}).buttons().container().appendTo("#datatable-buttons_wrapper .col-md-6:eq(0)"),
    $(".dataTables_length select").addClass("form-select form-select-sm")
});