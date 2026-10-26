import { useEffect, useState } from "react";
import "./Players.css";
import { apiRequest } from "../services/api";

function Players() {
    const [players, setPlayers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        let active = true;

        async function fetchPlayers() {
            try {
                const data = await apiRequest("/Player/player");

                if (active) {
                    setPlayers(data);
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

        fetchPlayers();

        return () => {
            active = false;
        };
    }, []);

    if (loading) {
        return <p>Loading players...</p>;
    }

    if (error) {
        return (
            <div>
                <h2>Error loading players</h2>
                <p>{error}</p>
            </div>
        );
    }


    return (
        <div className="players-container">
            <h1>Player List</h1>
            <p className="players-subtitle">
                All Players
            </p>

        {players.length === 0
            ? <p>There are no players in database.</p>
                : (
                    <div className="table-wrapper">
                        <table className="players-table">
                            <thead>
                                <th>
                                    <th>Name</th>
                                    <th>Age</th>
                                    <th>Position</th>
                                    <th>Clubs</th>
                                </th>
                            </thead>

                            <tbody>
                                {players.map(player => (
                                    <tr key={player.id}>
                                        <td>{player.name}</td>
                                        <td>{player.age}</td>
                                        <td>{player.position || "-"}</td>
                                        <td>{player.clubs?.join(", ") || "-"}</td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                </div>
            )}
    </div>
    );
}

export default Players;