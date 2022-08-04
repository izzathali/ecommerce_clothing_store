$(document).ready(function () {
    $("#sortproduct").DataTable({
        columnDefs: [
            { type: 'date', targets: [6] }
        ],
    });
});


$(document).ready(function () {
    $("#sortcategory").DataTable({
        columnDefs: [
            { type: 'date', targets: [1] }
        ],
    });
});

$(document).ready(function () {
    $("#sortorder").DataTable({
        columnDefs: [
            { type: 'date', targets: [5] }
        ],
    });
});

$(document).ready(function () {
    $("#sortcustomer").DataTable({
        columnDefs: [
            { type: 'date', targets: [5] }
        ],
    });
});

$(document).ready(function () {
    $("#sortstock").DataTable({
        columnDefs: [
            { type: 'date', targets: [6] }
        ],
    });
});

$(document).ready(function () {
    $("#sortuser").DataTable({
        columnDefs: [
            { type: 'date', targets: [6] }
        ],
    });
});


