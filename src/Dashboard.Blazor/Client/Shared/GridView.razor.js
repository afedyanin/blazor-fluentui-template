import perspective from "https://cdn.jsdelivr.net/npm/@finos/perspective/dist/cdn/perspective.js";

export async function loadDataGrid(schema, data, view) {
  const worker = perspective.worker();
  const table = await worker.table(schema);

  table.update(data);
  view.load(table);
}
export async function fetchDataGrid(schema, url, view) {
  const worker = perspective.worker();

  const table = await worker.table(schema);
  let resp = await fetch(url);
  let json = await resp.json();

  table.update(json);
  view.load(table);
}
export async function fetchParquet(url, view) {
  const worker = perspective.worker();
  let resp = await fetch(url);
  let ab = await resp.arrayBuffer();
  const table = await worker.table(ab);
  view.load(table);
}

export async function dispose() {

  if (view) {
    await view.delete();
  }

  if (table) {
    await table.delete();
  }
}

