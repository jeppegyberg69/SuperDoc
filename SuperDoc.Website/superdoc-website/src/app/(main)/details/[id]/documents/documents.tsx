"use client"
import React, { useEffect, useState } from 'react';
import { List, ListItem } from "@/common/list/list"
import { ListLayout } from "@/common/list-layout/list-layout"
import { SplitView } from "@/common/split-view/split-view"
import { DocumentTabs } from './document-tabs/document-tabs';
import { DocumentServiceQueryKeys, useDocuments } from '@/services/document-services';
import { CaseDetails } from '@/models/case-details';
import { DateTime } from 'luxon';
import { CaseDocument } from '@/models/document';
import { Button } from '@/components/ui/button';
import { CreateDocumentDialog } from './document-tabs/edit-dialog/create-document-dialog';
import { useQueryClient } from '@tanstack/react-query';
import { CaseServiceQueryKeys } from '@/services/case-service';
import { getWebSession } from '@/common/session-context/session-context';
import { Roles } from '@/common/access-control/access-control';

export type DocumentsProps = {
  details: CaseDetails;
};
export function Documents(props: DocumentsProps) {
  const queryClient = useQueryClient();
  const { data: documentData, isFetchedAfterMount } = useDocuments(props.details.case.id)
  const [selectedDocument, setSelectedDocument] = useState<CaseDocument>();

  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const onDialogOpenedChanged = (openState) => setIsDialogOpen(openState);

  const onDocumentItemClick = (document: CaseDocument) => {
    setSelectedDocument(document)
  }

  useEffect(() => {
    if (documentData && documentData.length > 0 && isFetchedAfterMount && !selectedDocument) {
      setSelectedDocument(documentData[0])
    }
  }, [isFetchedAfterMount, selectedDocument])



  const onDialogClose = () => {
    queryClient.invalidateQueries({ queryKey: [DocumentServiceQueryKeys.getDocuments] });
  }


  const left = (
    <div className="h-full">
      <List horizontalGridline="enabled">
        {documentData.map((document) => (
          <li
            key={document.id}
            data-selected={selectedDocument?.id === document?.id}
            className="flex flex-1 py-4 hover:bg-gray-500/10 h-full cursor-pointer data-[selected=true]:bg-primary/20"
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
      right={<DocumentTabs caseDocument={selectedDocument} />}
    />
  )

  const toolbar = (
    <div className='flex divide-x'>
      <h1 className="font-semibold text-xl self-center mr-4">Dokumenter</h1>
      {getWebSession().user.role !== Roles.User && (
        <div className='px-2'>
          <Button variant='default' onClick={() => { onDialogOpenedChanged(true) }}>Opret dokument</Button>
          <CreateDocumentDialog onClose={onDialogClose} isDialogOpen={isDialogOpen} onOpenChanged={onDialogOpenedChanged} details={props.details}></CreateDocumentDialog>
        </div>
      )}
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