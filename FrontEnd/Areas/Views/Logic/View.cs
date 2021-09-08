using System.Collections.Generic;

namespace FrontEnd.Areas.Organizations.Data
{
    public class View
    {
        private readonly List<ViewRow> m_viewRows = new();

        public List<ViewRow> Rows => m_viewRows;
        public int RowsCount => m_viewRows.Count;

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
            var newRow = AddRow();
            newRow.AddElement(element);
        }

        private ViewRow AddRow(ViewRow rowItems = null)
        {
            var row = rowItems ?? new ViewRow();
            m_viewRows.Add(row);
            return row;
        }
    }
}