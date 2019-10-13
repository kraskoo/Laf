import { HttpInterceptor, HttpRequest, HttpHandler, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { throwError, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { UserService } from '../services/user.service';

const apiUrl = environment.apiURL;

@Injectable()
export class AppHttpInterceptor implements HttpInterceptor {
  constructor(
    private userService: UserService,
    private router: Router) { }

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    let httpHeaders: HttpHeaders;
    if (!(req.url.endsWith('/login') || req.url.endsWith('/register'))) {
      const expiresIn = this.userService.user.expiresIn;
      const sessionExpired = new Date(Date.now()) > new Date(Date.now() + expiresIn);
      if (sessionExpired) {
        this.userService.removeUser();
        this.router.navigate([ '/account/login' ]);
        return of(null as any);
      }
    }

    if (this.userService.user && this.userService.user.token !== '') {
      httpHeaders = req.headers.append('Authorization', `Bearer ${this.userService.user.token}`);
    }

    return next.handle(req.clone({
      url: `${apiUrl}/${req.url}`,
      headers: httpHeaders
    })).pipe(catchError((err: HttpErrorResponse) => {
      if (err.status !== 200) {
        this.router.navigate([ '/bad-request', err.message ]);
      }

      return throwError(err);
    }));
  }
}
