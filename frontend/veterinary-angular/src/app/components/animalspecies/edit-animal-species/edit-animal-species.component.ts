import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import {
  AnimalSpeciesDto,
  SpeciesService,
} from 'src/app/services/generated-api-code';

@Component({
  selector: 'edit-add-animal-species',
  templateUrl: './edit-animal-species.component.html',
  styleUrls: ['./edit-animal-species.component.scss'],
})
export class EditAnimalSpeciesComponent implements OnInit {
  editSpeciesForm: FormGroup;
  validationErrors;

  @Input() data: any;
  species: AnimalSpeciesDto;

  constructor(
    private speciesService: SpeciesService,
    private fb: FormBuilder,
    private ngbModal: NgbActiveModal
  ) {}

  ngOnInit(): void {
    this.species = this.data.species;
    this.editSpeciesForm = this.fb.group({
      id: [this.species.id, Validators.required],
      name: [this.species.name, Validators.required],
    });
  }

  update(): void {
    this.speciesService
      .updateAnimalSpecies(this.editSpeciesForm.value)
      .subscribe(
        () => {
          this.ngbModal.close(this.editSpeciesForm.value);
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
