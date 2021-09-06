import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import {
  VaccineDto,
  VaccinesService,
} from 'src/app/services/generated-api-code';

@Component({
  selector: 'app-add-vaccine',
  templateUrl: './add-vaccine.component.html',
  styleUrls: ['./add-vaccine.component.scss'],
})
export class AddVaccineComponent implements OnInit {
  addVaccineForm: FormGroup;
  validationErrors;

  constructor(
    private vaccineService: VaccinesService,
    private fb: FormBuilder,
    private ngbModal: NgbActiveModal
  ) {}

  ngOnInit(): void {
    this.addVaccineForm = this.fb.group({
      name: ['', Validators.required],
    });
  }

  add(): void {
    this.vaccineService
      .createVaccine(this.addVaccineForm.get('name').value)
      .subscribe(
        (vaccine: VaccineDto) => {
          this.ngbModal.close(vaccine);
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
