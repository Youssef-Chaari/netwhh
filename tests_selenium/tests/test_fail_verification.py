import pytest
from pages.login_page import LoginPage

def test_screenshot_on_failure(driver):
    """
    Cas de test intentionnellement en échec pour vérifier le mécanisme de capture d'écran.
    Il tente de vérifier une URL impossible après le chargement de la page de login.
    """
    login_page = LoginPage(driver)
    login_page.open()
    
    # On force un échec d'assertion pour déclencher le hook de capture d'écran dans conftest.py
    print("\n[INFO] Exécution d'un échec volontaire pour test de capture d'écran...")
    assert "cette_chaine_n_existe_pas" in driver.current_url, \
        "Échec volontaire : la chaîne n'est pas présente dans l'URL. Une capture d'écran doit être générée."
