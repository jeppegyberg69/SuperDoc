export type Session = {
  user: {
    fullName: string;
    firstName: string;
    lastName: string;
    emailAddress: string;
    role: string;
    id: string;
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
      id: session.userId,
      role: session.role,
      emailAddress: session.emailAddress,
      firstName: session.firstName,
      lastName: session.lastName,
      fullName: session.firstName + session.lastName
    }
  }

  return newSession;
}

export function saveInStorage(session: Session) {
  localStorage.setItem("jpj_websession", JSON.stringify(session));
}

export function removeFromStorage() {
  localStorage.removeItem("jpj_websession");
}