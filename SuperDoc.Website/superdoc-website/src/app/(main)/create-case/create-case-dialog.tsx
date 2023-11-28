"use client"
import React from 'react';
import { CreateCaseForm } from './create-case-form';
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger, DialogClose } from "@/components/ui/dialog"

export type CreateCaseDialogProps = {};

export function CreateCaseDialog(props: CreateCaseDialogProps) {

  return (
    <Dialog>
      <DialogTrigger className='h-full'>Open</DialogTrigger>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Opret sag</DialogTitle>
          <CreateCaseForm closeDialog={() => DialogClose}></CreateCaseForm>
        </DialogHeader>
      </DialogContent>
    </Dialog>
  )
}