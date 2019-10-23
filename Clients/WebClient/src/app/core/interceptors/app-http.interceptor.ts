import { HttpInterceptor, HttpRequest, HttpHandler, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { throwError, EMPTY } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { UserService } from '../services/user.service';
import { RouterService } from '../services/router.service';

import { environment } from '../../../environments/environment';

const { url } = environment;

@Injectable()
export class AppHttpInterceptor implements HttpInterceptor {
  constructor(
    private userService: UserService,
    private routerService: RouterService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    const wholeUrl = `${url}/${req.url}`;
    const hasLoggedinUser = this.userService.hasLoggedinUser();
    let httpHeaders: HttpHeaders = req.headers;
    const clonedRequest = req.clone({
      url: wholeUrl,
      headers: httpHeaders,
      withCredentials: true
    });
    if (!(req.url.endsWith('/login') || req.url.endsWith('/register'))) {
      if (hasLoggedinUser) {
        const expiresIn = this.userService.user.expiresIn;
        const sessionExpired = new Date(Date.now()) > new Date(Date.now() + expiresIn);
        if (sessionExpired) {
          this.userService.removeCurrentUser();
          this.routerService.navigate(['/account/login']);
          return EMPTY;
        }
      }
    }

    if (req.url.endsWith('/invitationscount') && !hasLoggedinUser) {
      return EMPTY;
    }

    if (hasLoggedinUser) {
      httpHeaders = httpHeaders.set('Authorization', `Bearer ${this.userService.user.token}`);
    }

    return next.handle(
      clonedRequest.clone({
          ...clonedRequest,
          headers: httpHeaders }))
        .pipe(this.catchError(this.catchErrorFunc));
  }

  catchError(func) {
    return catchError(func.bind(this));
  }

  catchErrorFunc(err: HttpErrorResponse) {
    if (err.status !== 200) {
      this.routerService.navigate(['/bad-request', err.message]);
    }

    return throwError(err);
  }
}
