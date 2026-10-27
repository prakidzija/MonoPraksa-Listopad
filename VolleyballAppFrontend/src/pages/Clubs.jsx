import { useEffect, useState } from "react";
import { apiRequest } from "../services/api";
import "./Clubs.css";

function Clubs() {
    const [clubs, setClubs] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        let active = true;

        async function fetchClubs() {
            try {
                const data = await apiRequest("/Player/club");

                if (active) {
                    setClubs(data);
                }
            }
            catch (err) {
                if (active) {
                    setError(err.message);
                }
            }
            finally {
                if (active) {
                    setLoading(false);
                }
            }
        }

        fetchClubs();

        return () => {
            active = false;
        };
    }, []);

    return (
        <div className="clubs-container">
            <h1 id="clubs-heading">Clubs</h1>
            {loading ? (
                <p role="status">Loading clubs...</p>
            ) : error ? (
                <p role="alert">Error loading clubs: {error}</p>
            ) : clubs.length === 0 ? (
                <p>There are no clubs in the database.</p>
            ) : (
                <div className="clubs-table-wrapper">
                    <table className="clubs-table" aria-labelledby="clubs-heading">
                        <thead>
                            <tr>
                                <th scope="col">Name</th>
                                <th scope="col">Address</th>
                            </tr>
                        </thead>
                        <tbody>
                            {clubs.map(club => (
                                <tr key={club.id}>
                                    <td>{club.name}</td>
                                    <td>{club.adress || "-"}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            )}
        </div>
    );
}

export default Clubs;
