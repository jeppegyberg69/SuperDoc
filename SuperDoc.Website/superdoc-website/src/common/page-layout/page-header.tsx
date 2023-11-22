"use client"
import {
  Menubar,
  MenubarContent,
  MenubarItem,
  MenubarMenu,
  MenubarTrigger,
} from "@/components/ui/menubar"

import { getGlobalNavigationRoutes } from "@/models/route";

import { faBars } from "@fortawesome/free-solid-svg-icons/faBars";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Link from "next/link";

export function PageHeader() {
  const routes = getGlobalNavigationRoutes();
  const menubar = (
    <Menubar>
      <MenubarMenu>
        <MenubarTrigger><FontAwesomeIcon icon={faBars} /></MenubarTrigger>
        <MenubarContent>
          {routes.map((route) => (
            <MenubarItem key={route.path}><Link className="w-full" href={route.path}> {route.title}</Link> </MenubarItem>
          ))}
        </MenubarContent>
      </MenubarMenu>
    </Menubar>
  );

  return (
    <div className='grid grid-cols-3 bg-neutral-100 detail-layout-header w-full'>
      <div className="_navigation-section flex">
        <div>
          {menubar}
        </div>
      </div>
    </div>
  )
}