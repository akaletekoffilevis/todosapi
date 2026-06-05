# Index de la Documentation Todos API

## Documentation Principale

### Pour Commencer

- **[Démarrage Rapide](QUICKSTART.md)**
  - Installation en 5 minutes
  - Premiers appels API
  - Exemples avec curl

### Développement

- **[Documentation API Complète](API_DOCUMENTATION.md)**
  - Tous les endpoints détaillés
  - Exemples de requêtes/réponses
  - Codes d'erreur et validations
  - Authentification JWT

- **[Guide de Test](TESTING_GUIDE.md)**
  - Tests avec curl
  - Tests avec VS Code REST Client
  - Tests de sécurité

### Production

- **[Guide Production](PRODUCTION_GUIDE.md)**
  - Déploiement
  - Configuration de la base de données
  - Variables d'environnement
  - Logs et monitoring

### Documentation de Référence

- **[MIGRATE_TO_SQLSERVER.md](MIGRATE_TO_SQLSERVER.md)** - Migration SQLite vers SQL Server
- **[FUTURE_IMPROVEMENTS.md](FUTURE_IMPROVEMENTS.md)** - Roadmap et améliorations planifiées

---

## Fichiers de Référence à la Racine

### Configuration & Setup

- [README.md](../README.md) - Vue d'ensemble du projet
- [Program.cs](../Program.cs) - Point d'entrée de l'application
- [TodosApi.csproj](../TodosApi.csproj) - Configuration du projet
- [appsettings.json](../appsettings.json) - Configuration globale
- [appsettings.Development.json](../appsettings.Development.json) - Configuration développement

### Tests

- [TodosApi.http](../TodosApi.http) - Fichier de test HTTP pour VS Code
- [API_TEST_GUIDE.http](../API_TEST_GUIDE.http) - Guide des tests HTTP

### Contribution

- [CONTRIBUTING.md](../CONTRIBUTING.md) - Guide pour contribuer au projet
- [CHANGELOG.md](../CHANGELOG.md) - Historique des versions
- [LICENSE](../LICENSE) - Licence MIT

---

## Structure du Projet

```
todosapi/
├── Controllers/
│   ├── TodoAuthController.cs
│   └── TodoController.cs
├── Data/
│   ├── Todo.cs
│   ├── User.cs
│   └── TodoDbContext.cs
├── Services/
│   ├── TodoService.cs
│   └── Interfaces/
│       └── TodoServiceInterface.cs
├── Docs/
│   ├── INDEX.md (ce fichier)
│   ├── QUICKSTART.md
│   ├── API_DOCUMENTATION.md
│   ├── TESTING_GUIDE.md
│   ├── PRODUCTION_GUIDE.md
│   ├── MIGRATE_TO_SQLSERVER.md
│   └── FUTURE_IMPROVEMENTS.md
├── Migrations/
├── Properties/
├── Program.cs
├── README.md
├── CHANGELOG.md
├── CONTRIBUTING.md
└── LICENSE
```

---

## Parcours Recommandé

### Nouveau Développeur
1. Lire [README.md](../README.md)
2. Suivre [Démarrage Rapide](QUICKSTART.md)
3. Consulter [Documentation API](API_DOCUMENTATION.md)
4. Essayer les tests: [Guide de Test](TESTING_GUIDE.md)

### En Production
1. Consulter [Guide Production](PRODUCTION_GUIDE.md)
2. Lire [CHANGELOG.md](../CHANGELOG.md)

### Contribuer
1. Lire [CONTRIBUTING.md](../CONTRIBUTING.md)
2. Consulter [FUTURE_IMPROVEMENTS.md](FUTURE_IMPROVEMENTS.md)
3. Suivre le processus de Pull Request

---

## Liens Rapides

| Ressource | Lien |
|-----------|------|
| .NET 9 Documentation | https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-9 |
| ASP.NET Core Documentation | https://learn.microsoft.com/en-us/aspnet/core/ |
| Entity Framework Core | https://learn.microsoft.com/en-us/ef/core/ |

---

## Support

- Ouvrir une Issue GitHub
- Consulter la [Documentation](INDEX.md)
