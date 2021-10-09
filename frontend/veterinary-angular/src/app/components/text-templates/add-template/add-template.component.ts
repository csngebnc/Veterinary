import { TemplateService, MedicalRecordTextTemplate } from './../../../services/generated-api-code';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { AngularEditorConfig } from '@kolkov/angular-editor';

@Component({
  selector: 'app-add-template',
  templateUrl: './add-template.component.html',
  styleUrls: ['./add-template.component.scss'],
})
export class AddTemplateComponent implements OnInit {
  templateForm: FormGroup;

  constructor(
    private templateService: TemplateService,
    private fb: FormBuilder,
    private ngbModal: NgbActiveModal
  ) {}

  ngOnInit(): void {
    this.templateForm = this.fb.group({
      name: ['', Validators.required],
      htmlContent: ['', Validators.required],
    });
  }

  saveTemplate(): void {
    this.templateService
      .createTemplate(this.templateForm.value)
      .subscribe((template: MedicalRecordTextTemplate) => {
        this.ngbModal.close(template);
      });
  }

  close(): void {
    this.ngbModal.dismiss();
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
