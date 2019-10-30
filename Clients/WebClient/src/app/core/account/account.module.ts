import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  MatFormFieldModule,
  MatButtonModule,
  MatInputModule,
  MatOptionModule,
  MatSelectModule,
  MatSlideToggleModule
} from '@angular/material';

import { AccountRoutingModule } from './account-routing.module';

import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { FormsModule } from '@angular/forms';
import { LoadingModule } from '../loading/loading.module';
import { UploadImageComponent } from './upload-image/upload-image.component';
import { SettingsComponent } from './settings/settings.component';

@NgModule({
  declarations: [
    RegisterComponent,
    LoginComponent,
    UploadImageComponent,
    SettingsComponent
  ],
  imports: [
    CommonModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatOptionModule,
    MatSelectModule,
    MatSlideToggleModule,
    FormsModule,
    AccountRoutingModule,
    LoadingModule
  ]
})
export class AccountModule { }
