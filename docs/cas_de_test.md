# Catalogue des Cas de Test — BiProject QA

**Gestion Documentaire :**
- **Projet :** BiProject
- **Version :** 1.1 (Final)
- **Créé par :** Youssef Chaari (avec assistance IA Antigravity/Google Deepmind)
- **Revue par :** Youssef Chaari
- **Date de création :** 2026-03-12
- **Dernière mise à jour :** 2026-03-14

---

## CT-01 : Connexion réussie avec identifiants valides

| Champ | Valeur |
|---|---|
| **ID** | CT-01 |
| **Titre** | Connexion réussie avec identifiants valides (User) |
| **Niveau** | Tests système (E2E) — Selenium + POM |
| **Type** | Fonctionnel |
| **Technique** | Boîte noire — Partition d'équivalence (classe valide) |
| **Motivation** | On ne connaît pas le code interne Angular/API, on teste uniquement le comportement visible depuis le navigateur |
| **Fichier** | `tests_selenium/tests/test_login.py` |
| **Prérequis** | Backend sur :5120, Frontend sur :4200, compte utilisateur existant |
| **Données de test** | `test_user` / `TestPassword123!` |
| **Étapes** | 1. Naviguer vers `/login` 2. Saisir username 3. Saisir password 4. Cliquer Submit |
| **Résultat attendu** | Redirection vers `/products` |
| **Résultat réel** | Redirection vers `/products` confirmée |
| **Statut** | ✅ Pass |

---

## CT-02 : Connexion échouée — mauvais mot de passe

| Champ | Valeur |
|---|---|
| **ID** | CT-02 |
| **Titre** | Rejet d'une connexion avec mot de passe invalide |
| **Niveau** | Système (E2E) |
| **Type** | Fonctionnel |
| **Technique** | Boîte noire — Partition d'équivalence (classe invalide) |
| **Motivation** | Valider le comportement de l'application face à un input invalide, sans accès au code source |
| **Fichier** | `tests_selenium/tests/test_login.py` |
| **Prérequis** | Backend sur :5120, Frontend sur :4200 |
| **Données de test** | `test_user` / `MauvaisMotDePasse999!` |
| **Étapes** | 1. Naviguer vers `/login` 2. Saisir username 3. Saisir password 4. Cliquer Submit |
| **Résultat attendu** | Rester sur la page `/login` — aucune redirection |
| **Résultat réel** | Maintien sur la page `/login` avec message d'erreur |
| **Statut** | ✅ Pass |

---

## CT-03 : Accès à une route protégée sans authentification

| Champ | Valeur |
|---|---|
| **ID** | CT-03 |
| **Titre** | L’AuthGuard Angular bloque l’accès direct à `/products` |
| **Niveau** | Tests système (E2E) |
| **Type** | Non fonctionnel — Sécurité |
| **Technique** | Boîte noire — test de contrôle d’accès |
| **Motivation** | Vérifier que le mécanisme de sécurité côté client bloque l’accès à une route protégée sans authentification, sans dépendre de l’implémentation interne |
| **Fichier** | `tests_selenium/tests/test_login.py` |
| **Prérequis** | Frontend sur :4200, aucun token en session |
| **Données de test** | URL : `http://localhost:4200/products` |
| **Étapes** | 1. Se rendre directement sur `/products` sans se connecter |
| **Résultat attendu** | Redirection automatique vers `/login` |
| **Résultat réel** | Redirection vers `/login` effectuée |
| **Statut** | ✅ Pass |

---

## CT-04 : AuthService retourne un token pour identifiants valides

| Champ | Valeur |
|---|---|
| **ID** | CT-04 |
| **Titre** | `AuthService.LoginAsync` retourne un token JWT |
| **Niveau** | Tests unitaires (Backend) |
| **Type** | Fonctionnel |
| **Technique** | Boîte blanche — Test de flux interne du service |
| **Motivation** | On connaît la structure interne du service, on cherche à valider la logique sans dépendances réelles (DB InMemory) |
| **Fichier** | `BiProject.Tests/AuthServiceTests.cs` |
| **Prérequis** | DB InMemory avec un user Role=User, mot de passe haché BCrypt |
| **Données de test** | user=`testuser`, pwd=`GoodPassword123!` |
| **Étapes** | 1. Créer un user en DB InMemory 2. Appeler `LoginAsync` avec les bons identifiants |
| **Résultat attendu** | Retourne un `AuthResponseDto` avec un Token non vide |
| **Résultat réel** | `AuthResponseDto` retourné avec Token JWT |
| **Statut** | ✅ Pass |

