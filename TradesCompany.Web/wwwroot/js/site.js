var userId = document.getElementById("userId").value;
//var ChannelName = document.getElementById("ChannelName").value; // only get if we are on chatting page

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();
function reject() {
    console.log("Reject called");
}

function fullfill() {
    console.log("Fullfill called");
    connection.invoke("JoinGroup", `UserGroup_${userId}`);
    //connection.invoke("JoinGroup", ChannelName);
    connection.invoke("NotificationCount", userId);
    //connection.invoke("ReadMessage", userId, ChannelName); // update this line if needed
    connection.invoke("ChatNotificationCount", userId);
}

connection.start().then(fullfill, reject);

let count = 0;
function showComplaintCount() {
    const dot = document.getElementById("notificationcnt");
    if (dot) {
        count++;
        dot.textContent = count;
    }
}

connection.on("ReceiveBookingNotification", (NotificationType, message) => {
    const currentPage = window.location.href.split("/").pop().toLowerCase();
    if (currentPage !== "notification") {
        showComplaintCount()
        return;
    }
    connection.invoke("ReadNotification", userId);
});

connection.on("ReceiveNewQuotation", (NotificationType, Message) => {
    const currentPage = window.location.href.split("/").pop().toLowerCase();

    if (currentPage !== "notification") {
        showComplaintCount()
        return;
    }
    connection.invoke("ReadNotification", userId);
});

connection.on("ReceiveNewSchedule", (NotificationType, Message) => {
    const currentPage = window.location.href.split("/").pop().toLowerCase();

    if (currentPage !== "notification") {
        showComplaintCount()
        return;
    }
    connection.invoke("ReadNotification", userId);
});

connection.on("RecieveScheduleSeviceReminder", (NotificationType, Message) => {
    const currentPage = window.location.href.split("/").pop().toLowerCase();

    if (currentPage !== "notification") {
        showComplaintCount()
        return;
    }
    connection.invoke("ReadNotification", userId);
});

connection.on("ReceiveNotificationCount", (count) => {
    const currentPage = window.location.href.split("/").pop().toLowerCase();
    if (currentPage !== "notification") {
        document.getElementById("notificationcnt").innerText = count;
        return;
    }
})



connection.on("ReceiveChatNotificationCount", (count) => {
    //alert("Chat Count : ", count);
    const notificationCountElement = document.getElementById("chatnotificationcnt");
    if (notificationCountElement) {
        notificationCountElement.textContent = count > 0 ? count : '';
    }
});





