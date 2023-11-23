import {
  useQuery,
  useMutation,
  useQueryClient,
  QueryClient,
  QueryClientProvider,
  QueryKey,
} from '@tanstack/react-query'

export function login(username: string, password: string) {
  const queryKey: QueryKey = ['http://localhost:8000/login', {
    username: username,
    password: password
  }]

  const a = useQuery({
    queryKey: queryKey
  })


  return useQuery({
    queryKey: queryKey,
    select: transformLogin
  }
  );
}

function transformLogin(response) {
  console.log('response', response);

}