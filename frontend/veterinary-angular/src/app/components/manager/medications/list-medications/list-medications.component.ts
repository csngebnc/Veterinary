import { EditMedicationComponent } from './../edit-medication/edit-medication.component'
import { AddMedicationComponent } from './../add-medication/add-medication.component'
import {
  MedicationDto,
  MedicationService,
  PagedListOfMedicationDto,
} from './../../../../services/generated-api-code'
import { Component, OnInit, ViewChild } from '@angular/core'
import { MatTableDataSource } from '@angular/material/table'
import { ModalService } from 'src/app/services/modal.service'
import { MatPaginator, PageEvent } from '@angular/material/paginator'

@Component({
  selector: 'app-list-medications',
  templateUrl: './list-medications.component.html',
  styleUrls: ['./list-medications.component.scss'],
})
export class ListMedicationsComponent implements OnInit {
  @ViewChild(MatPaginator) paginator: MatPaginator
  pageEvent: PageEvent

  dataSource: MatTableDataSource<MedicationDto> = new MatTableDataSource<MedicationDto>()
  displayedColumns: string[] = ['name', 'unit', 'pricePerUnit', 'status', 'button']
  length: number = 0

  medications: MedicationDto[] = []

  constructor(private medicationService: MedicationService, private modalService: ModalService) {}

  ngOnInit(): void {
    this.loadMedications({ pageIndex: 0, pageSize: 10, length: 0 })
    this.dataSource.paginator = this.paginator
  }

  loadMedications(event: PageEvent) {
    this.medicationService
      .getMedicationsWithDetails(event.pageSize, event.pageIndex)
      .subscribe((medications: PagedListOfMedicationDto) => {
        this.medications = medications.items
        this.length = medications.totalCount
        this.dataSource.data = this.medications
      })
  }

  pageChanged(event: PageEvent): PageEvent {
    this.loadMedications(event)
    return event
  }

  add() {
    this.modalService.openModal(AddMedicationComponent, (medication: MedicationDto) => {
      this.medications.push(medication)
      this.length = this.length + 1
      this.dataSource.data = this.medications
    })
  }

  edit(medicationId: string) {
    this.modalService.openModal(
      EditMedicationComponent,
      (medication: MedicationDto) => {
        this.medications[this.medications.findIndex((v) => v.id === medicationId)] = medication
        this.dataSource.data = this.medications
      },
      { medication: this.medications.find((s) => s.id === medicationId) }
    )
  }

  changeState(medicationId: string) {
    this.medicationService.updateMedicationStatus(medicationId).subscribe(() => {
      let index = this.medications.findIndex((s) => s.id == medicationId)
      this.medications[index].isInactive = !this.medications[index].isInactive
    })
  }

  delete(medicationId: string) {
    this.medicationService.deleteMedication(medicationId).subscribe(() => {
      this.medications = this.medications.filter((s) => s.id !== medicationId)
      this.length = this.length - 1
      this.dataSource.data = this.medications
    })
  }
}
