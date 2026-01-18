# API de Gestion des Tâches Sécurisée

Une API REST sécurisée pour la gestion d'une liste de tâches avec authentification JWT.

## 📋 Table des matières

- [Authentification](#authentification)
- [Endpoints de Tâches](#endpoints-de-tâches)
- [Exemples d'Utilisation](#exemples-dutilisation)
- [Codes de Réponse](#codes-de-réponse)
- [Sécurité](#sécurité)

---

## Authentification

### 1. Inscription - `POST /api/auth/register`

Crée un nouveau compte utilisateur.

**Requête :**

```json
{
  "username": "john_doe",
  "password": "SecurePassword123"
}
```

**Réponse (201 Created) :**

```json
{
  "id": 1,
  "username": "john_doe",
  "message": "User registered successfully"
}
```

**Validations :**

- `username` : requis, 3-100 caractères
- `password` : requis, minimum 8 caractères

---

### 2. Connexion - `POST /api/auth/login`

Authentifie un utilisateur et retourne un token JWT valide pendant 1 heure.

**Requête :**

```json
{
  "username": "john_doe",
  "password": "SecurePassword123"
}
```

**Réponse (200 OK) :**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "username": "john_doe"
  },
  "message": "Login successful"
}
```

**Erreurs possibles :**

- `400 Bad Request` : Validations échouées
- `401 Unauthorized` : Identifiants invalides

---

## Endpoints de Tâches

> ⚠️ **IMPORTANT** : Tous les endpoints de tâches nécessitent une authentification JWT.
> Incluez le header : `Authorization: Bearer <token>`

### 1. Récupérer les tâches de l'utilisateur - `GET /api/tasks`

Retourne toutes les tâches de l'utilisateur connecté.

**Réponse (200 OK) :**

```json
[
  {
    "id": 1,
    "title": "Faire les courses",
    "description": "Lait, œufs, pain",
    "isCompleted": false,
    "createdAt": "2026-01-16T10:30:00Z",
    "userId": 1
  },
  {
    "id": 2,
    "title": "Appeler le client",
    "description": "Discuter du projet",
    "isCompleted": true,
    "createdAt": "2026-01-15T14:45:00Z",
    "userId": 1
  }
]
```

---

### 2. Récupérer une tâche spécifique - `GET /api/tasks/{id}`

Retourne une tâche spécifique (seulement si elle appartient à l'utilisateur connecté).

**Réponse (200 OK) :**

```json
{
  "id": 1,
  "title": "Faire les courses",
  "description": "Lait, œufs, pain",
  "isCompleted": false,
  "createdAt": "2026-01-16T10:30:00Z",
  "userId": 1
}
```

**Erreurs possibles :**

- `404 Not Found` : Tâche non trouvée ou n'appartient pas à l'utilisateur

---

### 3. Créer une tâche - `POST /api/tasks`

Crée une nouvelle tâche pour l'utilisateur connecté.

**Requête :**

```json
{
  "title": "Faire les courses",
  "description": "Lait, œufs, pain"
}
```

**Réponse (201 Created) :**

```json
{
  "id": 1,
  "title": "Faire les courses",
  "description": "Lait, œufs, pain",
  "isCompleted": false,
  "createdAt": "2026-01-16T10:30:00Z",
  "userId": 1
}
```

**Validations :**

- `title` : requis, 1-255 caractères
- `description` : optionnel, maximum 2000 caractères

---

### 4. Modifier une tâche - `PUT /api/tasks/{id}`

Modifie une tâche existante.

**Requête :**

```json
{
  "title": "Faire les courses (IMPORTANT)",
  "description": "Lait, œufs, pain, fromage",
  "isCompleted": false
}
```

**Réponse (204 No Content)**

**Erreurs possibles :**

- `404 Not Found` : Tâche non trouvée ou n'appartient pas à l'utilisateur

---

### 5. Marquer une tâche comme complétée - `PATCH /api/tasks/{id}/complete`

Marque une tâche comme complétée ou incomplétée.

**Requête :**

```
PATCH /api/tasks/1/complete?value=true
```

**Réponse (204 No Content)**

**Paramètres :**

- `value` (query) : `true` pour marquer comme complétée, `false` pour incomplétée (défaut : true)

---

### 6. Supprimer une tâche - `DELETE /api/tasks/{id}`

Supprime une tâche.

**Réponse (204 No Content)**

**Erreurs possibles :**

- `404 Not Found` : Tâche non trouvée ou n'appartient pas à l'utilisateur

---

## Exemples d'Utilisation

### Avec cURL

#### 1. S'inscrire

```bash
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "john_doe",
    "password": "SecurePassword123"
  }'
```

#### 2. Se connecter

```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "john_doe",
    "password": "SecurePassword123"
  }'
```

#### 3. Créer une tâche

```bash
TOKEN="votre_token_jwt"
curl -X POST http://localhost:5000/api/tasks \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "title": "Faire les courses",
    "description": "Lait, œufs, pain"
  }'
