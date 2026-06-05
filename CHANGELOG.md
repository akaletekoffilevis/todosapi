# Changelog

Tous les changements notables de ce projet sont documentés dans ce fichier.

Le format est basé sur [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
et le projet adhère à [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2025-01-19

### ✨ Ajouté

- **Authentification JWT**
  - Inscription utilisateur (`POST /api/auth/register`)
  - Connexion avec JWT Bearer token (`POST /api/auth/login`)
  - Token valide 1 heure
  - Hachage PBKDF2 avec SHA256 (100,000 itérations)

- **Gestion des Tâches**
  - Récupérer les tâches de l'utilisateur (`GET /api/tasks`)
  - Créer une tâche (`POST /api/tasks`)
  - Modifier une tâche (`PUT /api/tasks/{id}`)
  - Marquer une tâche comme complétée (`PATCH /api/tasks/{id}/complete`)
  - Supprimer une tâche (`DELETE /api/tasks/{id}`)

- **Sécurité**
  - Isolation des données par utilisateur
  - Validation des données côté serveur
  - Protection JWT Bearer sur tous les endpoints des tâches

- **Documentation & Tests**
  - Swagger UI intégré
  - Fichiers HTTP pour VS Code REST Client
  - Documentation complète en Markdown

- **Technologies**
  - ASP.NET Core 9.0
  - Entity Framework Core 9.0
  - SQLite
  - JWT Bearer 9.0.0
  - Swagger/OpenAPI 6.5.0

### 🐛 Corrigé

- N/A (première version stable)

### 🗑️ Supprimé

- N/A

---

## Améliorations Prévues

Voir [FUTURE_IMPROVEMENTS.md](Docs/FUTURE_IMPROVEMENTS.md) pour les fonctionnalités planifiées:

- [ ] Refresh tokens
- [ ] Pagination des tâches
- [ ] Filtrage avancé
- [ ] Catégories/Tags
- [ ] Partage de tâches
- [ ] Notifications
- [ ] Tests unitaires (xUnit)
- [ ] Logging avancé (Serilog)
- [ ] Rate limiting
- [ ] Cache (Redis)

---

[1.0.0]: https://github.com/yourusername/todosapi/releases/tag/v1.0.0
