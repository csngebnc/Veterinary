import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { VaccinesService, VaccineRecordDto } from 'src/app/services/generated-api-code';

@Component({
  selector: 'app-edit-vaccine-record',
  templateUrl: './edit-vaccine-record.component.html',
  styleUrls: ['./edit-vaccine-record.component.scss'],
})
export class EditVaccineRecordComponent implements OnInit {
  @Input() data: any;

  editVaccineRecordForm: FormGroup;
  maxDate: Date;

  record: VaccineRecordDto;

  constructor(
    private vaccineService: VaccinesService,
    private modal: NgbActiveModal,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.maxDate = new Date();
    this.vaccineService
      .getVaccineRecord(this.data.recordId)
      .subscribe((record: VaccineRecordDto) => {
        this.record = record;
        this.editVaccineRecordForm = this.fb.group({
          id: [record.id, Validators.required],
          date: [new Date(record.date), Validators.required],
          animalId: [record.animalId, Validators.required],
          vaccineId: [record.vaccineId, Validators.required],
        });
      });
  }

  save(): void {
    this.vaccineService.updateVaccineRecord(this.editVaccineRecordForm.value).subscribe(() => {
      this.modal.close(this.editVaccineRecordForm.value);
    });
  }

  cancel(): void {
    this.modal.dismiss();
  }

  vaccineSelected(vaccineId): void {
    this.editVaccineRecordForm.patchValue({ vaccineId: vaccineId });
    console.log(this.editVaccineRecordForm.value);
  }
}
