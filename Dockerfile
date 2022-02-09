<<<<<<< Updated upstream
FROM  mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 8000

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["WebAPI/WebAPI.csproj", "WebAPI/"]
COPY ["Business/Business.csproj", "Business/"]
COPY ["DataAccess/DataAccess.csproj", "DataAccess/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["Entities/Entities.csproj", "Entities/"]
RUN dotnet restore "WebAPI/WebAPI.csproj" 
COPY . .
WORKDIR "/src/WebAPI"
RUN dotnet build "WebAPI.csproj" -c Release -o /app/build
 
FROM build AS publish
RUN dotnet publish "WebAPI.csproj" -c Release -o /app/build

FROM base AS final
WORKDIR /app
COPY --from=publish /app/build .

ENV COMPlus_EnableDiagnostics=0 
ENV ASPNETCORE_URLS="http://*:8000"
ENV ASPNETCORE_ENVIRONMENT Production
ENTRYPOINT ["dotnet", "WebAPI.dll"]
=======
# https://itnext.io/smaller-docker-images-for-asp-net-core-apps-bee4a8fd1277

FROM alpine:latest as build
RUN apk add --no-cache libstdc++ libintl
WORKDIR /app
EXPOSE 80
COPY WebAPI/*.csproj .
RUN dotnet restore WebAPI/*.csproj
COPY . .
RUN dotnet publish WebAPI/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","Appneuron.Remote.api.dll" ]
>>>>>>> Stashed changes
