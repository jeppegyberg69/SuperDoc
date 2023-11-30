"use client"
import React, { useState } from 'react';
import { List, ListItem } from "@/common/list/list"
import { ListLayout } from "@/common/list-layout/list-layout"
import { SplitView } from "@/common/split-view/split-view"
import { DocumentTabs } from './document-tabs/document-tabs';
import { useDocuments, useDownloadRevisionFile } from '@/services/document-services';
import { CaseDetails } from '@/models/case-details';
import { DateTime } from 'luxon';
import { CaseDocument } from '@/models/document';
import { Button } from '@/components/ui/button';
import { CreateDocumentDialog } from './document-tabs/edit-dialog/create-document-dialog';
import { useQueryClient } from '@tanstack/react-query';
import { CaseServiceQueryKeys } from '@/services/case-service';

export type DocumentsProps = {
  details: CaseDetails;
};
const pdfUrlTemp = "https://www.africau.edu/images/default/sample.pdf";

export function Documents(props: DocumentsProps) {
  const queryClient = useQueryClient();
  const { data: documentData } = useDocuments(props.details.case.id)
  const [selectedDocument, setSelectedDocument] = useState<CaseDocument>();

  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const onDialogOpenedChanged = (openState) => setIsDialogOpen(openState);

  const onDocumentItemClick = (document: CaseDocument) => {

    setSelectedDocument(document)
  }

  const onDialogClose = () => {
    queryClient.invalidateQueries({ queryKey: [CaseServiceQueryKeys.getCaseManagers] });
  }


  const left = (
    <div className="h-full">
      <List horizontalGridline="enabled">
        {documentData.map((document) => (
          <li
            key={document.id}
            className="flex flex-1 py-4 hover:bg-accent/20 h-full"
            onClick={() => onDocumentItemClick(document)}
          >
            <ListItem>
              <div className="flex flex-1 flex-col">
                <span className='font-semibold'>{document.title}</span>
                <span className='italic text-xs text-muted-foreground'>{document.description}</span>
                <span className='text-xs text-muted-foreground'>{document.externalUsers.length > 0 && <>Eksterne brugere: {document.externalUsers.map((v) => v.emailAddress).join(', ')}</>}</span>
                <div className='flex flex-col'>
                  <span className='text-xs text-muted-foreground/70'>Oprettet: {DateTime.fromISO(document.dateCreated).toFormat("dd-LL-yyyy")} </span>
                  <span className='text-xs text-muted-foreground/70'>Sidst ændret: {DateTime.fromISO(document.dateCreated).toFormat("dd-LL-yyyy")} </span>
                </div>
              </div>
            </ListItem>
          </li>
        ))}
      </List>
    </div>
  )

  const list = (
    <SplitView
      left={left}
      right={<DocumentTabs pdfUrl={pdfUrlTemp} caseDocument={selectedDocument} />}
    />
  )

  const toolbar = (
    <div className='flex divide-x'>
      <h1 className="font-semibold text-xl self-center mr-4">Dokumenter</h1>
      <div className='px-2'>
        <Button variant='default' onClick={() => { onDialogOpenedChanged(true) }}>Opret dokument</Button>
        <CreateDocumentDialog onClose={onDialogClose} isDialogOpen={isDialogOpen} onOpenChanged={onDialogOpenedChanged} details={props.details}></CreateDocumentDialog>
      </div>
    </div>
  )

  return (
    <ListLayout
      isPanelLayout={false}
      toolbar={toolbar}
      list={list}
    />
  )
}