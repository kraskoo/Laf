import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/user.model';
import { UserFriends } from '../models/user-friends.model';

@Injectable({ providedIn: 'root' })
export class AccountService {
  constructor(
    private http: HttpClient) { }

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

    return this.http.get<UserFriends>(url);
  }

  users(search: string = null) {
    let url = 'account/friends';
    if (search) {
      url = `${url}?search=${search}`;
    }

    return this.http.get<User[]>(url);
  }

  addFriend(id: string) {
    const url = 'account/addfriend';
    const body = { id };
    return this.http.post(url, body);
  }

  acceptFriendship(id: string) {
    const url = 'account/confirmfriendship';
    const body = { id };
    return this.http.post(url, body);
  }

  invitationsCount() {
    const url = 'account/invitationscount';
    return this.http.get<number>(url);
  }

  dropUser(id: string) {
    const url = 'account/dropfriendship';
    const body = { id };
    return this.http.post(url, body);
  }

  blockUser(id: string) {
    const url = 'account/blockuser';
    const body = { id };
    return this.http.post(url, body);
  }
}
