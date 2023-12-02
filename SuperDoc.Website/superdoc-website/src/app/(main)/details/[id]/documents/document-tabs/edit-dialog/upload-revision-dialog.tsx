import React from 'react';
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger, DialogClose } from "@/components/ui/dialog"
import { Button } from '@/components/ui/button';
import { CreateDocumentForm } from './create-document-form';
import { CaseDetails } from '@/models/case-details';
import { CaseDocument } from '@/models/document';
import { UploadRevisionForm } from './upload-revision-form';

export type UploadRevisionDialogProps = {
  isDialogOpen: boolean;
  onOpenChanged: (value: boolean) => void;
  caseDocument: CaseDocument;
  onClose: () => void
};

export function UploadRevisionDialog(props: UploadRevisionDialogProps) {

  return (
    <Dialog open={props.isDialogOpen} onOpenChange={props.onOpenChanged}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Opret dokument</DialogTitle>
          <hr />
          <UploadRevisionForm closeDialog={() => props.onOpenChanged?.(false)} caseDocument={props.caseDocument}></UploadRevisionForm>
        </DialogHeader>
      </DialogContent>
    </Dialog>
  );
}