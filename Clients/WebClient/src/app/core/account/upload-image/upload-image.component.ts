import { Component, ViewChild } from '@angular/core';
import { NgForm, FormGroup, FormBuilder } from '@angular/forms';

import { RouterService } from '../../services/router.service';
import { AccountService } from '../../services/account.service';
import { UserService } from '../../services/user.service';

import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-upload-image',
  templateUrl: './upload-image.component.html',
  styleUrls: ['./upload-image.component.css']
})
export class UploadImageComponent {
  @ViewChild('f', { static: false }) form: NgForm;
  isLoading = false;
  fb: FormBuilder;
  formGroup: FormGroup;
  formData: FormData;

  constructor(
    private routerService: RouterService,
    private accountService: AccountService,
    private userService: UserService) {
    this.fb = new FormBuilder();
    this.formGroup = this.fb.group({
      image: ['']
    });
  }

  onChange(event) {
    if (event.target.files.length > 0) {
      document.getElementById('file-text').textContent = event.target.files[0].name;
      this.formGroup.get('image').setValue(event.target.files[0]);
    }
  }

  onSubmit(): void {
    if (this.form.valid) {
      this.isLoading = true;
      this.formData = new FormData();
      this.formData.append('file', this.formGroup.get('image').value);
      this.accountService.uploadImage(this.formData).subscribe((data) => {
        const user = this.userService.user;
        // tslint:disable-next-line: no-string-literal
        user.avatarPath = data['path'];
        this.userService.removeCurrentUser();
        this.userService.assignToCurrentUser(user);
        this.routerService.navigate(['/']);
        this.isLoading = false;
      });
    } else {
      console.log('invalid');
    }
  }

  get searchIcon(): string {
    return `${environment.url}/search-image.png`;
  }

  sendToServer() {
    window.open(`${environment.url}/health`);
  }
}
