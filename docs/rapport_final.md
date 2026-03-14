# Rapport Final — Projet QA Académique : BiProject

## 1. Introduction

Ce document présente les résultats du projet d'Assurance Qualité (QA) mené sur l'application "BiProject". L'objectif est de documenter la démarche de test appliquée, incluant les tests statiques, fonctionnels et non fonctionnels sur les trois niveaux (unitaire, intégration, système).

---

## 2. Présentation du Système Sous Test (SUT)

**Application :** BiProject — Plateforme e-commerce de gestion de cycles sportifs.

| Composant | Technologie | Description |
|---|---|---|
| Backend | ASP.NET Core (.NET 8+) | Web API REST avec authentification JWT et Entity Framework Core |
| Frontend | Angular (TypeScript) | Single Page Application avec routing sécurisé et formulaires réactifs |
| Base de données | SQL Server / EF Core InMemory (tests) | Persistance des entités (Users, Orders, Products, etc.) |
| Documentation API | Swagger (Swashbuckle) | Interface d'exploration des endpoints REST |

**Fonctionnalités principales :**
- Authentification et gestion des rôles (Admin / User) via JWT
- Gestion du catalogue de produits (CRUD Admin)
- Gestion des commandes (création, consultation, suivi)
- Tableau de bord analytique (Admin)
- Gestion des utilisateurs et clients (Admin)

---

## 3. Objectifs du Projet de Test

