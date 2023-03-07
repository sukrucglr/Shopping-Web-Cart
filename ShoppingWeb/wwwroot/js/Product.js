var dtable;

$(document).ready(function () {

    dtable = $('#myTable').DataTable({

        "ajax": { "url": "/Admin/Product/AllProducts" },
        "columns": [
            { "data": "name" },
            { "data": "description" },
            { "data": "price" },
            { "data": "category.name" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                    <a href="/Admin/Product/CreateUpdate?id=${data}"> <i class="bi bi-pencil-square"></i>Güncelle</a>
                    <a onClick=RemoveProduct("/Admin/Product/Delete/${data}")><i class="bi bi-trash"></i>Sil</a>
                        `     }
            }
        ]
    });
});
function RemoveProduct(url) {
    Swal.fire({
        title: 'Emin misin?',
        text: 'Bunu geri alamazsın',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonolor: '#d33',
        confirmButtonText: 'Evet, Sil',
    }).then((result) => {
        if (result.isConfirmed) {

            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dtable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }

            });
        }
    })


}