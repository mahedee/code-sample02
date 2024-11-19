# Define the base image
# Use the official ASP.NET Core runtime image the alias base sets here to use it in the next stage

# Switches the execution to a user named app. This is a security best practice to avoid running as root.
# The app user must exist in the aspnet:8.0 image. If not, this command will fail unless the user is created beforehand.

# Sets the working directory to /app. All subsequent commands in this stage will be relative to this directory.

# Informs Docker that the container will listen on ports 8080 and 8081. 
# This is for documentation purposes and does not expose the ports to the host by itself; you need to publish these ports explicitly with docker run -p.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Define the Build Stage
# Use the official ASP.NET Core SDK image to build the application
# Set the working directory to /src. This directory will be existing only in the build stage.
# Copies all files from the current directory on the host machine to the /src directory in the container.
# Restores project dependencies from the .csproj file. This ensures all necessary NuGet packages are downloaded.
# Builds the application in Release configuration and outputs the compiled files to the /app directory inside the container.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app

# Final State
# Use the base image defined in the first stage.
# Set the working directory to /app.
# Copies the compiled application from the build stage to the /app directory in the final image.
# Defines the command to run when the container starts. Here, it launches the application by executing the .NET runtime to run the HRM.API.dll file.
FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "HRM.API.dll"]

# You can see only the app directory in the final image because Multi-stage builds isolate the content of each stage from the other stages.
# Docker multi-stage build process implicitly discards everything in the intermediate stages when the final stage is built.