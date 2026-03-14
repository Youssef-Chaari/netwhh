# Catalogue des Cas de Test — BiProject QA

**Gestion Documentaire :**
- **Projet :** BiProject
- **Version :** 1.1 (Final)
- **Créé par :** Youssef Chaari (avec assistance IA Antigravity/Google Deepmind)
- **Revue par :** Youssef Chaari
- **Date de création :** 2026-03-12
- **Dernière mise à jour :** 2026-03-14

---

### CT-01 : Connexion réussie avec identifiants valides (User)

#### 1. Identification
- **ID Cas de test** : CT-01
- **Titre Cas de test** : Connexion réussie avec identifiants valides (User)
- **Créé par** : Youssef Chaari
- **Revue par** : Youssef Chaari
- **Version** : 1.1
- **Nom du testeur** : Youssef Chaari
- **Date de test** : 2026-03-14
- **Cas de test** : ✅ Pass

#### 2. Complément Méthodologique
- **Niveau** : Tests système (E2E) — Selenium + POM
- **Type** : Fonctionnel
- **Technique** : Boîte noire — Partition d'équivalence (classe valide)
- **Motivation** : On ne connaît pas le code interne Angular/API, on teste uniquement le comportement visible depuis le navigateur
- **Fichier** : `tests_selenium/tests/test_login.py`

#### 3. Prérequis
| # | Description |
|---|---|
| 1 | Backend sur :5120 |
| 2 | Frontend sur :4200 |
| 3 | compte utilisateur existant |

#### 4. Jeu de données de test
| # | Donnée | Valeur |
|---|---|---|
| 1 | Identifiants | `test_user` / `TestPassword123!` |

#### 5. Scénario de test
Valider l'accès à l'application pour un utilisateur membre via la mire de connexion nominale.

#### 6. Étapes
| Etape # | Étapes | Résultats Attendus | Résultats Réels | Pass / Fail / Blocked |
|---|---|---|---|---|
| 1 | Naviguer vers `/login` | Affichage du formulaire de connexion | Formulaire affiché | Pass |
| 2 | Saisir username et password | Champs renseignés correctement | Saisie effectuée | Pass |
| 3 | Cliquer sur le bouton 'Submit' | Redirection vers le tableau de bord (`/products`) | Redirection vers `/products` confirmée | Pass |

---

### CT-02 : Rejet d'une connexion avec mot de passe invalide

#### 1. Identification
- **ID Cas de test** : CT-02
- **Titre Cas de test** : Rejet d'une connexion avec mot de passe invalide
- **Créé par** : Youssef Chaari
- **Revue par** : Youssef Chaari
- **Version** : 1.1
- **Nom du testeur** : Youssef Chaari
- **Date de test** : 2026-03-14
- **Cas de test** : ✅ Pass

#### 2. Complément Méthodologique
- **Niveau** : Système (E2E)
- **Type** : Fonctionnel
- **Technique** : Boîte noire — Partition d'équivalence (classe invalide)
- **Motivation** : Valider le comportement de l'application face à un input invalide, sans accès au code source
- **Fichier** : `tests_selenium/tests/test_login.py`

#### 3. Prérequis
| # | Description |
|---|---|
| 1 | Backend sur :5120 |
| 2 | Frontend sur :4200 |

#### 4. Jeu de données de test
| # | Donnée | Valeur |
|---|---|---|
| 1 | Identifiants erronés | `test_user` / `MauvaisMotDePasse999!` |

#### 5. Scénario de test
Vérifier que le système interdit l'accès et informe l'utilisateur en cas de mot de passe erroné.

#### 6. Étapes
| Etape # | Étapes | Résultats Attendus | Résultats Réels | Pass / Fail / Blocked |
|---|---|---|---|---|
| 1 | Naviguer vers `/login` | Accès à la page de connexion | Page chargée | Pass |
| 2 | Saisir un mot de passe invalide | Caractères masqués dans le champ | Saisie OK | Pass |
| 3 | Cliquer sur 'Submit' | Maintien sur la page et affichage d'un message d'erreur | Erreur affichée, pas de redirection | Pass |

---

### CT-03 : L’AuthGuard Angular bloque l’accès direct à `/products`

