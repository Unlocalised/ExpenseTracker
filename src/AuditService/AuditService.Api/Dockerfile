# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN apt-get update && apt-get install -y curl
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AuditService/AuditService.Api/AuditService.Api.csproj", "AuditService/AuditService.Api/"]
COPY ["AuditService/AuditService.Infrastructure/AuditService.Infrastructure.csproj", "AuditService/AuditService.Infrastructure/"]
COPY ["ExpenseTracker/ExpenseTracker.Application/ExpenseTracker.Application.csproj", "ExpenseTracker/ExpenseTracker.Application/"]
COPY ["ExpenseTracker/ExpenseTracker.Domain/ExpenseTracker.Domain.csproj", "ExpenseTracker/ExpenseTracker.Domain/"]
COPY ["ExpenseTracker/ExpenseTracker.Api/ExpenseTracker.Api.csproj", "ExpenseTracker/ExpenseTracker.Api/"]
COPY ["AuditService/AuditService.Application/AuditService.Application.csproj", "AuditService/AuditService.Application/"]
RUN dotnet restore "./AuditService/AuditService.Api/AuditService.Api.csproj"
COPY . .
WORKDIR "/src/AuditService/AuditService.Api"
RUN dotnet build "./AuditService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AuditService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuditService.Api.dll"]