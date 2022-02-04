const RoleList = (props) => {

    //debugger;
    const roles = props.roles;
    const userRoles = props.userRoles;

    console.log("Role list : user roles");
    console.log(userRoles);
    var isChecked = true;

    function isExists(roleName) {

        console.log('role name' + roleName);
        console.log(userRoles);

        //if (!userRoles) return false;
        if (userRoles.length <= 0) return false;

        debugger;
        return userRoles.includes(roleName);
        // if (userRoles.filter(p => p.roleName.includes(roleName)).length > 0)
        //     return true;
        // else
        //     return false;
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