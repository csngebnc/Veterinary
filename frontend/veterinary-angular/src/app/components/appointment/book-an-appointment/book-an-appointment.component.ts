import { TokenService } from 'src/app/services/token.service';
import { ActivatedRoute, Router } from '@angular/router';
import {
  AvailableTime,
  TreatmentService,
  TreatmentIntervalService,
  HolidayService,
  HolidayDto,
  AnimalForSelectDto,
  AnimalService,
} from 'src/app/services/generated-api-code';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import {
  AppointmentService,
  DoctorForAppointmentDto,
  TreatmentDto,
} from './../../../services/generated-api-code';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatVerticalStepper } from '@angular/material/stepper';
import { MatCalendar } from '@angular/material/datepicker';
import { DatePipe } from '@angular/common';
export interface AppointmentFormSelectedValues {
  doctorName?: string;
  treatmentName?: string;
  time?: string;
  animalName?: string;
}

@Component({
  selector: 'app-book-an-appointment',
  templateUrl: './book-an-appointment.component.html',
  styleUrls: ['./book-an-appointment.component.scss'],
})
export class BookAnAppointmentComponent implements OnInit {
  @ViewChild(MatVerticalStepper) stepper: MatVerticalStepper;
  @ViewChild(MatCalendar) calendar: MatCalendar<Date>;

  animals: AnimalForSelectDto[] = [];
  doctors: DoctorForAppointmentDto[] = [];
  treatments: TreatmentDto[] = [];
  availableTimes: AvailableTime[] = [];
  addAppointmentForm: FormGroup;
  availableDays: Date[] = [];
  selectedTime: number;

  minDate: Date = new Date();

  selectedValues: AppointmentFormSelectedValues = {};
  showCalendar = false;

  constructor(
    private tokenService: TokenService,
    private appointmentService: AppointmentService,
    private animalService: AnimalService,
    private treatmentService: TreatmentService,
    private treatmentIntervalService: TreatmentIntervalService,
    private holidayService: HolidayService,
    private fb: FormBuilder,
    private datePipe: DatePipe,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    let day = new Date();
    for (let count = 0; count < 30; count++) {
      this.availableDays.push(new Date(day.getFullYear(), day.getMonth(), day.getDate()));
      day = new Date(day.setDate(day.getDate() + 1));
    }
    this.addAppointmentForm = this.fb.group({
      doctorId: ['', Validators.required],
      treatmentId: ['', Validators.required],
      startDate: ['', Validators.required],
      endDate: ['', Validators.required],
      animalId: [null],
      reasons: [''],
    });
    this.appointmentService.getDoctors().subscribe((doctors: DoctorForAppointmentDto[]) => {
      this.doctors = doctors;
    });
  }

  bookAppointment(): void {
    console.log(this.addAppointmentForm.get('animalId').value == null);
    const appointment = {
      doctorId: this.addAppointmentForm.get('doctorId').value,
      treatmentId: this.addAppointmentForm.get('treatmentId').value,
      startDate: this.addAppointmentForm.get('startDate').value,
      endDate: this.addAppointmentForm.get('endDate').value,
      animalId: this.addAppointmentForm.get('animalId').value,
      reasons: this.addAppointmentForm.get('reasons').value ?? '',
    };

    this.appointmentService
      .bookAnAppointment(appointment)
      .subscribe(() => this.router.navigateByUrl('/'));
  }

  doctorSelected(): void {
    var doctorId = this.addAppointmentForm.get('doctorId').value;
    this.selectedValues.doctorName = this.doctors.find(({ id }) => id === doctorId).name;
    this.treatmentService
      .getTreatmentsDoctorId(doctorId)
      .subscribe((treatments: TreatmentDto[]) => {
        this.treatments = treatments;
        this.holidayService
          .getDoctorHolidaysByInterval(doctorId, this.availableDays[0], 30)
          .subscribe((holidays: HolidayDto[]) => {
            let disabledDates: Date[] = [];
            holidays.forEach((holiday) => {
              let holidayStart = new Date(holiday.startDate);
              while (holidayStart < new Date(holiday.endDate)) {
                disabledDates.push(
                  new Date(
                    holidayStart.getFullYear(),
                    holidayStart.getMonth(),
                    holidayStart.getDate()
                  )
                );
                holidayStart = new Date(holidayStart.setDate(holidayStart.getDate() + 1));
              }

              this.availableDays = this.availableDays.filter(
                (d) =>
                  disabledDates.filter(
                    (hd) =>
                      hd.getFullYear() == d.getFullYear() &&
                      hd.getMonth() == d.getMonth() &&
                      hd.getDate() == d.getDate()
                  ).length <= 0
              );
            });

            this.stepper.next();
          });
      });
  }

