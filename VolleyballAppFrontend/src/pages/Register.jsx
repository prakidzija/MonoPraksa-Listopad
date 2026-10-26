function Register() {
    return (
        <div>
            <h1>Registration</h1>

            <input
                type="text"
                placeholder="Username"
            />

            <input
                type="email"
                placeholder="Email"
            />

            <input
                type="password"
                placeholder="Password"
            />

            <button type="button">
                Register
            </button>
        </div>
    );
}

export default Register;