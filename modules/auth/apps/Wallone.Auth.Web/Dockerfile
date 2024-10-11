#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["modules/auth/apps/Wallone.Auth.Web/Wallone.Auth.Web.csproj", "modules/auth/apps/Wallone.Auth.Web/"]
COPY ["modules/auth/src/Wallone.Auth.Services/Wallone.Auth.Services.csproj", "modules/auth/src/Wallone.Auth.Services/"]
COPY ["modules/auth/src/Wallone.Auth.Services.Contracts/Wallone.Auth.Services.Contracts.csproj", "modules/auth/src/Wallone.Auth.Services.Contracts/"]
RUN dotnet restore "./modules/auth/apps/Wallone.Auth.Web/Wallone.Auth.Web.csproj"
COPY . .
WORKDIR "/src/modules/auth/apps/Wallone.Auth.Web"
RUN dotnet build "./Wallone.Auth.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Wallone.Auth.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wallone.Auth.Web.dll"]