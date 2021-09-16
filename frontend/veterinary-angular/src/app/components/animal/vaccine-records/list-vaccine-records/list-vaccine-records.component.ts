import { EditVaccineRecordComponent } from './../../../vaccine-records/edit-vaccine-record/edit-vaccine-record.component';
import { PagedListOfVaccineRecordDto } from './../../../../services/generated-api-code';
import { ModalService } from 'src/app/services/modal.service';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { VaccineRecordDto, VaccinesService } from 'src/app/services/generated-api-code';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { AddVaccineRecordComponent } from 'src/app/components/vaccine-records/add-vaccine-record/add-vaccine-record.component';

@Component({
  selector: 'app-list-vaccine-records',
  templateUrl: './list-vaccine-records.component.html',
  styleUrls: ['./list-vaccine-records.component.scss'],
})
export class ListVaccineRecordsComponent implements OnInit {
  @Input() data: any;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  pageEvent: PageEvent;
  length: number = 0;

  records: VaccineRecordDto[];

  dataSource: MatTableDataSource<VaccineRecordDto> = new MatTableDataSource();
  displayedColumns: string[] = ['name', 'date', 'button'];

  constructor(
    private vaccineService: VaccinesService,
    private modalService: ModalService,
    private ngbModal: NgbActiveModal
  ) {}

  ngOnInit(): void {
    this.dataSource.paginator = this.paginator;
    this.loadRecords({ pageIndex: 0, pageSize: 5, length: 0 });
  }

  loadRecords(event: PageEvent): void {
    this.vaccineService
      .getVaccineRecords(this.data.animalId, event.pageSize, event.pageIndex)
      .subscribe((records: PagedListOfVaccineRecordDto) => {
        this.records = records.items;
        this.records.forEach((vr) => (vr.date = new Date(vr.date)));
        this.dataSource.data = this.records;
        this.length = records.totalCount;
      });
  }

  open() {
    this.modalService.openModal(
      AddVaccineRecordComponent,
      (record: VaccineRecordDto) => {
        this.records.push(record);
        this.dataSource.data = this.records;
        this.length = this.length + 1;
        this.paginator._changePageSize(this.paginator.pageSize);
      },
      { animalId: this.data.animalId }
    );
  }

  openEdit(recordId: string): void {
    this.modalService.openModal(
      EditVaccineRecordComponent,
      (record: VaccineRecordDto) => {
        let index = this.records.findIndex((r) => r.id == record.id);
        this.records[index].date = record.date;
        this.dataSource.data = this.records;
        this.length = this.length + 1;
      },
      { recordId: recordId }
    );
  }

  deleteRecord(recordId: string): void {
    this.vaccineService.deleteVaccineRecord(recordId).subscribe(() => {
      this.records = this.records.filter((r) => r.id != recordId);
      this.dataSource.data = this.records;
      this.length = this.length - 1;
    });
  }

  close(): void {
    this.ngbModal.dismiss();
  }
}
