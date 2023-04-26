import React from 'react';
import { Switch, Route, useRouteMatch } from 'react-router-dom';
import AuthorizeRoute from '../api-authorization/AuthorizeRoute';

//import { BooksList } from './BooksList';
import { SessionRequestAdd } from './SessionRequestAdd';

export function SessionsRouter() {
	let match = useRouteMatch();

	return (
		<Switch>
			<AuthorizeRoute path={`${match.path}/request/add/:bookId`} component={SessionRequestAdd} />
			{/*<AuthorizeRoute path={`${match.path}`} component={BooksList} />*/}
		</Switch>
	);
}