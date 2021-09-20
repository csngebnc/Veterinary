import { ActivatedRoute } from '@angular/router';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import {
  HolidayDto,
  HolidayService,
  PagedListOfHolidayDto,
} from 'src/app/services/generated-api-code';
import { ModalService } from 'src/app/services/modal.service';
import { AddHolidayComponent } from '../add-holiday/add-holiday.component';
import { EditHolidayComponent } from '../edit-holiday/edit-holiday.component';

@Component({
  selector: 'app-list-holidays',
  templateUrl: './list-holidays.component.html',
  styleUrls: ['./list-holidays.component.scss'],
})
export class ListHolidaysComponent implements OnInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  pageEvent: PageEvent;

  dataSource: MatTableDataSource<HolidayDto> = new MatTableDataSource<HolidayDto>();
  displayedColumns: string[] = ['duration', 'button'];
  length: number = 0;

  holidays: HolidayDto[] = [];
  doctorId: string;

  constructor(
    private holidayService: HolidayService,
    private modalService: ModalService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.loadHolidays({ pageIndex: 0, pageSize: 10, length: 0 });
    this.dataSource.paginator = this.paginator;
  }

  loadHolidays(event: PageEvent) {
    this.doctorId = this.route.snapshot.paramMap.get('doctorid');
    this.holidayService
      .getHolidays(this.doctorId, event.pageSize, event.pageIndex)
      .subscribe((holidays: PagedListOfHolidayDto) => {
        this.holidays = holidays.items;
        this.length = holidays.totalCount;
        this.dataSource.data = this.holidays;
      });
  }

  pageChanged(event: PageEvent): PageEvent {
    this.loadHolidays(event);
    return event;
  }

  add() {
    this.modalService.openModal(
      AddHolidayComponent,
      (holiday: HolidayDto) => {
        this.holidays.push(holiday);
        this.length = this.length + 1;
        this.dataSource.data = this.holidays;
      },
      { doctorId: this.doctorId }
    );
  }

  edit(holidayId: string) {
    this.modalService.openModal(
      EditHolidayComponent,
      (holiday: HolidayDto) => {
        this.holidays[this.holidays.findIndex((v) => v.id === holidayId)] = holiday;
        this.dataSource.data = this.holidays;
      },
      { holiday: this.holidays.find((s) => s.id === holidayId) }
    );
  }

  deleteHoliday(holidayId: string) {
    this.holidayService.deleteHoliday(holidayId).subscribe(() => {
      this.holidays = this.holidays.filter((s) => s.id !== holidayId);
      this.length = this.length - 1;
      this.dataSource.data = this.holidays;
    });
  }
}
