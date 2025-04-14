import type { TeamResponse } from '@/clients/responses/teamResponse.ts';

export type PlayerResponse = {
  id?: number;
  name?: string;
  teams?: TeamResponse[];
}
