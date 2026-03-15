# MATRICE DE TRAÇABILITÉ — BIPROJECT QA

**Projet :** BiProject  
**Auteurs :** Youssef Chaari & Mohamed Aziz Zouari  
**Date :** 15 Mars 2026  
**Version :** 1.2  

---

## 1. Introduction
Ce document établit le lien direct entre les exigences métiers (**REQ-XXX**) et les cas de test associés (**CT-XX**). Il permet de garantir la couverture exhaustive du périmètre et de visualiser l'état de qualification de chaque fonctionnalité.

---

## 2. Matrice de Traçabilité des Exigences

| Code Exigence | Description Fonctionnelle | Cas de Test (CT) | État de Qualification |
| :--- | :--- | :--- | :--- |
| **REQ-AUTH-01** | L'utilisateur peut se connecter avec sécurité (JWT/BCrypt) | **CT-01, 02, 04, 05, 06** | Qualifiée (✅) |
| **REQ-AUTH-02** | Les routes UI sont protégées contre les accès anonymes | **CT-03** | Qualifiée (✅) |
| **REQ-AUTH-03** | Inscription d'un nouveau compte avec gestion d'unicité | **CT-10, 11** | Qualifiée (✅) |
| **REQ-CAT-01** | Consultation fluide du catalogue et des détails produits | **CT-12, 13** | Qualifiée (✅) |
| **REQ-CAT-02** | Administration du catalogue réservée au rôle Admin | **CT-14, 15** | Qualifiée (✅) |
| **REQ-ORD-01** | Passage d'une commande et consultation de son historique | **CT-16, 17** | Qualifiée (✅) |
| **REQ-SEC-01** | Sécurité par défaut sur tous les endpoints de l'API | **CT-07, 08, 09** | Qualifiée (✅) |
| **REQ-SEC-02** | Défense contre les attaques IDOR sur les ressources privées | **CT-18, 20** | **Anomalie (❌)** |
| **REQ-SEC-03** | Segmentation stricte des privilèges BI/Analytics | **CT-19** | **Anomalie (❌)** |

---

## 3. Analyse de Couverture
Toutes les exigences critiques identifiées lors du lancement du projet ont été associées à au moins un scénario de test automatisé. 

- Les exigences fonctionnelles atteignent un taux de qualification de **100%**.
- Les exigences de sécurité révèlent des non-conformités majeures (Anomalies) qui sont documentées dans le rapport final pour remédiation.

---
*Fin du tableau de traçabilité.*
