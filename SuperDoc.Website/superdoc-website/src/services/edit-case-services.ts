import { getWebSession } from "@/common/session-context/session-context";
import { CaseDetails } from "@/models/case-details";
import { WebserviceResponse } from "@/models/webservice/webservice-model";

export type CreateCaseProps = {
  title: string,
  description: string,
  caseManagersId: string[]
}

export function createCase({ caseManagersId, description, title }: CreateCaseProps): Promise<CaseDetails> {
  const myHeaders = new Headers();
  myHeaders.append("Content-Type", "application/json");
  myHeaders.append(
    "Authorization",
    `Bearer ${getWebSession().token}`
  );

  const raw = JSON.stringify({
    "title": title,
    "description": description,
    "caseMangers": caseManagersId,
    "responsibleUserId": getWebSession().user.id
  });

  const requestOptions: RequestInit = {
    method: 'POST',
    headers: myHeaders,
    body: raw,
    redirect: 'follow'
  };


  return fetch("https://localhost:44304/api/Case/CreateOrUpdateCase", requestOptions)
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

