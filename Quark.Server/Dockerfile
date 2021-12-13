#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
ENV ASPNETCORE_URLS=https://+:5005;http://+:5006
WORKDIR /app
EXPOSE 5005
EXPOSE 5006

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Quark.Server/Quark.Server.csproj", "Quark.Server/"]
COPY ["Quark.Client/Quark.Client.csproj", "Quark.Client/"]
COPY ["Quark.Core/Quark.Core.csproj", "Quark.Core/"]
COPY ["Quark.Shared/Quark.Shared.csproj", "Quark.Shared/"]
COPY ["Quark.Infrastructure/Quark.Infrastructure.csproj", "Quark.Infrastructure/"]
RUN dotnet restore "Quark.Server/Quark.Server.csproj"
COPY . .
WORKDIR "/src/Quark.Server"
RUN dotnet build "Quark.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Quark.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
WORKDIR /app/Files
WORKDIR /app
ENTRYPOINT ["dotnet", "Quark.Server.dll"]
