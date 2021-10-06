import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { EditTreatmentIntervalComponent } from './../edit-treatment-interval/edit-treatment-interval.component';
import { AddTreatmentIntervalComponent } from './../add-treatment-interval/add-treatment-interval.component';
import { ModalService } from 'src/app/services/modal.service';
import {
  AppointmentService,
  PagedListOfTreatmentIntervalDetailsDto,
  TreatmentIntervalDetailsDto,
  TreatmentIntervalService,
} from './../../../../services/generated-api-code';
import { ActivatedRoute } from '@angular/router';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-list-treatment-intervals',
  templateUrl: './list-treatment-intervals.component.html',
  styleUrls: ['./list-treatment-intervals.component.scss'],
})
export class ListTreatmentIntervalsComponent implements OnInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  pageEvent: PageEvent;
  length: number = 0;

  doctorId: string;
  treatmentId: string;
  treatmentName: string;

  treatmentIntervals: TreatmentIntervalDetailsDto[] = [];
  dataSource: MatTableDataSource<TreatmentIntervalDetailsDto> =
    new MatTableDataSource<TreatmentIntervalDetailsDto>([]);
  displayedColumns: string[] = ['day', 'interval', 'status', 'button'];

  constructor(
    private route: ActivatedRoute,
    private intervalService: TreatmentIntervalService,
    private modalService: ModalService,
    private appointmentService: AppointmentService
  ) {}

  ngOnInit(): void {
    this.doctorId = this.route.snapshot.paramMap.get('doctorid');
    this.treatmentId = this.route.snapshot.paramMap.get('treatmentid');
    this.treatmentName = this.route.snapshot.paramMap.get('name');

    this.dataSource.paginator = this.paginator;
    this.appointmentService
      .getDoctorTreatmentAvailableTimes(new Date(), this.doctorId, this.treatmentId)
      .subscribe((res) => {
        console.log(res);
      });
    this.pageChanged({ pageIndex: 0, pageSize: 10, length: 0 });
  }

  add(): void {
    this.modalService.openModal(
      AddTreatmentIntervalComponent,
      (interval: TreatmentIntervalDetailsDto) => {
        this.pageChanged({ pageIndex: 0, pageSize: this.paginator.pageSize, length: 0 });
      },
      {
        doctorId: this.doctorId,
        treatmentId: this.treatmentId,
        treatmentName: this.treatmentName,
      },
      'lg'
    );
  }

  edit(intervalId: string): void {
    let interval = this.treatmentIntervals.find(({ id }) => id === intervalId);
    this.modalService.openModal(
      EditTreatmentIntervalComponent,
      (editedInterval: TreatmentIntervalDetailsDto) => {
        this.treatmentIntervals[this.treatmentIntervals.indexOf(interval, 0)] = editedInterval;
        this.dataSource.data = this.treatmentIntervals;
      },
      {
        doctorId: this.doctorId,
        interval: interval,
        treatmentName: this.treatmentName,
      },
      'lg'
    );
  }

  changeStatus(intervalId: string): void {
    let interval = this.treatmentIntervals.find(({ id }) => id === intervalId);
    this.treatmentIntervals[this.treatmentIntervals.indexOf(interval, 0)].isInactive =
      !interval.isInactive;
  }

  deleteInterval(intervalId: string): void {
    this.intervalService.deleteTreatmentInterval(intervalId).subscribe(() => {
      this.treatmentIntervals = this.treatmentIntervals.filter(({ id }) => id !== intervalId);
      this.dataSource.data = this.treatmentIntervals;
      this.length = this.length - 1;
    });
  }

  pageChanged(pageEvent: PageEvent) {
    this.intervalService
      .getTreatmentIntervalsWithDetails(this.treatmentId, pageEvent.pageSize, pageEvent.pageIndex)
      .subscribe((treatmentIntervals: PagedListOfTreatmentIntervalDetailsDto) => {
        this.treatmentIntervals = treatmentIntervals.items;
        this.dataSource.data = treatmentIntervals.items;
        this.length = treatmentIntervals.totalCount;
      });
  }
}
