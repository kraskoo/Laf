import { Component, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AccountService } from '../../services/account.service';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';
import { RouterService } from '../../services/router.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  @ViewChild('f', { static: false }) form: NgForm;
  isLoading = false;

  constructor(
    private userService: UserService,
    private accountService: AccountService,
    private router: Router,
    private routerService: RouterService) {
      this.routerService.handShakeAndBackTo('/');
    }

  onSubmit(): void {
    if (this.form.valid) {
      this.isLoading = true;
      const firstName = this.form.value.firstName;
      const lastName = this.form.value.lastName;
      const email = this.form.value.email;
      const password = this.form.value.password;
      const passwordConfirmation = this.form.value.passwordConfirmation;
      this.accountService.register(firstName, lastName, email, password, passwordConfirmation).subscribe(user => {
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
