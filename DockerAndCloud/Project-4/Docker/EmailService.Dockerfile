# runtime
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime

# build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /build
COPY Project-4.Consumers/EmailService/EmailService.csproj Consumers/EmailService/

RUN dotnet restore Consumers/EmailService/EmailService.csproj 
COPY Project-4.Consumers/EmailService/ Consumers/EmailService/
COPY CoreLibrary/ CoreLibrary/
RUN dotnet publish -c release -o /publish Consumers/EmailService/EmailService.csproj

FROM runtime AS final
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "EmailService.dll"]
