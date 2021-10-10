import { ActivatedRoute, Router } from '@angular/router';
import { TherapiaPickerComponent } from './../../_partials/therapia-picker/therapia-picker.component';
import { MedicationPickerComponent } from './../../_partials/medication-picker/medication-picker.component';
import {
  MedicalRecordTextTemplate,
  API_BASE_URL,
  MedicationForSelectDto,
  TherapiaForSelectDto,
  AppointmentService,
  AppointmentStatusEnum,
  AppointmentForRecordDto,
  MedicalRecordEditDto,
} from './../../../services/generated-api-code';
import { TokenService } from 'src/app/services/token.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import {
  AnimalService,
  AnimalForSelectDto,
  TemplateService,
  MedicalRecordService,
} from 'src/app/services/generated-api-code';
import { Component, OnInit, ViewChild } from '@angular/core';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { atLeastOne } from 'src/app/validators/at-least-one.validator';
import { MatSelectChange } from '@angular/material/select';
import { FileItem, FileUploader } from 'ng2-file-upload';
import { UserPickerComponent } from '../../_partials/user-picker/user-picker.component';

export interface LocalMedicationRecord {
  localId: number;
  amount: number;
  id?: string;
  name?: string | undefined;
  unitName?: string | undefined;
}

export interface LocalTherapiaRecord {
  localId: number;
  amount: number;
  id?: string;
  name?: string | undefined;
}

@Component({
  selector: 'app-add-medical-record',
  templateUrl: './add-medical-record.component.html',
  styleUrls: ['./add-medical-record.component.scss'],
})
export class AddMedicalRecordComponent implements OnInit {
  @ViewChild(MedicationPickerComponent) medicationPicker: MedicationPickerComponent;
  @ViewChild(TherapiaPickerComponent) therapiaPicker: TherapiaPickerComponent;
  @ViewChild(UserPickerComponent) userPicker: UserPickerComponent;

  recordForm: FormGroup;
  animals: AnimalForSelectDto[] = [];
  templates: MedicalRecordTextTemplate[] = [];
  files: FileItem[] = [];

  uploader: FileUploader;
  hasBaseDropZoneOver = false;

  appointmentId: string = '';
  recordId: string = '';
  record: MedicalRecordEditDto;

  medicationForm: FormGroup;
  therapiaForm: FormGroup;

  medicationToRecord: LocalMedicationRecord[] = [];
  therapiaToRecord: LocalTherapiaRecord[] = [];
  photosToDelete: string[] = [];

  constructor(
    private tokenService: TokenService,
    private animalService: AnimalService,
    private templateService: TemplateService,
    private recordService: MedicalRecordService,
    private appointmentService: AppointmentService,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.appointmentId = this.route.snapshot.params['appointmentId'];
    this.recordId = this.route.snapshot.params['recordId'];
  }

