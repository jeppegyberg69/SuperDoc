"use client"
import React, { useState } from 'react';
import { CreateCaseForm } from './create-case-form';
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger, DialogClose } from "@/components/ui/dialog"
import { Button } from '@/components/ui/button';

export type CreateCaseDialogProps = {
  isDialogOpen: boolean;
  onOpenChanged: (value: boolean) => void;
};

export function CreateCaseDialog(props: CreateCaseDialogProps) {
  return (
    <Dialog open={props.isDialogOpen} onOpenChange={props.onOpenChanged}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Opret sag</DialogTitle>
          <hr />
          <CreateCaseForm closeDialog={() => DialogClose}></CreateCaseForm>
        </DialogHeader>
      </DialogContent>
    </Dialog>
  )
}