---

## CT-05 : AuthService lève une exception pour un mauvais mot de passe

| Champ | Valeur |
|---|---|
| **ID** | CT-05 |
| **Titre** | `AuthService.LoginAsync` lève `UnauthorizedAccessException` |
| **Niveau** | Tests unitaires (Backend) |
| **Type** | Fonctionnel |
| **Technique** | Boîte blanche — Test de branche conditionnelle (validation BCrypt) |
| **Motivation** | Vérifier le comportement exact du service face à un mot de passe invalide |
| **Fichier** | `BiProject.Tests/AuthServiceTests.cs` |
| **Prérequis** | DB InMemory avec un user existant |
| **Données de test** | user=`testuser2`, pwd=`BadPassword` |
| **Étapes** | 1. Créer un user en DB InMemory 2. Appeler `LoginAsync` avec un mauvais mot de passe |
| **Résultat attendu** | Lève `UnauthorizedAccessException` avec message "Données d'authentification invalides." |
| **Résultat réel** | Exception `UnauthorizedAccessException` interceptée |
| **Statut** | ✅ Pass |

---

## CT-06 : AuthService lève une exception pour un utilisateur inconnu

| Champ | Valeur |
|---|---|
| **ID** | CT-06 |
| **Titre** | `AuthService.LoginAsync` rejette un username inexistant |
| **Niveau** | Tests unitaires (Backend) |
| **Type** | Fonctionnel |
| **Technique** | Boîte blanche — Test de branche (user non trouvé en DB) |
| **Motivation** | Éviter l'Account Enumeration : l'application ne doit pas distinguer "user inconnu" de "mauvais mdp" |
| **Fichier** | `BiProject.Tests/AuthServiceTests.cs` |
| **Prérequis** | DB InMemory vide |
| **Données de test** | user=`unknownuser`, pwd=`Any` |
| **Étapes** | 1. Appeler `LoginAsync` avec un username inconnu |
| **Résultat attendu** | Lève `UnauthorizedAccessException` — même message qu'un mauvais mot de passe |
| **Résultat réel** | Exception `UnauthorizedAccessException` capturée (message correct) |
| **Statut** | ✅ Pass |

---

## CT-07 : GET /api/orders sans token retourne 401 (Non-Fonctionnel Sécurité)

| Champ | Valeur |
|---|---|
| **ID** | CT-07 |
| **Titre** | Endpoint `/api/orders` rejette les requêtes non authentifiées |
| **Niveau** | Tests d’intégration (Backend) |
| **Type** | Non fonctionnel — Sécurité |
| **Technique** | Boîte noire — Test de contrôle d'accès |
| **Motivation** | Vérifier que la configuration ASP.NET `[Authorize]` fonctionne au niveau HTTP réel, pas seulement en code |
| **Fichier** | `BiProject.Tests/OrderIntegrationTests.cs` |
| **Prérequis** | API démarrée via `WebApplicationFactory` |
| **Données de test** | Request: `GET /api/orders` sans Header Token |
| **Étapes** | 1. Envoyer `GET /api/orders` sans header Authorization |
| **Résultat attendu** | HTTP `401 Unauthorized` |
| **Résultat réel** | Code `401 Unauthorized` reçu |
| **Statut** | ✅ Pass |

---

## CT-08 : POST /api/orders sans token retourne 401

