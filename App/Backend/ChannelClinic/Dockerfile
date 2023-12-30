#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ChannelClinic/ChannelClinic.csproj", "ChannelClinic/"]
RUN dotnet restore "ChannelClinic/ChannelClinic.csproj"
COPY . .
WORKDIR "/src/ChannelClinic"
RUN dotnet build "ChannelClinic.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChannelClinic.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5000
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChannelClinic.dll"]