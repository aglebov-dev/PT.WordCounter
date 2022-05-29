# App
`Attention: his application is a test task, so you should not use it in product development`
Application for counting words in data provided from various sources

## How to

The application supports 3 main data providers:
- file provider
- provider for console input
- provider to read data from the database

## Build

```console
dotnet build ./src -c Release
```

## Read from file

To process the file and write the report to the file, use the following command:

```console
cd ./src/PT.WordCounter/bin/Release/netcoreapp3.1
dotnet PT.WordCounter.dll file --source C:\\file.txt --target C:\\report.txt --utf8
```

> **source** - The file to be used for reading
>
> **target** - The file that will be used to record the report encoded in Window-1251
>
> **utf8** - Encoding of the file specified in the parameter source (If the parameter is not specified, the default encoding is Windows-1251)

*The report will be written to a file.*

## Console

To read data from the console, use the command:

```console
cd ./src/PT.WordCounter/bin/Release/netcoreapp3.1
dotnet PT.WordCounter.dll console --text "A, B and C were sitting on the pipe B fell A tried, who remained on the pipe"
```

> **text** - Text to be used for analysis

*The report will be displayed in the console.*

## Database:

To read data from the database, use the command:

```console
cd ./src/PT.WordCounter/bin/Release/netcoreapp3.1
dotnet PT.WordCounter.dll db --connection-string "User ID=developer;Password=empty;Host=localhost;Port=5432;Database=postgres;" --table "private.data-table" --column "description"
```

> **connection-string** - string for connecting to the database (only postgres database is supported)
>
> **table** - The table from which data is read
>
> **column** - Column in the table from which data is read

*The report will be displayed in the console.*
