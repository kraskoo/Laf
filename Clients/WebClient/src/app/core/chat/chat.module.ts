import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {
  MatButtonModule,
  MatBottomSheetModule,
  MatFormFieldModule,
  MatSidenavModule,
  MatToolbarModule,
  MatGridListModule,
  MatListModule,
  MatIconModule,
  MatInputModule,
  MatTableModule,
  MatTooltipModule,
  ErrorStateMatcher,
  ShowOnDirtyErrorStateMatcher
} from '@angular/material';
import { CdkTableModule } from '@angular/cdk/table';

import { MainChatComponent } from './main-chat/main-chat.component';

import { ChatRoutingModule } from './chat-routing.module';
import { FriendsListComponent } from './friends-list/friends-list.component';
import { InvitationsComponent, UserResultComponent } from './invitations/invitations.component';
import { ContactsComponent } from './contacts/contacts.component';

@NgModule({
  declarations: [
    MainChatComponent,
    FriendsListComponent,
    ContactsComponent,
    UserResultComponent,
    InvitationsComponent
  ],
  imports: [
    CdkTableModule,
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatBottomSheetModule,
    MatFormFieldModule,
    MatSidenavModule,
    MatToolbarModule,
    MatGridListModule,
    MatListModule,
    MatIconModule,
    MatInputModule,
    MatTableModule,
    MatTooltipModule,
    ChatRoutingModule
  ],
  entryComponents: [
    UserResultComponent
  ],
  providers: [
    { provide: ErrorStateMatcher, useClass: ShowOnDirtyErrorStateMatcher }
  ]
})
export class ChatModule { }
