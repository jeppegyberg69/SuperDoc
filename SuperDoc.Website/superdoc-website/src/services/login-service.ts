import { QueryKey, useQuery } from '@tanstack/react-query'

export function login(email: string, password: string) {
  const queryKey: QueryKey = ['https://localhost:44304/api/User/Login', {
    email: email,
    password: password
  }]

  return useQuery<any, any, any>({
    queryKey: queryKey,
    select: transformLogin
  });
}

function transformLogin(response) {
  console.log('response', response);


  return;
}