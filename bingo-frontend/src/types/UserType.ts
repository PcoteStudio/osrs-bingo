export interface User {
  isAuthenticated: boolean,
  authToken: string | null,
  refreshToken: string | null,
  name?: string,
  email?: string,
  role?: Role,
}

export enum Role {
  Admin,
  User
}
