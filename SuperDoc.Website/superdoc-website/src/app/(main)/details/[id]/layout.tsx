import { List, ListItem } from "@/common/list";
import { DetailsHeader } from "./header/details-header";


export default function DetailsLayout({ id, children }: any) {
  return (
    <div className='detail-layout h-full gap-4'>
      <DetailsHeader caseId={id} />
      <div className='detail-layout-sidebar p-4 bg-neutral-100'> <DetailsSidebar caseId={id} /></div>
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
            {item.name}
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
  const prefix = `details/${caseId}`

  return [
    {
      name: "Generalt",
      path: `${prefix}`
    },
    {
      name: "Historik",
      path: `${prefix}/history`
    },
  ]
}