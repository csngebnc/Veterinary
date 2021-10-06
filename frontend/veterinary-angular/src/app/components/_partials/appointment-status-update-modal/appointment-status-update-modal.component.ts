import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AppointmentService, AppointmentStatusEnum } from 'src/app/services/generated-api-code';
import {
  AppointmentForDoctorDto,
  LabelValuePairOfAppointmentStatusEnum,
} from './../../../services/generated-api-code';
import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-appointment-status-update-modal',
  templateUrl: './appointment-status-update-modal.component.html',
  styleUrls: ['./appointment-status-update-modal.component.scss'],
})
export class AppointmentStatusUpdateModalComponent implements OnInit {
  @Input() data: any;
  appointment: AppointmentForDoctorDto;

  statuses: LabelValuePairOfAppointmentStatusEnum[] = [];

  updateStatusForm: FormGroup;

  constructor(
    private appointmentService: AppointmentService,
    private fb: FormBuilder,
    private ngbModal: NgbActiveModal
  ) {}

  ngOnInit(): void {
    this.appointment = this.data.appointment;

    this.updateStatusForm = this.fb.group({
      appointmentId: [this.appointment.id, Validators.required],
      statusId: [this.appointment.status, Validators.required],
    });

    this.appointmentService
      .getStatuses()
      .subscribe((statuses: LabelValuePairOfAppointmentStatusEnum[]) => {
        this.statuses = statuses;
      });
  }

  getCurrentStatus(): string {
    return this.statuses.find(({ value }) => value == this.appointment.status)?.label;
  }

  updateStatus(): void {
    this.appointmentService.updateAppointmentStatus(this.updateStatusForm.value).subscribe(() => {
      this.appointment.status = this.updateStatusForm.get('statusId').value;
      this.ngbModal.close(this.appointment);
    });
  }

  close(): void {
    this.ngbModal.dismiss();
  }
}
