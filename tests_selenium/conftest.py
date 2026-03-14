"""
conftest.py — Configuration Selenium + ChromeDriver pour Pytest.

Chaque test reçoit une instance fraîche du navigateur (scope="function").
Le driver est fermé automatiquement après chaque test.
"""
import pytest
from selenium import webdriver
from selenium.webdriver.chrome.service import Service
from selenium.webdriver.chrome.options import Options
from webdriver_manager.chrome import ChromeDriverManager

BASE_URL = "http://localhost:4200"


@pytest.fixture(scope="function")
def driver():
    """Fixture Selenium : instancie Chrome et retourne le WebDriver."""
    options = Options()
    # Décommenter la ligne suivante pour lancer en mode visible (non headless)
    # options.add_argument("--headless")
    options.add_argument("--no-sandbox")
    options.add_argument("--disable-dev-shm-usage")
    options.add_argument("--window-size=1280,800")

    drv = webdriver.Chrome(
        service=Service(ChromeDriverManager().install()),
        options=options
    )
    drv.implicitly_wait(5)

    yield drv

    drv.quit()
