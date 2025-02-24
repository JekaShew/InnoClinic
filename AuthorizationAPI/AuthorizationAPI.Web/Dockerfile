#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# USER app
# WORKDIR /app
# EXPOSE 8080
# EXPOSE 8081

# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# ARG BUILD_CONFIGURATION=Release
# WORKDIR /src
# COPY ["AuthorizationAPI.Web/AuthorizationAPI.Web.csproj", "AuthorizationAPI.Web/"]
# RUN dotnet restore "./AuthorizationAPI.Web/AuthorizationAPI.Web.csproj"

# COPY . .

# WORKDIR "/src/AuthorizationAPI.Web"
# RUN dotnet build "./AuthorizationAPI.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# FROM build AS publish
# ARG BUILD_CONFIGURATION=Release
# RUN dotnet publish "./AuthorizationAPI.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "AuthorizationAPI.Web.dll"]

#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Development
WORKDIR /src
COPY ["CommonLibrary/CommonLibrary/CommonLibrary.csproj", "CommonLibrary/CommonLibrary/"]
COPY ["AuthorizationAPI/AuthorizationAPI.Domain/AuthorizationAPI.Domain.csproj", "AuthorizationAPI/AuthorizationAPI.Domain/"]
COPY ["AuthorizationAPI/AuthorizationAPI.Shared/AuthorizationAPI.Shared.csproj", "AuthorizationAPI/AuthorizationAPI.Shared/"]
COPY ["AuthorizationAPI/AuthorizationAPI.Persistance/AuthorizationAPI.Persistance.csproj", "AuthorizationAPI/AuthorizationAPI.Persistance/"]
COPY ["AuthorizationAPI/AuthorizationAPI.Presentation/AuthorizationAPI.Presentation.csproj", "AuthorizationAPI/AuthorizationAPI.Presentation/"]
COPY ["AuthorizationAPI/AuthorizationAPI.Services/AuthorizationAPI.Services.csproj", "AuthorizationAPI/AuthorizationAPI.Services/"]
COPY ["AuthorizationAPI/AuthorizationAPI.Services.Abstractions/AuthorizationAPI.Services.Abstractions.csproj", "AuthorizationAPI/AuthorizationAPI.Services.Abstractions/"]
COPY ["AuthorizationAPI/AuthorizationAPI.Web/AuthorizationAPI.Web.csproj", "AuthorizationAPI/AuthorizationAPI.Web/"]
RUN dotnet restore "./AuthorizationAPI/AuthorizationAPI.Web/AuthorizationAPI.Web.csproj"

COPY . .

WORKDIR "/src/AuthorizationAPI/AuthorizationAPI.Web"
RUN dotnet build "./AuthorizationAPI.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Development
RUN dotnet publish "./AuthorizationAPI.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthorizationAPI.Web.dll"]
