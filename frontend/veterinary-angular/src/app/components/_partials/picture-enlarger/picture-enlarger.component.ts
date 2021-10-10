import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-picture-enlarger',
  templateUrl: './picture-enlarger.component.html',
  styleUrls: ['./picture-enlarger.component.scss'],
})
export class PictureEnlargerComponent implements OnInit {
  @Input() data;

  constructor() {}

  ngOnInit(): void {}
}
