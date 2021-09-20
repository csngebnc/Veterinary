import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TreatmentDto, TreatmentService } from 'src/app/services/generated-api-code';

@Component({
  selector: 'app-edit-treatment',
  templateUrl: './edit-treatment.component.html',
  styleUrls: ['./edit-treatment.component.scss'],
})
export class EditTreatmentComponent implements OnInit {
  editTreatmentForm: FormGroup;
  validationErrors;

  @Input() data: any;
  treatment: TreatmentDto;

  constructor(
    private treatmentService: TreatmentService,
    private fb: FormBuilder,
    private ngbModal: NgbActiveModal
  ) {}

  ngOnInit(): void {
    this.treatment = this.data.treatment;
    this.editTreatmentForm = this.fb.group({
      treatmentId: [this.treatment.id, Validators.required],
      name: [this.treatment.name, Validators.required],
      duration: [this.treatment.duration, [Validators.required, Validators.pattern('\\d*')]],
    });
  }

  update(): void {
    this.treatmentService.updateTreatment(this.editTreatmentForm.value).subscribe(
      () => {
        this.ngbModal.close(this.editTreatmentForm.value);
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
