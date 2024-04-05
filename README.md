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
- Booking
  - List | Add | View | Update | Delete ✅
  - Note: In Insert and Update, We need login first if we don't log in then we will fetch database error because of "USERID"
- Booking Activity
  - List | Add | View | Update | Delete ✅
  - Booking amount will be changed based on the user's Add, Update, and View.
-User Authentication
  - Custom fields for user table (First Name || Last Name )
  - Created two roles:1.Admin 2.Guest
  - Every new user will be rolled as a Guest user.
  - Anonymous users can't book activity. It will redirect to the login page.
  - Only the admin can Add, Edit, or remove activity and rooms.
 
- User flow
  - User view all the available rooms.
  - User selects rooms and shows it details.
  - User needs to be registered to book a room.
  - Users can select check-in and check-out dates based on the total amount charged.
  - User can view booking details.
  - Users have the option to book/cancel activities taking place every day. Based on that, the booking total amount will be changed.
  - 
    
