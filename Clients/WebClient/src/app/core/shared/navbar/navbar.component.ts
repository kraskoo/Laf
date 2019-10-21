import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { AccountService } from '../../services/account.service';
import { UserService } from '../../services/user.service';
import { RouterService } from '../../services/router.service';
import { ConfigService } from '../../services/config.service';

import { AccountOwner } from '../../models/user.model';
import { config } from '../../services/configuration.service';

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
    private sideNavService: ConfigService,
    private routerService: RouterService) { }

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

  get user(): AccountOwner {
    return this.userService.user;
  }

  logout(): void {
    this.routerService.navigate(['/']);
    this.userService.removeCurrentUser();
  }
}
