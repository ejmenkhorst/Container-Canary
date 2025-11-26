FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# copy project files and restore
COPY . .
RUN dotnet restore

# publish
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80

COPY --from=build /app/publish .
EXPOSE 80

# Replace "Canary-API.dll" below with the actual assembly name if different
ENTRYPOINT ["dotnet", "Canary-API.dll"]