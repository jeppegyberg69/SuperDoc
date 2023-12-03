"use client";

import { isTokenExpired } from '@/models/session/session';
import { useRouter } from 'next/navigation';
import React, { useEffect } from 'react';
import { useWebSession } from '../session-context/session-context';

export type RequireAuthProps = {
  children: any;
};

export function RequireAuth(props: RequireAuthProps) {
  const router = useRouter()
  const session = useWebSession();

  // check if session is expired or doesnt exist atm
  useEffect(() => {
    if (!session || isTokenExpired(session) || !session?.token) {
      router.push("/login");
    };
  }, [session])

  return (
    <>
      {props.children}
    </>
  );
}
