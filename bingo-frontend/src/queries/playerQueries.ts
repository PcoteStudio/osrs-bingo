import { defineQuery, useQuery } from '@pinia/colada';
import { inject } from 'vue';
import { HttpClient } from '@/clients/httpClient.ts';

export const useGetAllPlayers = defineQuery(() => {
  const httpClient = inject(HttpClient.injectionKey)!;
  const {
    data,
    ...rest
  } = useQuery({
    key: () => ['player', 'all'],
    query: () => httpClient.getPlayers()
  });
  return {
    players: data,
    ...rest,
  };
})
