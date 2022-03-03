#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_URLS=http://*:80;
ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["/BFF.Web.csproj", "BFF.Web/"]
RUN dotnet restore "BFF.Web/BFF.Web.csproj"

WORKDIR "/src/BFF.Web"
COPY . .
WORKDIR "/src/BFF.Web"
RUN dotnet build "BFF.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BFF.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BFF.Web.dll"]