import type { InjectionKey } from 'vue'
import { AuthenticationClient } from '@/clients/authenticationClient.ts';

export class AuthenticationService {
  static injectionKey : InjectionKey<AuthenticationService> = Symbol("😎");

  private authToken: string | undefined = undefined;

  constructor(private readonly authenticationClient: AuthenticationClient) {
  }

  getToken(): string | undefined {
    return this.authToken;
  }

  async authenticate(username: string, password: string) {
    await this.authenticationClient.postAuthLogin(username, password);
    return true;
  }

  async disconnect() {
    const token = this.getToken();
    if (!token) {
      console.log('No token');
      return false;
    }

    await this.authenticationClient.postAuthRevoke(token);
    return true;
  }

  async createAccount(username: string, password: string, email: string) {
    return await this.authenticationClient.postAuthSignup(username, password, email);
  }
}
