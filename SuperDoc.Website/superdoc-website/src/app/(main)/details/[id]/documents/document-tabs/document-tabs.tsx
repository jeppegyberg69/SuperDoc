"use client";
import React, { useCallback, useEffect, useMemo, useState } from 'react';
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"
import { PdfViewer } from '@/common/pdf-viewer/pdf-viewer';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSortDown } from "@fortawesome/free-solid-svg-icons/faSortDown";
import { faSquareCheck } from "@fortawesome/free-solid-svg-icons/faSquareCheck";
import { faSquareMinus } from "@fortawesome/free-solid-svg-icons/faSquareMinus";
import { DocumentServiceQueryKeys, getRevisionFile, useDocumentRevisions } from '@/services/document-services';
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
import { UploadRevisionDialog } from './edit-dialog/upload-revision-dialog';
import { useQueryClient } from '@tanstack/react-query';
import { getWebSession } from '@/common/session-context/session-context';
import { Roles } from '@/common/access-control/access-control';

export type DocumentTabsProps = {
  caseDocument: CaseDocument;
};

export function DocumentTabs({ caseDocument }: DocumentTabsProps) {
  const queryClient = useQueryClient();
  const { data: revisionData, isFetched } = useDocumentRevisions(caseDocument?.id ?? '');
  const [revision, setRevision] = useState<DocumentRevision>();
  const [newUrl, setNewUrl] = useState("");
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const onDialogOpenedChanged = (openState) => setIsDialogOpen(openState);
  const onDialogClose = () => {
    queryClient.invalidateQueries({ queryKey: [DocumentServiceQueryKeys.getDocumentRevisions, caseDocument.caseId] });
  }

  useEffect(() => {
    if (revisionData && isFetched) {
      setRevision(revisionData[0])
    }
  }, [isFetched, revisionData])

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
  }, [revision])

  return (
    <>
      <Tabs defaultValue="dokumentvisning" className="h-full w-full">
        <TabsList className='w-full'>
          <TabsTrigger value="dokumentvisning">Dokumentvisning</TabsTrigger>
          <RevisionsList
            documentRevisions={revisionData}
            onRevisionClick={(v) => { setRevision(v) }}
            caseDocument={caseDocument}
            setIsDialogOpen={setIsDialogOpen}
          />
        </TabsList>
        <TabsContent value="dokumentvisning">
          <div className='h-full w-full flex'>
            <PdfViewer className='h-full w-full' url={newUrl}></PdfViewer>

            <div className='flex flex-col h-full w-64 mx-6 '>
              Underskrift:
              <div className='flex flex-col w-full text-sm text-muted-foreground'>
                <div>
                  <div>
                    Titel: {caseDocument?.title}
                  </div>
                  Brugere: {revision?.signatures?.map(v =>
                    <div key={v.emailAddress} className='w-full'>
                      {v.emailAddress} - {v.dateSigned ? <FontAwesomeIcon icon={faSquareCheck} color='green' /> : <FontAwesomeIcon icon={faSquareMinus} color='red' />}
                    </div>
                  )}
                </div>
              </div>
            </div>
          </div>
        </TabsContent>
      </Tabs>

      <UploadRevisionDialog onClose={onDialogClose} isDialogOpen={isDialogOpen} onOpenChanged={onDialogOpenedChanged} caseDocument={caseDocument}></UploadRevisionDialog>
    </>
  );
}

export type RevisionsListProps = {
  documentRevisions: DocumentRevision[];
  onRevisionClick: (revision: DocumentRevision) => void;
  caseDocument: CaseDocument;
  setIsDialogOpen: React.Dispatch<React.SetStateAction<boolean>>;
};

export function RevisionsList({ documentRevisions, onRevisionClick, caseDocument, setIsDialogOpen }: RevisionsListProps) {
  // the idea of this is that we can listen on changes to any variable we use, and if theres a change then the JSX should update.
  const renderer = useCallback(() => {
    return (
      <div className='h-full w-full flex items-center justify-end mx-4'>
        <Menubar className="mx-4">
          <MenubarMenu>
            <MenubarTrigger
              className="!hover:text-primary-foreground cursor-pointer">
              <span className='mx-2'>Revisioner</span>
              <FontAwesomeIcon icon={faSortDown} className='!align-top self-baseline' />
            </MenubarTrigger>
            <MenubarContent>
              {documentRevisions?.map((rev, index) => (
                <MenubarItem
                  className="h-12 text-base text-muted-foreground focus:bg-accent/20"
                  key={rev.id}
                >
                  <Button
                    variant='ghost'
                    className='w-full focus:!bg-accent/0 hover:bg-transparent'
                    onClick={() => onRevisionClick?.(rev)}
                  >
                    {'Revision: ' + rev.id}
                  </Button>
                </MenubarItem>
              ))}
              {getWebSession().user?.role !== Roles.User &&
                <>
                  <hr></hr>
                  <MenubarItem
                    className="focus:!bg-accent/0 hover:bg-transparent h-12 text-base text-muted-foreground"
                  >
                    <Button
                      disabled={!caseDocument}
                      variant='ghost'
                      className='w-full'
                      onClick={() => {
                        setIsDialogOpen(true)
                      }}
                    >
                      Opret ny revision
                    </Button>
                  </MenubarItem>
                </>
              }
            </MenubarContent>
          </MenubarMenu>
        </Menubar>
      </div >
    )
  }, [documentRevisions, onRevisionClick, setIsDialogOpen, caseDocument])

  return renderer();
}