FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["WebSocketChat/WebSocketChat.csproj", "WebSocketChat/"]
RUN dotnet restore "WebSocketChat/WebSocketChat.csproj"
COPY . .
WORKDIR "/src/WebSocketChat"
RUN dotnet build "WebSocketChat.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebSocketChat.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebSocketChat.dll"]