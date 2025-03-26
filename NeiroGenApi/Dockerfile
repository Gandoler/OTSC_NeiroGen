# Используем образ ASP.NET для выполнения
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8084

# Используем SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Устанавливаем git и клонируем репозиторий
RUN apt-get update && apt-get install -y git \
    && git clone https://<Token>@github.com/Gandoler/OTSC_NeiroGen.git myrepo

# Переходим в каталог с проектом
WORKDIR /src/myrepo/NeiroGenApi

# Восстанавливаем зависимости и собираем проект
RUN dotnet restore NeiroGenApi.csproj
RUN dotnet publish NeiroGenApi.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Финальный образ с минимальным размером
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .


# Запускаем приложение
ENTRYPOINT ["dotnet", "NeiroGenApi.dll"]