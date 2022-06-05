#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY GaleonServer.Api/GaleonServer.Api.csproj GaleonServer.Api.csproj
RUN dotnet restore GaleonServer.Api.csproj

COPY GaleonServer.Core/GaleonServer.Core.csproj GaleonServer.Core.csproj
RUN dotnet restore GaleonServer.Core.csproj

COPY GaleonServer.Infrastructure/GaleonServer.Infrastructure.csproj GaleonServer.Infrastructure.csproj
RUN dotnet restore GaleonServer.Infrastructure.csproj

COPY GaleonServer.Interfaces/GaleonServer.Interfaces.csproj GaleonServer.Interfaces.csproj
RUN dotnet restore GaleonServer.Interfaces.csproj

COPY GaleonServer.Models/GaleonServer.Models.csproj GaleonServer.Models.csproj
RUN dotnet restore GaleonServer.Models.csproj

COPY . .
WORKDIR /src
RUN dotnet build GaleonServer.Models.csproj -c Release -o /app

COPY . .
WORKDIR /src
RUN dotnet build GaleonServer.Interfaces.csproj -c Release -o /app

COPY . .
WORKDIR /src
RUN dotnet build GaleonServer.Infrastructure.csproj -c Release -o /app

COPY . .
WORKDIR /src
RUN dotnet build GaleonServer.Core.csproj -c Release -o /app

COPY . .
WORKDIR /src
RUN dotnet build GaleonServer.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish GaleonServer.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["dotnet", "GaleonServer.Api.dll"]