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
    const [registrations, setRegistrations] = useState([]);
    const [mode, setMode] = useState("");
    const [selectedId, setSelectedId] = useState("");
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
                const [playerData, clubData, registrationData] = await Promise.all([
                    apiRequest("/Player/player"), apiRequest("/Player/club"), apiRequest("/Player/registration")
                ]);
                if (active) {
                    setPlayers(playerData);
                    setClubs(clubData);
                    setRegistrations(registrationData);
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

    function chooseMode(nextMode) {
        setMode(nextMode);
        setSelectedId("");
        setForm({ ...emptyForm });
        setError("");
        setMessage("");
    }

    function selectRegistration(event) {
        const id = event.target.value;
        const registration = registrations.find(item => item.id === id);
        setSelectedId(id);
        setForm(registration ? {
            playerId: registration.playerId,
            clubId: registration.clubId,
            registrationType: registration.registrationType,
            jerseyNumber: String(registration.jerseyNumber),
            startDate: registration.startDate,
            endDate: registration.endDate || ""
        } : { ...emptyForm });
        setError("");
        setMessage("");
    }

    function resetForm() {
        if (mode === "edit" && selectedId) {
            const registration = registrations.find(item => item.id === selectedId);
            if (registration) {
                setForm({
                    playerId: registration.playerId,
                    clubId: registration.clubId,
                    registrationType: registration.registrationType,
                    jerseyNumber: String(registration.jerseyNumber),
                    startDate: registration.startDate,
                    endDate: registration.endDate || ""
                });
            }
        } else {
            setForm({ ...emptyForm });
        }
        setError("");
        setMessage("");
    }

    async function saveRegistration(event) {
        event.preventDefault();
        if (saving || (mode === "edit" && !selectedId)) return;
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
            await apiRequest(mode === "edit" ? `/Player/registration/${selectedId}` : "/Player/registration", {
                method: mode === "edit" ? "PUT" : "POST",
                body: JSON.stringify({ ...form, jerseyNumber, endDate: form.endDate || null })
            });
            if (mode === "create") setForm({ ...emptyForm });
            setMessage(mode === "edit" ? "Registration updated successfully." : "Player registered with the club successfully.");
            try {
                setRegistrations(await apiRequest("/Player/registration"));
            }
            catch (err) {
                setMessage(`Registration saved, but the list could not be refreshed. Reload the page to see it. ${err.message}`);
            }
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
            <h1>Player Registration</h1>
            <p>Create a registration or edit an existing one.</p>
            {loading ? <p role="status">Loading registration options...</p> :
                loadError ? <p role="alert">Unable to load registration options: {loadError}</p> :
                    players.length === 0 || clubs.length === 0 ? (
                        <p>Add <Link to="/players">players</Link> and <Link to="/clubs">clubs</Link> before managing registrations.</p>
                    ) : (
                        <>
                        <div className="registrations-choice" role="group" aria-label="Registration action">
                            <button type="button" aria-pressed={mode === "create"} disabled={saving} onClick={() => chooseMode("create")}>New registration</button>
                            <button type="button" aria-pressed={mode === "edit"} disabled={saving} onClick={() => chooseMode("edit")}>Edit existing registration</button>
                        </div>
                        {mode === "edit" && (
                            registrations.length === 0 ? <p>There are no registrations to edit.</p> : (
                                <label className="registrations-picker" htmlFor="registration-existing">
                                    Select a registration to edit
                                    <select id="registration-existing" value={selectedId} disabled={saving} onChange={selectRegistration}>
                                        <option value="">Select a registration</option>
                                        {registrations.map(registration => (
                                            <option key={registration.id} value={registration.id}>
                                                {players.find(player => player.id === registration.playerId)?.name || "Unknown player"} — {clubs.find(club => club.id === registration.clubId)?.name || "Unknown club"} — {registration.startDate} (#{registration.jerseyNumber})
                                            </option>
                                        ))}
                                    </select>
                                </label>
                            )
                        )}
                        {(mode === "create" || (mode === "edit" && selectedId)) && (
                        <form className="registrations-card" onSubmit={saveRegistration} aria-busy={saving}>
                            <fieldset disabled={saving}>
                                <legend>{mode === "edit" ? "Edit registration details" : "New registration details"}</legend>
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
                                <p>Choose a jersey number from 1 to 99.</p>
                                <div className="registrations-actions">
                                    <button type="submit">{saving ? "Saving..." : mode === "edit" ? "Save changes" : "Register player"}</button>
                                    <button type="button" onClick={resetForm}>Reset</button>
                                    <button type="button" onClick={() => chooseMode("")}>Cancel</button>
                                </div>
                            </fieldset>
                            {error && <p role="alert">{error}</p>}
                            {message && <p role="status">{message}</p>}
                        </form>
                        )}
                        </>
                    )}
        </div>
    );
}

export default PlayerRegistrations;
