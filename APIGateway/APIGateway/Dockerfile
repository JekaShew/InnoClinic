# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Development
WORKDIR /src
COPY ["APIGateway/APIGateway/APIGateway.csproj", "APIGateway/APIGateway/"]
RUN dotnet restore "./APIGateway/APIGateway/APIGateway.csproj"

COPY . .

WORKDIR "/src/APIGateway/APIGateway"
RUN dotnet build "./APIGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Development
RUN dotnet publish "./APIGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "APIGateway.dll"]