#### 1. Identification
- **ID Cas de test** : CT-03
- **Titre Cas de test** : L’AuthGuard Angular bloque l’accès direct à `/products`
- **Créé par** : Youssef Chaari
- **Revue par** : Youssef Chaari
- **Version** : 1.1
- **Nom du testeur** : Youssef Chaari
- **Date de test** : 2026-03-14
- **Cas de test** : ✅ Pass

#### 2. Complément Méthodologique
- **Niveau** : Tests système (E2E)
- **Type** : Non fonctionnel — Sécurité
- **Technique** : Boîte noire — test de contrôle d’accès
- **Motivation** : Vérifier que le mécanisme de sécurité côté client bloque l’accès à une route protégée sans authentification, sans dépendre de l’implémentation interne
- **Fichier** : `tests_selenium/tests/test_login.py`

#### 3. Prérequis
| # | Description |
|---|---|
| 1 | Frontend sur :4200 |
| 2 | aucun token en session |

#### 4. Jeu de données de test
| # | Donnée | Valeur |
|---|---|---|
| 1 | URL protégée | `http://localhost:4200/products` |

#### 5. Scénario de test
S'assurer qu'un utilisateur non authentifié est intercepté par le Guard et redirigé vers la connexion s'il tente d'accéder à une page sensible.

#### 6. Étapes
| Etape # | Étapes | Résultats Attendus | Résultats Réels | Pass / Fail / Blocked |
|---|---|---|---|---|
| 1 | Accès direct à l'URL `/products` | Redirection automatique vers `/login` | Redirection vers `/login` effectuée | Pass |

---

### CT-04 : `AuthService.LoginAsync` retourne un token JWT

#### 1. Identification
- **ID Cas de test** : CT-04
- **Titre Cas de test** : `AuthService.LoginAsync` retourne un token JWT
- **Créé par** : Youssef Chaari
- **Revue par** : Youssef Chaari
- **Version** : 1.1
- **Nom du testeur** : Youssef Chaari
- **Date de test** : 2026-03-14
- **Cas de test** : ✅ Pass

#### 2. Complément Méthodologique
- **Niveau** : Tests unitaires (Backend)
- **Type** : Fonctionnel
- **Technique** : Boîte blanche — Test de flux interne du service
- **Motivation** : On connaît la structure interne du service, on cherche à valider la logique sans dépendances réelles (DB InMemory)
- **Fichier** : `BiProject.Tests/AuthServiceTests.cs`

#### 3. Prérequis
| # | Description |
|---|---|
| 1 | DB InMemory avec un user Role=User |
| 2 | mot de passe haché BCrypt |

#### 4. Jeu de données de test
| # | Donnée | Valeur |
|---|---|---|
| 1 | Credentials API | `user=testuser`, `pwd=GoodPassword123!` |

#### 5. Scénario de test
Valider que la couche service génère un jeton d'authentification valide pour des identifiants corrects.

#### 6. Étapes
| Etape # | Étapes | Résultats Attendus | Résultats Réels | Pass / Fail / Blocked |
|---|---|---|---|---|
| 1 | Préparer l'utilisateur en mémoire | Utilisateur présent dans le contexte de test | Enregistré | Pass |
| 2 | Appeler la méthode `LoginAsync` | Objet `AuthResponseDto` retourné avec un Token non vide | Token JWT reçu | Pass |

---

### CT-05 : `AuthService.LoginAsync` lève `UnauthorizedAccessException`

#### 1. Identification
- **ID Cas de test** : CT-05
- **Titre Cas de test** : `AuthService.LoginAsync` lève `UnauthorizedAccessException`
- **Créé par** : Youssef Chaari
- **Revue par** : Youssef Chaari
- **Version** : 1.1
- **Nom du testeur** : Youssef Chaari
- **Date de test** : 2026-03-14
- **Cas de test** : ✅ Pass

#### 2. Complément Méthodologique
- **Niveau** : Tests unitaires (Backend)
- **Type** : Fonctionnel
- **Technique** : Boîte blanche — Test de branche conditionnelle (validation BCrypt)
- **Motivation** : Vérifier le comportement exact du service face à un mot de passe invalide
- **Fichier** : `BiProject.Tests/AuthServiceTests.cs`

