import type { TeamResponse } from '@/clients/responses/teamResponse.ts';
import type { UserResponse } from '@/clients/responses/userResponse.ts';

export type EventResponse = {
  id: number;
  name: string;
  startTime: string;
  endTime: string;
  teams: TeamResponse[];
  administrators: UserResponse;
}
