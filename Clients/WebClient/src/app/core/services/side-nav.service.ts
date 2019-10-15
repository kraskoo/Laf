import { Injectable } from '@angular/core';
import { MatSidenav } from '@angular/material';

let sideNavigation: MatSidenav = null;

@Injectable({ providedIn: 'root' })
export class SideNavService {
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
