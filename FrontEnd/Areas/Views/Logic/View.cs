using System.Collections.Generic;

namespace FrontEnd.Areas.Organizations.Data
{
    public class View
    {
        private List<List<ViewElement>> m_viewElements = new List<List<ViewElement>>();

        public List<List<ViewElement>> Elements => m_viewElements;
        public int Rows => m_viewElements.Count;
        public int ElementsInRow(int row) => m_viewElements[row].Count;

        public void AddRow(List<ViewElement> rowItems = null)
        {
            m_viewElements.Add(rowItems ?? new List<ViewElement>());
        }

        public void AddElement(ViewElement element, int row)
        {
            if (row > m_viewElements.Count - 1)
                return;
            m_viewElements[row].Add(element);
        }
    }
}