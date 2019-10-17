import { HttpInterceptor, HttpRequest, HttpHandler, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { throwError, EMPTY, never, Operator, OperatorFunction } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { UserService } from '../services/user.service';

const { url } = environment;

@Injectable()
export class AppHttpInterceptor implements HttpInterceptor {
  constructor(
    private userService: UserService,
    private router: Router) { }

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    if (req.url.includes('message/handshake')) {
      return next.handle(req.clone({
        url: `${url}/${req.url}`,
        withCredentials: true
      })).pipe(this.catchError(this.catchErrorFunc));
    }

    console.log(req.headers);
    console.log(req.headers.get('X-XSRF-TOKEN'));
    const hasLoggedinUser = this.userService.hasLoggedinUser();
    let httpHeaders: HttpHeaders;
    if (!(req.url.endsWith('/login') || req.url.endsWith('/register'))) {
      if (hasLoggedinUser) {
        const expiresIn = this.userService.user.expiresIn;
        const sessionExpired = new Date(Date.now()) > new Date(Date.now() + expiresIn);
        if (sessionExpired) {
          this.userService.removeCurrentUser();
          this.router.navigate(['/account/login']);
          return EMPTY;
        }
      }
    }

    if (req.url.endsWith('/invitationscount') && !hasLoggedinUser) {
      return EMPTY;
    }

    if (hasLoggedinUser) {
      httpHeaders = req.headers.append('Authorization', `Bearer ${this.userService.user.token}`);
    }

    req = req.clone({
      url: `${url}/${req.url}`,
      headers: httpHeaders,
      withCredentials: true
    });
    return next.handle(req).pipe(this.catchError(this.catchErrorFunc));
  }

  catchError(func) {
    return catchError(func);
  }

  catchErrorFunc(err: HttpErrorResponse) {
    if (err.status !== 200) {
      this.router.navigate(['/bad-request', err.message]);
    }

    return throwError(err);
  }
}
