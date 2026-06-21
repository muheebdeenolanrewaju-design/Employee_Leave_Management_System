FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything from your local directory into the container
COPY . .

# Restore dependencies and compile/publish the app in Release mode
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Switch to the lightweight runtime image for the final container
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Force .NET to run on Render's preferred port
ENV ASPNETCORE_HTTP_PORTS=10000
EXPOSE 10000

# Copy the compiled files from the build stage
COPY --from=build /app/publish .

# CRITICAL: Ensure this name matches your exact .csproj file name!
ENTRYPOINT ["dotnet", "Employee_Leave_Management_System.dll"]
