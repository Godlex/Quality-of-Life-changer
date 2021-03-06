import React from "react";
import { Button, Modal } from "react-bootstrap";

function CustomModal(props) {
  return (
    <Modal show={props.show}>
      <Modal.Header>
        <Modal.Title>{props.title}</Modal.Title>
      </Modal.Header>
      <Modal.Body>{props.children}</Modal.Body>
      <Modal.Footer>
        <Button variant={props.submitVariant} onClick={props.handleSubmit}>
          {props.submitButtonText}
        </Button>
        <Button variant={props.cancelVariant} onClick={props.handleCancel}>
          {props.cancelButtonText}
        </Button>
      </Modal.Footer>
    </Modal>
  );
}

export default CustomModal;
