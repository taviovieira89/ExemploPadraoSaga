# Use a SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the solution file and restore dependencies
COPY PropostaService.sln .
COPY PropostaService/PropostaService.Api/*.csproj PropostaService/PropostaService.Api/
COPY PropostaService/PropostaService.Application/*.csproj PropostaService/PropostaService.Application/
COPY PropostaService/PropostaService.Domain/*.csproj PropostaService/PropostaService.Domain/
COPY PropostaService/PropostaService.Infrastructure/*.csproj PropostaService/PropostaService.Infrastructure/
RUN dotnet restore PropostaService.sln

# Copy the remaining source code and build the application
COPY PropostaService/PropostaService.Api/ PropostaService/PropostaService.Api/
COPY PropostaService/PropostaService.Application/ PropostaService/PropostaService.Application/
COPY PropostaService/PropostaService.Domain/ PropostaService/PropostaService.Domain/
COPY PropostaService/PropostaService.Infrastructure/ PropostaService/PropostaService.Infrastructure/
WORKDIR /src/PropostaService/PropostaService.Api
RUN dotnet build "PropostaService.Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "PropostaService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use a runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PropostaService.Api.dll"]