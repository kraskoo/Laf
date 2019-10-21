import { Component, OnDestroy, ViewChild } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import { Observable } from 'rxjs';

import { AccountService } from '../../services/account.service';
import { ConfigService } from '../../services/config.service';

import { User } from '../../models/user.model';
import { UserFriends } from '../../models/user-friends.model';
import { config } from '../../services/configuration.service';

@Component({
  selector: 'app-main-chat',
  templateUrl: './main-chat.component.html',
  styleUrls: ['./main-chat.component.css']
})
export class MainChatComponent implements OnDestroy {
  private friends$: Observable<UserFriends> = this.accountService.friends(true);
  friends: UserFriends;
  @ViewChild(MatSidenav, { static: false })
  set sideNav(value: MatSidenav) {
    this.configService.sideNav = value;
  }

  constructor(
    private accountService: AccountService,
    private configService: ConfigService) {
    config.inChatPage = true;
    this.friends$.subscribe(data => this.friends = data);
  }

  ngOnDestroy(): void {
    config.inChatPage = false;
  }

  clickMenu() {
    this.configService.clickMenu();
  }

  get isInChatRoom(): boolean {
    return config.inChatPage;
  }
}
