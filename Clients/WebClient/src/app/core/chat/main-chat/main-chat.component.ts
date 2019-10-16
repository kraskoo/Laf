import { Component, OnDestroy, ViewChild, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { MatSidenav } from '@angular/material/sidenav';

import { User } from '../../models/user.model';
import { UserFriends } from '../../models/user-friends.model';
import { AccountService } from '../../services/account.service';

import { FriendsListComponent } from '../friends-list/friends-list.component';

import { config } from '../../services/configuration.service';
import { SideNavService } from '../../services/side-nav.service';

@Component({
  selector: 'app-main-chat',
  templateUrl: './main-chat.component.html',
  styleUrls: ['./main-chat.component.css']
})
export class MainChatComponent implements OnDestroy {
  private friends$: Observable<UserFriends> = this.accountService.friends(true);
  friends: UserFriends;
  selectedUser?: User;
  @ViewChild(MatSidenav, { static: false })
  set sideNav(value: MatSidenav) {
    this.sideNavService.sideNav = value;
  }

  constructor(
    private accountService: AccountService,
    private sideNavService: SideNavService) {
    config.inChatPage = true;
    this.friends$.subscribe(data => this.friends = data);
  }

  ngOnDestroy(): void {
    config.inChatPage = false;
  }

  clickMenu() {
    this.sideNavService.clickMenu();
  }

  selectUser(fl: FriendsListComponent) {
    this.selectedUser = fl.selectedUser;
  }

  get isInChatRoom(): boolean {
    return config.inChatPage;
  }
}
