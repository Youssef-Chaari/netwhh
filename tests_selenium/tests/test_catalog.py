import time
import pytest
import sys
import os

sys.path.insert(0, os.path.join(os.path.dirname(__file__), ".."))
from pages.login_page import LoginPage
from pages.products_page import ProductsPage

def test_consult_product_list(driver):
    """
    CT-12 : Consultation de la liste des produits par un utilisateur authentifié.
    Vérifie que le catalogue est chargé et visible.
    """
    # 1. Se connecter d'abord
    login_page = LoginPage(driver)
    login_page.login("test_user", "TestPassword123!")
    
    # Attendre la redirection via login_page
    login_page.wait_for_url_contains("/products")
    
    # 2. Vérifier le catalogue
    products_page = ProductsPage(driver)
    
    assert products_page.is_list_visible(), "[CT-12 ÉCHEC] La liste des produits n'est pas visible"
    count = products_page.get_product_count()
    assert count > 0, f"[CT-12 ÉCHEC] Aucun produit trouvé dans le catalogue (count={count})"
