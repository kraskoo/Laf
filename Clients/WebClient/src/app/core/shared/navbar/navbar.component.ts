import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { AccountService } from '../../services/account.service';
import { UserService } from '../../services/user.service';
import { RouterService } from '../../services/router.service';
import { ConfigService } from '../../services/config.service';

import { AccountOwner } from '../../models/user.model';

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
    private configService: ConfigService,
    private routerService: RouterService) { }

  ngOnInit(): void {
    this.invitationsCount$ = this.accountService.invitationsCount();
  }

  clickMenu() {
    this.configService.clickMenu();
  }

  get isInChatRoom(): boolean {
    return this.configService.inChatRoom;
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
