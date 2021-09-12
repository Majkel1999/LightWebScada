using System.Collections.Generic;
using Newtonsoft.Json;

namespace FrontEnd.Areas.Organizations.Data
{
    public class ViewRow
    {
        private const int MaximumElementsInRow = 3;

        [JsonProperty]
        private readonly List<ViewElement> m_elements = new();

        [JsonIgnore]
        public List<ViewElement> Elements => m_elements;
        [JsonIgnore]
        public int Count => m_elements.Count;
        [JsonIgnore]
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