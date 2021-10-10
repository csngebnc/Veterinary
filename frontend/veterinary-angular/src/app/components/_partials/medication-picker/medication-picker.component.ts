import { MedicationForSelectDto, MedicationService } from './../../../services/generated-api-code';
import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'app-medication-picker',
  templateUrl: './medication-picker.component.html',
  styleUrls: ['./medication-picker.component.scss'],
})
export class MedicationPickerComponent implements OnInit, OnDestroy {
  @Input() title: string = 'Gyógyszer';
  @Input() placeholderText: string = '';

  @Output() medicationSelected = new EventEmitter<MedicationForSelectDto>();

  searchParamControl = new FormControl();
  options: MedicationForSelectDto[] = [];
  hasValueSelected: boolean = false;

  private searchParamChanged: Subscription;

  constructor(private medicationService: MedicationService) {}

  ngOnInit(): void {
    this.searchParamChanged = this.searchParamControl.valueChanges
      .pipe(debounceTime(500), distinctUntilChanged())
      .subscribe(() => {
        if (this.searchParamControl.value?.length > 0) {
          if (this.hasValueSelected) {
            return;
          }
          this.medicationService
            .searchMedication(this.searchParamControl.value)
            .subscribe((result: MedicationForSelectDto[]) => {
              this.options = result;

              if (result.length === 0) {
                this.medicationSelected.emit(null);
              }
            });
        } else if (this.searchParamControl.value?.length === 0) {
          this.hasValueSelected = false;
          this.medicationSelected.emit(null);
          this.options = [];
        }
      });
  }

  clearForm(): void {
    this.hasValueSelected = false;
    this.searchParamControl.reset();
  }

  ngOnDestroy(): void {
    this.searchParamChanged?.unsubscribe();
  }

  getDisplayText(id: string) {
    var medication = this.options.find((option) => option.id === id);
    return medication ? medication.name : '';
  }

  selectOption() {
    this.hasValueSelected = true;
    this.medicationSelected.emit(
      this.options.find(({ id }) => id == this.searchParamControl.value)
    );
  }
}
