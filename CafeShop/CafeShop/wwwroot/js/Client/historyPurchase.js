
$(document).ready(function (e) {
    GetAll();
});

function GetAll() {
    let _url = "/Order/GetAll?";
    var request = $('#request').val();
    if (request.length > 0) {
        _url += "request=" + request;
    }

    $.ajax({
        url: _url,
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            var html = '';
            $.each(data.data, function (index, item) {
                let btnAprroved = `<button class = "btn btn-sm btn-danger" onclick="StatusApproved(${item.id},3)">Hủy đơn</button>`;

                html += `<tr class="align-middle">
                            <td scope="col" class="text-center"><a style="color: blue;  cursor: pointer; text-decoration: none;"  href="/Order/OrderDetails?OrderId=${item.id}">${item.orderCode}</a></td>
                            <td scope="col" class="text-center">${item.dateFormat}</td>
                            <td scope="col" class="">${item.statusText}</td>
                            <td scope="col" class="text-center">${item.totalMoney.toLocaleString('en-US')} VNĐ</td>
                            <td scope="col">${item.customerName}</td>
                            <td scope="col">${item.phoneNumber}</td>
                            <td scope="col">${item.address}</td>
                            <td scope="col" class="text-center">
                                ${item.status > 0 ? "" : btnAprroved}
                            </td>
                        </tr>`;
            })
            $('#tbody').html(html);
        },

        error: function (err) {
            MessageError(err.responseText);
        }
    });
}
$('#request').keydown(function (e) {
    if (e.keyCode == 13) {
        pageNumber = 1;
        GetAll();
    }
})

function StatusApproved(orderId, status) {
    if (confirm(`Ban có chắc muốn xác nhận hủy đơn hàng này không?`)) {
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