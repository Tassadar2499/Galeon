FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish GaleonServer.Api/GaleonServer.Api.csproj -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR app
COPY --from=build /app/build .

EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "GaleonServer.Api.dll"]