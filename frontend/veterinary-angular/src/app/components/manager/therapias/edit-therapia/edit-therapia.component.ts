import { TherapiaService, TherapiaDto } from './../../../../services/generated-api-code'
import { Component, Input, OnInit } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap'

@Component({
  selector: 'app-edit-therapia',
  templateUrl: './edit-therapia.component.html',
  styleUrls: ['./edit-therapia.component.scss'],
})
export class EditTherapiaComponent implements OnInit {
  editTherapiaForm: FormGroup
  validationErrors

  @Input() data: any
  therapia: TherapiaDto

  constructor(
    private therapiaService: TherapiaService,
    private fb: FormBuilder,
    private ngbModal: NgbActiveModal
  ) {}

  ngOnInit(): void {
    this.therapia = this.data.therapia
    this.editTherapiaForm = this.fb.group({
      id: [this.therapia.id, Validators.required],
      name: [this.therapia.name, Validators.required],
      price: [this.therapia.price, [Validators.required, Validators.pattern('\\d*')]],
    })
  }

  update(): void {
    this.therapiaService.updateTherapia(this.editTherapiaForm.value).subscribe(
      () => {
        this.ngbModal.close(this.editTherapiaForm.value)
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