| Champ | Valeur |
|---|---|
| **ID** | CT-08 |
| **Titre** | Création de commande bloquée sans authentification |
| **Niveau** | Tests d’intégration (Backend) |
| **Type** | Non fonctionnel — Sécurité |
| **Technique** | Boîte noire — Test de contrôle d'accès |
| **Motivation** | Valider la protection des opérations d'écriture sur les ressources métier |
| **Fichier** | `BiProject.Tests/OrderIntegrationTests.cs` |
| **Prérequis** | API démarrée via `WebApplicationFactory` |
| **Données de test** | Request: `POST /api/orders` sans Header Token |
| **Étapes** | 1. Envoyer `POST /api/orders` avec un body JSON sans header Authorization |
| **Résultat attendu** | HTTP `401 Unauthorized` |
| **Résultat réel** | Code `401 Unauthorized` reçu |
| **Statut** | ✅ Pass |

---

## CT-09 : POST /api/products sans token retourne 401

| Champ | Valeur |
|---|---|
| **ID** | CT-09 |
| **Titre** | Création de produit bloquée sans authentification |
| **Niveau** | Tests d’intégration (Backend) |
| **Type** | Non fonctionnel — Sécurité |
| **Technique** | Boîte noire — Test de contrôle d'accès |
| **Motivation** | Valider la protection Admin sur les ressources de catalogue produit |
| **Fichier** | `BiProject.Tests/OrderIntegrationTests.cs` |
| **Prérequis** | API démarrée via `WebApplicationFactory` |
| **Données de test** | Request: `POST /api/products` sans Header Token |
| **Étapes** | 1. Envoyer `POST /api/products` sans header Authorization |
| **Résultat attendu** | HTTP `401 Unauthorized` |
| **Résultat réel** | Code `401 Unauthorized` reçu |
| **Statut** | ✅ Pass |

---

## CT-10 : Inscription réussie d’un nouvel utilisateur

| Champ | Valeur |
|---|---|
| **ID** | CT-10 |
| **Titre** | Création d'un compte utilisateur via le formulaire public |
| **Niveau** | Tests système (E2E) |
| **Type** | Fonctionnel |
| **Technique** | Boîte noire — test de flux nominal |
| **Motivation** | Valider que le parcours d'inscription publique est opérationnel et crée bien une entrée en base |
| **Fichier** | `tests_selenium/tests/test_register.py` |
| **Prérequis** | Frontend sur :4200, Backend sur :5120 |
| **Données de test** | user=`new_user_123`, email=`new@example.com`, pwd=`Password123!` |
| **Étapes** | 1. Naviguer vers `/register` 2. Remplir tous les champs 3. Cliquer sur Register |
| **Résultat attendu** | Redirection vers `/products` après création réussie du compte |
| **Résultat réel** | Redirection vers `/products` confirmée |
| **Statut** | ✅ Pass |

---

## CT-11 : Inscription refusée — identifiant déjà existant

| Champ | Valeur |
|---|---|
| **ID** | CT-11 |
| **Titre** | Rejet d'inscription pour un username ou email déjà utilisé |
| **Niveau** | Tests système (E2E) |
| **Type** | Fonctionnel |
| **Technique** | Boîte noire — test de robustesse |
| **Motivation** | Garantir l'unicité des comptes et la clarté des messages d'erreur pour l'utilisateur |
| **Fichier** | `tests_selenium/tests/test_register.py` |
| **Prérequis** | Compte `test_user` déjà existant en base |
| **Données de test** | user=`test_user`, email=`test@example.com` |
| **Étapes** | 1. Naviguer vers `/register` 2. Tenter l'inscription avec les identifiants existants |
| **Résultat attendu** | Affichage d'un message d'erreur et maintien sur la page `/register` |
| **Résultat réel** | Inscription refusée avec message conforme ("Cet utilisateur existe déjà.") |
| **Statut** | ✅ Pass |

---

## CT-12 : Consultation de la liste des produits

| Champ | Valeur |
|---|---|
| **ID** | CT-12 |
| **Titre** | Affichage du catalogue complet pour un utilisateur authentifié |
| **Niveau** | Tests système (E2E) |
| **Type** | Fonctionnel |
| **Technique** | Boîte noire |
| **Motivation** | Vérifier que le composant `ProductsComponent` récupère et affiche les données de l'API |
| **Fichier** | `tests_selenium/tests/test_catalog.py` |
| **Prérequis** | Utilisateur connecté, produits présents en base |
| **Données de test** | Credentials valides |
| **Étapes** | 1. Se connecter 2. Naviguer vers `/products` |
| **Résultat attendu** | Affichage d'une liste de produits non vide (grid/table) |
| **Résultat réel** | Liste des produits visible après connexion |
| **Statut** | ✅ Pass |

