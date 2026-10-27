import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { apiRequest } from "../services/api";
import "./PlayerRegistrations.css";

const emptyForm = {
    playerId: "", clubId: "", registrationType: "permanent",
    jerseyNumber: "", startDate: "", endDate: ""
};

function PlayerRegistrations() {
    const [players, setPlayers] = useState([]);
    const [clubs, setClubs] = useState([]);
    const [form, setForm] = useState({ ...emptyForm });
    const [loading, setLoading] = useState(true);
    const [loadError, setLoadError] = useState("");
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState("");
    const [message, setMessage] = useState("");

    useEffect(() => {
        let active = true;
        async function loadOptions() {
            try {
                const [playerData, clubData] = await Promise.all([
                    apiRequest("/Player/player"), apiRequest("/Player/club")
                ]);
                if (active) {
                    setPlayers(playerData);
                    setClubs(clubData);
                }
            }
            catch (err) {
                if (active) setLoadError(err.message);
            }
            finally {
                if (active) setLoading(false);
            }
        }
        loadOptions();
        return () => { active = false; };
    }, []);

    function updateField(event) {
        const { name, value } = event.target;
        setForm(previous => ({ ...previous, [name]: value }));
        setMessage("");
        setError("");
    }

    async function registerPlayer(event) {
        event.preventDefault();
        if (saving) return;
        setError("");
        setMessage("");
        const jerseyNumber = Number(form.jerseyNumber);
        if (!form.playerId || !form.clubId || !form.startDate || form.jerseyNumber === "" ||
            !Number.isInteger(jerseyNumber) || jerseyNumber < 1 || jerseyNumber > 99 ||
            (form.endDate && form.endDate < form.startDate)) {
            setError("Complete the required fields with a whole-number jersey number from 1 to 99 and valid dates.");
            return;
        }
        setSaving(true);
        try {
            await apiRequest("/Player/registration", {
                method: "POST",
                body: JSON.stringify({ ...form, jerseyNumber, endDate: form.endDate || null })
            });
            setForm({ ...emptyForm });
            setMessage("Player registered with the club successfully.");
        }
        catch (err) {
            setError(err.message);
        }
        finally {
            setSaving(false);
        }
    }

    return (
        <div className="registrations-container">
            <h1>Player club registration</h1>
            <p>Register an existing player with a club. Saving requires an Admin account.</p>
            {loading ? <p role="status">Loading players and clubs...</p> :
                loadError ? <p role="alert">Unable to load registration options: {loadError}</p> :
                    players.length === 0 || clubs.length === 0 ? (
                        <p>Add <Link to="/players">players</Link> and <Link to="/clubs">clubs</Link> before creating a registration.</p>
                    ) : (
                        <form className="registrations-card" onSubmit={registerPlayer} aria-busy={saving}>
                            <fieldset disabled={saving}>
                                <legend>Registration details</legend>
                                <div className="registrations-fields">
                                    <label htmlFor="registration-player">Player
                                        <select id="registration-player" name="playerId" required value={form.playerId} onChange={updateField}>
                                            <option value="">Select a player</option>
                                            {players.map(player => <option key={player.id} value={player.id}>{player.name} (age {player.age}, {player.position || "No position"})</option>)}
                                        </select>
                                    </label>
                                    <label htmlFor="registration-club">Club
                                        <select id="registration-club" name="clubId" required value={form.clubId} onChange={updateField}>
                                            <option value="">Select a club</option>
                                            {clubs.map(club => <option key={club.id} value={club.id}>{club.name}</option>)}
                                        </select>
                                    </label>
                                    <label htmlFor="registration-type">Registration type
                                        <select id="registration-type" name="registrationType" required value={form.registrationType} onChange={updateField}>
                                            <option value="permanent">Permanent</option>
                                            <option value="loan">Loan</option>
                                            <option value="dual_license">Dual license</option>
                                        </select>
                                    </label>
                                    <label htmlFor="registration-jersey">Jersey number
                                        <input id="registration-jersey" name="jerseyNumber" type="number" min="1" max="99" step="1" required value={form.jerseyNumber} onChange={updateField} />
                                    </label>
                                    <label htmlFor="registration-start">Start date
                                        <input id="registration-start" name="startDate" type="date" required value={form.startDate} onChange={updateField} />
                                    </label>
                                    <label htmlFor="registration-end">End date (optional)
                                        <input id="registration-end" name="endDate" type="date" min={form.startDate || undefined} value={form.endDate} onChange={updateField} />
                                    </label>
                                </div>
                                <p>Choose a jersey number from 1 to 99. Numbers can be reused at the same club when registration dates do not overlap. The end date is included; a blank end date reserves the number indefinitely.</p>
                                <div className="registrations-actions">
                                    <button type="submit">{saving ? "Registering..." : "Register player"}</button>
                                    <button type="button" onClick={() => {
                                        setForm({ ...emptyForm });
                                        setError("");
                                        setMessage("");
                                    }}>Reset</button>
                                </div>
                            </fieldset>
                            {error && <p role="alert">{error}</p>}
                            {message && <p role="status">{message}</p>}
                        </form>
                    )}
        </div>
    );
}

export default PlayerRegistrations;
