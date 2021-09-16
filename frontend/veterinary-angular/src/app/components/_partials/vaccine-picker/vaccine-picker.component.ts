import { VaccinesService } from 'src/app/services/generated-api-code';
import { VaccineSearchResultDto, VaccineDto } from './../../../services/generated-api-code';
import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'app-vaccine-picker',
  templateUrl: './vaccine-picker.component.html',
  styleUrls: ['./vaccine-picker.component.scss'],
})
export class VaccinePickerComponent implements OnInit, OnDestroy {
  @Input() title: string = 'Válassz egy oltást';
  @Input() preSelectedVaccineId: string;
  @Output() vaccineSelected = new EventEmitter<string>();

  searchParamControl = new FormControl();
  options: VaccineSearchResultDto[] = [];

  validationError: string = '';

  private searchParamChanged: Subscription;
  selectedVaccine: VaccineSearchResultDto;

  constructor(private vaccineService: VaccinesService) {}

  ngOnInit(): void {
    if (this.preSelectedVaccineId) {
      this.vaccineService.getVaccine(this.preSelectedVaccineId).subscribe((vaccine: VaccineDto) => {
        this.selectedVaccine = vaccine;
      });
    } else {
      this.searchParamChanged = this.searchParamControl.valueChanges
        .pipe(debounceTime(1200), distinctUntilChanged())
        .subscribe(() => {
          if (this.searchParamControl.value.length > 2) {
            this.vaccineService.searchVaccines(this.searchParamControl.value).subscribe(
              (result: VaccineSearchResultDto[]) => {
                this.options = result;
                if (result.length === 0) {
                  this.vaccineSelected.emit(this.searchParamControl.value);
                }
              },
              (errors) => {
                const error = JSON.parse(errors['response']);
                this.validationError = error.errors.SearchParam[0];
              }
            );
          } else if (this.searchParamControl.value.length === 0) {
            this.validationError = '';
            this.options = [];
          }
        });
    }
  }

  ngOnDestroy(): void {
    this.searchParamChanged?.unsubscribe();
  }

  getDisplayText(id: string): string {
    var vaccine = this.options.find((option) => option.id === id);
    return vaccine ? vaccine.name : '';
  }

  selectOption(event): void {
    this.selectedVaccine = this.options.find((v) => v.id === event.option.value);
    this.vaccineSelected.emit(this.selectedVaccine.id);
  }

  chooseOther(): void {
    this.validationError = '';
    this.vaccineSelected.emit('');
    this.selectedVaccine = undefined;
  }
}
