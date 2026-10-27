import { Routes, Route, Link } from "react-router-dom";

import Home from "./pages/Home";
import Login from "./pages/Login";
import Register from "./pages/Register";
import Players from "./pages/Players";
import Clubs from "./pages/Clubs";
import PlayerRegistrations from "./pages/PlayerRegistrations";

function App() {
    return (
        <div>
            <nav>
                <Link to="/">HomePage</Link>{" | "}
                <Link to="/login">Login</Link>{" | "}
                <Link to="/register">Registration</Link>{" | "}
                <Link to="/players">Players</Link>{" | "}
                <Link to="/clubs">Clubs</Link>{" | "}
                <Link to="/player-registrations">Player club registration</Link>
            </nav>

            <hr />

            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register />} />
                <Route path="/players" element={<Players />} />
                <Route path="/clubs" element={<Clubs />} />
                <Route path="/player-registrations" element={<PlayerRegistrations />} />
            </Routes>
        </div>
    );
}

export default App;
