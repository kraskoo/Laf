import { Component, Input } from '@angular/core';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'emoji-list',
  templateUrl: './emoji-list.component.html',
  styleUrls: ['./emoji-list.component.css']
})
export class EmojiListComponent {
  @Input() emojis: string[];
  @Input() cols: number;
  @Input() rowHeight: string;

  copyEmoji(event: MouseEvent): void {
    event.stopPropagation();
    // tslint:disable-next-line: no-string-literal
    const e = event.target['textContent'];
    const m = document.getElementById('message') as HTMLInputElement;
    m.value = `${m.value}${e}`;
  }
}
