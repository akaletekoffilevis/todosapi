# Todos API - Gestion Sécurisée des Tâches

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com)

API REST sécurisée pour la gestion de tâches avec **ASP.NET Core 9**, **Entity Framework Core**, **authentification JWT** et **hachage PBKDF2**.

---

## Fonctionnalités

| Module | Endpoints | Protection |
|--------|-----------|------------|
| Inscription | `POST /api/auth/register` | ❌ Publique |
| Connexion JWT | `POST /api/auth/login` | ❌ Publique |
| CRUD Tâches | `GET/POST/PUT/DELETE /api/tasks` | ✅ JWT Bearer |
| Complétion | `PATCH /api/tasks/{id}/complete` | ✅ JWT Bearer |
| Isolation données | Chaque utilisateur voit ses tâches | ✅ JWT Claim |
| Documentation | Swagger UI + OpenAPI | Interactive |

---

## Stack Technique

| Technologie | Version |
|-------------|---------|
| ASP.NET Core | 9.0 |
| Entity Framework Core | 9.0 |
| SQLite | Latest |
| JWT Bearer | 9.0.0 |
| Swashbuckle / Swagger | 6.5.0 |

---

## Démarrage Rapide

```bash
dotnet restore
dotnet run
# 👉 http://localhost:5252/swagger
```

**S'inscrire :**
```bash
curl -X POST http://localhost:5252/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"username":"testuser","password":"Test@1234"}'
```

**Se connecter :**
```bash
curl -X POST http://localhost:5252/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"testuser","password":"Test@1234"}'
```

**Utiliser le token :**
```bash
export TOKEN="votre_token_jwt"
curl http://localhost:5252/api/tasks \
  -H "Authorization: Bearer $TOKEN"
```

---

## Structure du Projet

```
todosapi/
├── Controllers/           # Endpoints API
│   ├── TodoAuthController.cs
│   └── TodoController.cs
├── Data/                  # Modèles & DbContext
│   ├── Todo.cs
│   ├── User.cs
│   └── TodoDbContext.cs
├── Services/              # Logique métier
│   ├── TodoService.cs
│   └── Interfaces/
│       └── TodoServiceInterface.cs
├── Migrations/            # EF Core migrations
├── Properties/            # Configuration launch
├── Docs/                  # Documentation
├── Program.cs             # Point d'entrée
├── appsettings.json       # Configuration
└── TodosApi.http          # Tests REST Client
```

---

## Documentation

| Document | Description |
|----------|-------------|
| [Docs/API_DOCUMENTATION.md](Docs/API_DOCUMENTATION.md) | Endpoints détaillés avec exemples |
| [Docs/PRODUCTION_GUIDE.md](Docs/PRODUCTION_GUIDE.md) | Déploiement & configuration |
| [Docs/MIGRATE_TO_SQLSERVER.md](Docs/MIGRATE_TO_SQLSERVER.md) | Migration SQLite → SQL Server |

---

## Sécurité

- **Hachage** : PBKDF2 avec SHA256, 100 000 itérations, sel 16 bytes
- **JWT** : HS256, claims NameIdentifier + Name + Jti, validité 1h
- **Isolation** : toutes les requêtes filtrées par `UserId` extrait du token

---

## Licence

MIT — voir [LICENSE](LICENSE)
