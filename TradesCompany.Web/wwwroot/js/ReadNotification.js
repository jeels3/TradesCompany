const connection2 = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();
function reject() {
    console.log("Reject called");
}
function fullfill() {
    console.log("Fullfill called");
    connection2.invoke("ReadNotification", userId);
}

connection2.start().then(fullfill, reject);

var userId = document.getElementById("userId").value;