var dataTable;

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("inprocess")) {
        loadDataTable("inprocess")
    } else {
        if (url.includes("pending")) {
            loadDataTable("pending")
        } else {
            if (url.includes("completed")) {
                loadDataTable("completed")
            } else {
                if (url.includes("approved")) {
                    loadDataTable("approved")
                } else {
                    loadDataTable("all")
                }
            }
        }
    }
});


function loadDataTable(status) {
    dataTable = $('#myTable').DataTable({
        "ajax": {
            "url": "/Admin/Order/getAll?status=" + status
        },
        "columns": [
            {"data": "id", "width": "8%"},
            {"data": "name", "width": "18%"},
            {"data": "applecationUser.userName", "width": "18%"},
            {"data": "phoneNumber", "width": "15%"},
            {"data": "orderStatus", "width": "15%"},
            {"data": "orderTotal", "width": "15%"},

            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                        <a href="/Admin/Order/Detelis?OrderId=${data}"
                        class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i></a>
                      
					</div>
                        `
                },
                "width": "7"
            }

        ]
    });


}

//
// function cnformDelete(url) {
//
//     Swal.fire({
//         title: 'Are you sure?',
//         text: "You won't be able to revert this!",
//         icon: 'warning',
//         showCancelButton: true,
//         confirmButtonColor: '#3085d6',
//         cancelButtonColor: '#d33',
//         confirmButtonText: 'Yes, delete it!'
//     }).then((result) => {
//         if (result.isConfirmed) {
//             // $(".overlay").show();
//             $.ajax({
//                 url: url,
//                 type: 'DELETE',
//                 // dataType: 'json',
//                 // contentType: "application/json",
//                 // processData: false,
//
//                 success: function (data) {
//                     if (data.success == true) {
//                         dataTable.ajax.reload();
//                         toastr.success(data.message)
//
//                         // loadDataTable();
//                     } else
//                         toastr.error(data.message)
//                 },
//
//             });
//         }
//     })
// }
//