---

## CT-13 : Consultation du détail d’un produit existant

| Champ | Valeur |
|---|---|
| **ID** | CT-13 |
| **Titre** | Récupération des détails d'un produit (Backend) |
| **Niveau** | Tests d’intégration (Backend) |
| **Type** | Fonctionnel |
| **Technique** | Boîte noire |
| **Motivation** | Valider l'endpoint API unitaire (le frontend ne dispose pas encore de page détail dédiée) |
| **Fichier prévu** | `BiProject.Tests/ProductIntegrationTests.cs` |
| **Prérequis** | Produit ID=1 présent dans la base de données de test |
| **Données de test** | Request: `GET /api/products/1` avec Token valide |
| **Étapes** | 1. Envoyer une requête GET pour un ID de produit valide |
| **Résultat attendu** | HTTP `200 OK` avec le JSON complet du produit |
| **Résultat réel** | Non encore exécuté |
| **Statut** | ⏳ Not Executed |

---

## CT-14 : Blocage de création de produit (Non-Admin)

| Champ | Valeur |
|---|---|
| **ID** | CT-14 |
| **Titre** | Interdiction de création de ressource pour le rôle 'User' |
| **Niveau** | Tests d’intégration (Backend) |
| **Type** | Non fonctionnel — Sécurité |
| **Technique** | Boîte noire — Test de privilèges |
| **Motivation** | Vérifier que la restriction `[Authorize(Roles = "Admin")]` est effective au niveau de l'API |
| **Fichier** | `BiProject.Tests/SecurityIntegrationTests.cs` |
| **Prérequis** | JWT Token valide avec le rôle 'User' |
| **Données de test** | Request: `POST /api/products` |
| **Étapes** | 1. S'authentifier en tant que simple utilisateur 2. Tenter de créer un produit |
| **Résultat attendu** | HTTP `403 Forbidden` |
| **Résultat réel** | Accès bloqué avec HTTP 403 Forbidden |
| **Statut** | ✅ Pass |

---

## CT-15 : Création d’un produit par un administrateur

| Champ | Valeur |
|---|---|
| **ID** | CT-15 |
| **Titre** | Succès de création d'une ressource par un compte Admin |
| **Niveau** | Tests d’intégration (Backend) |
| **Type** | Fonctionnel |
| **Technique** | Boîte noire |
| **Motivation** | Valider le flux métier d'administration complète du catalogue produit |
| **Fichier prévu** | `BiProject.Tests/AdminIntegrationTests.cs` |
| **Prérequis** | JWT Token valide avec le rôle 'Admin' |
| **Données de test** | JSON `CreateProductDto` valide |
| **Étapes** | 1. S'authentifier en Admin 2. Envoyer une requête POST de création |
| **Résultat attendu** | HTTP `201 Created` avec la ressource créée |
| **Résultat réel** | Non encore exécuté |
| **Statut** | ⏳ Not Executed |

---

## CT-BUG-01 [SÉCURITÉ] : Démonstration de vulnérabilité IDOR

