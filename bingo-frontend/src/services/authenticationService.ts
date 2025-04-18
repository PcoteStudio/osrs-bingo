import type { InjectionKey } from 'vue'
import { AuthenticationClient } from '@/clients/authenticationClient.ts';

export class AuthenticationService {
  static injectionKey : InjectionKey<AuthenticationService> = Symbol("😎");
  constructor(private readonly authenticationClient: AuthenticationClient) {
  }

  async authenticate(username: string, password: string) {
    await this.authenticationClient.postAuthLogin(username, password);
    return true;
  }

  async disconnect() {
    await this.authenticationClient.postAuthRevoke();
    return true;
  }

  async createAccount(username: string, password: string, email: string) {
    return await this.authenticationClient.postAuthSignup(username, password, email);
  }
}
