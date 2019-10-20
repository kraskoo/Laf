import { User } from './user.model';

export interface Message {
  user: User;
  friend: User;
  creationDate: Date;
  text: string;
}

export interface MessagesByFriend {
  friendId: string;
  message: Message;
}
