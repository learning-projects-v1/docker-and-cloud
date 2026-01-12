FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /build

COPY Project-4.Api/Project-4.Publisher.Api/Project-4.Publisher.Api.csproj Api/Project-4.Publisher.Api/
RUN dotnet restore Api/Project-4.Publisher.Api/Project-4.Publisher.Api.csproj

COPY Project-4.Api/ Api/
COPY CoreLibrary/ CoreLibrary/
RUN dotnet publish Api/Project-4.Publisher.Api/Project-4.Publisher.Api.csproj -c Release -o /publish

FROM runtime
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "Project-4.Publisher.Api.dll"]
