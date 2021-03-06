import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { UserService } from '../services/user.service';

@Injectable({ providedIn: 'root' })
export class AnonymousGuard implements CanActivate {
  constructor(
    private userService: UserService,
    private router: Router
  ) { }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot) {
      if (!this.userService.hasLoggedinUser()) {
        return true;
      }

      this.router.navigate(['/unauthorized']);
      return false;
  }
}
