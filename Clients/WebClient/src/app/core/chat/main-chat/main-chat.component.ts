import { Component, OnDestroy, ViewChild, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { MatSidenav } from '@angular/material/sidenav';

import { User } from '../../models/user.model';
import { UserFriends } from '../../models/user-friends.model';
import { AccountService } from '../../services/account.service';
import { ChatService } from '../../services/chat.service';

import { FriendsListComponent } from '../friends-list/friends-list.component';

import { config } from '../../services/configuration.service';

@Component({
  selector: 'app-main-chat',
  templateUrl: './main-chat.component.html',
  styleUrls: ['./main-chat.component.css']
})
export class MainChatComponent implements OnInit, OnDestroy {
  friends$: Observable<UserFriends>;
  selectedUser?: User;
  @ViewChild(MatSidenav, { static: false })
  set sideNav(value: MatSidenav) {
    this.chatService.sideNav = value;
  }

  constructor(
    private accountService: AccountService,
    private chatService: ChatService) {
    config.inChatPage = true;
  }

  ngOnInit(): void {
    this.friends$ = this.accountService.friends(true);
    // tslint:disable-next-line: no-string-literal
    // this.friends = this.route.snapshot.data['friends'];
  }

  ngOnDestroy(): void {
    config.inChatPage = false;
  }

  clickClose() {
    this.chatService.clickMenu();
  }

  selectUser(fl: FriendsListComponent) {
    this.selectedUser = fl.selectedUser;
  }

  get isInChatRoom(): boolean {
    return config.inChatPage;
  }
}
