import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AnimalSpeciesDto, SpeciesService } from 'src/app/services/generated-api-code';

@Component({
  selector: 'app-add-animal-species',
  templateUrl: './add-animal-species.component.html',
  styleUrls: ['./add-animal-species.component.scss'],
})
export class AddAnimalSpeciesComponent implements OnInit {
  addSpeciesForm: FormGroup;
  validationErrors;

  constructor(
    private speciesService: SpeciesService,
    private fb: FormBuilder,
    private ngbModal: NgbActiveModal
  ) {}

  ngOnInit(): void {
    this.addSpeciesForm = this.fb.group({
      name: ['', Validators.required],
    });
  }

  add(): void {
    this.speciesService
      .createAnimalSpecies({ name: this.addSpeciesForm.get('name').value })
      .subscribe(
        (species: AnimalSpeciesDto) => {
          this.ngbModal.close(species);
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
