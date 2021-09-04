import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import {
  AnimalService,
  OwnedAnimalDto,
  PagedListOfOwnedAnimalDto,
} from 'src/app/services/generated-api-code';
import { ModalService } from 'src/app/services/modal.service';
import { TokenService } from 'src/app/services/token.service';
import { AddAnimalComponent } from '../add-animal/add-animal.component';

@Component({
  selector: 'app-animal-list',
  templateUrl: './animal-list.component.html',
  styleUrls: ['./animal-list.component.scss'],
})
export class AnimalListComponent implements OnInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  pageEvent: PageEvent;
  animalsDataSource = new MatTableDataSource<OwnedAnimalDto>();
  length: number = 0;
  isArchivedPage: boolean = false;

  userIdFromRoute: string = '';

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private tokenService: TokenService,
    private animalService: AnimalService,
    private modalService: ModalService
  ) {}

  ngOnInit(): void {
    this.animalsDataSource.paginator = this.paginator;
    this.userIdFromRoute = this.route.snapshot.paramMap.get('userid');

    this.validatePermission();
    this.pageChanged({ pageIndex: 0, pageSize: 6, length: 0 });
  }

  private validatePermission(): void {
    let userData = this.tokenService.getUserData();

    if (userData.role === 'User' && userData.id !== this.userIdFromRoute) {
      this.router.navigateByUrl('/');
    }
  }

  pageChanged(event: PageEvent): PageEvent {
    this.animalService
      .getOwnedAnimals(
        this.userIdFromRoute,
        6,
        event.pageIndex,
        this.isArchivedPage
      )
      .subscribe((response: PagedListOfOwnedAnimalDto) => {
        this.animalsDataSource.data = response.items;
        this.length = response.totalCount;
      });
    return event;
  }

  changePage(): void {
    this.isArchivedPage = !this.isArchivedPage;
    this.pageChanged({ pageIndex: 0, pageSize: 6, length: 0 });
  }

  open(): void {
    this.modalService.openModal(AddAnimalComponent, () =>
      this.pageChanged({ pageIndex: 0, pageSize: 6, length: 0 })
    );
  }

  deleteAnimal(animalId: string) {
    this.animalService.deleteAnimal(animalId).subscribe(() => {
      this.animalsDataSource.data = this.animalsDataSource.data.filter(
        (animal) => animal.id !== animalId
      );
      this.length -= 1;
    });
  }

  archiveAnimal(animalId: string) {
    this.animalService.updateAnimalArchiveStatus(animalId).subscribe(() => {
      this.animalsDataSource.data = this.animalsDataSource.data.filter(
        (animal) => animal.id !== animalId
      );
      this.length -= 1;
    });
  }
}
