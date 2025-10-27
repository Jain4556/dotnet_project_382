ExpenseTracker 💰
Project Title

ExpenseTracker – A Simple ASP.NET Core MVC Expense Management System

Description

ExpenseTracker is a lightweight web application built using ASP.NET Core MVC, Entity Framework Core, and SQLite.
It allows users to efficiently manage their personal or small business expenses.

Key Features:

Add, edit, and delete expense entries.

Categorize expenses (e.g., Food, Travel, Rent, Utilities).

View a summary of total expenses by category.

Visualize expense trends through charts (monthly & category-wise).

Responsive UI with clean Bootstrap-based styling.

This project is ideal for beginners learning .NET Core MVC + EF Core CRUD operations and database handling using SQLite.

Installation Steps
🧩 Prerequisites

Make sure you have the following installed on your system:

.NET 8 SDK or later

Visual Studio 2022
 or VS Code

Git

Optional: SQLite Studio
 to view the local database

📥 Steps to Set Up the Project

Clone the repository

git clone https://github.com/YOUR_USERNAME/ExpenseTracker.git
cd ExpenseTracker


Restore dependencies

dotnet restore


Add the initial migration (if not already added)

dotnet ef migrations add InitialCreate


Apply the migration and create the database

dotnet ef database update


This will create a local expense.db SQLite database file.

Run the application

dotnet run


Open your browser and navigate to:

https://localhost:5001


or

http://localhost:5000

How to Run the Project 🚀

Once you’ve followed the setup steps:

Launch the project with dotnet run or by pressing F5 in Visual Studio.

The home page will open — navigate to the Expenses section.

You can:

Add a new expense by selecting a category and entering the details.

View, edit, or delete existing expenses.

Check the Summary page to see spending charts (category-wise and monthly).

All your data is stored locally in expense.db (SQLite).