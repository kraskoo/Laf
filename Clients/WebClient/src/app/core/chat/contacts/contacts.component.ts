import { Component, OnDestroy, ElementRef, ViewChild } from '@angular/core';
import { AccountService } from '../../services/account.service';
import { UserService } from '../../services/user.service';
import { ChatService } from '../../services/chat.service';
import { User } from '../../models/user.model';
import { FriendsListComponent } from '../friends-list/friends-list.component';
import { functions } from '../../utils/elements.util';

const { getStylePropertyValue, increaseStylePropertyValue, decreaseStylePropertyValue, getScrollbarWidth } = functions;

export interface ResizableContainer {
  x: number;
  y: number;
  newX: number;
  newY: number;
  mouseInside: boolean;
  canResize: boolean;
  mouseDown: boolean;
}

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
  resizable: ResizableContainer = {
    x: 0,
    y: 0,
    newX: 0,
    newY: 0,
    mouseInside: false,
    canResize: false,
    mouseDown: false
  };

  constructor(
    private accountService: AccountService,
    private userService: UserService,
    private chatService: ChatService) {
      this.chatService.startConnection();
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
    } else if (this.selectedUser.id === id) {
      p.classList.add('yours-msg');
      p.textContent = `[${dt}] ${this.selectedUser.firstName} ${this.selectedUser.lastName}: ${message}`;
    } else {
      const thisUser = this.userService.get(id);
      const didUserExists = thisUser !== null;
      if (!didUserExists) {
        this.accountService.getById(id).subscribe(data => {
          this.userService.add(data);
          p.textContent = `[${dt}] ${data.firstName} ${data.lastName}: ${message}`;
          console.log(p);
        });
      } else {
        p.classList.add('yours-msg');
        p.textContent = `[${dt}] ${thisUser.firstName} ${thisUser.lastName}: ${message}`;
      }
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
    this.chatService.messageFrom(this.userService.user.id, text);
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

  mouseEnter() {
    this.resizable.mouseInside = true;
    document.body.style.cursor = 'e-resize';
  }

  mouseLeave() {
    this.resizable.canResize = this.resizable.mouseInside = this.resizable.mouseDown = false;
    document.body.style.cursor = 'default';
  }

  mouseMove(ev: MouseEvent) {
    const body = document.body;
    const bodyHalfWidth = body.offsetWidth / 2;
    const elementWidth = Number(getStylePropertyValue(ev.target as HTMLElement, 'width').match(/\d+/g)[0]);
    const elementMarginLeft = Number(getStylePropertyValue(ev.target as HTMLElement, 'margin-left').match(/\d+/g)[0]);
    if (this.resizable.mouseDown) {
      this.resizable.newX = ev.x;
      if (ev.x < bodyHalfWidth) {
        if (this.resizable.x > this.resizable.newX) {
          if (elementMarginLeft <= 36) {
            this.resizable.canResize = this.resizable.mouseInside = this.resizable.mouseDown = false;
            return;
          }

          decreaseStylePropertyValue(ev.target as HTMLElement, 'margin-left', this.resizable.x - this.resizable.newX);
          decreaseStylePropertyValue(ev.target as HTMLElement, 'margin-right', this.resizable.x - this.resizable.newX);
        } else {
          if (elementWidth <= 201) {
            this.resizable.canResize = this.resizable.mouseInside = this.resizable.mouseDown = false;
            return;
          }

          increaseStylePropertyValue(ev.target as HTMLElement, 'margin-left', this.resizable.newX - this.resizable.x);
          increaseStylePropertyValue(ev.target as HTMLElement, 'margin-right', this.resizable.newX - this.resizable.x);
        }
      } else {
        if (this.resizable.x > this.resizable.newX) {
          if (elementWidth <= 201) {
            this.resizable.canResize = this.resizable.mouseInside = this.resizable.mouseDown = false;
            return;
          }

          increaseStylePropertyValue(ev.target as HTMLElement, 'margin-left', this.resizable.x - this.resizable.newX);
          increaseStylePropertyValue(ev.target as HTMLElement, 'margin-right', this.resizable.x - this.resizable.newX);
        } else {
          if (elementMarginLeft <= 36) {
            this.resizable.canResize = this.resizable.mouseInside = this.resizable.mouseDown = false;
            return;
          }

          decreaseStylePropertyValue(ev.target as HTMLElement, 'margin-left', this.resizable.newX - this.resizable.x);
          decreaseStylePropertyValue(ev.target as HTMLElement, 'margin-right', this.resizable.newX - this.resizable.x);
        }
      }

      this.resizable.x = this.resizable.newX;
      return;
    }

    this.resizable.canResize = (ev.clientX <= elementMarginLeft + 5) ||
        (body.offsetWidth - ev.clientX - (elementMarginLeft + 5) - getScrollbarWidth() <= 5);
    body.style.cursor = this.resizable.canResize ? 'e-resize' : 'default';
  }

  mousedown(ev: MouseEvent) {
    ev.preventDefault();
    if (!(this.resizable.mouseInside && this.resizable.canResize)) {
      return;
    }

    this.resizable.x = ev.x;
    this.resizable.mouseDown = true;
  }

  mouseup(ev: MouseEvent) {
    ev.preventDefault();
    this.resizable.canResize = this.resizable.mouseInside = this.resizable.mouseDown = false;
  }

  contextmenu(ev: MouseEvent) {
    ev.preventDefault();
  }
}
