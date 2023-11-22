import { List, ListItem } from "@/common/list/list";
import { PageHeader } from "@/common/page-layout/page-header";
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
            <Link href={item.path}>{item.name}</Link>
          </ListItem>
        </li>
      ))}
    </List>
  )
}

type CustomRoute = {
  path: string;
  name: string;
}

export function detailsMenuRoutes(caseId: number): CustomRoute[] {
  const prefix = `/details/${caseId}`

  return [
    {
      name: "Dokumenter",
      path: `${prefix}`
    },
    {
      name: "Historik",
      path: `${prefix}/history`
    },
  ]
}