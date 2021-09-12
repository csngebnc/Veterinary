import {
  UserService,
  VeterinaryUserDto,
} from './../../../services/generated-api-code';
import {
  Component,
  Input,
  OnDestroy,
  OnInit,
  Output,
  EventEmitter,
} from '@angular/core';
import { FormControl } from '@angular/forms';
import { Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'app-user-picker',
  templateUrl: './user-picker.component.html',
  styleUrls: ['./user-picker.component.scss'],
})
export class UserPickerComponent implements OnInit, OnDestroy {
  @Input() title: string = 'Válassz egy felhasználót';
  @Input() userIdsToRemove: string[];

  @Input() preSelectedUserId: string;

  @Output() userIdSelected = new EventEmitter<string>();

  searchParamControl = new FormControl();
  options: VeterinaryUserDto[] = [];

  validationError: string = '';

  private searchParamChanged: Subscription;
  selectedUser: VeterinaryUserDto;

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    if (this.preSelectedUserId) {
      this.userService
        .getUser(this.preSelectedUserId)
        .subscribe((user: VeterinaryUserDto) => {
          this.selectedUser = user;
        });
    } else {
      this.searchParamChanged = this.searchParamControl.valueChanges
        .pipe(debounceTime(1200), distinctUntilChanged())
        .subscribe(() => {
          if (this.searchParamControl.value.length > 2) {
            this.userService
              .searchUsers(this.searchParamControl.value)
              .subscribe(
                (result: VeterinaryUserDto[]) => {
                  this.options = result;

                  if (this.userIdsToRemove) {
                    this.options = this.options.filter(
                      (opt) => this.userIdsToRemove.indexOf(opt.id) === -1
                    );
                  }

                  if (result.length === 0) {
                    this.userIdSelected.emit(this.searchParamControl.value);
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

  getDisplayText(id: string) {
    var user = this.options.find((option) => option.id === id);
    return user ? user.email : '';
  }

  selectOption(event) {
    this.selectedUser = this.options.find((u) => u.id === event.option.value);
    this.userIdSelected.emit(this.searchParamControl.value);
  }

  chooseOther() {
    this.userIdSelected.emit('');
    this.validationError = '';
    this.selectedUser = undefined;
  }
}
