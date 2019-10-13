import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AccountGuard } from '../guards/account.guard';
import { MainChatComponent } from './main-chat/main-chat.component';
import { InvitationsComponent } from './invitations/invitations.component';

const routes: Routes = [
  { path: '', component: MainChatComponent, canActivate: [AccountGuard] },
  { path: 'invitations', component: InvitationsComponent, canActivate: [AccountGuard] }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ChatRoutingModule { }
