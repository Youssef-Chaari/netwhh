"""
login_page.py — Page Object Model pour la page de connexion.

Encapsule tous les sélecteurs et actions relatifs à la page /login.
Permet aux tests d'être lisibles et indépendants des sélecteurs HTML.
"""
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC

BASE_URL = "http://localhost:4200"


class LoginPage:
    # Sélecteurs CSS basés sur le template Angular réel (login.component.ts)
    USERNAME_INPUT = (By.CSS_SELECTOR, "input[formControlName='username']")
    PASSWORD_INPUT = (By.CSS_SELECTOR, "input[formControlName='password']")
    SUBMIT_BUTTON = (By.CSS_SELECTOR, "button[type='submit']")

    def __init__(self, driver):
        self.driver = driver
        self.wait = WebDriverWait(driver, 10)

    def navigate(self):
        """Naviguer vers la page de connexion."""
        self.driver.get(f"{BASE_URL}/login")

    def enter_username(self, username: str):
        """Saisir le nom d'utilisateur."""
        field = self.wait.until(EC.presence_of_element_located(self.USERNAME_INPUT))
        field.clear()
        field.send_keys(username)

    def enter_password(self, password: str):
        """Saisir le mot de passe."""
        field = self.wait.until(EC.presence_of_element_located(self.PASSWORD_INPUT))
        field.clear()
        field.send_keys(password)

    def submit(self):
        """Cliquer sur le bouton de connexion."""
        btn = self.wait.until(EC.element_to_be_clickable(self.SUBMIT_BUTTON))
        btn.click()

    def login(self, username: str, password: str):
        """Effectuer le processus de connexion complet."""
        self.navigate()
        self.enter_username(username)
        self.enter_password(password)
        self.submit()

    def get_current_url(self) -> str:
        """Retourner l'URL courante du navigateur."""
        return self.driver.current_url
