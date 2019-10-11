import { Component } from '@angular/core';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'spinner',
  templateUrl: './spinner.component.html',
  styleUrls: ['./spinner.component.css']
})
export class SpinnerComponent {
  color = 'basic';
  mode = 'indeterminate';
  value = 100;
}
