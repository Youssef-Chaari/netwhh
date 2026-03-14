import os
import pytest
from datetime import datetime
from selenium import webdriver
from selenium.webdriver.chrome.service import Service
from selenium.webdriver.chrome.options import Options
from webdriver_manager.chrome import ChromeDriverManager

@pytest.fixture(scope="function")
def driver():
    """Fixture Selenium : instancie Chrome et retourne le WebDriver."""
    options = Options()
    # options.add_argument("--headless") # Décommenter pour le mode headless
    options.add_argument("--no-sandbox")
    options.add_argument("--disable-dev-shm-usage")
    options.add_argument("--window-size=1280,800")

    drv = webdriver.Chrome(
        service=Service(ChromeDriverManager().install()),
        options=options
    )
    drv.implicitly_wait(2) # Réduit car on va utiliser des explicit waits

    yield drv

    drv.quit()

@pytest.hookimpl(tryfirst=True, hookwrapper=True)
def pytest_runtest_makereport(item, call):
    """Hook pour capturer l'écran en cas d'échec de test."""
    outcome = yield
    rep = outcome.get_result()
    
    if rep.when == "call" and rep.failed:
        mode = "a" if os.path.exists("failures.txt") else "w"
        with open("failures.txt", mode) as f:
            if "driver" in item.fixturenames:
                web_driver = item.funcargs['driver']
                
                # Création du dossier screenshots si inexistant
                screenshot_dir = os.path.join(os.path.dirname(__file__), "screenshots")
                if not os.path.exists(screenshot_dir):
                    os.makedirs(screenshot_dir)
                
                # Nom du fichier : test_name_timestamp.png
                timestamp = datetime.now().strftime("%Y%m%d_%H%M%S")
                file_name = f"{item.name}_{timestamp}.png"
                file_path = os.path.join(screenshot_dir, file_name)
                
                web_driver.save_screenshot(file_path)
                print(f"\n[SCREENSHOT] Capture sauvegardée : {file_path}")
