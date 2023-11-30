export type DocumentRevision = {
  id: string
  dateUploaded: string
  signatures: Signatures[];
}

export type Signatures = {
  firstName: string;
  lastName: string;
  emailAddress: string;
  publicKey: string;
  signature: string;
  dateSigned: string;
}