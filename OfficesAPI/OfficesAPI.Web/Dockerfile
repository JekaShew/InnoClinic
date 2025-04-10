# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Development
WORKDIR /src
COPY ["CommonLibrary/CommonLibrary/CommonLibrary.csproj", "CommonLibrary/CommonLibrary/"]
COPY ["OfficesAPI/OfficesAPI.Domain/OfficesAPI.Domain.csproj", "OfficesAPI/OfficesAPI.Domain/"]
COPY ["OfficesAPI/OfficesAPI.Shared/OfficesAPI.Shared.csproj", "OfficesAPI/OfficesAPI.Shared/"]
COPY ["OfficesAPI/OfficesAPI.Persistance/OfficesAPI.Persistance.csproj", "OfficesAPI/OfficesAPI.Persistance/"]
COPY ["OfficesAPI/OfficesAPI.Presentation/OfficesAPI.Presentation.csproj", "OfficesAPI/OfficesAPI.Presentation/"]
COPY ["OfficesAPI/OfficesAPI.Services/OfficesAPI.Services.csproj", "OfficesAPI/OfficesAPI.Services/"]
COPY ["OfficesAPI/OfficesAPI.Services.Abstractions/OfficesAPI.Services.Abstractions.csproj", "OfficesAPI/OfficesAPI.Services.Abstractions/"]
COPY ["OfficesAPI/OfficesAPI.Web/OfficesAPI.Web.csproj", "OfficesAPI/OfficesAPI.Web/"]
RUN dotnet restore "./OfficesAPI/OfficesAPI.Web/OfficesAPI.Web.csproj"

COPY . .

WORKDIR "/src/OfficesAPI/OfficesAPI.Web"
RUN dotnet build "./OfficesAPI.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Development
RUN dotnet publish "./OfficesAPI.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OfficesAPI.Web.dll"]
