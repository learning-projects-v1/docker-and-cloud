# runtime
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime

# build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /build
COPY Project-4.Consumers/RetryService/RetryService.csproj Consumers/RetryService/

RUN dotnet restore Consumers/RetryService/RetryService.csproj 
COPY Project-4.Consumers/RetryService/ Consumers/RetryService/
COPY CoreLibrary/ CoreLibrary/
RUN dotnet publish -c release -o /publish Consumers/RetryService/RetryService.csproj

FROM runtime AS final
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "RetryService.dll"]
