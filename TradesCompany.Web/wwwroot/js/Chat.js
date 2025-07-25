var userId = document.getElementById("userId").value;
var ChannelName = document.getElementById("ChannelName").value;

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();
function fullfill() {
    console.log("Fullfill called");
    //alert(userId);
    connection.invoke("JoinGroup", `UserGroup_${userId}`);
    connection.invoke("JoinGroup", ChannelName);
}

connection.on("RecieveMessage", (message, senderId) => {
    if (senderId != userId) {
    const ui = document.getElementById("chatlist");
    ui.innerHTML += `<li class="in">
                    <div class="chat-img">
                        <img alt="Avtar" src="https://bootdey.com/img/Content/avatar/avatar1.png">
                    </div>
                    <div class="chat-body">
                        <div class="chat-message">
                            <h5>user</h5>
                            <p>${message}</p>
                        </div>
                    </div>
                </li>`;
    }
})

function sendMessage() {
    const message = document.getElementById("message").value;
    connection.invoke("SendMessage", message, userId, ChannelName).catch(err => console.error(err.toString()));
    const ui = document.getElementById("chatlist");
    ui.innerHTML += `
    <li class="out">
        <div class="chat-img">
            <img alt="Avtar" src="https://bootdey.com/img/Content/avatar/avatar6.png">
        </div>
        <div class="chat-body">
            <div class="chat-message">
                <div class="chat-message">
                    <p>This wala</p>
                    <h5>UserName</h5>
                    <p>${message}</p>
                </div>
            </div>
        </div>
    </li>
    `;
    message = "";
}
function reject() {
    console.log("Reject called");
}
connection.start().then(fullfill, reject);