FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY Quark.Server/*.csproj ./Quark.Server/
COPY Quark.Client/*.csproj ./Quark.Client/
COPY Quark.Core/*.csproj ./Quark.Core/
COPY Quark.Shared/*.csproj ./Quark.Shared/
COPY Quark.Infrastructure/*.csproj ./Quark.Infrastructure/

RUN dotnet restore

# copy everything else and build app
COPY Quark.Server/. ./Quark.Server/
COPY Quark.Client/. ./Quark.Client/
COPY Quark.Core/. ./Quark.Core/
COPY Quark.Shared/. ./Quark.Shared/
COPY Quark.Infrastructure/. ./Quark.Infrastructure/

WORKDIR /app/Quark.Server
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
RUN mkdir -p Files/Documents
RUN mkdir -p Files/Images/Assets
RUN mkdir -p Files/Images/ProfilePictures

COPY --from=build /app/Quark.Server/out ./
ENTRYPOINT ["dotnet", "Quark.Server.dll"]