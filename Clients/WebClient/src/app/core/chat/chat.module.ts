import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  MatButtonModule,
  MatSidenavModule,
  MatToolbarModule,
  MatGridListModule,
  MatListModule,
  MatIconModule,
  MatTooltipModule
} from '@angular/material';

import { MainChatComponent } from './main-chat/main-chat.component';

import { ChatRoutingModule } from './chat-routing.module';
import { FriendsListComponent } from './friends-list/friends-list.component';
import { BaseContentComponent } from './base-content/base-content.component';

@NgModule({
  declarations: [
    MainChatComponent,
    FriendsListComponent,
    BaseContentComponent
  ],
  imports: [
    CommonModule,
    MatButtonModule,
    MatSidenavModule,
    MatToolbarModule,
    MatGridListModule,
    MatListModule,
    MatIconModule,
    MatTooltipModule,
    ChatRoutingModule
  ]
})
export class ChatModule { }
