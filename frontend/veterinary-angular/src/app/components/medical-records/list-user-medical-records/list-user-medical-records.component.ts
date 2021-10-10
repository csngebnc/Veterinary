import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import {
  MedicalRecordDto,
  MedicalRecordService,
  PagedListOfMedicalRecordDto,
  UserService,
  VeterinaryUserDto,
} from 'src/app/services/generated-api-code';

@Component({
  selector: 'app-list-user-medical-records',
  templateUrl: './list-user-medical-records.component.html',
  styleUrls: ['./list-user-medical-records.component.scss'],
})
export class ListUserMedicalRecordsComponent implements OnInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;

  pageEvent: PageEvent;
  length: number = 0;

  user: VeterinaryUserDto;
  dataSource: MatTableDataSource<MedicalRecordDto> = new MatTableDataSource<MedicalRecordDto>([]);

  constructor(
    private userService: UserService,
    private route: ActivatedRoute,
    private recordService: MedicalRecordService
  ) {}

  ngOnInit(): void {
    this.dataSource.paginator = this.paginator;
    const userId = this.route.snapshot.paramMap.get('userId');
    this.userService.getUser(userId).subscribe((user: VeterinaryUserDto) => {
      this.user = user;
      this.pageChanged({ pageIndex: 0, pageSize: 10, length: 0 });
    });
  }

  pageChanged(event: PageEvent): PageEvent {
    this.recordService
      .getMedicalRecordsForUser(this.user.id, event.pageSize, event.pageIndex)
      .subscribe((response: PagedListOfMedicalRecordDto) => {
        this.dataSource.data = response.items;
        this.length = response.totalCount;
      });
    return event;
  }
}
