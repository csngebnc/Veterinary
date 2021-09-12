import {
  FormGroup,
  FormBuilder,
  Validators,
  FormGroupDirective,
} from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import {
  UserService,
  DoctorWithRoleDto,
} from './../../../../services/generated-api-code';
import { Component, OnInit, ViewChild } from '@angular/core';
import { TokenService } from 'src/app/services/token.service';

@Component({
  selector: 'app-list-doctors',
  templateUrl: './list-doctors.component.html',
  styleUrls: ['./list-doctors.component.scss'],
})
export class ListDoctorsComponent implements OnInit {
  @ViewChild(FormGroupDirective) formDirective: FormGroupDirective;

  constructor(
    private tokenService: TokenService,
    private userSevice: UserService,
    private fb: FormBuilder
  ) {}

  addDoctorForm: FormGroup = this.fb.group({
    userId: ['', Validators.required],
    roleName: ['', Validators.required],
  });

  isAddActivated: boolean = false;
  editId: string = '';

  userIdsToRemove: string[] = [];

  dataSource: MatTableDataSource<DoctorWithRoleDto> = new MatTableDataSource();
  displayedColumns: string[] = ['name', 'email', 'role', 'button'];

  ngOnInit(): void {
    this.loadDoctors();
  }

  loadDoctors(): void {
    this.userSevice
      .getDoctorsWithRole()
      .subscribe((doctors: DoctorWithRoleDto[]) => {
        this.userIdsToRemove = doctors.map((doctor) => doctor.id);

        this.dataSource.data = doctors.filter(
          (doctor) => doctor.id !== this.tokenService.getUserData().id
        );
      });
  }

  userSelected(userId: string): void {
    this.addDoctorForm.patchValue({
      userId: userId,
    });
  }

  addDoctor(): void {
    this.userSevice
      .changeUserRole(
        this.addDoctorForm.get('userId').value,
        this.addDoctorForm.get('roleName').value
      )
      .subscribe(() => {
        this.loadDoctors();
        this.resetView();
      });
  }

  editDoctor(doctorId: string): void {
    this.isAddActivated = false;
    this.editId = doctorId;

    this.addDoctorForm.patchValue({
      userId: doctorId,
      roleName: this.dataSource.data.find((doctor) => doctor.id === doctorId)
        .role,
    });
    this.changeView();
  }

  removeDoctor(doctorId: string): void {
    this.userSevice.changeUserRole(doctorId, 'User').subscribe(() => {
      this.loadDoctors();
    });
  }

  changeView(): void {
    this.isAddActivated = !this.isAddActivated;
    this.formDirective?.resetForm();
  }

  resetView(): void {
    this.editId = '';
    this.changeView();
  }
}
