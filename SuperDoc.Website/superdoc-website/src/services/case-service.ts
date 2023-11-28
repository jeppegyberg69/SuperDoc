"use client"
import { getWebSession } from "@/common/session-context/session-context";
import { Case } from "@/models/case";
import { CaseManagers } from "@/models/case-manager";
import { WebserviceResponse } from "@/models/webservice/webservice-model";
import { useQuery } from "@tanstack/react-query";

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
        return {
          status: response.status,
          statusText: response.statusText,
          data: resp// this wont error because we made sure that the response is ok earlier, so response.json is always an actual json value.
        }
      }
    })
    .then(transformGetCases)
}

function transformGetCases(response: WebserviceResponse): Case[] {
  return response.data.map((v): Case => ({
    id: v.caseId,
    caseNumber: v.caseNumber,
    title: v.title,
    description: v.description,
    caseManagers: v.caseManagers.map((cm): CaseManagers => ({ // skal ændres ti "CaseManagers", der er stavefejl i nuværrende webservice
      id: cm.userId,
      emailAddress: cm.emailAddress,
      firstName: cm.firstName,
      lastName: cm.lastName,
    })),
    responsibleUser: {
      id: v.responsibleUser.userId,
      emailAddress: v.responsibleUser.emailAddress,
      firstName: v.responsibleUser.firstName,
      lastName: v.responsibleUser.lastName,
    }
  }))
}

export function useGetCaseDetails(caseId: string) {
  return useQuery({
    queryKey: ['snowball'],
    queryFn() {
      return getCases()
        .then((cases) => {
          const caseData = cases.filter(v => v.id === caseId)[0]

          return {
            case: {
              ...caseData
            },
            documents: []

          } ?? null
        })
    },
    gcTime: 0 // cachetime/garbage collection time
  })
}

export function getCaseManagers(caseId?: string) {
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

  return fetch("https://localhost:44304/api/Case/GetCaseManagers", requestOptions)
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
    .then(transformGetCaseManagers)
}

function transformGetCaseManagers(response: WebserviceResponse): CaseManagers[] {
  const data = response.data;

  return data.map((v) => ({
    id: v.userId,
    emailAddress: v.emailAddress,
    firstName: v.firstName,
    lastName: v.lastName
  }))
}