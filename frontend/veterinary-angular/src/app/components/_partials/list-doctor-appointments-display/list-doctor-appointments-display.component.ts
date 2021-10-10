import { AppointmentStatusUpdateModalComponent } from './../appointment-status-update-modal/appointment-status-update-modal.component';
import { ModalService } from 'src/app/services/modal.service';
import {
  AppointmentForDoctorDto,
  AppointmentService,
  LabelValuePairOfAppointmentStatusEnum,
  PagedListOfAppointmentForDoctorDto,
} from './../../../services/generated-api-code';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-list-doctor-appointments-display',
  templateUrl: './list-doctor-appointments-display.component.html',
  styleUrls: ['./list-doctor-appointments-display.component.scss'],
})
export class ListDoctorAppointmentsDisplayComponent implements OnInit {
  @Input() pageSizeOptions: number[] = [5];
  @Input() fromToday: boolean = true;
  @Input() urlPathPrefix: string = './';
  @Input() doctorId: string;
  @Input() isHomePage: boolean = true;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  pageEvent: PageEvent;
  length: number = 0;

  statuses: LabelValuePairOfAppointmentStatusEnum[] = [];
  appointments: AppointmentForDoctorDto[] = [];
  dataSource: MatTableDataSource<AppointmentForDoctorDto> =
    new MatTableDataSource<AppointmentForDoctorDto>([]);
  displayedColumns: string[] = [
    'ownerName',
    'animalName',
    'animalSpecies',
    'treatmentName',
    'time',
    'reasons',
    'status',
    'button',
  ];

  constructor(private appointmentService: AppointmentService, private modalService: ModalService) {}

  ngOnInit(): void {
    this.appointmentService
      .getStatuses()
      .subscribe((statuses: LabelValuePairOfAppointmentStatusEnum[]) => {
        this.statuses = statuses;
        this.pageChanged({ pageIndex: 0, pageSize: this.pageSizeOptions[0], length: 0 });
      });
  }

  setState(appointmentId: string): void {
    let index = this.appointments.findIndex(({ id }) => id == appointmentId);
    this.modalService.openModal(
      AppointmentStatusUpdateModalComponent,
      (appointment: AppointmentForDoctorDto) => {
        this.appointments[index].status = appointment.status;
        this.dataSource.data = this.appointments;
      },
      {
        appointment: this.appointments[index],
      }
    );
  }

  pageChanged(event: PageEvent): PageEvent {
    this.appointmentService
      .getAppointmentsForDoctor(this.doctorId, this.fromToday, event.pageSize, event.pageIndex)
      .subscribe((appointmentsData: PagedListOfAppointmentForDoctorDto) => {
        this.appointments = [];
        appointmentsData.items.forEach((appointment) => {
          appointment.startDate = new Date(appointment.startDate);
          appointment.endDate = new Date(appointment.endDate);
          this.appointments.push(appointment);
        });
        this.dataSource.data = this.appointments;
        this.length = appointmentsData.totalCount;
      });

    return event;
  }
}
