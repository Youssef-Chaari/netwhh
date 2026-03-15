<style>
  header, footer { display: none !important; }
  @page { margin: 25mm; }
  body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; line-height: 1.6; color: #333; }
  h1 { color: #2c3e50; border-bottom: 2px solid #2c3e50; padding-bottom: 10px; margin-top: 40px; text-align: center; }
  h2 { color: #2980b9; border-left: 5px solid #2980b9; padding-left: 10px; margin-top: 35px; background-color: #f7f9fc; padding-top: 5px; padding-bottom: 5px; border-bottom: 1px solid #eee; }
  h3 { color: #16a085; margin-top: 30px; border-bottom: 1px dotted #ccc; }
  h4 { color: #d35400; margin-top: 20px; font-variant: small-caps; }
  table { width: 100%; border-collapse: collapse; margin-bottom: 20px; page-break-inside: auto; }
  th, td { border: 1px solid #bdc3c7; padding: 10px; text-align: left; }
  th { background-color: #ecf0f1; font-weight: bold; }
  .fail { color: #c0392b; font-weight: bold; }
  .pass { color: #27ae60; font-weight: bold; }
  .critical { background-color: #fce4e4; color: #c0392b; font-weight: bold; }
  .major { background-color: #fff4e5; color: #d35400; font-weight: bold; }
  .minor { background-color: #eafaf1; color: #27ae60; font-weight: bold; }
  code { background-color: #f8f8f8; border: 1px solid #ddd; padding: 2px 5px; border-radius: 3px; font-family: Consolas, monospace; }
  pre { background-color: #f8f8f8; border: 1px solid #ddd; padding: 15px; border-radius: 5px; overflow-x: auto; white-space: pre-wrap; word-wrap: break-word; }
  .page-break { page-break-before: always; }
</style>

# RAPPORT FINAL D’ASSURANCE QUALITÉ — MASTER DOCUMENT 🎓

**Projet :** BiProject - Système de Gestion Commerciale & BI  
**Équipe de Test :** Youssef Chaari & Mohamed Aziz Zouari  
**Date du Rendu :** 15 Mars 2026  
**Version :** 2.0 (Consolidation Intégrale & Autonome)

---

<div class="page-break"></div>

## 1. Introduction & Périmètre
### 1.1 Contexte du Projet
Ce document constitue le **livrable unique et exhaustif** de l'audit de qualité logicielle réalisé sur l'application **BiProject**. Il centralise les analyses statiques, le catalogue de tests dynamiques, la traçabilité des exigences et le bilan d'exécution.

### 1.2 Objectifs Stratégiques
- **Validation Fonctionnelle** : Garantir que les fonctionnalités de base (Auth, Catalogue, Paniers) répondent aux exigences.
- **Audit de Sécurité** : Identifier les vulnérabilités de contrôle d'accès (IDOR, Broken Access Control).
- **Automatisation Totale** : Mise en place d'une suite de non-régression multi-couches.

### 1.3 Système Sous Test (SUT)
BiProject est une application composée :
- D'un **Backend REST** sous ASP.NET Core 8.
- D'un **Frontend SPA** sous Angular.
- D'une gestion de données via Entity Framework.

---

## 2. Analyse Statique Détaillée
L'analyse statique a été réalisée sans exécution, via des revues de code et inspections de structure.

### 2.1 Échelle de Gravité
| Niveau | Impact |
|---|---|
| **🔴 Critique** | Faille de sécurité majeure ou blocage total. |
| **🟠 Majeure** | Violation grave de conception ou risque réel. |
| **🟡 Mineure** | Écart de convention ou optimisation. |

### 2.2 Rapport d'Anomalies Statiques
| ID | Fichier | Anomalie | Gravité | Recommandation |
|---|---|---|---|---|
| **AN-01** | `OrderController.cs` | **IDOR** (Accès direct aux commandes tiers) | 🔴 Critique | Valider le `UserId` via Claims JWT |
| **AN-02** | Order DTO | Trust Boundary Violation (UserId falsifiable) | 🟠 Majeure | Extraire l'ID du token côté serveur |
| **AN-03** | `AuthService.cs` | Absence de logging des échecs d'auth | 🟡 Mineure | Injecter `ILogger` pour l'audit |

### 2.3 Revue de la Qualité du Code de Test
L'audit des scripts de test (xUnit et Selenium) confirme le respect des standards :
- **Pattern AAA** : Appliqué à 100% des tests Backend.
- **POM (Page Object Model)** : Utilisé pour isoler la logique de test de l'UI Angular.
- **Clean Code** : Usage de `IClassFixture` et `conftest.py` pour une gestion efficace des ressources.

---

<div class="page-break"></div>

## 3. Catalogue Complet des Cas de Test (Spécifications)
Voici la spécification détaillée de chacun des **20 cas de test** automatisés.

### [CT-01] Connexion réussie (User)
- **Objectif** : Valider l'accès nominal avec des identifiants valides.
- **Type** : Fonctionnel (Boîte Noire).
- **Prérequis** : Backend (:5120) et Frontend (:4200) actifs.
- **Données** : `test_user` / `TestPassword123!`.
- **Scénario** : Saisie credentials -> Clic Submit -> Redirection `/products`.
- **Statut** : ✅ Pass.

### [CT-02] Rejet mot de passe invalide
- **Objectif** : Interdire l'accès en cas de mot de passe erroné.
- **Type** : Fonctionnel (Boîte Noire).
- **Statut** : ✅ Pass.

### [CT-03] Protection Guard (Accès Direct)
- **Objectif** : S'assurer que le Guard Angular redirige vers `/login` si accès direct à `/products`.
- **Type** : Sécurité.
- **Statut** : ✅ Pass.

### [CT-04] Génération Token JWT (Service)
- **Objectif** : Valider la création technique du jeton en couche service.
- **Type** : Unitaire (Boîte Blanche).
- **Statut** : ✅ Pass.

### [CT-05] Exception Auth (Service)
- **Objectif** : Valider la levée d'éxception `UnauthorizedAccessException` en cas de mauvais mot de passe.
- **Type** : Unitaire (Boîte Blanche).
- **Statut** : ✅ Pass.

### [CT-06] Rejet User Inconnu (Service)
- **Objectif** : Empêcher l'énumération de comptes.
- **Statut** : ✅ Pass.

### [CT-07] API Security (GET Orders)
- **Objectif** : Retourner 401 Unauthorized si l'appel GET est fait sans Token.
- **Type** : Intégration (Sécurité).
- **Statut** : ✅ Pass.

### [CT-08] API Security (POST Orders)
- **Objectif** : Interdire la création de commande anonyme.
- **Statut** : ✅ Pass.

### [CT-09] API Security (POST Products)
- **Objectif** : Interdire l'ajout de produits anonyme.
- **Statut** : ✅ Pass.

### [CT-10] Inscription Success (E2E)
- **Objectif** : Valider le flux COMPLET d'inscription UI -> API -> DB.
- **Statut** : ✅ Pass.

### [CT-11] Reject Doublon Inscription (E2E)
- **Objectif** : Afficher une erreur si le login existe déjà.
- **Statut** : ✅ Pass.

### [CT-12] Load Catalogue (E2E)
- **Objectif** : Vérifier que la grille de produits est chargée et visible après login.
- **Statut** : ✅ Pass.

### [CT-13] Product Details (Integration)
- **Objectif** : Valider l'endpoint `GET /api/products/{id}`.
- **Statut** : ✅ Pass.

### [CT-14] RBAC Protection (User tries to Create Product)
- **Objectif** : Retourner 403 Forbidden si un 'User' tente de créer un produit (réservé Admin).
- **Type** : Sécurité.
- **Statut** : ✅ Pass.

### [CT-15] Admin Privilege (Create Product success)
- **Objectif** : Autoriser la création de produit pour le rôle 'Admin'.
- **Statut** : ✅ Pass.

### [CT-16] History Filtering
- **Objectif** : S'assurer que l'utilisateur ne voit QUE ses propres commandes.
- **Statut** : ✅ Pass.

### [CT-17] Full Checkout Flow
- **Objectif** : Valider la création d'une commande valide via POST API.
- **Statut** : ✅ Pass.

### [CT-18] Isolation Isolation (IDOR Test Case)
- **Objectif** : **Validation de la faille IDOR**. Tenter de lire la commande ID=1 (UserA) avec le Token de UserB.
- **Attendu** : 403 Forbidden.
- **Réel** : 200 OK (Accès autorisé).
- **Statut** : ❌ <span class="fail">Fail (Révèle vulnérabilité)</span>.

### [CT-19] BI Analytics Protection
- **Objectif** : Interdire l'accès aux KPIs Analytics pour le rôle 'User'.
- **Attendu** : 403 Forbidden.
- **Réel** : 200 OK.
- **Statut** : ❌ <span class="fail">Fail (Révèle vulnérabilité)</span>.

### [CT-20] Dynamic IDOR Exploit
- **Objectif** : Preuve dynamique d'accès illégitime à une ressource dynamique tiers.
- **Attendu** : Blocage système.
- **Réel** : Fuite de données constatée.
- **Statut** : ❌ <span class="fail">Fail (Exploit PoC réussi)</span>.

---

<div class="page-break"></div>

## 4. Rapport d'Exécution & Bilan Dynamique
L'exécution a été réalisée sur un environnement de test isolé reproduisant la production.

### 4.1 Synthèse Globale
| Couche | Tests | <span class="pass">Pass</span> | <span class="fail">Fail</span> | Taux de Succès |
|---|---|---|---|---|
| **Backend (Unit/Int)** | 14 | 11 | 3 | 78.5% |
| **Frontend (E2E)** | 6 | 6 | 0 | 100% |
| **TOTAL** | **20** | **17** | **3** | **85%** |

### 4.2 Analyse des Échecs Révélateurs
Les 3 échecs (CT-18, CT-19, CT-20) confirment dynamiquement les failles détectées lors de l'analyse statique. 
- **Impact** : Risque de fuite de données personnelles et stratégiques.
- **Conclusion** : Le système est fonctionnellement robuste mais nécessite des correctifs de sécurité urgents.

### 4.3 Commandes d'Exécution
```powershell
# Démarrage
dotnet run --project BiProject.Api
npm start --prefix BiProject.Ui

# Exécution des tests
dotnet test BiProject.Tests
pytest tests_selenium/tests -v
```

---

<div class="page-break"></div>

## 5. Matrice de Traçabilité (Complète)
Toutes les exigences critiques sont couvertes par la suite de test.

| ID Exigence | Description | CT Associés | Statut Qualification |
|---|---|---|---|
| **REQ-AUTH-01** | Login/JWT Security | CT-01, 02, 04, 05, 06 | ✅ Qualifié |
| **REQ-AUTH-02** | UI Routes Protection | CT-03 | ✅ Qualifié |
| **REQ-AUTH-03** | Registration Flow | CT-10, 11 | ✅ Qualifié |
| **REQ-CAT-01** | Inventory Navigation | CT-12, 13 | ✅ Qualifié |
| **REQ-SEC-01** | API Authorize Defaults | CT-07, 08, 09, 14 | ✅ Qualifié |
| **REQ-SEC-02** | **Isolation IDOR** | **CT-18, CT-20** | ⚠️ **Anomalie Détectée** |
| **REQ-SEC-03** | **BI/Analytics RBAC** | **CT-19** | ⚠️ **Anomalie Détectée** |
| **REQ-ORD-01** | Order History & Checkout | CT-16, 17 | ✅ Qualifié |

---

## 6. Automatisation & Bonus
- **Robot Framework** : En plus de Selenium, un prototype Keyword-Driven (`tests_robot/`) a été validé avec 2 scénarios Pass.
- **Patterns** : Usage de **FluentAssertions** (Backend) et **Page Object Model** (Frontend) pour garantir la maintenabilité à long terme.

---

## 7. Déclaration d'usage de l'IA & Conclusion
Ce projet a été réalisé en pair-programming avec l'IA **Antigravity**. L'humain a défini la stratégie, conçu les tests de sécurité et validé chaque résultat, tandis que l'IA a assisté sur la génération de code de test et la consolidation documentaire.

### Conclusion Finale
Le projet BiProject QA démontre un haut niveau de maturité en automatisation. Les tests dynamiques ont rempli leur double mission : garantir le fonctionnement nominal (Pass à 85%) et prouver l'existence de failles critiques (Fails révélateurs). Le système est audité selon les standards industriels.

**Livrable prêt pour remise finale.**
