from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC

class RegisterPage:
    def __init__(self, driver):
        self.driver = driver
        self.url = "http://localhost:4200/register"
        self.first_name_input = (By.CSS_SELECTOR, "input[formControlName='firstName']")
        self.last_name_input = (By.CSS_SELECTOR, "input[formControlName='lastName']")
        self.username_input = (By.CSS_SELECTOR, "input[formControlName='username']")
        self.email_input = (By.CSS_SELECTOR, "input[formControlName='email']")
        self.password_input = (By.CSS_SELECTOR, "input[formControlName='password']")
        self.submit_button = (By.CSS_SELECTOR, "button[type='submit']")
        self.error_message_div = (By.CSS_SELECTOR, ".bg-red-900\/40")

    def load(self):
        self.driver.get(self.url)

    def register(self, first_name, last_name, username, email, password):
        WebDriverWait(self.driver, 10).until(EC.presence_of_element_located(self.first_name_input)).send_keys(first_name)
        self.driver.find_element(*self.last_name_input).send_keys(last_name)
        self.driver.find_element(*self.username_input).send_keys(username)
        self.driver.find_element(*self.email_input).send_keys(email)
        self.driver.find_element(*self.password_input).send_keys(password)
        self.driver.find_element(*self.submit_button).click()

    def get_error_message(self):
        try:
            element = WebDriverWait(self.driver, 5).until(EC.visibility_of_element_located(self.error_message_div))
            return element.text
        except:
            return None
