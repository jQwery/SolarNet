#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SolaraNet.AuthAPI/SolaraNet.AuthAPI.csproj", "SolaraNet.AuthAPI/"]
COPY ["SolaraNet.DataAccessLayer.EntityFramework/SolaraNet.DataAccessLayer.EntityFramework.csproj", "SolaraNet.DataAccessLayer.EntityFramework/"]
COPY ["AbstractsExceptions/AbstractsExceptions.csproj", "AbstractsExceptions/"]
COPY ["SolaraNet.DataAccessLayer.Entities/SolaraNet.DataAccessLayer.Entities.csproj", "SolaraNet.DataAccessLayer.Entities/"]
COPY ["SolaraNet.DataAccessLayer.Abstracts/SolaraNet.DataAccessLayer.Abstracts.csproj", "SolaraNet.DataAccessLayer.Abstracts/"]
COPY ["SolaraNet.BusinessLogic.Abstracts/SolaraNet.BusinessLogic.Abstracts.csproj", "SolaraNet.BusinessLogic.Abstracts/"]
COPY ["SolaraNet.BusinessLogic.Contracts/SolaraNet.BusinessLogic.Contracts.csproj", "SolaraNet.BusinessLogic.Contracts/"]
COPY ["SolaraNet.BusinessLogic.Implementations/SolaraNet.BusinessLogic.Implementations.csproj", "SolaraNet.BusinessLogic.Implementations/"]
RUN dotnet restore "SolaraNet.AuthAPI/SolaraNet.AuthAPI.csproj"
COPY . .
WORKDIR "/src/SolaraNet.AuthAPI"
RUN dotnet build "SolaraNet.AuthAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SolaraNet.AuthAPI.csproj" -c Release -o /app/publish

FROM base AS runtime
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SolaraNet.AuthAPI.dll"]