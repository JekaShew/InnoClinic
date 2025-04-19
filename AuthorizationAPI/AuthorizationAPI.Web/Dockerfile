
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5001

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
