import { TokenService } from 'src/app/services/token.service';
import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-list-doctor-appointments',
  templateUrl: './list-doctor-appointments.component.html',
  styleUrls: ['./list-doctor-appointments.component.scss'],
})
export class ListDoctorAppointmentsComponent implements OnInit {
  doctorId: string;

  constructor(private route: ActivatedRoute, private tokenService: TokenService) {}

  ngOnInit(): void {
    this.doctorId =
      this.route.snapshot.paramMap.get('doctorId') ?? this.tokenService.getUserData().id;
  }
}
