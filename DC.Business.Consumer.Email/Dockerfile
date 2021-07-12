#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 81

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["DC.Business.Consumer.Email/DC.Business.Consumer.Email.csproj", "DC.Business.Consumer.Email/"]
RUN dotnet restore "DC.Business.Consumer.Email/DC.Business.Consumer.Email.csproj"
COPY . .
WORKDIR "/src/DC.Business.Consumer.Email"
RUN dotnet build "DC.Business.Consumer.Email.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DC.Business.Consumer.Email.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DC.Business.Consumer.Email.dll"]