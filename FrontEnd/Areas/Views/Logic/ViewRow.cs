using System.Collections.Generic;

namespace FrontEnd.Areas.Organizations.Data
{
    public class ViewRow
    {
        private const int MaximumElementsInRow = 3;

        private readonly List<ViewElement> m_elements = new();

        public List<ViewElement> Elements => m_elements;
        public int Count => m_elements.Count;
        public bool Full => Count >= MaximumElementsInRow;

        public bool AddElement(ViewElement element)
        {
            if (Count >= MaximumElementsInRow)
                return false;
            m_elements.Add(element);
            return true;
        }
    }
}