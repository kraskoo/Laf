import { Component, ViewChild, Inject, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatBottomSheet, MAT_BOTTOM_SHEET_DATA, MatBottomSheetRef } from '@angular/material';
import { AccountService } from '../../services/account.service';
import { User } from '../../models/user.model';
import { Router, NavigationEnd } from '@angular/router';
import { Observable } from 'rxjs';
import { UserFriends } from '../../models/user-friends.model';

@Component({
  selector: 'app-invitations',
  templateUrl: './invitations.component.html',
  styleUrls: ['./invitations.component.css']
})
export class InvitationsComponent implements OnInit {
  friends$: Observable<UserFriends>;
  selectedUser?: User;
  @ViewChild(NgForm, { static: false }) form: NgForm;

  constructor(
    private router: Router,
    private bottomSheet: MatBottomSheet,
    private accountService: AccountService) {
    // tslint:disable-next-line: only-arrow-functions
    this.router.routeReuseStrategy.shouldReuseRoute = function () { return false; };
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        // Trick the Router into believing it's last link wasn't previously loaded
        this.router.navigated = false;
      }
    });
  }

  ngOnInit(): void {
    this.friends$ = this.accountService.friends(true);
    // tslint:disable-next-line: no-string-literal
    // this.friends = this.route.snapshot.data['friends'];
  }

  openBottomSheet(text: string): void {
    this.accountService.users(text).subscribe((users: User[]) => {
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
      this.router.navigate(['/chat/invitations']);
    });
  }

  dropUser(id: string) {
    this.accountService.dropUser(id).subscribe(() => {
      this.router.navigate(['/chat/invitations']);
    });
  }

  blockUser(id: string) {
    this.accountService.blockUser(id).subscribe(() => {
      this.router.navigate(['/chat/invitations']);
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
    private router: Router,
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
      this.router.navigate(['/chat/invitations']);
    });
  }
}
