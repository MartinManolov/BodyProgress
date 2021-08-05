var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

connection.on("NewMessage", function (message) {
    var msg = message.text.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");

    var encodedMsg = '<div class="container-fluid, border-dark shadow-lg rounded" style="border-block:medium">' +
        '<div class="row" >' +
                '<div class="col">' +
                    '<a class="nav-item text-dark" href="/Profiles/Info?username=@user.SenderUsername">' +
        message.senderUsername +
                    '</a>' +
                '</div>' +
                '<div class="col-sm-auto">' +
                    '<p class="text-secondary">' +
                    message.date +
                    '</p>' +
                '</div>' +
            '</div >' +
        '<p class="text-primary">' +
            message.text +
           '</p>'+
        '</div >';

    document.getElementById("chatBox").insertAdjacentHTML("beforeend", encodedMsg);
});

connection.start().catch(function (err) {
    return console.error(err.toString())
});

    document.getElementById("sendMessageButton").addEventListener("click", function (event) {

        var message = document.getElementById("messageInput").value;

        connection.invoke("SendToAll", message).catch(function (err) {

            return console.error(err.toString());

        });

        event.preventDefault();

        document.getElementById("messageInput").value = "";

    })