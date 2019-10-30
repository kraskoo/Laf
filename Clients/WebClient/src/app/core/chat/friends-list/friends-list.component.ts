import { Component, Input } from '@angular/core';

import { ConfigService, ConfigStorage } from '../../services/config.service';

import { User } from '../../models/user.model';

import { environment } from '../../../../environments/environment';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'chat-friends-list',
  templateUrl: './friends-list.component.html',
  styleUrls: ['./friends-list.component.css']
})
export class FriendsListComponent {
  private configStorage: ConfigStorage = new ConfigStorage();
  @Input() title: string;
  @Input() list: User[];

  constructor(public configService: ConfigService) { }

  clickMenu() {
    const shouldCloseMenu = this.configStorage.getValue('sAfterSelectUser', true) === 'true';
    if (shouldCloseMenu) {
      this.configService.clickMenu();
    }
  }

  selectUser(user: User) {
    this.configService.selectedUser = user;
    this.clickMenu();
  }

  avatarPath(user: User): string {
    return `${environment.url}${user.avatarPath}`;
  }
}
