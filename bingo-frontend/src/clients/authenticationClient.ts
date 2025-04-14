import type { IHttpMessageResponseUtil, IHttpPipeline } from '@mediaclip/http-pipeline';
import {
  HttpMessageBuilder,
  HttpMessageResponseUtil,
  HttpPipelineFactory,
  HttpPipelineOptions
} from '@mediaclip/http-pipeline';
import type { InjectionKey } from 'vue';
import type { LoginResponse } from '@/clients/responses/loginResponse.ts';

export class AuthenticationClient {
  static injectionKey : InjectionKey<AuthenticationClient>  = Symbol("🗝️");
  constructor(
    private readonly httpPipeline: IHttpPipeline,
    private readonly httpMessageResponseUtil: IHttpMessageResponseUtil
  ) {}

  static create() {
    const options = new HttpPipelineOptions();

    const pipeline = new HttpPipelineFactory().build(options);
    return new AuthenticationClient(pipeline, new HttpMessageResponseUtil());
  }

  async postAuthLogin(username: string, password: string) {
    const message = new HttpMessageBuilder()
      .withMethod('POST')
      .withUrl('/api/auth/login')
      .withCors({ includeCredentials: true })
      .withJsonBody({
        username: username,
        password: password
      })
      .build();

    await this.httpPipeline.send(message);
  }

  async postAuthSignup(username: string, password: string, email: string): Promise<LoginResponse> {
    const message = new HttpMessageBuilder()
      .withMethod('POST')
      .withUrl('/api/auth/signup')
      .withCors({ includeCredentials: true })
      .withJsonBody({
        username: username,
        password: password,
        email: email
      })
      .build();

    await this.httpPipeline.send(message);
    return this.httpMessageResponseUtil.parseJsonResponse<LoginResponse>(message);
  }



  async postAuthRevoke(token: string) {
    const message = new HttpMessageBuilder()
      .withMethod('POST')
      .withUrl('/api/auth/revoke')
      .withHeader('Authorization', 'Bearer ' + token)
      .build();

    await this.httpPipeline.send(message);
  }
}
