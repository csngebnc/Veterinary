import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-book-appointment-user-selector',
  templateUrl: './book-appointment-user-selector.component.html',
  styleUrls: ['./book-appointment-user-selector.component.scss'],
})
export class BookAppointmentUserSelectorComponent implements OnInit {
  selectedUserId: string;

  constructor(private ngbModal: NgbActiveModal) {}

  ngOnInit(): void {}

  close(): void {
    this.ngbModal.close();
  }

  saveSelectedId(userId: string): void {
    this.selectedUserId = userId;
  }
}
