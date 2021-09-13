import { TherapiaService, TherapiaDto } from './../../../../services/generated-api-code'
import { Component, OnInit } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap'

@Component({
  selector: 'app-add-therapia',
  templateUrl: './add-therapia.component.html',
  styleUrls: ['./add-therapia.component.scss'],
})
export class AddTherapiaComponent implements OnInit {
  addTherapiaForm: FormGroup
  validationErrors

  constructor(
    private therapiaService: TherapiaService,
    private fb: FormBuilder,
    private ngbModal: NgbActiveModal
  ) {
    this.addTherapiaForm = this.fb.group({
      name: ['', Validators.required],
      price: [0, [Validators.required, Validators.pattern('\\d*')]],
    })
  }

  ngOnInit(): void {}

  add(): void {
    this.therapiaService.createTherapia(this.addTherapiaForm.value).subscribe(
      (vaccine: TherapiaDto) => {
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
