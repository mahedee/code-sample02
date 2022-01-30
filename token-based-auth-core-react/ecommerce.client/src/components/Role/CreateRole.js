import { Component } from "react";

export default class CreateRole extends Component{
    constructor(props){
        super(props);
        this.state = {
            roleName: '',
            loading: true
        };

        this.onSubmit = this.onSubmit.bind(this);

    }

    onSubmit(e){
        e.preventDefault();
        const{history} = this.props;

        let roleObj = {
            roleName: this.state.roleName
        }
    }

    onChange(e){
        this.setState({[e.target.name]: e.target.value});
    }

    render(){
        return(
            <div className="row">
            <div className="col-md-4">
                <h3>Create new role</h3>
                <form onSubmit={this.onSubmit}>
                    <div className="form-group">
                        <label className="control-label">Role Name: </label>
                        <input className="form-control" type="text" name="roleName" value={this.state.roleName} onChange={this.onChange}></input>
                    </div>
                    <div className="form-group">
                        <input type="submit" value="Add Role" className="btn btn-primary"></input>
                    </div>

                </form>

            </div>
        </div>
        );
    }
}