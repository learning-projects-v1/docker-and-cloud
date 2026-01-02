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

# volume for logs
#RUN mkdir "/myApp/Logs" && chmod 777 Logs
#VOLUME ["/myApp/Logs"]
#ENV LOG_PATH="/myApp/Logs/log.txt"
#ENV ASPNETCORE_URLS=http://0.0.0.0:8080
#ENV ASPNETCORE_ENVIRONMENT=Production
#ENV FRONTEND_BASE=http://localhost:4200

EXPOSE 8080
ENTRYPOINT ["dotnet", "Project-2.dll"]


