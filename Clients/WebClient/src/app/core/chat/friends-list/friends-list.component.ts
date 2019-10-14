import { Component, Input } from '@angular/core';
import { User } from '../../models/user.model';
import { ChatService } from '../../services/chat.service';

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

  constructor(private chatService: ChatService) { }

  clickMenu() {
    this.chatService.clickMenu();
  }

  selectUser(user: User) {
    this.selectedUser = user;
    this.clickMenu();
  }
}
