const SessionManager = {

    getToken() {
        const token = sessionStorage.getItem('token');
        if (token) return token;
        else return null;
    },

    setUserSession(userName, token, userId, usersRole) {
        sessionStorage.setItem('userName', userName);
        sessionStorage.setItem('token', token);
        sessionStorage.setItem('userId', userId);
        sessionStorage.setItem('usersRole', usersRole);
    }
}

export default SessionManager;