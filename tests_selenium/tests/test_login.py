"""
test_login.py — Tests Système (E2E) Selenium + POM — version corrigée.

Correction critique apportée lors de la vérification du code source Angular :
  - /dashboard est protégé par authGuard + adminGuard (rôle Admin requis).
  - Un utilisateur de rôle "User" est redirigé vers /products après connexion.
  - Il n'existe PAS de compte admin seedé dans OltpDbContext.cs.
  - Les identifiants (admin / Admin123!) doivent être créés via l'API avant exécution.

Prérequis réels (vérifiés dans le code) :
  - Backend .NET en cours d'exécution : http://localhost:5120
  - Frontend Angular en cours d'exécution : http://localhost:4200
  - Un compte de test DOIT exister en base. Voir conftest.py pour le seed via API.

Sélecteurs HTML confirmés dans login.component.ts :
  - input[formControlName='username']  → présent (champ Username)
  - input[formControlName='password']  → présent (champ Password)
  - button[type='submit']              → présent

Routes confirmées dans app.routes.ts :
  - /login       → LoginComponent (public)
  - /dashboard   → DashboardComponent (authGuard + adminGuard → rôle Admin)
  - /products    → ProductsComponent (authGuard seulement → tout utilisateur connecté)
  - /**          → redirectTo: login (route inconnue)
"""
import time
import pytest
import sys
import os

sys.path.insert(0, os.path.join(os.path.dirname(__file__), ".."))
from pages.login_page import LoginPage

BASE_URL = "http://localhost:4200"

# ---------------------------------------------------------------------------
# CT-01 : Connexion réussie — utilisateur simple (rôle User)
# Correction : un User atterrit sur /products, pas /dashboard (adminGuard).
# ---------------------------------------------------------------------------
def test_login_successful_user(driver):
    """
    CT-01 : Connexion valide (rôle User) → redirection vers /products.
    Technique : Boîte noire — Partition d'équivalence (classe valide).
    Prérequis : compte test_user / TestPassword123! existant en base de données.
    """
    page = LoginPage(driver)
    page.login("test_user", "TestPassword123!")
    
    # Attente intelligente de la redirection
    page.wait_for_url_contains("/products")
    
    current = driver.current_url
    assert "/products" in current, (
        f"[CT-01 ÉCHEC] URL attendue: /products | URL obtenue: {current}"
    )


# ---------------------------------------------------------------------------
# CT-02 : Connexion échouée — mauvais mot de passe
# ---------------------------------------------------------------------------
def test_login_invalid_password(driver):
    """
    CT-02 : Connexion avec mauvais mot de passe → rester sur /login.
    Technique : Boîte noire — Partition d'équivalence (classe invalide).
    """
    page = LoginPage(driver)
    page.login("test_user", "MauvaisMotDePasse999!")
    
    # Ici on peut attendre un peu ou vérifier un message d'erreur
    # Pour CT-02, on reste sur /login
    page.wait_for_url_contains("/login")

    assert "/login" in driver.current_url, (
        f"[CT-02 ÉCHEC] Aurais dû rester sur /login | URL obtenue: {driver.current_url}"
    )


# ---------------------------------------------------------------------------
# CT-03 : Accès direct à une route protégée sans authentification
# ---------------------------------------------------------------------------
def test_protected_route_without_auth(driver):
    """
    CT-03 : Navigation directe vers /products sans être connecté → redirection /login.
    Technique : Boîte noire — Valeur limite (accès sans token).
    Confirmé dans le code : authGuard sur /products redirige vers /login si non connecté.
    """
    driver.get("http://localhost:4200/products")
    
    # L'AuthGuard est rapide, mais on utilise quand même une attente explicite
    LoginPage(driver).wait_for_url_contains("/login")

    assert "/login" in driver.current_url, (
        f"[CT-03 ÉCHEC] L'authGuard aurait dû rediriger vers /login | URL obtenue: {driver.current_url}"
    )
