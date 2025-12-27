# stage 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /myBuild
COPY Project-1.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c release -o /myPublish

# stage 2: run
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS run
WORKDIR /myApp
COPY --from=build /myPublish .

# log folder
RUN mkdir "/myApp/Logs" && chmod 777 Logs
VOLUME ["/myApp/Logs"]
ENV LOG_PATH="/myApp/Logs/log.txt"
ENTRYPOINT ["dotnet", "Project-1.dll"]
