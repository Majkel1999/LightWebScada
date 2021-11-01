using System;

public class ChartItem
{
    public double Value { get; set; }
    public DateTime Date { get; set; }

    public ChartItem(double value, DateTime date)
    {
        Value = value;
        Date = date;
    }
}