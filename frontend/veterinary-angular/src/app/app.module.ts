import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { OAuthModule, OAuthService } from 'angular-oauth2-oidc';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { environment } from 'src/environments/environment';
import * as generated from './services/generated-api-code';
import { apiUrl } from '../assets/configuration.json';
import { HttpClientModule } from '@angular/common/http';
import { generate } from 'rxjs';
import { HomeComponent } from './components/home/home.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { HomeHeaderComponent } from './components/home/pages/home-header/home-header.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialDesignModule } from './modules/material-design/material-design.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateRolePipe } from './pipes/translate-role.pipe';
import { HomeUserComponent } from './components/home/pages/home-body/home-user/home-user.component';
import { HomeDoctorComponent } from './components/home/pages/home-body/home-doctor/home-doctor.component';
import { AnimalListComponent } from './components/animal/animal-list/animal-list.component';
import { NgbModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AddAnimalComponent } from './components/animal/add-animal/add-animal.component';
import { ImageCropperModule } from 'ngx-image-cropper';
import { MAT_DATE_LOCALE } from '@angular/material/core';
import { MAT_RADIO_DEFAULT_OPTIONS } from '@angular/material/radio';
import { EditAnimalComponent } from './components/animal/edit-animal/edit-animal.component';
import { ListMedicalRecordsComponent } from './components/animal/medical-records/list-medical-records/list-medical-records.component';
import { MedicalRecordDisplayerComponent } from './components/_partials/medical-record-displayer/medical-record-displayer.component';

export function initializeApp(oauthService: OAuthService): any {
  return async () => {
    oauthService.configure({
      clientId: 'veterinary-angular',
      issuer: apiUrl,
      postLogoutRedirectUri: window.location.origin,
      redirectUri: window.location.origin,
      requireHttps: true,
      responseType: 'code',
      scope: 'openid api-openid',
      useSilentRefresh: true,
      skipIssuerCheck: true,
    });
    oauthService.setupAutomaticSilentRefresh();
    return oauthService.loadDiscoveryDocumentAndTryLogin();
  };
}

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NavMenuComponent,
    HomeHeaderComponent,
    TranslateRolePipe,
    HomeUserComponent,
    HomeDoctorComponent,
    AnimalListComponent,
    AddAnimalComponent,
    EditAnimalComponent,
    ListMedicalRecordsComponent,
    MedicalRecordDisplayerComponent,
  ],
  imports: [
    MaterialDesignModule,
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    OAuthModule.forRoot({
      resourceServer: {
        allowedUrls: [apiUrl],
        sendAccessToken: true,
      },
    }),
    BrowserAnimationsModule,
    NgbModule,
    ImageCropperModule,
  ],
  providers: [
    { provide: generated.API_BASE_URL, useValue: apiUrl },
    { provide: MAT_DATE_LOCALE, useValue: 'hu-HU' },
    { provide: MAT_RADIO_DEFAULT_OPTIONS, useValue: { color: 'primary' } },
    {
      provide: APP_INITIALIZER,
      useFactory: initializeApp,
      deps: [OAuthService],
      multi: true,
    },
    generated.AnimalService,
    generated.SpeciesService,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
