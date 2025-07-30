var currentData = null;
var chartInstance = null;

const chartTypeSelect = document.getElementById("charttype");
const dataTypeSelect = document.getElementById("datatype");

function renderChart(type, labels, values) {
    const ctx = document.getElementById('myChart').getContext('2d');

    if (chartInstance) {
        chartInstance.destroy();
    }

    chartInstance = new Chart(ctx, {
        type: type,
        data: {
            labels: labels,
            datasets: [{
                label: 'Service Revenue',
                backgroundColor: labels.map(() => generateRandomRGB()),
                borderColor: labels.map(() => generateRandomRGB()),
                data: values
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false
        }
    });
}

function generateRandomRGB() {
    const r = Math.floor(Math.random() * 256);
    const g = Math.floor(Math.random() * 256);
    const b = Math.floor(Math.random() * 256);
    return `rgb(${r}, ${g}, ${b})`;
}

function updateChart() {
    const selectedType = chartTypeSelect.value;
    const selectedDataType = dataTypeSelect.value;

    $.ajax({
        type: 'POST',
        url: `/Admin/ChartData?type=${selectedDataType}`,
        success: function (response) {
            const labels = response.map(item => item.ServiceName);
            const values = response.map(item => item.Revenue);

            currentData = response;
            renderChart(selectedType, labels, values);
        },
        error: function (error) {
            console.error("Failed to load chart data:", error);
        }
    });
}

// Event: Data type or chart type change
chartTypeSelect.addEventListener('change', updateChart);
dataTypeSelect.addEventListener('change', updateChart);

// Initial chart render
const initialLabels = currentData.map(x => x.ServiceName);
const initialValues = currentData.map(x => x.Revenue);
renderChart(chartTypeSelect.value, initialLabels, initialValues);

// Excel download
$('#downloadbtn').on('click', function () {
    if (confirm('Are you sure you want to download this Excel?')) {
        $.ajax({
            type: 'POST',
            url: '/Admin/DownloadExcel',
            data: {
                data: currentData
            },
            xhrFields: {
                responseType: 'blob'
            },
            success: function (response) {
                const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
                const url = URL.createObjectURL(blob);
                const a = document.createElement("a");
                a.href = url;
                a.download = "chart_data.xlsx";
                document.body.appendChild(a);
                a.click();
                document.body.removeChild(a);
                URL.revokeObjectURL(url);
            },
            error: function (xhr, status, error) {
                console.error("Error downloading Excel:", error);
            }
        });
    }
});