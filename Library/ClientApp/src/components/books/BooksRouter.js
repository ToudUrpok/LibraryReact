﻿import React from 'react';
import { Switch, Route, useRouteMatch } from 'react-router-dom';
import AuthorizeRoute from '../api-authorization/AuthorizeRoute';

import { BooksList } from './BooksList';
import { BookAdd } from './BookAdd';
import { BookEdit } from './BookEdit';

export function BooksRouter() {
	let match = useRouteMatch();

	return (
		<Switch>
			<AuthorizeRoute path={`${match.path}/edit/:bookId`} component={BookEdit} />
			<AuthorizeRoute path={`${match.path}/add`} component={BookAdd} />
			<AuthorizeRoute path={`${match.path}`} component={BooksList} />
		</Switch>
	);
}