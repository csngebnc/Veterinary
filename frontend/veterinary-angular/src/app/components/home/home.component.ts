import { Component, OnInit } from '@angular/core';
import { TokenService, UserData } from 'src/app/services/token.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  userInfo: UserData;

  constructor(private tokenService: TokenService) {}

  ngOnInit(): void {
    this.userInfo = this.tokenService.getUserData();
  }
}
