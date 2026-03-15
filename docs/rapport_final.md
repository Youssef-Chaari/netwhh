# Rapport Final — Projet QA Académique : BiProject (Y. Chaari & M. Zouari)

## 1. Introduction
Ce document constitue le rapport final exhaustif du projet d'Assurance Qualité mené sur l'application **BiProject**. Réalisé dans le cadre du module "Test et Qualité Logiciel", ce travail démontre la mise en œuvre pratique des standards industriels (ISTQB, ISO/IEC 25010) via une approche hybride mêlant analyses statiques et tests automatisés.

L'objectif est de garantir une plateforme e-commerce fiable, sécurisée et conforme aux exigences métier prioritaires.

---

## 2. Présentation détaillée du projet BiProject

### 2.1 Objectif métier
BiProject est une plateforme de commerce électronique dédiée au matériel cycliste. Elle offre des fonctionnalités distinctes pour les clients (catalogue, commandes) et les administrateurs (gestion de stock, analytics BI).

### 2.2 Architecture Technique
L'écosystème repose sur une architecture moderne découplée :
- **Backend** : API REST sous **ASP.NET Core 8** (C#) avec authentification JWT et Entity Framework Core.
- **Frontend** : Application Single Page (SPA) en **Angular** avec routing protégé.
- **Base de données** : SQL Server (Production) et EF Core InMemory (Environnement de test).

### 2.3 Périmètre du Système Sous Test (SUT)
Le périmètre d'audit se concentre sur les flux critiques :
- **Authentification** : Connexion, Inscription et Protection des routes.
- **Gestion du Catalogue** : Lecture et contrôle d'accès aux droits de création.
- **Transactions** : Sécurisation des données de commande (Focus IDOR).

---

## 3. Plan et stratégie de test (Barème : 5%)
La stratégie adoptée suit la **Pyramide des Tests** pour maximiser la couverture tout en maîtrisant la dette technique.

- **Stratégie de focalisation** : Priorité donnée aux risques de sécurité (Contrôle d'accès) et à la fiabilité du noyau d'authentification.
- **Niveaux de test** :
  - **Tests Unitaires** : Validation de la logique isolée des services (AuthService).
  - **Tests d'Intégration** : Utilisation de `WebApplicationFactory` pour tester l'API en mémoire avec de vrais appels HTTP.
  - **Tests Système (E2E)** : Simulation d'utilisateurs réels dans Chromium via Selenium WebDriver.
- **Justification des outils** : xUnit (Backend), Selenium + Pytest + POM (Frontend).

---

## 4. Tests statiques (Barème : 5%)
Trois activités statiques majeures ont permis de détecter des défauts structurels précoces :
1. **Revue de Code** : Identification manuelle d'une vulnérabilité **IDOR** critique dans `OrderController.cs` et d'une violation de frontière de confiance (Trust Boundary) dans les DTOs.
2. **Analyse de Sécurité** : Audit orienté OWASP (Broken Access Control).
3. **Audit de Qualité des Tests** : Validation de la structure des scripts (Pattern AAA) et de la maintenabilité (Page Object Model).
*Preuve : 3 anomalies documentées (1 Critique, 1 Majeure, 1 Mineure).*

---

## 5. Cas de test fonctionnels et non fonctionnels (Barème : 25%)

### 5.1 Bilan des Cas Exécutés
Un total de **20 cas de test uniques** ont été automatisés et exécutés :
- **14 Tests Backend** (Unitaires + Intégration)
- **6 Tests Selenium** (Système / E2E)

### 5.2 Typologie des tests
- **Fonctionnels** : Validation des parcours nominaux (Login ok) et alternatifs (Mauvais mdp, doublon inscription).
- **Non-fonctionnels (Sécurité)** : Tests de privilèges (User vs Admin) et test de validation de propriété (IDOR).

### 5.3 Focus : Tests de Validation de Sécurité (Preuves par l'échec)
Contrairement aux autres tests, **CT-20**, **CT-18** et **CT-19** sont des tests de validation de propriété de sécurité. Leurs échecs (Accès autorisé au lieu de bloqué) constituent les preuves dynamiques formelles des failles de contrôle d'accès identifiées.
- **CT-20** : Faille IDOR sur les commandes (**Validation de vulnérabilité** : Cloisonnement des données).
- **CT-18** : Isolation IDOR sur les commandes (**Filtre fixe** : Test d'isolation sur ordres existants).
- **CT-19** : Absence de restriction de rôle sur les données stratégiques Analytics (Accès user non restreint).

---

## 6. Automatisation (Barème : 40%)

### 6.1 Architecture des scripts
Les scripts ont été conçus pour être robustes et facilement maintenables :
- **Page Object Model (POM)** : Pour Selenium, les éléments UI (sélecteurs CSS) sont isolés dans des classes dédiées (ex: `LoginPage.py`).
- **Pattern AAA (Arrange, Act, Assert)** : Structure claire de chaque test pour une lisibilité maximale.
- **WebApplicationFactory** : Permet de monter l'API Backend de manière éphémère en mémoire pour des tests d'intégration ultra-rapides.

### 6.2 Qualité et Maintenance
L'usage de **Fixtures** (Pytest) et de **ClassFixtures** (xUnit) garantit un cycle de vie propre des données de test (isolation entre les exécutions).

---

## 7. Traçabilité et Livrables (Barème : 10%)
La cohérence documentaire est assurée par une matrice de traçabilité reliant les exigences aux résultats.
- **`cas_de_test.md`** : Catalogue détaillé des fiches de test.
- **`tracabilite.md`** : Matrice Exigences → Cas de Test.
- **`tests_statiques.md`** : Registre des anomalies structurelles.
- **`rapport_execution.md`** : Journal brut des sorties console.

---

## 8. Résultats d'exécution détaillés
| **Couche / Type** | **Total Exécutés** | **Pass (Succès)** | **Fail (Faille)** | **Not Executed** |
|---|---|---|---|---|
| **Backend (Unit/Intégration)** | 14 | 11 | 3 | 0 |
| **Selenium (E2E)** | 6 | 6 | 0 | 0 |
| **TOTAL** | **20** | **17** | **3** | **0** |

*Note sur les fails : Les échecs des tests Backend valident l'existence d'anomalies critiques de sécurité.*

---

## 9. Analyse des anomalies
1. **AN-01 (IDOR)** : Faille critique dans `OrderController`. Possibilité de consulter les commandes de n'importe quel utilisateur.
2. **AN-02 (Trust Boundary)** : L'API accepte un `UserId` venant du client, permettant une usurpation d'identité lors des commandes.
3. **AN-03 (Logging)** : Absence de traces sur les échecs d'authentification (risque de brute-force non détecté).
4. **AN-04 (Broken Access Control - Analytics)** : L'endpoint `/api/analytics/kpis` n'est pas restreint au rôle Admin, exposant des données BI à n'importe quel utilisateur (Confirmé par CT-19).

---

## 10. Procédure complète d'exécution

### 10.1 Prérequis
- SDK .NET 8, Node.js, Python 3.10.
- `pip install -r tests_selenium/requirements.txt`.

### 10.2 Commandes Backend
```powershell
# Build & Tests globaux (Backend)
dotnet test BiProject.Tests
```

### 10.3 Commandes Selenium
```powershell
# Lancement des tests E2E (API et UI doivent être démarrés)
cd tests_selenium
python -m pytest tests/ -v
```

### 10.4 Ordre recommandé
1. `dotnet run` (API) puis `npm run start` (UI).
2. Lancer les tests Backend (Validation rapide).
3. Lancer les tests Selenium (Validation de l'interface).

---

## 11. Limites et recommandations
- **Limites** : Seedage manuel de certains comptes de test. Les scénarios de commandes (CT-16, CT-17) ont été validés prioritairement via l'intégration Backend pour garantir la robustesse des flux de données.
- **Recommandations** : Corriger l'IDOR immédiatement en injectant le `ClaimTypes.NameIdentifier` dans la logique métier. Intégrer les tests dans une pipeline CI/CD (GitHub Actions).

---

## 12. Déclaration d'usage de l'IA
Ce projet a bénéficié de l'assistance de l'IA **Antigravity** (Google Deepmind) pour :
- La génération des structures de rapports et tableaux.
- L'écriture du boilerplate des tests Selenium (POM) et xUnit.
- L'aide au débuggage d'environnement.
*Toute la logique métier, la stratégie de test et la validation des résultats ont été pilotées et vérifiées par le testeur humain.*

---

---

## 13. Bonus Académique : Prototype Robot Framework
En complément de la suite Selenium principale, un prototype basé sur **Robot Framework** a été ajouté pour démontrer l’ouverture du projet à d’autres outils d’automatisation.

- **Objectif** : Tester la lisibilité des scénarios via une approche "Keyword-Driven".
- **Fichiers** : [login_tests.robot](file:///d:/projet/BiProject/tests_robot/login_tests.robot)
- **Scénarios couverts** : 
    1. Authentification nominale (Happy Path).
    2. Consultation automatique du catalogue après connexion.
- **Valeur ajoutée** : Meilleure collaboration avec les profils non techniques grâce à des keywords en langage naturel.
- **Statut** : Prototype fonctionnel (Preuve de concept).

---

## 14. Conclusion
Le projet BiProject QA présente une maturité de test avancée. La combinaison de la pyramide d'automatisation et de la détection proactive de vulnérabilités critiques via l'analyse statique répond aux exigences majeures de qualité fixées lors de la conception. Le projet est conforme au cahier des charges et prêt pour une remise académique.
