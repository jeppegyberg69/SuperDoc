"use client";
import React, { useEffect, useState } from 'react';
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
import { getRevisionFile, useDocumentRevisions, useDownloadRevisionFile } from '@/services/document-services';
import { CaseDocument } from '@/models/document';
import {
  Menubar,
  MenubarContent,
  MenubarItem,
  MenubarMenu,
  MenubarTrigger,
} from "@/components/ui/menubar"
import { Button } from '@/components/ui/button';
import { DocumentRevision } from '@/models/document-revision';


export type DocumentTabsProps = {
  caseDocument: CaseDocument;
  pdfUrl: string;
};

export function DocumentTabs({ caseDocument, pdfUrl }: DocumentTabsProps) {
  const [revision, setRevision] = useState<any>();
  const { data } = useDocumentRevisions(caseDocument?.id ?? '');
  const [newUrl, setNewUrl] = useState("");
  const getFile = () => {
    if (!revision) return;
    return getRevisionFile(revision?.id).then((resp) => {
      const blob = new Blob([resp], { type: 'application/pdf' });
      const blobUrl = URL.createObjectURL(blob);
      setNewUrl(blobUrl);
      (document.getElementById("pdfViewerIframe") as HTMLIFrameElement).src = blobUrl
    })
  }

  useEffect(() => {
    getFile()
  }, [data, revision])



  return (
    <Tabs defaultValue="dokumentvisning" className="h-full w-full">
      <TabsList className='w-full'>
        <TabsTrigger value="dokumentvisning">Dokumentvisning</TabsTrigger>
        <TabsTrigger value="kommentar">Kommentar</TabsTrigger>
        <RevisionsList
          documentRevisions={data}
          onRevisionClick={(v) => { setRevision(v) }}
        />
      </TabsList>
      <TabsContent value="dokumentvisning">
        <PdfViewer url={newUrl}></PdfViewer>
      </TabsContent>
      <TabsContent value="kommentar">
      </TabsContent>
    </Tabs>
  );
}

export type RevisionsListProps = {
  documentRevisions: DocumentRevision[];
  onRevisionClick: (revision: DocumentRevision) => void
};

export function RevisionsList({ documentRevisions, onRevisionClick }: RevisionsListProps) {

  return (
    <div className='h-full w-full flex justify-end mx-4'>
      <Menubar className="mx-4">
        <MenubarMenu>
          <MenubarTrigger
            className="menubar-trigger cursor-pointer">
            <span className='mx-2'>Revisioner</span>
            <FontAwesomeIcon icon={faSortDown} className='!align-top self-center' />
          </MenubarTrigger>
          <MenubarContent>
            {documentRevisions?.map((rev) => (
              <MenubarItem
                className="h-12 text-base text-muted-foreground"
                key={rev.id}
              >
                <Button
                  variant='ghost'
                  className='w-full'
                  onClick={() => onRevisionClick?.(rev)}
                >
                  {'Revision: ' + rev.id}

                </Button>
              </MenubarItem>
            ))}
          </MenubarContent>
        </MenubarMenu>
      </Menubar>
    </div>


  );
}