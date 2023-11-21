"use client"
import {
  Menubar,
  MenubarCheckboxItem,
  MenubarContent,
  MenubarItem,
  MenubarMenu,
  MenubarRadioGroup,
  MenubarRadioItem,
  MenubarSeparator,
  MenubarShortcut,
  MenubarSub,
  MenubarSubContent,
  MenubarSubTrigger,
  MenubarTrigger,
} from "@/components/ui/menubar"

import { faBars } from "@fortawesome/free-solid-svg-icons/faBars";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";


export function DetailsHeader({ caseId }) {
  const routes = detailsMenuRoutes(caseId);
  const menubar = (
    <Menubar>
      <MenubarMenu>
        <MenubarTrigger><FontAwesomeIcon icon={faBars} /></MenubarTrigger>
        <MenubarContent>
          {routes.map((route) => (
            <MenubarItem key={route.path}> {route.name} </MenubarItem>
          ))}
        </MenubarContent>
      </MenubarMenu>
    </Menubar>
  );

  return (
    <div className='grid grid-cols-3 bg-neutral-100 detail-layout-banner w-full'>
      <div className="_navigation-section flex">
        <div>
          {menubar}
        </div>
      </div>
    </div>
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