#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SolaraNet/SolaraNet.csproj", "SolaraNet/"]
COPY ["SolaraNet.DataAccessLayer.EntityFramework/SolaraNet.DataAccessLayer.EntityFramework.csproj", "SolaraNet.DataAccessLayer.EntityFramework/"]
COPY ["AbstractsExceptions/AbstractsExceptions.csproj", "AbstractsExceptions/"]
COPY ["SolaraNet.DataAccessLayer.Entities/SolaraNet.DataAccessLayer.Entities.csproj", "SolaraNet.DataAccessLayer.Entities/"]
COPY ["SolaraNet.DataAccessLayer.Abstracts/SolaraNet.DataAccessLayer.Abstracts.csproj", "SolaraNet.DataAccessLayer.Abstracts/"]
COPY ["SolaraNet.BusinessLogic.Abstracts/SolaraNet.BusinessLogic.Abstracts.csproj", "SolaraNet.BusinessLogic.Abstracts/"]
COPY ["SolaraNet.BusinessLogic.Contracts/SolaraNet.BusinessLogic.Contracts.csproj", "SolaraNet.BusinessLogic.Contracts/"]
COPY ["SolaraNet.BusinessLogic.Implementations/SolaraNet.BusinessLogic.Implementations.csproj", "SolaraNet.BusinessLogic.Implementations/"]
RUN dotnet restore "SolaraNet/SolaraNet.csproj"
COPY . .
WORKDIR "/src/SolaraNet"
RUN dotnet build "SolaraNet.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SolaraNet.csproj" -c Release -o /app/publish

FROM base AS runtime
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SolaraNet.dll"]