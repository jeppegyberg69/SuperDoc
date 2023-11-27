import { WebSessionContext, getWebSession } from '@/common/session-context/session-context';
import { Session, saveInStorage } from '@/models/session/session';
import { QueryKey, useQuery } from '@tanstack/react-query'
import { Context, useContext } from 'react';
import * as signals from 'signals';

export const sessionChangedSignal: signals.Signal<Session> = new signals.Signal();

export function useSession(): Session {
  return useContext<Session>(WebSessionContext as Context<Session>);
}

export function getSession() {
  return getWebSession();
}

function dispatchSessionChange(session: Session) {
  sessionChangedSignal.dispatch(session);
}


export function login(email: string, password: string): Promise<any> {
  const myHeaders = new Headers();
  myHeaders.append("Content-Type", "application/json");

  const raw = JSON.stringify({
    "email": email,
    "password": password
  });

  const requestOptions: RequestInit = {
    method: 'POST',
    headers: myHeaders,
    body: raw,
    redirect: 'follow'
  };

  return fetch("https://localhost:44304/api/User/Login", requestOptions)
    .then(async (response): Promise<WebserviceResponse> => {
      if (response.ok) {
        return {
          status: response.status,
          statusText: response.statusText,
          data: {
            ...await response.json() // this wont error because we made sure that the response is ok earlier, so response.json is always an actual json value.
          }
        }
      }
      // return Promise.reject()
    })
    .then(transformLogin)
    .then((response: any) => {
      saveInStorage(response);
      dispatchSessionChange(response);
      return response;
    })
    .catch((error) => {
    });
}

function transformLogin(response) {
  return response.data;
}



export type WebserviceResponse = {
  status: number,
  statusText: string,
  data?: {
    [key: string]: any
  }
}