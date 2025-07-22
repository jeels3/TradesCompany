
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();

debugger
connection.on("ReceiveBookingNotification",  Message => {
    Console.log("Notification", Message)
    alert(Message);
});

function fullfill() {
    console.log("Fullfill called");
    var userId = document.getElementById("userId").value;
    connection.invoke("JoinGroup", `UserGroup_${userId}`);
}

function reject() {
    console.log("Reject called");
}

connection.start().then(fullfill, reject);