  ngOnInit(): void {
    this.recordForm = this.fb.group(
      {
        doctorId: [this.tokenService.getUserData().id, Validators.required],
        date: [new Date(), Validators.required],
        ownerEmail: [''],
        ownerId: [null],
        animalId: [null],
        htmlContent: [''],
      },
      { validator: atLeastOne(Validators.required, ['ownerEmail', 'ownerId']) }
    );

    if (this.recordId) {
      this.recordService
        .getMedicalRecordsDetails(this.recordId)
        .subscribe((record: MedicalRecordEditDto) => {
          this.record = record;
          this.recordForm = this.fb.group(
            {
              doctorId: [this.tokenService.getUserData().id, Validators.required],
              date: [new Date(record.date), Validators.required],
              ownerEmail: [record.ownerEmail],
              ownerId: [record.ownerId],
              animalId: [record.animalId],
              htmlContent: [record.htmlContent],
            },
            { validator: atLeastOne(Validators.required, ['ownerEmail', 'ownerId']) }
          );
          if (record.ownerId) {
            this.userPicker.selectUserById(record.ownerId);
          } else {
            this.userPicker.selectUserByEmail(record.ownerEmail);
          }
          record.medicationRecords.forEach((medicationRecord) => {
            this.medicationToRecord.push({
              id: medicationRecord.id,
              name: medicationRecord.name,
              amount: medicationRecord.amount,
              unitName: medicationRecord.unitName,
              localId: this.medicationToRecord.length,
            });
          });
          record.therapiaRecords.forEach((therapiaRecord) => {
            this.therapiaToRecord.push({
              id: therapiaRecord.id,
              name: therapiaRecord.name,
              amount: therapiaRecord.amount,
              localId: this.therapiaToRecord.length,
            });
          });
        });
    }

    if (this.appointmentId) {
      this.recordService
        .getAppointmentDetails(this.appointmentId)
        .subscribe((appointment: AppointmentForRecordDto) => {
          this.recordForm.patchValue({
            ownerEmail: appointment.ownerEmail,
            ownerId: appointment.ownerId,
            animalId: appointment.animalId,
          });
          this.userPicker.selectUserById(appointment.ownerId);
          this.loadAnimals(appointment.ownerId);
        });
    }

    this.initializeUploader();

    this.medicationForm = this.fb.group({
      id: ['', Validators.required],
      name: [''],
      unitName: [''],
      amount: ['', [Validators.required, Validators.min(0)]],
    });

    this.therapiaForm = this.fb.group({
      id: ['', Validators.required],
      name: [''],
      amount: ['', [Validators.required, Validators.min(0)]],
    });

    this.templateService.getTemplates().subscribe((templates: MedicalRecordTextTemplate[]) => {
      this.templates = templates;
    });
  }

  loadAnimals(userIdentifier: string): void {
    if (userIdentifier) {
      this.animalService.getAnimalsForSelect(userIdentifier).subscribe(
        (animals: AnimalForSelectDto[]) => {
          this.recordForm.patchValue({ ownerId: userIdentifier, ownerEmail: '' });
          this.animals = animals;
        },
        () => this.recordForm.patchValue({ ownerEmail: userIdentifier, ownerId: null })
      );
    } else {
      this.recordForm.patchValue({ ownerEmail: '', ownerId: null });
    }
  }

  deletePhotoFromRecord(photoId: string) {
    if (confirm('Biztosan törölni szeretnéd a képet?')) {
      this.photosToDelete.push(photoId);
      this.record.photoUrls = this.record.photoUrls.filter((photo) => photo.id !== photoId);
    }
  }

  templateSelected(event: MatSelectChange): void {
    this.recordForm.patchValue({
      htmlContent:
        event.value === 'empty'
          ? ''
          : this.templates.find(({ id }) => id === event.value).htmlContent,
    });
  }

  medicationSelected(medication: MedicationForSelectDto): void {
    if (medication == null) {
      this.medicationForm.patchValue({ id: '', unitName: '', name: '' });
    } else {
      this.medicationForm.patchValue({
        id: medication.id,
        unitName: medication.unitName,
        name: medication.name,
      });
    }
  }

  therapiaSelected(therapia: TherapiaForSelectDto): void {
    if (therapia == null) {
      this.therapiaForm.patchValue({ id: '', name: '' });
    } else {
      this.therapiaForm.patchValue({
        id: therapia.id,
        name: therapia.name,
      });
    }
  }

  addMedication(): void {
    this.medicationToRecord.push({
      id: this.medicationForm.get('id').value,
      name: this.medicationForm.get('name').value,
      amount: this.medicationForm.get('amount').value,
      unitName: this.medicationForm.get('unitName').value,
      localId: this.medicationToRecord.length,
    });
    this.medicationForm.reset();
    this.medicationPicker.clearForm();
  }

  addTherapia(): void {
    this.therapiaToRecord.push({
      id: this.therapiaForm.get('id').value,
      name: this.therapiaForm.get('name').value,
      amount: this.therapiaForm.get('amount').value,
      localId: this.therapiaToRecord.length,
    });
    this.therapiaForm.reset();
    this.therapiaPicker.clearForm();
  }

  removeMedicationFromRecord(medicationId): void {
    this.medicationToRecord = this.medicationToRecord.filter(
      ({ localId }) => localId !== medicationId
    );
  }

