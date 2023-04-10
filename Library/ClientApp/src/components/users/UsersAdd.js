import React, { Component } from 'react';
import usersService from './UsersService';
import { AvForm, AvField } from 'availity-reactstrap-validation';
import { FormGroup, Form, Label, Input, Button } from 'reactstrap';
import { withTranslation } from 'react-i18next';

class UsersAddPlain extends Component {

	constructor(props) {
		super(props);
	}

	componentDidMount() {
	}

	handleClickCancel = () => {
		const { history } = this.props;

		history.push('/users');
	}

	handleValidSubmit = (event, values) => {
		const { history } = this.props;

		(async () => {
			await usersService.addUser(values);
			history.push('/users');
		})();
	}

	render() {
		const { t, i18n } = this.props;
		return (
			<AvForm onValidSubmit={this.handleValidSubmit}>
				<AvField name="firstName" label={t('FirstName')} required errorMessage={t('FieldInvalid')} validate={{
					required: { value: true, errorMessage: t('FieldRequired') },
					minLength: { value: 2 }
				}} />
				<AvField name="midName" label={t('MidName')} required errorMessage={t('FieldInvalid')} validate={{
					required: { value: true, errorMessage: t('FieldRequired') },
					minLength: { value: 2 }
				}} />
				<AvField name="lastName" label={t('LastName')} required errorMessage={t('FieldInvalid')} validate={{
					required: { value: true, errorMessage: t('FieldRequired') },
					minLength: { value: 2 }
				}} />
				<AvField name="identificationNumber" label={t('IdentificationNumber')} placeholder="5140397B013PB7" required validate={{
					required: { value: true, errorMessage: t('FieldRequired') },
					pattern: { value: '[0-9SA-Z]{14}' },
					maxLength: { value: 14 }
				}} />
				<AvField name="dateOfBirth" type="date" label={t('DateOfBirth')} required />
				<AvField name="phoneNumber" label={t('Phone')} placeholder="291112233" required validate={{
					required: { value: true, errorMessage: t('FieldRequired') },
					pattern: { value: '(29|44|33|25)[0-9]{7}' },
					maxLength: { value: 9 }
				}} />
				<AvField name="email" type="email" label={t('Email')} required />
				<AvField name="role" type="select" label={t('Role')} required>
					<option value="">---{t('SelectRole')}---</option>
					<option value="administrator">{t('Administrator')}</option>
					<option value="librarian">{t('Librarian')}</option>
					<option value="user">{t('User')}</option>
				</AvField>
				<AvField name="password" type="password" label={t('Password')} required autocomplete="off" />
				<AvField name="confirmPassword" type="password" label={t('ConfirmPassword')} required
					validate={{ match: { value: 'password' } }}
				/>
				<FormGroup>
					<Button>Save</Button>&nbsp;
					<Button onClick={this.handleClickCancel}>Cancel</Button>
				</FormGroup>
			</AvForm>
		);
	}
}

export const UsersAdd = withTranslation()(UsersAddPlain);

