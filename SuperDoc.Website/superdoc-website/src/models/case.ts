import { CaseManagers } from "./case-manager";

export type User = CaseManagers & {};

export type Case = {
  id: string;
  caseNumber: number;
  title: string;
  description: string;
  caseManagers: CaseManagers;
  responsibleUser: User;
}