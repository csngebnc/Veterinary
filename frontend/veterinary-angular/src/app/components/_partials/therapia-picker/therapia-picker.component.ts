import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { TherapiaForSelectDto, TherapiaService } from 'src/app/services/generated-api-code';

@Component({
  selector: 'app-therapia-picker',
  templateUrl: './therapia-picker.component.html',
  styleUrls: ['./therapia-picker.component.scss'],
})
export class TherapiaPickerComponent implements OnInit, OnDestroy {
  @Input() title: string = 'Kezelés';
  @Input() placeholderText: string = '';

  @Output() therapiaSelected = new EventEmitter<TherapiaForSelectDto>();

  searchParamControl = new FormControl();
  options: TherapiaForSelectDto[] = [];
  hasValueSelected: boolean = false;

  private searchParamChanged: Subscription;

  constructor(private therapiaService: TherapiaService) {}

  ngOnInit(): void {
    this.searchParamChanged = this.searchParamControl.valueChanges
      .pipe(debounceTime(500), distinctUntilChanged())
      .subscribe(() => {
        if (this.searchParamControl.value?.length > 0) {
          if (this.hasValueSelected) {
            return;
          }
          this.therapiaService
            .searchTherapia(this.searchParamControl.value)
            .subscribe((result: TherapiaForSelectDto[]) => {
              this.options = result;

              if (result.length === 0) {
                this.therapiaSelected.emit(null);
              }
            });
        } else if (this.searchParamControl.value?.length === 0) {
          this.hasValueSelected = false;
          this.therapiaSelected.emit(null);
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
    var therapia = this.options.find((option) => option.id === id);
    return therapia ? therapia.name : '';
  }

  selectOption() {
    this.hasValueSelected = true;
    this.therapiaSelected.emit(this.options.find(({ id }) => id == this.searchParamControl.value));
  }
}
