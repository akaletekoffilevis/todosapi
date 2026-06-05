# 🚀 Démarrage Rapide - Todos API

## ⚡ En 5 Minutes

### 1️⃣ Démarrer l'API

```bash
cd TodosApi
dotnet run
```

Vous verrez:

```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5252
      Now listening on: https://localhost:5001
```

✅ L'API est prête !

### 2️⃣ Accéder à Swagger

Ouvrez votre navigateur: **<http://localhost:5252/swagger>**

### 3️⃣ S'inscrire

```bash
curl -X POST http://localhost:5252/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"username":"testuser","password":"Test@1234"}'
```

Response:

```json
{"id":1,"username":"testuser","message":"User registered successfully"}
```

### 4️⃣ Se connecter

```bash
curl -X POST http://localhost:5252/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"testuser","password":"Test@1234"}'
```

Response:

```json
{
  "token":"eyJhbGciOiJIUzI1NiIs...",
  "user":{"id":1,"username":"testuser"},
  "message":"Login successful"
}
```

Sauvegardez le `token` !

### 5️⃣ Créer une tâche

```bash
TOKEN="votre_token_ici"

curl -X POST http://localhost:5252/api/tasks \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{"title":"Ma première tâche","description":"Test"}'
```

### 6️⃣ Voir les tâches

```bash
curl http://localhost:5252/api/tasks \
  -H "Authorization: Bearer $TOKEN"
```

Response:

```json
[
  {
    "id":1,
    "title":"Ma première tâche",
    "description":"Test",
    "isCompleted":false,
    "createdAt":"2026-01-16T...",
    "userId":1
  }
]
```

---

## 🎯 Endpoints Principaux

| Méthode | Endpoint | Description | Auth |
|---------|----------|-------------|------|
| POST | /api/auth/register | Inscription | ❌ |
| POST | /api/auth/login | Connexion | ❌ |
| GET | /api/tasks | Mes tâches | ✅ |
| POST | /api/tasks | Créer tâche | ✅ |
| PUT | /api/tasks/{id} | Modifier | ✅ |
| DELETE | /api/tasks/{id} | Supprimer | ✅ |
| PATCH | /api/tasks/{id}/complete | Marquer complétée | ✅ |

---

## 🔑 Format du Token

Copiez le token après login et utilisez-le comme:

```
Authorization: Bearer <token_ici>
```

Le token expire après **1 heure**.

---

## 💾 Données Sauvegardées

Les données sont stockées dans: `Data/todos.db`

Pour réinitialiser:

```bash
rm Data/todos.db
dotnet run
```

---

## 🐛 Erreurs Courantes

### "Port already in use"

```bash
# Arrêtez l'API sur un autre terminal
# Ou changez le port dans Properties/launchSettings.json
```

### "Tâche non trouvée (404)"

- Vérifiez l'ID
- Assurez-vous que la tâche est à vous

### "Token expiré"

- Reconnectez-vous pour obtenir un nouveau token

### "Authentification échouée (401)"

- Vérifiez que vous avez mis le header `Authorization: Bearer <token>`
- Vérifiez que le token n'a pas expiré

---

## 🧪 Test Rapide avec Swagger

1. Ouvrez <http://localhost:5252/swagger>
2. Cliquez sur **POST /api/auth/register**
3. Cliquez sur **"Try it out"**
4. Entrez un username et password
5. Cliquez sur **"Execute"**
6. Récupérez le token de la réponse
7. Cliquez sur le bouton **"Authorize"** en haut
8. Entrez: `Bearer <votre_token>`
9. Testez les autres endpoints !

---

**Besoin d'aide ?** Consultez API_DOCUMENTATION.md
