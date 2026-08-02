FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source

COPY ["./src/main/Core/Core.csproj", "./Core/"]
COPY ["./src/main/Infra.HttpClient/Infra.HttpClient.csproj", ".Infra.HttpClient/"]
COPY ["./src/main/Infra.MongoDB/Infra.MongoDB.csproj", "./Infra.MongoDB/"]
COPY ["./src/main/EntryPoint.WebApi/EntryPoint.WebApi.csproj", "./EntryPoint.WebApi/"]

RUN dotnet restore "./EntryPoint.WebApi/EntryPoint.WebApi.csproj" --force --no-cache

COPY ./src/main/Core/. ./Core/
COPY ./src/main/Infra.HttpClient/. ./Infra.HttpClient/
COPY ./src/main/Infra.MongoDB/. ./Infra.MongoDB/
COPY ./src/main/EntryPoint.WebApi/. ./EntryPoint.WebApi/

WORKDIR "/source/EntryPoint.WebApi"
FROM build AS publish
RUN dotnet publish "EntryPoint.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

# The .NET runtime images ship a non-root "app" user (uid 1654) and no longer carry adduser/useradd,
# so ownership is set while copying instead of by creating a user of our own.
COPY --from=publish --chown=app:app /app/publish .

ENV COMPlus_EnableDiagnostics=0

EXPOSE 8000
ENV ASPNETCORE_URLS=http://*:8000

USER app

ENTRYPOINT ["dotnet", "EntryPoint.WebApi.dll"]
