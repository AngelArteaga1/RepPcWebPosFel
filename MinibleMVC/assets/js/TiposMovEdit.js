var num = 0;

function AgregaConcepto() {
    let Serie = document.getElementById("Serie").value;
    let Secuencia = document.getElementById("Secuencia").value;
    let Correlativo = document.getElementById("Correlativo").value;
    let Automatico = document.getElementById("Automatico").checked;
    let Formato = document.getElementById("Formato").value;
    let Resolucion = document.getElementById("Resolucion").value;
    let Fecha = document.getElementById("Fecha").value;
    let Del = document.getElementById("Del").value;
    let Al = document.getElementById("Al").value;

    //console.log(Automatico);

    //agregamos tabla
    let Tabla = document.getElementById("tablaConceptos");
    let TR = document.createElement("tr");
    let TDSerie = document.createElement("td");
    let TDSecuencia = document.createElement("td");
    let TDCorrelativo = document.createElement("td");
    let TDAutomatico = document.createElement("td");
    let TDFormato = document.createElement("td");
    let TDResolucion = document.createElement("td");
    let TDFecha = document.createElement("td");
    let TDDel = document.createElement("td");
    let TDAl = document.createElement("td");


    TR.appendChild(TDSerie);
    TR.appendChild(TDSecuencia);
    TR.appendChild(TDCorrelativo);
    TR.appendChild(TDAutomatico);
    TR.appendChild(TDFormato);
    TR.appendChild(TDResolucion);
    TR.appendChild(TDFecha);
    TR.appendChild(TDDel);
    TR.appendChild(TDAl);


    TDSerie.innerHTML = Serie;
    TDSecuencia.innerHTML = Secuencia;
    TDCorrelativo.innerHTML = Correlativo;
    if (Automatico) {
        TDAutomatico.innerHTML = "SI";
    } else {
        TDAutomatico.innerHTML = "NO";
    }
    TDFormato.innerHTML = Formato;
    TDResolucion.innerHTML = Resolucion;
    TDFecha.innerHTML = Fecha;
    TDDel.innerHTML = Del;
    TDAl.innerHTML = Al;
    Tabla.appendChild(TR);

    //agregamos hiddens
    let DivConceptos = document.getElementById("divConceptosAdd");
    let HiddenIndex = document.createElement("input");
    let HiddenSerie = document.createElement("input");
    let HiddenSecuencia = document.createElement("input");
    let HiddenCorrelativo = document.createElement("input");
    let HiddenAutomatico = document.createElement("input");
    let HiddenFormato = document.createElement("input");
    let HiddenResolucion = document.createElement("input");
    let HiddenFecha = document.createElement("input");
    let HiddenDel = document.createElement("input");
    let HiddenAl = document.createElement("input");
    let HiddenActualizar = document.createElement("input");

    HiddenIndex.name = "conceptosAdd.Index";
    HiddenIndex.value = num;
    HiddenIndex.type = "hidden";

    HiddenSerie.name = "conceptosAdd[" + num + "].idSerie";
    HiddenSerie.value = Serie;
    HiddenSerie.type = "hidden";
    HiddenSecuencia.name = "conceptosAdd[" + num + "].secuencia";
    HiddenSecuencia.value = Secuencia;
    HiddenSecuencia.type = "hidden";
    HiddenCorrelativo.name = "conceptosAdd[" + num + "].correlativo";
    HiddenCorrelativo.value = Correlativo;
    HiddenCorrelativo.type = "hidden";

    HiddenAutomatico.name = "conceptosAdd[" + num + "].usaCorrelativo";
    HiddenAutomatico.type = "checkbox";
    if (Automatico) {
        HiddenAutomatico.value = "S";
        HiddenAutomatico.checked = true;
    } else {
        HiddenAutomatico.value = "N";
        HiddenAutomatico.checked = false;
    }

    HiddenFormato.name = "conceptosAdd[" + num + "].formatoImpresion";
    HiddenFormato.value = Formato;
    HiddenFormato.type = "hidden";
    HiddenResolucion.name = "conceptosAdd[" + num + "].resolucionNumero";
    HiddenResolucion.value = Resolucion;
    HiddenResolucion.type = "hidden";
    HiddenFecha.name = "conceptosAdd[" + num + "].fechaAutorizacion";
    HiddenFecha.value = Fecha;
    HiddenFecha.type = "hidden";
    HiddenDel.name = "conceptosAdd[" + num + "].res_del";
    HiddenDel.value = Del;
    HiddenDel.type = "hidden";
    HiddenAl.name = "conceptosAdd[" + num + "].res_al";
    HiddenAl.value = Al;
    HiddenAl.type = "hidden";

    HiddenActualizar.name = "conceptosAdd[" + num + "].actualizar";
    HiddenActualizar.value = "N";
    HiddenActualizar.type = "hidden";


    DivConceptos.appendChild(HiddenIndex);
    DivConceptos.appendChild(HiddenSerie);
    DivConceptos.appendChild(HiddenSecuencia);
    DivConceptos.appendChild(HiddenCorrelativo);
    DivConceptos.appendChild(HiddenAutomatico);
    DivConceptos.appendChild(HiddenFormato);
    DivConceptos.appendChild(HiddenResolucion);
    DivConceptos.appendChild(HiddenFecha);
    DivConceptos.appendChild(HiddenDel);
    DivConceptos.appendChild(HiddenAl);
    DivConceptos.appendChild(HiddenActualizar);


    document.getElementById("Serie").value = "";
    document.getElementById("Secuencia").value = "";
    document.getElementById("Correlativo").value = "";
    document.getElementById("Formato").value = "";
    document.getElementById("Resolucion").value = "";
    document.getElementById("Fecha").value = "";
    document.getElementById("Del").value = "";
    document.getElementById("Al").value = "";
    //document.getElementById("Automatico").value = "";

    num++;
}

