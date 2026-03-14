*** Settings ***
Documentation     Scénario de test Bonus avec Robot Framework.
...               Ce test démontre l'utilisation de keywords lisibles pour l'authentification.
Library           SeleniumLibrary

*** Variables ***
${BROWSER}        Chrome
${BASE_URL}       http://localhost:4200
${VALID_USER}     test_user
${VALID_PWD}      TestPassword123!

*** Test Cases ***
Connexion Réussie Via Robot Framework
    [Documentation]    Vérifie qu'un utilisateur peut se connecter avec Robot Framework.
    Ouvrir Le Navigateur Sur La Page De Connexion
    Saisir Les Identifiants    ${VALID_USER}    ${VALID_PWD}
    Soumettre Le Formulaire
    Vérifier La Redirection Vers Le Catalogue
    [Teardown]    Fermer Le Navigateur

Consultation Du Catalogue Après Connexion
    [Documentation]    Vérifie que le catalogue est visible après authentification.
    Ouvrir Le Navigateur Sur La Page De Connexion
    Saisir Les Identifiants    ${VALID_USER}    ${VALID_PWD}
    Soumettre Le Formulaire
    Vérifier La Présence Des Produits Dans La Grille
    [Teardown]    Fermer Le Navigateur

*** Keywords ***
Ouvrir Le Navigateur Sur La Page De Connexion
    Open Browser    ${BASE_URL}/login    ${BROWSER}
    Maximize Browser Window
    Set Selenium Implicit Wait    5 seconds

Saisir Les Identifiants
    [Arguments]    ${username}    ${password}
    Input Text      css:input[formControlName='username']    ${username}
    Input Text      css:input[formControlName='password']    ${password}

Soumettre Le Formulaire
    Click Button    css:button[type='submit']

Vérifier La Redirection Vers Le Catalogue
    Wait Until Location Contains    /products    timeout=10s
    Location Should Contain         /products

Vérifier La Présence Des Produits Dans La Grille
    # Attendre la redirection d'abord
    Vérifier La Redirection Vers Le Catalogue
    # Vérifier que les cartes produits (grid glass-panel) sont visibles
    Wait Until Element Is Visible    css:.grid > div.glass-panel    timeout=10s
    Page Should Contain Element      css:.grid > div.glass-panel

Fermer Le Navigateur
    Close Browser
