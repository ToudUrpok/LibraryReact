import React, { Component } from 'react';
import { BookItemsList } from './books/BookItemsList';
import authService from './api-authorization/AuthorizeService'
import { UserRoles } from './api-authorization/ApiAuthorizationConstants';
import { withTranslation } from 'react-i18next';

export class HomePlain extends Component {
    static displayName = HomePlain.name;

	constructor(props) {
		super(props);

		this.state = {
			isAuthenticated: false,
			hasAdminRole: false,
			hasLibrarianRole: false,
			hasUserRole: false
		};
	}

	componentDidMount() {
		this._subscription = authService.subscribe(() => this.populateState());
		this.populateState();
	}

	async populateState() {
		const isAuthenticated = await authService.isAuthenticated();
		const hasAdminRole = false;
		const hasLibrarianRole = false;
		const hasUserRole = false;

		if (isAuthenticated) {
			hasAdminRole = await authService.hasRole(UserRoles.Administrator);
			hasLibrarianRole = await authService.hasRole(UserRoles.Librarian);
			hasUserRole = await authService.hasRole(UserRoles.User);
		}

		this.setState({
			isAuthenticated,
			hasAdminRole,
			hasLibrarianRole,
			hasUserRole
		});
	}

	render() {
		const { t, i18n } = this.props;
		return (
			<div>
				{
					(!this.state.isAuthenticated ||  this.state.hasUserRole) &&
					<BookItemsList />
				}
				{
					this.state.hasAdminRole &&
					<div>
						<h1>Admin home page</h1>
					</div>
				}
				{
					this.state.hasLibrarianRole &&
					<div>
						<h1>Librarian home page</h1>
					</div>
				}
			</div>
		);
	}
}

export const Home = withTranslation()(HomePlain);
