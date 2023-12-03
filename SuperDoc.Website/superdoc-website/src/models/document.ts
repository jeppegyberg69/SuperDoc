import { User } from "./case";

export type CaseDocument = {
  id: string;
  caseId: string;
  title: string;
  description: string;
  dateCreated: string;
  dateModified: string;
  externalUsers: User[];
}

