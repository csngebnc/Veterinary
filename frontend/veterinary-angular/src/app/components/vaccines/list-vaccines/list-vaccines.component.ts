import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import {
  VaccineDto,
  VaccinesService,
} from 'src/app/services/generated-api-code';
import { ModalService } from 'src/app/services/modal.service';
import { AddVaccineComponent } from '../add-vaccine/add-vaccine.component';
import { EditVaccineComponent } from '../edit-vaccine/edit-vaccine.component';

@Component({
  selector: 'app-list-vaccines',
  templateUrl: './list-vaccines.component.html',
  styleUrls: ['./list-vaccines.component.scss'],
})
export class ListVaccinesComponent implements OnInit {
  vaccines: VaccineDto[] = [];

  dataSource: MatTableDataSource<VaccineDto>;
  displayedColumns: string[] = ['name', 'status', 'button'];

  constructor(
    private vaccineService: VaccinesService,
    private modalService: ModalService
  ) {}

  ngOnInit(): void {
    this.vaccineService.getVaccines().subscribe((vaccines: VaccineDto[]) => {
      this.vaccines = vaccines;
      this.dataSource = new MatTableDataSource<VaccineDto>(this.vaccines);
    });
  }

  add() {
    this.modalService.openModal(AddVaccineComponent, (vaccine: VaccineDto) => {
      this.vaccines.push(vaccine);
      this.dataSource.data = this.vaccines;
    });
  }

  edit(vaccineId: string) {
    this.modalService.openModal(
      EditVaccineComponent,
      (vaccine: VaccineDto) => {
        this.vaccines[this.vaccines.findIndex((v) => v.id === vaccineId)] =
          vaccine;
        this.dataSource.data = this.vaccines;
      },
      { vaccine: this.vaccines.find((s) => s.id === vaccineId) }
    );
  }

  changeState(vaccineId: string) {
    this.vaccineService.updateVaccineStatus(vaccineId).subscribe(() => {
      let index = this.vaccines.findIndex((s) => s.id == vaccineId);
      this.vaccines[index].isInactive = !this.vaccines[index].isInactive;
    });
  }

  delete(vaccineId: string) {
    this.vaccineService.deleteVaccine(vaccineId).subscribe(() => {
      this.vaccines = this.vaccines.filter((s) => s.id !== vaccineId);
      this.dataSource.data = this.vaccines;
    });
  }
}
