# 1. Etapa de compilación con .NET 8 SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia solo el archivo de proyecto y restaura paquetes
COPY *.csproj ./
RUN dotnet restore

# Copia todo el código y publica la aplicación
COPY . ./
RUN dotnet publish -c Release -o /app/out

# 2. Etapa de ejecución con .NET 8 Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Comando para arrancar la API
ENTRYPOINT ["dotnet", "Software2.dll"]
