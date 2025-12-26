#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#USER $APP_UID
#WORKDIR /app
#EXPOSE 8080
#EXPOSE 8081
#
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#COPY ["Project-1/Project-1/Project-1.csproj", "Project-1/Project-1/"]
#RUN dotnet restore "Project-1/Project-1/Project-1.csproj"
#COPY . .
#WORKDIR "/src/Project-1/Project-1"
#RUN dotnet build "./Project-1.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "./Project-1.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "Project-1.dll"]

#FROM mcr.microsoft.com/dotnet/sdk:8.0
#WORKDIR /app
#COPY . .
#RUN dotnet publish -c release -o myOutput
#ENTRYPOINT ["dotnet", "myOutput/Project-1.dll"]

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
# RUN touch "/myApp/Logs/log.txt"
VOLUME ["/myApp/Logs"]
ENV LOG_PATH="/myApp/Logs/log.txt"
ENTRYPOINT ["dotnet", "Project-1.dll"]
