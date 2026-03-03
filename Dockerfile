FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["dotnet-poc/dotnet-poc/dotnet-poc.csproj", "dotnet-poc/dotnet-poc/"]
RUN dotnet restore "dotnet-poc/dotnet-poc/dotnet-poc.csproj"
COPY . .
WORKDIR "/src/dotnet-poc/dotnet-poc"
RUN dotnet build "dotnet-poc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "dotnet-poc.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "dotnet-poc.dll"]
