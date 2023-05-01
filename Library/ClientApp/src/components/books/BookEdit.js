import React, { Component } from 'react';
import { withRouter } from 'react-router';
import booksService from './BooksService';
import { AvForm, AvField } from 'availity-reactstrap-validation';
import { FormGroup, Form, Label, Input, Button } from 'reactstrap';
import { withTranslation } from 'react-i18next';


class BookEditPlain extends Component {
	constructor(props) {
		super(props);
		this.state = { book: null, loading: true };

		const { match } = this.props;
		this.bookId = match.params.bookId;
	}

	componentDidMount() {
		this.retrieveFormData();
	}

	handleInputChange = (event) => {
		const target = event.target;
		const value = target.type === 'checkbox' ? target.checked : target.value;
		const name = target.name;

		this.state.book[name] = value;
		this.setState({ book: this.state.book });
	}

	handleClickCancel = () => {
		const { history } = this.props;

		history.push('/books');
	}

	handleValidSubmit = (event, values) => {
		const { history } = this.props;

		(async () => {
			await booksService.updateBook(this.bookId, values);
			history.push('/books');
		})();
	}

	renderBookForm(book) {
		const { t, i18n } = this.props;
		return (
			<AvForm model={book} onValidSubmit={this.handleValidSubmit}>
				<AvField name="id" type="hidden" />
				<AvField name="name" label={t('BookName')} required errorMessage={t('FieldInvalid')} validate={{
					required: { value: true, errorMessage: t('FieldRequired') },
					minLength: { value: 2 },
					maxLength: { value: 200 }
				}} />
				<AvField name="isbn" label={t('ISBN')} validate={{
					required: { value: true, errorMessage: t('FieldRequired') },
					pattern: { value: '[0-9]{13}' },
					maxLength: { value: 13 }
				}} />
				<AvField name="authors" label={t('Authors')} required errorMessage={t('FieldInvalid')} validate={{
					required: { value: true, errorMessage: t('FieldRequired') },
					minLength: { value: 2 },
					maxLength: { value: 200 }
				}} />
				<AvField name="genre" label={t('Genre')} required errorMessage={t('FieldInvalid')} validate={{
					required: { value: true, errorMessage: t('FieldRequired') },
					minLength: { value: 2 },
					maxLength: { value: 200 }
				}} />
				<AvField name="year" type="number" label={t('PublishingYear')} errorMessage={t('FieldInvalid')} validate={{
					min: { value: 1000 },
					max: { value: 2200 }
				}} />
				<AvField name="quantity" type="hidden" />
				<FormGroup>
					<Button>{t('Save')}</Button>&nbsp;
					<Button onClick={this.handleClickCancel}>{t('Cancel')}</Button>
				</FormGroup>
			</AvForm>
		);
	}

	render() {
		let contents = this.state.loading
			? <p><em>Loading...</em></p>
			: this.renderBookForm(this.state.book);

		return (
			<div>
				<h1 id="tabelLabel">Book</h1>
				{contents}
			</div>
		);
	}

	async retrieveFormData() {
		const data = await booksService.getBook(this.bookId);
		this.setState({ book: data, loading: false });
	}
}

export const BookEdit = withTranslation()(withRouter(BookEditPlain));