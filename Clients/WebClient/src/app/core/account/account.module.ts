import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { AccountRoutingModule } from './account-routing.module';

import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { FormsModule } from '@angular/forms';
import { LoadingModule } from '../loading/loading.module';
import { UploadImageComponent } from './upload-image/upload-image.component';

@NgModule({
  declarations: [
    RegisterComponent,
    LoginComponent,
    UploadImageComponent
  ],
  imports: [
    CommonModule,
    MatInputModule,
    MatFormFieldModule,
    MatButtonModule,
    FormsModule,
    AccountRoutingModule,
    LoadingModule
  ]
})
export class AccountModule { }
