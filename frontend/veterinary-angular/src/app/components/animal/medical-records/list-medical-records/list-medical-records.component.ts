import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AnimalDto, AnimalService } from 'src/app/services/generated-api-code';
import { TokenService } from 'src/app/services/token.service';

@Component({
  selector: 'app-list-medical-records',
  templateUrl: './list-medical-records.component.html',
  styleUrls: ['./list-medical-records.component.scss'],
})
export class ListMedicalRecordsComponent implements OnInit {
  constructor(
    private animalService: AnimalService,
    private route: ActivatedRoute,
    private modalService: NgbModal,
    private tokenService: TokenService
  ) {}

  animal: AnimalDto;
  medicalRecords: any[] = [
    {
      id: 1,
      date: new Date(),
      doctorName: 'Doki',
      anamnesis: 'Nincs',
      symptoma: 'Symp',
      details: 'részletek',
      therapiaRecords: [],
      photos: [],
    },
  ];

  ngOnInit(): void {
    const animalId = this.route.snapshot.paramMap.get('animalid');
    this.animalService.getAnimal(animalId).subscribe((animal: AnimalDto) => {
      this.animal = animal;
      // TODO: Load MedicalRecords
      // TODO: Pagination
    });
  }

  openVaccineRecordsDialog(): void {
    // TODO
  }
}
