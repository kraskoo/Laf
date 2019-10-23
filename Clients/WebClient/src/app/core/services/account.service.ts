import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AccountOwner, User } from '../models/user.model';
import { UserFriends } from '../models/user-friends.model';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AccountService {
  constructor(private http: HttpClient) { }

  login(email: string, password: string) {
    return this.http.post<AccountOwner>('account/login',
      {
        email,
        password
      }, {
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json'
      }
    });
  }

  register(firstName: string, lastName: string, email: string, password: string, passwordConfirmation: string) {
    return this.http.post<AccountOwner>(
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

  friends(friends: boolean = false, search: string = null): Observable<UserFriends> {
    let url = 'account/friends';
    if (friends) {
      url = `${url}?friends=true`;
    }

    if (search) {
      url = friends ? `${url}&search=${search}` : `${url}?search=${search}`;
    }

    return this.http.get<UserFriends>(url);
  }

  users(search: string = null): Observable<UserFriends[]> {
    let url = 'account/friends';
    if (search) {
      url = `${url}?search=${search}`;
    }

    return this.http.get<UserFriends[]>(url, {
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json'
      }
    });
  }

  getById(id: string): Observable<User> {
    const url = `account/byId/${id}`;
    return this.http.get<User>(url);
  }

  addFriend(friendId: string) {
    const url = 'account/addfriend';
    const body = { friendId };
    return this.http.post(url, body);
  }

  acceptFriendship(friendId: string) {
    const url = 'account/confirmfriendship';
    const body = { friendId };
    return this.http.post(url, body);
  }

  invitationsCount(): Observable<number> {
    const url = 'account/invitationscount';
    return this.http.get<number>(url);
  }

  reject(friendId: string) {
    const url = 'account/reject';
    const body = { friendId };
    return this.http.post(url, body);
  }

  dropUser(friendId: string) {
    const url = 'account/dropfriendship';
    const body = { friendId };
    return this.http.post(url, body);
  }

  blockUser(friendId: string) {
    const url = 'account/blockuser';
    const body = { friendId };
    return this.http.post(url, body);
  }

  unblockUser(friendId: string) {
    const url = 'account/unblockuser';
    const body = { friendId };
    return this.http.post(url, body);
  }

  uploadImage(form) {
    const url = 'account/upload';
    return this.http.post(url, form);
  }
}
