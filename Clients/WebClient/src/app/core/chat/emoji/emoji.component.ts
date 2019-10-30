import { Component } from '@angular/core';
import { emojis } from '../../models/emoji.model';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'emoji',
  templateUrl: './emoji.component.html',
  styleUrls: ['./emoji.component.css']
})
export class EmojiComponent {
  emojiByKey(key: string) {
    return emojis[key];
  }
}
