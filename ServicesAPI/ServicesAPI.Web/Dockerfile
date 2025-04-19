# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5004

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Development
WORKDIR /src
COPY ["CommonLibrary/CommonLibrary/CommonLibrary.csproj", "CommonLibrary/CommonLibrary/"]
COPY ["ServicesAPI/ServicesAPI.Domain/ServicesAPI.Domain.csproj", "ServicesAPI/ServicesAPI.Domain/"]
COPY ["ServicesAPI/ServicesAPI.Shared/ServicesAPI.Shared.csproj", "ServicesAPI/ServicesAPI.Shared/"]
COPY ["ServicesAPI/ServicesAPI.Persistance/ServicesAPI.Persistance.csproj", "ServicesAPI/ServicesAPI.Persistance/"]
COPY ["ServicesAPI/ServicesAPI.Presentation/ServicesAPI.Presentation.csproj", "ServicesAPI/ServicesAPI.Presentation/"]
COPY ["ServicesAPI/ServicesAPI.Application/ServicesAPI.Application.csproj", "ServicesAPI/ServicesAPI.Application/"]
COPY ["ServicesAPI/ServicesAPI.Web/ServicesAPI.Web.csproj", "ServicesAPI/ServicesAPI.Web/"]
RUN dotnet restore "./ServicesAPI/ServicesAPI.Web/ServicesAPI.Web.csproj"

COPY . .

WORKDIR "/src/ServicesAPI/ServicesAPI.Web"
RUN dotnet build "./ServicesAPI.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Development
RUN dotnet publish "./ServicesAPI.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ServicesAPI.Web.dll"]
