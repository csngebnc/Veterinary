import { Component, HostListener, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { base64ToFile, ImageCroppedEvent } from 'ngx-image-cropper';
import {
  AnimalDto,
  AnimalService,
  AnimalSpeciesDto,
  SpeciesService,
} from 'src/app/services/generated-api-code';

@Component({
  selector: 'app-edit-animal',
  templateUrl: './edit-animal.component.html',
  styleUrls: ['./edit-animal.component.scss'],
})
export class EditAnimalComponent implements OnInit {
  animalId: string;
  animal: AnimalDto;

  active = 1;
  imageChangedEvent: any = '';
  croppedImage: any = '';
  validationErrors;

  @HostListener('window:beforeunload', ['$event']) unloadNotification(
    $event: any
  ) {
    if (this.editAnimalForm.dirty) {
      $event.returnValue = true;
    }
  }

  editAnimalForm: FormGroup;
  maxDate: Date;
  speciesList: AnimalSpeciesDto[] = [];

  constructor(
    private animalService: AnimalService,
    private speciesService: SpeciesService,
    private route: ActivatedRoute,
    private fb: FormBuilder
  ) {
    this.animalId = this.route.snapshot.paramMap.get('animalid');
  }

  ngOnInit(): void {
    this.maxDate = new Date();
    this.speciesService
      .getAnimalSpecies()
      .subscribe((response: AnimalSpeciesDto[]) => {
        this.speciesList = response;
        this.refreshData();
      });
  }

  refreshData(): void {
    this.animalService.getAnimal(this.animalId).subscribe((animal) => {
      this.animal = animal;
      this.initializeForm();
    });
  }

  initializeForm(): void {
    this.editAnimalForm = this.fb.group({
      id: [this.animal.id],
      name: [this.animal.name, Validators.required],
      sex: [this.animal.sex, Validators.required],
      dateOfBirth: [this.animal.dateOfBirth, Validators.required],
      speciesid: [this.animal.speciesId, Validators.required],
      weight: [this.animal.weight],
      subspecies: [this.animal.subSpeciesName],
    });
  }

  updateAnimal(): void {
    if (this.editAnimalForm.get('subspecies').value === '') {
      this.editAnimalForm.patchValue({ subspecies: null });
    }
    this.animalService
      .updateAnimal(this.animalId, this.editAnimalForm.value)
      .subscribe(() => {});
  }

  updatePhoto(): void {
    this.animalService
      .updateAnimalPhoto(this.animal.ownerId, this.animalId, {
        fileName: 'photo',
        data: this.croppedImage,
      })
      .subscribe((photoUrl: string) => (this.animal.photoUrl = photoUrl));
  }

  deletePhoto(): void {
    this.animalService
      .deleteAnimalPhoto(this.animalId)
      .subscribe((photoUrl: string) => {
        this.animal.photoUrl = photoUrl;
        (this.croppedImage = ''), (this.imageChangedEvent = '');
      });
  }

  fileChangeEvent(event: any) {
    this.imageChangedEvent = event;
  }
  imageCropped(event: ImageCroppedEvent) {
    this.croppedImage = base64ToFile(event.base64);
  }
}
