import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { UserService } from './user.service';
import { Message, MessagesByFriend } from '../models/message.model';

const messages: MessagesByFriend[] = [];

@Injectable({ providedIn: 'root' })
export class MessageService {

  constructor(
    private http: HttpClient,
    private userService: UserService) { }

  messages(friendId: string): Observable<Message[]> {
    return this.http.post<Message[]>('message/messages', { friendId });
  }

  create(friendId: string, creationDate: Date, text: string) {
    return this.http.post('message/create', { friendId, creationDate, text });
  }

  edit(friendId: string, creationDate: Date, text: string) {
    return this.http.post('message/edit', { friendId, creationDate, text });
  }

  remove(friendId: string, creationDate: Date, text: string) {
    return this.http.post('message/edit', { friendId, creationDate, text });
  }

  containsMessages(friendId: string) {
    return messages.filter(m => m.friendId === friendId).length > 0;
  }

  getAll(friendId: string) {
    return messages.filter(
      m => (m.friendId === friendId && m.message.user.id === this.userService.user.id) ||
          (m.message.user.id === friendId && m.friendId === this.userService.user.id));
  }

  addAll(msgs: Message[]) {
    for (const message of msgs) {
      const filtred = messages.filter(
        m => m.message.creationDate === message.creationDate &&
        m.message.friend.id === message.friend.id &&
        m.message.text === message.text &&
        m.message.user.id === message.user.id);
      if (filtred.length === 0) {
        messages.push({ friendId: message.friend.id, message });
      }
    }
  }
}
