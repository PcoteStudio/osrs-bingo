import { defineQuery, useQuery } from '@pinia/colada';
import { inject } from 'vue';
import { HttpClient } from '@/clients/httpClient.ts';

export const useCurrentUser = defineQuery(() => {
  const httpClient = inject(HttpClient.injectionKey)!;
  const {
    data,
    ...rest
  } = useQuery({
    key: () => ['user', 'me'],
    query: () => httpClient.getUsersMe()
  });
  return {
    user: data,
    ...rest,
  };
})
