import authService from '../api-authorization/AuthorizeService';
import { ApplicationPaths, QueryParameterNames } from '../api-authorization/ApiAuthorizationConstants'

export class SessionsService {
	async requestSession(bookId) {
		//const token = await authService.getAccessToken();

		//try {
		//	const headers = { 'content-type': 'application/json' };
		//	if (token) headers['Authorization'] = `Bearer ${token}`;

		//	const response = await fetch('api/books', {
		//		method: 'POST',
		//		body: JSON.stringify(book),
		//		headers: headers,
		//	});

		//	if (response.ok) {
		//		return;
		//	}
		//	else {
		//		throw new Error("HTTP error! Code: " + response.status);
		//	}
		//}
		//catch (error) {
		//	console.log(error);
		//	throw error;
		//}
	}

	get getSessionExpireDate() {
		return this.calcSessionExpireDate();
	}

	calcSessionExpireDate() {
		const curDate = Date.now();
		//current date + 15 days
		return curDate + (1000 * 60 * 60 * 24 * 15);
	}

	static get instance() { return sessionsService; }
}

const sessionsService = new SessionsService();

export default sessionsService;