import { Component, Inject, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';

import { MatBottomSheetRef, MatBottomSheet, MAT_BOTTOM_SHEET_DATA } from '@angular/material';

import { AccountService } from '../../services/account.service';
import { User } from '../../models/user.model';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'chat-base-content',
  templateUrl: './base-content.component.html',
  styleUrls: ['./base-content.component.css']
})
export class BaseContentComponent {
  @ViewChild(NgForm, { static: false }) form: NgForm;

  constructor(
    private bottomSheet: MatBottomSheet,
    private accountService: AccountService) { }

  onSubmit() {
    if (this.form.valid) {
      this.openBottomSheet(this.form.value.search);
    }
  }

  openBottomSheet(text: string): void {
    this.accountService.users(text).subscribe((users: User[]) => {
      this.bottomSheet.open(UserResultComponent, { data: users });
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
      this.router.navigate(['/chat']);
    });
  }
}
