# Todos API - Gestion Sécurisée des Tâches

Une API REST robuste et sécurisée pour la gestion d'une liste de tâches (Todo List) développée avec **ASP.NET Core 9** et **Entity Framework Core**.
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com)

## 📚 Documentation

- 🚀 **[Démarrage Rapide](Docs/QUICKSTART.md)** - Installation et premiers pas
- 📖 **[Documentation API](Docs/API_DOCUMENTATION.md)** - Endpoints détaillés avec exemples
- 🧪 **[Guide de Test](Docs/TESTING_GUIDE.md)** - Tests avec VS Code REST Client
- 📦 **[Guide Production](Docs/PRODUCTION_GUIDE.md)** - Déploiement et configuration
- 📚 **[Index Complet](Docs/INDEX.md)** - Navigation de toute la documentation
- 🗄️ **[Migration SQLite → SQL Server](Docs/MIGRATE_TO_SQLSERVER.md)** - Guide migration base de données

## 🎯 Fonctionnalités Principales

### Authentification & Sécurité

- ✅ **Inscription d'utilisateurs** : `POST /api/auth/register`
- ✅ **Authentification JWT** : `POST /api/auth/login` (token valide 1 heure)
- ✅ **Hachage sécurisé** : PBKDF2 avec SHA256 (100,000 itérations)
- ✅ **Autorisation JWT Bearer** : Protection de tous les endpoints des tâches
- ✅ **Validation des données** : Data Annotations côté serveur
- ✅ **Isolation des données** : Chaque utilisateur ne voit que ses tâches

### Gestion des Tâches (Protégées par JWT)

- ✅ **Récupérer les tâches** : `GET /api/tasks`
- ✅ **Créer une tâche** : `POST /api/tasks`
- ✅ **Modifier une tâche** : `PUT /api/tasks/{id}`
- ✅ **Marquer comme complétée** : `PATCH /api/tasks/{id}/complete`
- ✅ **Supprimer une tâche** : `DELETE /api/tasks/{id}`

### Documentation & Testing

- ✅ **Swagger UI** : Documentation interactive
- ✅ **Fichiers HTTP** : Tests directs avec VS Code REST Client
- ✅ **Documentation complète** : API_DOCUMENTATION.md avec exemples

---

## 📦 Technologies Utilisées

| Technologie | Version | Utilité |
|-----------|---------|---------|
| ASP.NET Core | 9.0 | Framework web |
| Entity Framework Core | 9.0 | ORM |
| SQLite | Latest | Base de données |
| JWT Bearer | 9.0.0 | Authentification |
| Swagger/OpenAPI | 6.5.0 | Documentation API |
| .NET | 9.0 | Runtime |

---

## 🚀 Démarrage Rapide

### Prérequis

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Un terminal ou PowerShell
- (Optionnel) [VS Code](https://code.visualstudio.com/)

### Installation & Exécution

1. **Cloner ou extraire le projet**

   ```bash
   cd todosapi
   ```

2. **Restaurer les dépendances**

   ```bash
   dotnet restore
   ```

3. **Construire le projet**

   ```bash
   dotnet build
   ```

4. **Exécuter l'application**

   ```bash
   dotnet run
   ```

5. **Accéder à l'API**
   - **Swagger UI** : <http://localhost:5252/swagger>
   - **Base API** : <http://localhost:5252>

---

## 📚 Guide d'Utilisation

### 1️⃣ S'inscrire

```bash
curl -X POST http://localhost:5252/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "password": "Test@1234"
  }'
```

**Réponse** (201 Created) :

```json
{
  "id": 1,
  "username": "testuser",
  "message": "User registered successfully"
}
```

### 2️⃣ Se Connecter

```bash
curl -X POST http://localhost:5252/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "password": "Test@1234"
  }'
```

**Réponse** (200 OK) :

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "username": "testuser"
  },
  "message": "Login successful"
}
```

**Conservez ce token** - il sera nécessaire pour tous les appels aux endpoints des tâches.

### 3️⃣ Créer une Tâche

```bash
TOKEN="votre_token_jwt"
curl -X POST http://localhost:5252/api/tasks \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "title": "Faire les courses",
    "description": "Lait, œufs, pain"
  }'
```

### 4️⃣ Récupérer les Tâches

```bash
curl -X GET http://localhost:5252/api/tasks \
  -H "Authorization: Bearer $TOKEN"
```

### 5️⃣ Modifier une Tâche

```bash
curl -X PUT http://localhost:5252/api/tasks/1 \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "title": "Faire les courses (URGENT)",
    "description": "Lait, œufs, pain, fromage",
    "isCompleted": false
  }'
```

### 6️⃣ Marquer comme Complétée

```bash
curl -X PATCH "http://localhost:5252/api/tasks/1/complete?value=true" \
  -H "Authorization: Bearer $TOKEN"
