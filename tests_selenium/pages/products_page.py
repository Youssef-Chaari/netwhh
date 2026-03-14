from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC

class ProductsPage:
    def __init__(self, driver):
        self.driver = driver
        self.url = "http://localhost:4200/products"
        # Pour les utilisateurs standards, c'est une grille de cartes
        self.product_cards = (By.CSS_SELECTOR, ".grid > div.glass-panel")
        self.cart_units_span = (By.CSS_SELECTOR, ".text-\[var\(--color-neon-cyan\)\]")

    def load(self):
        self.driver.get(self.url)

    def is_list_visible(self):
        try:
            WebDriverWait(self.driver, 10).until(EC.presence_of_element_located(self.product_cards))
            return True
        except:
            return False

    def get_product_count(self):
        elements = self.driver.find_elements(*self.product_cards)
        return len(elements)
