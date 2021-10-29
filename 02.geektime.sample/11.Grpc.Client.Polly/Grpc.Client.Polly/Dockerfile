#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-nanoserver-1903 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-nanoserver-1903 AS build
WORKDIR /src
COPY ["samples/11.Grpc.Client.Polly/Grpc.Client.Polly/11.Grpc.Client.Polly.csproj", "samples/11.Grpc.Client.Polly/Grpc.Client.Polly/"]
RUN dotnet restore "samples/11.Grpc.Client.Polly/Grpc.Client.Polly/11.Grpc.Client.Polly.csproj"
COPY . .
WORKDIR "/src/samples/11.Grpc.Client.Polly/Grpc.Client.Polly"
RUN dotnet build "11.Grpc.Client.Polly.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "11.Grpc.Client.Polly.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "11.Grpc.Client.Polly.dll"]