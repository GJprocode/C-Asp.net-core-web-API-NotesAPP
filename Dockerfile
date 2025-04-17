# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything and restore dependencies
COPY . ./
RUN dotnet restore

# Build the app
RUN dotnet publish -c Release -o out

# Use the ASP.NET runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Expose the port (adjust if your app uses a different one)
EXPOSE 80

# Start the app
ENTRYPOINT ["dotnet", "NotesBE.dll"]
