$(document).ready(function (e) {
    GetAll();
});

var pageNumber = 1;
var totalPage = 0;
var accountId = 0;
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
    if (accountId == 0) {
        $('#btn_deleteModal').hide();
    }
}

function CloseModal() {
    accountId = 0;
    document.getElementById('form').reset();
    $('#importedMaterial').val("");
    $('#staticBackdrop').modal('hide');
}

$('#add_new').click(function () {
    $('#staticBackdropLabel').text("Thêm mới loại sản phẩm");
    showModal();
})
$('#btn_deleteModal').click(function () {
    DeleteById(accountId);
});
function GetAll() {
    let _url = "/Admin/Account/GetAll?";
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
            var html = '';
            $.each(data.data, function (index, item) {
                html += `<tr class="align-middle">
                            <td scope="col" class="align-center text-center" style="white-space: nowrap">
                                <button class="btn btn-sm btn-primary" onclick="GetById(${item.id})" ><i class="bi bi-pencil-square"></i> Sửa</button>
                                 <button class="btn btn-sm btn-danger" onclick="DeleteById(${item.id})"><i class="bi bi-trash3"></i> Xóa</button>
                            </td>
                            <td scope="col" class="text-center"> <a style="color: blue;  cursor: pointer;" onclick="GetById(${item.id})">${item.email} </a></td>
                            <td scope="col">${item.fullName ?? ""}</td>
                            <td scope="col">${item.genderText ?? ""}</td>
                            <td scope="col">${item.roleText ?? ""}</td>
                            <td scope="col">${item.phoneNumber ?? ""}</td>
                            <td scope="col">${item.address ?? ""}</td>
                            
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
    $('#staticBackdropLabel').text("Cập nhật Size");
    accountId = id;
    let _url = "/Admin/Account/GetById";
    $.ajax({
        url: _url,
        type: 'GET',
        dataType: 'json',
        data: {
            Id: id
        },
        contentType: 'application/json',
        success: function (data) {
            console.log(data);
            $("#formName").val(data.fullName);
            $("#formGender").val(data.gender);
            $("#formRole").val(data.role);
            $("#formIsActive").val(data.isActive);
            $("#formPhoneNumber").val(data.phoneNumber);
            $("#formEmail").val(data.email);
            $("#formPassword").val(data.passWord);
            $("#formPasswordConfirm").val(data.passWord);
            $("#formAddress").val(data.address);

        },
        error: function (err) {
            MessageError(err.responseText);
        }
    });
    showModal();
}
function CreateOrUpdate() {

    let ps = $("#formPassword").val();
    let pscf = $("#formPasswordConfirm").val();
    let fn = $("#formName").val();
    let em = $("#formEmail").val();

    if (fn == null || fn.length <= 0) {
        alert("Hãy nhập họ tên!");
        return
    }
    if (em == null || em.length <= 0) {
        alert("Hãy nhập email!");
        return
    }

    if (ps == null || ps.length <= 0) {
        alert("Hãy nhập mật khẩu!");
        return
    }

    if (pscf == null || pscf.length <= 0) {
        alert("Hãy nhập mật khẩu xác nhận!");
        return
    }

    if (ps != pscf) {
        alert("Mật khẩu xác nhận chưa chính xác!");
        return
    }

    var obj = {
        Id: accountId,
        FullName: $("#formName").val(),
        Role: $("#formRole").val(),
        Gender: $("#formGender").val(),
        IsActive: parseInt($("#formIsActive").val()),
        PhoneNumber: $("#formPhoneNumber").val(),
        Email: $("#formEmail").val(),
        Password: $("#formPassword").val(),
        Address: $("#formAddress").val()
    };

    let _url = "/Admin/Account/CreateOrUpdate";
    $.ajax({
        type: 'POST',
        url: _url,
        contentType: 'application/json;charset=utf-8',
        data: JSON.stringify(obj),
        success: function (result) {
            CloseModal();
            GetAll();
        },
        error: function (err) {
            MessageError(err.responseText);
        }
    });

}
function DeleteById(id) {
    if (confirm("Bạn có chắc chắn muốn thực hiện thao tác này?") == true) {
        let _url = "/Admin/ProductSize/Delete";
        $.ajax({
            type: 'GET',
            url: _url,
            contentType: 'application/json;charset=utf-8',
            data: {
                Id: id,
            },
            success: function (result) {
                CloseModal();
                pageNumber = 1;
                GetAll();

            },
            error: function (err) {
                MessageError(err.responseText);
            }
        });
    }
}