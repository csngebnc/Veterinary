import { TreatmentIntervalDetailsDto } from './../../../../services/generated-api-code';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TreatmentIntervalService } from 'src/app/services/generated-api-code';

@Component({
  selector: 'app-add-treatment-interval',
  templateUrl: './add-treatment-interval.component.html',
  styleUrls: ['./add-treatment-interval.component.scss'],
})
export class AddTreatmentIntervalComponent implements OnInit {
  data: any;

  addIntervalForm: FormGroup;

  selectedDayIndex: number;

  constructor(
    private intervalService: TreatmentIntervalService,
    private fb: FormBuilder,
    private ngbModal: NgbActiveModal
  ) {}

  ngOnInit(): void {
    this.addIntervalForm = this.fb.group({
      treatmentId: [this.data.treatmentId, Validators.required],
      startHour: [
        '',
        [Validators.required, Validators.pattern('\\d*'), Validators.min(0), Validators.max(23)],
      ],
      startMin: [
        '',
        [Validators.required, Validators.pattern('\\d*'), Validators.min(0), Validators.max(59)],
      ],
      endHour: [
        '',
        [Validators.required, Validators.pattern('\\d*'), Validators.min(0), Validators.max(23)],
      ],
      endMin: [
        '',
        [Validators.required, Validators.pattern('\\d*'), Validators.min(0), Validators.max(59)],
      ],
      dayOfWeek: [
        '',
        [Validators.required, Validators.pattern('\\d*'), Validators.min(0), Validators.max(6)],
      ],
    });
  }

  selectDay(dayIndex: number): void {
    this.selectedDayIndex = dayIndex;
    this.addIntervalForm.patchValue({ dayOfWeek: dayIndex });
  }

  add(): void {
    this.intervalService
      .createTreatmentInterval(this.data.doctorId, this.addIntervalForm.value)
      .subscribe((interval: TreatmentIntervalDetailsDto) => this.ngbModal.close(interval));
  }

  close(): void {
    this.ngbModal.dismiss();
  }

  startInput(timeString: any) {
    let times: string = timeString.target.value.split(':');
    this.addIntervalForm.patchValue({ startHour: Number(times[0]), startMin: Number(times[1]) });
  }

  endInput(timeString: any) {
    let times: string = timeString.target.value.split(':');
    this.addIntervalForm.patchValue({ endHour: Number(times[0]), endMin: Number(times[1]) });
  }
}
