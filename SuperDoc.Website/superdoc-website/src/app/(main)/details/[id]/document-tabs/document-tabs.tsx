"use client";
import React from 'react';
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { PdfViewer } from '@/common/pdf-viewer/pdf-viewer';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSortDown } from "@fortawesome/free-solid-svg-icons/faSortDown";


export type DocumentTabsProps = {};

export function DocumentTabs(props: DocumentTabsProps) {
  return (
    <Tabs defaultValue="dokumentvisning" className="h-full w-full">
      <TabsList className='w-full'>
        <TabsTrigger value="dokumentvisning">Dokumentvisning</TabsTrigger>
        <TabsTrigger value="kommentar">Kommentar</TabsTrigger>
        <div className='h-full w-full flex justify-end mx-4'>
          <DropdownMenu>
            <DropdownMenuTrigger> <span>Revisioner</span> <FontAwesomeIcon icon={faSortDown} className='!align-top' /> </DropdownMenuTrigger>
            <DropdownMenuContent className="mx-1">
              {/*NOTE: Mangler webservice til revisioner */}
              {['Revision 1.0', 'Revision 1.2', 'Seneste revision'].map((revision) => {
                return (
                  <DropdownMenuLabel key={revision}>
                    {revision}
                  </DropdownMenuLabel>
                )
              })}
            </DropdownMenuContent>
          </DropdownMenu>
        </div>
      </TabsList>
      <TabsContent value="dokumentvisning">
        <PdfViewer url="https://www.africau.edu/images/default/sample.pdf"></PdfViewer>
      </TabsContent>
      <TabsContent value="kommentar">
      </TabsContent>
    </Tabs>
  );
}