"use client"
import React, { useEffect, useState } from "react";
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
  DropdownMenuLabel,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { getGlobalNavigationRoutes } from "@/models/route";

import { faBars } from "@fortawesome/free-solid-svg-icons/faBars";
import { faCalendarCheck } from "@fortawesome/free-solid-svg-icons/faCalendarCheck";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Link from "next/link";
import { clearWebSession, getWebSession } from "../session-context/session-context";
import { useRouter } from "next/navigation";

export type PageHeaderProps = {
  left?: React.ReactNode;
  right?: React.ReactNode;
}

export function PageHeader(props: PageHeaderProps) {
  const router = useRouter()
  const session = getWebSession();

  const [userInitials, setUserInitials] = useState("");

  useEffect(() => {
    setUserInitials(
      [session?.user?.firstName, session?.user?.lastName]
        .join(' ')
        .split(' ')
        .map(word => word[0])
        .filter(v => !!v)
        .slice(0, 2)
        .join('')
    )
  }, [session])


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
      <AvatarFallback className="bg-slate-500 text-primary-foreground">
        {session && userInitials && (userInitials ?? '')}
      </AvatarFallback>
    </Avatar>
  )

  const dropdownMenu = (
    <DropdownMenu>
      <DropdownMenuTrigger>{avatar}</DropdownMenuTrigger>
      <DropdownMenuContent className="mx-1">
        <DropdownMenuLabel className="w-full block">
          <Link href={"/login"} onClick={() => { clearWebSession(); }} className="p-1 rounded-sm w-full block hover:bg-gray-700/20" >
            Log ud
          </Link>
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