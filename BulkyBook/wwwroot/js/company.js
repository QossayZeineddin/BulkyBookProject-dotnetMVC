var dataTable;

$(document).ready(function () {
    loadDataTable();
});

 
function loadDataTable() {
    dataTable = $('#myTable').DataTable({
        "ajax": {
            "url": "/Admin/Company/getAll"
        },
        "columns": [
            {"data": "name", "width": "15%"},
            {"data": "city", "width": "15%"},
            {"data": "phoneNumber", "width": "15%"},
            { "data": "postalCode", "width": "15%"},

            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                        <a href="/Admin/Company/Upsert?id=${data}"
                        class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                       <a onClick=cnformDelete('/Admin/Company/Delete/${data}')
                        class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
					</div>
                        `
                },
                "width": "15%"
            }

        ]
    });


}

function cnformDelete(url) {

    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            // $(".overlay").show();
            $.ajax({
                url: url,
                type: 'DELETE',
                // dataType: 'json',
                // contentType: "application/json",
                // processData: false,

                success: function (data) {
                    if (data.success == true) {
                        dataTable.ajax.reload();
                        toastr.success(data.message)

                        // loadDataTable();
                    } else
                        toastr.error(data.message)
                },

            });
        }
    })
}

