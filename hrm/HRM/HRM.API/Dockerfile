#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

EXPOSE 8011
ENV ASPNETCORE_URLS http://*:8011

ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["/HRM.API.csproj", "HRM.API/"]
RUN dotnet restore "HRM.API/HRM.API.csproj"

WORKDIR "/src/HRM.API"
COPY . .
WORKDIR "/src/HRM.API"
RUN dotnet build "HRM.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HRM.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HRM.API.dll"]