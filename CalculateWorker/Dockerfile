#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CalculateWorker/CalculateWorker.csproj", "CalculateWorker/"]
RUN dotnet restore "CalculateWorker/CalculateWorker.csproj"
COPY . .
WORKDIR "/src/CalculateWorker"
RUN dotnet build "CalculateWorker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CalculateWorker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CalculateWorker.dll"]