```

### 7️⃣ Supprimer une Tâche

```bash
curl -X DELETE http://localhost:5252/api/tasks/1 \
  -H "Authorization: Bearer $TOKEN"
```

---

## 📖 Documentation Complète

Consultez [API_DOCUMENTATION.md](./API_DOCUMENTATION.md) pour :

- Tous les endpoints détaillés
- Exemples complets avec cURL et Postman
- Détails de sécurité
- Troubleshooting
- Configuration avancée

---

## 🧪 Test des Endpoints

### Avec REST Client (VS Code)

1. Installez l'extension [REST Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client)
2. Ouvrez le fichier `TodosApi.http`
3. Cliquez sur "Send Request" au-dessus de chaque endpoint

### Avec Postman

1. Importez la configuration depuis `TodosApi.http`
2. Utilisez les variables d'environnement pour le token
3. Testez tous les endpoints interactivement

### Avec cURL

Tous les exemples utilisent cURL - voir section **Guide d'Utilisation** ci-dessus.

---

## 🔒 Architecture de Sécurité

### Authentification JWT

```
┌─────────────┐         ┌──────────┐         ┌───────────┐
│   Client    │────────>│   API    │────────>│  Database │
│             │<────────│  (Token) │<────────│           │
└─────────────┘         └──────────┘         └───────────┘
       │
       │ Authorization: Bearer <JWT>
       │ (valide 1 heure)
       └─► NameIdentifier: UserId
           Name: Username
           Jti: Unique ID
```

### Hachage des Mots de Passe

```
Password ──┐
           ├──> PBKDF2-SHA256 ──> PasswordHash
    Salt ──┘    (100,000 iterations)
    
Hash stocké en base de données
Salt stocké en base de données
```

### Isolation des Données

- Chaque requête JWT contient l'ID utilisateur
- Les tâches sont filtrées par `UserId` en base de données
- Impossible d'accéder aux tâches d'autres utilisateurs

---

## 📁 Structure du Projet

```
todosapi/
├── Controllers/
│   ├── TodoAuthController.cs      # Endpoints auth
│   └── TodoController.cs          # Endpoints tâches
├── Data/
│   ├── User.cs                    # Modèle utilisateur
│   ├── Todo.cs                    # Modèle tâche
│   └── TodoDbContext.cs           # DbContext EF Core
├── Services/
│   ├── TodoService.cs             # Logique métier
│   └── Interfaces/
│       └── TodoServiceInterface.cs # Contrat du service
├── Properties/
│   └── launchSettings.json
├── Program.cs                     # Configuration
├── appsettings.json              # Configuration app
├── TodosApi.csproj               # Fichier projet
├── TodosApi.http                 # Tests HTTP
├── API_DOCUMENTATION.md          # Doc complète
└── README.md                      # Ce fichier
```

---

## ⚙️ Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "SqliteDbConnection": "Data Source=Data/todos.db"
  },
  "Jwt": {
    "Key": "CHANGE_THIS_DEVELOPMENT_SECRET_KEY_32_CHARS_MIN",
    "Issuer": "TodosApi",
    "Audience": "TodosApiClient",
    "ExpiresMinutes": 60
  }
}
```

### Configuration en Production

**⚠️ IMPORTANT** : Changez ces valeurs avant de déployer en production :

1. **Clé JWT** : Générez une clé sécurisée d'au moins 32 caractères

   ```bash
   dotnet user-secrets set "Jwt:Key" "votre-clé-sécurisée-ici"
   ```

2. **Bases de données** : Utilisez SQL Server au lieu de SQLite

   ```json
   "ConnectionStrings": {
     "SqliteDbConnection": "Server=tcp:your-server.database.windows.net,1433;Initial Catalog=TodosDb;Persist Security Info=False;User ID=sa;Password=...;Encrypt=True;"
   }
   ```

3. **HTTPS** : Activez HTTPS obligatoirement
4. **CORS** : Configurez les origines autorisées

---

## 🧬 Flux d'Authentification

```
1. POST /api/auth/register
   ├─ Valider username/password
   ├─ Hasher password (PBKDF2)
   ├─ Créer User
   └─ Retourner User ID

2. POST /api/auth/login
   ├─ Valider username/password
   ├─ Vérifier password (PBKDF2)
   ├─ Générer JWT Token
   │  ├─ Claims: UserId, Username
   │  ├─ Expiration: +1 heure
   │  └─ Signé avec clé secrète
   └─ Retourner Token

3. GET /api/tasks (avec Bearer Token)
   ├─ Valider JWT signature
   ├─ Valider expiration
   ├─ Extraire UserId du token
   ├─ Récupérer Tasks WHERE UserId = token.UserId
   └─ Retourner Tasks
```

