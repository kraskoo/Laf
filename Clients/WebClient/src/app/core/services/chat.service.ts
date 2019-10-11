import { MatSidenav } from '@angular/material/sidenav';
import { Injectable } from '@angular/core';

let sideNavigation: MatSidenav = null;

@Injectable({ providedIn: 'root' })
export class ChatService {
  get sideNav(): MatSidenav {
    return sideNavigation;
  }

  set sideNav(value: MatSidenav) {
    sideNavigation = value;
  }

  clickMenu() {
    sideNavigation.toggle();
  }
}
