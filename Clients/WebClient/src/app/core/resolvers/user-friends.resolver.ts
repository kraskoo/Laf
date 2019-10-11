import { AccountService } from '../services/account.service';
import { UserFriends } from '../models/user-friends.model';
import { Resolve, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export default class UserFriendsResolver implements Resolve<UserFriends> {
  constructor(private accountService: AccountService) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    return this.accountService.users(true);
  }
}
