var userId = document.getElementById("userId").value;
var ChannelName = document.getElementById("ChannelName").value;

const connection2 = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();
function fullfill() {
    console.log("Fullfill called");
    connection2.invoke("JoinGroup", ChannelName);
    connection2.invoke("ReadMessage", userId, ChannelName);
    //connection.invoke("JoinGroup", `UserGroup_${userId}`);
    //connection.invoke("ChatNotificationCount", userId);
}
function reject() {
    console.log("rejected connectoin");
}

connection2.start().then(fullfill, reject);

debugger
document.addEventListener("DOMContentLoaded", function () {
    const currentPage = window.location.href.split("/").pop().toLowerCase();
    if (currentPage.startsWith("groupchat")) {
        return;
    }
});


connection2.on("RecieveMessage", (message, senderId) => {
    //const currentPage = window.location.href.split("/").pop().toLowerCase();
    //if (currentPage.startsWith("userlisting")) {
       
    //}

    if (senderId != userId) {
        const ui = document.getElementById("chatlist");

        // Get current time formatted as HH:mm
        const now = new Date();
        const hours = now.getHours().toString().padStart(2, '0');
        const minutes = now.getMinutes().toString().padStart(2, '0');
        const timeFormatted = `${hours}:${minutes}`;

        ui.innerHTML += `
            <li class="in d-flex mb-3">
                <img alt="Avatar" src="https://bootdey.com/img/Content/avatar/avatar1.png" class="rounded-circle me-3" width="40" height="40" />
                <div class="message-content bg-secondary rounded p-2 shadow-sm">
                    <p class="mb-1 text-white">${message}</p>
                    <small class="text-white">${timeFormatted}</small>
                </div>
            </li>
        `;
    }
});

function sendMessage() {
    debugger
    const message = document.getElementById("message").value;
    if (message.trim() != "") {
    connection2.invoke("SendMessage", message, userId, ChannelName).catch(err => console.error(err.toString()));
    const ui = document.getElementById("chatlist");
    const now = new Date();
    const hours = now.getHours().toString().padStart(2, '0');
    const minutes = now.getMinutes().toString().padStart(2, '0');
    const timeFormatted = `${hours}:${minutes}`;
    ui.innerHTML += `
               <li class=" out d-flex justify-content-end mb-3">
                <div class="message-content bg-primary text-white rounded p-2 shadow-sm me-3 text-end">
                    <p class="mb-1">${message}</p>
                        <small class="text-white">${timeFormatted}</small>
                </div>
                <img alt="Avatar" src="https://bootdey.com/img/Content/avatar/avatar6.png" class="rounded-circle" width="40" height="40" />
                </li>
                `;
    
    }
    document.getElementById("message").value = "";
}
