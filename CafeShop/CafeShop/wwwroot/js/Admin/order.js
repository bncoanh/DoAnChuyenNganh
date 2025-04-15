$(document).ready(function (e) {
    GetAll();
});
var pageNumber = 1;
var totalPage = 0;
var productId = 0;
var htmlSize = "";
var _attachFiles = [];
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
function GetAll() {
    var request = $('#request').val() ?? "";
    var status = $('#order_status option:selected').val();
    var dateStart = $('#date_start').val();
    var dateEnd = $('#date_end').val();
    let obj = {
        request,
        pageNumber,
        status,
        dateStart,
        dateEnd
    };
    $.ajax({
        url: "/Admin/Order/GetAll",
        data: JSON.stringify(obj),
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            var html = '';
            $.each(data.data, function (index, item) {
                html += `<tr class="align-middle">
                            <td scope="col" class="align-center text-center" style="white-space: nowrap">
                                <btn class="btn btn-sm btn-danger" title="Hủy đơn hàng" ${item.status >= 2 ? "hidden" : ""} onclick="StatusApproved(${item.id}, 3, 'Hủy đơn hàng')"><i class="bi bi-x-circle-fill"></i></btn>
                                <btn class="btn btn-sm btn-success" title="Giao hàng" ${item.status >= 1 ? "hidden" : ""} onclick="StatusApproved(${item.id}, 1, 'Giao đơn hàng')"><i class="bi bi-truck"></i></btn>
                                <btn class="btn btn-sm btn-primary" title="Thành công" ${item.status >= 2 ? "hidden" : ""}  onclick="StatusApproved(${item.id}, 2, 'Hoàn thành đơn hàng')"><i class="bi bi-check-lg"></i></btn>
                            </td>
                            <td scope="col" class="text-center"> <a style="color: blue;  cursor: pointer;" onclick="GetDetails(${item.id}, event)">${item.orderCode} </a></td>
                            <td scope="col">${moment(item.createDate).format('DD/MM/YYYY HH:mm:ss')}</td>
                            <td class="text-center" scope="col">${item.status === 0 ? "Chờ xác nhận" : (item.status === 1 ? "Đang giao" : (item.status === 2 ? "Hoàn thành" : "Đã hủy"))}</td>
                            <td scope="col">${item.customerName ?? ""}</td>
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
function GetDetails(id, event) {
    var htmlDetails = `<tr class="order-details" id="order_details_${id}" style="border-left: 2px solid red; border-right: 2px solid red; border-bottom: 2px solid red;">
                    <td colspan="7" class="p-4">
                        <div class="card-body p-0">
                            <table class="table table-sm m-0 table-bordered">
                                <thead>
                                    <tr>
                                        <th class="text-nowrap text-center text-dark py-1" style="height:auto !important;">STT</th>
                                        <th class="text-nowrap text-center text-dark py-1" style="height:auto !important; width:15%;">Sản phẩm</th>
                                        <th class="text-nowrap text-center text-dark py-1" style="height:auto !important; width:15%;">Size</th>
                                        <th class="text-nowrap text-center text-dark py-1" style="height:auto !important;">Đơn giá</th>
                                        <th class="text-nowrap text-center text-dark py-1" style="height:auto !important;">Số lượng</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    @tbody
                                </tbody>
                            </table>
                        </div>
                        <div class="row">
                          <label for="totalMoney" class="col-sm-2 col-form-label">Tổng tiền:</label>
                          <div class="col-sm-10">
                            <input type="text" class="form-control text-center" id="totalMoney" value="@totalMoney" disabled readonly>
                          </div>
                        </div>
                    </td>
                </tr>`;

    var isShowDetail = $(`#order_details_${id}`).length;
    var el = $(event.target).parent();
    if (isShowDetail > 0) {
        $(`#order_details_${id}`).remove();
    } else {
        $('.order-details').remove();
        $.ajax({
            url: '/Admin/Order/GetDetail',
            type: 'GET',
            dataType: 'json',
            data: {
                OrderId: id
            },
            contentType: 'application/json',
            success: function (result) {
                console.log(result)

                var htmlBody = '';
                let totalMoney = 0;
                //Hiển thị danh sách chi tiết
                $.each(result, function (index, item) {
                    totalMoney += (item.price * item.quantity) 
                    var styleText = item.sizeName;
                    htmlBody += `<tr>
                                    <td style="height:auto !important;padding:10px !important;" class="text-center align-middle">${index + 1}</td>
                                    <td style="height:auto !important;padding:10px !important;" class="text-left">
                                              <img src="${item.imageUrl}" alt="" class="img-fluid d-block mx-auto mb-3">
                                              <h5 class="text-dark">${item.productName}</h5>
                                              
                                    </td>
                                    <td style="height:auto !important;padding:10px !important;" class="text-center align-middle">${item.sizeName}</td>
                                    <td style="height:auto !important;padding:10px !important;" class="text-center align-middle">${item.price.toLocaleString('en-US') } VNĐ</td>
                                    <td style="height:auto !important;padding:10px !important;" class="text-center align-middle">${item.quantity}</td>
                                </tr>`;
                });
                // <p class="small text-muted font-italic">${item.sizeName}</p>
                htmlDetails = htmlDetails.replace('@tbody', htmlBody);
                htmlDetails = htmlDetails.replace('@totalMoney', `${totalMoney.toLocaleString('en-US')} VNĐ`);

                $(htmlDetails).insertAfter($(el).parent());
               
            },

            error: function (err) {
                MessageError(err.responseText);
            }
        });
    }
}
function StatusApproved(orderId, status, text) {
    if (confirm(`Ban có chắc muốn xác nhận ${text} không?`)) {
        $.ajax({
            type: 'GET',
            url: "/Admin/Order/ChangeStatusOrder",
            contentType: 'application/json;charset=utf-8',
            data: { orderId, status },
            success: function (result) {
                if (result.status != 1) {
                    alert(result.message)
                }
                GetAll();
            },
            error: function (err) {
                MessageError(err.responseText);
            }
        });
    }
}

