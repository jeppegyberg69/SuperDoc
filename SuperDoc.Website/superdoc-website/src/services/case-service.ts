"use client"
import { getWebSession } from "@/common/session-context/session-context";
import { Case } from "@/models/case";
import { CaseManagers } from "@/models/case-manager";
import { buildConfig } from "@/models/webservice/base-url";
import { WebserviceResponse } from "@/models/webservice/webservice-model";
import { useQuery } from "@tanstack/react-query";

const QueryKeys = {
  getCases: "/api/Case/GetCases",
  useGetDetails: "caseDetailQueryKey",
  getCaseManagers: "/api/Case/GetCaseManagers"
}
export { QueryKeys as CaseServiceQueryKeys }

export function getCases() {
  const myHeaders = new Headers();
  myHeaders.append(
    "Authorization",
    `Bearer ${getWebSession().token}`
  );

  const requestOptions: RequestInit = {
    method: 'GET',
    headers: myHeaders,
    redirect: 'follow',
    // make sure not to cache any data, as this can make loading data look odd
    cache: "no-store"
  };

  return fetch(`${buildConfig.API}${QueryKeys.getCases}`, requestOptions)
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
    caseManagers: v.caseManagers.map((cm): CaseManagers => ({
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

export function useGetCases() {
  return useQuery({
    gcTime: 0,
    queryKey: [QueryKeys.getCases],
    queryFn() {
      return getCases()
    },
    initialData: []
  })
}

export function useGetCaseDetails(caseId: string) {
  return useQuery({
    queryKey: [QueryKeys.useGetDetails],
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

  return fetch(`${buildConfig.API}${QueryKeys.getCaseManagers}`, requestOptions)
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