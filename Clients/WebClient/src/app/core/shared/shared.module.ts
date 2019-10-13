import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  MatCommonModule,
  MatMenuModule,
  MatGridListModule,
  MatListModule,
  MatToolbarModule,
  MatButtonModule,
  MatIconModule,
  MatTooltipModule,
  MatSidenavModule
} from '@angular/material';

import { AppRoutingModule } from '../../app-routing.module';

import { HomeComponent } from './home/home.component';
import { NavbarComponent } from './navbar/navbar.component';
import { FontComponent } from './font/font.component';
import { FooterComponent } from './footer/footer.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';
import { BadRequestComponent } from './bad-request/bad-request.component';
import { ThemeComponent } from './theme/theme.component';

@NgModule({
  declarations: [
    NavbarComponent,
    FooterComponent,
    HomeComponent,
    NotFoundComponent,
    UnauthorizedComponent,
    BadRequestComponent,
    FontComponent,
    ThemeComponent
  ],
  imports: [
    CommonModule,
    AppRoutingModule,
    MatCommonModule,
    MatMenuModule,
    MatGridListModule,
    MatListModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatTooltipModule,
    MatSidenavModule
  ],
  exports: [
    NavbarComponent,
    FooterComponent
  ]
})
export class SharedModule { }
