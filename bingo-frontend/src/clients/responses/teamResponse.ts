import type { PlayerResponse } from '@/clients/responses/playerResponse.ts';
import type { EventResponse } from '@/clients/responses/eventResponse.ts';

export type TeamResponse = {
  id: number;
  avatar: string;
  name: string;
  event: EventResponse;
  players?: PlayerResponse[];
}
