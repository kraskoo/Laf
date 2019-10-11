import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AccountGuard } from '../guards/account.guard';
import { MainChatComponent } from './main-chat/main-chat.component';
import UserFriendsResolver from '../resolvers/user-friends.resolver';

const routes: Routes = [
  { path: '', component: MainChatComponent, canActivate: [AccountGuard], resolve: { friends: UserFriendsResolver } }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ChatRoutingModule { }
