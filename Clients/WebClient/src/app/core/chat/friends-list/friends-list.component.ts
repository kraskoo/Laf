import { Component, Input } from '@angular/core';

import { ConfigService } from '../../services/config.service';

import { User } from '../../models/user.model';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'chat-friends-list',
  templateUrl: './friends-list.component.html',
  styleUrls: ['./friends-list.component.css']
})
export class FriendsListComponent {
  @Input() title: string;
  @Input() list: User[];

  constructor(private configService: ConfigService) { }

  clickMenu() {
    this.configService.clickMenu();
  }

  selectUser(user: User) {
    this.configService.selectedUser = user;
  }
}
