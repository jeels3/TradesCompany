var userId = document.getElementById("userId").value;

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();

connection.on("ReceiveBookingNotification", (NotificationType, message) => {
    alert(message);
    const currentPage = window.location.href.split("/").pop().toLowerCase();
    if (currentPage !== "notification") {
        //document.getElementById("notificationcnt").value += 10;
        return;
    }
    connection.invoke("ReadNotification",userId);
});

connection.on("ReceiveNewQuotation", (NotificationType, Message) => {
    const currentPage = window.location.href.split("/").pop().toLowerCase();

    if (currentPage !== "notification") {
        console.log("Notification");
        return;
    }
    connection.invoke("ReadNotification", userId);
});

connection.on("ReceiveNewSchedule", (NotificationType, Message) => {
    const currentPage = window.location.href.split("/").pop().toLowerCase();

    if (currentPage !== "notification") {
        console.log("Notification");
        return;
    }
    connection.invoke("ReadNotification", userId);
});

connection.on("RecieveScheduleSeviceReminder", (NotificationType, Message) => {
    const currentPage = window.location.href.split("/").pop().toLowerCase();

    if (currentPage !== "notification") {
        //showComplaintCount();
        console.log("Notification");
        return;
    }
    connection.invoke("ReadNotification", userId);
});

function fullfill() {
    console.log("Fullfill called");
    //alert(userId);
    connection.invoke("JoinGroup", `UserGroup_${userId}`);
}
function reject() {
    console.log("Reject called");
}

connection.start().then(fullfill, reject);

