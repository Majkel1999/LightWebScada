using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using ScadaCommon;

namespace FrontEnd.Areas.Organizations.Data
{
    public class View
    {
        public string Name { get; set; }

        [JsonProperty] private List<ViewRow> m_viewRows = new();

        [JsonIgnore] public List<ViewRow> Rows => m_viewRows;
        [JsonIgnore] public int RowsCount => m_viewRows.Count;

        /// <summary>
        /// Fetches all neded registers for view update.
        /// Does not duplicate registers, even if two ViewElements use the same register
        /// from the same client.
        /// </summary>
        /// <returns>List where
        /// Item1 -> register address
        /// Item2 -> client Id
        /// Item3 -> registerType
        /// </returns>
        public List<(int, int, RegisterType)> GetRegisters()
        {
            List<(int, int, RegisterType)> registers = new List<(int, int, RegisterType)>();
            foreach (ViewRow row in Rows)
            {
                foreach (ViewElement element in row.Elements)
                {
                    if (registers.Any(x => x.Item1 == element.RegisterAddress &&
                        x.Item2 == element.ClientId &&
                        x.Item3 == element.RegisterType))
                        continue;
                    registers.Add((element.RegisterAddress,element.ClientId, element.RegisterType));
                }
            }
            return registers;
        }

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