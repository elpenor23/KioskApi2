FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

COPY KioskApi2.csproj .
COPY KioskApi2.sln .
RUN dotnet restore KioskApi2.sln

COPY . .

WORKDIR "/src/."
RUN dotnet build "KioskApi2.csproj" -c Release -o /app/build
FROM build AS publish

RUN dotnet publish "KioskApi2.csproj" -c Release -o /app/publish

EXPOSE 8080

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KioskApi2.dll"]