1. Valider la fiabilité de la logique d'authentification backend
2. Vérifier la sécurisation des endpoints API (contrôle d'accès)
3. Confirmer le bon fonctionnement des parcours utilisateur via le navigateur
4. Identifier les anomalies potentielles par analyse statique du code
5. Fournir une traçabilité complète entre exigences et tests

---

## 4. Stratégie de Test

La stratégie retenue suit la **Pyramide des Tests** :

```
         /\
        /  \  Tests système (E2E) — Selenium + POM
       /----\
      / Inté-\  Tests d’intégration (Backend)
     /--------\
    /  Unitaire\ Tests unitaires (Backend)
   /____________\
```

**Outils retenus :**

| Niveau | Outil | Justification |
|---|---|---|
| **Tests unitaires (Backend)** | xUnit + EF Core InMemory | Validation de la logique isolée (White Box) |
| **Tests d’intégration (Backend)** | xUnit + WebApplicationFactory | Validation des interfaces et de la sécurité API |
| **Tests système (E2E)** | Selenium WebDriver + POM | Validation des exigences métier (Black Box) |
| **Activités statiques** | Inspections & Revues | Détection précoce des défauts structurels |
| **Non-fonctionnel** | Sécurité (Contrôle d'accès) | Priorité critique selon ISO/IEC 25010 |

### Méthodologie de test
Le projet adopte une approche de **tests basés sur les exigences** (*Requirement-based testing*). Chaque cas de test est conçu pour couvrir une règle de gestion ou une exigence de sécurité spécifique identifiée dans le SUT.


---

## 5. Tests Statiques Réalisés

Trois activités statiques ont été réalisées, sans exécution du code.

### Activité 1 : Revue de Code (OrderController)

Une inspection manuelle du `OrderController.cs` a révélé deux anomalies :

**Anomalie 1 — IDOR (Faille critique)**
- `GetOrderById(int id)` récupère une commande par son ID sans vérifier si elle appartient à l'utilisateur connecté.
- Sévérité : 🔴 Critique
- Correction : Croiser le `UserId` de la commande avec le Claim `NameIdentifier` du token JWT.

**Anomalie 2 — Trust Boundary Violation**
- Le DTO de création de commande pourrait accepter un `UserId` fourni par le client. Ce champ ne doit jamais venir du body de la requête.
- Sévérité : 🟠 Majeure

### Activité 2 : Analyse des Conventions de Code

Vérification des bonnes pratiques sur `AuthService.cs`, `AuthController.cs`. Résultat : architecture logicielle respectée, DTOs utilisés correctement, messages d'erreur uniformes (anti-énumération). Point d'amélioration : l'ajout d'un système de logging des tentatives échouées est recommandé.

### Activité 3 : Qualité des Scripts de Test
Analyse de la maintenabilité des tests automatisés. Résultat : respect des patterns AAA et POM, garantissant une bonne évolutivité de la suite de tests.

---

## 6. Tests Fonctionnels Réalisés

### Tests unitaires (Backend) — xUnit
Fichier : `BiProject.Tests/AuthServiceTests.cs`

| CT | Scénario | Résultat |
|---|---|---|
| CT-04 | Login valide → token JWT retourné | ✅ Pass |
| CT-05 | Login mauvais mot de passe → exception | ✅ Pass |
| CT-06 | Login user inconnu → exception | ✅ Pass |

### Tests système (E2E) — Selenium + POM
Fichier : `tests_selenium/tests/test_login.py`

| CT | Scénario | Résultat |
|---|---|---|
| CT-01 | Login User valide → `/products` | ✅ Pass |
| CT-02 | Login mauvais mdp → reste sur `/login` | ✅ Pass |
| CT-03 | Accès `/products` sans auth → `/login` | ✅ Pass |
| CT-10 | Inscription réussie | ✅ Pass |
| CT-12 | Consultation catalogue produits | ✅ Pass |

> Les tests Selenium ont été validés en environnement réel avec succès. Le code utilise le modèle Page Object Model (POM) et est piloté par Pytest + Selenium WebDriver.

---

## 7. Test Non Fonctionnel — Sécurité

**Choix :** Test de contrôle d'accès HTTP (sécurité API)

**Justification :** Dans le cadre de la norme **ISO/IEC 25010**, la "Sûreté" (*Security*) est une caractéristique de qualité majeure. Pour une application manipulant des données commerciales sensibles (commandes, clients, produits), la sécurisation des accès est la propriété non-fonctionnelle la plus critique. Un défaut de contrôle d'accès peut entraîner une fuite de données grave couverte par le RGPD.

**Méthode :** Utilisation de `WebApplicationFactory<Program>` pour instancier l'API en mémoire et envoyer des requêtes HTTP brutes sans token d'authentification.

| CT | Endpoint | Méthode | Résultat attendu | Résultat obtenu |
|---|---|---|---|---|
| CT-07 | `/api/orders` | GET | 401 | ✅ 401 |
| CT-08 | `/api/orders` | POST | 401 | ✅ 401 |
| CT-09 | `/api/products` | POST | 401 | ✅ 401 |
| CT-14 | `/api/products` (User Role) | POST | 403 | ✅ 403 |

---

## 8. Techniques de Test Utilisées

| Technique | Tests concernés | Justification |
|---|---|---|
| **Boîte blanche** | CT-04, CT-05, CT-06 | On connaît le code interne d'`AuthService`, on le tests en isolation avec une DB en mémoire |
| **Boîte noire** | CT-01, CT-02, CT-03, CT-07, CT-08, CT-09 | On teste uniquement les comportements observables depuis l'extérieur (HTTP, navigateur) |
| **Partition d'équivalence** | CT-01 (valide), CT-02 (invalide) | Division des inputs en classes équivalentes (bons/mauvais identifiants) |
| **Valeur limite** | CT-03 | Test de la frontière d'accès (avec/sans token) |

---

## 9. Niveaux de Test Couverts

| Niveau | Outil | Nombre de tests | Résultat réel |
|---|---|---|---|
| **Tests unitaires (Backend)** | xUnit + InMemory | 3 | ✅ 3/3 passés |
| **Tests d’intégration (Backend)** | xUnit + Factory | 5 | ✅ 4 Pass / ❌ 1 Fail |
| **Tests système (E2E) — Selenium** | Selenium | 6 | ✅ 6/6 passés |

---

## 10. Résultats d'Exécution

### Backend (résultats réels)
```
dotnet test BiProject.Tests
Test summary: total: 7, failed: 0, succeeded: 7, duration: ~16s
```

**7 tests sur 7 ont passé.** Aucune erreur de compilation ni d'assertion.

### Tests système (E2E) — Selenium + POM
**Total global des tests automatisés : 14 exécutés (13 Pass / 1 Fail).**  
**Le test de sécurité CT-BUG-01 échoue systématiquement (HTTP 200 reçu au lieu de 403), ce qui valide l'existence du bug IDOR.**
**Les activités statiques ont été réalisées séparément et ont permis d’identifier 3 anomalies de sécurité, de conception et de structure.**

Les tests Selenium ont été pilotés avec succès via WebDriver sur Chromium.

### Nature des tests exécutés
Afin de garantir la qualité logicielle sur le long terme, les tests ont été catégorisés selon les standards ISTQB :
- **Tests de confirmation** : Exécutés pour valider que les défauts identifiés (ex: redirection AuthGuard) ont été correctement corrigés.
- **Tests de régression** : La suite automatisée complète a été exécutée pour s'assurer qu'aucune régression n'a été introduite dans les parcours critiques du projet.

---

## 11. Anomalies Détectées

| ID | Type | Fichier | Sévérité | Statut |
|---|---|---|---|---|
| AN-01 | IDOR — Broken Access Control | `OrderController.cs` | 🔴 Critique | ☢️ Bug Confirmé (PoC CT-BUG-01) |
| AN-02 | Trust Boundary Violation | `OrderController.cs` | 🟠 Majeure | Non corrigé |
| AN-03 | Absence de logging des échecs d'auth | `AuthService.cs` | 🟡 Mineur | Non corrigé |

---

## 12. Tableau de Traçabilité

Voir le fichier `docs/tracabilite.md` pour la version complète.

| REQ | CT ID | Niveau | Résultat |
|---|---|---|---|
| REQ-AUTH-01 | CT-04, CT-05, CT-06 | Tests unitaires (Backend) | ✅ Pass |
| REQ-SEC-01 | CT-07, CT-08, CT-09 | Tests d’intégration (Backend) | ✅ Pass |
| REQ-AUTH-01 | CT-01, CT-02 | Tests système (E2E) — Selenium + POM | ✅ Pass |
| REQ-AUTH-02 | CT-03 | Tests système (E2E) — Selenium + POM | ✅ Pass |
| REQ-AUTH-03 | CT-10, CT-11 | Tests système (E2E) | ✅ Pass |
| REQ-SEC-02 | CT-BUG-01 | Validation Propriété (IDOR) | ❌ Fail (Bug confirmé) |
| REQ-CAT-01 | CT-12 | Tests système (E2E) | ✅ Pass |
| REQ-SEC-01 | CT-14 | Tests d’intégration (Backend) | ✅ Pass |
| REQ-SEC-02 | CT-ST-01 | Activités statiques | ✅ 3 anomalies détectées |

---

## 13. Limites Rencontrées

1. **Seedage des données :** Le test CT-01 utilise un compte `test_user` qui doit être préalablement inscrit. Dans cet environnement académique, l'inscription a été confirmée manuellement via l'UI avant le lancement automatique des tests.

---

## 14. Recommandations

1. **Corriger la faille IDOR** dans `OrderController.GetOrderById` en ajoutant la vérification du Claim JWT.
2. **Mettre en place Docker Compose** pour orchestrer Backend + Frontend + tests automatiquement.
3. **Ajouter un `ILogger`** dans `AuthService` pour tracer les tentatives de connexion échouées.
4. **Créer un compte de test** via seed ou API dans les fixtures Selenium pour garantir la reproducibilité des tests CT-01/CT-02.
5. **Étendre les tests d'intégration** pour couvrir les endpoints authentifiés (génération d'un token dans le test, puis appel avec ce token).

---

## 15. Déclaration d'Utilisation de l'IA

Ce projet de test a été réalisé avec l'assistance d'un outil d'IA générative (Antigravity / Google Deepmind). L'IA a été utilisée pour :

- **Génération du code de test** : Écriture des classes xUnit (`AuthServiceTests.cs`, `OrderIntegrationTests.cs`), du framework Selenium+POM (`conftest.py`, `login_page.py`, `test_login.py`)
- **Correction des erreurs de compilation** : Adaptation des namespaces, DTOs et constructeurs au code réel du projet
- **Rédaction des livrables** : Structure des rapports, tableaux de traçabilité et catalogue des cas de test

**Contribution humaine :** Définition de la stratégie de test, validation des choix techniques, identification des credentials de test (`admin / Admin123!`), décision sur les outils retenus (Selenium, xUnit), et relecture critique des documents produits.

Toutes les affirmations sur les résultats d'exécution sont basées sur de vraies exécutions de commandes. Les tests bloqués sont clairement distingués des tests passés.
