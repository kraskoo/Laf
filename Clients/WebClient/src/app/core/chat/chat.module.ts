import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {
  MatButtonModule,
  MatBottomSheetModule,
  MatFormFieldModule,
  MatSidenavModule,
  MatTabsModule,
  MatToolbarModule,
  MatGridListModule,
  MatMenuModule,
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
import { ChatComponent } from './chat/chat.component';
import { ResizableDirective } from '../directives/resizable.directive';
import { LoadingModule } from '../loading/loading.module';
import { EmojiComponent } from './emoji/emoji.component';
import { EmojiListComponent } from './emoji-list/emoji-list.component';

@NgModule({
  declarations: [
    MainChatComponent,
    FriendsListComponent,
    UserResultComponent,
    InvitationsComponent,
    ResizableDirective,
    ChatComponent,
    EmojiComponent,
    EmojiListComponent
  ],
  imports: [
    CdkTableModule,
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatBottomSheetModule,
    MatFormFieldModule,
    MatSidenavModule,
    MatTabsModule,
    MatToolbarModule,
    MatGridListModule,
    MatMenuModule,
    MatListModule,
    MatIconModule,
    MatInputModule,
    MatTableModule,
    MatTooltipModule,
    ChatRoutingModule,
    LoadingModule
  ],
  entryComponents: [
    UserResultComponent
  ],
  providers: [
    { provide: ErrorStateMatcher, useClass: ShowOnDirtyErrorStateMatcher }
  ]
})
export class ChatModule { }
