from selenium.webdriver.common.by import By
from selenium.webdriver.support import expected_conditions as EC
from .base_page import BasePage

class ProductsPage(BasePage):
    # Pour les utilisateurs standards, c'est une grille de cartes
    PRODUCT_CARDS = (By.CSS_SELECTOR, ".grid > div.glass-panel")

    def load(self):
        self.navigate_to("/products")

    def is_list_visible(self):
        try:
            self.wait.until(EC.presence_of_all_elements_located(self.PRODUCT_CARDS))
            return True
        except:
            return False

    def get_product_count(self):
        elements = self.driver.find_elements(*self.PRODUCT_CARDS)
        return len(elements)
