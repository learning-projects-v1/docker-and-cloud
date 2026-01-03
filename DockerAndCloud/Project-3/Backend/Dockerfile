# stage 1
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /build
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c release -o /publish

# stage 2
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS run
WORKDIR /myApp
COPY --from=build /publish .

# make log path
#RUN mkdir "/myApp/Logs" && chmod 777 Logs
#VOLUME ["/myApp/Logs"]

EXPOSE 8080
ENTRYPOINT ["dotnet", "Project-3.dll"]


