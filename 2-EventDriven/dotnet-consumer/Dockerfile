# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /app

# Copy both project folders
COPY . .

# Restore dependencies
RUN dotnet restore Producer
RUN dotnet restore Consumer

# Build both apps
RUN dotnet build Producer 
RUN dotnet build Consumer 

# Run both concurrently
CMD ["/bin/bash", "-c", "dotnet run --project Producer & dotnet run --project Consumer"]
