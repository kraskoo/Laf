import { User } from './user.model';

export interface UserFriends {
  friends: User[];
  blockedUsers: User[];
  invitedUsers: User[];
  invitations: User[];
}