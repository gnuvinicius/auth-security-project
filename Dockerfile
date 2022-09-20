FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src

COPY Security.csproj .

RUN dotnet restore "Security.csproj"

COPY . .

RUN dotnet publish "Security.csproj" -c Debug -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final

WORKDIR /app

COPY --from=build /publish .

ENTRYPOINT ["dotnet", "Security.dll"]