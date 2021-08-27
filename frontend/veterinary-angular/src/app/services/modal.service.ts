import { OnInit } from '@angular/core';
import { Injectable } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Injectable({
  providedIn: 'root',
})
export class ModalService {
  constructor(private ngbModal: NgbModal) {}

  public openModal(
    component: any,
    callback: Function,
    data?: any,
    errCallback?: Function
  ): void {
    const modal = this.ngbModal.open(component);
    if (data) {
      modal.componentInstance.data = data;
    }
    modal.result.then(
      (cb) => callback(cb),
      (err) => {
        if (errCallback) {
          errCallback(err);
        }
      }
    );
  }
}
