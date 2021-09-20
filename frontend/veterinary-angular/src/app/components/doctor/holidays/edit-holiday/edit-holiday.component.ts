import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { HolidayService } from 'src/app/services/generated-api-code';

@Component({
  selector: 'app-edit-holiday',
  templateUrl: './edit-holiday.component.html',
  styleUrls: ['./edit-holiday.component.scss'],
})
export class EditHolidayComponent implements OnInit {
  data: any;
  editHolidayForm: FormGroup;

  constructor(
    private holidayService: HolidayService,
    private fb: FormBuilder,
    private ngbModal: NgbActiveModal
  ) {}

  ngOnInit(): void {
    this.editHolidayForm = this.fb.group({
      holidayId: [this.data.holiday.id, Validators.required],
      doctorId: [this.data.holiday.doctorId, Validators.required],
      startDate: [new Date(this.data.holiday.startDate), Validators.required],
      endDate: [new Date(this.data.holiday.endDate), Validators.required],
    });
  }

  editHoliday() {
    this.holidayService.updateHoliday(this.editHolidayForm.value).subscribe(() => {
      this.ngbModal.close(this.editHolidayForm.value);
    });
  }

  close(): void {
    this.ngbModal.dismiss();
  }
}
