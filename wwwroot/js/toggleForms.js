function toggleForms() {
    const loginForm = document.getElementById('login-form');
    const registerForm = document.getElementById('register-form');

    // Chuyển đổi giữa hai form
    if (loginForm.classList.contains('form-hidden')) {
        loginForm.classList.remove('form-hidden');
        registerForm.classList.add('form-hidden');
    } else {
        loginForm.classList.add('form-hidden');
        registerForm.classList.remove('form-hidden');
    }
}