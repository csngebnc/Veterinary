import {
  TherapiaDto,
  TherapiaService,
  PagedListOfTherapiaDto,
} from './../../../../services/generated-api-code'
import { EditTherapiaComponent } from './../edit-therapia/edit-therapia.component'
import { AddTherapiaComponent } from './../add-therapia/add-therapia.component'
import { Component, OnInit, ViewChild } from '@angular/core'
import { MatPaginator, PageEvent } from '@angular/material/paginator'
import { MatTableDataSource } from '@angular/material/table'
import { ModalService } from 'src/app/services/modal.service'

@Component({
  selector: 'app-list-therapias',
  templateUrl: './list-therapias.component.html',
  styleUrls: ['./list-therapias.component.scss'],
})
export class ListTherapiasComponent implements OnInit {
  @ViewChild(MatPaginator) paginator: MatPaginator
  pageEvent: PageEvent

  dataSource: MatTableDataSource<TherapiaDto> = new MatTableDataSource<TherapiaDto>()
  displayedColumns: string[] = ['name', 'price', 'status', 'button']
  length: number = 0

  therapias: TherapiaDto[] = []

  constructor(private therapiaService: TherapiaService, private modalService: ModalService) {}

  ngOnInit(): void {
    this.loadTherapias({ pageIndex: 0, pageSize: 10, length: 0 })
    this.dataSource.paginator = this.paginator
  }

  loadTherapias(event: PageEvent) {
    this.therapiaService
      .getTherapias(event.pageSize, event.pageIndex)
      .subscribe((therapias: PagedListOfTherapiaDto) => {
        this.therapias = therapias.items
        this.length = therapias.totalCount
        this.dataSource.data = this.therapias
      })
  }

  pageChanged(event: PageEvent): PageEvent {
    this.loadTherapias(event)
    return event
  }

  add() {
    this.modalService.openModal(AddTherapiaComponent, (therapia: TherapiaDto) => {
      this.therapias.push(therapia)
      this.length = this.length + 1
      this.dataSource.data = this.therapias
    })
  }

  edit(therapiaId: string) {
    this.modalService.openModal(
      EditTherapiaComponent,
      (therapia: TherapiaDto) => {
        this.therapias[this.therapias.findIndex((v) => v.id === therapiaId)] = therapia
        this.dataSource.data = this.therapias
      },
      { therapia: this.therapias.find((s) => s.id === therapiaId) }
    )
  }

  changeState(therapiaId: string) {
    this.therapiaService.updateTherapiaStatus(therapiaId).subscribe(() => {
      let index = this.therapias.findIndex((s) => s.id == therapiaId)
      this.therapias[index].isInactive = !this.therapias[index].isInactive
    })
  }

  delete(therapiaId: string) {
    this.therapiaService.deleteTherapia(therapiaId).subscribe(() => {
      this.therapias = this.therapias.filter((s) => s.id !== therapiaId)
      this.length = this.length - 1
      this.dataSource.data = this.therapias
    })
  }
}