| Champ | Valeur |
|---|---|
| **ID** | CT-BUG-01 |
| **Titre** | Accès non autorisé à une commande d'un autre utilisateur (IDOR) |
| **Niveau** | Tests d’intégration (Backend) |
| **Type** | Non fonctionnel — Sécurité |
| **Technique** | Boîte noire — Test de contrôle d'accès |
| **Motivation** | Démontrer une vulnérabilité IDOR (Insecure Direct Object Reference) avant correction |
| **Fichier** | `BiProject.Tests/VulnerabilityExploitTests.cs` |
| **Prérequis** | Commande ID=10 (UserA). Token valide pour UserB. |
| **Données de test** | Request: `GET /api/orders/10` avec Token UserB |
| **Étapes** | 1. Tenter d'accéder à l'ID d'une commande d'un autre utilisateur |
| **Résultat attendu** | HTTP `403 Forbidden` ou `404 Not Found` (Sécurité attendue) |
| **Résultat réel** | HTTP `200 OK` (Échec : la propriété de sécurité attendue n'est pas respectée) |
| **Statut** | ❌ Fail (Bug IDOR confirmé par échec de validation) |

---

## CT-16 : Consultation de mes commandes

| Champ | Valeur |
|---|---|
| **ID** | CT-16 |
| **Titre** | Affichage filtré des transactions de l'utilisateur connecté |
| **Niveau** | Tests système (E2E) |
| **Type** | Fonctionnel |
| **Technique** | Boîte noire |
| **Motivation** | Vérifier que le routage `/my-orders` affiche correctement les données filtrées par l'API |
| **Fichier prévu** | `tests_selenium/tests/test_orders.py` |
| **Prérequis** | L'utilisateur `test_user` dispose de commandes en base |
| **Données de test** | Credentials `test_user` |
| **Étapes** | 1. Se connecter 2. Accéder à l'onglet "Mes Commandes" |
| **Résultat attendu** | Affichage de la liste des commandes appartenant à l'utilisateur |
| **Résultat réel** | Non encore exécuté |
| **Statut** | ⏳ Not Executed |

---

## CT-17 : Création d’une commande valide

| Champ | Valeur |
|---|---|
| **ID** | CT-17 |
| **Titre** | Validation du processus de création de commande (Checkout) |
| **Niveau** | Tests système (E2E) |
| **Type** | Fonctionnel |
| **Technique** | Boîte noire |
| **Motivation** | Valider l'intégration complète entre le frontend et l'API de commande |
| **Fichier prévu** | `tests_selenium/tests/test_orders.py` |
| **Prérequis** | Utilisateur connecté, produits sélectionnés |
| **Données de test** | Panier avec IDs de produits valides |
| **Étapes** | 1. Sélectionner un produit 2. Confirmer la commande |
| **Résultat attendu** | Confirmation de commande et décrémentation/enregistrement en base |
| **Résultat réel** | Non encore exécuté |
| **Statut** | ⏳ Not Executed |

---

## CT-18 : Isolation des commandes (Sécurité anti-IDOR)

| Champ | Valeur |
|---|---|
| **ID** | CT-18 |
| **Titre** | Blocage de l'accès à une ressource appartenant à autrui |
| **Niveau** | Tests d’intégration (Backend) |
| **Type** | Non fonctionnel — Sécurité |
| **Technique** | Boîte noire — test de contrôle d'accès |
| **Motivation** | Vérifier l'absence de faille IDOR (Insecure Direct Object Reference) |
| **Fichier prévu** | `BiProject.Tests/SecurityIntegrationTests.cs` |
| **Prérequis** | Commande ID=10 (UserA). Token valide pour UserB. |
| **Données de test** | Request: `GET /api/orders/10` avec Token UserB |
| **Étapes** | 1. Tenter d'accéder à l'ID d'une commande d'un autre utilisateur |
| **Résultat attendu** | HTTP `403 Forbidden` ou `404 Not Found` (selon politique de sécurité) |
| **Résultat réel** | Non encore exécuté |
| **Statut** | ⏳ Not Executed |

---

## CT-19 : Limitation des privilèges Analytics (BI)

| Champ | Valeur |
|---|---|
| **ID** | CT-19 |
| **Titre** | Blocage de l'accès aux données de Business Intelligence pour le rôle 'User' |
| **Niveau** | Tests d’intégration (Backend) |
| **Type** | Non fonctionnel — Sécurité |
| **Technique** | Boîte noire — Test de privilèges |
| **Motivation** | Vérifier la protection des données stratégiques (Analytics) au niveau API |
| **Fichier prévu** | `BiProject.Tests/SecurityIntegrationTests.cs` |
| **Prérequis** | JWT Token avec le rôle 'User' |
| **Données de test** | Request: `GET /api/analytics/kpis` |
| **Étapes** | 1. Authentifier un compte non-admin 2. Appeler l'endpoint analytique |
| **Résultat attendu** | HTTP `403 Forbidden` |
| **Résultat réel** | Non encore exécuté |
| **Statut** | ⏳ Not Executed |
