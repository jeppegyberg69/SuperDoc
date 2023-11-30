"use client";
import React from 'react';
import { Dialog, DialogContent, DialogHeader, DialogTitle } from "@/components/ui/dialog"
import { EditCaseManagersForm } from './edit-case-managers-form';
import { CaseDetails } from '@/models/case-details';
import { EditCaseDescriptionForm } from './edit-case-description-form';

export type EditCaseDescriptionDialogProps = {
  isDialogOpen: boolean
  onOpenedChange: (value: boolean) => void;
  details: CaseDetails;
  onClose: () => void
};

export function EditCaseDescriptionDialog(props: EditCaseDescriptionDialogProps) {

  return (
    <Dialog open={props.isDialogOpen} onOpenChange={props.onOpenedChange}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Rediger sagsdetaljer</DialogTitle>
          <EditCaseDescriptionForm onClose={props.onClose} closeDialog={() => props.onOpenedChange(false)} details={props.details} />
        </DialogHeader>
      </DialogContent>
    </Dialog>
  );
}