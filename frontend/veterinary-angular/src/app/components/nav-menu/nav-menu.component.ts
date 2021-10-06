import { BookAppointmentUserSelectorComponent } from './../doctor/appointments/book-appointment-user-selector/book-appointment-user-selector.component';
import { ModalService } from './../../services/modal.service';
import { Component, OnInit } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { TokenService } from 'src/app/services/token.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss'],
})
export class NavMenuComponent implements OnInit {
  constructor(
    public tokenService: TokenService,
    private modalService: ModalService,
    private authService: OAuthService
  ) {}

  ngOnInit(): void {}

  logOut(): void {
    this.authService.logOut();
  }

  getRole(): string {
    return this.tokenService.getUserData().role;
  }

  getUserId(): string {
    return this.tokenService.getUserData().id;
  }

  bookingMenu(): void {
    this.modalService.openModal(BookAppointmentUserSelectorComponent, () => {});
  }
}
