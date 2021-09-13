import { MedicationDto, MedicationService } from './../../../../services/generated-api-code'
import { Component, Input, OnInit } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap'

@Component({
  selector: 'app-edit-medication',
  templateUrl: './edit-medication.component.html',
  styleUrls: ['./edit-medication.component.scss'],
})
export class EditMedicationComponent implements OnInit {
  editMedicationForm: FormGroup
  validationErrors

  @Input() data: any
  medication: MedicationDto

  constructor(
    private medicationService: MedicationService,
    private fb: FormBuilder,
    private ngbModal: NgbActiveModal
  ) {}

  ngOnInit(): void {
    this.medication = this.data.medication
    this.editMedicationForm = this.fb.group({
      id: [this.medication.id, Validators.required],
      name: [this.medication.name, Validators.required],
      unit: [this.medication.unit, [Validators.required, Validators.pattern('\\d*')]],
      unitName: [this.medication.unitName, Validators.required],
      pricePerUnit: [
        this.medication.pricePerUnit,
        [Validators.required, Validators.pattern('\\d*')],
      ],
    })
  }

  update(): void {
    this.medicationService.updateMedication(this.editMedicationForm.value).subscribe(
      () => {
        this.ngbModal.close(this.editMedicationForm.value)
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
