FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /iit

# Copy csproj and restore as distinct layers
#COPY *.csproj ./
#RUN dotnet restore

# Copy everything else and build
COPY Server/. ./Server
COPY Core/. ./Core
RUN dotnet publish -c Release Server/IIT.Server/ -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /iit/out .
ENTRYPOINT ["dotnet", "IIT.Server.dll"]