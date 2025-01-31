# Pour le lancement en local < il faut lancer le back et le front separement >


# EMG Voitures

## Pr�requis
- .NET SDK 6 ou sup�rieur
- SQL Server ou LocalDB

## Configuration
1. Clonez le repository.
2. Configurez votre cha�ne de connexion � la base de donn�es dans `appsettings.json`.
3. Cr�ez la base de donn�es via les migrations :

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
