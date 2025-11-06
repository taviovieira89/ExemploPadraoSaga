# Use a SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the solution file and restore dependencies
COPY ContratacaoService.sln .
COPY ContratacaoService/ContratacaoService.Api/*.csproj ContratacaoService/ContratacaoService.Api/
COPY ContratacaoService/ContratacaoService.Application/*.csproj ContratacaoService/ContratacaoService.Application/
COPY ContratacaoService/ContratacaoService.Domain/*.csproj ContratacaoService/ContratacaoService.Domain/
COPY ContratacaoService/ContratacaoService.Infrastructure/*.csproj ContratacaoService/ContratacaoService.Infrastructure/
RUN dotnet restore ContratacaoService.sln

# Copy the remaining source code and build the application
COPY ContratacaoService/ContratacaoService.Api/ ContratacaoService/ContratacaoService.Api/
COPY ContratacaoService/ContratacaoService.Application/ ContratacaoService/ContratacaoService.Application/
COPY ContratacaoService/ContratacaoService.Domain/ ContratacaoService/ContratacaoService.Domain/
COPY ContratacaoService/ContratacaoService.Infrastructure/ ContratacaoService/ContratacaoService.Infrastructure/
WORKDIR /src/ContratacaoService/ContratacaoService.Api
RUN dotnet build "ContratacaoService.Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "ContratacaoService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use a runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContratacaoService.Api.dll"]