import { AddTreatmentComponent } from './../add-treatment/add-treatment.component';
import { TreatmentDto, TreatmentService } from '../../../../services/generated-api-code';
import { Component, OnInit } from '@angular/core';
import { ModalService } from 'src/app/services/modal.service';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { EditTreatmentComponent } from '../edit-treatment/edit-treatment.component';

@Component({
  selector: 'app-list-treatments',
  templateUrl: './list-treatments.component.html',
  styleUrls: ['./list-treatments.component.scss'],
})
export class ListTreatmentsComponent implements OnInit {
  treatments: TreatmentDto[] = [];
  doctorId: string;

  dataSource: MatTableDataSource<TreatmentDto>;
  displayedColumns: string[] = ['name', 'duration', 'status', 'button'];

  constructor(
    private route: ActivatedRoute,
    private treatmentService: TreatmentService,
    private modalService: ModalService
  ) {}

  ngOnInit(): void {
    this.doctorId = this.route.snapshot.paramMap.get('doctorid');
    this.treatmentService
      .getAllTreatmentsByDoctorId(this.doctorId)
      .subscribe((treatments: TreatmentDto[]) => {
        this.treatments = treatments;
        this.dataSource = new MatTableDataSource<TreatmentDto>(this.treatments);
      });
  }

  add() {
    this.modalService.openModal(AddTreatmentComponent, (treatment: TreatmentDto) => {
      this.treatments.push(treatment);
      this.dataSource.data = this.treatments;
    });
  }

  edit(treatmentId: string) {
    this.modalService.openModal(
      EditTreatmentComponent,
      (treatments: TreatmentDto) => {
        this.treatments[this.treatments.findIndex((s) => s.id === treatmentId)] = treatments;
        this.dataSource.data = this.treatments;
      },
      { treatment: this.treatments.find((s) => s.id === treatmentId) }
    );
  }

  changeState(treatmentId: string) {
    this.treatmentService.updateTreatmentStatus(treatmentId).subscribe(() => {
      let index = this.treatments.findIndex((s) => s.id == treatmentId);
      this.treatments[index].isInactive = !this.treatments[index].isInactive;
    });
  }

  deleteTreatment(treatmentId: string) {
    this.treatmentService.deleteTreatment(treatmentId).subscribe(() => {
      this.treatments = this.treatments.filter((s) => s.id !== treatmentId);
      this.dataSource.data = this.treatments;
    });
  }
}
