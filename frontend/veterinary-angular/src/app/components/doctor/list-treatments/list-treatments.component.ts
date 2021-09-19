import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-list-treatments',
  templateUrl: './list-treatments.component.html',
  styleUrls: ['./list-treatments.component.scss']
})
export class ListTreatmentsComponent implements OnInit {
  species: AnimalSpeciesDto[] = [];

  dataSource: MatTableDataSource<AnimalSpeciesDto>;
  displayedColumns: string[] = ['name', 'status', 'button'];

  constructor(
    private speciesService: SpeciesService,
    private modalService: ModalService
  ) {}

  ngOnInit(): void {
    this.speciesService
      .getAnimalSpecies()
      .subscribe((species: AnimalSpeciesDto[]) => {
        this.species = species;
        this.dataSource = new MatTableDataSource<AnimalSpeciesDto>(
          this.species
        );
      });
  }

  add() {
    this.modalService.openModal(
      AddAnimalSpeciesComponent,
      (species: AnimalSpeciesDto) => {
        this.species.push(species);
        this.dataSource.data = this.species;
      }
    );
  }

  edit(speciesId: string) {
    this.modalService.openModal(
      EditAnimalSpeciesComponent,
      (species: AnimalSpeciesDto) => {
        this.species[this.species.findIndex((s) => s.id === speciesId)] =
          species;
        this.dataSource.data = this.species;
      },
      { species: this.species.find((s) => s.id === speciesId) }
    );
  }

  changeState(speciesId: string) {
    this.speciesService.updateAnimalSpeciesStatus(speciesId).subscribe(() => {
      let index = this.species.findIndex((s) => s.id == speciesId);
      this.species[index].isInactive = !this.species[index].isInactive;
    });
  }

  delete(speciesId: string) {
    this.speciesService.deleteAnimalSpecies(speciesId).subscribe(() => {
      this.species = this.species.filter((s) => s.id !== speciesId);
      this.dataSource.data = this.species;
    });
  }
}
