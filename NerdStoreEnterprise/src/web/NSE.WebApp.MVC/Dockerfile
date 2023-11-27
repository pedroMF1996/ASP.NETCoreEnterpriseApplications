#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["web/NSE.WebApp.MVC/NSE.WebApp.MVC.csproj", "web/NSE.WebApp.MVC/"]
COPY ["building blocks/NSE.Core/NSE.Core.csproj", "building blocks/NSE.WebApp.Core/"]
COPY ["building blocks/NSE.WebAPI.Core/NSE.WebAPI.Core.csproj", "building blocks/NSE.WebApp.WebAPI.Core/"]

RUN dotnet restore "web/NSE.WebApp.MVC/NSE.WebApp.MVC.csproj"
COPY . .

WORKDIR "web/NSE.WebApp.MVC"
RUN dotnet build "NSE.WebApp.MVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NSE.WebApp.MVC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app 
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NSE.WebApp.MVC.dll"]