#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/GaleonServer/GaleonServer.Api/GaleonServer.Api.csproj", "src/GaleonServer/GaleonServer.Api/"]
COPY ["src/GaleonServer/GaleonServer.Infrastructure/GaleonServer.Infrastructure.csproj", "src/GaleonServer/GaleonServer.Infrastructure/"]
COPY ["src/GaleonServer/GaleonServer.Core/GaleonServer.Core.csproj", "src/GaleonServer/GaleonServer.Core/"]
COPY ["src/GaleonServer/GaleonServer.Interfaces/GaleonServer.Interfaces.csproj", "src/GaleonServer/GaleonServer.Interfaces/"]
COPY ["src/GaleonServer/GaleonServer.Models/GaleonServer.Models.csproj", "src/GaleonServer/GaleonServer.Models/"]
RUN dotnet restore "src/GaleonServer/GaleonServer.Api/GaleonServer.Api.csproj"
COPY . .
WORKDIR "/src/src/GaleonServer/GaleonServer.Api"
RUN dotnet build "GaleonServer.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GaleonServer.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GaleonServer.Api.dll"]