import { OAuthService } from 'angular-oauth2-oidc';
import { Injectable } from '@angular/core';
import { CanActivate, CanActivateChild } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate, CanActivateChild {
  constructor(private authService: OAuthService) {}

  canActivate(): boolean {
    if (!this.authService.hasValidAccessToken()) {
      this.authService.initCodeFlow();
      return false;
    } else {
      return true;
    }
  }

  canActivateChild(): boolean {
    return this.canActivate();
  }
}
