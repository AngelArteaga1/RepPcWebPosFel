$(function cargarTabla() {
    var e = {};
    $(".table-edits tr").editable({
        dropdowns: {
            gender: ["Male", "Female"]
        }, edit: function (t) {
            $(".edit i", this).removeClass("fa-pencil-alt").addClass("fa-save").attr("title", "Save")
            $("td[data-field=id] input", this)[0].setAttribute("readonly", "true");
            $("td[data-field=id] input", this)[0].setAttribute("class", "form-control");
        }, save: function (t) {
            $(".edit i", this).removeClass("fa-save").addClass("fa-pencil-alt").attr("title", "Edit"),
                this in e && (e[this].destroy(), delete e[this])
        }, cancel: function (t) {
            $(".edit i", this).removeClass("fa-save").addClass("fa-pencil-alt").attr("title", "Edit"),
                this in e && (e[this].destroy(), delete e[this])
        }
    })
});