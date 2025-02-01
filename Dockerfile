# �tape de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiez les fichiers du projet et restaurez les d�pendances
COPY . .
RUN dotnet restore

# Publiez l'application
RUN dotnet publish -c Release -o /app/publish

# �tape d'ex�cution
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiez les fichiers publi�s depuis l'�tape de build
COPY --from=build /app/publish .

# Exposez le port utilis� par l'application
EXPOSE 80

# D�finissez la commande pour ex�cuter l'application
ENTRYPOINT ["dotnet", "EMGVoitures.dll"]