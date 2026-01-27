FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["src/Services/OrderService/OrderService/OrderService.csproj", "src/Services/OrderService/OrderService/"]
COPY ["src/Shared/EventBus/EventBus/EventBus.csproj", "src/Shared/EventBus/EventBus/"]

# Restore dependencies
RUN dotnet restore "src/Services/OrderService/OrderService/OrderService.csproj"

# Copy source code
COPY . .
WORKDIR "/src/src/Services/OrderService/OrderService"
RUN dotnet build "OrderService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderService.dll"]