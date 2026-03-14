# Tableau de Traçabilité — BiProject QA (Y. Chaari & M. Zouari)

Ce tableau relie les exigences fonctionnelles et non-fonctionnelles du système aux cas de test implémentés.

| Exigence | Description | CT ID | Niveau | Type | Technique | Fichier | Résultat |
|---|---|---|---|---|---|---|---|
| REQ-AUTH-01 | L'utilisateur peut se connecter avec des identifiants valides | CT-01 | Tests système (E2E) — Selenium + POM | Fonctionnel | Boîte noire | `test_login.py` | ✅ Pass |
| REQ-AUTH-01 | Le système rejette un mot de passe incorrect | CT-02 | Tests système (E2E) — Selenium + POM | Fonctionnel | Boîte noire | `test_login.py` | ✅ Pass |
| REQ-AUTH-02 | Les routes protégées bloquent l'accès sans token | CT-03 | Tests système (E2E) — Selenium + POM | Non fonctionnel — Sécurité | Boîte noire | `test_login.py` | ✅ Pass |
| REQ-AUTH-01 | `AuthService` génère un token JWT valide | CT-04 | Tests unitaires (Backend) | Fonctionnel | Boîte blanche | `AuthServiceTests.cs` | ✅ Pass |
| REQ-AUTH-01 | `AuthService` rejette un mauvais mot de passe | CT-05 | Tests unitaires (Backend) | Fonctionnel | Boîte blanche | `AuthServiceTests.cs` | ✅ Pass |
| REQ-AUTH-01 | `AuthService` rejette un utilisateur inconnu | CT-06 | Tests unitaires (Backend) | Fonctionnel | Boîte blanche | `AuthServiceTests.cs` | ✅ Pass |
| REQ-SEC-01 | `GET /api/orders` rejette les requêtes sans token | CT-07 | Tests d’intégration (Backend) | Sécurité | Boîte noire | `OrderIntegrationTests.cs` | ✅ Pass |
| REQ-SEC-01 | `POST /api/orders` rejette les requêtes sans token | CT-08 | Tests d’intégration (Backend) | Sécurité | Boîte noire | `OrderIntegrationTests.cs` | ✅ Pass |
| REQ-SEC-01 | `POST /api/products` rejette les requêtes sans token | CT-09 | Tests d’intégration (Backend) | Sécurité | Boîte noire | `OrderIntegrationTests.cs` | ✅ Pass |
| REQ-AUTH-03 | Inscription réussie d'un nouveau compte | CT-10 | Système (E2E) | Fonctionnel | Boîte noire | `test_register.py` | ✅ Pass |
| REQ-AUTH-03 | Rejet de doublons à l'inscription | CT-11 | Système (E2E) | Fonctionnel | Boîte noire | `test_register.py` | ✅ Pass |
| REQ-CAT-01 | Consultation du catalogue de produits | CT-12 | Système (E2E) | Fonctionnel | Boîte noire | `test_catalog.py` | ✅ Pass |
| REQ-CAT-01 | Consultation du détail d'un produit | CT-13 | Intégration (Backend) | Fonctionnel | Boîte noire | `ProductIntegrationTests.cs` | ✅ Pass |
| REQ-SEC-01 | Restriction Admin sur création de produits | CT-14 | Intégration (Backend) | Sécurité | Boîte noire | `SecurityIntegrationTests.cs` | ✅ Pass |
| REQ-CAT-02 | Création d'un produit par Admin | CT-15 | Intégration (Backend) | Fonctionnel | Boîte noire | `AdminIntegrationTests.cs` | ✅ Pass |
| REQ-ORD-01 | Consultation de ses propres commandes | CT-16 | Intégration (Backend) | Fonctionnel | Boîte noire | `OrderIntegrationTests.cs` | ✅ Pass |
| REQ-ORD-02 | Passage d'une commande valide | CT-17 | Intégration (Backend) | Fonctionnel | Boîte noire | `OrderIntegrationTests.cs` | ✅ Pass |
| REQ-SEC-02 | Identification d'anomalies (IDOR, Trust Boundary, Logging, Structure) | CT-ST-01 | Statique | Sécurité / Qualité | Revues & Inspections | Plusieurs fichiers | ✅ 3 anomalies détectées |
| REQ-SEC-02 | Protection contre l'accès IDOR aux commandes | CT-18 | Intégration (Backend) | Sécurité | Boîte noire | `SecurityIntegrationTests.cs` | ✅ Pass |
| REQ-SEC-03 | Restriction Admin sur Analytics BI | CT-19 | Intégration (Backend) | Sécurité | Boîte noire | `SecurityIntegrationTests.cs` | ✅ Pass |
| — | Validation de la faille IDOR (Preuve dynamique) | CT-BUG-01 | Intégration (Backend) | Sécurité | Boîte noire | `VulnerabilityExploitTests.cs` | ❌ Fail |

### Démonstration de Vulnérabilités (Dynamic Bug Exploit)
| Exigence | Bug | CT ID | Niveau | Type | Fichier | Statut |
|---|---|---|---|---|---|---|
| REQ-SEC-02 | Faille IDOR sur GetOrderById | CT-BUG-01 | Intégration (Sécurité) | Validation de Bug | `VulnerabilityExploitTests.cs` | ❌ Fail (Bug IDOR) |

> Les tests Selenium (CT-01, CT-02, CT-03) ont été validés en environnement réel avec succès.
