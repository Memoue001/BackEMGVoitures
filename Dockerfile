# Étape de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiez les fichiers du projet et restaurez les dépendances
COPY . .
RUN dotnet restore

# Publiez l'application
RUN dotnet publish -c Release -o /app/publish

# Étape d'exécution
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiez les fichiers publiés depuis l'étape de build
COPY --from=build /app/publish .

# Exposez le port utilisé par l'application
EXPOSE 80

# Définissez la commande pour exécuter l'application
ENTRYPOINT ["dotnet", "EMGVoitures.dll"]