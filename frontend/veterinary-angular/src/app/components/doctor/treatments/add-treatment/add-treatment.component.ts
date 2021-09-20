import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TreatmentDto, TreatmentService } from 'src/app/services/generated-api-code';

@Component({
  selector: 'app-add-treatment',
  templateUrl: './add-treatment.component.html',
  styleUrls: ['./add-treatment.component.scss'],
})
export class AddTreatmentComponent implements OnInit {
  addTreatmentForm: FormGroup;
  validationErrors;

  constructor(
    private treatmentService: TreatmentService,
    private fb: FormBuilder,
    private ngbModal: NgbActiveModal
  ) {
    this.addTreatmentForm = this.fb.group({
      name: ['', Validators.required],
      duration: [0, [Validators.required, Validators.pattern('\\d*')]],
    });
  }

  ngOnInit(): void {}

  add(): void {
    this.treatmentService.createTreatment(this.addTreatmentForm.value).subscribe(
      (vaccine: TreatmentDto) => {
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
