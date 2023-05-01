import authService from '../api-authorization/AuthorizeService';
import { ApplicationPaths, QueryParameterNames } from '../api-authorization/ApiAuthorizationConstants'

export class BooksService {
	async getBooks(page, pageSize, sortOrder, searchString) {
		const token = await authService.getAccessToken();
		const offset = (page - 1) * pageSize;

		// Taken from here: https://fetch.spec.whatwg.org/#fetch-api
		let url = "api/books?" + "offset=" + offset + "&limit=" + pageSize + "&sortOrder=" + sortOrder;
		if (searchString !== "")
			url += "&searchString=" + encodeURIComponent(searchString);

		try {
			const response = await fetch(url, {
				headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
			});

			if (response.ok) {
				const jsonData = await response.json();
				return jsonData;
			}
			else {
				if (response.status == 401) { // Refresh access token if it is expired
					window.location.href =
						`${ApplicationPaths.Login}?${QueryParameterNames.ReturnUrl}=${encodeURI(window.location.href)}`;
				}

				throw new Error("HTTP error! Code: " + response.status);
			}
		}
		catch (error) {
			console.log(error);
			throw error;
		}
	}

	async getBookItems(page, pageSize, sortOrder, searchString) {
		const offset = (page - 1) * pageSize;

		// Taken from here: https://fetch.spec.whatwg.org/#fetch-api
		let url = "api/home?" + "offset=" + offset + "&limit=" + pageSize + "&sortOrder=" + sortOrder;
		if (searchString !== "")
			url += "&searchString=" + encodeURIComponent(searchString);

		try {
			const response = await fetch(url);

			if (response.ok) {
				const jsonData = await response.json();
				return jsonData;
			}
			else {
				throw new Error("HTTP error! Code: " + response.status);
			}
		}
		catch (error) {
			console.log(error);
			throw error;
		}
	}

	//async deleteUser(userId) {
	//	const token = await authService.getAccessToken();

	//	try {
	//		const headers = { 'content-type': 'application/json' };
	//		if (token) headers['Authorization'] = `Bearer ${token}`;

	//		const response = await fetch('api/users/' + userId, {
	//			method: 'DELETE',
	//			headers: headers,
	//		});

	//		if (response.ok) {
	//			return;
	//		}
	//		else {
	//			throw new Error("HTTP error! Code: " + response.status);
	//		}
	//	}
	//	catch (error) {
	//		console.log(error);
	//		throw error;
	//	}
	//}

	async getBook(bookId) {
		const token = await authService.getAccessToken();

		try {
			const response = await fetch('api/books/' + bookId, {
				headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
			});

			if (response.ok) {
				const jsonData = await response.json();
				return jsonData;
			}
			else {
				throw new Error("HTTP error! Code: " + response.status);
			}
		}
		catch (error) {
			console.log(error);
			throw error;
		}
	}

	async addBook(book) {
		const token = await authService.getAccessToken();

		try {
			const headers = { 'content-type': 'application/json' };
			if (token) headers['Authorization'] = `Bearer ${token}`;

			const response = await fetch('api/books', {
				method: 'POST',
				body: JSON.stringify(book),
				headers: headers,
			});

			if (response.ok) {
				return;
			}
			else {
				throw new Error("HTTP error! Code: " + response.status);
			}
		}
		catch (error) {
			console.log(error);
			throw error;
		}
	}

	async updateBook(bookId, book) {	//bookId ?
		const token = await authService.getAccessToken();

		try {
			const headers = { 'content-type': 'application/json' };
			if (token) headers['Authorization'] = `Bearer ${token}`;

			const response = await fetch('api/books/' + bookId, {
				method: 'PUT',
				body: JSON.stringify(book),
				headers: headers,
			});

			if (response.ok) {
				return;
			}
			else {
				throw new Error("HTTP error! Code: " + response.status);
			}
		}
		catch (error) {
			console.log(error);
			throw error;
		}
	}

	static get instance() { return booksService; }
}

const booksService = new BooksService();

export default booksService;