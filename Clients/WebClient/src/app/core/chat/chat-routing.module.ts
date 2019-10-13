import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AccountGuard } from '../guards/account.guard';
import { MainChatComponent } from './main-chat/main-chat.component';

const routes: Routes = [
  { path: '', component: MainChatComponent, canActivate: [AccountGuard] }
];

@NgModule({
  imports: [RouterModule.forChild(routes) ],
  exports: [RouterModule]
})
export class ChatRoutingModule { }
