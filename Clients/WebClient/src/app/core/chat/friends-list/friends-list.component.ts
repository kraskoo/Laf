import { Component, Input } from '@angular/core';

import { SideNavService } from '../../services/side-nav.service';
import { MessageService } from '../../services/message.service';

import { User } from '../../models/user.model';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'chat-friends-list',
  templateUrl: './friends-list.component.html',
  styleUrls: ['./friends-list.component.css']
})
export class FriendsListComponent {
  selectedUser?: User;

  @Input() title: string;
  @Input() list: User[];

  constructor(
    private sideNavService: SideNavService,
    private messageService: MessageService) { }

  clickMenu() {
    this.sideNavService.clickMenu();
  }

  selectUser(user: User) {
    this.selectedUser = user;
    if (!this.messageService.containsMessages(this.selectedUser.id)) {
      this.messageService.messages(this.selectedUser.id).subscribe(messages => {
        this.messageService.addAll(messages);
      });
    }
  }
}
