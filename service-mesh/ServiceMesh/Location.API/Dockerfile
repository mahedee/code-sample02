#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_URLS=http://*:80;
ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["/Location.API.csproj", "Location.API/"]
RUN dotnet restore "Location.API/Location.API.csproj"

WORKDIR "/src/Location.API"
COPY . .
WORKDIR "/src/Location.API"
RUN dotnet build "Location.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Location.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Location.API.dll"]