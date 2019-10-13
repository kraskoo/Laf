import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/user.model';
import { UserService } from './user.service';
import { UserFriends } from '../models/user-friends.model';

@Injectable({ providedIn: 'root' })
export class AccountService {
  constructor(
    private http: HttpClient,
    private userService: UserService) { }

  login(email: string, password: string) {
    return this.http.post<User>('account/login', { email, password });
  }

  register(firstName: string, lastName: string, email: string, password: string, passwordConfirmation: string) {
    return this.http.post<User>(
      'account/register', {
        firstName,
        lastName,
        email,
        password,
        passwordConfirmation
      }, {
        headers: {
          Accept: 'application/json',
          'Content-Type': 'application/json'
        }
      });
  }

  friends(friends: boolean = false, search: string = null) {
    let url = 'account/friends';
    if (friends) {
      url = `${url}?friends=true`;
    }

    if (search) {
      url = friends ? `${url}&search=${search}` : `${url}?search=${search}`;
    }

    const headers = { Authorization: `Bearer ${this.userService.user.token}` };
    return this.http.get<UserFriends>(url, { headers });
  }

  users(search: string = null) {
    let url = 'account/friends';
    if (search) {
      url = `${url}?search=${search}`;
    }

    const headers = { Authorization: `Bearer ${this.userService.user.token}` };
    return this.http.get<User[]>(url, { headers });
  }

  addFriend(id: string) {
    const url = 'account/addfriend';
    const body = { id };
    const headers = { Authorization: `Bearer ${this.userService.user.token}` };
    return this.http.post(url, body, { headers });
  }
}
