# Approach

## Overview

The React page calls `GET /api/convert?number=123.45`. ASP.NET Core serves both the API and the built page, so there is only one thing to host.

The routine is `NumberConverter.TryConvert(string? input, out string words)`. It returns `false` for invalid input and passes the words back through the `out` parameter, as the brief asks. This is the same pattern as `int.TryParse`, so it should look familiar to C# developers.

## Algorithm

1. Check the input is digits with an optional 1 or 2 digit decimal part.
2. Split it into dollars and cents.
3. Take the dollars three digits at a time from the right, and add THOUSAND, MILLION and so on to each group.
4. Add the units and join the two halves with AND.

Numbers from 0 to 19 and the tens are looked up in arrays. Everything else is built from those.

## Decisions

**Parsing the text instead of using `double` or `decimal.TryParse`.** `double` is a bad fit for money: `Math.Floor(1.15 * 100)` gives 114. `decimal` is accurate, but by default `decimal.TryParse` accepts `1,2,3` as 123 and `5-` as -5. Checking the text with one regex and then parsing the digits is easier to follow and test.

**`long` for dollars.** Amounts are capped at 15 digits (999 trillion), which is well within the range of `long` and more than enough for money.

**Where AND goes.** The example in the brief uses Australian/British style, so the same rule applies throughout: `1,001` is ONE THOUSAND AND ONE, and `1,100` is ONE THOUSAND ONE HUNDRED.

**Rejecting input instead of guessing.**

| Input | Why it is rejected |
|---|---|
| `1.234` | Rounding would change the amount without telling the user |
| `1,000` | `1,50` means 1.50 in some countries |
| `-5` | The brief gives no wording for a negative amount |
| `$5` | The page already works in dollars, so a symbol adds nothing |

**Zero parts are left out.** `123.00` is ONE HUNDRED AND TWENTY-THREE DOLLARS and `0.45` is FORTY-FIVE CENTS. `0` is ZERO DOLLARS.

## Dependencies

The API uses only ASP.NET Core, with no NuGet packages. xUnit and `Mvc.Testing` are used only by the test project. React and Vite are used only for the page.

