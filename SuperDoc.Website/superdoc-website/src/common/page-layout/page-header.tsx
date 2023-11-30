"use client"
import React from "react";
import {
  Menubar,
  MenubarContent,
  MenubarItem,
  MenubarMenu,
  MenubarTrigger,
} from "@/components/ui/menubar"
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { getGlobalNavigationRoutes } from "@/models/route";

import { faBars } from "@fortawesome/free-solid-svg-icons/faBars";
import { faCalendarCheck } from "@fortawesome/free-solid-svg-icons/faCalendarCheck";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Link from "next/link";

export type PageHeaderProps = {
  left?: React.ReactNode;
  right?: React.ReactNode;
}

export function PageHeader(props: PageHeaderProps) {
  const routes = getGlobalNavigationRoutes();
  const menubar = (
    <Menubar className="mx-4">
      <MenubarMenu>
        <MenubarTrigger
          className="menubar-trigger cursor-pointer">
          <FontAwesomeIcon icon={faBars} />
        </MenubarTrigger>
        <MenubarContent>
          {routes.map((route) => (
            <MenubarItem
              className="h-12 text-base text-muted-foreground"
              key={route.path}
            >
              <Link className="w-full flex" href={route.path}>
                <div className="mx-2">
                  <FontAwesomeIcon icon={faCalendarCheck} />
                </div>
                {route.title}
              </Link>
            </MenubarItem>
          ))}
        </MenubarContent>
      </MenubarMenu>
    </Menubar>
  );

  const avatar = (
    <Avatar>
      <AvatarImage src="https://github.com/shadcn.png" />
      <AvatarFallback>CN</AvatarFallback>
    </Avatar>
  )

  const dropdownMenu = (
    <DropdownMenu>
      <DropdownMenuTrigger>
        {avatar}
      </DropdownMenuTrigger>
      <DropdownMenuContent className="mx-1">
        <DropdownMenuLabel>
          {/*NOTE:  fjern session og log brugeren ud */}
          Log ud
        </DropdownMenuLabel>
      </DropdownMenuContent>
    </DropdownMenu>
  )

  return (
    <div className='grid grid-cols-3 bg-neutral-100 page-layout-header w-full'>
      <div className="flex flex-1">
        {menubar}
        <div className="flex flex-1 self-center">
          {props.left}
        </div>
      </div>
      <div></div>
      <div className="flex justify-end mr-4">
        {dropdownMenu}
      </div>
    </div>
  )
}