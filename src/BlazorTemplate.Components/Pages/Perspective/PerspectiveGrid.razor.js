import perspective from "https://cdn.jsdelivr.net/npm/@finos/perspective/dist/cdn/perspective.js";

export async function loadJson(schema, data, view) {
  const worker = perspective.worker();
  const table = await worker.table(schema);
  table.update(data);
  view.load(table);
}

export async function fetchJson(schema, url, view) {
  const worker = perspective.worker();
  const table = await worker.table(schema);
  let resp = await fetch(url);
  let json = await resp.json();

  table.update(json);
  view.load(table);
}

export async function fetchArrow(url, view) {
  const worker = perspective.worker();
  let resp = await fetch(url);
  let ab = await resp.arrayBuffer();
  const table = await worker.table(ab);
  view.load(table);
}

export async function fetchWebSocket(schema, view) {

  let scheme = document.location.protocol === "https:" ? "wss" : "ws";
  let port = document.location.port ? (":" + document.location.port) : "";
  let connectionUrl = scheme + "://" + document.location.hostname + port + "/tripdata";
  const socket = new WebSocket(connectionUrl);
  const worker = perspective.worker();

  let resp = await fetch(schema);
  let jsonSchema = await resp.json();
    const table = await worker.table(jsonSchema);
  view.load(table);
  
  view.addEventListener("perspective-click", function (event) {
    console.info("Click event fired!");
  });

  view.addEventListener("perspective-config-update", function (event) {
    console.log("The config has changed!");
  });
  view.addEventListener("perspective-view-update", function (event) {
    console.log("The view updated!");
  });

  view.addEventListener("change", function (event) {
    console.log("View has changed!");
  });
  
  socket.onopen = function (event) {
    console.info("socket open");
  };

  socket.onclose = function (event) {
    console.info("socket closed");

    const plugin = document.querySelector("perspective-viewer-datagrid regular-table");

    plugin.addEventListener("scroll", (event) => {
      console.info("Scroll event fired!");
    });
  };


  socket.onerror = function (event) {
    console.info("socket error: " + event);
  };

  socket.onmessage = async function (event) {
    console.info("socket data received");
    let data = await event.data.arrayBuffer();
    table.update(data);
  };
}

export async function fetchServerSide(tablename, viewer) {

  // Create two perspective interfaces, one remotely via WebSocket,
  // and one local via WebWorker.
  const url = window.location.origin.replace("https", "wss");
  const websocket = perspective.websocket(url+"/tripdata");
  const worker = perspective.worker();

  // Open a `Table` that is hosted on the server. All instructions
  // will be proxied to the server `Table` - no calculations are
  // done on the client.
  const remote_table = await websocket.open_table(tablename);
  const view = await remote_table.view();

  // Create a `table` from this, owned by the local WebWorker.
  // Data is transferred from `view` to the local WebWorker, both
  // the current state and all future updates, as Arrows.
  const local_table = await worker.table(view, { limit: 10000 });

  // Load this in the `<perspective-viewer>`.
  viewer.load(local_table);
}

export async function dispose() {

  if (view) {
    await view.delete();
  }

  if (table) {
    await table.delete();
  }
}

