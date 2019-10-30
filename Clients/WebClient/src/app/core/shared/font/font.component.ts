import { Component } from '@angular/core';
import { ConfigStorage } from '../../services/config.service';
import { fontVariables } from '../../models/font.model';

const { fonts, url: fontUrl, css: fontCss, withoutCss } = fontVariables;

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'font',
  templateUrl: './font.component.html',
  styleUrls: ['./font.component.css']
})
export class FontComponent {
  private conf = 'font';
  fontKey = 'Default';
  fontKeys: string[] = fonts.map(f => f.key);
  private configStorage: ConfigStorage = new ConfigStorage();

  constructor() {
    const value = this.configStorage.getValue(this.conf, true);
    if (value) {
      this.changeFont(value);
    } else {
      this.configStorage = ConfigStorage.defaultValueFor(this.conf, this.fontKey, true);
    }
  }

  changeFont(key: string): void {
    this.fontKey = this.configStorage.setOrAddValue(this.conf, key, true).getValue(this.conf, true);
    const font = fonts.filter(f => f.key === key)[0].font;
    const fontStyle = document.getElementById('font-style');
    const fontLink = document.getElementById('font-link');
    fontLink.setAttribute('href', `${fontUrl}${font.link}`);
    if (font.css) {
      fontStyle.textContent = fontCss.replace('#!fontname!#', font.css);
    } else {
      fontStyle.textContent = withoutCss;
    }
  }
}
