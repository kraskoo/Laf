import { Directive, ElementRef, Renderer2, AfterViewInit, Input } from '@angular/core';
import { ConfigService } from '../services/config.service';

import { functions } from '../utils/elements.util';

const { getStylePropertyValue, increaseStylePropertyValue, decreaseStylePropertyValue, getScrollbarWidth } = functions;

export interface ResizableContainer {
  x: number;
  y: number;
  newX: number;
  newY: number;
  mouseInside: boolean;
  canResize: boolean;
  mouseDown: boolean;
}

@Directive({
  // tslint:disable-next-line: directive-selector
  selector: '[resizable]'
})
export class ResizableDirective implements AfterViewInit {
  @Input() minResizableMargin: number;
  @Input() minResizableWidth: number;
  @Input() htmlSelector: string;

  private resizer: ResizableContainer = {
    x: 0,
    y: 0,
    newX: 0,
    newY: 0,
    mouseInside: false,
    canResize: false,
    mouseDown: false
  };
  private body: HTMLElement;
  private element: HTMLElement;
  private resizableElement: HTMLElement;

  constructor(private el: ElementRef<HTMLElement>, private renderer: Renderer2, private configService: ConfigService) { }

  ngAfterViewInit(): void {
    this.body = document.body;
    this.resizableElement = document.querySelector(this.htmlSelector).parentElement;
    this.element = this.el.nativeElement;
    this.renderer.listen(this.element, 'mouseenter', () => this.mouseEnter());
    this.renderer.listen(this.element, 'mouseleave', () => this.mouseLeave());
    this.renderer.listen(this.element, 'mousemove', (ev) => this.mouseMove(ev));
    this.renderer.listen(this.element, 'mousedown', (ev) => this.mouseDown(ev));
    this.renderer.listen(this.element, 'mouseup', (ev) => this.mouseUp(ev));
  }

  mouseEnter() {
    this.resizer.mouseInside = true;
    this.resizableElement.style.cursor = 'e-resize';
  }

  mouseLeave() {
    this.resizer.mouseInside = this.resizer.mouseDown = this.resizer.canResize = false;
    this.resizableElement.style.cursor = 'default';
  }

  private get isOpenFriendList(): boolean {
    return this.configService.sideNav.opened;
  }

  private resetValues() {
    this.resizer.canResize = this.resizer.mouseInside = this.resizer.mouseDown = false;
    this.resizer.x = this.resizer.newX = this.resizer.y = this.resizer.newY = 0;
  }

  private didReachMinMargin(elementMarginLeft: number): boolean {
    if (elementMarginLeft <= this.minResizableMargin) {
      this.resetValues();
      return true;
    }

    return false;
  }

  private didReachMinWidth(elementWidth: number): boolean {
    if (elementWidth <= this.minResizableWidth) {
      this.resetValues();
      return true;
    }

    return false;
  }

  private increaseMargin(newSize: number, elementWidth: number): boolean {
    if (this.didReachMinWidth(elementWidth)) {
      return true;
    }

    increaseStylePropertyValue(this.element, 'margin-left', newSize);
    increaseStylePropertyValue(this.element, 'margin-right', newSize);
    return false;
  }

  private decreaseMargin(newSize: number, elementMarginLeft: number): boolean {
    if (this.didReachMinMargin(elementMarginLeft)) {
      return true;
    }

    decreaseStylePropertyValue(this.element, 'margin-left', newSize);
    decreaseStylePropertyValue(this.element, 'margin-right', newSize);
    return false;
  }

  mouseMove(ev: MouseEvent) {
    const bodyHalfWidth = this.body.offsetWidth / 2;
    const elementWidth = Number(getStylePropertyValue(this.element, 'width').match(/\d+/g)[0]);
    const elementMarginLeft = Number(
      getStylePropertyValue(this.element, 'margin-left').match(/\d+/g)[0]);
    if (this.resizer.mouseDown) {
      this.resizer.newX = ev.x;
      if (ev.x < bodyHalfWidth) {
        if (this.resizer.x > this.resizer.newX) {
          if (this.decreaseMargin(this.resizer.x - this.resizer.newX, elementMarginLeft)) {
            return;
          }
        } else {
          if (this.increaseMargin(this.resizer.newX - this.resizer.x, elementWidth)) {
            return;
          }
        }
      } else {
        if (this.resizer.x > this.resizer.newX) {
          if (this.increaseMargin(this.resizer.x - this.resizer.newX, elementWidth)) {
            return;
          }
        } else {
          if (this.decreaseMargin(this.resizer.newX - this.resizer.x, elementMarginLeft)) {
            return;
          }
        }
      }

      this.resizer.x = this.resizer.newX;
      return;
    }

    const friendList = document.getElementById('friend-list');
    const flWidth = this.isOpenFriendList && friendList ? friendList.offsetWidth : 0;
    this.resizer.canResize = (ev.clientX <= elementMarginLeft + 5 + flWidth) ||
      (this.body.offsetWidth - ev.clientX - (elementMarginLeft + 5) - getScrollbarWidth() <= 5);
    this.resizableElement.style.cursor = this.resizer.canResize ? 'e-resize' : 'default';
  }

  mouseDown(ev: MouseEvent) {
    if ((!(this.resizer.mouseInside && this.resizer.canResize)) || ev.buttons !== 1) {
      return;
    }

    ev.preventDefault();
    this.resizer.x = ev.x;
    this.resizer.mouseDown = true;
  }

  mouseUp(ev: MouseEvent) {
    this.resetValues();
    ev.preventDefault();
  }
}
