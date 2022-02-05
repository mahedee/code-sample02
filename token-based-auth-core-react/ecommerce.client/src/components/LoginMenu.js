import { Link } from 'react-router-dom';
import {
    NavLink
} from 'reactstrap';

const LoginMenu = (props) => {

    //debugger;
    const menuText = props.menuText;
    const menuURL = props.menuURL;

    console.log(menuText);
    console.log(menuURL);
    const loginMenu = (
        menuText && menuURL ? (
            <NavLink tag={Link} className='text-dark' to={menuURL}>{menuText}</NavLink>
        ) : (
            <NavLink tag={Link} className='text-dark' to="/login">Login</NavLink>
        )
    )

    return loginMenu;

}

export default LoginMenu;