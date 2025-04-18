import type {
  IHttpMessageResponseUtil,
  IHttpPipeline
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
import type { PlayerResponse } from '@/clients/responses/playerResponse.ts';

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

  async getPlayers(): Promise<PlayerResponse[]> {
    const message = new HttpMessageBuilder()
      .withMethod('GET')
      .withUrl('/api/players')
      .build();

    await this.httpPipeline.send(message);
    return this.httpMessageResponseUtil.parseJsonResponse<PlayerResponse[]>(message);
  }

  async seedDatabase(): Promise<Response> {
    const message = new HttpMessageBuilder()
      .withMethod('POST')
      .withUrl('/api/dev/seed')
      .build();

    await this.httpPipeline.send(message);
    return this.httpMessageResponseUtil.parseJsonResponse<Response>(message);
  }

    async dropDatabase(): Promise<Response> {
    const message = new HttpMessageBuilder()
      .withMethod('POST')
      .withUrl('/api/dev/drop')
      .build();

    await this.httpPipeline.send(message);
    return this.httpMessageResponseUtil.parseJsonResponse<Response>(message);
  }
}

class AuthenticationHttpPipelinePolicy extends HttpPipelinePolicy {
  constructor(private readonly authService: AuthenticationService) {
    super()
  }

  async process(message: HttpMessage, pipeline: HttpPipelinePolicy[]): Promise<void> {
    await this.processNext(message, pipeline);

    if (message.response?.status === 401) {
      await this.processNext(message, pipeline);
    }
  }
}
