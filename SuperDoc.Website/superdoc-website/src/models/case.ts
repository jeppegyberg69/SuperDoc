import { CaseManagers } from "./case-manager";

export type Case = {
  id: string;
  title: string;
  description: string;
  caseManagers: CaseManagers;
  responsibleUser: any;
}