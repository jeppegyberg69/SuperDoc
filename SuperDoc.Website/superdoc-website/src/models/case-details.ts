import { Case } from "./case"
import { CaseDocument } from "./document";

export type CaseDetails = {
  case: Case;
  documents?: CaseDocument[];
}