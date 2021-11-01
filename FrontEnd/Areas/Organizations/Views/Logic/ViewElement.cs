using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using org.mariuszgromada.math.mxparser;
using ScadaCommon;

namespace FrontEnd.Areas.Organizations.Data
{
    public class ViewElement
    {
        public ViewType ViewType;

        [JsonProperty] private RegisterType m_registerType;
        [JsonProperty] private int m_registerAddress;
        [JsonProperty] private int m_clientId = 1;
        [JsonProperty] private string m_title;
        [JsonProperty] private string m_expression;
        [JsonIgnore] private List<DisplayedData> m_initialData;
        [JsonIgnore] private RegisterFrame m_frameValue;
        [JsonIgnore] private double m_displayedValue;
        [JsonIgnore] private bool m_updated = false;


        [JsonIgnore] public DateTime TimeStamp => m_frameValue?.Timestamp.ToLocalTime() ?? DateTime.Now;
        [JsonIgnore] public List<DisplayedData> InitialData => m_initialData;
        [JsonIgnore] public RegisterType RegisterType => m_registerType;
        [JsonIgnore] public string Title => m_title;
        [JsonIgnore] public string Expression => m_expression;
        [JsonIgnore] public int RegisterAddress => m_registerAddress;
        [JsonIgnore] public double Value => m_displayedValue;
        [JsonIgnore] public int ClientId => m_clientId;

        public void SetRegisterAddress(int address) => m_registerAddress = address;
        public void SetRegisterType(RegisterType registerType) => m_registerType = registerType;
        public void SetClientId(int id) => m_clientId = id;
        public void SetTitle(string title) => m_title = title;
        public void SetMathExpression(string expression) => m_expression = expression;

        public void SetValue(RegisterFrame value)
        {
            if (m_frameValue != null && m_frameValue.Timestamp == value.Timestamp)
                return;
            m_frameValue = value;
            m_displayedValue = CalculateDisplayedValue(m_frameValue.Value);
            m_updated = true;
        }

        private double CalculateDisplayedValue(int value)
        {
            if (String.IsNullOrEmpty(Expression))
                return value;
            Argument x = new Argument("x", value);
            Expression exp = new Expression(Expression, x);
            return exp.calculate();
        }

        public double? GetValue()
        {
            if (m_updated)
            {
                m_updated = false;
                return Value;
            }
            return null;
        }

        public void SetInitialData(List<RegisterFrame> dataSets)
        {
            CalculateInitialValues(dataSets);
            SetValue(dataSets.OrderByDescending(x => x.Timestamp).FirstOrDefault());
        }

        private void CalculateInitialValues(List<RegisterFrame> dataSets)
        {
            m_initialData = new List<DisplayedData>();
            foreach (RegisterFrame frame in dataSets)
            {
                m_initialData.Add(new DisplayedData()
                {
                    Value = CalculateDisplayedValue(frame.Value),
                    Timestamp = frame.Timestamp
                });
            }
        }
    }
}