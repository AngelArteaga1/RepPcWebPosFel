/* ====================Eliminar Los Detalles ======== */
function Delete(id, idHtmlObject, nameObject, deleteDivPrincipal) {

    //Eliminamos del DOM los elementos
    var nNum = 0;
    var div = document.getElementById("divConceptosEdit" + id);
    var valIteracion = document.getElementById("idIteracion").value;
    if (div != null) {

        for (var j = 0; j < valIteracion; j++) {
            //Variable String para formar el id 
            let idHtmlElementIt = "conceptosEdit_" + j + idHtmlObject;
            //Obtenemos Id
            let idHtmlObj0 = document.getElementById(idHtmlElementIt);

            if (id != j) {
                //reasignamos Name 
                idHtmlObj0.setAttribute("name", "conceptosEdit[" + nNum + "]." + nameObject);

                //Reasignamos Id
                idHtmlObj0.setAttribute("id", "conceptosEdit_" + nNum + idHtmlObject);
                nNum = nNum + 1;
            } else {
                //reasignamos Name 
                idHtmlObj0.setAttribute("name", "conceptosEdit[" + valIteracion + "]." + nameObject);

                //Reasignamos Id
                idHtmlObj0.setAttribute("id", "conceptosEdit_" + valIteracion + idHtmlObject);
            }

        }



        if (deleteDivPrincipal == "S") {
            //Reasignamos id, name de botones y div
            var nNum = 0;
            for (var j = 0; j < valIteracion; j++) {

                //Variable String para formar el id                     
                let idHtmlButtonDelete = "buttonDelete" + j;
                let idHtmlDivConceptos = "divConceptosEdit" + j;

                //Obtenemos Id                    
                let idHtmlButtonDeleteObj = document.getElementById(idHtmlButtonDelete);
                let idHtmlDivConceptosObj = document.getElementById(idHtmlDivConceptos);

                if (id != j) {
                    //reasignamos Name                                                    
                    idHtmlButtonDeleteObj.setAttribute("onclick", "EliminarConcepto(" + nNum + ");");

                    //Reasignamos Id                        
                    idHtmlButtonDeleteObj.setAttribute("id", "buttonDelete" + nNum);
                    idHtmlDivConceptosObj.setAttribute("id", "divConceptosEdit" + nNum);

                    nNum = nNum + 1;

                } else {

                    //reasignamos Name                                                    
                    idHtmlButtonDeleteObj.setAttribute("onclick", "Delete(" + valIteracion + ");");

                    //Reasignamos Id                        
                    idHtmlButtonDeleteObj.setAttribute("id", "buttonDelete" + valIteracion);
                    idHtmlDivConceptosObj.setAttribute("id", "divConceptosEdit" + valIteracion);

                }

            }

            while (div.hasChildNodes()) {
                div.removeChild(div.lastChild);
            }

            let garbage = document.getElementById("divConceptosEditPadre");
            garbage.removeChild(div);

            //Reasignamos Valores
            num++;
            valIteracion = valIteracion - 1;

            document.getElementById("idContador").value = num;
            document.getElementById("idIteracion").value = valIteracion;
            document.getElementById("idItemUltimoELiminado").value = id;
        }


    }





}