import { useEffect, useState } from "react";

function Home() {
    const [message, setMessage] = useState("Connecting...");
    const [error, setError] = useState("");

    useEffect(() => {
        fetch("/api/health")
            .then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP ${response.status}`);
                }

                return response.json();
            })
            .then(data => setMessage(data.message))
            .catch(err => setError(err.message));
    }, []);

    return (
        <div>
            <h1>Volleyball App</h1>
            <h2>Connection check</h2>

            {error ? (
                <p>Error: {error}</p>
            ) : (
                <p>{message}</p>
            )}
        </div>
    );
}

export default Home;