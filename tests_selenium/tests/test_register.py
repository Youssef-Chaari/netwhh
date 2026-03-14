import time
import pytest
import sys
import os
import random
import string

sys.path.insert(0, os.path.join(os.path.dirname(__file__), ".."))
from pages.register_page import RegisterPage

def get_random_string(length):
    letters = string.ascii_lowercase
    return ''.join(random.choice(letters) for i in range(length))

def test_successful_registration(driver):
    """
    CT-10 : Inscription réussie d'un nouvel utilisateur.
    Vérifie la redirection vers /products après soumission du formulaire.
    """
    page = RegisterPage(driver)
    page.load()
    
    unique_id = get_random_string(6)
    username = f"user_{unique_id}"
    email = f"{username}@test.com"
    
    page.register("John", "Doe", username, email, "Password123!")
    
    # Attente intelligente de la redirection via BasePage
    page.wait_for_url_contains("/products")
    
    current_url = driver.current_url
    assert "/products" in current_url, f"[CT-10 ÉCHEC] Redirection vers /products attendue, obtenue : {current_url}"

def test_registration_duplicate_user(driver):
    """
    CT-11 (Bonus/Partiel) : Inscription refusée avec identifiant existant.
    Garantit que le système affiche une erreur pour un doublon.
    """
    page = RegisterPage(driver)
    page.load()
    
    # Utilisation du compte de test déjà existant
    page.register("Test", "User", "test_user", "test@example.com", "TestPassword123!")
    
    # get_error_message utilise déjà un WebDriverWait interne
    error_msg = page.get_error_message()
    assert error_msg is not None, "[CT-11 ÉCHEC] Aucun message d'erreur affiché pour un doublon"
    assert any(term in error_msg for term in ["Identifiant déjà existant", "Erreur", "existe déjà"]), f"Message inattendu: {error_msg}"
    assert "/register" in driver.current_url
