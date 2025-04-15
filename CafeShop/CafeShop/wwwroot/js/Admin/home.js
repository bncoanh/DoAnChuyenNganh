$(document).ready(() => {
    ChartTopSale();
    ChartHardestToSell();
    ChartPuchase();
})

function getTopSale() {
    const topSale = $('#selected_top_sale').val()
    return new Promise(resolve => {
        $.get({
            url: `/Admin/Home/GetTopSale`,
            data: { topSale },
            success: data => resolve(data),
            error: error => resolve(null)
        })
    })
}

async function ChartTopSale() {
    let result = await getTopSale();
    let data = [];
    let categories = [];
    if (result) {
        result.forEach(res => {
            data.push(parseInt(res.totalSales));
            categories.push(res.name);
        })
    }

    var options = {
        series: [{
            name: 'Lượt bán',
            data: data
        }],
        chart: {
            height: 250,
            type: 'bar',
        },
        plotOptions: {
            bar: {
                borderRadius: 0,
                dataLabels: {
                    position: 'center', // top, center, bottom
                },
            }
        },
        dataLabels: {
            enabled: true,
            formatter: function (val) {
                return val;
            },
            offsetY: -20,
            style: {
                fontSize: '12px',
                colors: ["#304758"]
            }
        },

        xaxis: {
            categories: categories,
            position: 'bottom',
            axisBorder: {
                show: false
            },
            axisTicks: {
                show: false
            },
            crosshairs: {
                fill: {
                    type: 'gradient',
                    gradient: {
                        colorFrom: '#D8E3F0',
                        colorTo: '#BED1E6',
                        stops: [0, 100],
                        opacityFrom: 0.4,
                        opacityTo: 0.5,
                    }
                }
            },
            tooltip: {
                enabled: true,
            }
        },
        yaxis: {
            axisBorder: {
                show: false
            },
            axisTicks: {
                show: false,
            },
            labels: {
                show: false,
                formatter: function (val) {
                    return val + " sản phẩm";
                }
            }

        }
    };
    $("#chart_top_sale").html("");
    var chart = new ApexCharts(document.querySelector("#chart_top_sale"), options);
    chart.render();
}


function getHardestToSell() {
    const topSale = $('#selected_hardest_to_sell').val()
    return new Promise(resolve => {
        $.get({
            url: `/Admin/Home/GetHardestToSell`,
            data: { topSale },
            success: data => resolve(data),
            error: error => resolve(null)
        })
    })
}

async function ChartHardestToSell() {
    let result = await getHardestToSell();
    let data = [];
    let categories = [];

    if (result) {
        result.forEach(res => {
            data.push(parseInt(res.totalSales));
            categories.push(res.name);
        })
    }

    var options = {
        series: [{
            name: 'Lượt bán',
            data: data
        }],
        chart: {
            height: 250,
            type: 'bar',
        },
        plotOptions: {
            bar: {
                borderRadius: 0,
                dataLabels: {
                    position: 'center', // top, center, bottom
                },
            }
        },
        dataLabels: {
            enabled: true,
            formatter: function (val) {
                return val;
            },
            offsetY: -20,
            style: {
                fontSize: '12px',
                colors: ["#304758"]
            }
        },

        xaxis: {
            categories: categories,
            position: 'bottom',
            axisBorder: {
                show: false
            },
            axisTicks: {
                show: false
            },
            crosshairs: {
                fill: {
                    type: 'gradient',
                    gradient: {
                        colorFrom: '#D8E3F0',
                        colorTo: '#BED1E6',
                        stops: [0, 100],
                        opacityFrom: 0.4,
                        opacityTo: 0.5,
                    }
                }
            },
            tooltip: {
                enabled: true,
            }
        },
        yaxis: {
            axisBorder: {
                show: false
            },
            axisTicks: {
                show: false,
            },
            labels: {
                show: false,
                formatter: function (val) {
                    return val + " sản phẩm";
                }
            }

        }

    };
    $("#chart_hardest_to_sell").html("");
    var chart = new ApexCharts(document.querySelector("#chart_hardest_to_sell"), options);
    chart.render();
}

function getPuchase() {
    let dateStr = $('#inputMonth').val()
    let parts = dateStr.split('-');
    let year = parseInt(parts[0], 0);
    let month = parseInt(parts[1], 0);
    return new Promise(resolve => {
        $.get({
            url: `/Admin/Home/GetPuchase`,
            data: { month, year },
            success: data => resolve(data),
            error: error => resolve(null)
        })
    })
}

async function ChartPuchase() {
    let result = await getPuchase();
    let data = [];
    let categories = [];

    if (result) {
        result.forEach(res => {
            data.push(parseInt(res.totalMoney));
            let parts = res.dayInMonth.split('-');
            categories.push(parseInt(parts[0], 0));
        })
    }
    var options = {
        series: [{
            name: "VNĐ",
            data: data
        }],
        chart: {
            height: 250,
            type: 'bar',
        },
        plotOptions: {
            bar: {
                borderRadius: 0,
                dataLabels: {
                    position: 'center',
                },
            }
        },
        dataLabels: {
            enabled: true,
            formatter: function (val) {
                return val;
            },
            offsetY: -20,
            style: {
                fontSize: '12px',
                colors: ["#304758"]
            }
        },

        xaxis: {
            categories: categories,
            position: 'bottom',
            axisBorder: {
                show: false
            },
            axisTicks: {
                show: false
            },
            crosshairs: {
                fill: {
                    type: 'gradient',
                    gradient: {
                        colorFrom: '#D8E3F0',
                        colorTo: '#BED1E6',
                        stops: [0, 100],
                        opacityFrom: 0.4,
                        opacityTo: 0.5,
                    }
                }
            },
            tooltip: {
                enabled: true,
            }
        },
        yaxis: {
            axisBorder: {
                show: false
            },
            axisTicks: {
                show: false,
            },
            labels: {
                show: false,
                formatter: function (val) {
                    return val.toLocaleString() + " VNĐ";
                }
            }

        }

    };
    $("#chart_puchase").html("");
    var chart = new ApexCharts(document.querySelector("#chart_puchase"), options);
    chart.render();
}


function onchangeSelectTop(id, idText) {
    let val = $(id).val();
    if (idText == '#title_top_sale') {
        ChartTopSale();
        $(idText).text(`Top ${val} sản phẩm bán chạy`);
    }
    else {
        ChartHardestToSell();
        $(idText).text(`Top ${val} sản phẩm bán ít nhất`);
    }

}

function reloadChartPuchase() {
    let dateStr = $("#inputMonth").val();
    let month = dateStr.split("-")[1].replace(/^0+/, '');;
    let year = dateStr.split("-")[0].replace(/^0+/, '');;
    let text = `Doanh thu tháng ${month}-${year}`;

    $("#title_puchase").text(text);
    ChartPuchase();
}