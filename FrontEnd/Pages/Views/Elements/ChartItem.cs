using System;

public class ChartItem
{
    public int Value { get; set; }
    public DateTime Date { get; set; }

    public ChartItem(int value, DateTime date)
    {
        Value = value;
        Date = date;
    }
}