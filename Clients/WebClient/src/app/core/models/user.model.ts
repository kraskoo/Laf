export interface AccountOwner {
  token: string;
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  roles: string[];
  expiresIn: number;
  expires: Date;
}

export interface User {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  roles: string[];
}

export interface UserById {
  id: string;
  user: User;
}
