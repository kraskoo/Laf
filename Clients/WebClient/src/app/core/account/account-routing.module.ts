import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { UploadImageComponent } from './upload-image/upload-image.component';

import { AnonymousGuard } from '../guards/anonymous.guard';
import { AccountGuard } from '../guards/account.guard';

const routes: Routes = [
  { path: 'register', component: RegisterComponent, canActivate: [AnonymousGuard] },
  { path: 'login', component: LoginComponent, canActivate: [AnonymousGuard] },
  { path: 'upload-image', component: UploadImageComponent, canActivate: [AccountGuard] }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AccountRoutingModule { }