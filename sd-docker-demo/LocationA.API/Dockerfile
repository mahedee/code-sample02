#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["LocationA.API/LocationA.API.csproj", "Location.API/"]
RUN dotnet restore "LocationA.API/LocationA.API.csproj"
COPY . .
WORKDIR "/src/LocationA.API"
RUN dotnet build "LocationA.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LocationA.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LocationA.API.dll"]