#### 3. Prérequis
| # | Description |
|---|---|
| 1 | DB InMemory avec un user existant |

#### 4. Jeu de données de test
| # | Donnée | Valeur |
|---|---|---|
| 1 | Identifiants Backend | `user=testuser2`, `pwd=BadPassword` |

#### 5. Scénario de test
Vérifier que le service lève une exception sécurisée en cas de discordance du mot de passe.

#### 6. Étapes
| Etape # | Étapes | Résultats Attendus | Résultats Réels | Pass / Fail / Blocked |
|---|---|---|---|---|
| 1 | Simuler un utilisateur existant | Contexte prêt | Prêt | Pass |
| 2 | Appeler `LoginAsync` avec mot de passe incorrect | Levée d'une `UnauthorizedAccessException` | Exception interceptée avec succès | Pass |

---

### CT-06 : `AuthService.LoginAsync` rejette un username inexistant

#### 1. Identification
- **ID Cas de test** : CT-06
- **Titre Cas de test** : `AuthService.LoginAsync` rejette un username inexistant
- **Créé par** : Youssef Chaari
- **Revue par** : Youssef Chaari
- **Version** : 1.1
- **Nom du testeur** : Youssef Chaari
- **Date de test** : 2026-03-14
- **Cas de test** : ✅ Pass

#### 2. Complément Méthodologique
- **Niveau** : Tests unitaires (Backend)
- **Type** : Fonctionnel
- **Technique** : Boîte blanche — Test de branche (user non trouvé en DB)
- **Motivation** : Éviter l'Account Enumeration : l'application ne doit pas distinguer "user inconnu" de "mauvais mdp"
- **Fichier** : `BiProject.Tests/AuthServiceTests.cs`

#### 3. Prérequis
| # | Description |
|---|---|
| 1 | DB InMemory vide |

#### 4. Jeu de données de test
| # | Donnée | Valeur |
|---|---|---|
| 1 | User inconnu | `user=unknownuser`, `pwd=Any` |

#### 5. Scénario de test
Confirmer que le service rejette uniformément les utilisateurs non référencés pour prévenir l'énumération de comptes.

#### 6. Étapes
| Etape # | Étapes | Résultats Attendus | Résultats Réels | Pass / Fail / Blocked |
|---|---|---|---|---|
| 1 | Appeler `LoginAsync` sur base vide | Levée d'une `UnauthorizedAccessException` | Message d'erreur identique au mauvais mot de passe | Pass |

---

### CT-07 : Endpoint `/api/orders` rejette les requêtes non authentifiées

#### 1. Identification
- **ID Cas de test** : CT-07
- **Titre Cas de test** : Endpoint `/api/orders` rejette les requêtes non authentifiées
- **Créé par** : Youssef Chaari
- **Revue par** : Youssef Chaari
- **Version** : 1.1
- **Nom du testeur** : Youssef Chaari
- **Date de test** : 2026-03-14
- **Cas de test** : ✅ Pass

#### 2. Complément Méthodologique
- **Niveau** : Tests d’intégration (Backend)
- **Type** : Non fonctionnel — Sécurité
- **Technique** : Boîte noire — Test de contrôle d'accès
- **Motivation** : Vérifier que la configuration ASP.NET `[Authorize]` fonctionne au niveau HTTP réel, pas seulement en code
- **Fichier** : `BiProject.Tests/OrderIntegrationTests.cs`

#### 3. Prérequis
| # | Description |
|---|---|
| 1 | API démarrée via `WebApplicationFactory` |

#### 4. Jeu de données de test
| # | Donnée | Valeur |
|---|---|---|
| 1 | Requête HTTP | `GET /api/orders` sans Header Authorization |

#### 5. Scénario de test
Valider que la protection de l'API bloque l'accès aux commandes pour tout client HTTP dépourvu de jeton.

#### 6. Étapes
| Etape # | Étapes | Résultats Attendus | Résultats Réels | Pass / Fail / Blocked |
|---|---|---|---|---|
| 1 | Envoyer une requête sans header Token | Réponse HTTP `401 Unauthorized` | Code 401 reçu | Pass |

