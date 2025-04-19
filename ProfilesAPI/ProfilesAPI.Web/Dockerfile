# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5003

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Development
WORKDIR /src
COPY ["CommonLibrary/CommonLibrary/CommonLibrary.csproj", "CommonLibrary/CommonLibrary/"]
COPY ["ProfilesAPI/ProfilesAPI.Domain/ProfilesAPI.Domain.csproj", "ProfilesAPI/ProfilesAPI.Domain/"]
COPY ["ProfilesAPI/ProfilesAPI.Shared/ProfilesAPI.Shared.csproj", "ProfilesAPI/ProfilesAPI.Shared/"]
COPY ["ProfilesAPI/ProfilesAPI.Persistance/ProfilesAPI.Persistance.csproj", "ProfilesAPI/ProfilesAPI.Persistance/"]
COPY ["ProfilesAPI/ProfilesAPI.Presentation/ProfilesAPI.Presentation.csproj", "ProfilesAPI/ProfilesAPI.Presentation/"]
COPY ["ProfilesAPI/ProfilesAPI.Services/ProfilesAPI.Services.csproj", "ProfilesAPI/ProfilesAPI.Services/"]
COPY ["ProfilesAPI/ProfilesAPI.Services.Abstractions/ProfilesAPI.Services.Abstractions.csproj", "ProfilesAPI/ProfilesAPI.Services.Abstractions/"]
COPY ["ProfilesAPI/ProfilesAPI.Web/ProfilesAPI.Web.csproj", "ProfilesAPI/ProfilesAPI.Web/"]
RUN dotnet restore "./ProfilesAPI/ProfilesAPI.Web/ProfilesAPI.Web.csproj"

COPY . .

WORKDIR "/src/ProfilesAPI/ProfilesAPI.Web"
RUN dotnet build "./ProfilesAPI.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Development
RUN dotnet publish "./ProfilesAPI.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProfilesAPI.Web.dll"]
