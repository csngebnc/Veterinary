import { MedicationDto, MedicationService } from './../../../../services/generated-api-code'
import { Component, OnInit } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap'

@Component({
  selector: 'app-add-medication',
  templateUrl: './add-medication.component.html',
  styleUrls: ['./add-medication.component.scss'],
})
export class AddMedicationComponent implements OnInit {
  addMedicationForm: FormGroup
  validationErrors

  constructor(
    private medicationService: MedicationService,
    private fb: FormBuilder,
    private ngbModal: NgbActiveModal
  ) {
    this.addMedicationForm = this.fb.group({
      name: ['', Validators.required],
      unit: [0, [Validators.required, Validators.pattern('\\d*')]],
      unitName: ['', Validators.required],
      pricePerUnit: [0, [Validators.required, Validators.pattern('\\d*')]],
    })
  }

  ngOnInit(): void {}

  add(): void {
    this.medicationService.createMedication(this.addMedicationForm.value).subscribe(
      (vaccine: MedicationDto) => {
        this.ngbModal.close(vaccine)
      },
      (err) => {
        this.validationErrors = err
      }
    )
  }

  close(): void {
    this.ngbModal.dismiss()
  }
}
