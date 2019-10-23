import { Component, ViewChild } from '@angular/core';
import { NgForm, FormGroup, FormBuilder } from '@angular/forms';

import { RouterService } from '../../services/router.service';
import { AccountService } from '../../services/account.service';

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
    private accountService: AccountService) {
    this.fb = new FormBuilder();
    this.formGroup = this.fb.group({
      image: ['']
    });
  }

  onChange(event) {
    if (event.target.files.length > 0) {
      this.formGroup.get('image').setValue(event.target.files[0]);
    }
  }

  onSubmit(): void {
    if (this.form.valid) {
      this.isLoading = true;
      this.formData = new FormData();
      this.formData.append('file', this.formGroup.get('image').value);
      this.accountService.uploadImage(this.formData).subscribe(() => {
        this.routerService.navigate(['/']);
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
