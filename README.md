# Pour le lancement en local < il faut lancer le back et le front separement >


# EMG Voitures

## Prérequis
- .NET SDK 6 ou supérieur
- SQL Server ou LocalDB

## Configuration
1. Clonez le repository.
2. Configurez votre chaîne de connexion à la base de données dans `appsettings.json`.
3. Créez la base de données via les migrations :

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
