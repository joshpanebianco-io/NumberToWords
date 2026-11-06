# Test Plan

## Automated tests

Run `dotnet test` from the repo root.

**`NumberConverterTests`** tests the conversion directly. The cases are listed as `[InlineData]` in the test file.

| Area | Examples |
|---|---|
| Example from the brief | `123.45` |
| Zero | `0`, `0.00` |
| Singular and plural | `1`, `2`, `0.01`, `0.02`, `1.01` |
| Teens and tens | `11`, `19`, `20`, `21`, `99` |
| Hundreds | `100`, `101`, `110`, `999` |
| AND placement | `1001`, `1100`, `1101`, `1000001`, `1001001` |
| Large scales | `1000000`, `1000000000`, `1000000000000` |
| Largest amount | `999999999999999.99` |
| Cents formats | `1.5`, `1.05`, `1.00`, `.5` |
| Leading zeros and spaces | `007`, `0000000000000000001`, `"  12  "` |
| Invalid: empty | null, empty, spaces only |
| Invalid: format | `abc`, `12a`, `-1`, `+1`, `$5`, `1e5`, `1,000`, `1 000` |
| Invalid: decimals | `.`, `1.`, `1.234`, `1.2.3` |
| Invalid: other | Arabic-Indic digits, 16 digit amount |

**`ApiTests`** starts the real app in memory and calls the endpoint.

| Request | Expected |
|---|---|
| `?number=123.45` | 200 with the words |
| `?number=abc` | 400 with an error message |
| `?number=` | 400 with an error message |
| No parameter | 400 with an error message |

## Manual UI tests

Start the app as described in the README, then open http://localhost:5000.

| # | Steps | Expected |
|---|---|---|
| 1 | Open the page | Cursor is in the Amount box |
| 2 | Enter `123.45` and press Enter | ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS |
| 3 | Change the amount | The previous result disappears |
| 4 | Enter `abc` and click Convert | Red error message explaining the format |
| 5 | Clear the box and press Enter | Same error message |
| 6 | Narrow the window to phone width and enter `999999999999999.99` | The whole result wraps inside the card, with no sideways scrolling |
| 7 | Stop the server and click Convert | "Could not reach the server. Please try again." |

Tested in Microsoft Edge.

## Not covered

No load testing, since each request is a small calculation with no database or file access. The page has no automated tests because it only sends the request and shows the response, and the manual checks cover that.
