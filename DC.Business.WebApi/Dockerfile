#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY DC.Business.WebApi/DC.Business.WebApi.csproj DC.Business.WebApi/
RUN dotnet restore "DC.Business.WebApi/DC.Business.WebApi.csproj"
COPY . .
WORKDIR "/src/DC.Business.WebApi"
RUN dotnet build "DC.Business.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DC.Business.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DC.Business.WebApi.dll"]
