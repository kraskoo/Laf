import { Component, Input } from '@angular/core';
import { User } from '../../models/user.model';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'chat-friends-list',
  templateUrl: './friends-list.component.html',
  styleUrls: ['./friends-list.component.css']
})
export class FriendsListComponent {
  @Input() title: string;
  @Input() list: User[];
}
