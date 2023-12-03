'use client';

import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { useEffect, useState } from 'react'
import { WebSessionContext, anonymousWebSession, setWebSession as contextSetWebSession, createFromStorage } from '@/common/session-context/session-context';
import { Session } from '@/models/session/session';
import { sessionChangedSignal } from '@/services/login-service';

export default function CustomQueryClientProvider({ children }) {
  const [queryClient] = useState(() => new QueryClient())

  return (
    <QueryClientProvider client={queryClient}>{children}</QueryClientProvider>
  )
}

export type SessionProviderProps = {
  children?: any;
};

export function SessionProvider(props: SessionProviderProps) {
  const contextwebSession = createFromStorage() ?? anonymousWebSession;

  const [webSession, setWebSession] = useState<Session>(() => {
    contextSetWebSession(contextwebSession);
    return contextwebSession;
  });

  useEffect(() => {
    const webSessionSubscription = sessionChangedSignal?.add(session => {
      contextSetWebSession(session);
      setWebSession(session);
    });

    // cleanup from any listener events
    return () => {
      webSessionSubscription?.detach();
    };
  }, []);

  return (
    <WebSessionContext.Provider value={webSession}>
      {props.children}
    </WebSessionContext.Provider>
  );
}
