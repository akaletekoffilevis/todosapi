# Migration de SQLite vers SQL Server

Ce guide explique comment migrer votre TodosApi de **SQLite (local)** vers **SQL Server (serveur)**.

---

## 📋 Pourquoi migrer vers SQL Server ?

| Feature | SQLite | SQL Server |
|---------|--------|-----------|
| **Persistance** | Fichier local | Base de données serveur |
| **Concurrence** | Limitée | Excellente |
| **Performance** | Petites données | Moyennes à grandes données |
| **Production** | ⚠️ Non recommandé | ✅ Recommandé |
| **Scalabilité** | Fichier unique | Illimitée |
| **Sauvegarde** | Manuelle | Automatique |

---

## 🚀 Étapes de Migration

### **Étape 1 : Installer le package SQL Server**

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

---

### **Étape 2 : Mettre à jour `appsettings.json`**

Remplace la connexion SQLite par SQL Server :

**Avant (SQLite) :**
```json
{
  "ConnectionStrings": {
    "SqliteDbConnection": "Data Source=Data/todos.db"
  }
}
```

**Après (SQL Server) :**
```json
{
  "ConnectionStrings": {
    "SqlServerConnection": "Server=localhost;Database=TodosDb;User Id=sa;Password=YourPassword123!;Encrypt=false;TrustServerCertificate=true;"
  }
}
```

**Explication des paramètres :**
- `Server=localhost` : SQL Server sur la machine locale
- `Database=TodosDb` : Nom de la base de données
- `User Id=sa` : Administrateur SQL Server
- `Password=YourPassword123!` : Mot de passe (voir Docker)
- `Encrypt=false` : Non chiffré pour développement
- `TrustServerCertificate=true` : Accepte les certificats auto-signés

---

### **Étape 3 : Mettre à jour `Program.cs`**

Remplace `UseSqlite()` par `UseSqlServer()` :

**Avant :**
```csharp
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteDbConnection")));
```

**Après :**
```csharp
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection")));
```

---

### **Étape 4 : Mettre à jour le code d'initialisation BD**

Simplifie l'initialisation (plus besoin de créer le dossier Data) :

**Avant :**
```csharp
try
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
        Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, "Data"));
        db.Database.EnsureCreated();
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error creating database: {ex.Message}");
}
```

**Après :**
```csharp
try
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
        // Apply pending migrations (won't recreate if DB already exists)
        db.Database.Migrate();
        Console.WriteLine("✅ Database migrated successfully");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Error migrating database: {ex.Message}");
    throw;
}
```

---

## 🐳 Lancer SQL Server avec Docker

### **Option 1 : Docker Desktop (Facile)**

```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourPassword123!" `
  -p 1433:1433 `
  --name sqlserver `
  -d mcr.microsoft.com/mssql/server:2022-latest
```

**Paramètres :**
- `-e "ACCEPT_EULA=Y"` : Accepter la licence
- `-e "SA_PASSWORD=YourPassword123!"` : Mot de passe admin (min 8 chars, complexe)
- `-p 1433:1433` : Port SQL Server
- `--name sqlserver` : Nom du conteneur
- `-d` : Exécuter en arrière-plan

### **Option 2 : Docker Compose**

Crée `docker-compose.yml` à la racine du projet :

```yaml
version: '3.8'
services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      ACCEPT_EULA: "Y"
      SA_PASSWORD: "YourPassword123!"
    ports:
      - "1433:1433"
    volumes:
      - sqlserver_data:/var/opt/mssql
    
volumes:
  sqlserver_data:
