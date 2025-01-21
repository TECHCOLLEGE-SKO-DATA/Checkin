## Startup

The program opens two windows at startup

1. [MainWindow](../CheckInSystem/Views/Windows/MainWindow.xaml) containing a content control for switching between the difference views used for managing employees.
2. [EmployeeOverview](../CheckInSystem/Views/Windows/EmployeeOverview.xaml) always opens on the second monitor and shows an overview of the employees who are supposed to be on site.

All the logic run at startup is run from the `run` method in [Startup.cs](../CheckInSystem/Startup.cs).  
Some of the things done at startup are:

- Ensuring a database connection.
- Getting employees and groups from the database.
- Checkout employee who did not checkout the last day.
- Ending offsite time for an employee, if it is past the last day they had offsite time.
- Opening the EmployeeOverView on the second screen.

If no administrator account exists, it creates a default account with the
username **_"sko"_** and password _**"test123"**_.

## Database

The database used for this program is "SQL Server Express" run locally on the
machine. The database is set up using [SetUpDB.bat](../Database_SQL/SetUpDB.bat)
batch script, when the script is run it expects the default address set by SQL
Express when installing for the first time.

The connection to the database from the program, is established using a static
connection string, set to the default address of SQL express when installing
for the first time. Dapper is used for mapping data between the program and database.

## MainWindow

The MainWindow have the following views it switches between:

- [LoginScreen.xaml](../CheckInSystem/Views/UserControls/LoginScreen.xaml)
  is the first screen you see when you open the program and where the admin
  login to control the rest of the program.
- [AdminPanel.xaml](../CheckInSystem/Views/UserControls/AdminPanel.xaml)
  is the top and bottom bar for employee view and has [AdminEmployeeView.xaml](../CheckInSystem/Views/UserControls/AdminEmployeeView.xaml)
  inside a ContentControl.
    - [AdminEmployeeView.xaml](../CheckInSystem/Views/UserControls/AdminEmployeeView.xaml)
      shows a list of employees and have buttons for modifying the employee.
      The view takes a list of employees as an input and show the employees in
      the inputted list.
- [AdminGroupView.xaml](../CheckInSystem/Views/UserControls/AdminGroupView.xaml)
  is used for adding new groups and editing the name and visibility of the group.
- [EmployeeTimeView.xaml](../CheckInSystem/Views/UserControls/EmployeeTimeView.xaml)
  **_(work in progress)_** show all the times for a selected employee and
  allows adding and modifying their times.

## Card Reader

When a card is scanned the following things happen in order.
Returns if any of the following points are triggered:

1. Check the string we got from the card reader is not an empty string,
   when this happen we tell the card reader to send out an error beep.
2. If selected in admin panel, open a window to edit the details of the scanned card.
3. If selected in [EditEmployeeWindow](../CheckInSystem/Views/Windows/EditEmployeeWindow.xaml),
   replace the the scanned Card ID with an existing employee.
4. Sends the card ID to the database and gets the updated data from the database.