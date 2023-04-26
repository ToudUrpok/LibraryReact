import React, { Component } from 'react'
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import { withTranslation } from 'react-i18next';

class SessionRequestCreateModalPlain extends Component {
    constructor(props) {
        super(props);
        this.handleConfirmClick = this.handleConfirmClick.bind(this);
	}

	componentDidMount() {
    }

    handleConfirmClick = (event) => {
        event.preventDefault();
        //booksService.book();
        this.props.toggle();
    }

	render() {
		const { t, i18n } = this.props;
		return (
            <div>
                <Modal isOpen={this.props.isOpen} toggle={this.props.toggle}>
                    <ModalHeader toggle={this.props.toggle}>{t('SessionModalHeader')}</ModalHeader>
                    <ModalBody>
                        Requested book: {this.props.item}
                        <br/>
                        {t('ModalBodyText')}
                    </ModalBody>
                    <ModalFooter>
                        <Button color="primary" onClick={this.handleConfirmClick}>
                            {t('Confirm')}
                        </Button>{' '}
                        <Button color="secondary" onClick={this.props.toggle}>
                            {t('Cancel')}
                        </Button>
                    </ModalFooter>
                </Modal>
            </div>
        );
	}
}

export const SessionRequestCreateModal = withTranslation()(SessionRequestCreateModalPlain);