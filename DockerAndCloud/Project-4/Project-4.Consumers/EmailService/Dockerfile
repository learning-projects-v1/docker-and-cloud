# runtime
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime

# build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /build
COPY *.csproj .
COPY ../../CoreLibrary/*.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c release -o /publish

FROM runtime AS final
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "EmailService.dll"]
