using System.Text.RegularExpressions;

namespace NumberToWords.Api;

public static class NumberConverter
{
    private const int MaxDollarDigits = 15;

    private static readonly Regex ValidAmount = new(@"^[0-9]*(\.[0-9]{1,2})?$");

    private static readonly string[] Ones =
    {
        "", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN",
        "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN"
    };

    private static readonly string[] Tens =
    {
        "", "", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY"
    };

    private static readonly string[] Scales = { "", "THOUSAND", "MILLION", "BILLION", "TRILLION" };

    public static bool TryConvert(string? input, out string words)
    {
        words = "";

        string amount = input?.Trim() ?? "";
        if (amount == "" || !ValidAmount.IsMatch(amount))
        {
            return false;
        }

        string[] parts = amount.Split('.');
        string dollarDigits = parts[0].TrimStart('0');
        if (dollarDigits.Length > MaxDollarDigits)
        {
            return false;
        }

        long dollars = dollarDigits == "" ? 0 : long.Parse(dollarDigits);

        // PadRight so that "1.5" is 50 cents, not 5
        int cents = parts.Length == 2 ? int.Parse(parts[1].PadRight(2, '0')) : 0;

        if (cents == 0)
        {
            words = WithUnit(dollars, "DOLLAR");
        }
        else if (dollars == 0)
        {
            words = WithUnit(cents, "CENT");
        }
        else
        {
            words = $"{WithUnit(dollars, "DOLLAR")} AND {WithUnit(cents, "CENT")}";
        }

        return true;
    }

    private static string WithUnit(long number, string unit)
    {
        string plural = number == 1 ? "" : "S";
        return $"{ToWords(number)} {unit}{plural}";
    }

    private static string ToWords(long number)
    {
        if (number == 0)
        {
            return "ZERO";
        }

        var parts = new List<string>();
        long remaining = number;

        // Work through the number three digits at a time, from the right
        for (int scale = 0; remaining > 0; scale++)
        {
            int group = (int)(remaining % 1000);
            remaining /= 1000;

            if (group == 0)
            {
                continue;
            }

            string text = GroupToWords(group);

            if (scale > 0)
            {
                text += " " + Scales[scale];
            }

            // 1,001 is ONE THOUSAND AND ONE, but 1,101 has its AND inside the group
            if (scale == 0 && group < 100 && remaining > 0)
            {
                text = "AND " + text;
            }

            parts.Insert(0, text);
        }

        return string.Join(" ", parts);
    }

    // Handles 1 to 999
    private static string GroupToWords(int group)
    {
        string text = "";

        if (group >= 100)
        {
            text = Ones[group / 100] + " HUNDRED";
            group %= 100;

            if (group > 0)
            {
                text += " AND ";
            }
        }

        if (group >= 20)
        {
            text += Tens[group / 10];

            if (group % 10 > 0)
            {
                text += "-" + Ones[group % 10];
            }
        }
        else
        {
            text += Ones[group];
        }

        return text;
    }
}
