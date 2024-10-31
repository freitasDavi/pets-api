FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081



FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Pets.csproj", "./"]
RUN dotnet restore "Pets.csproj"
COPY . .
ARG db_database
ARG db_host
ARG db_password
ARG db_user
ARG token_audience
ARG token_issuer
ARG token_key
WORKDIR "/src/"
RUN echo '{ "Token": { "Audience": "'$token_audience'", "Issuer": "'$token_issuer'", "Key": "'$token_key'" }, "ConnectionString": { "db": "Host='$db_host';Database='$db_database';Username='$db_user';Password='$db_password'" } }' > appsettings.Release.json
RUN dotnet build "Pets.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Pets.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pets.dll"]
