import { Link } from "react-router-dom";
import './NavBar.css';

export default function NavBar() {
    return (
        <nav>
            <ul>
                <li>
                    <Link to="/todo">Todo</Link>
                </li>
            </ul>
        </nav>
    );
}