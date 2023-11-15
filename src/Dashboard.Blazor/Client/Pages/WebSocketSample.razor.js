export function initElements() {
  let connectionUrl = document.getElementById("connectionUrl");
  let connectButton = document.getElementById("connectButton");
  let stateLabel = document.getElementById("stateLabel");
  let sendMessage = document.getElementById("sendMessage");
  let sendButton = document.getElementById("sendButton");
  let commsLog = document.getElementById("commsLog");
  let closeButton = document.getElementById("closeButton");
  let socket;

  let scheme = document.location.protocol === "https:" ? "wss" : "ws";
  let port = document.location.port ? (":" + document.location.port) : "";

  connectionUrl.value = scheme + "://" + document.location.hostname + port + "/ws";

  closeButton.onclick = function () {
    if (!socket || socket.readyState !== WebSocket.OPEN) {
      alert("socket not connected");
    }
    socket.close(1000, "Closing from client");
  };

  sendButton.onclick = function () {
    if (!socket || socket.readyState !== WebSocket.OPEN) {
      alert("socket not connected");
    }
    var data = sendMessage.value;
    socket.send(data);
    commsLog.innerHTML += '<tr>' +
      '<td class="commslog-client">Client</td>' +
      '<td class="commslog-server">Server</td>' +
      '<td class="commslog-data">' + htmlEscape(data) + '</td></tr>';
  };

  connectButton.onclick = function () {
    stateLabel.innerHTML = "Connecting";
    socket = new WebSocket(connectionUrl.value);
    socket.onopen = function (event) {
      updateState();
      commsLog.innerHTML += '<tr>' +
        '<td colspan="3" class="commslog-data">Connection opened</td>' +
        '</tr>';
    };
    socket.onclose = function (event) {
      updateState();
      commsLog.innerHTML += '<tr>' +
        '<td colspan="3" class="commslog-data">Connection closed. Code: ' + htmlEscape(event.code) + '. Reason: ' + htmlEscape(event.reason) + '</td>' +
        '</tr>';
    };
    socket.onerror = updateState;
    socket.onmessage = function (event) {
      commsLog.innerHTML += '<tr>' +
        '<td class="commslog-server">Server</td>' +
        '<td class="commslog-client">Client</td>' +
        '<td class="commslog-data">' + htmlEscape(event.data) + '</td></tr>';
    };
  };

  function updateState() {
    function disable() {
      sendMessage.disabled = true;
      sendButton.disabled = true;
      closeButton.disabled = true;
    }
    function enable() {
      sendMessage.disabled = false;
      sendButton.disabled = false;
      closeButton.disabled = false;
    }

    connectionUrl.disabled = true;
    connectButton.disabled = true;

    if (!socket) {
      disable();
    } else {
      switch (socket.readyState) {
        case WebSocket.CLOSED:
          stateLabel.innerHTML = "Closed";
          disable();
          connectionUrl.disabled = false;
          connectButton.disabled = false;
          break;
        case WebSocket.CLOSING:
          stateLabel.innerHTML = "Closing...";
          disable();
          break;
        case WebSocket.CONNECTING:
          stateLabel.innerHTML = "Connecting...";
          disable();
          break;
        case WebSocket.OPEN:
          stateLabel.innerHTML = "Open";
          enable();
          break;
        default:
          stateLabel.innerHTML = "Unknown WebSocket State: " + htmlEscape(socket.readyState);
          disable();
          break;
      }
    }
  };


  function htmlEscape(str) {
    return str.toString()
      .replace(/&/g, '&amp;')
      .replace(/"/g, '&quot;')
      .replace(/'/g, '&#39;')
      .replace(/</g, '&lt;')
      .replace(/>/g, '&gt;');
  };
}

