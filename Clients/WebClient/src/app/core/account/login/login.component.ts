import { Component, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';

import { RouterService } from '../../services/router.service';
import { CookieService } from '../../services/cookie.service';
import { AccountService } from '../../services/account.service';
import { UserService } from '../../services/user.service';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  @ViewChild('f', { static: false }) form: NgForm;
  isLoading = false;

  constructor(
    private routerService: RouterService,
    private cookieService: CookieService,
    private userService: UserService,
    private accountService: AccountService) { }

  onSubmit(): void {
    if (this.form.valid) {
      this.isLoading = true;
      const email = this.form.value.email;
      const password = this.form.value.password;
      this.accountService.login(email, password).subscribe(user => {
        user.expires = new Date(user.expiresIn);
        this.userService.assignToCurrentUser(user);
        this.routerService.navigate(['/']);
        this.cookieService.setAll();
        this.isLoading = false;
      });
    } else {
      console.log('invalid');
    }
  }

  sendToServer() {
    window.open(`${environment.url}/health`);
  }
}