---

## 🐛 Troubleshooting

### La base de données n'a pas été créée

```bash
# Vérifier que le dossier Data existe
mkdir Data
# Re-exécuter l'app pour créer la base
dotnet run
```

### Token expiré

```bash
# Re-connectez-vous pour obtenir un nouveau token
POST /api/auth/login
```

### Utilisateur existe déjà

```bash
# Utilisez un username différent
POST /api/auth/register (avec un autre username)
```

### Tâche non trouvée (404)

```bash
# Assurez-vous que :
# 1. La tâche existe
# 2. La tâche appartient à votre utilisateur
# 3. Vous utilisez le bon ID
```

## 🛠️ Changements Récents (JWT & Build) – 2026-01-18

### Problème corrigé

- Erreur d'authentification JWT: `WWW-Authenticate: Bearer error="invalid_token"` et `Method not found: TokenValidationResult..ctor(...)`.

### Causes racines

- Conflits de versions entre `Microsoft.AspNetCore.Authentication.JwtBearer` 9.x et des références explicites d'IdentityModel (`Microsoft.IdentityModel.Tokens` / `System.IdentityModel.Tokens.Jwt`) qui ne correspondaient pas aux APIs attendues.
- Ordre du middleware susceptible d'empêcher la validation correcte lorsque le token est traité.
- Double `app.Run()` dans `Program.cs` (arrêt prématuré / comportement indéterminé).

### Modifications apportées

- `TodosApi.csproj`
  - Cible restaurée sur **.NET 9** (`net9.0`).
  - Paquets alignés sur **ASP.NET Core 9.x** (OpenAPI, JwtBearer, EF Core, Sqlite).
  - Suppression des références explicites à `Microsoft.IdentityModel.Tokens` et `System.IdentityModel.Tokens.Jwt` pour laisser `JwtBearer` importer les versions compatibles transitivement.

- `Program.cs`
  - Réorganisation du pipeline: `UseForwardedHeaders` → `UseCors` → `UseAuthentication` → `UseAuthorization` → `MapControllers`.
  - Désactivation de la redirection HTTPS en mode Développement (utilisation HTTP locale) pour éviter des interactions non désirées pendant les tests.
  - Ajout de handlers d'événements (`OnAuthenticationFailed`, `OnTokenValidated`, `OnChallenge`) pour journaliser les erreurs JWT.
  - Suppression des appels en double à `app.Run()`.

### Comment tester (JWT)

1. Construire et lancer:

   ```bash
   dotnet clean
   dotnet restore
   dotnet build
   dotnet run -c Debug
   ```

2. Dans `API_TEST_GUIDE.http`:
   - `POST /api/auth/register` (si nouvel utilisateur)
   - `POST /api/auth/login` et récupérer le **nouveau token**
   - Remplacer le token dur dans la requête `Authorization: Bearer ...` par le **token fraîchement obtenu**
   - Appeler un endpoint protégé (`GET /api/tasks`, `POST /api/tasks`)

### Bonnes pratiques

- Toujours utiliser un **token frais** (les tokens expirés/anciens provoquent `invalid_token`).
- Ne pas pin les paquets IdentityModel manuellement avec ASP.NET Core 9; laisser `JwtBearer` gérer les versions transitives.
- En Production, activer HTTPS et configurer précisément CORS.

### Dépannage rapide

- Si `invalid_token` persiste:
  - Vérifier `Issuer`, `Audience`, et la **clé** dans `appsettings(.Development).json`.
  - Regénérer le token via `/api/auth/login`.
  - Consulter la console: les logs `OnAuthenticationFailed` / `OnChallenge` indiquent la cause exacte.

### API

- ✅ RESTful conventions
- ✅ Proper HTTP status codes
- ✅ CORS support
- ✅ Swagger/OpenAPI documentation

### Sécurité

- ✅ Hachage sécurisé des passwords
- ✅ JWT avec signature HS256
- ✅ Validation des entrées
- ✅ Isolation des données par utilisateur

---

## 🤝 Contribuer

Les contributions sont bienvenues ! Pour contribuer :

1. Fork le projet
2. Créez une branche (`git checkout -b feature/AmazingFeature`)
3. Commitez vos changements (`git commit -m 'Add some AmazingFeature'`)
4. Pushez à la branche (`git push origin feature/AmazingFeature`)
5. Ouvrez une Pull Request

---

## 📄 Licence

Ce projet est sous licence MIT. Voir le fichier LICENSE pour plus de détails.

---

## 📞 Support

Pour toute question ou problème :

1. Consultez la [documentation API complète](./Docs/API_DOCUMENTATION.md)
2. Vérifiez la section [Troubleshooting](#-troubleshooting)
3. Ouvrez une issue sur GitHub

---
