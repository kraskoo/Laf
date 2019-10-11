export interface User {
  token: string;
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  roles: string[];
  expiresIn: number;
  expires: Date;
}
