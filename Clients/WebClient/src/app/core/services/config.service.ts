import { Injectable } from '@angular/core';
import { MatSidenav } from '@angular/material';
import { User } from '../models/user.model';

const storage: { [ key: string ]: string } = {};
let sideNavigation: MatSidenav = null;
let selectedUser: User = null;
let inChatPage = false;

@Injectable({ providedIn: 'root' })
export class ConfigService {
  get sideNav(): MatSidenav {
    return sideNavigation;
  }

  set sideNav(value: MatSidenav) {
    sideNavigation = value;
  }

  get inChatRoom() {
    return inChatPage;
  }

  set inChatRoom(value: boolean) {
    inChatPage = value;
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

export class ConfigStorage {
  static defaultValueFor(key: string, value: string, inApp: boolean = false): ConfigStorage {
    if (inApp) {
      localStorage.setItem(key, value);
    } else {
      storage[key] = value;
    }

    return new ConfigStorage();
  }

  getValue(key: string, fromApp: boolean = false): string {
    switch (key) {
      case 'sOpenAtStart':
      case 'sAfterSelectUser':
      case 'sAlwaysOpen': {
        if (fromApp && !localStorage.getItem(key)) {
          return this.setOrAddValue(key, 'false', true).getValue(key, true);
        } else if (!Object.keys(storage).includes(key)) {
          return this.setOrAddValue(key, 'false').getValue(key);
        }

        break;
      }
      case 'sMode': {
        if (fromApp && !localStorage.getItem(key)) {
          return this.setOrAddValue(key, 'side', true).getValue(key, true);
        } else if (!Object.keys(storage).includes(key)) {
          return this.setOrAddValue(key, 'side').getValue(key);
        }

        break;
      }
    }

    return fromApp ? localStorage.getItem(key) : Object.keys(storage).includes(key) ? storage[key] : null;
  }

  setOrAddValue(key: string, value: string, inApp: boolean = false): ConfigStorage {
    if (inApp) {
      localStorage.setItem(key, value);
    } else {
      storage[key] = value;
    }

    return this;
  }
}
