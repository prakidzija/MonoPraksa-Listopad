import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { apiRequest } from "../services/api";
import "./Registrations.css";

function Registrations() {
    const { playerId } = useParams();
    const [player, setPlayer] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        let active = true;

        async function fetchPlayer() {
            setLoading(true);
            setError("");
            setPlayer(null);

            try {
                const data = await apiRequest(`/Player/${encodeURIComponent(playerId)}`);
                if (active) setPlayer(data);
            }
            catch (err) {
                if (active) setError(err.message);
            }
            finally {
                if (active) setLoading(false);
            }
        }

        fetchPlayer();
        return () => { active = false; };
    }, [playerId]);

    const registrations = player?.playerRegistrations || [];

    return (
        <main className="player-registrations-container">
            <Link className="player-registrations-back" to="/players">Back to players</Link>
            <h1>{player ? `${player.name}'s registrations` : "Registrations"}</h1>

            {loading ? (
                <p role="status">Loading registrations...</p>
            ) : error ? (
                <p role="alert">Unable to load player registrations: {error}</p>
            ) : registrations.length === 0 ? (
                <p>This player has no registrations.</p>
            ) : (
                <div className="player-registrations-table-wrapper">
                    <table className="player-registrations-table">
                        <thead>
                            <tr>
                                <th scope="col">Club</th>
                                <th scope="col">Registration type</th>
                                <th scope="col">Jersey number</th>
                                <th scope="col">Start date</th>
                                <th scope="col">End date</th>
                            </tr>
                        </thead>
                        <tbody>
                            {registrations.map(registration => (
                                <tr key={registration.id}>
                                    <td>{registration.club?.name || "-"}</td>
                                    <td>{registration.registrationType?.replaceAll("_", " ") || "-"}</td>
                                    <td>{registration.jerseyNumber}</td>
                                    <td>{registration.startDate}</td>
                                    <td>{registration.endDate || "Ongoing"}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            )}
        </main>
    );
}

export default Registrations;
