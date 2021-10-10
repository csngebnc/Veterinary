import { Injectable } from '@angular/core';

export class UserData {
  id: string;
  name: string;
  email: string;
  photoUrl: string;
  role: string;
}

@Injectable({
  providedIn: 'root',
})
export class TokenService {
  constructor() {}

  public getToken(): string {
    return sessionStorage.getItem('access_token');
  }

  public getUserData(): UserData {
    const token = this.getToken();
    if (!token) {
      return undefined;
    }

    const arr = token.split('.');
    const decoded = JSON.parse(this.b64DecodeUnicode(arr[1]));
    return {
      id: decoded['sub'],
      name: decoded['name'],
      email: decoded['email'],
      photoUrl: decoded['picture'],
      role: decoded['role'],
    };
  }

  b64DecodeUnicode(str) {
    return decodeURIComponent(
      atob(str)
        .split('')
        .map(function (c) {
          return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        })
        .join('')
    );
  }
}