  removeTherapiaFromRecord(therapiaId): void {
    this.therapiaToRecord = this.therapiaToRecord.filter(({ localId }) => localId !== therapiaId);
  }

  executeSaveOrUpdate(): void {
    if (this.record) {
      this.update();
    } else {
      this.save();
    }
  }

  save(): void {
    this.recordService
      .createMedicalRecord({
        data: this.recordForm.value,
        medications: this.medicationToRecord.map((localRecord) => {
          return { id: localRecord.id, amount: localRecord.amount };
        }),
        therapias: this.therapiaToRecord.map((localRecord) => {
          return { id: localRecord.id, amount: localRecord.amount };
        }),
      })
      .subscribe((recordId: string) => {
        if (this.appointmentId) {
          this.appointmentService
            .updateAppointmentStatus({
              appointmentId: this.appointmentId,
              statusId: AppointmentStatusEnum.Closed,
            })
            .subscribe(() => {
              if (this.files.length === 0) {
                this.router.navigateByUrl('/');
              }

              this.files.forEach((file) => {
                this.recordService
                  .addPhoto(recordId, { fileName: 'photo', data: file._file })
                  .subscribe(() => {
                    if (this.files.indexOf(file) === this.files.length - 1) {
                      this.router.navigateByUrl('/');
                    }
                  });
              });
            });
        }
      });
  }

  update(): void {
    this.recordService
      .updateMedicalRecord({
        medicalRecordId: this.recordId,
        data: this.recordForm.value,
        medications: this.medicationToRecord.map((localRecord) => {
          return { id: localRecord.id, amount: localRecord.amount };
        }),
        therapias: this.therapiaToRecord.map((localRecord) => {
          return { id: localRecord.id, amount: localRecord.amount };
        }),
      })
      .subscribe(() => {
        this.photosToDelete.forEach((photoId) => {
          this.recordService.removePhotoFromRecord(this.recordId, photoId).subscribe(() => {
            if (this.photosToDelete.indexOf(photoId) == this.photosToDelete.length - 1) {
              if (this.files.length === 0) {
                this.router.navigateByUrl('/');
              }
              this.files.forEach((file) => {
                this.recordService
                  .addPhoto(this.recordId, { fileName: 'photo', data: file._file })
                  .subscribe(() => {
                    if (this.files.indexOf(file) === this.files.length - 1) {
                      this.router.navigateByUrl('/');
                    }
                  });
              });
            }
          });
        });
      });
  }

  removePhoto(photo: FileItem) {
    this.files = this.files.splice(this.files.indexOf(photo), 1);
    photo.remove();
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: API_BASE_URL + 'records/add-photo/',
      authToken: 'Bearer ' + this.tokenService.getToken(),
      isHTML5: true,
      allowedFileType: ['image'],
      autoUpload: false,
      maxFileSize: 8 * 1024 * 1024 * 10,
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
      this.files.push(file);
    };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        console.log(response);
      }
    };
  }

  fileOverBase(e: any) {
    this.hasBaseDropZoneOver = e;
  }

  editorConfig: AngularEditorConfig = {
    editable: true,
    spellcheck: false,
    height: '400px',
    minHeight: '200px',
    maxHeight: 'auto',
    width: 'auto',
    minWidth: '0',
    translate: '',
    enableToolbar: true,
    showToolbar: true,
    placeholder: '',
    defaultParagraphSeparator: '',
    defaultFontName: 'Courier New',
    defaultFontSize: '3',
    fonts: [
      { class: 'courier-new', name: 'Courier New' },
      { class: 'times-new-roman', name: 'Times New Roman' },
      { class: 'arial', name: 'Arial' },
    ],
    sanitize: true,
    toolbarPosition: 'top',
    toolbarHiddenButtons: [
      ['link', 'unlink', 'insertImage', 'insertVideo'],
      [
        'indent',
        'outdent',
        'textColor',
        'backgroundColor',
        'customClasses',
        'justifyLeft',
        'justifyCenter',
        'justifyRight',
        'justifyFull',
      ],
    ],
  };
}
