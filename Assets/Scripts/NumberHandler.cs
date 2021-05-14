using static System.Environment;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class NumberHandler
{
    private static readonly string format0 = "f0";
    private static readonly string format1 = "f1";
    private static readonly string format2 = "f2";
    private static readonly KeyValuePair<double, string>[] numAbbrevs = new KeyValuePair<double, string>[]
    {
        new KeyValuePair<double, string>(Mathf.Pow(10, 3), "K"),
        new KeyValuePair<double, string>(Mathf.Pow(10, 6), "M"),
        new KeyValuePair<double, string>(Mathf.Pow(10, 9), "B"),
        new KeyValuePair<double, string>(Mathf.Pow(10, 12), "T"),
        new KeyValuePair<double, string>(Mathf.Pow(10, 15), "q"),
        new KeyValuePair<double, string>(Mathf.Pow(10, 18), "Q"),
        new KeyValuePair<double, string>(Mathf.Pow(10, 21), "s"),
        new KeyValuePair<double, string>(Mathf.Pow(10, 24), "S"),
        new KeyValuePair<double, string>(Mathf.Pow(10, 27), "O"),
        new KeyValuePair<double, string>(Mathf.Pow(10, 30), "N"),
        new KeyValuePair<double, string>(Mathf.Pow(10, 33), "d"),
        new KeyValuePair<double, string>(Mathf.Pow(10, 33), "U"),
        new KeyValuePair<double, string>(Mathf.Pow(10, 36), "D"),
    }; 

    public static string NumberToTextInOneLine(this double number, bool withFraction = false)
    {
        foreach (var item in numAbbrevs.Reverse())
        {
            if (number >= item.Key)
            {
                if (number / item.Key < 100)
                {
                    if (number / item.Key < 10)
                    {
                        return ((number / item.Key)).ToString(format1) + item.Value;
                    }
                    return ((number / item.Key)).ToString(format0) + item.Value;
                }
                return ((number / item.Key)).ToString(format0) + item.Value;
            }
        }
        if(withFraction)
        {
            if (number < 100)
            {
                if (number < 10)
                {
                    return ((number)).ToString(format2);
                }
                return ((number)).ToString(format1);
            }
        }
        return Mathf.Round((float)number).ToString(format0);
    }
    public static string NumberToTextInOneLine(this int number, bool withFraction = false)
    {
        foreach (var item in numAbbrevs.Reverse())
        {
            if (number >= item.Key)
            {
                if (number / item.Key < 100)
                {
                    if (number / item.Key < 10)
                    {
                        return ((number / item.Key)).ToString(format1) + item.Value;
                    }
                    return ((number / item.Key)).ToString(format0) + item.Value;
                }
                return ((number / item.Key)).ToString(format0) + item.Value;
            }
        }
        if(withFraction)
        {
            if (number < 100)
            {
                if (number < 10)
                {
                    return ((number)).ToString(format2);
                }
                return ((number)).ToString(format1);
            }
        }
        return Mathf.Round(number).ToString(format0);
    }
}
