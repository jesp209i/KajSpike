#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["KajSpike/KajSpike.Api.csproj", "KajSpike/"]
COPY ["KajSpike.ApplicationService/KajSpike.ApplicationService.csproj", "KajSpike.ApplicationService/"]
COPY ["KajSpike.Domain/KajSpike.Domain.csproj", "KajSpike.Domain/"]
COPY ["KajSpike.Framework/KajSpike.Framework.csproj", "KajSpike.Framework/"]
COPY ["KajSpike.Infrastructure/KajSpike.Infrastructure.csproj", "KajSpike.Infrastructure/"]
RUN dotnet restore "KajSpike/KajSpike.Api.csproj"
COPY . .
WORKDIR "/src/KajSpike"
RUN dotnet build "KajSpike.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KajSpike.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KajSpike.Api.dll"]