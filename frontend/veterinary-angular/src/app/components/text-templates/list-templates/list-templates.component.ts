import { ViewTemplateComponent } from './../view-template/view-template.component';
import { AddTemplateComponent } from './../add-template/add-template.component';
import { EditTemplateComponent } from './../edit-template/edit-template.component';
import { MedicalRecordTextTemplate } from './../../../services/generated-api-code';
import { Component, OnInit } from '@angular/core';
import { TemplateService } from 'src/app/services/generated-api-code';
import { MatTableDataSource } from '@angular/material/table';
import { ModalService } from 'src/app/services/modal.service';

@Component({
  selector: 'app-list-templates',
  templateUrl: './list-templates.component.html',
  styleUrls: ['./list-templates.component.scss'],
})
export class ListTemplatesComponent implements OnInit {
  textTemplates: MedicalRecordTextTemplate[] = [];

  dataSource: MatTableDataSource<MedicalRecordTextTemplate> =
    new MatTableDataSource<MedicalRecordTextTemplate>([]);
  displayedColumns: string[] = ['name', 'button'];

  constructor(private textTemplatesService: TemplateService, private modalService: ModalService) {}

  ngOnInit(): void {
    this.textTemplatesService
      .getTemplates()
      .subscribe((textTemplates: MedicalRecordTextTemplate[]) => {
        this.textTemplates = textTemplates;
        this.dataSource.data = this.textTemplates;
      });
  }

  add() {
    this.modalService.openModal(
      AddTemplateComponent,
      (textTemplates: MedicalRecordTextTemplate) => {
        this.textTemplates.push(textTemplates);
        this.dataSource.data = this.textTemplates;
      },
      {},
      'lg'
    );
  }

  open(textTemplateId) {
    this.modalService.openModal(
      ViewTemplateComponent,
      () => {},
      {
        textTemplate: this.textTemplates.find(({ id }) => id === textTemplateId),
      },
      'lg'
    );
  }

  edit(textTemplateId: string) {
    this.modalService.openModal(
      EditTemplateComponent,
      (textTemplate: MedicalRecordTextTemplate) => {
        this.textTemplates[this.textTemplates.findIndex(({ id }) => id === textTemplateId)] =
          textTemplate;
        this.dataSource.data = this.textTemplates;
      },
      { template: this.textTemplates.find(({ id }) => id === textTemplateId) },
      'lg'
    );
  }

  delete(textTemplateId: string) {
    this.textTemplatesService.deleteTemplate(textTemplateId).subscribe(() => {
      this.textTemplates = this.textTemplates.filter(({ id }) => id !== textTemplateId);
      this.dataSource.data = this.textTemplates;
    });
  }
}
