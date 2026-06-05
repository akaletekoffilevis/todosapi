# 🧪 Guide de Test Complet - Todos API

Ce guide fournit tous les tests nécessaires pour valider le bon fonctionnement de l'API.

## 📋 Table des matières

1. [Tests d'Authentification](#tests-dauthentification)
2. [Tests de Gestion des Tâches](#tests-de-gestion-des-tâches)
3. [Tests de Sécurité](#tests-de-sécurité)
4. [Tests de Validation](#tests-de-validation)
5. [Scripts de Test Automatisés](#scripts-de-test-automatisés)

---

## Tests d'Authentification

### Test 1: Inscription - Succès

**Endpoint**: `POST /api/auth/register`

**Requête**:

```bash
curl -X POST http://localhost:5252/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "john_doe",
    "password": "SecurePassword123"
  }'
```

**Résultat attendu**:

- Status: `201 Created`
- Body:

  ```json
  {
    "id": 1,
    "username": "john_doe",
    "message": "User registered successfully"
  }
  ```

---

### Test 2: Inscription - Username vide

**Requête**:

```bash
curl -X POST http://localhost:5252/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "",
    "password": "SecurePassword123"
  }'
```

**Résultat attendu**:

- Status: `400 Bad Request`
- Message d'erreur de validation

---

### Test 3: Inscription - Password trop court

**Requête**:

```bash
curl -X POST http://localhost:5252/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "john_doe",
    "password": "short"
  }'
```

**Résultat attendu**:

- Status: `400 Bad Request`
- Message: "Password must be at least 8 characters long"

---

### Test 4: Inscription - Username déjà pris

**Requête** (après Test 1):

```bash
curl -X POST http://localhost:5252/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "john_doe",
    "password": "AnotherPassword123"
  }'
```

**Résultat attendu**:

- Status: `409 Conflict`
- Message: "Username already taken"

---

### Test 5: Connexion - Succès

**Requête**:

```bash
curl -X POST http://localhost:5252/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "john_doe",
    "password": "SecurePassword123"
  }'
```

**Résultat attendu**:

- Status: `200 OK`
- Body:

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

**Action**: Sauvegardez le token pour les tests suivants

---

### Test 6: Connexion - Password incorrect

**Requête**:

```bash
curl -X POST http://localhost:5252/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "john_doe",
    "password": "WrongPassword123"
  }'
```

**Résultat attendu**:

- Status: `401 Unauthorized`
- Message: "Invalid credentials"

---

### Test 7: Connexion - Username inexistant

**Requête**:

```bash
curl -X POST http://localhost:5252/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "nonexistent",
    "password": "Password123"
  }'
```

**Résultat attendu**:

- Status: `401 Unauthorized`
- Message: "Invalid credentials"

---

## Tests de Gestion des Tâches

### Préalable: Obtenir un token valide

```bash
TOKEN=$(curl -s -X POST http://localhost:5252/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "john_doe",
    "password": "SecurePassword123"
  }' | grep -o '"token":"[^"]*"' | cut -d'"' -f4)

echo "Token: $TOKEN"
```

---

### Test 8: Créer une tâche - Succès

**Endpoint**: `POST /api/tasks`

**Requête**:

```bash
curl -X POST http://localhost:5252/api/tasks \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "title": "Faire les courses",
    "description": "Lait, œufs, pain"
  }'
```

**Résultat attendu**:

- Status: `201 Created`
- Body:

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

---

### Test 9: Créer une tâche - Sans authentification

**Requête** (sans Authorization header):

```bash
curl -X POST http://localhost:5252/api/tasks \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Une tâche",
    "description": "Description"
  }'
```

**Résultat attendu**:

- Status: `401 Unauthorized`

---

### Test 10: Créer une tâche - Titre manquant

**Requête**:

```bash
curl -X POST http://localhost:5252/api/tasks \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "title": "",
    "description": "Description sans titre"
  }'
```

**Résultat attendu**:

- Status: `400 Bad Request`
- Message de validation

---

### Test 11: Récupérer les tâches

**Endpoint**: `GET /api/tasks`

**Requête**:

```bash
curl -X GET http://localhost:5252/api/tasks \
  -H "Authorization: Bearer $TOKEN"
```

**Résultat attendu**:

- Status: `200 OK`
- Body: Array des tâches de l'utilisateur

  ```json
  [
    {
      "id": 1,
      "title": "Faire les courses",
      "description": "Lait, œufs, pain",
      "isCompleted": false,
      "createdAt": "2026-01-16T10:30:00Z",
      "userId": 1
    }
  ]
  ```

---

### Test 12: Récupérer une tâche spécifique

**Endpoint**: `GET /api/tasks/{id}`

**Requête**:

```bash
curl -X GET http://localhost:5252/api/tasks/1 \
  -H "Authorization: Bearer $TOKEN"
```

**Résultat attendu**:

- Status: `200 OK`
- Body: La tâche avec ID 1

---

### Test 13: Modifier une tâche - Succès

**Endpoint**: `PUT /api/tasks/{id}`

**Requête**:

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

**Résultat attendu**:

- Status: `204 No Content`

---

### Test 14: Modifier une tâche - Tâche inexistante

**Requête**:

```bash
curl -X PUT http://localhost:5252/api/tasks/999 \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "title": "Titre",
    "description": "Description",
    "isCompleted": false
  }'
```

**Résultat attendu**:

- Status: `404 Not Found`
- Message: "Task not found"

---

### Test 15: Marquer comme complétée

**Endpoint**: `PATCH /api/tasks/{id}/complete`

**Requête**:

```bash
curl -X PATCH "http://localhost:5252/api/tasks/1/complete?value=true" \
  -H "Authorization: Bearer $TOKEN"
```

**Résultat attendu**:

- Status: `204 No Content`

Vérification (GET /api/tasks/1):

```bash
curl -X GET http://localhost:5252/api/tasks/1 \
  -H "Authorization: Bearer $TOKEN"
```

Le champ `isCompleted` doit être `true`.

---

### Test 16: Marquer comme incomplétée

**Requête**:

```bash
curl -X PATCH "http://localhost:5252/api/tasks/1/complete?value=false" \
  -H "Authorization: Bearer $TOKEN"
```

**Résultat attendu**:

- Status: `204 No Content`
- `isCompleted` = `false` après vérification

---

### Test 17: Supprimer une tâche

**Endpoint**: `DELETE /api/tasks/{id}`

**Requête**:

```bash
curl -X DELETE http://localhost:5252/api/tasks/1 \
  -H "Authorization: Bearer $TOKEN"
```

**Résultat attendu**:

- Status: `204 No Content`

Vérification (GET /api/tasks/1):

```bash
curl -X GET http://localhost:5252/api/tasks/1 \
  -H "Authorization: Bearer $TOKEN"
```

Résultat attendu:

- Status: `404 Not Found`

---

### Test 18: Supprimer une tâche inexistante

**Requête**:

```bash
curl -X DELETE http://localhost:5252/api/tasks/999 \
  -H "Authorization: Bearer $TOKEN"
```

**Résultat attendu**:

- Status: `404 Not Found`
- Message: "Task not found"

---

## Tests de Sécurité

### Test 19: Token expiré

1. Attendez que le token expire (1 heure par défaut)
2. Essayez de faire une requête:

```bash
curl -X GET http://localhost:5252/api/tasks \
  -H "Authorization: Bearer $EXPIRED_TOKEN"
```

**Résultat attendu**:

- Status: `401 Unauthorized`

---

### Test 20: Token invalide

**Requête**:

```bash
curl -X GET http://localhost:5252/api/tasks \
  -H "Authorization: Bearer invalid_token_here"
```

**Résultat attendu**:

- Status: `401 Unauthorized`

---

### Test 21: Token d'un autre utilisateur

1. Créez 2 utilisateurs:

   ```bash
   # Utilisateur 1
   curl -X POST http://localhost:5252/api/auth/register \
     -H "Content-Type: application/json" \
     -d '{"username": "user1", "password": "Password1234"}'
   
   # Utilisateur 2
   curl -X POST http://localhost:5252/api/auth/register \
     -H "Content-Type: application/json" \
     -d '{"username": "user2", "password": "Password5678"}'
   ```

2. Créez une tâche avec Utilisateur 1
3. Essayez d'y accéder avec le token d'Utilisateur 2:

```bash
# Token d'Utilisateur 2
TOKEN_USER2=$(curl -s -X POST http://localhost:5252/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username": "user2", "password": "Password5678"}' | grep -o '"token":"[^"]*"' | cut -d'"' -f4)

# Tenter d'accéder à la tâche de l'Utilisateur 1 (ID 2)
curl -X GET http://localhost:5252/api/tasks/2 \
  -H "Authorization: Bearer $TOKEN_USER2"
```

**Résultat attendu**:

- Status: `404 Not Found` (la tâche ne doit pas être accessible)

---

### Test 22: Isolation des données

1. Utilisateur 1 crée 3 tâches
2. Utilisateur 2 crée 2 tâches
3. Chaque utilisateur récupère ses tâches:

**Utilisateur 1**:

```bash
curl -X GET http://localhost:5252/api/tasks \
  -H "Authorization: Bearer $TOKEN_USER1"
```

Doit retourner 3 tâches.

**Utilisateur 2**:

```bash
curl -X GET http://localhost:5252/api/tasks \
  -H "Authorization: Bearer $TOKEN_USER2"
```

Doit retourner 2 tâches.

---

## Tests de Validation

### Test 23: Username trop court

**Requête**:

```bash
curl -X POST http://localhost:5252/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "ab",
    "password": "SecurePass123"
  }'
```

**Résultat attendu**:

- Status: `400 Bad Request`
- Message: "Username must be between 3 and 100 characters"

---

### Test 24: Username trop long

**Requête**:

```bash
curl -X POST http://localhost:5252/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "'"$(printf 'a%.0s' {1..101})"'",
    "password": "SecurePass123"
  }'
```

**Résultat attendu**:

- Status: `400 Bad Request`

---

### Test 25: Description trop longue

**Requête**:

```bash
curl -X POST http://localhost:5252/api/tasks \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "title": "Titre",
    "description": "'"$(printf 'a%.0s' {1..2001})"'"
  }'
```

**Résultat attendu**:

- Status: `400 Bad Request`
- Message: "Description cannot exceed 2000 characters"

---

## Scripts de Test Automatisés

### Script PowerShell de Test Complet

```powershell
# test-api.ps1

$baseUrl = "http://localhost:5252"
$testResults = @()

function Test-Endpoint {
    param(
        [string]$Name,
        [string]$Method,
        [string]$Endpoint,
        [object]$Body,
        [string]$Token,
        [int]$ExpectedStatus
    )
    
    $headers = @{
        "Content-Type" = "application/json"
    }
    
    if ($Token) {
        $headers["Authorization"] = "Bearer $Token"
    }
    
    try {
        $response = Invoke-WebRequest `
            -Uri "$baseUrl$Endpoint" `
            -Method $Method `
            -Headers $headers `
            -Body ($Body | ConvertTo-Json) `
            -ErrorAction Stop
        
        $success = $response.StatusCode -eq $ExpectedStatus
    }
    catch {
        $response = $_.Exception.Response
        $success = $response.StatusCode -eq $ExpectedStatus
    }
    
    $result = @{
        Name = $Name
        Method = $Method
        Endpoint = $Endpoint
        Expected = $ExpectedStatus
        Actual = $response.StatusCode
        Passed = $success
    }
    
    return $result
}

Write-Host "🧪 Début des tests API..." -ForegroundColor Cyan

# Test 1: Inscription
$registerTest = Test-Endpoint `
    -Name "Registration" `
    -Method "POST" `
    -Endpoint "/api/auth/register" `
    -Body @{username="testuser"; password="TestPass123"} `
    -ExpectedStatus 201

$testResults += $registerTest
Write-Host "$(if ($registerTest.Passed) {'✅'} else {'❌'}) $($registerTest.Name)" -ForegroundColor $(if ($registerTest.Passed) {'Green'} else {'Red'})

# ... ajouter d'autres tests ...

# Résumé
$passed = ($testResults | Where-Object {$_.Passed}).Count
$total = $testResults.Count

Write-Host ""
Write-Host "════════════════════════════════════════" -ForegroundColor Cyan
Write-Host "Résultats: $passed/$total tests passés" -ForegroundColor $(if ($passed -eq $total) {'Green'} else {'Yellow'})
Write-Host "════════════════════════════════════════" -ForegroundColor Cyan
```

### Script Bash de Test Complet

```bash
#!/bin/bash

BASE_URL="http://localhost:5252"
PASSED=0
FAILED=0

# Couleurs
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
CYAN='\033[0;36m'
NC='\033[0m' # No Color

test_endpoint() {
    local name=$1
    local method=$2
    local endpoint=$3
    local data=$4
    local token=$5
    local expected=$6
    
    local cmd="curl -s -w '%{http_code}' -X $method $BASE_URL$endpoint"
    cmd="$cmd -H 'Content-Type: application/json'"
    
    if [ ! -z "$token" ]; then
        cmd="$cmd -H 'Authorization: Bearer $token'"
    fi
    
    if [ ! -z "$data" ]; then
        cmd="$cmd -d '$data'"
    fi
    
    local status=$(eval $cmd | tail -c 4)
    
    if [ "$status" == "$expected" ]; then
        echo -e "${GREEN}✅${NC} $name (Expected: $expected, Got: $status)"
        ((PASSED++))
    else
        echo -e "${RED}❌${NC} $name (Expected: $expected, Got: $status)"
        ((FAILED++))
    fi
}

echo -e "${CYAN}🧪 Début des tests API...${NC}\n"

# Tests
test_endpoint "Registration" "POST" "/api/auth/register" \
    '{"username":"testuser","password":"TestPass123"}' "" "201"

# ... ajouter d'autres tests ...

echo ""
echo -e "${CYAN}════════════════════════════════════════${NC}"
echo -e "Résultats: ${GREEN}$PASSED${NC} passés, ${RED}$FAILED${NC} échoués"
echo -e "${CYAN}════════════════════════════════════════${NC}"
```

---

## ✅ Checklist de Validation

- [ ] Test 1-7: Authentification
- [ ] Test 8-18: Gestion des tâches
- [ ] Test 19-22: Sécurité
- [ ] Test 23-25: Validation
- [ ] Scripts de test exécutés avec succès
- [ ] Tous les endpoints testés
- [ ] Codes de statut HTTP corrects
- [ ] Isolation des données confirmée
- [ ] Hachage sécurisé confirmé
- [ ] JWT validation confirmée

---


