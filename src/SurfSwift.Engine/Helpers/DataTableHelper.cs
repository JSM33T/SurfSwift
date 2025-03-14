using Microsoft.Playwright;
using System.Data;

namespace SurfSwift.Engine.Helpers
{

    /// <summary>
    /// Provides helper methods for extracting a table's data from a Playwright page into a DataTable.
    /// </summary>
    public static class DataTableHelper
    {
        /// <summary>
        /// Extracts a table's data from the specified container on the page and returns it as a DataTable.
        /// </summary>
        /// <param name="page">The Playwright page instance.</param>
        /// <param name="containerSelector">The CSS selector for the container that holds the table (default is "table").</param>
        /// <returns>A DataTable containing the extracted table data.</returns>
        /// <exception cref="Exception">Thrown if the table is not found within the specified container.</exception>
        public static async Task<DataTable> ExtractTableAsync(IPage page, string containerSelector = "table")
        {
            var dataTable = new DataTable();

            var table = await page.QuerySelectorAsync($"{containerSelector} table")
                ?? throw new Exception("Table not found in the specified container");

            // Extract headers (th elements)
            var headers = await table.QuerySelectorAllAsync("thead tr th, tr:first-child th, tr:first-child td");
            foreach (var header in headers)
            {
                var headerText = await header.InnerTextAsync();
                dataTable.Columns.Add(headerText?.Trim()
                                      ?? $"Column{dataTable.Columns.Count + 1}");
            }

            // Extract rows (skip header if already processed)
            var rows = await table.QuerySelectorAllAsync("tbody tr, tr:not(:first-child)");
            foreach (var row in rows)
            {
                var cells = await row.QuerySelectorAllAsync("td");

                if (cells.Count != dataTable.Columns.Count)
                    continue;

                var rowData = new List<string>();

                foreach (var cell in cells)
                    rowData.Add(await cell.InnerTextAsync());

                dataTable.Rows.Add(rowData.ToArray());
            }

            return dataTable;
        }
    }
}
