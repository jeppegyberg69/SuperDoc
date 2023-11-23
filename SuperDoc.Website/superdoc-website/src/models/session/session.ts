export type Session = {
  user: {
    fullName: string;
    firstName: string;
    lastName: string;
    email: string;
    role: string;
  }
  token: string;
  validFrom: string;
  validTo: string;
}


export function isTokenExpired(session: Session): boolean {
  return new Date(session?.validTo) < new Date();
}


export function createSessionFromToken(session): Session {
  const newSession: Session = {
    token: session.token,
    validFrom: session.validFrom,
    validTo: session.validTo,

    user: {
      role: session.role,
      email: session.email,
      firstName: session.firstName,
      lastName: session.lastName,
      fullName: session.firstName + session.lastName
    }
  }

  return newSession;
}