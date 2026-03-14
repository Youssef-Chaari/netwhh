"""
base_page.py — Abstraction commune pour toutes les pages Selenium.

Centralise l'instance du driver, l'attente explicite et la configuration globale.
"""
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC

BASE_URL = "http://localhost:4200"

class BasePage:
    def __init__(self, driver, timeout=10):
        self.driver = driver
        self.wait = WebDriverWait(driver, timeout)
        self.base_url = BASE_URL

    def wait_for_url_contains(self, fragment: str):
        """Attend que l'URL contienne un fragment spécifique."""
        self.wait.until(EC.url_contains(fragment))

    def wait_for_url_matches(self, pattern: str):
        """Attend que l'URL corresponde à un motif (regex ou exact)."""
        self.wait.until(EC.url_to_be(f"{self.base_url}{pattern}"))

    def get_current_url(self) -> str:
        """Retourne l'URL courante."""
        return self.driver.current_url

    def navigate_to(self, path: str):
        """Navigue directement vers un chemin relatif."""
        self.driver.get(f"{self.base_url}{path}")
