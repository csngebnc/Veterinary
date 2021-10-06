import {
  AppointmentStatusEnum,
  LabelValuePairOfAppointmentStatusEnum,
  PagedListOfAppointmentForUserDto,
} from './../../../services/generated-api-code';
import { TokenService } from './../../../services/token.service';
import { ActivatedRoute } from '@angular/router';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { AppointmentForDoctorDto, AppointmentService } from 'src/app/services/generated-api-code';

@Component({
  selector: 'app-list-appointments',
  templateUrl: './list-appointments.component.html',
  styleUrls: ['./list-appointments.component.scss'],
})
export class ListAppointmentsComponent implements OnInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  pageEvent: PageEvent;
  length: number = 0;

  userId: string;

  statuses: LabelValuePairOfAppointmentStatusEnum[] = [];
  appointments: AppointmentForDoctorDto[] = [];
  dataSource: MatTableDataSource<AppointmentForDoctorDto> =
    new MatTableDataSource<AppointmentForDoctorDto>([]);
  displayedColumns: string[] = [
    'doctorName',
    'animalName',
    'animalSpecies',
    'treatmentName',
    'time',
    'reasons',
    'status',
    'button',
  ];

  constructor(
    private appointmentService: AppointmentService,
    private route: ActivatedRoute,
    private tokenService: TokenService
  ) {}

  ngOnInit(): void {
    this.userId = this.route.snapshot.paramMap.get('userId') ?? this.tokenService.getUserData().id;
    this.appointmentService
      .getStatuses()
      .subscribe((statuses: LabelValuePairOfAppointmentStatusEnum[]) => {
        this.statuses = statuses;
        this.pageChanged({ pageIndex: 0, pageSize: 10, length: 0 });
      });
  }

  isResignDisabled(appointmentId: string): boolean {
    return (
      this.appointments.find(({ id }) => id == appointmentId).status > AppointmentStatusEnum.New
    );
  }

  resignAppointment(appointmentId: string): void {
    this.appointmentService
      .updateAppointmentStatus({
        appointmentId: appointmentId,
        statusId: AppointmentStatusEnum.Resigned,
      })
      .subscribe(() => {
        let index = this.appointments.findIndex(({ id }) => id == appointmentId);
        this.appointments[index].status = AppointmentStatusEnum.Resigned;
        this.dataSource.data = this.appointments;
      });
  }

  pageChanged(event: PageEvent): PageEvent {
    this.appointmentService
      .getAppointmentsForUser(this.userId, '', event.pageSize, event.pageIndex)
      .subscribe((appointmentsData: PagedListOfAppointmentForUserDto) => {
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
