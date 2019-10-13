import { Component } from '@angular/core';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'theme',
  templateUrl: './theme.component.html',
  styleUrls: ['./theme.component.css']
})
export class ThemeComponent {
  private theme = '';

  constructor() {
    const currentTheme = localStorage.getItem('theme');
    if (!currentTheme) {
      localStorage.setItem('theme', 'indigo-pink');
      this.theme = 'indigo-pink';
    } else {
      this.theme = currentTheme;
    }

    this.changeTheme(this.theme);
  }

  changeTheme(theme: string): void {
    this.theme = theme;
    localStorage.setItem('theme', theme);
    const themeLink = document.getElementById('theme-link');
    themeLink.setAttribute('href', `/assets/themes/${theme}.css`);
  }

  isMyColor(color: string): boolean {
    return this.theme === color;
  }
}
