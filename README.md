# Monthly Energy Consumption Aggregator aka MECA

The aim of this console app is to consume a `consumption-data.csv` file located at `src/MECA.ConsoleApp/data/input/consumption-data.csv`. The file contains daily smart meter readings collected at midnight for a given day. The app will read the data and aggregate it so we can see the total consumption for a given month. The aggredated data will written to `consumption-output.csv` located at ``src/MECA.ConsoleApp/data/output/consumption-output.csv`.

## Tasks

- [x] Read File from `data\input` folder.
  - [x] Check that file exists else terminate
  - [x] Read file data
- [x] Load data from file into data structure.
  - [x] As we read data map to `{ date, consumption }`
    - [x] Date is read as ISO-1806 format without time information. Safe to assume time is midnight as per brief in `challenge\README.md`
    - [x] Consumption is an `int`
- [x] Aggregate data by Year/Month so we know actual for a given month.
  - [x] Group data by `MMM yyyy`
  - [x] Get most recent reading of the Month Year.
  - [x] Foreach Month Year add to new dictionary using year month as key. Value can be calculated by substracting the previous month reading from the current month reading.
- [x] Print data to file format `Month Year, Consumption`
  - [x] Create new file in `output` folder.
  - [x] Format each record into a csv string that will represent a row
  - [x] write all rows to file

### Strectch

- Output stats to console

## To Run

- Install [.Net 5 SDK]();

- Run `dotnet build`
- Run `dotnet run --project ./src/MECA.ConsoleApp/MECA.ConsoleApp.csproj`

## Known Issues

- Will not generate new directories if the expected path doesn't exist
- Will If we fail to complete aggregations mid way then we have no means of tracking what we have done will therefore have to restart
- No logging was done therefor debuggin production behavior will be difficult
- No means of tracking previously processed files. Output file will always be recreated.
