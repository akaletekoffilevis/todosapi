# 🧪 Quick Test Instructions

## Démarrage Rapide - 2 Minutes

### ✅ Étape 1: Démarrer l'API
```bash
cd TodosApi
dotnet run
```

**Attendez ce message**:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5252
```

### ✅ Étape 2: Ouvrir VS Code REST Client
1. Ouvrez le fichier `TodosApi.http`
2. Cliquez "Send Request" sur **Register a new user**
3. Vérifiez la réponse (201 Created)

### ✅ Étape 3: Récupérer le Token
1. Cliquez "Send Request" sur **Login and get JWT token**
2. Copie automatique du token dans `@token`

### ✅ Étape 4: Tester les Endpoints
Cliquez "Send Request" sur chaque endpoint:
- ✅ Get all tasks
- ✅ Create a new task
- ✅ Update a task
- ✅ Mark task as completed
- ✅ Delete a task

---

## Test via Postman

1. Importer le fichier `TodosApi.http`
2. Configurer la variable `token` avec le token du login
3. Tester chaque endpoint

---

## Test via cURL (Terminal)

```bash
# 1. S'inscrire
curl -X POST http://localhost:5252/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"username":"testuser","password":"Test@1234"}'

# 2. Se connecter (copier le token)
TOKEN=$(curl -s -X POST http://localhost:5252/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"testuser","password":"Test@1234"}' | jq -r '.token')

echo "Token: $TOKEN"

# 3. Créer une tâche
curl -X POST http://localhost:5252/api/tasks \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{"title":"Test Task","description":"Testing API"}'

# 4. Récupérer les tâches
curl http://localhost:5252/api/tasks \
  -H "Authorization: Bearer $TOKEN"
```

---

## ✅ Validation

Tous les tests doivent retourner:
- ✅ Register: `201 Created`
- ✅ Login: `200 OK` avec token
- ✅ Get tasks: `200 OK` avec liste
- ✅ Create task: `201 Created`
- ✅ Update task: `200 OK`
- ✅ Mark complete: `200 OK`
- ✅ Delete task: `204 No Content`

---

## 🐛 Troubleshooting

| Erreur | Solution |
|--------|----------|
| Port 5252 déjà utilisé | Utilisez un autre port dans launchSettings.json |
| invalid_token | Utilisez un token frais du login |
| User already exists | Utilisez un autre username |
| Password too short | Minimum 8 caractères |

---

**C'est prêt! Go! 🚀**
