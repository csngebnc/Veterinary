import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import {
  VaccineDto,
  VaccinesService,
} from 'src/app/services/generated-api-code';

@Component({
  selector: 'app-edit-vaccine',
  templateUrl: './edit-vaccine.component.html',
  styleUrls: ['./edit-vaccine.component.scss'],
})
export class EditVaccineComponent implements OnInit {
  editVaccineForm: FormGroup;
  validationErrors;

  @Input() data: any;
  vaccine: VaccineDto;

  constructor(
    private vaccineService: VaccinesService,
    private fb: FormBuilder,
    private ngbModal: NgbActiveModal
  ) {}

  ngOnInit(): void {
    this.vaccine = this.data.vaccine;
    this.editVaccineForm = this.fb.group({
      id: [this.vaccine.id, Validators.required],
      name: [this.vaccine.name, Validators.required],
    });
  }

  update(): void {
    this.vaccineService.updateVaccine(this.editVaccineForm.value).subscribe(
      () => {
        this.ngbModal.close(this.editVaccineForm.value);
      },
      (err) => {
        this.validationErrors = err;
      }
    );
  }

  close(): void {
    this.ngbModal.dismiss();
  }
}
