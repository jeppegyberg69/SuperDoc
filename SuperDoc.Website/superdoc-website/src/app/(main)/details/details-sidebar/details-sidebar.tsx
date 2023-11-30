"use client"

import { List, ListItem } from "@/common/list/list";

import Link from "next/link";
import { useState } from "react";
import { detailsMenuRoutes } from "./details-routes";

export function DetailsSidebar({ caseId }) {
  const routes = detailsMenuRoutes(caseId);
  const [activeRoute, setActiveRoute] = useState("documents");

  return (
    <List>
      {routes.map((item) => (
        <li
          key={item.id}
          data-active={activeRoute === item.id ? 'active' : 'inactive'}
          className='h-full list-none flex align-middle border-l data-[active=active]:border-x-black data-[active=inactive]:border-x-transparent'
        >
          <ListItem>
            <Link className="w-full h-12 flex align-middle self-center justify-center" href={item.path} onClick={() => setActiveRoute(item.id)}>
              {item.icon && (
                <div className="mx-2 self-center">
                  {item.icon}
                </div>
              )}
              <span className="self-center">{item.name}</span>
            </Link>
          </ListItem>
        </li>
      ))}
    </List>
  )
}