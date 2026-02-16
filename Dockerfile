# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore
COPY FynosAPI/FynosAPI.csproj ./FynosAPI/
RUN dotnet restore ./FynosAPI/FynosAPI.csproj

# Copy everything else
COPY FynosAPI ./FynosAPI
WORKDIR /app/FynosAPI
RUN dotnet publish -c Release -o /out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .
EXPOSE 8080
ENTRYPOINT ["dotnet", "FynosAPI.dll"]
