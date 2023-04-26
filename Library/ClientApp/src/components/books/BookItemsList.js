import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import booksService from '../books/BooksService';
import { Pager } from '../Pager';
import { FormGroup, Form, Input, Button } from 'reactstrap';
import { withTranslation } from 'react-i18next';
import sessionsService from '../sessions/SessionsService';
import { SessionRequestCreateModal } from '../sessions/SessionRequestCreateModal';

class BookItemsListPlain extends Component {
	constructor(props) {
		super(props);
		this.state = {
			bookItems: [],
			page: 1,
			pageSize: 10,
			maxPages: 5,
			sortOrder: "Name",
			searchString: "",
			loading: true
		};
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

	renderBookItemsTable() {
		const { bookItems, totalBookItems, sortOrder } = this.state;
		const { t, i18n } = this.props;

		return (
			<div>
				<table className='table table-striped' aria-labelledby="tableLabel">
					<thead>
						<tr>
							<th>
								<a href="#" onClick={(e) => this.handleHeaderClick(e, 'Name')} >
									{t('BookName')}
									{sortOrder == 'Name' && <span>&#8897;</span>}
									{sortOrder == 'Name_desc' && <span>&#8896;</span>}
								</a>
							</th>
							<th>
								<a href="#" onClick={(e) => this.handleHeaderClick(e, 'Author')} >
									{t('Authors')}
									{sortOrder == 'Author' && <span>&#8897;</span>}
									{sortOrder == 'Author_desc' && <span>&#8896;</span>}
								</a>
							</th>
							<th>
								<a href="#" onClick={(e) => this.handleHeaderClick(e, 'Genre')} >
									{t('Genre')}
									{sortOrder == 'Genre' && <span>&#8897;</span>}
									{sortOrder == 'Genre_desc' && <span>&#8896;</span>}
								</a>
							</th>
							<th>{t('PublishingYear')}</th>
							<th>{t('ISBN')}</th>
							<th/>
						</tr>
					</thead>
					<tbody>
						{bookItems.map(book =>
							<tr key={book.id}>
								<td>{book.name}</td>
								<td>{book.authors}</td>
								<td>{book.genre}</td>
								<td>{book.year}</td>
								<td>{book.isbn}</td>
								<td>{book.isAvailable ? <Link to={'/sessions/request/add/' + book.id}>{t('Book')}</Link> : ""}</td>
							</tr>
						)}
					</tbody>
				</table>
				<Pager totalItems={totalBookItems} page={this.state.page} pageSize={this.state.pageSize} maxPages={this.state.maxPages} handlePageChange={this.handlePageChange} />
			</div>
		);
	}

	render() {
		const { t, i18n } = this.props;
		let contents = this.state.loading
			? <p><em>{t('Loading')}</em></p>
			: this.renderBookItemsTable();

		return (
			<div>
				<h1 id="tableLabel">{t('SearchBook')}</h1>
				<Form inline onSubmit={this.handleSearchFormSubmit}>
					<FormGroup>
						<Input
							type="text"
							name="searchString"
							style={{ width: '400px', marginRight: '30px' }}
							value={this.state.searchString}
							onChange={this.handleInputChange}
							placeholder={t('SearchPlaceholder')}
						/>
					</FormGroup>&nbsp;
					<Button style={{ width: '100px', marginRight: '15px' }}>{t('Search')}</Button>&nbsp;
					<Button style={{ width: '100px' }} onClick={this.handleSearchFormReset}>{t('Reset')}</Button>
				</Form><br />

				{contents}
			</div>
		);
	}

	async populateBooksData() {
		try {

			const data = await booksService.getBookItems(this.state.page, this.state.pageSize, this.state.sortOrder, this.state.searchString);
			this.setState({ totalBookItems: data.totalBookItems, bookItems: data.bookItems, loading: false });
		}
		catch (error) {
			// Handle error
		}
	}
}

export const BookItemsList = withTranslation()(BookItemsListPlain);