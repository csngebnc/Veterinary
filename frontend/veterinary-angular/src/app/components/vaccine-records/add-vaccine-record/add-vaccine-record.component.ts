import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { VaccineRecordDto, VaccinesService } from './../../../services/generated-api-code';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-add-vaccine-record',
  templateUrl: './add-vaccine-record.component.html',
  styleUrls: ['./add-vaccine-record.component.scss'],
})
export class AddVaccineRecordComponent implements OnInit {
  @Input() data: any;

  addVaccineRecordForm: FormGroup;
  maxDate: Date;

  constructor(
    private vaccineService: VaccinesService,
    private modal: NgbActiveModal,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.maxDate = new Date();

    this.addVaccineRecordForm = this.fb.group({
      date: ['', Validators.required],
      animalId: [this.data.animalId, Validators.required],
      vaccineId: ['', Validators.required],
    });
  }

  save(): void {
    this.vaccineService
      .createVaccineRecord(this.addVaccineRecordForm.value)
      .subscribe((record: VaccineRecordDto) => {
        this.modal.close(record);
      });
  }

  cancel(): void {
    this.modal.dismiss();
  }

  vaccineSelected(vaccineId): void {
    this.addVaccineRecordForm.patchValue({ vaccineId: vaccineId });
    console.log(this.addVaccineRecordForm.value);
  }
}
