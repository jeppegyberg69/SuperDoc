import { getWebSession } from "@/common/session-context/session-context";
import { CaseDocument } from "@/models/document";
import { DocumentRevision } from "@/models/document-revision";
import { buildConfig } from "@/models/webservice/base-url";
import { WebserviceResponse } from "@/models/webservice/webservice-model";
import { useQuery } from "@tanstack/react-query";

export const DocumentServiceQueryKeys = {
  getDocuments: "/api/Document/GetDocumentsByCaseId",
  getDocumentRevisions: "/api/Revision/GetRevisionsByDocumentId"
}



export function getDocuments(caseId: string): Promise<CaseDocument[]> {
  const myHeaders = new Headers();
  myHeaders.append(
    "Authorization",
    `Bearer ${getWebSession().token}`
  );

  const requestOptions: RequestInit = {
    method: 'GET',
    headers: myHeaders,
    redirect: 'follow'
  };


  let params = "";
  params += `caseId=${caseId}`;


  return fetch(`${buildConfig.API}${DocumentServiceQueryKeys.getDocuments}?${params}`, requestOptions)
    .then(async (response): Promise<WebserviceResponse> => {
      if (response.ok) {
        const resp = await response.json()
        return {
          status: response.status,
          statusText: response.statusText,
          data: resp// this wont error because we made sure that the response is ok earlier, so response.json is always an actual json value.
        }
      }
    })
    .then(transformGetDocuments)
}

function transformGetDocuments(response: WebserviceResponse): CaseDocument[] {
  const data = response.data;

  return data.map(v => ({
    id: v.documentId,
    caseId: v.caseId,
    title: v.title,
    description: v.description,
    dateCreated: v.dateCreated,
    dateModified: v.dateModified,
    externalUsers: v.externalUsers.map(user => ({
      id: user.userId,
      emailAddress: user.emailAddress,
      firstName: user.firstName,
      lastName: user.lastName,
    }))
  }))
}

export function useDocuments(caseId: string) {

  return useQuery({
    queryKey: [DocumentServiceQueryKeys.getDocuments],
    queryFn() { return getDocuments(caseId) },
    initialData: []
  })
}


export function getDocumentRevisions(documentId: string): Promise<DocumentRevision[]> {
  const myHeaders = new Headers();
  myHeaders.append(
    "Authorization",
    `Bearer ${getWebSession().token}`
  );

  const requestOptions: RequestInit = {
    method: 'GET',
    headers: myHeaders,
    redirect: 'follow'
  };


  let params = "";
  params += `documentId=${documentId}`;


  return fetch(`${buildConfig.API}${DocumentServiceQueryKeys.getDocumentRevisions}?${params}`, requestOptions)
    .then(async (response): Promise<WebserviceResponse> => {
      if (response.ok) {
        const resp = await response.json()
        return {
          status: response.status,
          statusText: response.statusText,
          data: resp// this wont error because we made sure that the response is ok earlier, so response.json is always an actual json value.
        }
      }
    })
    .then(transformGetDocumentRevisions)
}

function transformGetDocumentRevisions(response: WebserviceResponse): DocumentRevision[] {
  const data = response.data;

  return data.map(v => ({
    id: v.revisionId,
    dateUploaded: v.dateUploaded,
    signatures: v.signatures.map((signature) => ({
      firstName: signature.firstName,
      lastName: signature.lastName,
      emailAddress: signature.emailAddress,
      publicKey: signature.publicKey,
      signature: signature.signature,
      dateSigned: signature.dateSigned,
    }))
  }))
}


export function useDocumentRevisions(caseId: string) {
  return useQuery<DocumentRevision[], any, DocumentRevision[]>({
    queryKey: [DocumentServiceQueryKeys.getDocumentRevisions, caseId],
    queryFn() {
      if (!caseId) {
        return [];
      }

      return getDocumentRevisions(caseId)
    },
  })
}



export type CreateDocumentProps = {
  caseId: string;
  title: string,
  description: string;
}

export function createDocument({ caseId, description, title, }: CreateDocumentProps): Promise<string> {
  const myHeaders = new Headers();
  myHeaders.append("Content-Type", "application/json");
  myHeaders.append(
    "Authorization",
    `Bearer ${getWebSession().token}`
  );

  const raw = JSON.stringify({
    "caseId": caseId,
    "title": title,
    "description": description,
  });

  const requestOptions: RequestInit = {
    method: 'POST',
    headers: myHeaders,
    body: raw,
    redirect: 'follow'
  };


  return fetch(`${buildConfig.API}/api/Document/CreateOrUpdateDocument`, requestOptions)
    .then(async (response): Promise<WebserviceResponse> => {
      if (response.ok) {
        const resp = await response.json()
        return {
          status: response.status,
          statusText: response.statusText,
          data: resp // this wont error because we made sure that the response is ok earlier, so response.json is always an actual json value.
        }
      }
    })
    .then((response: WebserviceResponse) => { return response.data as unknown as string })
}


export function getRevisionFile(revisionId: string): Promise<any> {
  const myHeaders = new Headers();
  myHeaders.append(
    "Authorization",
    `Bearer ${getWebSession().token}`
  );

  const requestOptions: RequestInit = {
    method: 'GET',
    headers: myHeaders,
    redirect: 'follow'
  };


  let params = "";
  params += `revisionId=${revisionId}`;
  // params += `access_token=${getWebSession().token}`;

  return fetch(`${buildConfig.API}/api/Revision/DownloadRevision?${params}`, requestOptions)
    .then(async (response): Promise<WebserviceResponse> => {
      if (response.ok) {
        const respArrayBuffer = await response.arrayBuffer()


        return {
          status: response.status,
          statusText: response.statusText,
          data: respArrayBuffer// this wont error because we made sure that the response is ok earlier, so response.json is always an actual json value.
        }
      }
    })
    .then((response) => response?.data)
}