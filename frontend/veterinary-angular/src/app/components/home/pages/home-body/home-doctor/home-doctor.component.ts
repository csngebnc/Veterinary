import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-home-doctor',
  templateUrl: './home-doctor.component.html',
  styleUrls: ['./home-doctor.component.scss'],
})
export class HomeDoctorComponent implements OnInit {
  @Input() doctorId: string;

  constructor() {}

  ngOnInit(): void {}
}
