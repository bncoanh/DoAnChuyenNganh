$(document).ready(function (e) {
    GetAll();
});

var pageNumber = 1;
var totalPage = 0;
var productTypeId = 0;
$('#request').keydown(function (e) {
    if (e.keyCode == 13) {
        pageNumber = 1;
        GetAll();
    }
})
function previousPage() {
    if (pageNumber > 1) {
        pageNumber -= 1;
        GetAll();
    }
}
function nextPage() {
    if (pageNumber < totalPage) {
        pageNumber += 1;
        GetAll();
    }
}
$('#pageSize').change(function () {
    pageNumber = 1;
    GetAll();
});

$('#groupId').change(function () {
    pageNumber = 1;
    GetAll();
});

function Pagination() {
    var html = `<li class="page-item"> <a onclick="previousPage()" class="page-link cusor" aria - label="Previous" > <span aria-hidden="true">&laquo;</span> </a > 
                </li > `;
    if (totalPage == 1) {
        html += `<li class="page-item"><a class="page-link" style="background-color: aliceblue;">${pageNumber}</a></li>`
    }
    else if (pageNumber == 1) {
        html += `<li class="page-item"><a class="page-link" style="background-color: aliceblue;">${pageNumber}</a></li>
            <li class="page-item cusor"><a onclick="nextPage()" class="page-link">${pageNumber + 1}</a></li>`;
    }
    else if (pageNumber == totalPage) {
        html += `<li class="page-item cusor"><a onclick="previousPage()"  class="page-link">${pageNumber - 1}</a></li>
            <li class="page-item"><a style="background-color: aliceblue;" class="page-link">${pageNumber}</a></li>`;
    }
    else {
        html += `<li class="page-item cusor"><a onclick="previousPage()" class="page-link">${pageNumber - 1}</a></li>
            <li class="page-item"><a class="page-link" style="background-color: aliceblue;" >${pageNumber}</a></li>
            <li class="page-item cusor"><a onclick="nextPage()" class="page-link">${pageNumber + 1}</a></li>`;
    }
    html += `<li class="page-item cusor">
            <a onclick="nextPage()" class="page-link" aria-label="Next">
                <span aria-hidden="true">&raquo;</span>
            </a>
         </li>`;

    $('#pagination').html(html);
}
$('#btn_search').click(function () {
    pageNumber = 1;
    GetAll();
})



function showModal() {
    $('#staticBackdrop').modal('show');
    if (productTypeId == 0) {
        $('#btn_deleteModal').hide();
    }
}

function CloseModal() {
    productTypeId = 0;
    document.getElementById('form').reset();
    $('#importedMaterial').val("");
    $('#staticBackdrop').modal('hide');
}

$('#add_new').click(function () {
    $('#staticBackdropLabel').text("Thêm mới loại sản phẩm");
    showModal();
})
$('#btn_deleteModal').click(function () {
    DeleteById(productTypeId);
});
function GetAll() {
    let _url = "/Admin/Product/GetAllProductType?";
    var request = $('#request').val();
    var groupId = $('#groupId option:selected').val();
    if (request.length > 0) {
        _url += "request=" + request;
    }
    _url += "&pageNumber=" + pageNumber;

    $.ajax({
        url: _url,
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            console.log(data);
            var html = '';
            $.each(data.data, function (index, item) {
                html += `<tr class="align-middle">
                            <td scope="col" class="align-center text-center" style="white-space: nowrap">
                                <button class="btn btn-sm btn-primary" onclick="GetById(${item.id})" ><i class="bi bi-pencil-square"></i> Sửa</button>
                                <button class="btn btn-sm btn-danger" onclick="DeleteById(${item.id})"><i class="bi bi-trash3"></i> Xóa</button>
                            </td>
                            <td scope="col" class="text-center"> <a style="color: blue;  cursor: pointer;" onclick="GetById(${item.id})">${item.code} </a></td>
                            <td scope="col">${item.name}</td>
                            <td scope="col">${item.groupType == 1 ? "Coffe" : (item.groupType == 2 ? "Trà" : "Khác")}</td>
                            <td scope="col">${item.description ?? ""}</td>
                            
                        </tr>`;
            })
            let total = Math.ceil(data.totalCount.totalCount / 10);
            totalPage = total > 0 ? total : 1;
            $('#tbody').html(html);
            $('#page_details').text(`Trang ${pageNumber} / ${totalPage}`);
            $('#pageNumber').val(pageNumber);
            Pagination();
        },

        error: function (err) {
            MessageError(err.responseText);
        }
    });
}


function GetById(id) {
    $('#btn_deleteModal').show();
    $('#staticBackdropLabel').text("Cập nhật loại sản phẩm");
    productTypeId = id;
    let _url = "/Admin/Product/GetByIdProductType";
    $.ajax({
        url: _url,
        type: 'GET',
        dataType: 'json',
        data: {
            Id: id
        },
        contentType: 'application/json',
        success: function (data) {
            $('#formCode').val(data.code);
            $('#formName').val(data.name);
            $('#formGroupType').val(data.groupType);
            $('#formNote').val(data.description);
            
        },
        error: function (err) {
            MessageError(err.responseText);
        }
    });
    showModal();
}
function CreateOrUpdate() {
    var obj = {
        Id: parseInt( productTypeId),
        Code: $("#formCode").val(),
        Name: $("#formName").val(),
        GroupType: parseInt($("#formGroupType").val()),
        Description: $("#formNote").val(),
    };

    let _url = "/Admin/Product/CreateProductType";
    $.ajax({
        type: 'POST',
        url: _url,
        contentType: 'application/json;charset=utf-8',
        data: JSON.stringify(obj),
        success: function (result) {
            if (result.status == 0) {
                alert(result.message)
            }
            else {
                CloseModal();
                GetAll();
            }
        },
        error: function (err) {
            MessageError(err.responseText);
        }
    });

}
function DeleteById(id) {
    if (confirm("Bạn có chắc chắn muốn thực hiện thao tác này?") == true) {
        let _url = "/Admin/Product/DeleteProductType";
        $.ajax({
            type: 'GET',
            url: _url,
            contentType: 'application/json;charset=utf-8',
            data: {
                Id: id,
            },
            success: function (result) {
                if (result.status == 1) {
                    CloseModal();
                    pageNumber = 1;
                    GetAll();
                }
                else alert(result.message);

            },
            error: function (err) {
                MessageError(err.responseText);
            }
        });
    }
}