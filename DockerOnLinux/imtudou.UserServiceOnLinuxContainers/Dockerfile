#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 5004

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["DockerOnLinux/imtudou.UserServiceOnLinuxContainers/imtudou.UserServiceOnLinuxContainers.csproj", "DockerOnLinux/imtudou.UserServiceOnLinuxContainers/"]
RUN dotnet restore "DockerOnLinux/imtudou.UserServiceOnLinuxContainers/imtudou.UserServiceOnLinuxContainers.csproj"
COPY . .
WORKDIR "/src/DockerOnLinux/imtudou.UserServiceOnLinuxContainers"
RUN dotnet build "imtudou.UserServiceOnLinuxContainers.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "imtudou.UserServiceOnLinuxContainers.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "imtudou.UserServiceOnLinuxContainers.dll"]