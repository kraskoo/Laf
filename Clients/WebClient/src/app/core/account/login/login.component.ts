import { Component, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { AccountService } from '../../services/account.service';
import { Router } from '@angular/router';
import { RouterService } from '../../services/router.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  @ViewChild('f', { static: false }) form: NgForm;
  isLoading = false;

  constructor(
    private userService: UserService,
    private accountService: AccountService,
    private router: Router,
    public routerService: RouterService) {
      this.routerService.handShakeAndBackTo('/');
    }

  onSubmit(): void {
    if (this.form.valid) {
      this.isLoading = true;
      const email = this.form.value.email;
      const password = this.form.value.password;
      this.accountService.login(email, password).subscribe(user => {
        user.expires = new Date(user.expiresIn);
        this.userService.assignToCurrentUser(user);
        this.router.navigate(['/']);
        this.isLoading = false;
      });
    } else {
      console.log('invalid');
    }
  }
}
