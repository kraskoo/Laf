import { Component, OnDestroy, ElementRef, ViewChild } from '@angular/core';
import { ChatService } from '../../services/chat.service';
import { User } from '../../models/user.model';
import { FriendsListComponent } from '../friends-list/friends-list.component';
import { UserService } from '../../services/user.service';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'contacts',
  templateUrl: './contacts.component.html',
  styleUrls: ['./contacts.component.css']
})
export class ContactsComponent implements OnDestroy {
  @ViewChild('container', { static: false }) container: ElementRef;
  selectedUser: User;
  currentMessage = '';

  constructor(
    private userService: UserService,
    private chatService: ChatService) {
      this.chatService.startConnection();
      // tslint:disable-next-line: only-arrow-functions
      this.chatService.initRevieceMessage((id: string, message: string) => this.onReviece(id, message));
    }

  ngOnDestroy(): void {
    this.chatService.stopConnection(this.onReviece);
  }

  onReviece(id: string, message: string) {
    const currentDate = new Date(Date.now());
    const dt = `${currentDate.toLocaleDateString()} ${currentDate.toLocaleTimeString()}`;
    const p = document.createElement('p');
    p.classList.add('cht-msg');
    if (this.userService.user.id === id) {
      p.classList.add('mine-msg');
      p.textContent = `[${dt}] ${this.userService.user.firstName} ${this.userService.user.lastName}: ${message}`;
    } else {
      p.classList.add('yours-msg');
      p.textContent = `[${dt}] ${this.selectedUser.firstName} ${this.selectedUser.lastName}: ${message}`;
    }

    const container = this.container.nativeElement;
    const parent = container.parentElement;
    container.insertBefore(p, container.children[container.children.length]);
    const length = container.children.length - 1;
    parent.scrollTop = container.scrollHeight - container.clientHeight + container.children[length].clientHeight;
  }

  selectUser(friendsList: FriendsListComponent) {
    this.selectedUser = friendsList.selectedUser;
  }

  sendMessage(text: string) {
    this.chatService.messageTo(this.userService.user.id, text);
  }

  proccessMessage(ev: KeyboardEvent) {
    // tslint:disable-next-line: no-string-literal
    this.currentMessage = ev.target['value'];
    if (ev.key === 'Enter') {
      this.sendMessage(this.currentMessage);
      // tslint:disable-next-line: no-string-literal
      this.currentMessage = ev.target['value'] = '';
    }
  }
}
