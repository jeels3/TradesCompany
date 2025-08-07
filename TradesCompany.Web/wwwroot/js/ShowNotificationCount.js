const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();
function reject() {
    console.log("Reject called");
}

function fullfill() {
    console.log("Fulfill successfull");
    connection.invoke("NotificationCount", userId);
    connection.invoke("ChatNotificationCount", userId);
    connection.invoke("JoinGroup", `UserGroup_${userId}`);
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
