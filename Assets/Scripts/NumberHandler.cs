using static System.Environment;
using UnityEngine;

public static class NumberHandler
{
    private static readonly float Q = Mathf.Pow(10, 15);
    private static readonly float T = Mathf.Pow(10, 12);
    private static readonly float B = Mathf.Pow(10, 9);
    private static readonly float M = Mathf.Pow(10, 6);
    private static readonly float K = Mathf.Pow(10, 3);

    public static string NumberToTextInOneLineWithoutFraction(this float number)
    {
        if (number >= Q)
        {
            if (number / Q < 100)
            {
                if (number / Q < 10)
                {
                    return ((number / Q)).ToString("f2") + "Q";
                }
                return ((number / Q)).ToString("f1") + "Q";
            }

            return ((number / Q)).ToString("f0") + "Q";
        }
        if (number >= T)
        {
            if (number / T < 100)
            {
                if (number / T < 10)
                {
                    return ((number / T)).ToString("f2") + "T";
                }
                return ((number / T)).ToString("f1") + "T";
            }

            return ((number / T)).ToString("f0") + "T";
        }
        else if(number >= B)
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
            return Mathf.Round(number).ToString("f0");
        }
    }
    public static string NumberToTextInOneLineWithoutFraction(this int number)
    {
        if (number >= Q)
        {
            if (number / Q < 100)
            {
                if (number / Q < 10)
                {
                    return ((number / Q)).ToString("f2") + "Q";
                }
                return ((number / Q)).ToString("f1") + "Q";
            }

            return ((number / Q)).ToString("f0") + "Q";
        }
        if (number >= T)
        {
            if (number / T < 100)
            {
                if (number / T < 10)
                {
                    return ((number / T)).ToString("f2") + "T";
                }
                return ((number / T)).ToString("f1") + "T";
            }

            return ((number / T)).ToString("f0") + "T";
        }
        else if (number >= B)
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
            return Mathf.Round(number).ToString("f0");
        }
    }
    public static string NumberToTextInOneLineWithFraction(this float number)
    {
        if (number >= Q)
        {
            if (number / Q < 100)
            {
                if (number / Q < 10)
                {
                    return ((number / Q)).ToString("f2") + "Q";
                }
                return ((number / Q)).ToString("f1") + "Q";
            }

            return ((number / Q)).ToString("f0") + "Q";
        }
        if (number >= T)
        {
            if (number / T < 100)
            {
                if (number / T < 10)
                {
                    return ((number / T)).ToString("f2") + "T";
                }
                return ((number / T)).ToString("f1") + "T";
            }

            return ((number / T)).ToString("f0") + "T";
        }
        else if (number >= B)
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
            if (number < 100)
            {
                if (number < 10)
                {
                    return ((number)).ToString("f2");
                }
                return ((number)).ToString("f1");
            }
            return ((number)).ToString("f0");
        }
    }
}
