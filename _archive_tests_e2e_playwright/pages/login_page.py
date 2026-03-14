class LoginPage:
    def __init__(self, page):
        self.page = page
        self.url = "http://localhost:4200/login"
        self.email_input = self.page.locator("input[type='email']")
        self.password_input = self.page.locator("input[type='password']")
        self.submit_btn = self.page.locator("button[type='submit']")

    def navigate(self):
        self.page.goto(self.url)

    def login(self, email, password):
        self.email_input.fill(email)
        self.password_input.fill(password)
        self.submit_btn.click()
