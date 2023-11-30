"use client";
import React from 'react';
import { useForm } from 'react-hook-form';
import { useRouter } from 'next/navigation';
import { useQuery } from '@tanstack/react-query';
import { cn } from "@/lib/utils"

import { Checkbox } from "@/components/ui/checkbox"
import { getCaseManagers } from '@/services/case-service';
import { createCase } from '@/services/edit-case-services'
import { CaseDetails } from '@/models/case-details';

import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage, } from "@/components/ui/form"
import { Button } from '@/components/ui/button';
import { Input } from "@/components/ui/input"
import { zodResolver } from "@hookform/resolvers/zod"
import * as z from "zod"
import { getWebSession } from '@/common/session-context/session-context';
import { createDocument } from '@/services/document-services';

const formSchema = z.object({
  title: z.string().min(1, {
    message: "Titlen skal være mindst 1 karakter langt",
  }),
  description: z.string().max(1024, {
    message: "Beskrivelsen må højst være 1024 karakter langt",
  }),
})
export type CreateDocumentFormProps = {
  closeDialog: () => void;
  details: CaseDetails;
};

export function CreateDocumentForm(props: CreateDocumentFormProps) {
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      title: "",
      description: "",
    },
  })

  async function onSubmit(values: z.infer<typeof formSchema>) {
    await createDocument({
      caseId: props.details.case.id,
      title: values.title,
      description: values.description,
    });

    props.closeDialog();
  }


  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-8">
        <FormField
          control={form.control}
          name="title"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Titel</FormLabel>
              <FormControl>
                <Input placeholder='Titel på dokument' {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        <FormField
          control={form.control}
          name="description"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Beskrivelse</FormLabel>
              <FormControl>
                <Input placeholder="Beskrivelse af dokumentet" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        <Button type="submit">Submit</Button>
      </form>
    </Form>
  );
}