  treatmentSelected(): void {
    this.selectedValues.treatmentName = this.treatments.find(
      ({ id }) => id === this.addAppointmentForm.get('treatmentId').value
    ).name;

    this.treatmentIntervalService
      .getTreatmentIntervalDays(this.addAppointmentForm.get('treatmentId').value)
      .subscribe((days: number[]) => {
        this.availableDays = this.availableDays.filter(
          (availableDay) => days.indexOf(availableDay.getDay()) != -1
        );
        this.showCalendar = true;
        this.loadDay(0);
      });
  }

  loadDay(index: number): void {
    if (index > this.availableDays.length - 1 && this.availableTimes.length == 0) {
      this.availableTimes = [];
      return;
    }
    this.appointmentService
      .getDoctorTreatmentAvailableTimes(
        this.availableDays[index],
        this.addAppointmentForm.get('doctorId').value,
        this.addAppointmentForm.get('treatmentId').value
      )
      .subscribe((times: AvailableTime[]) => {
        if (times.length == 0) {
          this.loadDay(index + 1);
        } else {
          times.forEach((time) => {
            this.availableTimes.push({
              id: time.id,
              startTime: new Date(time.startTime),
              endTime: new Date(time.endTime),
            });
          });
          this.calendar.selected = this.availableDays[index];
          this.calendar.activeDate = this.availableDays[index];
          this.calendar.updateTodaysDate();
          this.stepper.next();
        }
      });
  }

  setEmpty(): void {
    this.addAppointmentForm.patchValue({ animalId: null });
    this.selectedValues.animalName = '';
  }

  onValueChange(value: Date): void {
    this.calendar.selected = value;
    this.calendar.updateTodaysDate();
    this.appointmentService
      .getDoctorTreatmentAvailableTimes(
        value,
        this.addAppointmentForm.get('doctorId').value,
        this.addAppointmentForm.get('treatmentId').value
      )
      .subscribe((times: AvailableTime[]) => {
        this.availableTimes = [];
        times.forEach((time) => {
          this.availableTimes.push({
            id: time.id,
            startTime: new Date(time.startTime),
            endTime: new Date(time.endTime),
          });
        });
      });
  }

  customDateFilter = (dateToBeValidated): boolean => {
    let day = new Date(dateToBeValidated);
    return (
      this.availableDays.filter(
        (availableDay) =>
          availableDay.getFullYear() == day.getFullYear() &&
          availableDay.getMonth() == day.getMonth() &&
          availableDay.getDate() == day.getDate()
      ).length > 0
    );
  };

  getSelectedPropName(propName: string, noBraces?: boolean): string {
    if (noBraces) {
      return this.selectedValues[propName];
    }
    return `- (${this.selectedValues[propName]})`;
  }

  addReasons(): void {
    this.selectedValues.animalName = this.animals.find(
      ({ id }) => id == this.addAppointmentForm.get('animalId').value
    )?.name;
  }

  setSelectedTime(timeId: number): void {
    this.selectedTime = timeId;
    var time = this.availableTimes.find(({ id }) => id == timeId);
    this.addAppointmentForm.patchValue({ startDate: time.startTime, endDate: time.endTime });
    this.selectedValues.time = `${this.datePipe.transform(
      time.startTime,
      'yyyy. MM. dd. HH:mm'
    )} - ${this.datePipe.transform(time.startTime, 'yyyy. MM. dd. HH:mm')}`;
    this.chooseAnimal();
  }

  chooseAnimal(): void {
    var ownerId = this.route.snapshot.paramMap.get('userId') ?? this.tokenService.getUserData().id;
    this.animalService.getAnimalsForSelect(ownerId).subscribe((animals: AnimalForSelectDto[]) => {
      this.animals = animals;
      this.stepper.next();
    });
  }
}
