# runtime
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime

# build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /build
COPY Project-4.Consumers/PaymentService/PaymentService.csproj Consumers/PaymentService/

RUN dotnet restore Consumers/PaymentService/PaymentService.csproj 
COPY Project-4.Consumers/PaymentService/ Consumers/PaymentService/
COPY CoreLibrary/ CoreLibrary/
RUN dotnet publish -c release -o /publish Consumers/PaymentService/PaymentService.csproj

FROM runtime AS final
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "PaymentService.dll"]
