import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { User } from '../../models/user.model';

import { UserService } from '../../services/user.service';
import { ChatService } from '../../services/chat.service';
import { config } from '../../services/configuration.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  constructor(
    private userService: UserService,
    private chatService: ChatService,
    private router: Router) { }

  clickSideNav() {
    this.chatService.clickMenu();
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
