# RAPPORT D'EXÉCUTION DES TESTS — BIPROJECT QA

**Projet :** BiProject  
**Auteurs :** Youssef Chaari & Mohamed Aziz Zouari  
**Date :** 15 Mars 2026  
**Version :** 1.2 (Harmonisée)  

---

## 1. Synthèse Globale des Résultats

L'exécution de la suite de tests automatisée a permis de valider la stabilité fonctionnelle du système tout en confirmant des points de vulnérabilité critiques.

| Indicateur | Valeur |
| :--- | :--- |
| **Total des tests exécutés** | 20 |
| **Tests réussis (Pass)** | 17 |
| **Tests échoués (Fail)** | 3 |
| **Taux de succès global** | **85%** |

---

## 2. Résultats par Couche de Test

### 2.1 Tests Unitaires (Backend)
Validation des services métier en isolation.
- **Total** : 3 tests
- **Statut** : 3 ✅ Pass
- **Outil** : xUnit (`AuthServiceTests.cs`)

### 2.2 Tests d'Intégration (Backend)
Vérification des endpoints API et de la sécurité serveur.
- **Total** : 11 tests
- **Statut** : 8 ✅ Pass / 3 ❌ Fail
- **Outil** : xUnit (`OrderIntegrationTests.cs`, `SecurityIntegrationTests.cs`)

### 2.3 Tests Système (E2E)
Simulation de parcours utilisateurs complets dans le navigateur.
- **Total** : 6 tests
- **Statut** : 6 ✅ Pass (100%)
- **Outil** : Selenium + POM

---

## 3. Détail des Échecs Révélateurs

| Cas de Test | Nature de l'échec | Résultat Réel | Statut |
| :--- | :--- | :--- | :--- |
| **CT-18** | Isolation IDOR | Accès autorisé à une commande tierce | ❌ Fail |
| **CT-19** | Restriction RBAC | Utilisateur simple accède aux données BI | ❌ Fail |
| **CT-20** | Preuve IDOR | Fuite de données constatée via API | ❌ Fail |

---

## 4. Commandes d'Exécution

### Backend et Intégration
```powershell
dotnet test BiProject.Tests
```

### Système (UI)
```powershell
# Prérequis : Backend et Frontend actifs
pytest tests_selenium/tests -v
```

---
*Fin du rapport d'exécution.*
