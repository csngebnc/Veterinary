import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { base64ToFile, ImageCroppedEvent } from 'ngx-image-cropper';
import {
  AnimalDto,
  AnimalService,
  AnimalSpeciesDto,
  SpeciesService,
} from 'src/app/services/generated-api-code';
import { TokenService } from 'src/app/services/token.service';

@Component({
  selector: 'app-add-animal',
  templateUrl: './add-animal.component.html',
  styleUrls: ['./add-animal.component.scss'],
})
export class AddAnimalComponent implements OnInit {
  addAnimalForm: FormGroup;
  maxDate: Date;
  validationErrors;

  imageChangedEvent: any = '';

  species: AnimalSpeciesDto[] = [];

  constructor(
    private modal: NgbActiveModal,
    private fb: FormBuilder,
    private tokenService: TokenService,
    private animalService: AnimalService,
    private speciesService: SpeciesService
  ) {}

  ngOnInit(): void {
    this.maxDate = new Date();
    this.addAnimalForm = this.fb.group({
      name: ['', Validators.required],
      sex: ['hím', Validators.required],
      dateOfBirth: ['', Validators.required],
      speciesid: ['', Validators.required],
      photo: [''],
    });

    this.speciesService
      .getAnimalSpecies()
      .subscribe(
        (species: AnimalSpeciesDto[]) =>
          (this.species = species.filter((s) => !s.isInactive))
      );
  }

  addAnimal(): void {
    this.animalService
      .createAnimal(
        this.tokenService.getUserData().id,
        this.addAnimalForm.get('name').value,
        this.addAnimalForm.get('dateOfBirth').value,
        this.addAnimalForm.get('sex').value,
        this.addAnimalForm.get('speciesid').value,
        this.imageChangedEvent
          ? {
              fileName: 'photo',
              data: this.addAnimalForm.get('photo').value,
            }
          : undefined
      )
      .subscribe(() => this.modal.close());
  }

  fileChangeEvent(event: any): void {
    this.imageChangedEvent = event;
  }

  imageCropped(event: ImageCroppedEvent): void {
    let croppedImage = base64ToFile(event.base64);
    this.addAnimalForm.patchValue({ photo: croppedImage });
    this.addAnimalForm.get('photo').updateValueAndValidity();
    console.log(this.addAnimalForm.get('photo'));
  }

  close(): void {
    this.modal.dismiss();
  }
}
