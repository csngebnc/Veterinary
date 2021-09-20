import { HolidayDto, HolidayService } from './../../../../services/generated-api-code';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-add-holiday',
  templateUrl: './add-holiday.component.html',
  styleUrls: ['./add-holiday.component.scss'],
})
export class AddHolidayComponent implements OnInit {
  data: any;
  addHolidayForm: FormGroup;

  constructor(
    private holidayService: HolidayService,
    private fb: FormBuilder,
    private ngbModal: NgbActiveModal
  ) {}

  ngOnInit(): void {
    this.addHolidayForm = this.fb.group({
      doctorId: [this.data.doctorId, Validators.required],
      startDate: ['', Validators.required],
      endDate: ['', Validators.required],
    });
  }

  addHoliday() {
    this.holidayService
      .createHoliday(this.addHolidayForm.value)
      .subscribe((holiday: HolidayDto) => {
        this.ngbModal.close(holiday);
      });
  }

  close(): void {
    this.ngbModal.dismiss();
  }
}
