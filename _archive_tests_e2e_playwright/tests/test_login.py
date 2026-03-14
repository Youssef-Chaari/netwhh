from pages.login_page import LoginPage
from playwright.sync_api import expect

def test_login_successful_admin(page):
    """CT-01: Connexion réussie"""
    login_page = LoginPage(page)
    login_page.navigate()
    login_page.login("admin@admin.com", "Admin123!")
    expect(page).to_have_url("http://localhost:4200/dashboard", timeout=5000)

def test_login_invalid_password(page):
    """CT-02: Rejet d'un mot de passe incorrect"""
    login_page = LoginPage(page)
    login_page.navigate()
    login_page.login("admin@admin.com", "MauvaisMdp")
    # L'utilisateur doit rester sur /login
    expect(page).to_have_url("http://localhost:4200/login", timeout=3000)
    
def test_dashboard_access_without_auth(page):
    """CT-03: Accès à une page protégée sans authentification (E2E)"""
    page.goto("http://localhost:4200/dashboard")
    # Le Guard Angular devrait rediriger vers /login
    expect(page).to_have_url("http://localhost:4200/login", timeout=3000)

def test_orders_page_access_admin(page):
    """CT-04: Consultation d'une page métier une fois connecté"""
    login_page = LoginPage(page)
    login_page.navigate()
    login_page.login("admin@admin.com", "Admin123!")
    
    page.goto("http://localhost:4200/orders")
    expect(page).to_have_url("http://localhost:4200/orders", timeout=3000)
    # Vérification d'un titre ou tableau si présent
    expect(page.locator("app-orders")).to_be_visible(timeout=5000)
