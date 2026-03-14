from selenium.webdriver.common.by import By
from selenium.webdriver.support import expected_conditions as EC
from .base_page import BasePage

class LoginPage(BasePage):
    # Sélecteurs CSS basés sur le template Angular réel
    USERNAME_INPUT = (By.CSS_SELECTOR, "input[formControlName='username']")
    PASSWORD_INPUT = (By.CSS_SELECTOR, "input[formControlName='password']")
    SUBMIT_BUTTON = (By.CSS_SELECTOR, "button[type='submit']")

    def navigate(self):
        """Naviguer vers la page de connexion."""
        self.navigate_to("/login")

    def enter_username(self, username: str):
        """Saisir le nom d'utilisateur."""
        field = self.wait.until(EC.element_to_be_clickable(self.USERNAME_INPUT))
        field.clear()
        field.send_keys(username)

    def enter_password(self, password: str):
        """Saisir le mot de passe."""
        field = self.wait.until(EC.element_to_be_clickable(self.PASSWORD_INPUT))
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
