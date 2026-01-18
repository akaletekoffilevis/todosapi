# Configuration de Production

Ce fichier contient les directives pour déployer l'API en production de manière sécurisée.

## 🔐 Sécurité en Production

### 1. Clé JWT

**JAMAIS** utilisez la clé par défaut en production !

```bash
# Générer une clé sécurisée de 256 bits
$bytes = [System.Security.Cryptography.RandomNumberGenerator]::GetBytes(32)
$key = [Convert]::ToBase64String($bytes)
Write-Host "Clé JWT: $key"
```

Ensuite, configurez-la :

```bash
# Via User Secrets (développement)
dotnet user-secrets set "Jwt:Key" "votre-clé-sécurisée"

# Via Environment Variables (production)
$env:Jwt__Key = "votre-clé-sécurisée"

# Via appsettings.Production.json
{
  "Jwt": {
    "Key": "votre-clé-sécurisée"
  }
}
```

### 2. Connection String

**En production, utilisez SQL Server** au lieu de SQLite :

```json
{
  "ConnectionStrings": {
    "SqliteDbConnection": "Server=tcp:your-server.database.windows.net,1433;Initial Catalog=TodosDb;Persist Security Info=False;User ID=sa;Password=YourPassword;Encrypt=True;Connection Timeout=30;"
  }
}
```

### 3. Certificats HTTPS

1. **Générer un certificat SSL**

   ```bash
   # Avec Let's Encrypt (recommandé)
   certbot certonly --standalone -d yourdomain.com
   
   # Ou auto-signé (développement seulement)
   dotnet dev-certs https --trust
   ```

2. **Configurer HTTPS** dans Program.cs

   ```csharp
   var cert = new X509Certificate2("path/to/cert.pfx", "password");
   builder.WebHost.UseKestrel(options =>
   {
       options.ListenAnyIP(443, listenOptions =>
       {
           listenOptions.UseHttps(cert);
       });
   });
   ```

### 4. CORS Configuré

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("ProductionPolicy", builder =>
    {
        builder.WithOrigins("https://yourdomain.com", "https://www.yourdomain.com")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});
```

### 5. Variables d'Environnement

```bash
# Linux / macOS / PowerShell
export ASPNETCORE_ENVIRONMENT=Production
export ASPNETCORE_URLS=https://0.0.0.0:443
export ASPNETCORE_HTTPS_PORT=443
export Jwt__Key=votre-clé-sécurisée
export ConnectionStrings__SqliteDbConnection=connexion-bd

# Windows CMD
set ASPNETCORE_ENVIRONMENT=Production
set Jwt__Key=votre-clé-sécurisée
```

---

## 📦 Déploiement

### Option 1: Azure App Service

1. **Créer une App Service**

   ```bash
   az appservice plan create --resource-group rg-todos --name plan-todos --sku B1
   az webapp create --resource-group rg-todos --plan plan-todos --name api-todos
   ```

2. **Publier l'application**

   ```bash
   dotnet publish -c Release -o ./publish
   az webapp deployment source config-zip --resource-group rg-todos --name api-todos --src-path ./publish.zip
   ```

3. **Configurer les App Settings**

   ```bash
   az webapp config appsettings set \
     --resource-group rg-todos \
     --name api-todos \
     --settings ASPNETCORE_ENVIRONMENT=Production \
     Jwt__Key="votre-clé" \
     ConnectionStrings__SqliteDbConnection="connexion-bd"
   ```

### Option 2: Docker

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 443
ENV ASPNETCORE_URLS=https://+:443
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "TodosApi.dll"]
```

### Option 3: Self-Hosted (Linux/Windows)

1. **Publier en Release**

   ```bash
   dotnet publish -c Release -o ./publish
   ```

2. **Transférer les fichiers**

   ```bash
   scp -r ./publish user@server:/var/www/todos-api
   ```

3. **Créer un service systemd** (Linux)

   ```ini
   [Unit]
   Description=Todos API
   After=network.target

   [Service]
   Type=notify
   ExecStart=/usr/bin/dotnet /var/www/todos-api/TodosApi.dll
   WorkingDirectory=/var/www/todos-api
   Environment="ASPNETCORE_ENVIRONMENT=Production"
   Environment="Jwt__Key=votre-clé"
   Restart=on-failure
   RestartSec=10
   SyslogIdentifier=todos-api
   User=www-data

   [Install]
   WantedBy=multi-user.target
   ```

