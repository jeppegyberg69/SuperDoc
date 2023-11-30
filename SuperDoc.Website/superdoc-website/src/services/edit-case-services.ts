import { getWebSession } from "@/common/session-context/session-context";
import { CaseDetails } from "@/models/case-details";
import { buildConfig } from "@/models/webservice/base-url";
import { WebserviceResponse } from "@/models/webservice/webservice-model";

export type CreateCaseProps = {
  caseId?: string
  title: string,
  description: string,
  caseManagersId: string[],
  responsibleUserId?: string
}

export function createCase({ caseId, caseManagersId, description, title, responsibleUserId }: CreateCaseProps): Promise<CaseDetails> {
  const myHeaders = new Headers();
  myHeaders.append("Content-Type", "application/json");
  myHeaders.append(
    "Authorization",
    `Bearer ${getWebSession().token}`
  );
    console.log('responsible',responsibleUserId);
    
  const raw = JSON.stringify({
    "caseId": caseId,
    "title": title,
    "description": description,
    "caseMangers": caseManagersId,
    "responsibleUserId": responsibleUserId ?? getWebSession().user.id
  });

  const requestOptions: RequestInit = {
    method: 'POST',
    headers: myHeaders,
    body: raw,
    redirect: 'follow'
  };


  return fetch(`${buildConfig.API}/api/Case/CreateOrUpdateCase`, requestOptions)
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
    .then(transformCreateCaseResponse)
}

function transformCreateCaseResponse(response: WebserviceResponse): CaseDetails {
  const data = response.data;

  return {
    case: {
      id: data.caseId,
      caseNumber: data.caseNumber,
      title: data.title,
      description: data.description,
      responsibleUser: {
        id: data.userId,
        emailAddress: data.emailAddress,
        firstName: data.firstName,
        lastName: data.lastName
      },
      caseManagers: data.caseManagers.map((cm) => ({
        id: data.userId,
        emailAddress: data.emailAddress,
        firstName: data.firstName,
        lastName: data.lastName
      })),
    },
    documents: []
  }
}

