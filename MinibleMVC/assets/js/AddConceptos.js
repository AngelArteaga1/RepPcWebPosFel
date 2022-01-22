function CrearConceptos(num, numRow, nameObject1, idObject1, typeObject1, idObjectlb1, txtObject1, nameObject2, idObject2, typeObject2, idObjectlb2, txtObject2) {

    //Elementos Principales
    let DivConceptos = document.getElementById("divConceptos");

    // Div Primera fila
    let divRow1 = document.createElement("div");
    let divCol1 = document.createElement("div");
    let divElemento1 = document.createElement("div");

    let LabelElemento1 = document.createElement("label");
    let Elemento1 = document.createElement("input");

    divRow1.setAttribute("class", "row");
    divCol1.setAttribute("class", "col-md-6");
    divElemento1.setAttribute("class", "form-group");
    LabelElemento1.setAttribute("class", "col-form-label");
    LabelElemento1.setAttribute("for", idObject1 + num);

    if (typeObject1 == "text" || typeObject1 == "date") {
        Elemento1.setAttribute("class", "form-control");
    } else {
        Elemento1.setAttribute("class", "flat-red");
    }

    LabelElemento1.innerHTML = txtObject1;
    LabelElemento1.id = idObjectlb1 + num;
    Elemento1.id = idObject1 + num;
    Elemento1.name = "conceptosAdd[" + num + "]." + nameObject1;
    Elemento1.type = typeObject1;


    if (numRow > 1) {
        var divCol2 = document.createElement("div");
        var divElemento2 = document.createElement("div");

        var LabelElemento2 = document.createElement("label");
        var Elemento2 = document.createElement("input");

        divCol2.setAttribute("class", "col-md-6");
        divElemento2.setAttribute("class", "form-group");
        LabelElemento2.setAttribute("class", "col-form-label");
        LabelElemento2.setAttribute("for", idObject2 + num);

        if (typeObject2 == "text" || typeObject2 == "date") {
            Elemento2.setAttribute("class", "form-control");
        } else {
            Elemento2.setAttribute("class", "flat-red");
        }

        LabelElemento2.innerHTML = txtObject2;
        LabelElemento2.id = idObjectlb2 + num;
        Elemento2.id = idObject2 + num;
        Elemento2.name = "conceptosAdd[" + num + "]." + nameObject2;
        Elemento2.type = typeObject2;

    }


    divElemento1.appendChild(LabelElemento1);
    divElemento1.appendChild(Elemento1);
    divCol1.appendChild(divElemento1);
    divRow1.appendChild(divCol1);

    if (numRow > 1) {
        divElemento2.appendChild(LabelElemento2);
        divElemento2.appendChild(Elemento2);
        divCol2.appendChild(divElemento2);
        divRow1.appendChild(divCol2);
    }

    DivConceptos.appendChild(divRow1);

}