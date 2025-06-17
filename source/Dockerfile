# 1. Use .NET 8 SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set working directory for the application
WORKDIR /source

# Copy solution and projects
COPY . .

# Restore .NET dependencies
RUN dotnet restore "MtdKey.OrderMaker.sln"

# Node should be present (with node and npm)
# Use NodeSource repositories
RUN apt-get update -y \
    && apt-get install -y curl \
    && curl -fsSL https://deb.nodesource.com/setup_18.x | bash - \
    && apt-get install -y nodejs \
    && rm -rf /var/lib/apt/lists/*

# Use Node to build TypeScript if needed
WORKDIR /source/MtdKey.OrderMaker.Server

# Install NPM dependencies
RUN npm install

# Compile TypeScript
RUN npm run release

# Publish the application
RUN dotnet publish "MtdKey.OrderMaker.csproj" -c Release -o /app


# The application should include the proper configuration files.
# The project should have appsettings.Docker.json with configured secrets.
# Otherwise, remove this command.
# IMPORTANT: The mv command should be performed INSIDE the application directory (/app)
WORKDIR /app
RUN mv appsettings.Docker.json appsettings.json


# 2. Use ASP.NET Core Runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app

# Copy build output from intermediate layer
COPY --from=build /app .

ENTRYPOINT ["dotnet", "MtdKey.OrderMaker.dll"]
