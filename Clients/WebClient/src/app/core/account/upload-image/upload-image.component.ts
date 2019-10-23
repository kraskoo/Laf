import { Component, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';

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

  constructor(
    private routerService: RouterService,
    private accountService: AccountService) { }

  onSubmit(): void {
    if (this.form.valid) {
      this.isLoading = true;
      const file = this.form.value.file;
      console.log(file);
      this.accountService.uploadImage(file).subscribe(() => {
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
