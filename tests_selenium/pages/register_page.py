from selenium.webdriver.common.by import By
from selenium.webdriver.support import expected_conditions as EC
from .base_page import BasePage

class RegisterPage(BasePage):
    FIRST_NAME_INPUT = (By.CSS_SELECTOR, "input[formControlName='firstName']")
    LAST_NAME_INPUT = (By.CSS_SELECTOR, "input[formControlName='lastName']")
    USERNAME_INPUT = (By.CSS_SELECTOR, "input[formControlName='username']")
    EMAIL_INPUT = (By.CSS_SELECTOR, "input[formControlName='email']")
    PASSWORD_INPUT = (By.CSS_SELECTOR, "input[formControlName='password']")
    SUBMIT_BUTTON = (By.CSS_SELECTOR, "button[type='submit']")
    ERROR_MESSAGE_DIV = (By.CSS_SELECTOR, ".bg-red-900\/40")

    def load(self):
        self.navigate_to("/register")

    def register(self, first_name, last_name, username, email, password):
        self.wait.until(EC.element_to_be_clickable(self.FIRST_NAME_INPUT)).send_keys(first_name)
        self.driver.find_element(*self.LAST_NAME_INPUT).send_keys(last_name)
        self.driver.find_element(*self.USERNAME_INPUT).send_keys(username)
        self.driver.find_element(*self.EMAIL_INPUT).send_keys(email)
        self.driver.find_element(*self.PASSWORD_INPUT).send_keys(password)
        self.driver.find_element(*self.SUBMIT_BUTTON).click()

    def get_error_message(self):
        try:
            element = self.wait.until(EC.visibility_of_element_located(self.ERROR_MESSAGE_DIV))
            return element.text
        except:
            return None
