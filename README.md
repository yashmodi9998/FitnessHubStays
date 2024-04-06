# Fitness Hub Booking System

## Introduction

This project is a booking system for a fitness hub, allowing users to book rooms and activities offered by the hub. It provides a user-friendly interface for browsing available rooms, viewing activity schedules, and managing bookings.

## Features

- **Booking Management**: Allows users to book rooms and activities available in the fitness hub.
- **Room Management**: Provides functionalities for managing rooms and their availability.
- **Activity Management**: Enables administrators to add, edit, and delete various activities offered by the fitness hub.
- **Booking Activity Management**: Manages the activities booked by users, providing details of their bookings.
- **User Registration**: Users need to register to book a room, ensuring accountability and security.
- **Room Availability**: Users can view all available rooms and their details before making a booking.
- **Booking Details**: Users can view detailed information about their bookings, including check-in and check-out dates and total amount charged.
- **Daily Activities**: Users have the option to book or cancel activities taking place every day, with the booking total amount adjusted accordingly.

## Components

### 1. UserController

- Handles user registration and authentication.
- Manages user-specific actions and views.

### 2. RoomViewController

- Provides views for users to browse available rooms and their details.
- Handles room booking and cancellation requests.

### 3. BookingViewController

- Manages the user interface for handling bookings.
- Displays booking details and allows users to manage their bookings.

### 4. ActivityViewController

- Displays a list of daily activities available for booking.
- Allows users to book or cancel activities based on their preferences.

### 5. BookingActivityController

- Manages the user interface for handling booking activities.
- Communicates with the API to perform CRUD operations on booking activities.

### 6. BookingDataController

- Provides API endpoints for CRUD operations on bookings.
- Interacts with the Entity Framework to perform operations on the database.

### 7. RoomDataController

- Provides API endpoints for CRUD operations on rooms.
- Interacts with the Entity Framework to perform operations on the database.

### 8. ActivityDataController

- Provides API endpoints for CRUD operations on activities.
- Interacts with the Entity Framework to perform operations on the database.

### 9. BookingActivityDataController

- Provides API endpoints for CRUD operations on booking activities.
- Interacts with the Entity Framework to perform operations on the database.

### 10. Models

- Contains model classes for users, bookings, rooms, activities, and booking activities.
- Includes DTOs for transferring data between controllers and APIs.

### 11. Views

- Contains HTML views for rendering the user interface of each component.
- Views include pages for user registration, browsing rooms, managing bookings, and booking/canceling activities.

## Setup Instructions

To run this project locally, follow these steps:

1. Clone the repository to your local machine.
2. Open the project in Visual Studio.
3. Set up the database connection string in `Web. config`.
4. Run the Entity Framework migrations to create or update the database schema.
5. Build the solution to restore dependencies.
6. Run the project using IIS Express or your preferred development server.

## Dependencies

- **ASP.NET MVC**: Framework for building web applications.
- **Entity Framework**: ORM tool for interacting with the database.
- **HttpClient**: Library for making HTTP requests to the API.
- **JavaScriptSerializer**: Utility for serializing and deserializing JSON data.

## C# ASP.NET WebAPI Project Notes:

- Clone the project repository.
- Add the App_Data table into the project folder.
- Open Visual Studio.
  - Navigate to Tools > Nuget Package Manager > Package Manager Console.
  - Execute the following commands in the Package Manager Console:
    - `Enable-Migrations`
    - `Add-Migration <migration_name>`
    - `Update-Database`
