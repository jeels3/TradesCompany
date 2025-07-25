$(document).load(function () {
    $.ajax({
        url: `/Common/ShowNotificationCount`,
        method: 'GET',
        success: function (res) {
            alert(res.message);
            location.reload();
        },
        error: function (xhr) {
            const err = xhr.responseJSON?.message || 'Error Scheduling Request';
            alert(err);
        }
    });
});