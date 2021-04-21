#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
ENV ASPNETCORE_URLS "http://+:5000"
ENV ASPNETCORE_ENVIRONMENT Staging

WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Service.Data/Service.Data.csproj", "Service.Data/"]
COPY ["Service.Base/Service.Base.csproj", "Service.Base/"]
RUN dotnet restore "Service.Data/Service.Data.csproj"
COPY . .
WORKDIR "/src/Service.Data"
RUN dotnet build "Service.Data.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Service.Data.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Service.Data.dll"]