---

### CT-08 : Création de commande bloquée sans authentification

#### 1. Identification
- **ID Cas de test** : CT-08
- **Titre Cas de test** : Création de commande bloquée sans authentification
- **Créé par** : Youssef Chaari
- **Revue par** : Youssef Chaari
- **Version** : 1.1
- **Nom du testeur** : Youssef Chaari
- **Date de test** : 2026-03-14
- **Cas de test** : ✅ Pass

#### 2. Complément Méthodologique
- **Niveau** : Tests d’intégration (Backend)
- **Type** : Non fonctionnel — Sécurité
- **Technique** : Boîte noire — Test de contrôle d'accès
- **Motivation** : Valider la protection des opérations d'écriture sur les ressources métier
- **Fichier** : `BiProject.Tests/OrderIntegrationTests.cs`

#### 3. Prérequis
| # | Description |
|---|---|
| 1 | API démarrée via `WebApplicationFactory` |

#### 4. Jeu de données de test
| # | Donnée | Valeur |
|---|---|---|
| 1 | Création Commande | `POST /api/orders` sans Jeton Auth |

#### 5. Scénario de test
Vérifier l'impossibilité de modifier l'état du système (création de commande) sans être authentifié au préalable.

#### 6. Étapes
| Etape # | Étapes | Résultats Attendus | Résultats Réels | Pass / Fail / Blocked |
|---|---|---|---|---|
| 1 | Envoyer une requête POST anonyme | Réponse HTTP `401 Unauthorized` | Code 401 reçu | Pass |

---

### CT-09 : Création de produit bloquée sans authentification

#### 1. Identification
- **ID Cas de test** : CT-09
- **Titre Cas de test** : Création de produit bloquée sans authentification
- **Créé par** : Youssef Chaari
- **Revue par** : Youssef Chaari
- **Version** : 1.1
- **Nom du testeur** : Youssef Chaari
- **Date de test** : 2026-03-14
- **Cas de test** : ✅ Pass

#### 2. Complément Méthodologique
- **Niveau** : Tests d’intégration (Backend)
- **Type** : Non fonctionnel — Sécurité
- **Technique** : Boîte noire — Test de contrôle d'accès
- **Motivation** : Valider la protection Admin sur les ressources de catalogue produit
- **Fichier** : `BiProject.Tests/OrderIntegrationTests.cs`

#### 3. Prérequis
| # | Description |
|---|---|
| 1 | API démarrée via `WebApplicationFactory` |

#### 4. Jeu de données de test
| # | Donnée | Valeur |
|---|---|---|
| 1 | Resource Produit | `POST /api/products` sans Jeton |

#### 5. Scénario de test
S'assurer que l'ajout de nouveaux produits au catalogue est protégé et interdit aux utilisateurs anonymes.

#### 6. Étapes
| Etape # | Étapes | Résultats Attendus | Résultats Réels | Pass / Fail / Blocked |
|---|---|---|---|---|
| 1 | Envoyer requête POST anonyme | Réponse HTTP `401 Unauthorized` | Code 401 reçu | Pass |

---

### CT-10 : Création d'un compte utilisateur via le formulaire public

#### 1. Identification
- **ID Cas de test** : CT-10
- **Titre Cas de test** : Création d'un compte utilisateur via le formulaire public
- **Créé par** : Youssef Chaari
- **Revue par** : Youssef Chaari
- **Version** : 1.1
- **Nom du testeur** : Youssef Chaari
- **Date de test** : 2026-03-14
- **Cas de test** : ✅ Pass

#### 2. Complément Méthodologique
- **Niveau** : Tests système (E2E)
- **Type** : Fonctionnel
- **Technique** : Boîte noire — test de flux nominal
- **Motivation** : Valider que le parcours d'inscription publique est opérationnel et crée bien une entrée en base
- **Fichier** : `tests_selenium/tests/test_register.py`

#### 3. Prérequis
| # | Description |
|---|---|
| 1 | Frontend sur :4200 |
| 2 | Backend sur :5120 |

