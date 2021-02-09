# monthly-energy-consumption-aggregator

## Tasks

- Read File from `incoming` folder.
  - Check that file exists else terminate
  - Read file data
- Load data from file into data structure.
  - As we read data map to `{ date, consumption }`
    - Date is read as ISO-1806 format
    - Consumption is an `int`
- Aggregate data by Year/Month.
  - Aggregate data so we only know the `yyyy MMM` and `Consumption`
  - Foreach item map to new dictionary using year month as key and increment the value by consumption
- Print data to file format `Month Year, Consumption`
  - Create new file in `aggregates` folder
  - Iterate dictionary while appending item to newly create file

### Strectch

- Output stats to console
