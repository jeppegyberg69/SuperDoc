"use client";
import React from 'react';
import { Dialog, DialogContent, DialogHeader, DialogTitle } from "@/components/ui/dialog"
import { EditCaseManagersForm } from './edit-case-managers-form';
import { CaseDetails } from '@/models/case-details';
import { EditCaseInformationForm } from './edit-case-information-form';

export type EditCaseInformationDialogProps = {
  isDialogOpen: boolean
  onOpenedChange: (value: boolean) => void;
  details: CaseDetails;
  onClose: () => void
};

export function EditCaseInformationDialog(props: EditCaseInformationDialogProps) {
  return (
    <Dialog open={props.isDialogOpen} onOpenChange={props.onOpenedChange}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Rediger sagsdetaljer</DialogTitle>
          <hr />
          <EditCaseInformationForm onClose={props.onClose} closeDialog={() => props.onOpenedChange(false)} details={props.details} />
        </DialogHeader>
      </DialogContent>
    </Dialog>
  );
}