"use client";

import { Session, isTokenExpired } from '@/models/session/session';
import { useRouter } from 'next/navigation';
import React, { useEffect, useState } from 'react';

export type RequireAuthProps = {
  children: any;
};

export function RequireAuth(props: RequireAuthProps) {
  const router = useRouter()
  const [session, setSession] = useState<Session>();

  useEffect(() => {
    const b = localStorage.getItem("jpj_session")
    setSession(JSON.parse(b));
  }, [])


  // useEffect(() => {
  //   if (!session || isTokenExpired(session)) {
  //     router.push("/login");
  //   };
  // }, [session])

  return (
    <>
    {/* Should wait with returning props.children, and instead return a suspense or alike. */}
      {props.children}
    </>
  );
}