"use client";
import React, { useEffect, useRef, useState } from 'react';
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"
import { PdfViewer } from '@/common/pdf-viewer/pdf-viewer';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSortDown } from "@fortawesome/free-solid-svg-icons/faSortDown";
import { getRevisionFile, useDocumentRevisions } from '@/services/document-services';
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
  const [selectedFile, setSelectedFile] = useState(null);
  const { data } = useDocumentRevisions(caseDocument?.id ?? '');
  const [newUrl, setNewUrl] = useState("");

  const onFileInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const input = event.target;
    const files = input.files;

    if (files.length > 0) {
      const file = files[0];
      setSelectedFile(file);

      // Convert the selected file to a Blob
      const blob = new Blob([file], { type: file.type });

      // Now you can pass 'blob' to your web service
      // For example, you might want to send it using fetch or Axios
      // fetch('your-web-service-url', { method: 'POST', body: blob });

    }

    console.log(files);
  }

  const handleFileUpload = (event) => {
    const input = event.target;
    const files = input.files;

    if (files.length > 0) {
      const file = files[0];
      setSelectedFile(file);

      // Convert the selected file to a Blob
      const blob = new Blob([file], { type: file.type });

      // Now you can pass 'blob' to your web service
      // For example, you might want to send it using fetch or Axios
      // fetch('your-web-service-url', { method: 'POST', body: blob });
    }
  };
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
          onFileInputChange={onFileInputChange}
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
  onRevisionClick: (revision: DocumentRevision) => void;
  onFileInputChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
};

export function RevisionsList({ documentRevisions, onRevisionClick, onFileInputChange }: RevisionsListProps) {
  const fileInputRef = useRef<HTMLInputElement>();
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
            <MenubarItem
              className="!focus:bg-transparent !hover:bg-transparent h-12 text-base text-muted-foreground"
            >
              <Button
                variant='ghost'
                className='w-full'
                onClick={() => {
                  fileInputRef.current.click();
                }}
              >
                Opret ny revision
              </Button>

            </MenubarItem>
          </MenubarContent>
        </MenubarMenu>
      </Menubar>
      <input
        ref={fileInputRef}
        type='file'
        accept="application/pdf"
        style={{ display: "none" }}
        onChange={onFileInputChange}
      />
    </div>
  );
}