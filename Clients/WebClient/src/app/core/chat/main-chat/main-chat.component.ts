import { Component, OnDestroy, ViewChild, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MatSidenav } from '@angular/material/sidenav';

import { ChatService } from '../../services/chat.service';

import { config } from '../../services/configuration.service';
import { UserFriends } from '../../models/user-friends.model';
export interface Tile {
  color: string;
  cols: number;
  rows: number;
  text: string;
}

@Component({
  selector: 'app-main-chat',
  templateUrl: './main-chat.component.html',
  styleUrls: ['./main-chat.component.css']
})
export class MainChatComponent implements OnInit, OnDestroy {
  friends: UserFriends;
  tiles: Tile[] = [
    { text: 'Search', cols: 3, rows: 1, color: 'lightblue' },
    { text: 'Chat', cols: 3, rows: 11, color: 'lightgreen' },
    { text: 'Message', cols: 3, rows: 1, color: 'lightpink' }
  ];

  constructor(
    private route: ActivatedRoute,
    private chatService: ChatService) {
      config.inChatPage = true;
  }

  ngOnInit(): void {
    // tslint:disable-next-line: no-string-literal
    this.friends = this.route.snapshot.data['friends'];
  }

  ngOnDestroy(): void {
    config.inChatPage = false;
  }

  @ViewChild(MatSidenav, { static: false })
  set sideNav(value: MatSidenav) {
    this.chatService.sideNav = value;
  }

  get isInChatRoom(): boolean {
    return config.inChatPage;
  }
}
