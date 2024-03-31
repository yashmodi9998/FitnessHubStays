# FitnessHubStays

## C# ASP.NET WebAPI Project Notes:

- Clone the project repository.
- Add App_Data table into the project folder.
- Open Visual Studio.
  - Navigate to Tools > Nuget Package Manager > Package Manager Console.
  - Execute the following commands in the Package Manager Console:
    - `Enable-Migrations`
    - `Add-Migration <migration_name>`
    - `Update-Database`

### Tasks to Do:

- Room
  - List | Add | View | Update | Delete ✅
- Activity
  - List | Add | View | Update | Delete ✅
  - Changes Database Schema
- User Authentication ✅
  - Custom fields for user table (First Name || Last Name ) ✅
- Booking
  - List | Add | View | Update | Delete ✅
  - Note: In Insert and Update, We need login first if we don't login then we will fetch database error because of "USERID"
- Booking Activity
