import { User } from './user.model';

export interface UserFriends {
  friends: User[];
  invitedFriends: User[];
  invitations: User[];
  awaitableFriends: User[];
}
