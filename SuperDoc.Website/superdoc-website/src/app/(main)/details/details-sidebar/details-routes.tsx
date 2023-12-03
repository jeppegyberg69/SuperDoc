import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faFileLines } from "@fortawesome/free-solid-svg-icons/faFileLines";

type CustomRoute = {
  id: string
  path: string;
  name: string;
  icon?: React.ReactNode
}

export function detailsMenuRoutes(caseId: number): CustomRoute[] {
  const prefix = `/details/${caseId}`

  return [
    {
      id: "documents",
      name: "Dokumenter",
      path: `${prefix}`,
      icon: <FontAwesomeIcon icon={faFileLines} />
    }
  ]
}