﻿var dataTable;

$(document).ready(function () {
    loadDataTable();
});
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: "/order/getall" },
        "columns": [
            { data: 'orderHeaderId', "width": "5%" },
            { data: 'email', "width":"25%"},
            { data: 'phone', "width":"25%"},
            { data: 'status', "width":"25%"},
            { data: 'orderTotal', "width":"25%"},
            {
                data: 'orderHeaderId',
                "render": function (data) {
                    return  `<div class="w-75 btn-group" role="group">
                                <a href="/order/orderDetails?orderId=${data}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square"></i>
                                </a>
                            </div>`
                },
                "width": "10%"
            },
        ]
    });
}