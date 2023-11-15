import perspective from "https://cdn.jsdelivr.net/npm/@finos/perspective/dist/cdn/perspective.js";

export async function loadDataGrid(schema, data, view) {
  const worker = perspective.worker();

  let table = await worker.table(schema);

  table.update(data);
  view.load(table);
}
export async function fetchDataGrid(schema, url, view) {
  const worker = perspective.worker();

  let table = await worker.table(schema);
  let resp = await fetch(url);
  let json = await resp.json();

  table.update(json);
  view.load(table);
}


