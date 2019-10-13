import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './core/shared/home/home.component';
import { NotFoundComponent } from './core/shared/not-found/not-found.component';
import { UnauthorizedComponent } from './core/shared/unauthorized/unauthorized.component';
import { BadRequestComponent } from './core/shared/bad-request/bad-request.component';

const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'not-found', component: NotFoundComponent },
  { path: 'unauthorized', component: UnauthorizedComponent },
  { path: 'bad-request/:message', component: BadRequestComponent },
  { path: 'account', loadChildren: () => import('./core/account/account.module').then(m => m.AccountModule).catch(console.error) },
  {
    path: 'chat',
    loadChildren: () => import('./core/chat/chat.module').then(m => m.ChatModule).catch(console.error),
    runGuardsAndResolvers: 'always'
  },
  { path: '**', redirectTo: 'not-found', pathMatch: 'full' }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      onSameUrlNavigation: 'reload'
    })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