4. **Activer le service**

   ```bash
   sudo systemctl enable todos-api
   sudo systemctl start todos-api
   ```

---

## 🔍 Monitoring & Logging

### 1. Logging Structuré

```csharp
builder.Services.AddLogging(config =>
{
    config.ClearProviders();
    config.AddConsole();
    config.AddFile("logs/api-{Date}.txt");
});
```

### 2. Application Insights (Azure)

```csharp
builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPINSIGHTS_CONNECTIONSTRING"]);
```

### 3. Health Checks

```csharp
builder.Services.AddHealthChecks()
    .AddDbContextCheck<TodoDbContext>();

app.MapHealthChecks("/health");
```

---

## 🧪 Tests en Production

### Health Check

```bash
curl https://api.yourdomain.com/health
```

### Swagger (optionnel en prod)

```bash
# Désactiver en production
if (!app.Environment.IsProduction()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

### Tests d'API

```bash
# S'inscrire
curl -X POST https://api.yourdomain.com/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"username": "testuser", "password": "TestPass123"}'

# Se connecter
curl -X POST https://api.yourdomain.com/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username": "testuser", "password": "TestPass123"}'
```

---

## 📊 Checklist de Production

### Avant Déploiement

- [ ] Clé JWT changée
- [ ] Connection string configurée pour le SGBD en production
- [ ] HTTPS/SSL configuré
- [ ] CORS restreint aux domaines autorisés
- [ ] Logs configurés
- [ ] Environment = Production
- [ ] Swagger désactivé en production
- [ ] Health checks mis en place
- [ ] Backups de BD configurés
- [ ] Monitoring/Alerting actif

### Après Déploiement

- [ ] Tester les endpoints en production
- [ ] Vérifier les logs
- [ ] Monitorer la performance
- [ ] Vérifier la sécurité (SSL, CORS)
- [ ] Planifier les sauvegardes
- [ ] Documenter la configuration

---

## 🚨 Gestion des Erreurs

### Configuration de production

```csharp
if (!app.Environment.IsDevelopment())
{
    // Ne pas exposer les détails d'erreur
    app.UseExceptionHandler("/api/error");
    app.UseHsts(); // HSTS header
}
```

### Réponse d'erreur sécurisée

```csharp
// NE PAS faire en production
{
  "error": "Exception complète avec stack trace"
}

// FAIRE en production
{
  "message": "Une erreur est survenue. ID erreur: xyz123"
}
```

---

## 🔄 CI/CD Pipeline

### Avec GitHub Actions

```yaml
name: Build and Deploy

on:
  push:
    branches: [ main ]

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v2
      
      - name: Setup .NET
        uses: actions/setup-dotnet@v1
        with:
          dotnet-version: 9.0.x
      
      - name: Build
        run: dotnet build -c Release
      
      - name: Publish
        run: dotnet publish -c Release -o ./publish
      
      - name: Deploy to Azure
        uses: azure/webapps-deploy@v2
        with:
          app-name: api-todos
          package: ./publish
          publish-profile: ${{ secrets.AZURE_PUBLISH_PROFILE }}
```

---

## 🆘 Troubleshooting Production

### L'API ne démarre pas

```bash
# Vérifier les logs
journalctl -u todos-api -n 50

# Vérifier le port
sudo netstat -tlnp | grep 443

# Vérifier les permissions
sudo chown -R www-data /var/www/todos-api
```

### Erreurs de connexion à la BD

```bash
# Tester la connexion
telnet your-server.database.windows.net 1433

# Vérifier les credentials
echo "Server=...;Password=..." | sqlcmd -S your-server.database.windows.net
```

### Certificat HTTPS expiré

```bash
# Renouveler Let's Encrypt
sudo certbot renew --force-renewal
```

---

## 📈 Performance & Optimisation

### Caching

```csharp
builder.Services.AddMemoryCache();
builder.Services.AddResponseCaching();
app.UseResponseCaching();
```

### Compression

```csharp
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});
app.UseResponseCompression();
```

### Connection Pooling

```json
{
  "ConnectionStrings": {
    "SqliteDbConnection": "... Max Pool Size=50;"
  }
}
```

---

