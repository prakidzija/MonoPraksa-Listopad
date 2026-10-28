import { useEffect, useRef, useState } from "react";
import { apiRequest } from "../services/api";
import FilterButton from "../components/FilterButton";
import AddButton from "../components/AddButton";
import "./Clubs.css";
import styles from "./ClubsFilters.module.css";

const emptyFilters = { clubName: "", playerName: "" };

function Clubs() {
    const [clubs, setClubs] = useState([]);
    const [players, setPlayers] = useState([]);
    const [registrations, setRegistrations] = useState([]);
    const [filtersOpen, setFiltersOpen] = useState(false);
    const [draftFilters, setDraftFilters] = useState({ ...emptyFilters });
    const [filters, setFilters] = useState({ ...emptyFilters });
    const [addClubOpen, setAddClubOpen] = useState(false);
    const [newClub, setNewClub] = useState({ name: "", adress: "" });
    const [saving, setSaving] = useState(false);
    const [saveError, setSaveError] = useState("");
    const [saveMessage, setSaveMessage] = useState("");
    const savePending = useRef(false);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        let active = true;

        async function fetchClubs() {
            try {
                const [clubData, playerData, registrationData] = await Promise.all([
                    apiRequest("/Player/club"),
                    apiRequest("/Player/player"),
                    apiRequest("/Player/registration")
                ]);

                if (active) {
                    setClubs(clubData);
                    setPlayers(playerData);
                    setRegistrations(registrationData);
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

    const clubName = filters.clubName.trim().toLowerCase();
    const playerName = filters.playerName.trim().toLowerCase();
    const matchingPlayerIds = new Set(players
        .filter(player => (player.name || "").toLowerCase().includes(playerName))
        .map(player => player.id));
    const matchingClubIds = new Set(registrations
        .filter(registration => matchingPlayerIds.has(registration.playerId))
        .map(registration => registration.clubId));
    const filteredClubs = clubs.filter(club =>
        (club.name || "").toLowerCase().includes(clubName) &&
        (!playerName || matchingClubIds.has(club.id))
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
        setDraftFilters({ ...emptyFilters });
        setFilters({ ...emptyFilters });
    }

    function cancelAddClub() {
        setAddClubOpen(false);
        setNewClub({ name: "", adress: "" });
        setSaveError("");
    }

    function updateNewClub(event) {
        const { name, value } = event.target;
        setNewClub(previous => ({ ...previous, [name]: value }));
    }

    async function saveClub(event) {
        event.preventDefault();
        if (savePending.current) return;

        const name = newClub.name.trim();
        const adress = newClub.adress.trim();
        if (!name || name.length > 30 || !adress || adress.length > 40) {
            setSaveError("Enter a name (up to 30 characters) and address (up to 40 characters).");
            return;
        }

        savePending.current = true;
        setSaving(true);
        setSaveError("");
        setSaveMessage("");
        try {
            await apiRequest("/Player/club", {
                method: "POST",
                body: JSON.stringify({ name, adress })
            });
            cancelAddClub();
            setSaveMessage("Club added successfully. Active filters may hide the new club.");
            try {
                setClubs(await apiRequest("/Player/club"));
            }
            catch (err) {
                setSaveMessage(`Club added, but the list could not be refreshed. Reload the page to see it. ${err.message}`);
            }
        }
        catch (err) {
            setSaveError(err.message);
        }
        finally {
            savePending.current = false;
            setSaving(false);
        }
    }

    return (
        <div className="clubs-container">
            <h1 id="clubs-heading">Clubs</h1>
            {!loading && !error && (
                <>
                    <div className={styles.toolbar}>
                        <AddButton
                            label="Add club"
                            expanded={addClubOpen}
                            controls="clubs-add-card"
                            loading={saving}
                            onClick={() => {
                                if (addClubOpen) cancelAddClub();
                                else {
                                    setAddClubOpen(true);
                                    setSaveMessage("");
                                }
                            }}
                        />
                        <FilterButton
                            expanded={filtersOpen}
                            controls="clubs-filter-card"
                            disabled={clubs.length === 0}
                            onClick={() => setFiltersOpen(previous => !previous)}
                        />
                    </div>
                    {saveMessage && <p role="status">{saveMessage}</p>}
                    <form
                        id="clubs-add-card"
                        className={styles.card}
                        hidden={!addClubOpen}
                        onSubmit={saveClub}
                        aria-labelledby="clubs-add-heading"
                        aria-busy={saving}
                    >
                        <h2 id="clubs-add-heading">Add club</h2>
                        <p>Saving a club requires an Admin account.</p>
                        <fieldset className={styles.fieldset} disabled={saving}>
                            <div className={styles.fields}>
                                <label htmlFor="club-add-name">
                                    Name
                                    <input id="club-add-name" name="name" type="text" required maxLength={30} value={newClub.name} onChange={updateNewClub} />
                                </label>
                                <label htmlFor="club-add-address">
                                    Address
                                    <input id="club-add-address" name="adress" type="text" required maxLength={40} value={newClub.adress} onChange={updateNewClub} />
                                </label>
                            </div>
                            <div className={styles.actions}>
                                <AddButton type="submit" label="Save" loading={saving} />
                                <button type="button" className={`${styles.action} ${styles.reset}`} onClick={cancelAddClub}>Cancel</button>
                            </div>
                        </fieldset>
                        {saveError && <p role="alert">{saveError}</p>}
                    </form>
                    <form
                        id="clubs-filter-card"
                        className={styles.card}
                        hidden={!filtersOpen}
                        onSubmit={applyFilters}
                        aria-labelledby="clubs-filter-heading"
                    >
                        <h2 id="clubs-filter-heading">Filter clubs</h2>
                        <div className={styles.fields}>
                            <label htmlFor="club-filter-name">
                                Club name
                                <input id="club-filter-name" name="clubName" type="search" value={draftFilters.clubName} onChange={updateFilter} placeholder="Search by club name" />
                            </label>
                            <label htmlFor="club-filter-player">
                                Player name
                                <input id="club-filter-player" name="playerName" type="search" value={draftFilters.playerName} onChange={updateFilter} placeholder="Search by player name" />
                            </label>
                        </div>
                        <div className={styles.actions}>
                            <button type="submit" className={styles.action}>Apply</button>
                            <button type="button" className={`${styles.action} ${styles.reset}`} onClick={resetFilters}>Reset</button>
                        </div>
                    </form>
                    <p role="status">Showing {filteredClubs.length} of {clubs.length} clubs</p>
                </>
            )}
            {loading ? (
                <p role="status">Loading clubs...</p>
            ) : error ? (
                <p role="alert">Error loading clubs: {error}</p>
            ) : clubs.length === 0 ? (
                <p>There are no clubs in the database.</p>
            ) : filteredClubs.length === 0 ? (
                <p>No clubs match the selected filters.</p>
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
                            {filteredClubs.map(club => (
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
