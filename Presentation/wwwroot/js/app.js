var table;

$(document).ready(function () {
    table = $("#dt-members").DataTable({

        // Design Assets
        stateSave: true,
        autoWidth: true,

        // ServerSide Setups
        processing: true,
        serverSide: true,

        // Paging Setups
        paging: true,
        lengthMenu: [[5, 10, 25, 50, 100, -1], [5, 10, 25, 50, 100, "All"]],
        pageLength: 10,
        pagingType: "full_numbers",

        // Custom Export Buttons
        dom: 'lBfrtip',
        buttons: [
            {
                text: 'Excel',
                action: function () {
                    exportToExcel();
                }
            },
            {
                text: 'CSV',
                action: function () {
                    exportToCsv();
                }
            },
            {
                text: 'HTML',
                action: function () {
                    exportToHtml();
                }
            },
            {
                text: 'JSON',
                action: function () {
                    exportToJson();
                }
            },
            {
                text: 'XML',
                action: function () {
                    exportToXml();
                }
            },
            {
                text: 'YAML',
                action: function () {
                    exportToYaml();
                }
            }
        ],

        // Searching Setups
        searching: { regex: true },

        // Ajax Filter
        ajax: {
            url: "/Members/LoadTable",
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            data: function (d) {
                return JSON.stringify(d);
            }
        },

        // Columns Setups
        columns: [
            { data: "profilePicturePath" },
            { data: "name" },
            { data: "email" }
        ],

        // Column Definitions
        columnDefs: [
            { targets: "no-sort", orderable: false },
            { targets: "no-search", searchable: false },
            {
                targets: "trim",
                render: function (data, type, full, meta) {
                    if (type === "display") {
                        data = strtrunc(data, 10);
                    }

                    return data;
                }
            },
            { targets: "date-type", type: "date-eu" },
            {
                targets: 0,
                data: "profilePicturePath",
                render: function (data, type, row) {
                    return `<img src='/uploads/${row.profilePicturePath}' alt="Profile Picture" class="img-fluid rounded" style="max-width: 50px; max-height: 50px;">`;
                },
                orderable: false
            },
            {
                targets: 3,
                data: null,
                defaultContent:
                    "<a class='btn btn-link' role='button' href='#' onclick='assign(this)'>Assign Member</a> | " +
                    "<a class='btn btn-link' role='button' href='#' onclick='edit(this)'> Edit</a > | " +
                    "<a class= 'btn btn-link' role='button' href='#' onclick='details(this)' > Details</a> | " +
                    "<a class= 'btn btn-link' role = 'button' href = '#' onclick = 'deletion(this)' > Delete</a > ",
                orderable: false
            },
        ]
    });
});

function strtrunc(str, num) {
    if (str.length > num) {
        return str.slice(0, num) + "...";
    }
    else {
        return str;
    }
}

function assign(rowContext) {
    if (table) {
        var data = table.row($(rowContext).parents("tr")).data();
        window.location.href = `/Members/Assign/${data["id"]}`;
    }
}
function edit(rowContext) {
    if (table) {
        var data = table.row($(rowContext).parents("tr")).data();
        window.location.href = `/Members/Edit/${data["id"]}`;
    }
}
function details(rowContext) {
    if (table) {
        var data = table.row($(rowContext).parents("tr")).data();
        window.location.href = `/Members/Details/${data["id"]}`;
    }
}
function deletion(rowContext) {
    if (table) {
        var data = table.row($(rowContext).parents("tr")).data();
        window.location.href = `/Members/Delte/${data["id"]}`;
    }
}

function renderDownloadForm(format) {
    $('#export-to-file-form').attr('action', '/Members/ExportTable?format=' + format);

    // Get jQuery DataTables AJAX params
    var datatableParams = $('#dt-members').DataTable().ajax.params();

    // If the input exists, replace value, if not create the input and append to form
    if ($("#export-to-file-form input[name=dtParametersJson]").val()) {
        $('#export-to-file-form input[name=dtParametersJson]').val(datatableParams);
    } else {
        var searchModelInput = $("<input>")
            .attr("type", "hidden")
            .attr("name", "dtParametersJson")
            .val(datatableParams);

        $('#export-to-file-form').append(searchModelInput);
    }
}

function exportToExcel() {
    renderDownloadForm("excel");

    $("#export-to-file-form").submit();
}

function exportToCsv() {
    renderDownloadForm("csv");

    $("#export-to-file-form").submit();
}

function exportToHtml() {
    renderDownloadForm("html");

    $("#export-to-file-form").submit();
}

function exportToJson() {
    renderDownloadForm("json");

    $("#export-to-file-form").submit();
}

function exportToXml() {
    renderDownloadForm("xml");

    $("#export-to-file-form").submit();
}

function exportToYaml() {
    renderDownloadForm("yaml");

    $("#export-to-file-form").submit();
}