import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import {
  TreatmentIntervalDetailsDto,
  TreatmentIntervalService,
} from 'src/app/services/generated-api-code';

@Component({
  selector: 'app-edit-treatment-interval',
  templateUrl: './edit-treatment-interval.component.html',
  styleUrls: ['./edit-treatment-interval.component.scss'],
})
export class EditTreatmentIntervalComponent implements OnInit {
  data: any;

  editIntervalForm: FormGroup;
  editedInterval: TreatmentIntervalDetailsDto;
  selectedDayIndex: number;

  constructor(
    private intervalService: TreatmentIntervalService,
    private fb: FormBuilder,
    private ngbModal: NgbActiveModal
  ) {}

  ngOnInit(): void {
    this.editedInterval = this.data.interval;
    this.editIntervalForm = this.fb.group({
      id: [this.editedInterval.id, Validators.required],
      treatmentId: [this.editedInterval.treatmentId, Validators.required],
      startHour: [
        this.editedInterval.startHour,
        [Validators.required, Validators.pattern('\\d*'), Validators.min(0), Validators.max(23)],
      ],
      startMin: [
        this.editedInterval.startMin,
        [Validators.required, Validators.pattern('\\d*'), Validators.min(0), Validators.max(59)],
      ],
      endHour: [
        this.editedInterval.endHour,
        [Validators.required, Validators.pattern('\\d*'), Validators.min(0), Validators.max(23)],
      ],
      endMin: [
        this.editedInterval.endMin,
        [Validators.required, Validators.pattern('\\d*'), Validators.min(0), Validators.max(59)],
      ],
      dayOfWeek: [
        this.editedInterval.dayOfWeek,
        [Validators.required, Validators.pattern('\\d*'), Validators.min(1), Validators.max(7)],
      ],
    });
  }

  getTime(property: string): string {
    return this.editedInterval[property + 'Hour'] + ':' + this.editedInterval[property + 'Min'];
  }

  selectDay(dayIndex: number): void {
    this.selectedDayIndex = dayIndex;
    this.editIntervalForm.patchValue({ dayOfWeek: dayIndex });
  }

  edit(): void {
    this.intervalService
      .updateTreatmentInterval(this.data.doctorId, this.editIntervalForm.value)
      .subscribe(() => this.ngbModal.close(this.editIntervalForm.value));
  }

  close(): void {
    this.ngbModal.dismiss();
  }

  startInput(timeString: any) {
    let times: string = timeString.target.value.split(':');
    this.editIntervalForm.patchValue({ startHour: Number(times[0]), startMin: Number(times[1]) });
  }

  endInput(timeString: any) {
    let times: string = timeString.target.value.split(':');
    this.editIntervalForm.patchValue({ endHour: Number(times[0]), endMin: Number(times[1]) });
  }
}
