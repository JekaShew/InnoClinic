# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5005

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Development
WORKDIR /src
COPY ["CommonLibrary/CommonLibrary/CommonLibrary.csproj", "CommonLibrary/CommonLibrary/"]
COPY ["AppointmentAPI/AppointmentAPI.Domain/AppointmentAPI.Domain.csproj", "AppointmentAPI/AppointmentAPI.Domain/"]
COPY ["AppointmentAPI/AppointmentAPI.Shared/AppointmentAPI.Shared.csproj", "AppointmentAPI/AppointmentAPI.Shared/"]
COPY ["AppointmentAPI/AppointmentAPI.Persistance/AppointmentAPI.Persistance.csproj", "AppointmentAPI/AppointmentAPI.Persistance/"]
COPY ["AppointmentAPI/AppointmentAPI.Presentation/AppointmentAPI.Presentation.csproj", "AppointmentAPI/AppointmentAPI.Presentation/"]
COPY ["AppointmentAPI/AppointmentAPI.Application/AppointmentAPI.Application.csproj", "AppointmentAPI/AppointmentAPI.Application/"]
COPY ["AppointmentAPI/AppointmentAPI.Web/AppointmentAPI.Web.csproj", "AppointmentAPI/AppointmentAPI.Web/"]
RUN dotnet restore "./AppointmentAPI/AppointmentAPI.Web/AppointmentAPI.Web.csproj"

COPY . .

WORKDIR "/src/AppointmentAPI/AppointmentAPI.Web"
RUN dotnet build "./AppointmentAPI.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Development
RUN dotnet publish "./AppointmentAPI.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AppointmentAPI.Web.dll"]
