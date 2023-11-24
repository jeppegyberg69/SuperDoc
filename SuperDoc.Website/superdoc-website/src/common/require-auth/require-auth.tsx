"use client";

import { Session, isTokenExpired } from '@/models/session/session';
import { useRouter } from 'next/navigation';
import React, { useEffect, useState } from 'react';
import { useWebSession } from '../session-context/session-context';

export type RequireAuthProps = {
  children: any;
};

export function RequireAuth(props: RequireAuthProps) {
  const router = useRouter()
  const session = useWebSession();

  useEffect(() => {
    if (!session || isTokenExpired(session)) {
      router.push("/login");
    };
  }, [session])

  return (
    <>
      {/* Should wait with returning props.children, and instead return a suspense or alike. */}
      {props.children}
    </>
  );
}
