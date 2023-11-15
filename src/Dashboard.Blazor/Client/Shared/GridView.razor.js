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
