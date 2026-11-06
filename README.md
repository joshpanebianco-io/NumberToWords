# Number to Words

Converts a dollar amount to words. `123.45` becomes `ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS`.

C# / ASP.NET Core API with a React page.

## Run the pre-built app

Needs the ASP.NET Core runtime, version 9 or later.

```
cd publish
NumberToWords.Api.exe
```

Open http://localhost:5000. On Mac or Linux, run `dotnet NumberToWords.Api.dll` instead.

Start it from inside `publish`, otherwise the page returns a 404.

## Build from source

Needs the .NET 9 SDK and Node 22.

```
cd client
npm install
npm run build
cd ..
dotnet run --project src/NumberToWords.Api
```

Open http://localhost:5000.

## Run the tests

```
dotnet test
```

See [docs/TestPlan.md](docs/TestPlan.md) for the full plan, including the manual UI checks.

## Host it

```
cd client
npm install
npm run build
cd ..
dotnet publish src/NumberToWords.Api -c Release -o publish
```

Copy `publish` to the server and start the app from inside that folder. To use a different port, set `ASPNETCORE_URLS`, for example `ASPNETCORE_URLS=http://0.0.0.0:8080`. The published folder includes a `web.config` for IIS.

## Use it

Type an amount into the page and press Convert or Enter.

To call the API directly:

```
GET /api/convert?number=123.45

200 {"words":"ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS"}
400 {"error":"Enter an amount like 123.45, ..."}
```

## Working on the page

Run the API with the `dotnet run` command above, then run `npm run dev` in `client`. Vite serves the page with hot reload and forwards `/api` calls to port 5000.

## Layout

```
src/NumberToWords.Api/       API. The conversion is in NumberConverter.cs
tests/NumberToWords.Tests/   Unit and API tests
client/                      React page
publish/                     Pre-built app
docs/                        Approach and test plan
```
