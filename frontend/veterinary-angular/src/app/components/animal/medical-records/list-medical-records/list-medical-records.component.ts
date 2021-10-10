import { PagedListOfMedicalRecordDto } from './../../../../services/generated-api-code';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { ModalService } from 'src/app/services/modal.service';
import { ListVaccineRecordsComponent } from './../../vaccine-records/list-vaccine-records/list-vaccine-records.component';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {
  AnimalDto,
  AnimalService,
  MedicalRecordDto,
  MedicalRecordService,
} from 'src/app/services/generated-api-code';
import { TokenService } from 'src/app/services/token.service';

@Component({
  selector: 'app-list-medical-records',
  templateUrl: './list-medical-records.component.html',
  styleUrls: ['./list-medical-records.component.scss'],
})
export class ListMedicalRecordsComponent implements OnInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;

  pageEvent: PageEvent;
  length: number = 0;

  animal: AnimalDto;
  dataSource: MatTableDataSource<MedicalRecordDto> = new MatTableDataSource<MedicalRecordDto>([]);

  constructor(
    private animalService: AnimalService,
    private route: ActivatedRoute,
    private modalService: ModalService,
    private recordService: MedicalRecordService,
    private tokenService: TokenService
  ) {}

  ngOnInit(): void {
    this.dataSource.paginator = this.paginator;
    const animalId = this.route.snapshot.paramMap.get('animalid');
    this.animalService.getAnimal(animalId).subscribe((animal: AnimalDto) => {
      this.animal = animal;
      this.pageChanged({ pageIndex: 0, pageSize: 10, length: 0 });
    });
  }

  openVaccineRecordsDialog(): void {
    this.modalService.openModal(ListVaccineRecordsComponent, () => {}, {
      animalId: this.animal.id,
    });
  }

  pageChanged(event: PageEvent): PageEvent {
    this.recordService
      .getMedicalRecordsForAnimal(this.animal.id, event.pageSize, event.pageIndex)
      .subscribe((response: PagedListOfMedicalRecordDto) => {
        this.dataSource.data = response.items;
        this.length = response.totalCount;
      });
    return event;
  }
}
