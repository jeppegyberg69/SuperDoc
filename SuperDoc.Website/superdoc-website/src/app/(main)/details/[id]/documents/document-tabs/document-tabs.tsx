"use client";
import React, { useEffect, useRef, useState } from 'react';
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"
import { PdfViewer } from '@/common/pdf-viewer/pdf-viewer';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSortDown } from "@fortawesome/free-solid-svg-icons/faSortDown";
import { faSquareCheck } from "@fortawesome/free-solid-svg-icons/faSquareCheck";
import { faSquareMinus } from "@fortawesome/free-solid-svg-icons/faSquareMinus";
import { DocumentServiceQueryKeys, getRevisionFile, uploadRevision, useDocumentRevisions } from '@/services/document-services';
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

export type DocumentTabsProps = {
  caseDocument: CaseDocument;
  pdfUrl: string;
};

export function DocumentTabs({ caseDocument }: DocumentTabsProps) {
  const queryClient = useQueryClient();
  const { data: documentRevisionsData, isFetched } = useDocumentRevisions(caseDocument?.id ?? '');

  const [revision, setRevision] = useState<DocumentRevision>();
  const [newUrl, setNewUrl] = useState("");
  const [isDialogOpen, setIsDialogOpen] = useState(false);

  const onDialogOpenedChanged = (openState) => setIsDialogOpen(openState);
  const onDialogClose = () => {
    queryClient.invalidateQueries({ queryKey: [DocumentServiceQueryKeys.getDocumentRevisions, caseDocument.caseId] });
  }

  useEffect(() => {
    if (documentRevisionsData && documentRevisionsData.length > 0 && !revision && isFetched) {
      setRevision(documentRevisionsData[0])
    }
  }, [isFetched, documentRevisionsData])

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
  }, [documentRevisionsData, revision])

  return (
    <>
      <Tabs defaultValue="dokumentvisning" className="h-full w-full">
        <TabsList className='w-full'>
          <TabsTrigger value="dokumentvisning">Dokumentvisning</TabsTrigger>
          <TabsTrigger value="kommentar">Kommentar</TabsTrigger>
          <RevisionsList
            documentRevisions={documentRevisionsData}
            onRevisionClick={(v) => { setRevision(v) }}
            caseDocument={caseDocument}
            setIsDialogOpen={setIsDialogOpen}
          />
        </TabsList>
        <TabsContent value="dokumentvisning">
          <div className='h-full w-full flex '>
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
        <TabsContent value="kommentar">
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
            <hr></hr>
            <MenubarItem
              className="focus:!bg-accent/0 hover:bg-transparent h-12 text-base text-muted-foreground"
            >
              <Button
                disabled={documentRevisions?.length === 0}
                variant='ghost'
                className='w-full'
                onClick={() => {
                  setIsDialogOpen(true)
                }}
              >
                Opret ny revision
              </Button>

            </MenubarItem>
          </MenubarContent>
        </MenubarMenu>
      </Menubar>
    </div>
  );
}

// for (int i • Ø; i revisions .Length; i")
// Revisions. Add(ne
// Revision*nber revisions. Length — i