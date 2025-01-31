# #FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# ARG TAG=ltsc2022
# #FROM mcr.microsoft.com/dotnet/aspnet:8.0-nanoserver-$TAG AS base
# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# WORKDIR /app
# EXPOSE 8080
# EXPOSE 44333
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-$TAG AS build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["KioskApi2.csproj", "./"]
RUN dotnet restore "./KioskApi2.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "KioskApi2.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "KioskApi2.csproj" -c Release -o /app/publish
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KioskApi2.dll"]
