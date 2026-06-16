FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["El-Nabaash.API/El-Nabaash.API.csproj", "El-Nabaash.API/"]
RUN dotnet restore "El-Nabaash.API/El-Nabaash.API.csproj"
COPY . .
WORKDIR "/src/El-Nabaash.API"
RUN dotnet build "./El-Nabaash.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./El-Nabaash.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "El-Nabaash.API.dll"]
