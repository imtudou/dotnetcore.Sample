#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 5006

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["DockerOnLinux/imtudou.ProductServiceOnLinuxContainers/imtudou.ProductServiceOnLinuxContainers.csproj", "DockerOnLinux/imtudou.ProductServiceOnLinuxContainers"]
RUN dotnet restore "DockerOnLinux/imtudou.ProductServiceOnLinuxContainers/imtudou.ProductServiceOnLinuxContainers.csproj"
COPY . .
WORKDIR "/src/DockerOnLinux/imtudou.ProductServiceOnLinuxContainers"
RUN dotnet build "imtudou.ProductServiceOnLinuxContainers.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "imtudou.ProductServiceOnLinuxContainers.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "imtudou.ProductServiceOnLinuxContainers.dll"]