import { Injectable } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { CookieService } from './cookie.service';

@Injectable({ providedIn: 'root' })
export class RouterService {
  constructor(private router: Router, public cookieService: CookieService) { }

  handShakeAndBackTo(url: string) {
    // tslint:disable-next-line: only-arrow-functions
    this.router.routeReuseStrategy.shouldReuseRoute = function() { return false; };
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.router.navigated = false;
      }
    });
    if (this.cookieService.empty()) {
      this.cookieService.handShake().subscribe(() => {
        this.cookieService.setAll();
        this.router.navigate([url]);
      });
    }
  }
}
