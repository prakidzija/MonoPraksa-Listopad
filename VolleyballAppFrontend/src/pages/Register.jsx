import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { apiRequest } from "../services/api";

function Register() {
    const [username, setUsername] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    const [loading, setLoading] = useState(false);

    const navigate = useNavigate();

    async function handleRegister(event) {
        event.preventDefault();

        setError("");

        if (password.length < 8 || !/[A-Z]/.test(password) || !/[0-9]/.test(password)) {

            setError("Password must have at least 8 charactes, one uppercase letter and one number.");
            return;
        }

        setLoading(true);

        try {
            await apiRequest("/auth/register", {
                method: "POST",
                body: JSON.stringify({
                    username,
                    email,
                    password
                })
            });

            navigate("/login");

        }
        catch (err) {
            setError(err.message);
        }
        finally {
            setLoading(false);
        }
    }

    return (
        <div>
            <h1>Registration</h1>

            <form onSubmit={handleRegister}>
                <div>
                    <label htmlFor="register-username">Korisničko ime</label>
                    <input
                        id="register-username"
                        type="text"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                        maxLength={100}
                        required
                    />
                </div>

                <div>
                    <label htmlFor="register-email">Email</label>
                    <input
                        id="register-email"
                        type="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        maxLength={120}
                        required
                    />
                </div>

                <div>
                    <label htmlFor="register-password">Password</label>
                    <input
                        id="register-password"
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        minLength={8}
                        required
                    />
                </div>

                <button type="submit" disabled={loading}>
                    {loading ? "Registering..." : "Register"}
                </button>
            </form>

            {error && <p style={{ color: "red" }}>{error}</p>}
        </div>
    );
}

export default Register;