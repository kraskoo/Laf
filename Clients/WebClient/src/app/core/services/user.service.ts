import { Injectable } from '@angular/core';
import { CookieService } from './cookie.service';
import { AccountOwner, User, UserById } from '../models/user.model';

@Injectable({ providedIn: 'root' })
export class UserService {
  private usersById: UserById[] = [];

  constructor(private cookieService: CookieService) { }

  add(user: User): void {
    this.usersById[user.id] = user;
  }

  remove(id: string) {
    const filtredUsers = this.usersById.filter(x => x.id === id);
    if (filtredUsers.length > 0) {
      this.usersById = this.usersById.filter(x => x.id !== filtredUsers[0].id);
    }
  }

  get(id: string): User {
    const filtredUsers = this.usersById.filter(x => x.id === id);
    let user: User = null;
    if (filtredUsers.length > 0) {
      user = filtredUsers[0].user;
    }

    return user;
  }

  hasLoggedinUser(): boolean {
    return localStorage.getItem('user') !== null;
  }

  hasRole(role: string): boolean {
    return JSON.parse(localStorage.getItem('user')).roles.includes(role);
  }

  get user(): AccountOwner {
    return JSON.parse(localStorage.getItem('user')) as AccountOwner;
  }

  assignToCurrentUser(user: AccountOwner): void {
    localStorage.setItem('user', JSON.stringify(user));
  }

  removeCurrentUser(): void {
    localStorage.removeItem('user');
  }
}
