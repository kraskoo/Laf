import { Injectable } from '@angular/core';
import { MatSidenav } from '@angular/material';
import { User } from '../models/user.model';

let sideNavigation: MatSidenav = null;
let selectedUser: User = null;

@Injectable({ providedIn: 'root' })
export class ConfigService {
  get sideNav(): MatSidenav {
    return sideNavigation;
  }

  set sideNav(value: MatSidenav) {
    sideNavigation = value;
  }

  get selectedUser() {
    return selectedUser;
  }

  set selectedUser(value: User) {
    selectedUser = value;
  }

  clickMenu() {
    sideNavigation.toggle();
  }
}
