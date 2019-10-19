import { Injectable } from '@angular/core';
import { Router, NavigationEnd, NavigationExtras } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class RouterService {
  constructor(private router: Router) { }

  getBackFrom(where): RouterService {
    this.getBackFrom.bind(where);
    // tslint:disable-next-line: only-arrow-functions
    this.router.routeReuseStrategy.shouldReuseRoute = function() { return false; };
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.router.navigated = false;
      }
    });
    return this;
  }

  navigate(commands: any[], extras?: NavigationExtras): void | Promise<boolean> {
    if (extras) {
      return this.router.navigate(commands, extras);
    }

    this.router.navigate(commands);
  }
}
