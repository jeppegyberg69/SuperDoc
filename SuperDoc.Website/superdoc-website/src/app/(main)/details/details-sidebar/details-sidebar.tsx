"use client"

import { List, ListItem } from "@/common/list/list";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faFileLines } from "@fortawesome/free-solid-svg-icons/faFileLines";
import { faClockRotateLeft } from "@fortawesome/free-solid-svg-icons/faClockRotateLeft";

import Link from "next/link";
import { useState } from "react";

export function DetailsSidebar({ caseId }) {
  const routes = detailsMenuRoutes(caseId);
  const [activeRoute, setActiveRoute] = useState("documents");

  return (
    <List>
      {routes.map((item) => (
        <li key={item.id} data-active={activeRoute === item.id ? 'active' : 'inactive'} className='list-none flex align-middle border-l data-[active=active]:border-x-black data-[active=inactive]:border-x-transparent'>
          <ListItem>
            <Link className="w-full flex align-middle justify-center" href={item.path} onClick={() => setActiveRoute(item.id)}>
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
    },
    {
      id: "history",
      name: "Historik",
      path: `${prefix}/history`,
      icon: <FontAwesomeIcon icon={faClockRotateLeft} />
    },
  ]
}