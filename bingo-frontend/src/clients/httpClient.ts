import type {
  IHttpMessageResponseUtil,
  IHttpPipeline,
  IHttpPipelineFactory
} from '@mediaclip/http-pipeline';
import {
  HttpMessage,
  HttpMessageBuilder,
  HttpMessageResponseUtil,
  HttpPipelineFactory,
  HttpPipelineOptions,
  HttpPipelinePolicy
} from '@mediaclip/http-pipeline';
import { AuthenticationService } from '../services/authenticationService.ts';
import type { InjectionKey } from 'vue';
import type { UserResponse } from '@/clients/responses/userResponse.ts';

export class HttpClient {
  static injectionKey : InjectionKey<HttpClient>  = Symbol("💩");
  constructor(
    private readonly httpPipeline: IHttpPipeline,
    private readonly httpMessageResponseUtil: IHttpMessageResponseUtil
  ) {}

  static create(authService: AuthenticationService) {
    const options = new HttpPipelineOptions();
    options.perRetryPolicies.push(new AuthenticationHttpPipelinePolicy(authService));

    const pipeline = new HttpPipelineFactory().build(options);
    return new HttpClient(pipeline, new HttpMessageResponseUtil());
  }

  async getUsersMe(): Promise<UserResponse> {
    const message = new HttpMessageBuilder()
      .withMethod('GET')
      .withUrl('/api/users/me')
      .build();

    await this.httpPipeline.send(message);
    return this.httpMessageResponseUtil.parseJsonResponse<UserResponse>(message);
  }
}

class AuthenticationHttpPipelinePolicy extends HttpPipelinePolicy {
  constructor(private readonly authService: AuthenticationService) {
    super()
  }

  async process(message: HttpMessage, pipeline: HttpPipelinePolicy[]): Promise<void> {
    message.request.headers['Authorization'] = 'Bearer ' + this.authService.getToken();
    await this.processNext(message, pipeline);

    if (message.response?.status === 401) {
      await this.authService.refresh();
      message.request.headers['Authorization'] = 'Bearer ' + this.authService.getToken();
      await this.processNext(message, pipeline);
    }
  }
}
