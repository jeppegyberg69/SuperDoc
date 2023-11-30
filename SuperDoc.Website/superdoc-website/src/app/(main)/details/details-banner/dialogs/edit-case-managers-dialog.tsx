"use client";
import React from 'react';
import { Dialog, DialogClose, DialogContent, DialogHeader, DialogTitle } from "@/components/ui/dialog"
import { EditCaseManagersForm } from './edit-case-managers-form';
import { CaseDetails } from '@/models/case-details';

export type EditCaseManagersDialogProps = {
  isDialogOpen: boolean
  onOpenedChange: (value: boolean) => void;
  details: CaseDetails;
  onClose: () => void
};

export function EditCaseManagersDialog(props: EditCaseManagersDialogProps) {
  return (
    <Dialog open={props.isDialogOpen} onOpenChange={props.onOpenedChange}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Sagsbehandlere</DialogTitle>
          <EditCaseManagersForm onClose={props.onClose} closeDialog={() => props.onOpenedChange(false)} details={props.details} />
        </DialogHeader>
      </DialogContent>
    </Dialog>
  )
}