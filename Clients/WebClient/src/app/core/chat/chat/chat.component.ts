import { Component, OnDestroy, ElementRef, ViewChild } from '@angular/core';

import { AccountService } from '../../services/account.service';
import { UserService } from '../../services/user.service';
import { ChatService } from '../../services/chat.service';
import { MessageService } from '../../services/message.service';
import { ConfigService } from '../../services/config.service';

import { MessagesByFriend } from '../../models/message.model';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnDestroy {
  @ViewChild('container', { static: false }) container: ElementRef;
  isLoading = true;
  currentMessage = '';

  constructor(
    private configService: ConfigService,
    private accountService: AccountService,
    private userService: UserService,
    private chatService: ChatService,
    private messageService: MessageService) {
      this.chatService.startConnection().then(() => { this.isLoading = false; }).catch(console.error);
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
      this.messageService.create(this.configService.selectedUser.id, currentDate, message).subscribe(() => { });
    } else if (this.configService.selectedUser.id === id) {
      p.classList.add('yours-msg');
      p.textContent = `[${dt}] ${this.configService.selectedUser.firstName} ${this.configService.selectedUser.lastName}: ${message}`;
    } else {
      const thisUser = this.userService.get(id);
      const didUserExists = thisUser !== null;
      if (!didUserExists) {
        this.accountService.getById(id).subscribe(data => {
          this.userService.add(data);
          p.textContent = `[${dt}] ${data.firstName} ${data.lastName}: ${message}`;
        });
      } else {
        p.classList.add('yours-msg');
        p.textContent = `[${dt}] ${thisUser.firstName} ${thisUser.lastName}: ${message}`;
      }
    }

    this.scrollToBottom(p);
  }

  scrollToBottom(p: HTMLParagraphElement) {
    const container = this.container.nativeElement;
    const parent = container.parentElement;
    container.insertBefore(p, container.children[container.children.length]);
    const length = container.children.length - 1;
    parent.scrollTop = container.scrollHeight - container.clientHeight + container.children[length].clientHeight;
  }

  appendRecentMessages(messages: MessagesByFriend[]) {
    for (const msg of messages) {
      const currentDate = new Date(msg.message.creationDate);
      const dt = `${currentDate.toLocaleDateString()} ${currentDate.toLocaleTimeString()}`;
      const p = document.createElement('p');
      p.classList.add('cht-msg');
      p.classList.add(this.userService.user.id === msg.message.user.id ? 'mine-msg' : 'yours-msg');
      p.textContent = `[${dt}] ${msg.message.user.firstName} ${msg.message.user.lastName}: ${msg.message.text}`;
      this.scrollToBottom(p);
    }
  }

  removeAllMessages() {
    const chatMessages = document.getElementsByClassName('cht-msg');
    for (const message of Array.from(chatMessages)) {
      message.parentElement.removeChild(message);
    }
  }

  updateMessages() {
    if (!this.configService.selectedUser) {
      return;
    }

    this.messageService.messages(this.configService.selectedUser.id).subscribe(m => {
      this.messageService.addAll(m);
      const messages = this.messageService.getAll(this.configService.selectedUser.id);
      this.removeAllMessages();
      this.appendRecentMessages(messages);
    });
  }

  sendMessage(text: string) {
    this.chatService.messageFrom(this.userService.user.id, text);
  }

  proccessMessage(ev: KeyboardEvent) {
    // tslint:disable-next-line: no-string-literal
    const input = (ev.target as HTMLInputElement);
    this.currentMessage = input.value;
    if (ev.key === 'Enter') {
      this.sendMessage(this.currentMessage);
      // tslint:disable-next-line: no-string-literal
      this.currentMessage = input.value = '';
    }
  }
}