```

#### 4. Récupérer les tâches

```bash
curl -X GET http://localhost:5000/api/tasks \
  -H "Authorization: Bearer $TOKEN"
```

#### 5. Modifier une tâche

```bash
curl -X PUT http://localhost:5000/api/tasks/1 \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "title": "Faire les courses (URGENT)",
    "description": "Lait, œufs, pain",
    "isCompleted": false
  }'
```

#### 6. Marquer comme complétée

```bash
curl -X PATCH "http://localhost:5000/api/tasks/1/complete?value=true" \
  -H "Authorization: Bearer $TOKEN"
```

#### 7. Supprimer une tâche

```bash
curl -X DELETE http://localhost:5000/api/tasks/1 \
  -H "Authorization: Bearer $TOKEN"
```

---

### Avec Postman

1. **Créer une variable d'environnement** :
   - `token` : (laisser vide au départ)
   - `baseUrl` : `http://localhost:5000`

2. **Endpoint de Login** :
   - Ajouter un script de test pour extraire et sauvegarder le token :

   ```javascript
   if (pm.response.code === 200) {
     var jsonData = pm.response.json();
     pm.environment.set("token", jsonData.token);
   }
   ```

3. **Utiliser le token** :
   - Dans les headers, ajouter : `Authorization: Bearer {{token}}`

---

## Codes de Réponse

| Code | Description |
|------|-------------|
| `200 OK` | Requête réussie |
| `201 Created` | Ressource créée avec succès |
| `204 No Content` | Opération réussie (pas de contenu à retourner) |
| `400 Bad Request` | Erreur de validation des données |
| `401 Unauthorized` | Authentification manquante ou invalide |
| `404 Not Found` | Ressource non trouvée |
| `409 Conflict` | L'utilisateur existe déjà |
| `500 Internal Server Error` | Erreur serveur |

---

## Sécurité

### Authentification JWT

- **Durée de validité** : 1 heure (configurable dans `appsettings.json`)
- **Algorithme** : HS256 (HMAC with SHA256)
- **Claims** :
  - `NameIdentifier` : ID utilisateur
  - `Name` : Nom d'utilisateur
  - `Jti` : JWT ID unique

### Hachage des mots de passe

- **Algorithme** : PBKDF2 avec SHA256
- **Itérations** : 100,000
- **Longueur du sel** : 16 bytes
- **Longueur du hash** : 32 bytes

### Isolation des données

- Chaque utilisateur ne peut accéder qu'à ses propres tâches
- Les IDs sont vérifiés à partir du token JWT

### Bonnes pratiques

1. **Ne jamais exposer le JWT en URL** - Utilisez toujours le header `Authorization`
2. **Changez la clé secrète en production** - Modifiez `Jwt:Key` dans `appsettings.json`
3. **Utilisez HTTPS en production** - Les tokens JWT doivent être transmis via HTTPS
4. **Gérez les expirations** - Le token expire après 1 heure
5. **Validez toujours les entrées** - Data Annotations valident les données côté serveur

---

## Configuration

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

**⚠️ En production** :

- Changez la clé `Jwt:Key` par une clé sécurisée d'au moins 32 caractères
- Utilisez des variables d'environnement pour les secrets
- Activez HTTPS

---

## Démarrage rapide

1. **Construire le projet**

   ```bash
   dotnet build
   ```

2. **Exécuter l'application**

   ```bash
   dotnet run
   ```

3. **Accéder à Swagger** : <http://localhost:5000/swagger>

---

## Troubleshooting

### Token invalide ou expiré

- Reconnectez-vous pour obtenir un nouveau token

### L'utilisateur existe déjà

- Utilisez un nom d'utilisateur différent

### Tâche non trouvée (404)

- Vérifiez que la tâche appartient à votre utilisateur
- Assurez-vous d'avoir utilisé le bon ID

### Authentification échouée (401)

- Vérifiez que vous avez fourni le header `Authorization: Bearer <token>`
- Vérifiez que le token n'a pas expiré

---

**Version API** : 1.0  
**Dernière mise à jour** : 16 janvier 2026
