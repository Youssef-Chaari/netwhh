# Rapport des Tests Statiques — BiProject QA

## 1. Objectif et Méthodologie
Les tests statiques visent à identifier des défauts, des vulnérabilités de sécurité ou des écarts par rapport aux standards de codage **sans exécuter le code**. 

Cette démarche est complémentaire aux tests dynamiques : là où les tests dynamiques valident le comportement opérationnel du système, les tests statiques permettent de détecter des anomalies structurelles précoces, réduisant ainsi le coût de correction technique.

---

## 2. Échelle de Gravité des Anomalies
Pour assurer une évaluation objective, nous utilisons l'échelle de gravité suivante :

| Niveau | Description |
|---|---|
| **🔴 Critique** | Faille de sécurité majeure ou blocage fonctionnel total (ex: IDOR). Correction immédiate impérative. |
| **🟠 Majeure** | Violation grave des principes de conception ou risque de sécurité réel. À corriger avant production. |
| **🟡 Mineure** | Écart par rapport aux conventions ou optimisation recommandée (ex: logging, commentaires). |

---

## 3. Activité 1 : Revue Manuelle de Code (Code Review)
L'inspection humaine s'est concentrée sur la logique métier et le respect des conventions .NET sur les fichiers `AuthService.cs`, `AuthController.cs` et `OrderController.cs`.

**Points forts identifiés :**
- Utilisation systématique des DTOs pour isoler les entités de base de données.
- Séparation des responsabilités entre les couches (Contrôleur et Service).
- Gestion sécurisée des secrets JWT via la configuration.

---

## 4. Activité 2 : Analyse Statique Orientée Sécurité
Analyse approfondie des contrôles d'accès et des flux de données entre les frontières de confiance (*Trust Boundaries*).

### Focus : Broken Access Control (OWASP A01:2021)
Le code suivant dans `OrderController.cs` a été identifié comme vulnérable :

```csharp
[HttpGet("{id}")]
[Authorize(Roles = "Admin, User")]
public async Task<IActionResult> GetOrderById(int id)
{
    var order = await _orderService.GetOrderByIdAsync(id);
    if (order == null) return NotFound();
    return Ok(order); // Anomalie : Accès direct sans contrôle d'appartenance
}
```

---

## 5. Registre des Anomalies Détectées

| ID | Fichier / Zone | Anomalie | Type | Gravité | Impact | Recommandation | Statut |
|---|---|---|---|---|---|---|---|
| AN-01 | `OrderController.cs` | IDOR (Broken Access Control) | Sécurité | 🔴 Critique | Fuite de données privées (ventes tiers) | Vérifier le `UserId` via Claims JWT | Recommandé |
| AN-02 | DTO Création Order | Trust Boundary Violation | Architecture | 🟠 Majeure | Risque de commande au nom d'autrui | Ignorer le `UserId` venant du client | Recommandé |
| AN-03 | `AuthService.cs` | Absence de logging des échecs | Observabilité | 🟡 Mineur | Difficulté de détection d'attaques brute-force | Injecter et utiliser `ILogger` | Recommandé |

---

## 6. Recommandations de Correction Concrètes

### Recommandation 1 (Priorité Critique - IDOR)
Modifier la méthode `GetOrderById` pour croiser l'ID de l'utilisateur extrait du token JWT avec le champ `UserId` de la commande récupérée en base. Si l'utilisateur n'est pas Admin et n'est pas le propriétaire, retourner un code `403 Forbidden`.

### Recommandation 2 (Priorité Majeure - Trust Boundary)
Dans le service de création de commande, ne jamais faire confiance au `UserId` fourni dans le corps de la requête. Utiliser exclusivement `User.FindFirstValue(ClaimTypes.NameIdentifier)` pour identifier l'auteur de la commande.

---

## 7. Activité 3 : Revue de Qualité des Scripts de Test
Cette activité consiste en une analyse statique de la structure, de la lisibilité et de la maintenabilité des scripts de test automatisés (Backend et Frontend).

**Périmètre :** `BiProject.Tests/`, `tests_selenium/tests/`, `tests_selenium/pages/`.

### Critères et Constats Factuels

| Critère | Constat Factuel | Conclusion |
|---|---|---|
| **Pattern AAA** | Les tests `AuthServiceTests.cs` et `OrderIntegrationTests.cs` sont structurés en blocs "// Arrange", "// Act", "// Assert". | ✅ Conforme |
| **Separation of Concerns** | Usage du **Page Object Model (POM)** dans `login_page.py`. Les sélecteurs CSS sont isolés de la logique de test. | ✅ Conforme |
| **Gestion du cycle de vie** | Utilisation de `IClassFixture` dans xUnit et de `conftest.py` (fixtures pytest) pour le driver Selenium. | ✅ Conforme |
| **Nommage** | Méthodes descriptives : `LoginAsync_WithInvalidPassword_ThrowsUnauthorizedAccessException` (C#) et `test_login_successful_user` (Python). | ✅ Conforme |
| **Maintenabilité** | Les URLs et sélecteurs critiques sont centralisés. Pas de "Hard-coded values" répétées dans les scripts système. | ✅ Conforme |

**Conclusion sur la qualité structurelle :**
Les scripts présentent un haut niveau de maturité technique. L'usage rigoureux du POM et du pattern AAA garantit une faible dette technique et une facilité de maintenance en cas d'évolution de l'interface utilisateur ou des contrats d'API.

---

## 8. Limites de l'Analyse Statique
Bien que performante pour détecter des failles de conception, l'analyse statique présente des limites :
- **Faux positifs** : Certaines alertes peuvent ne pas être exploitables en condition réelle.
- **Défauts temporels** : Elle ne permet pas de détecter des problèmes liés à la latence réseau, aux fuites de mémoire à l'exécution ou aux conditions de concurrence (*Race Conditions*).
- **Dépendance à l'environnement** : Les erreurs liées à la configuration d'infrastructure (Reverse Proxy, SSL termination) restent invisibles.

---

## 9. Conclusion
L'analyse statique a permis d'anticiper des vulnérabilités critiques avant toute phase de test dynamique. La correction de l'AN-01 est jugée prioritaire pour renforcer la protection des données personnelles au sein du système.
