import { Routes, Route, Link } from "react-router-dom";

import Home from "./pages/Home";
import Login from "./pages/Login";
import Register from "./pages/Register";
import Players from "./pages/Players";
import Clubs from "./pages/Clubs";

function App() {
    return (
        <div>
            <nav>
                <Link to="/">HomePage</Link>{" | "}
                <Link to="/login">Prijava</Link>{" | "}
                <Link to="/register">Registracija</Link>{" | "}
                <Link to="/players">Igrači</Link>{" | "}
                <Link to="/clubs">Klubovi</Link>
            </nav>

            <hr />

            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register />} />
                <Route path="/players" element={<Players />} />
                <Route path="/clubs" element={<Clubs />} />
            </Routes>
        </div>
    );
}

export default App;
