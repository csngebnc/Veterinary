import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AnimalListComponent } from './components/animal/animal-list/animal-list.component';
import { EditAnimalComponent } from './components/animal/edit-animal/edit-animal.component';
import { HomeComponent } from './components/home/home.component';
import { AuthGuard } from './guards/auth.guard';

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
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
