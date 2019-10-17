import { Component } from '@angular/core';
import { RouterService } from '../../services/router.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  constructor(private routerService: RouterService) {
    this.routerService.handShakeAndBackTo('/');
  }
}
