import { Component } from "react";
import { getData, putData } from "../services/AccessAPI";

export default class EditRole extends Component {
    constructor(props) {
        super(props);
        this.state = {
            id: '',
            roleName: '',
        };

        this.onChange = this.onChange.bind(this);
        this.onSubmit = this.onSubmit.bind(this);

    }

    onChange(e) {
        this.setState({ [e.target.name]: e.target.value });
    }

 
    componentDidMount() {
        const { id } = this.props.match.params;
        getData('api/Role/' + id).then(
            (result) => {
                if (result) {
                    this.setState({
                        id: result.id,
                        roleName: result.roleName,
                        loading: false
                    });
                }
            }
        );
    }

    onSubmit(e) {
        e.preventDefault();
        const { history } = this.props;
        const { id } = this.props.match.params;

        let roleObj = {
            id: this.state.id,
            roleName: this.state.roleName
        }

        putData('api/Role/Edit/' + id, roleObj).then((result) => {
            let responseJson = result;
            //console.log("update response: ");
            if (responseJson) {
                console.log(responseJson);
                history.push('/admin/roles');
            }
        }

        );
    }

    render() {
        return (
            <div className="row">
                <div className="col-md-4">
                    <h3>Edit Role</h3>
                    <form onSubmit={this.onSubmit}>
                        <div className="form-group">
                            <label className="control-label">Role Name: </label>
                            <input className="form-control" type="text" value={this.state.roleName} onChange={this.onChange} name="roleName"
                                onKeyDown={this.onKeyDown} ></input>
                        </div>

                        <div className="form-group">
                            <button onClick={this.onUpdateCancel} className="btn btn-default">Cancel</button>
                            <input type="submit" value="Edit" className="btn btn-primary"></input>
                        </div>

                    </form>

                </div>
            </div>
        );
    }
}