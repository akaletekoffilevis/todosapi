# Guide de Contribution

Merci de contribuer à Todos API! Ce document fournit les directives pour contribuer au projet.

## 🐛 Signaler un Bug

Pour signaler un bug, veuillez:

1. Vérifier que le bug n'a pas déjà été rapporté dans les [Issues](../../issues)
2. Ouvrir une nouvelle issue avec un titre descriptif
3. Fournir:
   - Une description claire du problème
   - Les étapes pour reproduire
   - Le comportement attendu vs observé
   - La version de .NET utilisée
   - Les logs d'erreur pertinents

## 💡 Proposer une Amélioration

Consultez le fichier [Docs/FUTURE_IMPROVEMENTS.md](Docs/FUTURE_IMPROVEMENTS.md) pour les améliorations planifiées.

Pour proposer une nouvelle fonctionnalité:

1. Créer une issue avec le label `enhancement`
2. Décrire clairement la fonctionnalité proposée
3. Fournir des cas d'usage et des exemples

## 🔧 Processus de Développement

### Prérequis

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Visual Studio Code ou Visual Studio 2022
- Git

### Installation locale

```bash
git clone https://github.com/yourusername/TodosApi.git
cd TodosApi
dotnet restore
dotnet run --project TodosApi.csproj
```

### Structure du Projet

```
Controllers/       # Endpoints API
Data/             # Modèles et DbContext
Services/         # Logique métier
Docs/             # Documentation
Tests/            # Tests unitaires (à implémenter)
```

### Standards de Code

- Utiliser des noms de variables explicites
- Commenter le code complexe
- Suivre les conventions C# (PascalCase pour les classes/méthodes)
- Valider les entrées utilisateur

### Commits

- Utiliser des messages clairs et descriptifs
- Exemple: `fix: correction du hachage de mot de passe`
- Référencer les issues: `fix: #123 - description`

## ✅ Checklist avant Pull Request

- [ ] Le code est testé
- [ ] La documentation est mise à jour
- [ ] Pas de hardcoding ou de secrets
- [ ] Les conventions de code sont respectées
- [ ] Les messages de commit sont descriptifs

## 📋 Pull Request

1. Fork le projet
2. Créer une branche (`git checkout -b feature/AmazingFeature`)
3. Commit les changements (`git commit -m 'Add AmazingFeature'`)
4. Push vers la branche (`git push origin feature/AmazingFeature`)
5. Ouvrir une Pull Request

## 📜 Licence

En contribuant, vous acceptez que vos contributions soient licencées sous la [MIT License](../LICENSE).

---

**Questions?** Ouvrez une issue ou consultez la [documentation API](Docs/API_DOCUMENTATION.md).
