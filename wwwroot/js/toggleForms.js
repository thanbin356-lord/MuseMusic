// function toggleForms() {
//     const loginForm = document.getElementById('login-form');
//     const registerForm = document.getElementById('register-form');

//     // Chuyển đổi giữa hai form
//     if (loginForm.classList.contains('form-hidden')) {
//         loginForm.classList.remove('form-hidden');
//         registerForm.classList.add('form-hidden');
//     } else {
//         loginForm.classList.add('form-hidden');
//         registerForm.classList.remove('form-hidden');
//     }
// }
function toggleForms(nextFormId) {
    // Lấy tất cả các form bằng ID
    const loginForm = document.getElementById('login-form');
    const adminloginForm = document.getElementById('admin-login-form');
    const registerForm = document.getElementById('register-form');
    const register2Form = document.getElementById('register2-form');
    const resetRequestForm = document.getElementById('reset-request-form');
    const emailVerificationForm = document.getElementById('email-verification-form');
    const createPasswordForm = document.getElementById('create-password-form');

    // Ẩn tất cả các form trước
    loginForm.classList.add('form-hidden');
    adminloginForm.classList.add('form-hidden');
    registerForm.classList.add('form-hidden');
    register2Form.classList.add('form-hidden');
    resetRequestForm.classList.add('form-hidden');
    emailVerificationForm.classList.add('form-hidden');
    createPasswordForm.classList.add('form-hidden');

    // Hiển thị form tương ứng dựa vào nextFormId
    if (nextFormId === 'login-form') {
        loginForm.classList.remove('form-hidden');
    } else if (nextFormId == 'admin-login-form') {
        adminloginForm.classList.remove('form-hidden');
    } else if (nextFormId === 'register-form') {
        registerForm.classList.remove('form-hidden');
    } else if (nextFormId === 'register2-form') {
        register2Form.classList.remove('form-hidden');
    } else if (nextFormId === 'reset-request-form') {
        resetRequestForm.classList.remove('form-hidden');
    } else if (nextFormId === 'email-verification-form') {
        emailVerificationForm.classList.remove('form-hidden');
    } else if (nextFormId === 'create-password-form') {
        createPasswordForm.classList.remove('form-hidden');
    } else {
        console.error(`Form với id "${nextFormId}" không tồn tại.`);
    }
}
