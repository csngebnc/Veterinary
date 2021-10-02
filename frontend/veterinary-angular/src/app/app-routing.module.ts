import { ListHolidaysComponent } from './components/doctor/holidays/list-holidays/list-holidays.component';
import { ListTreatmentsComponent } from './components/doctor/treatments/list-treatments/list-treatments.component';
import { ListTherapiasComponent } from './components/manager/therapias/list-therapias/list-therapias.component';
import { ListMedicationsComponent } from './components/manager/medications/list-medications/list-medications.component';
import { ListDoctorsComponent } from './components/manager/doctors/list-doctors/list-doctors.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AnimalListComponent } from './components/animal/animal-list/animal-list.component';
import { EditAnimalComponent } from './components/animal/edit-animal/edit-animal.component';
import { ListMedicalRecordsComponent } from './components/animal/medical-records/list-medical-records/list-medical-records.component';
import { ListAnimalSpeciesComponent } from './components/animalspecies/list-animal-species/list-animal-species.component';
import { HomeComponent } from './components/home/home.component';
import { ListVaccinesComponent } from './components/vaccines/list-vaccines/list-vaccines.component';
import { AuthGuard } from './guards/auth.guard';
import { ListTreatmentIntervalsComponent } from './components/doctor/treatment-intervals/list-treatment-intervals/list-treatment-intervals.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    canActivate: [AuthGuard],
    pathMatch: 'full',
  },
  {
    path: 'animals/:userid',
    component: AnimalListComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'animal/:animalid',
    component: EditAnimalComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'records/:animalid',
    component: ListMedicalRecordsComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'species',
    component: ListAnimalSpeciesComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'vaccines',
    component: ListVaccinesComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'doctors',
    component: ListDoctorsComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'medications',
    component: ListMedicationsComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'therapias',
    component: ListTherapiasComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'treatments/:doctorid',
    component: ListTreatmentsComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'treatments/:doctorid/:treatmentid',
    component: ListTreatmentIntervalsComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'holidays/:doctorid',
    component: ListHolidaysComponent,
    canActivate: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
