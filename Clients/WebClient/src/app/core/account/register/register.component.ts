import { Component, ViewChild } from '@angular/core';
import { NgForm, FormControl, Validators } from '@angular/forms';
import { AccountService } from '../../services/account.service';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  firstName = new FormControl('', [Validators.required, Validators.pattern(/[A-Z][a-z]{2,34}/g)]);
  lastName = new FormControl('', [Validators.required, Validators.pattern(/[A-Z][a-z]{2,34}/g)]);
  email = new FormControl('', [Validators.required, Validators.email]);
  password = new FormControl('', [Validators.required, Validators.pattern(/[a-zA-Z0-9]{6,30}/g)]);
  passwordConfirmation = new FormControl('', [Validators.required, Validators.pattern(/[a-zA-Z0-9]{6,30}/g)]);
  isLoading = false;

  constructor(
    private userService: UserService,
    private accountService: AccountService,
    private router: Router) { }

  onSubmit(form: NgForm): void {
    if (form.valid) {
      this.isLoading = true;
      const firstName = form.value.firstName;
      const lastName = form.value.lastName;
      const email = form.value.email;
      const password = form.value.password;
      const passwordConfirmation = form.value.passwordConfirmation;
      this.accountService.register(firstName, lastName, email, password, passwordConfirmation).subscribe(user => {
        console.log(user);
        user.expires = new Date(Date.now() + user.expiresIn);
        this.userService.addUser(user);
        this.router.navigate(['/']);
        this.isLoading = false;
      });
    } else {
      console.log('invalid');
    }
  }
}
