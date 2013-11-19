var townId = 24;

function townChanged() {
    townId = $("#towns").data("kendoDropDownList").value();
    var grid = $("#sample-forecast-grid").data("kendoGrid");
    grid.dataSource.read();
    var grid2 = $("#sample-forecast-grid2").data("kendoGrid");
    grid2.dataSource.read();
    var grid3 = $("#sample-forecast-grid3").data("kendoGrid");
    grid3.dataSource.read();
}

function selectedTownId() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1;

    var yyyy = today.getFullYear();
    if (dd < 10) { dd = '0' + dd } if (mm < 10) { mm = '0' + mm } today = dd + '.' + mm + '.' + yyyy;

    var townIdSelected = { townId: townId, date: today };
    return townIdSelected;
}

function filterTowns() {
    return {
        areas: $("#areas").val()
    };
}

function selectedTownIdTomorrow() {
    var today = new Date();
    var dd = today.getDate() + 1;
    var mm = today.getMonth() + 1;
    if (dd > 30) {
        dd = 1;
        mm = mm+1;
    }
   
    if (mm > 12) {
        mm = 1;
    }
    var yyyy = today.getFullYear();
    if (dd < 10) { dd = '0' + dd } if (mm < 10) { mm = '0' + mm } today = dd + '.' + mm + '.' + yyyy;

    var townIdSelected = { townId: townId, date: today };
    return townIdSelected;
}

function selectedTownIdDayAfterTomorrow() {
    var today = new Date();
    var dd = today.getDate() + 2;
    var mm = today.getMonth() + 1;
    if (dd > 30) {
        dd = 2;
        mm += 1;
    }
   
    if (mm > 12) {
        mm = 1;
    }

    var yyyy = today.getFullYear();
    if (dd < 10) { dd = '0' + dd } if (mm < 10) { mm = '0' + mm } today = dd + '.' + mm + '.' + yyyy;

    var townIdSelected = { townId: townId, date: today };
    return townIdSelected;
}