import { Component, ViewChild, Inject } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatBottomSheet, MAT_BOTTOM_SHEET_DATA, MatBottomSheetRef } from '@angular/material';
import { Observable } from 'rxjs';

import { AccountService } from '../../services/account.service';
import { UserFriends } from '../../models/user-friends.model';
import { RouterService } from '../../services/router.service';
import { User } from '../../models/user.model';

@Component({
  selector: 'app-invitations',
  templateUrl: './invitations.component.html',
  styleUrls: ['./invitations.component.css']
})
export class InvitationsComponent {
  private friends$: Observable<UserFriends> = this.accountService.friends(true);
  friends: UserFriends;
  selectedUser?: User;
  isLoading = true;
  @ViewChild(NgForm, { static: false }) form: NgForm;

  constructor(
    private routerService: RouterService,
    private bottomSheet: MatBottomSheet,
    private accountService: AccountService) {
    this.routerService.getBackFrom(this);
    this.friends$.subscribe(data => {
      this.isLoading = false;
      this.friends = data;
    });
  }

  openBottomSheet(text: string): void {
    this.accountService.users(text).subscribe((users: UserFriends[]) => {
      this.bottomSheet.open(UserResultComponent, { data: users });
    });
  }

  onSubmit() {
    if (this.form.valid) {
      this.openBottomSheet(this.form.value.search);
    }
  }

  acceptFriend(id: string) {
    this.accountService.acceptFriendship(id).subscribe(() => {
      this.routerService.navigate(['/chat/invitations']);
    });
  }

  reject(id: string) {
    this.accountService.reject(id).subscribe(() => {
      this.routerService.navigate(['/chat/invitations']);
    });
  }

  dropUser(id: string) {
    this.accountService.dropUser(id).subscribe(() => {
      this.routerService.navigate(['/chat/invitations']);
    });
  }

  blockUser(id: string) {
    this.accountService.blockUser(id).subscribe(() => {
      this.routerService.navigate(['/chat/invitations']);
    });
  }

  unblockUser(id: string) {
    this.accountService.unblockUser(id).subscribe(() => {
      this.routerService.navigate(['/chat/invitations']);
    });
  }
}

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'user-result',
  templateUrl: './user-result.component.html'
})
export class UserResultComponent {
  displayedColumns: string[] = ['firstName', 'lastName', 'email', 'action'];

  constructor(
    @Inject(MAT_BOTTOM_SHEET_DATA) public data: User[],
    private bottomSheetRef: MatBottomSheetRef<UserResultComponent>,
    private routerService: RouterService,
    private accountService: AccountService) { }

  closeSearchResults() {
    this.bottomSheetRef.dismiss();
  }

  openLink(event: MouseEvent): void {
    this.bottomSheetRef.dismiss();
    event.preventDefault();
  }

  invite(id: string) {
    this.accountService.addFriend(id).subscribe(() => {
      this.bottomSheetRef.dismiss();
      this.routerService.navigate(['/chat/invitations']);
    });
  }
}
