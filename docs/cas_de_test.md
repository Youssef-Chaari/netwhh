# CATALOGUE DÉTAILLÉ DES CAS DE TEST — BIPROJECT QA

**Projet :** BiProject (Architecture .NET & Angular)  
**Auteurs :** Youssef Chaari & Mohamed Aziz Zouari  
**Date :** 15 Mars 2026  
**Version :** 3.0 (Fiches Académiques d'Audit)  

---

## 1. Introduction
Le présent document regroupe les 20 fiches descriptives des cas de test automatisés réalisés lors de l'audit de qualité logicielle du projet BiProject. Conformément aux exigences académiques, chaque fiche détaille les métadonnées de test, la méthodologie employée, les jeux de données manipulés et le déroulement technique précis des étapes.

---

## 2. Dossier des Cas de Test (CT-01 à CT-20)

---

## CT-01 : Connexion réussie avec identifiants valides

### 1. Identification
| Champ | Valeur |
| :--- | :--- |
| **ID Cas de test** | CT-01 |
| **Titre Cas de test** | Connexion réussie avec identifiants valides |
| **Créé par** | Youssef Chaari |
| **Revue par** | Mohamed Aziz Zouari |
| **Version** | 1.0 |
| **Nom du testeur** | Youssef Chaari |
| **Date de test** | 15 Mars 2026 |
| **Résultat Final** | ✅ Pass |

### 2. Complément méthodologique
| Champ | Valeur |
| :--- | :--- |
| **Niveau** | Système (E2E) |
| **Type** | Fonctionnel |
| **Technique** : Boîte Noire (Partition d'équivalence valide) |
| **Motivation** | Valider l'accès sécurisé des utilisateurs nominaux à l'application. |
| **Fichier** | `tests_selenium/tests/test_login.py` |

### 3. Prérequis
| Condition | État attendu |
| :--- | :--- |
| Backend API | Actif (port 5120) |
| Frontend Angular | Actif (port 4200) |
| Base de données | Compte `test_user` injecté |

### 4. Jeu de données de test
| Paramètre | Valeur |
| :--- | :--- |
| `username` | `test_user` |
| `password` | `TestPassword123!` |

### 5. Scénario de test
L'utilisateur accède à l'interface Web, saisit ses informations d'identité et valide par une pression sur le bouton de soumission. Le système doit confirmer l'identité et rediriger l'utilisateur vers son catalogue de produits.

### 6. Étapes
| Étape # | Étapes | Résultats attendus | Résultats réels | Pass / Fail / Blocked |
| :---: | :--- | :--- | :--- | :---: |
| 1 | Naviguer vers l'URL `http://localhost:4200/login` | Page de login affichée avec les champs requis | Page affichée | ✅ Pass |
| 2 | Saisir l'identifiant `test_user` | Champ renseigné sans avertissement | Identifiant saisi | ✅ Pass |
| 3 | Saisir le mot de passe valide | Champ masqué correctement rempli | Mot de passe saisi | ✅ Pass |
| 4 | Cliquer sur "Se connecter" (submit) | Redirection vers `/products` | Redirection réussie | ✅ Pass |
| 5 | Vérifier le cookie/token de session | L'utilisateur est reconnu comme authentifié | Session active | ✅ Pass |

---

## CT-02 : Rejet d'une connexion avec mot de passe invalide

### 1. Identification
| Champ | Valeur |
| :--- | :--- |
| **ID Cas de test** | CT-02 |
| **Titre Cas de test** | Rejet d'une connexion avec mot de passe invalide |
| **Créé par** | Youssef Chaari |
| **Revue par** : Mohamed Aziz Zouari |
| **Version** | 1.0 |
| **Nom du testeur** | Youssef Chaari |
| **Date de test** | 15 Mars 2026 |
| **Résultat Final** | ✅ Pass |

### 2. Complément méthodologique
| Champ | Valeur |
| :--- | :--- |
| **Niveau** | Système (E2E) |
| **Type** | Fonctionnel |
| **Technique** | Boîte Noire (Partition d'équivalence invalide) |
| **Motivation** | Vérifier la protection contre les accès frauduleux par erreur de saisie. |
| **Fichier** | `tests_selenium/tests/test_login.py` |

### 3. Prérequis
| Condition | État attendu |
| :--- | :--- |
| Compte existant | `test_user` déjà présent dans la base OLTP |

### 4. Jeu de données de test
| Paramètre | Valeur |
| :--- | :--- |
| `username` | `test_user` |
| `password` | `MauvaisMotDePasse999!` |

### 5. Scénario de test
Tenter une authentification système avec un mot de passe connu comme erroné. L'accès doit être systématiquement refusé sans exception.

### 6. Étapes
| Étape # | Étapes | Résultats attendus | Résultats réels | Pass / Fail / Blocked |
| :---: | :--- | :--- | :--- | :---: |
| 1 | Accéder à la route `/login` | Formulaire visible | Formulaire présent | ✅ Pass |
| 2 | Saisir les identifiants erronés | Données saisies | Saisie ok | ✅ Pass |
| 3 | Soumettre le formulaire | Rester sur `/login` avec message d'erreur | Pas de redirection | ✅ Pass |
| 4 | Vérifier l'état de la redirection | L'URL actuelle reste `/login` | URL maintenue | ✅ Pass |

---

## CT-03 : Blocage de l'accès direct via AuthGuard Angular

### 1. Identification
| Champ | Valeur |
| :--- | :--- |
| **ID Cas de test** | CT-03 |
| **Titre Cas de test** | Blocage de l'accès direct via AuthGuard Angular |
| **Créé par** | Youssef Chaari |
| **Revue par** | Mohamed Aziz Zouari |
| **Version** | 1.0 |
| **Nom du testeur** | Youssef Chaari |
| **Date de test** | 15 Mars 2026 |
| **Résultat Final** | ✅ Pass |

### 2. Complément méthodologique
| Champ | Valeur |
| :--- | :--- |
| **Niveau** | Système (E2E) |
| **Type** | Sécurité |
| **Technique** | Valeur limite (Accès sans privilèges) |
| **Motivation** | Garantir que les routes client sont inaccessibles sans session. |
| **Fichier** | `tests_selenium/tests/test_login.py` |

### 3. Prérequis
| Condition | État attendu |
| :--- | :--- |
| Session locale | Aucune (Nettoyage des cookies/LocalStorage) |

### 4. Jeu de données de test
| Paramètre | Valeur |
| :--- | :--- |
| **URL Cible** | `http://localhost:4200/products` |

### 5. Scénario de test
Un attaquant ou un utilisateur non connecté tente d'accéder directement à la galerie de produits par URL. Le Guard Angular doit intercepter la navigation et rediriger vers la page de login.

### 6. Étapes
| Étape # | Étapes | Résultats attendus | Résultats réels | Pass / Fail / Blocked |
| :---: | :--- | :--- | :--- | :---: |
| 1 | Saisir l'URL `/products` dans le navigateur | Déclenchement automatique de redirection | Redirection effectuée | ✅ Pass |
| 2 | Vérifier l'URL finale | Redirection vers `/login` | URL est `/login` | ✅ Pass |
| 3 | Contrôler l'affichage | Page de connexion demandant les accès | Page de login active | ✅ Pass |

---

## CT-04 : Génération technique d'un jeton JWT valide

### 1. Identification
| Champ | Valeur |
| :--- | :--- |
| **ID Cas de test** | CT-04 |
| **Titre Cas de test** | Génération technique d'un jeton JWT valide |
| **Créé par** : Youssef Chaari |
| **Revue par** | Mohamed Aziz Zouari |
| **Version** | 1.0 |
| **Nom du testeur** | Youssef Chaari |
| **Date de test** | 15 Mars 2026 |
| **Résultat Final** | ✅ Pass |

### 2. Complément méthodologique
| Champ | Valeur |
| :--- | :--- |
| **Niveau** | Unitaire (Backend) |
| **Type** | Fonctionnel |
| **Technique** | Boîte Blanche (Code logic) |
| **Motivation** | Valider l'algorithme d'émission des titres d'identité (Security Token Service). |
| **Fichier** | `BiProject.Tests/AuthServiceTests.cs` |

### 3. Prérequis
| Condition | État attendu |
| :--- | :--- |
| Contexte de test | `DbContext` Mock (Memory) configuré |
| Configuration | Clé Secrète JWT chargée |

### 4. Jeu de données de test
| Paramètre | Valeur |
| :--- | :--- |
| `UserObject` | `testuser` (rôle 'User') |

### 5. Scénario de test
Appeler directement le service d'authentification et vérifier que le token généré respecte la structure JWT et contient les claims d'identité requis.

### 6. Étapes
| Étape # | Étapes | Résultats attendus | Résultats réels | Pass / Fail / Blocked |
| :---: | :--- | :--- | :--- | :---: |
| 1 | Initialiser le `AuthService` avec les mocks | Service prêt pour exécution | Service initialisé | ✅ Pass |
| 2 | Appeler la méthode `LoginAsync` | Retourne un `AuthResponseDto` | Objet reçu | ✅ Pass |
| 3 | Vérifier le champ `Token` | Longue chaîne encodée non vide | Jeton valide reçu | ✅ Pass |
| 4 | Vérifier le `Username` | Correspond à la saisie ("testuser") | Match effectif | ✅ Pass |

---

## CT-05 : Levée d'exception pour mot de passe erroné (Service)

### 1. Identification
| Champ | Valeur |
| :--- | :--- |
| **ID Cas de test** | CT-05 |
| **Titre Cas de test** | Levée d'exception pour mot de passe erroné (Service) |
| **Créé par** | Youssef Chaari |
| **Revue par** | Mohamed Aziz Zouari |
| **Version** | 1.0 |
| **Nom du testeur** | Youssef Chaari |
| **Date de test** | 15 Mars 2026 |
| **Résultat Final** | ✅ Pass |

### 2. Complément méthodologique
| Champ | Valeur |
| :--- | :--- |
| **Niveau** | Unitaire (Backend) |
| **Type** | Fonctionnel |
| **Technique** | Boîte Blanche |
| **Motivation** | Confirmer la gestion sécurisée des erreurs métier via des exceptions. |
| **Fichier** | `BiProject.Tests/AuthServiceTests.cs` |

### 3. Prérequis
| Condition | État attendu |
| :--- | :--- |
| `DbContext` | Contient un utilisateur `testuser2` |

### 4. Jeu de données de test
| Paramètre | Valeur |
| :--- | :--- |
| `InputPassword` | "BadPassword" |

### 5. Scénario de test
Vérifier qu'une tentative de login logic avec un mauvais secret déclenche systématiquement une `UnauthorizedAccessException` avec le message correct.

### 6. Étapes
| Étape # | Étapes | Résultats attendus | Résultats réels | Pass / Fail / Blocked |
| :---: | :--- | :--- | :--- | :---: |
| 1 | Invoquer `LoginAsync` de manière asynchrone | Capture de l'exception par le moteur de test | Exception capturée | ✅ Pass |
| 2 | Vérifier le type d'exception | Type `UnauthorizedAccessException` | Type conforme | ✅ Pass |
| 3 | Vérifier le message d'erreur | "Données d'authentification invalides." | Message identique | ✅ Pass |

---

## CT-06 : Rejet d'un utilisateur inexistant (Service)

### 1. Identification
| Champ | Valeur |
| :--- | :--- |
| **ID Cas de test** | CT-06 |
| **Titre Cas de test** | Rejet d'un utilisateur inexistant (Service) |
| **Créé par** | Youssef Chaari |
| **Revue par** | Mohamed Aziz Zouari |
| **Version** | 1.0 |
| **Nom du testeur** | Youssef Chaari |
| **Date de test** | 15 Mars 2026 |
| **Résultat Final** | ✅ Pass |

### 2. Complément méthodologique
| Champ | Valeur |
| :--- | :--- |
| **Niveau** | Unitaire (Backend) |
| **Type** | Fonctionnel |
| **Technique** | Boîte Blanche |
| **Motivation** | Empêcher toute fuite d'information sur la validité d'un compte (Time-based/Message-based). |
| **Fichier** | `BiProject.Tests/AuthServiceTests.cs` |

### 3. Prérequis
| Condition | État attendu |
| :--- | :--- |
| Base OLTP | Utilisateur `unknownuser` est absent |

### 4. Jeu de données de test
| Paramètre | Valeur |
| :--- | :--- |
| `username` | `unknownuser` |

### 5. Scénario de test
S'assurer que le service retourne le même rejet (Exception) pour un compte inexistant que pour un compte valide avec mauvais mot de passe.

### 6. Étapes
| Étape # | Étapes | Résultats attendus | Résultats réels | Pass / Fail / Blocked |
| :---: | :--- | :--- | :--- | :---: |
| 1 | Rechercher `unknownuser` via `LoginAsync` | Déclenchement immédiat de l'exception | Exception déclenchée | ✅ Pass |
| 2 | Valider le message d'erreur uniforme | Message neutre d'erreur | Message conforme | ✅ Pass |

---

## CT-07 : Protection de l'endpoint GET /api/orders (Anonyme)

### 1. Identification
| Champ | Valeur |
| :--- | :--- |
| **ID Cas de test** | CT-07 |
| **Titre Cas de test** : Protection de l'endpoint GET /api/orders (Anonyme) |
| **Créé par** | Youssef Chaari |
| **Revue par** | Mohamed Aziz Zouari |
| **Version** | 1.0 |
| **Nom du testeur** | Youssef Chaari |
| **Date de test** | 15 Mars 2026 |
| **Résultat Final** | ✅ Pass |

### 2. Complément méthodologique
| Champ | Valeur |
| :--- | :--- |
| **Niveau** | Intégration (Backend) |
| **Type** | Sécurité |
| **Technique** | Boîte Noire |
| **Motivation** | Valider la présence de barrières de sécurité HTTP (Middleware Auth). |
| **Fichier** | `BiProject.Tests/OrderIntegrationTests.cs` |

### 3. Prérequis
| Condition | État attendu |
| :--- | :--- |
| API Engine | Serveur ASP.NET prêt à répondre |

### 4. Jeu de données de test
| Paramètre | Valeur |
| :--- | :--- |
| **Header** | Aucun (Sans Bearer Token) |

### 5. Scénario de test
Effectuer une requête directe sur l'API de consultation des commandes sans s'authentifier au préalable. Le serveur doit refuser l'accès dès la réception.

### 6. Étapes
| Étape # | Étapes | Résultats attendus | Résultats réels | Pass / Fail / Blocked |
| :---: | :--- | :--- | :--- | :---: |
| 1 | Appeler `GET http://localhost:5120/api/orders` | Rejet par l'infrastructure sécurité | Requête rejetée | ✅ Pass |
| 2 | Analyser le code de réponse HTTP | `401 Unauthorized` | `401 Unauthorized` | ✅ Pass |

---

## CT-08 : Protection de l'endpoint POST /api/orders (Anonyme)

### 1. Identification
| Champ | Valeur |
| :--- | :--- |
| **ID Cas de test** | CT-08 |
| **Titre Cas de test** : Protection de l'endpoint POST /api/orders (Anonyme) |
| **Créé par** : Youssef Chaari |
| **Revue par** | Mohamed Aziz Zouari |
| **Version** | 1.0 |
| **Nom du testeur** | Youssef Chaari |
| **Date de test** | 15 Mars 2026 |
| **Résultat Final** | ✅ Pass |

### 2. Complément méthodologique
| Champ | Valeur |
| :--- | :--- |
| **Niveau** | Intégration (Backend) |
| **Type** | Sécurité |
| **Technique** | Boîte Noire |
| **Motivation** | Interdire la manipulation de ressources sensibles par des anonymes. |
| **Fichier** | `BiProject.Tests/OrderIntegrationTests.cs` |

### 3. Prérequis
| Condition | État attendu |
| :--- | :--- |
| Middleware ASP | Décorateurs `[Authorize]` actifs sur le contrôleur |

### 4. Jeu de données de test
| Paramètre | Valeur |
| :--- | :--- |
| **Payload** | Body JSON factice |

### 5. Scénario de test
S'assurer qu'aucun enregistrement de commande n'est techniquement possible sans jeton JWT valide au niveau de la couche API.

### 6. Étapes
| Étape # | Étapes | Résultats attendus | Résultats réels | Pass / Fail / Blocked |
| :---: | :--- | :--- | :--- | :---: |
| 1 | Envoyer une requête POST brute | Code HTTP erreur attendu | Code erreur reçu | ✅ Pass |
| 2 | Vérifier le code de réponse | `401 Unauthorized` | `401 Unauthorized` | ✅ Pass |

---

## CT-09 : Protection de l'endpoint POST /api/products (Anonyme)

### 1. Identification
| Champ | Valeur |
| :--- | :--- |
| **ID Cas de test** | CT-09 |
| **Titre Cas de test** : Protection de l'endpoint POST /api/products (Anonyme) |
| **Créé par** | Youssef Chaari |
| **Revue par** | Mohamed Aziz Zouari |
| **Version** | 1.0 |
| **Nom du testeur** | Youssef Chaari |
| **Date de test** | 15 Mars 2026 |
| **Résultat Final** | ✅ Pass |

### 2. Complément méthodologique
| Champ | Valeur |
| :--- | :--- |
| **Niveau** | Intégration (Backend) |
| **Type** | Sécurité |
| **Technique** : Boîte Noire |
| **Motivation** | Protéger l'intégrité du catalogue contre les écritures non identifiées. |
| **Fichier** | `BiProject.Tests/OrderIntegrationTests.cs` |

### 4. Jeu de données de test
| Paramètre | Valeur |
| :--- | :--- |
| **URL** | `/api/products` |

### 5. Scénario de test
Tenter l'ajout d'un nouveau produit par un appel API direct sans en-tête d'authentification. L'accès doit être bloqué.

### 6. Étapes
| Étape # | Étapes | Résultats attendus | Résultats réels | Pass / Fail / Blocked |
| :---: | :--- | :--- | :--- | :---: |
| 1 | Executer un POST vers products | Retour 401 | Accès bloqué (401) | ✅ Pass |

---

## CT-10 : Inscription d'un nouveau compte (Flux complet)

### 1. Identification
| Champ | Valeur |
| :--- | :--- |
| **ID Cas de test** | CT-10 |
| **Titre Cas de test** | Inscription d'un nouveau compte (Flux complet) |
| **Créé par** | Youssef Chaari |
| **Revue par** | Mohamed Aziz Zouari |
| **Version** | 1.0 |
| **Nom du testeur** | Youssef Chaari |
| **Date de test** | 15 Mars 2026 |
| **Résultat Final** | ✅ Pass |

### 2. Complément méthodologique
| Champ | Valeur |
| :--- | :--- |
| **Niveau** | Système (E2E) |
| **Type** : Fonctionnel |
| **Technique** | Boîte Noire |
| **Motivation** | Valider la fluidité du tunnel d'accueil pour les nouveaux utilisateurs. |
| **Fichier** | `tests_selenium/tests/test_register.py` |

### 3. Prérequis
| Condition | État attendu |
| :--- | :--- |
| Workspace | Frontend accessible sur le port 4200 |

### 4. Jeu de données de test
| Paramètre | Valeur |
| :--- | :--- |
| `firstName` | `John` |
| `lastName` | `Doe` |
| `username` | `user_[ID_aléatoire]` |
| `email` | `user_[ID]@test.com` |

### 5. Scénario de test
Un nouveau prospect remplit ses informations de profil. Le système doit créer le compte, initialiser son rôle par défaut ('User') et le connecter immédiatement.

### 6. Étapes
| Étape # | Étapes | Résultats attendus | Résultats réels | Pass / Fail / Blocked |
| :---: | :--- | :--- | :--- | :---: |
| 1 | Accéder à `/register` | Formulaire d'inscription disponible | Page chargée | ✅ Pass |
| 2 | Renseigner les champs obligatoires | Données acceptées sans erreur locale | Champs saisis | ✅ Pass |
| 3 | Cliquer sur "S'inscrire" | Création en base de données | Compte créé | ✅ Pass |
| 4 | Attendre la redirection | Entrée sur `/products` sous session | Redirection ok | ✅ Pass |

---

## CT-11 : Rejet d'une inscription en cas de doublon d'email

### 1. Identification
| Champ | Valeur |
| :--- | :--- |
| **ID Cas de test** | CT-11 |
| **Titre Cas de test** | Rejet d'une inscription en cas de doublon d'email |
| **Créé par** | Youssef Chaari |
| **Revue par** | Mohamed Aziz Zouari |
| **Version** | 1.0 |
| **Nom du testeur** | Youssef Chaari |
| **Date de test** | 15 Mars 2026 |
| **Résultat Final** | ✅ Pass |

### 2. Complément méthodologique
| Champ | Valeur |
| :--- | :--- |
| **Niveau** | Système (E2E) |
| **Type** | Fonctionnel |
| **Technique** | Boîte Noire (Donnée invalide - doublon) |
| **Motivation** | Assurer l'intégrité de la base de données et l'unicité des comptes. |
| **Fichier** | `tests_selenium/tests/test_register.py` |

### 3. Prérequis
| Condition | État attendu |
| :--- | :--- |
| Injecteur de données | Compte `test_user` déjà présent avec l'email `test@example.com` |

### 4. Jeu de données de test
| Paramètre | Valeur |
| :--- | :--- |
| `username` | `test_user` (existant) |
| `email` | `test@example.com` (existant) |

### 5. Scénario de test
Tenter d'utiliser les informations d'un compte déjà existant pour créer une nouvelle identité. Le système doit afficher un message d'erreur explicite et interdire l'action.

### 6. Étapes
| Étape # | Étapes | Résultats attendus | Résultats réels | Pass / Fail / Blocked |
| :---: | :--- | :--- | :--- | :---: |
| 1 | Se rendre sur la page `/register` | Formulaire prêt | Formulaire chargé | ✅ Pass |
| 2 | Renseigner les champs du doublon | Validation locale sans rejet préalable | Champs saisis | ✅ Pass |
| 3 | Cliquer sur "S'inscrire" | Message d'alerte : "Identifiant déjà existant" | Message affiché | ✅ Pass |
| 4 | Vérifier l'URL | Rester sur `/register` | URL inchangée | ✅ Pass |

---

## CT-12 : Chargement et affichage de la grille de produits

### 1. Identification
| Champ | Valeur |
| :--- | :--- |
| **ID Cas de test** | CT-12 |
| **Titre Cas de test** | Chargement et affichage de la grille de produits |
| **Créé par** | Youssef Chaari |
| **Revue par** | Mohamed Aziz Zouari |
| **Version** | 1.0 |
| **Nom du testeur** | Youssef Chaari |
| **Date de test** | 15 Mars 2026 |
| **Résultat Final** | ✅ Pass |

### 2. Complément méthodologique
| Champ | Valeur |
| :--- | :--- |
| **Niveau** | Système (E2E) |
| **Type** | Fonctionnel |
| **Technique** | Boîte Noire |
| **Motivation** | Valider l'intégration UI/API pour l'affichage du catalogue. |
| **Fichier** | `tests_selenium/tests/test_catalog.py` |

### 3. Prérequis
| Condition | État attendu |
| :--- | :--- |
| Session active | Utilisateur authentifié via LoginPage |
| Produits | Présence d'au moins 1 article en base |

### 4. Jeu de données de test
| Paramètre | Valeur |
| :--- | :--- |
| **Compte** | `test_user` / `TestPassword123!` |

### 5. Scénario de test
Après connexion, naviguer vers le catalogue et s'assurer que les composants visuels (cartes produits) sont chargés dynamiquement depuis l'API.

### 6. Étapes
| Étape # | Étapes | Résultats attendus | Résultats réels | Pass / Fail / Blocked |
| :---: | :--- | :--- | :--- | :---: |
| 1 | S'authentifier avec succès | Redirection vers `/products` | Redirection ok | ✅ Pass |
| 2 | Vérifier la grille de produits | Composant product-list visible | Liste affichée | ✅ Pass |
| 3 | Compter les éléments | Nombre de produits > 0 | Produits présents | ✅ Pass |

---

## CT-13 : Récupération unitaire des détails d'un produit (API)

### 1. Identification
| Champ | Valeur |
| :--- | :--- |
| **ID Cas de test** | CT-13 |
| **Titre Cas de test** | Récupération unitaire des détails d'un produit (API) |
| **Créé par** | Youssef Chaari |
| **Revue par** | Mohamed Aziz Zouari |
| **Version** | 1.0 |
| **Nom du testeur** | Youssef Chaari |
| **Date de test** | 15 Mars 2026 |
| **Résultat Final** | ✅ Pass |

### 2. Complément méthodologique
| Champ | Valeur |
| :--- | :--- |
| **Niveau** | Intégration (Backend) |
| **Type** | Fonctionnel |
| **Technique** | Boîte Noire |
| **Motivation** | Garantir la précision des données envoyées au client Web. |
| **Fichier** | `BiProject.Tests/ProductIntegrationTests.cs` |

### 3. Prérequis
| Condition | État attendu |
| :--- | :--- |
| Ressource | Produit ID connu existant |

### 4. Jeu de données de test
| Paramètre | Valeur |
| :--- | :--- |
| **id** | ID dynamique récupéré via GET `/api/products` |

### 5. Scénario de test
L'API doit être capable de retourner l'intégralité d'un objet Produit (Nom, Prix, Description) pour un identifiant unique fourni en paramètre.

### 6. Étapes
| Étape # | Étapes | Résultats attendus | Résultats réels | Pass / Fail / Blocked |
| :---: | :--- | :--- | :--- | :---: |
| 1 | Appeler l'endpoint `/api/products/{id}` | Statut HTTP 200 OK | Statut 200 | ✅ Pass |
| 2 | Désérialiser le JSON réponse | Objet `ProductDto` complet | Objet correct | ✅ Pass |
| 3 | Comparer l'ID reçu | ID match avec la requête | ID identique | ✅ Pass |
| 4 | Vérifier le nom du produit | Chaîne non vide | Nom présent | ✅ Pass |

---

## CT-14 : Interdiction de création de produit pour le rôle 'User'

### 1. Identification
| Champ | Valeur |
| :--- | :--- |
| **ID Cas de test** | CT-14 |
| **Titre Cas de test** | Interdiction de création de produit pour le rôle 'User' |
| **Créé par** | Youssef Chaari |
| **Revue par** | Mohamed Aziz Zouari |
| **Version** | 1.0 |
| **Nom du testeur** | Youssef Chaari |
| **Date de test** | 15 Mars 2026 |
| **Résultat Final** | ✅ Pass |

### 2. Complément méthodologique
| Champ | Valeur |
| :--- | :--- |
| **Niveau** | Intégration (Backend) |
| **Type** | Sécurité (RBAC) |
| **Technique** | Valeur limite (Droit insuffisant) |
| **Motivation** | Restreindre les actions administratives aux rôles autorisés. |
| **Fichier** | `BiProject.Tests/SecurityIntegrationTests.cs` |

### 3. Prérequis
| Condition | État attendu |
| :--- | :--- |
| Jeton JWT | Généré avec le claim `Role=User` |

### 4. Jeu de données de test
| Paramètre | Valeur |
| :--- | :--- |
| **Payload** | Produit : "FORBIDDEN-ITEM" |

### 5. Scénario de test
Un utilisateur simple tente de contourner l'UI pour créer un produit via un outil de requête HTTP (ex: Postman ou test d'intégration). Le serveur doit intercepter le rôle et rejeter l'écriture.

### 6. Étapes
| Étape # | Étapes | Résultats attendus | Résultats réels | Pass / Fail / Blocked |
| :---: | :--- | :--- | :--- | :---: |
| 1 | Effectuer un POST `/api/products` avec Token User | Rejet impératif par le serveur | Accès refusé | ✅ Pass |
| 2 | Vérifier le code d'erreur | `403 Forbidden` | `403 Forbidden` | ✅ Pass |

---

## CT-15 : Autorisation de création de produit pour le rôle 'Admin'

### 1. Identification
| Champ | Valeur |
| :--- | :--- |
| **ID Cas de test** | CT-15 |
| **Titre Cas de test** | Autorisation de création de produit pour le rôle 'Admin' |
| **Créé par** | Youssef Chaari |
| **Revue par** | Mohamed Aziz Zouari |
| **Version** | 1.0 |
| **Nom du testeur** | Youssef Chaari |
| **Date de test** | 15 Mars 2026 |
| **Résultat Final** | ✅ Pass |

### 2. Complément méthodologique
| Champ | Valeur |
| :--- | :--- |
| **Niveau** | Intégration (Backend) |
| **Type** | Fonctionnel / Droits |
| **Technique** | Boîte Noire |
| **Motivation** | Valider le plein accès administratif aux ressources du catalogue. |
| **Fichier** | `BiProject.Tests/AdminIntegrationTests.cs` |

### 3. Prérequis
| Condition | État attendu |
| :--- | :--- |
| Jeton JWT | Généré avec le claim `Role=Admin` |

### 4. Jeu de données de test
| Paramètre | Valeur |
| :--- | :--- |
| **Nom Produit** | "Vélo de route Carbone Pro CT15" |
| **Code** | "BIKE-ADMIN-CT15" |

### 5. Scénario de test
Un administrateur authentifié procède à l'ajout d'une nouvelle référence produit. L'opération doit réussir et persister la donnée.

### 6. Étapes
| Étape # | Étapes | Résultats attendus | Résultats réels | Pass / Fail / Blocked |
| :---: | :--- | :--- | :--- | :---: |
| 1 | Envoyer POST avec Token Admin | Réponse positive du serveur | Code 201 Created | ✅ Pass |
| 2 | Vérifier la création en base | Objet récupérable via GET | Produit trouvé | ✅ Pass |

---

## CT-16 : Filtrage automatique de l'historique des commandes

### 1. Identification
| Champ | Valeur |
| :--- | :--- |
| **ID Cas de test** | CT-16 |
| **Titre Cas de test** | Filtrage automatique de l'historique des commandes |
| **Créé par** | Youssef Chaari |
| **Revue par** | Mohamed Aziz Zouari |
| **Version** | 1.0 |
| **Nom du testeur** | Youssef Chaari |
| **Date de test** | 15 Mars 2026 |
| **Résultat Final** | ✅ Pass |

### 2. Complément méthodologique
| Champ | Valeur |
| :--- | :--- |
| **Niveau** | Intégration (Backend) |
| **Type** | Fonctionnel |
| **Technique** | Boîte Noire |
| **Motivation** | Confirmer le cloisonnement logique des données utilisateurs. |
| **Fichier** | `BiProject.Tests/OrderIntegrationTests.cs` |

### 3. Prérequis
| Condition | État attendu |
| :--- | :--- |
| Authentification | Token User `user_ct16` actif |

### 4. Jeu de données de test
| Paramètre | Valeur |
| :--- | :--- |
| **Utilisateur** | `user_ct16` |

### 5. Scénario de test
Un utilisateur consulte ses commandes. Le serveur doit extraire son identité depuis le JWT et filtrer la réponse SQL pour ne retourner que les lignes lui appartenant.

### 6. Étapes
| Étape # | Étapes | Résultats attendus | Résultats réels | Pass / Fail / Blocked |
| :---: | :--- | :--- | :--- | :---: |
| 1 | Appeler GET `/api/orders/my-orders` | Statut OK 200 | Statut 200 reçu | ✅ Pass |
| 2 | Scanner la liste de retour | Tous les `UserId` = utilisateur courant | Filtrage ok | ✅ Pass |

---

## CT-17 : Validation du workflow complet de Checkout

### 1. Identification
| Champ | Valeur |
| :--- | :--- |
| **ID Cas de test** | CT-17 |
| **Titre Cas de test** | Validation du workflow complet de Checkout |
| **Créé par** | Youssef Chaari |
| **Revue par** | Mohamed Aziz Zouari |
| **Version** | 1.0 |
| **Nom du testeur** | Youssef Chaari |
| **Date de test** | 15 Mars 2026 |
| **Résultat Final** | ✅ Pass |

### 2. Complément méthodologique
| Champ | Valeur |
| :--- | :--- |
| **Niveau** | Intégration (Backend) |
| **Type** | Fonctionnel |
| **Technique** | Boîte Noire |
| **Motivation** | Garantir la validité du coeur de métier : le passage de commande. |
| **Fichier** | `BiProject.Tests/OrderIntegrationTests.cs` |

### 3. Prérequis
| Condition | État attendu |
| :--- | :--- |
| Stock | Produit 1 disponible |

### 4. Jeu de données de test
| Paramètre | Valeur |
| :--- | :--- |
| **Items** | `[ { ProductId: 1, Quantity: 1 } ]` |

### 5. Scénario de test
Simuler l'envoi d'un panier complet vers le serveur et valider que l'enregistrement s'effectue sans erreur technique.

### 6. Étapes
| Étape # | Étapes | Résultats attendus | Résultats réels | Pass / Fail / Blocked |
| :---: | :--- | :--- | :--- | :---: |
| 1 | Envoyer la commande (POST) | Succès de l'opération | Code 201 Created | ✅ Pass |
| 2 | Vérifier l'objet de confirmation | ID de la nouvelle commande présent | ID généré | ✅ Pass |

---

## CT-18 : Isolation par défaut des commandes (Check IDOR)

### 1. Identification
| Champ | Valeur |
| :--- | :--- |
| **ID Cas de test** | CT-18 |
| **Titre Cas de test** | Isolation par défaut des commandes (Check IDOR) |
| **Créé par** | Youssef Chaari |
| **Revue par** | Mohamed Aziz Zouari |
| **Version** | 1.0 |
| **Nom du testeur** | Youssef Chaari |
| **Date de test** | 15 Mars 2026 |
| **Résultat Final** | ❌ Fail |

### 2. Complément méthodologique
| Champ | Valeur |
| :--- | :--- |
| **Niveau** | Intégration (Backend) |
| **Type** | Sécurité |
| **Technique** | Boîte Noire (Exploitation IDOR) |
| **Motivation** | Détecter les failles de contrôle d'accès sur l'objet Commande (Insecure Direct Object Reference). |
| **Fichier** | `BiProject.Tests/SecurityIntegrationTests.cs` |

### 3. Prérequis
| Condition | État attendu |
| :--- | :--- |
| Donneés cibles | Commande ID=1 appartenant à un autre utilisateur |

### 4. Jeu de données de test
| Paramètre | Valeur |
| :--- | :--- |
| **URL Cible** | `/api/orders/1` |
| **Token** | `test_user` (Invalide pour cet ID) |

### 5. Scénario de test
Un utilisateur authentifié tente délibérément de lire une commande dont il n'est pas le propriétaire en manipulant l'ID dans l'URL. Le système doit bloquer l'accès.

### 6. Étapes
| Étape # | Étapes | Résultats attendus | Résultats réels | Pass / Fail / Blocked |
| :---: | :--- | :--- | :--- | :---: |
| 1 | Appeler GET `/api/orders/1` | Rejet 403 Forbidden ou 404 | **Succès 200 OK** | ❌ Fail |
| 2 | Analyser le contenu reçu | Accès interdit | Détails de la commande lus | ❌ Fail |

---

## CT-19 : Restriction de l'accès aux KPIs de Business Intelligence

### 1. Identification
| Champ | Valeur |
| :--- | :--- |
| **ID Cas de test** | CT-19 |
| **Titre Cas de test** | Restriction de l'accès aux KPIs de Business Intelligence |
| **Créé par** | Youssef Chaari |
| **Revue par** | Mohamed Aziz Zouari |
| **Version** | 1.0 |
| **Nom du testeur** | Youssef Chaari |
| **Date de test** | 15 Mars 2026 |
| **Résultat Final** | ❌ Fail |

### 2. Complément méthodologique
| Champ | Valeur |
| :--- | :--- |
| **Niveau** | Intégration (Backend) |
| **Type** | Sécurité (RBAC) |
| **Technique** | Boîte Noire |
| **Motivation** | Vérifier que les données BI sensibles sont réservées aux profils Admin. |
| **Fichier** | `BiProject.Tests/SecurityIntegrationTests.cs` |

### 3. Prérequis
| Condition | État attendu |
| :--- | :--- |
| Role Mapping | `test_user` possède uniquement le rôle "User" |

### 4. Jeu de données de test
| Paramètre | Valeur |
| :--- | :--- |
| **URL** | `/api/analytics/kpis` |

### 5. Scénario de test
L'utilisateur tente de consulter les indicateurs financiers de l'entreprise. Le serveur doit rejeter la requête pour défaut d'autorisation administrative.

### 6. Étapes
| Étape # | Étapes | Résultats attendus | Résultats réels | Pass / Fail / Blocked |
| :---: | :--- | :--- | :--- | :---: |
| 1 | GET `/api/analytics/kpis` avec Token User | Rejet 403 Forbidden | **Succès 200 OK** | ❌ Fail |
| 2 | Vérifier l'accès aux données | Accès refusé | Données stratégiques lues | ❌ Fail |

---

## CT-20 : Exploitation dynamique de la faille IDOR (Vulnerability Exploit)

### 1. Identification
| Champ | Valeur |
| :--- | :--- |
| **ID Cas de test** | CT-20 |
| **Titre Cas de test** | Exploitation dynamique de la faille IDOR (Vulnerability Exploit) |
| **Créé par** | Youssef Chaari |
| **Revue par** | Mohamed Aziz Zouari |
| **Version** | 1.0 |
| **Nom du testeur** | Youssef Chaari |
| **Date de test** | 15 Mars 2026 |
| **Résultat Final** | ❌ Fail |

### 2. Complément méthodologique
| Champ | Valeur |
| :--- | :--- |
| **Niveau** | Intégration (Backend) |
| **Type** | Sécurité (Exploitation) |
| **Technique** | Boîte Noire |
| **Motivation** | Démonstration formelle d'une vulnérabilité par manipulation d'ID dynamique. |
| **Fichier** | `BiProject.Tests/VulnerabilityExploitTests.cs` |

### 3. Prérequis
| Condition | État attendu |
| :--- | :--- |
| Lab de test | Capacité d'instancier un utilisateur victime à la volée |

### 4. Jeu de données de test
| Paramètre | Valeur |
| :--- | :--- |
| **Attaquant** | `user_attacker_val` |
| **Victime** | `user_victim_val` |

### 5. Scénario de test
L'utilisateur Victime crée une commande. L'utilisateur Attaquant (indépendant) tente de la lire en connaissant l'ID généré par la victime. L'API ne doit jamais autoriser ce croisement.

### 6. Étapes
| Étape # | Étapes | Résultats attendus | Résultats réels | Pass / Fail / Blocked |
| :---: | :--- | :--- | :--- | :---: |
| 1 | Créer commande pour Victime | Succès (ID=X) | Commande créée | ✅ Pass |
| 2 | Lire ID=X avec Token Attaquant | Rejet 403/404 | **Succès 200 OK** | ❌ Fail |
| 3 | Analyser le contenu de la fuite | Données protégées | Payload victime exposé | ❌ Fail |

---
*Fin du catalogue détaillé des 20 cas de test.*
