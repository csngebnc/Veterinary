import { Component, Input, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { UserData } from 'src/app/services/token.service';

@Component({
  selector: 'app-home-header',
  templateUrl: './home-header.component.html',
  styleUrls: ['./home-header.component.scss'],
})
export class HomeHeaderComponent implements OnInit {
  @Input() userInfo: UserData;

  notesFormControl: FormControl;

  constructor() {}

  ngOnInit(): void {
    this.notesFormControl = new FormControl(
      localStorage.getItem(`note-${this.userInfo.id}`)
    );
  }

  openEdit() {
    // TODO
  }

  deletePhoto() {
    // TODO
  }

  saveNotes() {
    if (this.notesFormControl.value.trim().length === 0) {
      localStorage.removeItem('note-' + this.userInfo.id);
    } else {
      localStorage.setItem(
        'note-' + this.userInfo.id,
        this.notesFormControl.value
      );
    }
  }
}
