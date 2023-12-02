'use client';

import { Session, isTokenExpired } from "@/models/session/session";
import { createContext, useContext } from "react";

let currentSession: Session;

export function getWebSession(): Session {
  const session = currentSession ?? anonymousWebSession;
  return session;
}

export function setWebSession(session: Session) {
  currentSession = session;
}

export const WebSessionContext = createContext<Session>(null);

export function useWebSession() {
  return useContext<Session>(WebSessionContext);
}

export function createFromStorage(): Session | null {
  if (typeof window !== 'undefined') {
    let item;
    item = localStorage.getItem("jpj_websession");

    if (!item) {
      return null;
    }

    const session = JSON.parse(item) as Session;

    return isTokenExpired(session) ? null : session;
  }
  return null;
}

export const anonymousWebSession: Session = {
  token: null,
  user: null,
  validFrom: null,
  validTo: null,
};

export function clearWebSession() {
  localStorage.removeItem("jpj_websession");
  setWebSession(null);
}