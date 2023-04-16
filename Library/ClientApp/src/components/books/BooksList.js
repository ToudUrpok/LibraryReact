import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import booksService from '../books/BooksService';
import { Pager } from '../Pager';
import { FormGroup, Form, Input, Button } from 'reactstrap';

export class BooksList extends Component {
	constructor(props) {
		super(props);
		this.state = { books: [], page: 1, pageSize: 10, maxPages: 5, sortOrder: "Name", searchString: "", loading: true };
	}

	componentDidMount() {
		this.populateBooksData();
	}

	handlePageChange = (page) => {
		this.setState({ page: page }, () => this.populateBooksData());
	}

	handleHeaderClick = (event, header) => {
		event.preventDefault();

		let newSortOrder = this.state.sortOrder;

		switch (header) {
			case 'Name': {
				newSortOrder = this.state.sortOrder === 'Name' ? 'Name_desc' : 'Name';
				break;
			}
			case 'Author': {
				newSortOrder = this.state.sortOrder === 'Author' ? 'Author_desc' : 'Author';
				break;
			}
			case 'Genre': {
				newSortOrder = this.state.sortOrder === 'Genre' ? 'Genre_desc' : 'Genre';
				break;
			}
		}

		this.setState({ page: 1, sortOrder: newSortOrder }, () => this.populateBooksData());
		return false;
	}

	handleInputChange = (event) => {
		const target = event.target;
		const value = target.type === 'checkbox' ? target.checked : target.value;
		const name = target.name;

		this.setState({ [name]: value });
	}

	handleSearchFormSubmit = (event) => {
		event.preventDefault();
		this.populateBooksData();
	}

	handleSearchFormReset = (event) => {
		event.preventDefault();
		this.setState({ page: 1, searchString: "" }, () => this.populateBooksData());
	}

	renderBooksTable() {
		const { books, totalBooks, sortOrder } = this.state;

		return (
			<div>
				<table className='table table-striped' aria-labelledby="tableLabel">
					<thead>
						<tr>
							<th>
								<a href="#" onClick={(e) => this.handleHeaderClick(e, 'Name')} >
									Name
									{sortOrder == 'Name' && <span>&#8897;</span>}
									{sortOrder == 'Name_desc' && <span>&#8896;</span>}
								</a>
							</th>
							<th>
								<a href="#" onClick={(e) => this.handleHeaderClick(e, 'Author')} >
									Author
									{sortOrder == 'Author' && <span>&#8897;</span>}
									{sortOrder == 'Author_desc' && <span>&#8896;</span>}
								</a>
							</th>
							<th>
								<a href="#" onClick={(e) => this.handleHeaderClick(e, 'Genre')} >
									Genre
									{sortOrder == 'Genre' && <span>&#8897;</span>}
									{sortOrder == 'Genre_desc' && <span>&#8896;</span>}
								</a>
							</th>
							<th>Publishing year</th>
							<th>ISBN</th>
							<th />
						</tr>
					</thead>
					<tbody>
						{books.map(book =>
							<tr key={book.id}>
								<td>{book.name}</td>
								<td>{book.authors}</td>
								<td>{book.genre}</td>
								<td>{book.year}</td>
								<td>{book.isbn}</td>
								<td><Link to={'/books/edit/' + book.id}>Edit</Link></td>
							</tr>
						)}
					</tbody>
				</table>
				<Pager totalItems={totalBooks} page={this.state.page} pageSize={this.state.pageSize} maxPages={this.state.maxPages} handlePageChange={this.handlePageChange} />
			</div>
		);
	}

	render() {
		let contents = this.state.loading
			? <p><em>Loading...</em></p>
			: this.renderBooksTable();

		return (
			<div>
				<h1 id="tableLabel">Books catalogue</h1>
				<Link to='/books/add/'>Add Book</Link>
				<Form inline onSubmit={this.handleSearchFormSubmit}>
					<FormGroup>
						<Input type="text" name="searchString" value={this.state.searchString} onChange={this.handleInputChange} placeholder="Book's Name or Author" />
					</FormGroup>&nbsp;
					<Button>Search</Button>&nbsp;
					<Button onClick={this.handleSearchFormReset}>Reset</Button>
				</Form><br />
				{contents}
			</div>
		);
	}

	async populateBooksData() {
		try {

			const data = await booksService.getBooks(this.state.page, this.state.pageSize, this.state.sortOrder, this.state.searchString);
			console.log(data.books);
			this.setState({ totalBooks: data.totalBooks, books: data.books, loading: false });
		}
		catch (error) {
			// Handle error
		}
	}
}