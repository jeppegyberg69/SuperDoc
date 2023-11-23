import { List, ListItem } from "@/common/list/list";
import { PageHeader } from "@/common/page-layout/page-header";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faFileLines } from "@fortawesome/free-solid-svg-icons/faFileLines";
import { faClockRotateLeft } from "@fortawesome/free-solid-svg-icons/faClockRotateLeft";

import Link from "next/link";

export default function DetailsLayout({ params, children }: { params: { id: any }, children: any }) {
  return (
    <div className='detail-layout h-full gap-4'>
      <PageHeader />
      <div className='detail-layout-sidebar p-4'><DetailsSidebar caseId={params.id} /></div>
      <div className="detail-layout-content">
        {children}
      </div>
    </div>
  );
}

export function DetailsSidebar({ caseId }) {
  const routes = detailsMenuRoutes(caseId);
  return (
    <List>
      {routes.map((item) => (
        <li key={item.path} className='list-none flex align-middle'>
          <ListItem>
            <Link className="w-full flex align-middle justify-center" href={item.path}>
              {item.icon && (
                <div className="mx-2">
                  {item.icon}
                </div>
              )} <span className="self-center">{item.name}</span>
            </Link>
          </ListItem>
        </li>
      ))}
    </List>
  )
}

type CustomRoute = {
  path: string;
  name: string;
  icon?: React.ReactNode
}

export function detailsMenuRoutes(caseId: number): CustomRoute[] {
  const prefix = `/details/${caseId}`

  return [
    {
      name: "Dokumenter",
      path: `${prefix}`,
      icon: <FontAwesomeIcon icon={faFileLines} />
    },
    {
      name: "Historik",
      path: `${prefix}/history`,
      icon: <FontAwesomeIcon icon={faClockRotateLeft} />
    },
  ]
}