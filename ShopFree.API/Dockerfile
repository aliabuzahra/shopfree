# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["ShopFree.API/ShopFree.API.csproj", "ShopFree.API/"]
COPY ["ShopFree.Application/ShopFree.Application.csproj", "ShopFree.Application/"]
COPY ["ShopFree.Domain/ShopFree.Domain.csproj", "ShopFree.Domain/"]
COPY ["ShopFree.Infrastructure/ShopFree.Infrastructure.csproj", "ShopFree.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "ShopFree.API/ShopFree.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/ShopFree.API"
RUN dotnet build "ShopFree.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "ShopFree.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy published app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ShopFree.API.dll"]

