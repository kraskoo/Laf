import { Component } from '@angular/core';
import { ConfigStorage } from '../../services/config.service';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'theme',
  templateUrl: './theme.component.html',
  styleUrls: ['./theme.component.css']
})
export class ThemeComponent {
  private configStorage: ConfigStorage = new ConfigStorage();
  private conf = 'theme';
  private theme = '';

  constructor() {
    const currentTheme = this.configStorage.getValue(this.conf, true);
    if (!currentTheme) {
      this.theme = this.configStorage.setOrAddValue(this.conf, 'indigo-pink', true).getValue(this.conf, true);
    } else {
      this.theme = currentTheme;
    }

    this.changeTheme(this.theme);
  }

  changeTheme(theme: string): void {
    this.theme = this.configStorage.setOrAddValue(this.conf, theme, true).getValue(this.conf, true);
    const themeLink = document.getElementById('theme-link');
    themeLink.setAttribute('href', `/assets/themes/${theme}.css`);
  }

  isMyColor(color: string): boolean {
    return this.theme === color;
  }
}
