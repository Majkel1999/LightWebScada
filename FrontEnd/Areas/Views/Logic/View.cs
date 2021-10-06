using System.Collections.Generic;
using Newtonsoft.Json;

namespace FrontEnd.Areas.Organizations.Data
{
    public class View
    {
        public string Name { get; set; }

        [JsonProperty] private List<ViewRow> m_viewRows = new();

        [JsonIgnore] public List<ViewRow> Rows => m_viewRows;
        [JsonIgnore] public int RowsCount => m_viewRows.Count;

        public void AddToFirstOpen(ViewElement element)
        {
            foreach (var row in m_viewRows)
            {
                if (!row.Full)
                {
                    row.AddElement(element);
                    return;
                }
            }
            ViewRow newRow = AddRow();
            newRow.AddElement(element);
        }

        public void Remove(ViewElement element)
        {
            foreach (var row in m_viewRows)
            {
                if (row.Elements.Contains(element))
                {
                    row.Elements.Remove(element);
                    break;
                }
            }
            CheckRows();
        }

        private void CheckRows()
        {
            for (int i = 0; i < m_viewRows.Count; i++)
            {
                if (m_viewRows[i].Elements.Count == 0)
                {
                    m_viewRows.RemoveAt(i);
                    i--;
                }
            }
        }

        private ViewRow AddRow(ViewRow rowItems = null)
        {
            ViewRow row = rowItems ?? new ViewRow();
            m_viewRows.Add(row);
            return row;
        }
    }
}