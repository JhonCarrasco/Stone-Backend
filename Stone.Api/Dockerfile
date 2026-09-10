# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

#ENV DB_CONNECTION_STRING="Server=localhost,1433;Initial Catalog=StoneDb;Persist Security Info=False;User ID=sa;Password=esteban;Trust Server Certificate=True;"
#ENV DB_CONNECTION_STRING="Data Source=192.168.100.6,14330;Initial Catalog=StoneDb;Persist Security Info=True;User ID=SA;Password=Esteban123.;TrustServerCertificate=True;"
# ENV DB_CONNECTION_STRING="Data Source=azuresqledge,1433;Initial Catalog=StoneDb;Persist Security Info=True;User ID=SA;Password=Esteban123.;TrustServerCertificate=True;"
#ENV DB_CONNECTION_STRING="Server=azuresqledge,1433;Initial Catalog=StoneDb;Persist Security Info=False;User ID=sa;Password=Esteban123.;Trust Server Certificate=True;"

# This stage is used to build the service project 
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Stone.Api/Stone.Api.csproj", "Stone.Api/"]
COPY ["Stone.Dto/Stone.Dto.csproj", "Stone.Dto/"]
COPY ["Stone.Entities/Stone.Entities.csproj", "Stone.Entities/"]
COPY ["Stone.Services/Stone.Services.csproj", "Stone.Services/"]
COPY ["Stone.Repositories/Stone.Repositories.csproj", "Stone.Repositories/"]
COPY ["Stone.Persistence/Stone.Persistence.csproj", "Stone.Persistence/"]
RUN dotnet restore "./Stone.Api/Stone.Api.csproj"
COPY . .
WORKDIR "/src/Stone.Api"
RUN dotnet build "./Stone.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Stone.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Stone.Api.dll"]