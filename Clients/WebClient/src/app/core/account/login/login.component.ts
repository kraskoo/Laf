import { Component } from '@angular/core';
import { NgForm, FormControl, Validators } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { AccountService } from '../../services/account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  isLoading = false;
  email = new FormControl('', [Validators.required, Validators.email]);
  password = new FormControl('', [Validators.required, Validators.pattern(/[a-zA-Z0-9]{6,30}/g)]);

  constructor(
    private userService: UserService,
    private accountService: AccountService,
    private router: Router) { }

  onSubmit(form: NgForm): void {
    if (form.valid) {
      this.isLoading = true;
      const email = form.value.email;
      const password = form.value.password;
      this.accountService.login(email, password).subscribe(user => {
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
