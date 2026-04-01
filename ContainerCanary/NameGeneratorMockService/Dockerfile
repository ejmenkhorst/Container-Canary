FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# copy csproj and restore to leverage layer cache
COPY *.csproj ./
RUN dotnet restore ./NameGeneratorMockService.csproj

# copy remaining sources and publish
COPY . .
RUN dotnet publish ./NameGeneratorMockService.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "NameGeneratorMockService.dll"]