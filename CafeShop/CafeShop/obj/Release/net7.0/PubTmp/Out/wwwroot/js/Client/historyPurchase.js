
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
            console.log(data);
            $.each(data.data, function (index, item) {
                html += `<tr class="align-middle">
                            <td scope="col" class="text-center">${item.orderCode}</td>
                            <td scope="col" class="text-center">${item.dateFormat}</td>
                            <td scope="col" class="text-center">${item.statusText}</td>
                            <td scope="col" class="text-center">${item.totalMoney.toLocaleString('en-US')} VNĐ</td>
                            <td scope="col">${item.customerName}</td>
                            <td scope="col">${item.phoneNumber}</td>
                            <td scope="col">${item.address}</td>
                            <td scope="col" class="text-center"> <a href="/Order/OrderDetails?OrderId=${item.id}">Chi tiết</a></td>
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