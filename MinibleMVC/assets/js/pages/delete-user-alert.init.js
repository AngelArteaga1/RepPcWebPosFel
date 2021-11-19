!function (t) {
    "use strict";
    function e() { } e.prototype.init = function () {
        t("#sa-params").click(function () {
            Swal.fire({
                title: "Estas seguro?", text: "No hay forma de rehacer esta función!", icon: "warning", showCancelButton: !0, confirmButtonText: "Si, Eliminar!", cancelButtonText: "No, Cancelar!", confirmButtonClass: "btn btn-success mt-2", cancelButtonClass: "btn btn-danger ms-2 mt-2", buttonsStyling: !1
            }).then(function (t) {
                if (t.value) {
                    //SI CONFIRMA
                    Swal.fire({ title: "Eliminado!", text: "El usuario ha sido eliminado.", icon: "success" })
                } else {
                    //SI CANCELA
                    t.dismiss === Swal.DismissReason.cancel && Swal.fire({ title: "Cancelado", text: "El usuario no se ha eliminado :)", icon: "error" })
                }
            })
        })
    }, t.SweetAlert = new e, t.SweetAlert.Constructor = e
}(window.jQuery), function () { "use strict"; window.jQuery.SweetAlert.init() }();