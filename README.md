# Endurance
A Library Management System using ASP.NET Core and Blazor.

This is my first application that I am hell bent to complete it to the last droplet rather than leave it halfway after major work is done.
The project uses complete .NET Stack from Blazor to ASP.NET Core 6.0 to Microsoft SQL Server. It also uses other open source components such as MudBlazor, LazyCache, AutoMapper, HangFire, MediatR


## Steps to run this project

- Normal build _(This uses a normal SQL Server instance)_
  - Open `/Quark.Server/Properties/launchSettings.json` file.
  - Go the `Quark.Server` profile under `profiles` key.
  - Under `environmentVariables`, modify the value of `DbServer` from `localhost` to your own SQL Server address. If you are using LocalDB, change it to `(localdb)/SQLLocalDB`. Also it is recommended to update the `DbUser` and `DbPassword` variables.
  - If you are using Visual Studio, simply start the application in `Quark.Server` launch profile. If you are using Visual Studio code, use `dotnet run` in the terminal.
  - Make sure the SQL Server is properly accessible.

- Using docker _(This uses SQL Server inside docker container)_
  - Install Docker Desktop(For windows and macOS users) or Docker CE(For Linux users). _Note that using docker in windows requires a proper installation of WSL 2._
  - Download this repository zip file and extract the solution files into folder of your wish.
  - Open a terminal at the that folder . Type `docker-compose up` and press enter. This command creates 2 container images, each for the ASP.NET Core app and SQL Server, after which both the containers start running.
  - Open your browser and browse to `https://localhost:8080` to run the web app.
  - Alternatively, the application can be run through Visual Studio, by choosing the _Quark.Server_(Kestrel) as launch profile, which runs at `https://localhost:5001` _without   docker_. But this step requires the SQL server container to be running, which can be started from Docker desktop if not running previously.
  - Similarly, if using Visual Studio Code, simply use `dotnet run` in the terminal and browse `https://localhost:5001`, while ensuring the SQL Server container is running
