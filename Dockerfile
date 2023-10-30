FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source

COPY ["Core/Core.csproj", "./Core/"]
COPY ["Infra.HttpClient/Infra.HttpClient.csproj", "./Infra.HttpClient/"]
COPY ["Infra.MongoDB/Infra.MongoDB.csproj", "./Infra.MongoDB/"]
COPY ["EntryPoint.WebApi/EntryPoint.WebApi.csproj", "./EntryPoint.WebApi/"]

RUN dotnet restore "./EntryPoint.WebApi/EntryPoint.WebApi.csproj" --force --no-cache

COPY ./Core/. ./Core/
COPY ./Infra.HttpClient/. ./Infra.HttpClient/
COPY ./Infra.MongoDB/. ./Infra.MongoDB/
COPY ./EntryPoint.WebApi/. ./EntryPoint.WebApi/

WORKDIR "/source/EntryPoint.WebApi"
FROM build AS publish
RUN dotnet publish "EntryPoint.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EntryPoint.WebApi.dll"]

ENV COMPlus_EnableDiagnostics=0

EXPOSE 8000
ENV ASPNETCORE_URLS=http://*:8000

RUN addgroup --group ragnarok --gid 2000 \
&& adduser \    
    --uid 1000 \
    --gid 2000 \
    "surtur" 

RUN chown surtur:ragnarok /app
USER surtur:ragnarok
