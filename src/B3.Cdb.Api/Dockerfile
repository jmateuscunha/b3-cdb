FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["/B3.Cdb.Api/B3.Cdb.Api.csproj", "./B3.Cdb.Api/"]
RUN dotnet restore "./B3.Cdb.Api/B3.Cdb.Api.csproj"

COPY . .
WORKDIR "./B3.Cdb.Api"
RUN dotnet build "B3.Cdb.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "B3.Cdb.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "B3.Cdb.Api.dll"]