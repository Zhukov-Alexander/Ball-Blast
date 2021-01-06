using static System.Environment;
using UnityEngine;

public static class NumberHandler
{
    private static readonly float B = Mathf.Pow(10, 9);
    private static readonly float M = Mathf.Pow(10, 6);
    private static readonly float K = Mathf.Pow(10, 3);

    public static string NumberToTextInOneLine(this float number)
    {
        if (number >= B)
        {
            if (number / B < 100)
            {
                if (number / B < 10)
                {
                    return ((number / B)).ToString("f2") + "B";
                }
                return ((number / B)).ToString("f1") + "B";
            }

            return ((number / B)).ToString("f0") + "B";
        }
        else if (number >= M)
        {
            if (number / M < 100)
            {
                if (number / M < 10)
                {
                    return ((number / M)).ToString("f2") + "M";
                }
                return ((number / M)).ToString("f1") + "M";
            }

            return ((number / M)).ToString("f0") + "M";
        }
        else if (number >= K)
        {
            if (number / K < 100)
            {
                if (number / K < 10)
                {
                    return ((number / K)).ToString("f2") + "K";
                }
                return ((number / K)).ToString("f1") + "K";
            }
            return ((number / K)).ToString("f0") + "K";
        }
        else
        {
            return number.ToString("f0");
        }
    }
}
