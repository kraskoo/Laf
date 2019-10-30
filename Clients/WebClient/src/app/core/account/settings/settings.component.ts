import { Component, OnInit, ViewChild } from '@angular/core';
import { ConfigStorage } from '../../services/config.service';
import { MatSlideToggleChange, MatSlideToggle } from '@angular/material';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {
  private configStorage: ConfigStorage = new ConfigStorage();
  sideNavModes: string[] = [ 'over', 'push', 'side' ];
  sOpenAtStart: boolean;
  sAfterSelectUser: boolean;
  sAlwaysOpen: boolean;
  sideNavMode: string;
  @ViewChild('sOpenAtStartChecked', { static: false }) sOpenAtStartChecked: MatSlideToggle;
  @ViewChild('sAfterSelectUserChecked', { static: false }) sAfterSelectUserChecked: MatSlideToggle;

  ngOnInit(): void {
    this.sOpenAtStart = Boolean(this.configStorage.getValue('sOpenAtStart', true) === 'true');
    this.sAfterSelectUser = Boolean(this.configStorage.getValue('sAfterSelectUser', true) === 'true');
    this.sAlwaysOpen = Boolean(this.configStorage.getValue('sAlwaysOpen', true) === 'true');
    this.sideNavMode = this.configStorage.getValue('sMode', true);
  }

  private trueOrFalseToString(value: boolean): string {
    return value ? 'true' : 'false';
  }

  openAtStartOnChange(e: MatSlideToggleChange) {
    if (this.configStorage.getValue('sAlwaysOpen', true) === 'false') {
      this.configStorage.setOrAddValue('sOpenAtStart', this.trueOrFalseToString(e.checked), true);
    }
  }

  afterSelectUserOnChange(e: MatSlideToggleChange) {
    if (this.configStorage.getValue('sAlwaysOpen', true) === 'false') {
      this.configStorage.setOrAddValue('sAfterSelectUser', this.trueOrFalseToString(e.checked), true);
    }
  }

  alwaysOpenOnChange(e: MatSlideToggleChange) {
    if (e.checked) {
      this.sOpenAtStartChecked.checked = !e.checked;
      this.sAfterSelectUserChecked.checked = !e.checked;
    }

    this.sOpenAtStartChecked.disabled = e.checked;
    this.sAfterSelectUserChecked.disabled = e.checked;
    this.configStorage.setOrAddValue('sOpenAtStart', this.trueOrFalseToString(false), true);
    this.configStorage.setOrAddValue('sAfterSelectUser', this.trueOrFalseToString(false), true);
    this.configStorage.setOrAddValue('sAlwaysOpen', this.trueOrFalseToString(e.checked), true);
  }

  selectSidenavModeOnChange(value: string) {
    this.configStorage.setOrAddValue('sMode', value, true);
  }
}
