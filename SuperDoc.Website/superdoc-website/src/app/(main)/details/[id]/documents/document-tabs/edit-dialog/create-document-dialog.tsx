import React from 'react';
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger, DialogClose } from "@/components/ui/dialog"
import { Button } from '@/components/ui/button';
import { CreateDocumentForm } from './create-document-form';
import { CaseDetails } from '@/models/case-details';

export type CreateDocumentDialogProps = {
  isDialogOpen: boolean;
  onOpenChanged: (value: boolean) => void;
  details: CaseDetails
  onClose: () => void
};

export function CreateDocumentDialog(props: CreateDocumentDialogProps) {

  return (
    <Dialog open={props.isDialogOpen} onOpenChange={props.onOpenChanged}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Opret dokument</DialogTitle>
          <hr />
          <CreateDocumentForm onClose={() => props.onClose?.()} closeDialog={() => props.onOpenChanged?.(false)} details={props.details}></CreateDocumentForm>
        </DialogHeader>
      </DialogContent>
    </Dialog>
  );
}