/* ====================Eliminar Los Detalles de Tipos movimientos ======== */

function Delete(id) {

    //console.log(id);
    Swal.fire({
        title: "Estas seguro?", text: "No hay forma de rehacer esta función!", icon: "warning", showCancelButton: !0, confirmButtonText: "Si, Eliminar!", cancelButtonText: "No, Cancelar!", confirmButtonClass: "btn btn-success mt-2", cancelButtonClass: "btn btn-danger ms-2 mt-2", buttonsStyling: !1
    }).then(async function (t) {
        if (t.value) {
            //SI CONFIRMA                

            //Hacemos una lista de campos ocultos que se eliminaran 
            let DivConceptos = document.getElementById("divConceptosDelete");
            let HiddenIdMovSerie = document.createElement("input");
            let idHtmlMovSerie = "conceptos_" + id + "__idInternoTIposMovimientosSeries";

            //obtenemos los valores del contador y el id del item a eliminar
            let num = document.getElementById("idContador").value;
            let IdModelMovSerie = document.getElementById(idHtmlMovSerie).value;


            HiddenIdMovSerie.name = "conceptosDelete[" + num + "].idInternoTIposMovimientosSeries";
            HiddenIdMovSerie.value = IdModelMovSerie;
            HiddenIdMovSerie.type = "hidden";

            DivConceptos.appendChild(HiddenIdMovSerie);

            //Eliminamos del DOM los elementos
            var nNum = 0;
            var div = document.getElementById("divConceptosEdit" + id);
            var valIteracion = document.getElementById("idIteracion").value;
            if (div != null) {

                for (var i = 0; i < valIteracion; i++) {

                    //Variable String para formar el id 
                    let idHtmlMovSerieIt0 = "conceptos_" + i + "__idInternoTIposMovimientosSeries";
                    let idHtmlMovSerieIt1 = "conceptos_" + i + "__idInternoTiposMovimientos";
                    let idHtmlMovSerieIt2 = "conceptos_" + i + "__idSerie";
                    let idHtmlMovSerieIt3 = "conceptos_" + i + "__correlativo";
                    //let idHtmlMovSerieIt4 = "conceptos_" + i + "__usaCorrelativo";
                    let idHtmlMovSerieIt5 = "conceptos_" + i + "__formatoImpresion";
                    let idHtmlMovSerieIt6 = "conceptos_" + i + "__fechaAutorizacion";
                    let idHtmlMovSerieIt7 = "conceptos_" + i + "__res_del";
                    let idHtmlMovSerieIt8 = "conceptos_" + i + "__res_al";
                    let idHtmlMovSerieIt9 = "conceptos_" + i + "__resolucionNumero";
                    let idHtmlMovSerieIt10 = "conceptos_" + i + "__secuencia";
                    let idHtmlButtonDelete = "buttonDelete" + i;
                    let idHtmlDivConceptos = "divConceptosEdit" + i;

                    //Obtenemos Id
                    let idHtmlMovSerieObj0 = document.getElementById(idHtmlMovSerieIt0);
                    let idHtmlMovSerieObj1 = document.getElementById(idHtmlMovSerieIt1);
                    let idHtmlMovSerieObj2 = document.getElementById(idHtmlMovSerieIt2);
                    let idHtmlMovSerieObj3 = document.getElementById(idHtmlMovSerieIt3);
                    //let idHtmlMovSerieObj4 = document.getElementById(idHtmlMovSerieIt4);
                    let idHtmlMovSerieObj5 = document.getElementById(idHtmlMovSerieIt5);
                    let idHtmlMovSerieObj6 = document.getElementById(idHtmlMovSerieIt6);
                    let idHtmlMovSerieObj7 = document.getElementById(idHtmlMovSerieIt7);
                    let idHtmlMovSerieObj8 = document.getElementById(idHtmlMovSerieIt8);
                    let idHtmlMovSerieObj9 = document.getElementById(idHtmlMovSerieIt9);
                    let idHtmlMovSerieObj10 = document.getElementById(idHtmlMovSerieIt10);
                    let idHtmlButtonDeleteObj = document.getElementById(idHtmlButtonDelete);
                    let idHtmlDivConceptosObj = document.getElementById(idHtmlDivConceptos);

                    if (id != i) {
                        //reasignamos Name                            
                        idHtmlMovSerieObj0.setAttribute("name", "conceptos[" + nNum + "].idInternoTIposMovimientosSeries");
                        idHtmlMovSerieObj1.setAttribute("name", "conceptos[" + nNum + "].idInternoTiposMovimientos");
                        idHtmlMovSerieObj2.setAttribute("name", "conceptos[" + nNum + "].idSerie");
                        idHtmlMovSerieObj3.setAttribute("name", "conceptos[" + nNum + "].correlativo");
                        //idHtmlMovSerieObj4.setAttribute("name", "conceptos[" + nNum + "].usaCorrelativo");
                        idHtmlMovSerieObj5.setAttribute("name", "conceptos[" + nNum + "].formatoImpresion");
                        idHtmlMovSerieObj6.setAttribute("name", "conceptos[" + nNum + "].fechaAutorizacion");
                        idHtmlMovSerieObj7.setAttribute("name", "conceptos[" + nNum + "].res_del");
                        idHtmlMovSerieObj8.setAttribute("name", "conceptos[" + nNum + "].res_al");
                        idHtmlMovSerieObj9.setAttribute("name", "conceptos[" + nNum + "].resolucionNumero");
                        idHtmlMovSerieObj10.setAttribute("name", "conceptos[" + nNum + "].secuencia");
                        idHtmlButtonDeleteObj.setAttribute("onclick", "Delete(" + nNum + ");");

                        //Reasignamos Id
                        idHtmlMovSerieObj0.setAttribute("id", "conceptos_" + nNum + "__idInternoTIposMovimientosSeries");
                        idHtmlMovSerieObj1.setAttribute("id", "conceptos_" + nNum + "__idInternoTiposMovimientos");
                        idHtmlMovSerieObj2.setAttribute("id", "conceptos_" + nNum + "__idSerie");
                        idHtmlMovSerieObj3.setAttribute("id", "conceptos_" + nNum + "__correlativo");
                        // idHtmlMovSerieObj4.setAttribute("id", "conceptos_" + nNum + "__usaCorrelativo");
                        idHtmlMovSerieObj5.setAttribute("id", "conceptos_" + nNum + "__formatoImpresion");
                        idHtmlMovSerieObj6.setAttribute("id", "conceptos_" + nNum + "__fechaAutorizacion");
                        idHtmlMovSerieObj7.setAttribute("id", "conceptos_" + nNum + "__res_del");
                        idHtmlMovSerieObj8.setAttribute("id", "conceptos_" + nNum + "__res_al");
                        idHtmlMovSerieObj9.setAttribute("id", "conceptos_" + nNum + "__resolucionNumero");
                        idHtmlMovSerieObj10.setAttribute("id", "conceptos_" + nNum + "__secuencia");
                        idHtmlButtonDeleteObj.setAttribute("id", "buttonDelete" + nNum);
                        idHtmlDivConceptosObj.setAttribute("id", "divConceptosEdit" + nNum);

                        nNum = nNum + 1;

                    } else {

                        //reasignamos Name                            
                        idHtmlMovSerieObj0.setAttribute("name", "conceptos[" + valIteracion + "].idInternoTIposMovimientosSeries");
                        idHtmlMovSerieObj1.setAttribute("name", "conceptos[" + valIteracion + "].idInternoTiposMovimientos");
                        idHtmlMovSerieObj2.setAttribute("name", "conceptos[" + valIteracion + "].idSerie");
                        idHtmlMovSerieObj3.setAttribute("name", "conceptos[" + valIteracion + "].correlativo");
                        //idHtmlMovSerieObj4.setAttribute("name", "conceptos[" + nNum + "].usaCorrelativo");
                        idHtmlMovSerieObj5.setAttribute("name", "conceptos[" + valIteracion + "].formatoImpresion");
                        idHtmlMovSerieObj6.setAttribute("name", "conceptos[" + valIteracion + "].fechaAutorizacion");
                        idHtmlMovSerieObj7.setAttribute("name", "conceptos[" + valIteracion + "].res_del");
                        idHtmlMovSerieObj8.setAttribute("name", "conceptos[" + valIteracion + "].res_al");
                        idHtmlMovSerieObj9.setAttribute("name", "conceptos[" + valIteracion + "].resolucionNumero");
                        idHtmlMovSerieObj10.setAttribute("name", "conceptos[" + valIteracion + "].secuencia");
                        idHtmlButtonDeleteObj.setAttribute("onclick", "Delete(" + valIteracion + ");");

                        //Reasignamos Id
                        idHtmlMovSerieObj0.setAttribute("id", "conceptos_" + valIteracion + "__idInternoTIposMovimientosSeries");
                        idHtmlMovSerieObj1.setAttribute("id", "conceptos_" + valIteracion + "__idInternoTiposMovimientos");
                        idHtmlMovSerieObj2.setAttribute("id", "conceptos_" + valIteracion + "__idSerie");
                        idHtmlMovSerieObj3.setAttribute("id", "conceptos_" + valIteracion + "__correlativo");
                        // idHtmlMovSerieObj4.setAttribute("id", "conceptos_" + nNum + "__usaCorrelativo");
                        idHtmlMovSerieObj5.setAttribute("id", "conceptos_" + valIteracion + "__formatoImpresion");
                        idHtmlMovSerieObj6.setAttribute("id", "conceptos_" + valIteracion + "__fechaAutorizacion");
                        idHtmlMovSerieObj7.setAttribute("id", "conceptos_" + valIteracion + "__res_del");
                        idHtmlMovSerieObj8.setAttribute("id", "conceptos_" + valIteracion + "__res_al");
                        idHtmlMovSerieObj9.setAttribute("id", "conceptos_" + valIteracion + "__resolucionNumero");
                        idHtmlMovSerieObj10.setAttribute("id", "conceptos_" + valIteracion + "__secuencia");
                        idHtmlButtonDeleteObj.setAttribute("id", "buttonDelete" + valIteracion);
                        idHtmlDivConceptosObj.setAttribute("id", "divConceptosEdit" + valIteracion);

                    }

                }

                while (div.hasChildNodes()) {
                    div.removeChild(div.lastChild);
                }

                let garbage = document.getElementById("divConceptosEditPadre");
                garbage.removeChild(div);
            }

            //Reasignamos Valores
            num++;
            valIteracion = valIteracion - 1;

            document.getElementById("idContador").value = num;
            document.getElementById("idIteracion").value = valIteracion;
            document.getElementById("idItemUltimoELiminado").value = id;


            Swal.fire({ title: "Eliminado!", text: "El Tipo de movimiento ha sido eliminado.", icon: "success" })
        } else {
            //SI CANCELA
            t.dismiss === Swal.DismissReason.cancel && Swal.fire({ title: "Cancelado", text: "El Tipo de movimiento no se ha eliminado :)", icon: "error" })
        }
    })
}