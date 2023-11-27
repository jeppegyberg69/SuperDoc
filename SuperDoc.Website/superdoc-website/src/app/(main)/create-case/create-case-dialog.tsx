import React from 'react';
import { Button } from '@/components/ui/button';
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog"
import { useForm } from 'react-hook-form';
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form"
import { Input } from "@/components/ui/input"
import { zodResolver } from "@hookform/resolvers/zod"
import * as z from "zod"
import { Case } from '@/models/case';
import { CreateCaseForm } from './create-case-form';

export type CreateCaseDialogProps = {};

export function CreateCaseDialog(props: CreateCaseDialogProps) {

  return (
    <Dialog>
      <DialogTrigger className='h-full'>Open</DialogTrigger>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Opret sag</DialogTitle>
          <CreateCaseForm></CreateCaseForm>
        </DialogHeader>
      </DialogContent>
    </Dialog>
  )
}