```

Puis lance :
```bash
docker-compose up -d
```

---

## 🔄 Créer les Migrations

### **Créer la migration initiale :**
```bash
dotnet ef migrations add InitialCreate
```

### **Voir les migrations créées :**
```bash
ls Migrations/
```

Tu devrais voir :
```
Migrations/
├── 20240119120000_InitialCreate.cs
├── 20240119120000_InitialCreate.Designer.cs
└── MigrationsDbContextModelSnapshot.cs
```

### **Appliquer la migration :**
```bash
dotnet run
```

La migration s'applique automatiquement au démarrage grâce à `db.Database.Migrate()`.

---

## ✅ Tester la Migration

### **1. Vérifier la connexion SQL Server :**

Ouvre SQL Server Management Studio (SSMS) ou Azure Data Studio :

```
Serveur: localhost,1433
Authentification: SQL Server Authentication
Login: sa
Password: YourPassword123!
```

### **2. Vérifie la base de données créée :**

```sql
SELECT name FROM sys.databases WHERE name = 'TodosDb';
```

### **3. Teste l'API :**

```bash
# Register
curl -X POST http://localhost:5252/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"username":"testuser","password":"Test@1234"}'

# Login
curl -X POST http://localhost:5252/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"testuser","password":"Test@1234"}'

# Get todos (avec token du login)
curl -X GET http://localhost:5252/api/todos \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

---

## 🐛 Dépannage

### **Erreur : "Connection timeout"**
```
Solution: Vérifie que Docker est lancé et le port 1433 est libre
docker ps | grep sqlserver
```

### **Erreur : "Login failed for user 'sa'"**
```
Solution: Le mot de passe doit être complexe (maj, min, chiffre, spécial)
Minimum 8 caractères
```

### **Erreur : "Cannot find type 'UseSqlServer'"**
```
Solution: Installe le package manquant
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

### **Réinitialiser la base de données (DEV ONLY) :**

```bash
# Supprimer la dernière migration
dotnet ef migrations remove

# Supprimer la base de données manuellement dans SSMS
DROP DATABASE TodosDb;

# Créer une nouvelle migration
dotnet ef migrations add InitialCreate

# Relancer l'API
dotnet run
```

---

## 🔐 Production - Bonnes Pratiques

### **1. Utiliser des variables d'environnement :**

```csharp
// Au lieu de hardcoder le mot de passe
var password = Environment.GetEnvironmentVariable("SQL_PASSWORD");
var connectionString = $"Server=prod-server.com;Database=TodosDb;User Id=sa;Password={password};";
```

### **2. Sauvegardes automatiques :**

SQL Server gère les sauvegardes. Configure-les dans :
- SQL Server Management Studio → Properties → Backup
- Ou via Azure SQL Database (cloud)

### **3. Connection string sécurisée :**

```json
{
  "ConnectionStrings": {
    "SqlServerConnection": "Server=prod-db.database.windows.net,1433;Database=TodosDb;User Id=dbadmin@prodserver;Password={password};Encrypt=true;Connection Timeout=30;"
  }
}
```

---

## 📊 Migration de Données Existantes

Si tu as déjà des données dans SQLite :

### **Exporter depuis SQLite :**
```bash
# Exporter en CSV
sqlite3 Data/todos.db ".mode csv" ".output todos_export.csv" "SELECT * FROM Todos;"
```

### **Importer dans SQL Server :**
```sql
-- Dans SQL Server Management Studio
BULK INSERT [TodosDb].[dbo].[Todos]
FROM 'C:\path\to\todos_export.csv'
WITH (
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    FIRSTROW = 2
);
```

Ou utilise un outil comme :
- **SQL Server Integration Services (SSIS)**
- **Azure Data Studio**
- **Redgate SQL Compare**

---

## ✨ Résumé

| Étape | Action |
|-------|--------|
| 1️⃣ | Installer package SQL Server |
| 2️⃣ | Mettre à jour `appsettings.json` |
| 3️⃣ | Mettre à jour `Program.cs` |
| 4️⃣ | Lancer SQL Server (Docker) |
| 5️⃣ | Créer migration : `dotnet ef migrations add InitialCreate` |
| 6️⃣ | Lancer l'API : `dotnet run` |
| 7️⃣ | Vérifier dans SSMS |

---

## 🚀 Prochaines Étapes

- ✅ Tester en développement
- ✅ Configurer CI/CD pour appliquer les migrations
- ✅ Monitorer les performances
- ✅ Configurer les alertes

**Besoin d'aide ? Consulte les logs :**
```bash
dotnet run --loglevel Debug
```
