import React, { Component } from 'react';
import booksService from './BooksService';
import { AvForm, AvField } from 'availity-reactstrap-validation';
import { FormGroup, Form, Label, Input, Button } from 'reactstrap';
import { withTranslation } from 'react-i18next';

class BookAddPlain extends Component {

	constructor(props) {
		super(props);
	}

	componentDidMount() {
	}

	handleClickCancel = () => {
		const { history } = this.props;

		history.push('/books');
	}

	handleValidSubmit = (event, values) => {
		const { history } = this.props;

		(async () => {
			await booksService.addBook(values);
			history.push('/books');
		})();
	}

	render() {
		const { t, i18n } = this.props;
		return (
			<AvForm onValidSubmit={this.handleValidSubmit}>
				<AvField name="name" label={t('BookName')} required errorMessage={t('FieldInvalid')} validate={{
					required: { value: true, errorMessage: t('FieldRequired') },
					minLength: { value: 2 },
					maxLength: { value: 200 }
				}} />
				<AvField name="ISBN" label={t('ISBN')} placeholder="9780733426094" validate={{
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
				<AvField name="quantity" type="number" value="0" label={t('Quantity')} required errorMessage={t('FieldInvalid')} validate={{
					min: { value: 0 },
					max: { value: 100 }
				}} />
				<FormGroup>
					<Button>{t('Save')}</Button>&nbsp;
					<Button onClick={this.handleClickCancel}>{t('Cancel')}</Button>
				</FormGroup>
			</AvForm>
		);
	}
}

export const BookAdd = withTranslation()(BookAddPlain);