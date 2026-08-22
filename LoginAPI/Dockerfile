FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["LoginAPI.csproj", "./"]
RUN dotnet restore "./LoginAPI.csproj"

COPY . .

RUN dotnet publish "LoginAPI.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LoginAPI.dll"]
