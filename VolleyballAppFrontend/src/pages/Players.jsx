import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import "./Players.css";
import { apiRequest } from "../services/api";

function Players() {
    const [players, setPlayers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");
    const [filtersOpen, setFiltersOpen] = useState(false);
    const [draftFilters, setDraftFilters] = useState({ name: "", age: "", position: "", club: "" });
    const [filters, setFilters] = useState({ name: "", age: "", position: "", club: "" });
    const [addPlayerOpen, setAddPlayerOpen] = useState(false);
    const [newPlayer, setNewPlayer] = useState({ name: "", age: "", position: "" });
    const [saving, setSaving] = useState(false);
    const [saveError, setSaveError] = useState("");
    const [saveMessage, setSaveMessage] = useState("");

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


    const positions = [...new Set(players.map(player => player.position).filter(Boolean))].sort();
    const clubs = [...new Set(players.flatMap(player => player.clubs || []))].sort();
    const filteredPlayers = players.filter(player =>
        player.name.toLowerCase().includes(filters.name.trim().toLowerCase()) &&
        (filters.age === "" || player.age === Number(filters.age)) &&
        (!filters.position || player.position === filters.position) &&
        (!filters.club || (filters.club === "__no_club__"
            ? !player.clubs?.length
            : player.clubs?.includes(filters.club)))
    );

    function updateFilter(event) {
        const { name, value } = event.target;
        setDraftFilters(previous => ({ ...previous, [name]: value }));
    }

    function applyFilters(event) {
        event.preventDefault();
        setFilters({ ...draftFilters });
    }

    function resetFilters() {
        const emptyFilters = { name: "", age: "", position: "", club: "" };
        setDraftFilters(emptyFilters);
        setFilters(emptyFilters);
    }

    function updateNewPlayer(event) {
        const { name, value } = event.target;
        setNewPlayer(previous => ({ ...previous, [name]: value }));
    }

    function cancelAddPlayer() {
        setAddPlayerOpen(false);
        setNewPlayer({ name: "", age: "", position: "" });
        setSaveError("");
    }

    async function savePlayer(event) {
        event.preventDefault();
        if (saving) return;

        const name = newPlayer.name.trim();
        const age = Number(newPlayer.age);
        if (!name || name.length > 30 || !Number.isInteger(age) || age < 1 || age > 2147483647 || !newPlayer.position) {
            setSaveError("Enter a name (up to 30 characters), a positive whole-number age, and a position.");
            return;
        }

        setSaving(true);
        setSaveError("");
        setSaveMessage("");

        try {
            await apiRequest("/Player", {
                method: "POST",
                body: JSON.stringify({ name, age, position: newPlayer.position })
            });
        }
        catch (err) {
            setSaveError(err.message);
            setSaving(false);
            return;
        }

        cancelAddPlayer();
        setSaveMessage("Player added successfully.");

        try {
            const data = await apiRequest("/Player/player");
            setPlayers(data);
        }
        catch (err) {
            setSaveMessage(`Player added, but the list could not be refreshed. Reload the page to see it. ${err.message}`);
        }
        finally {
            setSaving(false);
        }
    }

    return (
        <div className="players-container">
            <h1>Player List</h1>
            <p className="players-subtitle">
                All Players
            </p>

            <div className="players-toolbar">
                <button
                    type="button"
                    className="players-filter-button"
                    aria-expanded={addPlayerOpen}
                    aria-controls="players-add-card"
                    disabled={saving}
                    onClick={() => {
                        if (addPlayerOpen) {
                            cancelAddPlayer();
                        } else {
                            setAddPlayerOpen(true);
                            setSaveMessage("");
                        }
                    }}
                >
                    Add player
                </button>
                <button
                    type="button"
                    className="players-filter-button"
                    aria-expanded={filtersOpen}
                    aria-controls="players-filter-card"
                    onClick={() => setFiltersOpen(previous => !previous)}
                >
                    Filter
                </button>
            </div>

            {saveMessage && <p role="status">{saveMessage}</p>}

            {addPlayerOpen && (
                <form id="players-add-card" className="players-filter-card" onSubmit={savePlayer} aria-labelledby="players-add-heading" aria-busy={saving}>
                    <h2 id="players-add-heading">Add player</h2>
                    <p>Saving a player requires an Admin account.</p>
                    <fieldset className="players-add-fieldset" disabled={saving}>
                        <div className="players-filter-fields">
                            <label htmlFor="player-add-name">
                                Name
                                <input id="player-add-name" name="name" type="text" maxLength="30" required value={newPlayer.name} onChange={updateNewPlayer} placeholder="Player name" />
                            </label>
                            <label htmlFor="player-add-age">
                                Age
                                <input id="player-add-age" name="age" type="number" min="1" max="2147483647" step="1" required value={newPlayer.age} onChange={updateNewPlayer} placeholder="Player age" />
                            </label>
                            <label htmlFor="player-add-position">
                                Position
                                <select id="player-add-position" name="position" required value={newPlayer.position} onChange={updateNewPlayer}>
                                    <option value="">Select a position</option>
                                    {["Setter", "Libero", "Middle", "Opposite", "Outside"].map(position => (
                                        <option key={position} value={position}>{position}</option>
                                    ))}
                                </select>
                            </label>
                        </div>
                        <div className="players-filter-actions">
                            <button type="submit" className="players-filter-button">{saving ? "Saving..." : "Save"}</button>
                            <button type="button" className="players-filter-reset" onClick={cancelAddPlayer}>Cancel</button>
                        </div>
                    </fieldset>
                    {saveError && <p role="alert">{saveError}</p>}
                </form>
            )}

            {filtersOpen && (
                <form id="players-filter-card" className="players-filter-card" onSubmit={applyFilters}>
                    <h2>Filter players</h2>
                    <div className="players-filter-fields">
                        <label htmlFor="player-filter-name">
                            Name
                            <input id="player-filter-name" name="name" type="search" value={draftFilters.name} onChange={updateFilter} placeholder="Search by name" />
                        </label>
                        <label htmlFor="player-filter-age">
                            Age
                            <input id="player-filter-age" name="age" type="number" min="1" step="1" value={draftFilters.age} onChange={updateFilter} placeholder="Any age" />
                        </label>
                        <label htmlFor="player-filter-position">
                            Position
                            <select id="player-filter-position" name="position" value={draftFilters.position} onChange={updateFilter}>
                                <option value="">All positions</option>
                                {positions.map(position => <option key={position} value={position}>{position}</option>)}
                            </select>
                        </label>
                        <label htmlFor="player-filter-club">
                            Club
                            <select id="player-filter-club" name="club" value={draftFilters.club} onChange={updateFilter}>
                                <option value="">All clubs</option>
                                <option value="__no_club__">No club</option>
                                {clubs.map(club => <option key={club} value={club}>{club}</option>)}
                            </select>
                        </label>
                    </div>
                    <div className="players-filter-actions">
                        <button type="submit" className="players-filter-button">Apply</button>
                        <button type="button" className="players-filter-reset" onClick={resetFilters}>Reset</button>
                    </div>
                </form>
            )}

            <p role="status">Showing {filteredPlayers.length} of {players.length} players</p>

        {players.length === 0
            ? <p>There are no players in database.</p>
            : filteredPlayers.length === 0
                ? <p>No players match the selected filters.</p>
                : (
                    <div className="table-wrapper">
                        <table className="players-table">
                            <thead>
                                <tr>
                                    <th scope="col">Name</th>
                                    <th scope="col">Age</th>
                                    <th scope="col">Position</th>
                                    <th scope="col">Clubs</th>
                                </tr>
                            </thead>

                            <tbody>
                                {filteredPlayers.map(player => (
                                    <tr key={player.id}>
                                        <td><Link className="players-name-link" to={`/players/${player.id}/registrations`}>{player.name}</Link></td>
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
