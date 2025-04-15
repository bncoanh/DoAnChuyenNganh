$(document).ready(function (e) {
    GetAll();
    GetAllProductType();
});
var pageNumber = 1;
var totalPage = 0;
var typeId = 0;
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

function Pagination() {
    var html = ``;
    if (totalPage == 1) {
        html += ` <li class="page-item disabled">
                            <a class="page-link active rounded-0 mr-3 shadow-sm border-top-0 border-left-0" tabindex="-1">1</a>
                        </li>`
    }
    else if (pageNumber == 1) {
        html += `<li class="page-item disabled"> <a class="page-link active rounded-0 mr-3 shadow-sm border-top-0 border-left-0" tabindex="-1">${pageNumber}</a> </li>
                <li class="page-item"> <a onclick="nextPage()" class="page-link rounded-0 mr-3 shadow-sm border-top-0 border-left-0 text-dark" >${pageNumber + 1}</a> </li>`;
    }
    else if (pageNumber == totalPage) {
        html += `
                <li class="page-item"> <a onclick="previousPage()" class="page-link rounded-0 mr-3 shadow-sm border-top-0 border-left-0 text-dark" >${pageNumber - 1}</a> </li>
                <li class="page-item disabled"> <a class="page-link active rounded-0 mr-3 shadow-sm border-top-0 border-left-0" tabindex="-1">${pageNumber}</a> </li>`;
    }
    else {
        html += `<li class="page-item"> <a onclick="previousPage()" class="page-link rounded-0 mr-3 shadow-sm border-top-0 border-left-0 text-dark" >${pageNumber - 1}</a> </li>
                <li class="page-item disabled"> <a class="page-link active rounded-0 mr-3 shadow-sm border-top-0 border-left-0" tabindex="-1">${pageNumber}</a> </li>
                <li class="page-item"> <a onclick="nextPage()" class="page-link rounded-0 mr-3 shadow-sm border-top-0 border-left-0 text-dark" >${pageNumber + 1}</a> </li>`;
    }
    html += ``;

    $('#pagination').html(html);
}
$('#btn_search').click(function () {
    pageNumber = 1;
    GetAll();
})
function GetAll() {
    let _url = "/Shop/GetALlProduct?";
    var request = $('#request').val();
    var groupId = $('#groupId option:selected').val();
    if (request.length > 0) {
        _url += "request=" + request + "&";
    }
    _url += `typeId=${typeId}&pageNumber=${pageNumber}`;

    $.ajax({
        url: _url,
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            var html = '';
            $.each(data.result.data, function (index, item) {
                html += `<div class="col-md-3" style="font-size: 10px; text-align: center;">
                        <div class="card mb-4 product-wap rounded-0">
                            <div class="card rounded-0">
                                <img class="card-img rounded-0 img-fluid" src="${item.imageUrl}" />
                                <div class="card-img-overlay rounded-0 product-overlay d-flex align-items-center justify-content-center" >
                                    <ul class="list-unstyled">
                                        <li><a class="btn btn-success text-white mt-2" href="Shop/IndexDetails?prId=${item.id}"><i class="bi bi-eye text-light"></i></a></li>
                                        <li><a class="btn btn-success text-white mt-2" href="Shop/IndexDetails?prId=${item.id}"><i class="bi bi-cart-plus text-light"></i></a></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="card-body" style="height: 160px;">
                            <div style="height: 50px;">
                                <a href="Shop/IndexDetails?prId=${item.id}" class="h3 text-decoration-none" >${item.name}</a>
                                </div>
                                <ul class="list-unstyled d-flex justify-content-center mb-1">
                                    <li>
                                        <i class="text-warning bi bi-star-fill"></i>
                                        <i class="text-warning bi bi-star-fill"></i>
                                        <i class="text-warning bi bi-star-fill"></i>
                                        <i class="text-warning bi bi-star-fill"></i>
                                        <i class="text-warning bi bi-star-fill"></i>

                                    </li>
                                </ul>
                                <p class="text-center mb-0">${item.price.toLocaleString('en-US')} VNĐ</p>
                            </div>
                        </div>
                    </div>`;
            })
            let total = Math.ceil(data.result.total.totalCount / 12);
            totalPage = total > 0 ? total : 1;
            $('#list_product').html(html);
            Pagination();
        },

        error: function (err) {
            MessageError(err.responseText);
        }
    });
}
function GetAllProductType() {
    $.ajax({
        type: 'GET',
        url: "/Shop/GetAllProductType",
        contentType: 'application/json;charset=utf-8',
        data: {},
        success: function (result) {
            let htmlCoffe = ``;
            let htmlTea = ``;
            let htmlDifferent = ``;
            result.coffe.forEach(e => {
                htmlCoffe += `<li class="ps-2 mt-1"><a class="text-decoration-none fs-5 ps-2" href="#" onclick="changeType(${e.id})">${e.name}</a></li>`;
            })
            result.tea.forEach(e => {
                htmlTea += `<li class="ps-2 mt-1"><a class="text-decoration-none fs-5 ps-2" href="#" onclick="changeType(${e.id})">${e.name}</a></li>`;
            })
            result.different.forEach(e => {
                htmlDifferent += `<li class="ps-2 mt-1"><a class="text-decoration-none fs-5 ps-2" href="#" onclick="changeType(${e.id})">${e.name}</a></li>`;
            })

            $("#coffe_list").html(htmlCoffe);
            $("#tea_list").html(htmlTea);
            $("#different_list").html(htmlDifferent);
        },
        error: function (err) {
            MessageError(err.responseText);
        }
    });
}

function changeType(type){
    typeId = type;
    pageNumber = 1;
    GetAll();
}