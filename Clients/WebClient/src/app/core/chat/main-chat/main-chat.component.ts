import { Component, OnDestroy, ViewChild } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import { Observable } from 'rxjs';

import { AccountService } from '../../services/account.service';
import { SideNavService } from '../../services/side-nav.service';

import { FriendsListComponent } from '../friends-list/friends-list.component';

import { User } from '../../models/user.model';
import { UserFriends } from '../../models/user-friends.model';
import { config } from '../../services/configuration.service';
import { MessageService } from '../../services/message.service';

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
    private sideNavService: SideNavService,
    private messageService: MessageService) {
    config.inChatPage = true;
    this.friends$.subscribe(data => this.friends = data);
  }

  ngOnDestroy(): void {
    config.inChatPage = false;
  }

  clickMenu() {
    this.sideNavService.clickMenu();
  }

  get isInChatRoom(): boolean {
    return config.inChatPage;
  }
}
