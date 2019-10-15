import { Component, OnInit } from '@angular/core';
import { ChatService } from '../../services/chat.service';
import { User } from '../../models/user.model';
import { FriendsListComponent } from '../friends-list/friends-list.component';
import { UserService } from '../../services/user.service';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'chat-base-content',
  templateUrl: './base-content.component.html',
  styleUrls: ['./base-content.component.css']
})
export class BaseContentComponent implements OnInit {
  selectedUser: User;
  currentMessage = '';

  constructor(
    private userService: UserService,
    private chatService: ChatService) { }

  ngOnInit(): void {
    this.chatService.startConnection();
    // tslint:disable-next-line: only-arrow-functions
    this.chatService.initRevieceMessage((id: string, message: string) => {
      const currentDate = new Date(Date.now());
      const dt = `${currentDate.toLocaleDateString()} ${currentDate.toLocaleTimeString()}`;
      const chatContent = document.getElementById('chat-content');
      const p = document.createElement('p');
      p.classList.add('cht-msg');
      if (this.userService.user.id === id) {
        p.classList.add('mine-msg');
        p.textContent = `[${dt}] ${this.userService.user.firstName} ${this.userService.user.lastName}: ${message}`;
      } else {
        p.classList.add('yours-msg');
        p.textContent = `[${dt}] ${this.selectedUser.firstName} ${this.selectedUser.lastName}: ${message}`;
      }

      chatContent.insertBefore(p, chatContent.children.item[chatContent.children.item.length - 1]);
    });
  }

  selectUser(friendsList: FriendsListComponent) {
    this.selectedUser = friendsList.selectedUser;
  }

  sendMessage(text: string) {
    this.chatService.messageTo(this.userService.user.id, text);
  }

  proccessMessage(ev: KeyboardEvent, input: HTMLInputElement) {
    this.currentMessage = input.value;
    if (ev.key === 'Enter') {
      this.sendMessage(this.currentMessage);
      this.currentMessage = input.value = '';
      const chatContainer = document.querySelector('#chat-container');
      chatContainer.scrollTop = chatContainer.scrollHeight - chatContainer.clientHeight;
    }
  }
}
