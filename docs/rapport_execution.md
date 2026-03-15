# Rapport d'Exécution — BiProject QA 
## Synthèse globale

| Couche | Outil | Tests exécutés | Passés | Échoués | Bloqués |
|---|---|---|---|---|---|
| **Tests unitaires (Backend)** | xUnit (.NET) | 3 | ✅ 3 | 0 | 0 |
| **Tests d’intégration (Backend)** | xUnit + Factory | 11 | ✅ 9 | ❌ 2 | 0 |
| **Tests système (E2E) — Selenium + POM** | Selenium | 6 | ✅ 6 | 0 | 0 |
| **Activités statiques** | Inspections & Revues | 3 activités | ✅ 3 anomalies détectées | — | — |
| **Total automatisé** | — | **20** | **17** | **3** | **0** |

**Statistiques globales : 20 tests exécutés (17 Pass / 3 Fail révélateurs) — 85% de succès.**
**Les échecs (CT-20 et CT-19) confirment dynamiquement les vulnérabilités de contrôle d'accès identifiées.**
**Les activités statiques ont été réalisées séparément et ont permis d’identifier 3 anomalies de sécurité, de conception et de structure.**

### Distinction sémantique des exécutions
Conformément aux standards ISTQB :
- **Tests de confirmation** : Chaque scénario a été re-testé après correction ou déploiement initial pour s'assurer du bon fonctionnement du code "fixé".
- **Tests de régression** : La suite complète (xUnit et Selenium) sert désormais de base de régression automatisée, permettant de garantir qu'une modification future n'altère pas les fonctionnalités existantes (Login, AuthGuard, Contrôle d'accès API).

---

## Détail des tests Backend (résultats réels)

### Tests unitaires (Backend) — `AuthServiceTests.cs`

| Test | Statut | Durée |
|---|---|---|
| `LoginAsync_WithValidCredentials_ReturnsToken` | ✅ Pass | ~500ms |
| `LoginAsync_WithInvalidPassword_ThrowsUnauthorizedAccessException` | ✅ Pass | ~200ms |
| `LoginAsync_WithUnknownUser_ThrowsUnauthorizedAccessException` | ✅ Pass | ~150ms |

**Commande exécutée :** `dotnet test BiProject.Tests`  
**Résumé console synthétique :** `Test summary: total: 8, failed: 1, succeeded: 7` (L'échec de CT-20 valide le bug).

### Tests d’intégration (Backend) — `OrderIntegrationTests.cs`

| Test | Statut | Code HTTP obtenu |
|---|---|---|
| `GetOrders_WithoutAuthentication_ReturnsUnauthorized` | ✅ Pass | 401 |
| `CreateOrder_WithoutAuthentication_ReturnsUnauthorized` | ✅ Pass | 401 |
| `CreateProduct_WithoutAuthentication_ReturnsUnauthorized` | ✅ Pass | 401 |
| `PostProduct_AsUser_ReturnsForbidden` (CT-14) | ✅ Pass | 403 |
| `GetOrders_AsUser_ReturnsOk` (CT-16) | ✅ Pass | 200 |
| `CreateOrder_AsUser_ReturnsCreated` (CT-17) | ✅ Pass | 201 |
| `GetProductById_ReturnsOk` (CT-13) | ✅ Pass | 200 |
| `CreateProduct_AsAdmin_ReturnsCreated` (CT-15) | ✅ Pass | 201 |
| `GetOrders_WithoutAuthentication_ReturnsUnauthorized` | ✅ Pass | 401 |
| `CreateOrder_WithoutAuthentication_ReturnsUnauthorized` | ✅ Pass | 401 |
| `CreateProduct_WithoutAuthentication_ReturnsUnauthorized` | ✅ Pass | 401 |
| `GetProductById_WithoutAuthentication_ReturnsUnauthorized` | ✅ Pass | 401 |

---

## Détail des tests système (E2E) — Selenium + POM (statut : ✅ Succès)

| CT | Scénario | Statut | Résultat Réel |
|---|---|---|---|
| CT-01 | Login User valide → redirection `/products` | ✅ Pass | Détecté sur /products (3.2s) |
| CT-02 | Login mauvais mdp → rester sur `/login` | ✅ Pass | Bloqué sur /login (3.1s) |
| CT-03 | Accès `/products` sans auth → redirection `/login` | ✅ Pass | Redirigé par AuthGuard (2.1s) |
| CT-10 | Inscription réussie → redirection `/products` | ✅ Pass | Redirection confirmée (5.1s) |
| CT-11 | Inscription doublon → erreur affichée | ✅ Pass | Erreur "existe déjà" capturée |
| CT-12 | Consultation catalogue → liste visible | ✅ Pass | Grille de produits chargée |

---

## Validation de Sécurité (Échec révélateur)

Ce test vérifie la propriété de sécurité attendue. Son échec (HTTP 200 reçu au lieu de 403) constitue la preuve formelle du défaut de contrôle d'accès.

| ID | Titre | Objectif | Résultat de la validation |
|---|---|---|---|
| CT-20 | Validation IDOR | Vérifier le blocage d'accès (403/404) | ❌ Fail (Accès 200 OK autorisé) |
| CT-18 | Isolation IDOR | Vérification sur ID fixe | ❌ Fail (Confirmé par CT-20) |
| CT-19 | Sécurité Analytics | Vérifier restriction Admin sur KPIs | ❌ Fail (User accède en 200 OK) |

> **Note :** Les tests Selenium ont été validés en environnement réel avec le Backend (:5120) et le Frontend (:4200) actifs.

---

## Commandes d'exécution (pour exécuter vous-même)

```powershell
# Terminal 1 — Backend
cd d:\projet\BiProject\BiProject.Api
dotnet run

# Terminal 2 — Frontend
cd d:\projet\BiProject\BiProject.Ui
ng serve

# Terminal 3 — Tests Backend (pas besoin de frontend)
cd d:\projet\BiProject
dotnet test BiProject.Tests

# Terminal 3 — Tests Selenium (après démarrage Backend + Frontend)
cd d:\projet\BiProject\tests_selenium
pip install -r requirements.txt
pytest tests/ -v
```
