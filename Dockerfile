FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG GITHUB_TOKEN
ARG GITHUB_ACTOR
WORKDIR /src

# Add GitHub Packages source with authentication
RUN if [ ! -z "$GITHUB_TOKEN" ] && [ ! -z "$GITHUB_ACTOR" ]; then \
    dotnet nuget add source --username "$GITHUB_ACTOR" --password "$GITHUB_TOKEN" --store-password-in-clear-text --name github "https://nuget.pkg.github.com/$GITHUB_ACTOR/index.json"; \
    fi

# Copy project files
COPY ["src/E-Commerce.CustomerManagement.Api/E-Commerce.CustomerManagement.Api.csproj", "src/E-Commerce.CustomerManagement.Api/"]
COPY ["src/E-Commerce.CustomerManagement.Application/E-Commerce.CustomerManagement.Application.csproj", "src/E-Commerce.CustomerManagement.Application/"]
COPY ["src/E-Commerce.CustomerManagement.Domain/E-Commerce.CustomerManagement.Domain.csproj", "src/E-Commerce.CustomerManagement.Domain/"]
COPY ["src/E-Commerce.CustomerManagement.Infrastructure/E-Commerce.CustomerManagement.Infrastructure.csproj", "src/E-Commerce.CustomerManagement.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "src/E-Commerce.CustomerManagement.Api/E-Commerce.CustomerManagement.Api.csproj"

# Copy source code
COPY . .

# Build and publish
WORKDIR "/src/src/E-Commerce.CustomerManagement.Api"
RUN dotnet build "E-Commerce.CustomerManagement.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "E-Commerce.CustomerManagement.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "E-Commerce.CustomerManagement.Api.dll"]