#### 4. Jeu de données de test
| # | Donnée | Valeur |
|---|---|---|
| 1 | Nouvel inscrit | `user=new_user_123`, `email=new@example.com`, `pwd=Password123!` |

#### 5. Scénario de test
Vérifier le succès du parcours d'enrôlement d'un nouvel utilisateur depuis l'interface Web.

#### 6. Étapes
| Etape # | Étapes | Résultats Attendus | Résultats Réels | Pass / Fail / Blocked |
|---|---|---|---|---|
| 1 | Accéder à `/register` | Affichage du formulaire d'inscription | Formulaire chargé | Pass |
| 2 | Renseigner les champs du formulaire | Validation locale des champs (absence d'erreurs) | Saisie valide | Pass |
| 3 | Cliquer sur 'Register' | Création du compte et redirection vers `/products` | Redirection confirmée | Pass |

---

### CT-11 : Rejet d'inscription pour un username ou email déjà utilisé

#### 1. Identification
- **ID Cas de test** : CT-11
- **Titre Cas de test** : Rejet d'inscription pour un username ou email déjà utilisé
- **Créé par** : Youssef Chaari
- **Revue par** : Youssef Chaari
- **Version** : 1.1
- **Nom du testeur** : Youssef Chaari
- **Date de test** : 2026-03-14
- **Cas de test** : ✅ Pass

#### 2. Complément Méthodologique
- **Niveau** : Tests système (E2E)
- **Type** : Fonctionnel
- **Technique** : Boîte noire — test de robustesse
- **Motivation** : Garantir l'unicité des comptes et la clarté des messages d'erreur pour l'utilisateur
- **Fichier** : `tests_selenium/tests/test_register.py`

#### 3. Prérequis
| # | Description |
|---|---|
| 1 | Compte `test_user` déjà existant en base |

#### 4. Jeu de données de test
| # | Donnée | Valeur |
|---|---|---|
| 1 | Doublon identifiant | `user=test_user`, `email=test@example.com` |

#### 5. Scénario de test
Valider que l'application détecte et refuse proprement la création d'un compte utilisant des identifiants déjà réservés.

#### 6. Étapes
| Etape # | Étapes | Résultats Attendus | Résultats Réels | Pass / Fail / Blocked |
|---|---|---|---|---|
| 1 | Naviguer vers `/register` | Formulaire prêt | Formulaire OK | Pass |
| 2 | Tenter l'inscription avec identifiants existants | Message d'alerte spécifique ("Cet utilisateur existe déjà") | Alerte affichée | Pass |

---

### CT-12 : Affichage du catalogue complet pour un utilisateur authentifié

#### 1. Identification
- **ID Cas de test** : CT-12
- **Titre Cas de test** : Affichage du catalogue complet pour un utilisateur authentifié
- **Créé par** : Youssef Chaari
- **Revue par** : Youssef Chaari
- **Version** : 1.1
- **Nom du testeur** : Youssef Chaari
- **Date de test** : 2026-03-14
- **Cas de test** : ✅ Pass

#### 2. Complément Méthodologique
- **Niveau** : Tests système (E2E)
- **Type** : Fonctionnel
- **Technique** : Boîte noire
- **Motivation** : Vérifier que le composant `ProductsComponent` récupère et affiche les données de l'API
- **Fichier** : `tests_selenium/tests/test_catalog.py`

#### 3. Prérequis
| # | Description |
|---|---|
| 1 | Utilisateur connecté |
| 2 | produits présents en base |

#### 4. Jeu de données de test
| # | Donnée | Valeur |
|---|---|---|
| 1 | Connexion Session | `test_user` / `TestPassword123!` |

#### 5. Scénario de test
Confirmer la capacité de l'utilisateur à consulter la liste des produits après s'être identifié.

#### 6. Étapes
| Etape # | Étapes | Résultats Attendus | Résultats Réels | Pass / Fail / Blocked |
|---|---|---|---|---|
| 1 | Se connecter à l'application | Accès réussi | Connecté | Pass |
| 2 | Consulter la page `/products` | Affichage d'une grille de produits non vide | Catalogue visible | Pass |

---

### CT-13 : Récupération des détails d'un produit (Backend)

#### 1. Identification
- **ID Cas de test** : CT-13
- **Titre Cas de test** : Récupération des détails d'un produit (Backend)
- **Créé par** : Youssef Chaari
- **Revue par** : Youssef Chaari
- **Version** : 1.1
- **Nom du testeur** : Youssef Chaari
- **Date de test** : 2026-03-14
- **Cas de test** : ⏳ Not Executed

#### 2. Complément Méthodologique
- **Niveau** : Tests d’intégration (Backend)
- **Type** : Fonctionnel
- **Technique** : Boîte noire
- **Motivation** : Valider l'endpoint API unitaire (le frontend ne dispose pas encore de page détail dédiée)
- **Fichier** : `BiProject.Tests/ProductIntegrationTests.cs`

#### 3. Prérequis
| # | Description |
|---|---|
| 1 | Produit ID=1 présent dans la base de données de test |

#### 4. Jeu de données de test
| # | Donnée | Valeur |
|---|---|---|
| 1 | Consultation Unitaire | `GET /api/products/1` avec Jeton valide |

#### 5. Scénario de test
Vérifier l'API de récupération granulaire d'une fiche produit.

#### 6. Étapes
| Etape # | Étapes | Résultats Attendus | Résultats Réels | Pass / Fail / Blocked |
|---|---|---|---|---|
| 1 | Envoyer une requête GET pour un ID | HTTP `200 OK` avec JSON détaillé | Non encore exécuté | Not Executed |

---

### CT-14 : Interdiction de création de produit pour le rôle 'User'

#### 1. Identification
- **ID Cas de test** : CT-14
- **Titre Cas de test** : Interdiction de création de produit pour le rôle 'User'
- **Créé par** : Youssef Chaari
- **Revue par** : Youssef Chaari
- **Version** : 1.1
- **Nom du testeur** : Youssef Chaari
- **Date de test** : 2026-03-14
- **Cas de test** : ✅ Pass

#### 2. Complément Méthodologique
- **Niveau** : Tests d’intégration (Backend)
- **Type** : Non fonctionnel — Sécurité
- **Technique** : Boîte noire — Test de privilèges
- **Motivation** : Vérifier que la restriction `[Authorize(Roles = "Admin")]` est effective au niveau de l'API
- **Fichier** : `BiProject.Tests/SecurityIntegrationTests.cs`

#### 3. Prérequis
| # | Description |
|---|---|
| 1 | JWT Token valide avec le rôle 'User' |

#### 4. Jeu de données de test
| # | Donnée | Valeur |
|---|---|---|
| 1 | Action interdite | `POST /api/products` (Tentative de fraude) |

#### 5. Scénario de test
Valider le moteur de permissions en s'assurant qu'un utilisateur standard ne peut pas altérer le catalogue.

#### 6. Étapes
| Etape # | Étapes | Résultats Attendus | Résultats Réels | Pass / Fail / Blocked |
|---|---|---|---|---|
| 1 | Porter un Token 'User' | Authentification reconnue | Authentifié | Pass |
| 2 | Tenter une création POST | Réponse HTTP `403 Forbidden` | Accès refusé par l'API | Pass |

---

### CT-15 : Succès de création d'une ressource par un compte Admin

#### 1. Identification
- **ID Cas de test** : CT-15
- **Titre Cas de test** : Succès de création d'une ressource par un compte Admin
- **Créé par** : Youssef Chaari
- **Revue par** : Youssef Chaari
- **Version** : 1.1
- **Nom du testeur** : Youssef Chaari
- **Date de test** : 2026-03-14
- **Cas de test** : ⏳ Not Executed

#### 2. Complément Méthodologique
- **Niveau** : Tests d’intégration (Backend)
- **Type** : Fonctionnel
- **Technique** : Boîte noire
- **Motivation** : Valider le flux métier d'administration complète du catalogue produit
- **Fichier** : `BiProject.Tests/AdminIntegrationTests.cs`

#### 3. Prérequis
| # | Description |
|---|---|
| 1 | JWT Token valide avec le rôle 'Admin' |

#### 4. Jeu de données de test
| # | Donnée | Valeur |
|---|---|---|
| 1 | Payload Admin | DTO `CreateProduct` complet |

#### 5. Scénario de test
Valider que les comptes privilégiés peuvent administrer le catalogue de produits avec succès.

#### 6. Étapes
| Etape # | Étapes | Résultats Attendus | Résultats Réels | Pass / Fail / Blocked |
|---|---|---|---|---|
| 1 | Authentification Admin | Jeton obtenu | N/A | Not Executed |
| 2 | Envoi du POST de création | HTTP `201 Created` | N/A | Not Executed |

---

### CT-BUG-01 : Validation du cloisonnement des données de commande (Contrôle IDOR)

#### 1. Identification
- **ID Cas de test** : CT-BUG-01
- **Titre Cas de test** : Validation du cloisonnement des données de commande (Contrôle IDOR)
- **Créé par** : Youssef Chaari
- **Revue par** : Youssef Chaari
- **Version** : 1.1
- **Nom du testeur** : Youssef Chaari
- **Date de test** : 2026-03-14
- **Cas de test** : ❌ Fail (Bug IDOR confirmé par échec de validation)

#### 2. Complément Méthodologique
- **Niveau** : Tests d’intégration (Backend)
- **Type** : Non fonctionnel — Sécurité
- **Technique** : Boîte noire — Test de contrôle d'accès
- **Motivation** : Démontrer une vulnérabilité IDOR (Insecure Direct Object Reference) avant correction
- **Fichier** : `BiProject.Tests/VulnerabilityExploitTests.cs`

#### 3. Prérequis
| # | Description |
|---|---|
| 1 | Commande ID=10 (UserA). Token valide pour UserB. |

#### 4. Jeu de données de test
| # | Donnée | Valeur |
|---|---|---|
| 1 | Attaque IDOR | `GET /api/orders/10` avec Jeton UserB |

#### 5. Scénario de test
Vérifier que le système interdit la lecture d'une commande appartenant à un tiers via son identifiant direct (cloisonnement strict).

#### 6. Étapes
| Etape # | Étapes | Résultats Attendus | Résultats Réels | Pass / Fail / Blocked |
|---|---|---|---|---|
| 1 | Tentative d'accès à la ressource d'un tiers | HTTP `403 Forbidden` attendu (Isolation) | HTTP `200 OK` (Accès illégitime constaté) | Fail |

---

### CT-16 : Affichage filtré des transactions de l'utilisateur connecté

#### 1. Identification
- **ID Cas de test** : CT-16
- **Titre Cas de test** : Affichage filtré des transactions de l'utilisateur connecté
- **Créé par** : Youssef Chaari
- **Revue par** : Youssef Chaari
- **Version** : 1.1
- **Nom du testeur** : Youssef Chaari
- **Date de test** : 2026-03-14
- **Cas de test** : ⏳ Not Executed

#### 2. Complément Méthodologique
- **Niveau** : Tests système (E2E)
- **Type** : Fonctionnel
- **Technique** : Boîte noire
- **Motivation** : Vérifier que le routage `/my-orders` affiche correctement les données filtrées par l'API
- **Fichier** : `tests_selenium/tests/test_orders.py`

#### 3. Prérequis
| # | Description |
|---|---|
| 1 | L'utilisateur `test_user` dispose de commandes en base |

#### 4. Jeu de données de test
| # | Donnée | Valeur |
|---|---|---|
| 1 | Session User | `test_user` actif |

#### 5. Scénario de test
S'assurer que l'utilisateur ne visualise que son historique de commandes personnel.

#### 6. Étapes
| Etape # | Étapes | Résultats Attendus | Résultats Réels | Pass / Fail / Blocked |
|---|---|---|---|---|
| 1 | Navigation vers 'Mes Commandes' | Liste des commandes de l'utilisateur uniquement | N/A | Not Executed |

---

### CT-17 : Validation du processus de création de commande (Checkout)

#### 1. Identification
- **ID Cas de test** : CT-17
- **Titre Cas de test** : Validation du processus de création de commande (Checkout)
- **Créé par** : Youssef Chaari
- **Revue par** : Youssef Chaari
- **Version** : 1.1
- **Nom du testeur** : Youssef Chaari
- **Date de test** : 2026-03-14
- **Cas de test** : ⏳ Not Executed

#### 2. Complément Méthodologique
- **Niveau** : Tests système (E2E)
- **Type** : Fonctionnel
- **Technique** : Boîte noire
- **Motivation** : Valider l'intégration complète entre le frontend et l'API de commande
- **Fichier** : `tests_selenium/tests/test_orders.py`

#### 3. Prérequis
| # | Description |
|---|---|
| 1 | Utilisateur connecté |
| 2 | produits sélectionnés |

#### 4. Jeu de données de test
| # | Donnée | Valeur |
|---|---|---|
| 1 | Workflow d'achat | Panier garni d'identifiants valides |

#### 5. Scénario de test
Valider la chaîne complète d'achat depuis la sélection jusqu'à l'enregistrement en base de données.

#### 6. Étapes
| Etape # | Étapes | Résultats Attendus | Résultats Réels | Pass / Fail / Blocked |
|---|---|---|---|---|
| 1 | Confirmation du panier | Page de succès et mise à jour de l'historique | N/A | Not Executed |

---

### CT-18 : Isolation des commandes (Sécurité anti-IDOR)

#### 1. Identification
- **ID Cas de test** : CT-18
- **Titre Cas de test** : Isolation des commandes (Sécurité anti-IDOR)
- **Créé par** : Youssef Chaari
- **Revue par** : Youssef Chaari
- **Version** : 1.1
- **Nom du testeur** : Youssef Chaari
- **Date de test** : 2026-03-14
- **Cas de test** : ⏳ Not Executed

#### 2. Complément Méthodologique
- **Niveau** : Tests d’intégration (Backend)
- **Type** : Non fonctionnel — Sécurité
- **Technique** : Boîte noire — test de contrôle d'accès
- **Motivation** : Vérifier l'absence de faille IDOR (Insecure Direct Object Reference)
- **Fichier** : `BiProject.Tests/SecurityIntegrationTests.cs`

#### 3. Prérequis
| # | Description |
|---|---|
| 1 | Commande ID=10 (UserA). Token valide pour UserB. |

#### 4. Jeu de données de test
| # | Donnée | Valeur |
|---|---|---|
| 1 | Sonde Sécurité | `GET /api/orders/10` (Target tiers) |

#### 5. Scénario de test
Validation "post-corrective" de l'étanchéité des ressources utilisateur.

#### 6. Étapes
| Etape # | Étapes | Résultats Attendus | Résultats Réels | Pass / Fail / Blocked |
|---|---|---|---|---|
| 1 | Lecture resource tiers | HTTP `403 Forbidden` | N/A | Not Executed |

---

### CT-19 : Limitation des privilèges Analytics (BI)

#### 1. Identification
- **ID Cas de test** : CT-19
- **Titre Cas de test** : Limitation des privilèges Analytics (BI)
- **Créé par** : Youssef Chaari
- **Revue par** : Youssef Chaari
- **Version** : 1.1
- **Nom du testeur** : Youssef Chaari
- **Date de test** : 2026-03-14
- **Cas de test** : ⏳ Not Executed

#### 2. Complément Méthodologique
- **Niveau** : Tests d’intégration (Backend)
- **Type** : Non fonctionnel — Sécurité
- **Technique** : Boîte noire — Test de privilèges
- **Motivation** : Vérifier la protection des données stratégiques (Analytics) au niveau API
- **Fichier** : `BiProject.Tests/SecurityIntegrationTests.cs`

#### 3. Prérequis
| # | Description |
|---|---|
| 1 | JWT Token avec le rôle 'User' |

#### 4. Jeu de données de test
| # | Donnée | Valeur |
|---|---|---|
| 1 | Accès interdit | `GET /api/analytics/kpis` |

#### 5. Scénario de test
Garantir que les statistiques métier confidentielles ne sont accessibles qu'aux administrateurs.

#### 6. Étapes
| Etape # | Étapes | Résultats Attendus | Résultats Réels | Pass / Fail / Blocked |
|---|---|---|---|---|
| 1 | Appel Analytics en tant qu'User | HTTP `403 Forbidden` | N/A | Not Executed |
