# TECHCOLLEGE
This repository is managed and maintained by students and teachers from TECHCOLLEGE in Denmark. External contributions are as of now not necessary.

# CheckInSystem
A simple check in system using an [ACR122U][card-reader-docs] card reader and a
local Microsoft SQL Server database.

## Running

1. Download SQL Express from Microsoft. Click [here](https://go.microsoft.com/fwlink/p/?linkid=2216019).
    1. Choose the basic installation.
2. Clone the repository.
3. Run the [Database Setup Script](Database_SQL/SetUpDB.bat).
4. Build the program or download it from [releases](https://github.com/TECHCOLLEGE-SKO-DATA/Checkin/releases).

## Documentation

- [Introduction](docs/Intro.md)
- [ACR122U Documentation][card-reader-docs]

## To-Do Tracking 

Features and fixes not done yet are tracked in [Todo.md](docs/Todo.md)

[card-reader-docs]: https://www.acs.com.hk/download-manual/419/API-ACR122U-2.04.pdf

## ⚠ Setting Up Your `app.config`
1. Copy `app.config-example` to `app.config`.
2. Open `app.config` and update:
   - **SQL Server Name** (`SqlServiceName`)
   - **Database Connection String** (`connectionString`)
3. In **Visual Studio**, right-click `app.config` → Properties → Set **"Copy to Output Directory"** to **"Copy always"**.
4. If using `dotnet build`, ensure `app.config` exists in `bin\Debug` and `bin\Release`.

🚀 If you get a **missing database error**, check if `app.config` is copied correctly.
