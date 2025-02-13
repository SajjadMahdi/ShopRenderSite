# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR ./
COPY ["Shop.Web/Shop.Web.csproj", "Shop.Web/"]
COPY ["Shop.Application/Shop.Application.csproj", "Shop.Application/"]
COPY ["Shop.Domain/Shop.Domain.csproj", "Shop.Domain/"]
COPY ["Shop.Infra.Data/Shop.Infra.Data.csproj", "Shop.Infra.Data/"]
COPY ["Shop.Infra.IoC/Shop.Infra.IoC.csproj", "Shop.Infra.IoC/"]
RUN dotnet restore "./Shop.Web/Shop.Web.csproj"
COPY . .
WORKDIR "/src/Shop.Web"
RUN dotnet build "./Shop.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Shop.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Shop.Web.dll"]

## This stage is used when running from VS in fast mode (Default for Debug configuration)
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#USER app
#WORKDIR /app
#EXPOSE 8080
#EXPOSE 8081
#
## This stage is used to build the service project
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#
## Copy all project files
#COPY ["Shop.Web/Shop.Web.csproj", "Shop.Web/"]
#COPY ["Shop.Application/Shop.Application.csproj", "Shop.Application/"]
#COPY ["Shop.Domain/Shop.Domain.csproj", "Shop.Domain/"]
#COPY ["Shop.Infra.Data/Shop.Infra.Data.csproj", "Shop.Infra.Data/"]
#COPY ["Shop.Infra.IoC/Shop.Infra.IoC.csproj", "Shop.Infra.IoC/"]
#
## Restore dependencies
#RUN dotnet restore "Shop.Web/Shop.Web.csproj"
#
## Copy the rest of the application code
#COPY . .
#
## Build the project
#WORKDIR "/src/Shop.Web"
#RUN dotnet build "Shop.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
## This stage is used to publish the service project to be copied to the final stage
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "Shop.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
## This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "Shop.Web.dll"]