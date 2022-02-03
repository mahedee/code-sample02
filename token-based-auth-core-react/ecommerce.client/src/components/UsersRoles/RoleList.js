const RoleList = (props) => {

    debugger;
    const roles = props.roles;
    const usersRoles = props.usersRoles;

    console.log("Role list");
    console.log(roles);
    var isChecked = true;

    function isExists(roleName) {
        if (!usersRoles) return false;
        if (usersRoles.filter(p => p.roleName.includes(roleName)).length > 0)
            return true;
        else
            return true;
    }

    const roleList = (
        <div>
            <ul className="checkBoxList">
                {
                    //isChecked = roles.includes('Admin');
                    roles.map((role, index) => (


                        // (roles.filter(p => p.roleName.includes('Admin')).length > 0) ? 
                        // isChecked = true : isChecked = false

                        // {
                        //     console.log('found');
                        // } 
                        // : 
                        // {
                        //     //console.log('not found')

                        // }


                        <li key={index}>
                            <input type="checkbox" checked={isExists(role.roleName)} ></input>
                            <span class="input-group-addon">&nbsp;</span>
                            <label>{role.roleName}</label>
                        </li>
                    )
                    )
                }
            </ul>
        </div>
    );

    return roleList;
}

export default RoleList;