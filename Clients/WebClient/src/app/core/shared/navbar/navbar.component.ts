import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { User } from '../../models/user.model';

import { UserService } from '../../services/user.service';
import { ChatService } from '../../services/chat.service';
import { config } from '../../services/configuration.service';

import { fontVariables } from '../../models/font.model';
const { fonts, url: fontUrl, css: fontCss, withoutCss } = fontVariables;

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  private theme = '';
  fontKey = 'Default';
  fontKeys: string[] = fonts.map(f => f.key);

  constructor(
    private userService: UserService,
    private chatService: ChatService,
    private router: Router) { }

  ngOnInit(): void {
    const currentTheme = localStorage.getItem('theme');
    if (!currentTheme) {
      localStorage.setItem('theme', 'indigo-pink');
      this.theme = 'indigo-pink';
    } else {
      this.theme = currentTheme;
    }

    this.changeTheme(this.theme);
  }

  changeFont(key: string): void {
    this.fontKey = key;
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

  clickSideNav() {
    this.chatService.clickMenu();
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

  get isInChatRoom(): boolean {
    return config.inChatPage;
  }

  get hasLoggedinUser(): boolean {
    return this.userService.hasLoggedinUser();
  }

  isAdmin(): boolean {
    return this.userService.hasRole('Administrator');
  }

  get user(): User {
    return this.userService.user;
  }

  logout(): void {
    this.userService.removeUser();
    this.router.navigate(['/']);
  }
}
