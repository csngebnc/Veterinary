import { MedicalRecordTextTemplate } from './../../../services/generated-api-code';
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TemplateService } from 'src/app/services/generated-api-code';
import { AngularEditorConfig } from '@kolkov/angular-editor';

@Component({
  selector: 'app-edit-template',
  templateUrl: './edit-template.component.html',
  styleUrls: ['./edit-template.component.scss'],
})
export class EditTemplateComponent implements OnInit {
  templateForm: FormGroup;

  @Input() data: any;
  template: MedicalRecordTextTemplate;

  constructor(
    private templateService: TemplateService,
    private fb: FormBuilder,
    private ngbModal: NgbActiveModal
  ) {}

  ngOnInit(): void {
    this.template = this.data.template;
    this.templateForm = this.fb.group({
      templateId: [this.template.id, Validators.required],
      name: [this.template.name, Validators.required],
      htmlContent: [this.template.htmlContent, Validators.required],
    });
  }

  updateTemplate(): void {
    this.templateService.updateTemplate(this.templateForm.value).subscribe(() => {
      this.template.name = this.templateForm.get('name').value;
      this.template.htmlContent = this.templateForm.get('htmlContent').value;
      this.ngbModal.close(this.template);
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
