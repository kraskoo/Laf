import { Component, OnDestroy, ViewChild } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import { Observable } from 'rxjs';

import { AccountService } from '../../services/account.service';
import { ConfigService, ConfigStorage } from '../../services/config.service';

import { UserFriends } from '../../models/user-friends.model';

@Component({
  selector: 'app-main-chat',
  templateUrl: './main-chat.component.html',
  styleUrls: ['./main-chat.component.css']
})
export class MainChatComponent implements OnDestroy {
  private configStorage: ConfigStorage = new ConfigStorage();
  private friends$: Observable<UserFriends> = this.accountService.friends(true);
  friends: UserFriends;
  @ViewChild(MatSidenav, { static: false })
  set sideNav(value: MatSidenav) {
    this.configService.sideNav = value;
  }

  constructor(
    private accountService: AccountService,
    private configService: ConfigService) {
    this.configService.inChatRoom = true;
    this.friends$.subscribe(data => this.friends = data);
    this.configStorage.getValue('sMode', true);
  }

  ngOnDestroy(): void {
    this.configService.selectedUser = null;
    this.configService.inChatRoom = false;
  }

  get sideNavMode(): string {
    return this.configStorage.getValue('sMode', true);
  }

  get sideNavOpen(): boolean {
    return this.configStorage.getValue('sOpenAtStart', true) === 'true' ||
          this.configStorage.getValue('sAlwaysOpen', true) === 'true';
  }

  clickMenu() {
    this.configService.clickMenu();
  }

  get isInChatRoom(): boolean {
    return this.configService.inChatRoom;
  }
}
