import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { User } from '../../models/user.model';

import { UserService } from '../../services/user.service';
import { config } from '../../services/configuration.service';
import { Observable } from 'rxjs';
import { AccountService } from '../../services/account.service';
import { SideNavService } from '../../services/side-nav.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  invitationsCount$: Observable<number>;
  constructor(
    private accountService: AccountService,
    private userService: UserService,
    private sideNavService: SideNavService,
    private router: Router) { }

  ngOnInit(): void {
    this.invitationsCount$ = this.accountService.invitationsCount();
  }

  clickMenu() {
    this.sideNavService.clickMenu();
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
