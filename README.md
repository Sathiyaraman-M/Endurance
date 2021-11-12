# Endurance
A Library Management System using ASP.NET Core and Blazor.

This is my first application that I am hell bent to complete it to the last droplet rather than leave it halfway after major work is done.
The project uses complete .NET Stack from Blazor to ASP.NET Core 6.0 to Microsoft SQL Server. It also uses other open source components such as MudBlazor, LazyCache, AutoMapper, HangFire, MediatR


## Steps to build and test this project

- Download and install .NET 6.0 from [here](https://dotnet.microsoft.com/download/dotnet/6.0). 
- Make sure atleast you have Microsoft SQL Server LocalDB feature, in order to run this project.
- Next configure the database connection string in the `Quark.Server/app.development.json`.
- If you are using Visual Studio 2022, you can directly open the solution file `Endurance.sln` and build and run the solution.
  -  Open Package Manager Console and run the following command to generate a database in SQL Server
     ```
     Update-Database
     ```
- If you are using Visual Studio Code, install the C# VS Code extension (OmniSharp)
  - Open the project folder in your VSCode terminal.
  - Run the following command to generate a database in SQL Server
    ```
    dotnet ef database update
    ```
  - Run the following to build and run the project 
    ```
    dotnet run
    ``` 
