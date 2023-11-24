"use client"
import { getWebSession } from "@/common/session-context/session-context";
import { WebserviceResponse } from "./login-service";
import { Case } from "@/models/case";
import { CaseManagers } from "@/models/case-manager";

export function getCases() {
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

  return fetch("https://localhost:44304/api/Case/GetCases", requestOptions)
    .then(async (response): Promise<WebserviceResponse> => {
      if (response.ok) {
        const resp = await response.json()
        console.log(resp);

        return {
          status: response.status,
          statusText: response.statusText,
          data: resp// this wont error because we made sure that the response is ok earlier, so response.json is always an actual json value.

        }
      }
    })
    .then(transformGetCases)
  // .catch((error) => {
  //   console.log('dsa', error);
  // });
}


function transformGetCases(response: WebserviceResponse): Case[] {
  console.log(response);

  return response.data.map((v): Case => ({
    id: v.caseId,
    title: v.title,
    description: v.description,
    caseManagers: v.caseMangers.map((cm): CaseManagers => ({ // skal ændres ti "CaseManagers", der er stavefejl i nuværrende webservice
      id: cm.userId,
      emailAddress: cm.emailAddress,
      firstName: cm.firstName,
      lastName: cm.lastName,
    })),
    responsibleUser: [],
  }))
}