"use client"
import React from 'react';
import { Button } from '@/components/ui/button';
import { useForm } from 'react-hook-form';

import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage, } from "@/components/ui/form"
import { zodResolver } from "@hookform/resolvers/zod"
import * as z from "zod"
import { CaseDetails } from '@/models/case-details';
import { createCase } from '@/services/edit-case-services';
import { Input } from '@/components/ui/input';


const formSchema = z.object({
  title: z.string().min(1, {
    message: "Titlen skal være mindst 1 karakter langt",
  }),
  description: z.string().max(1024, {
    message: "Beskrivelsen må højst være 1024 karakter langt",
  })
})

export type EditCaseDescriptionFormProps = {
  details: CaseDetails;
  closeDialog: () => void;
  onClose: () => void
};

export function EditCaseDescriptionForm(props: EditCaseDescriptionFormProps) {
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      title: props.details.case.title ?? "",
      description: props.details.case.description ?? "",
    },
  })

  async function onSubmit(values: z.infer<typeof formSchema>) {
    await createCase({
      caseId: props.details.case.id,
      responsibleUserId: props.details.case.responsibleUser.id,
      caseManagersId: [...props.details.case.caseManagers.map(v => v.id)],

      // new values
      description: values.description,
      title: values.title,
    });

    // close dialog and push user into the case that was just created
    props.onClose();
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
                <Input placeholder='Sagstitel' {...field} />
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
                <Input placeholder="Beskrivelse af sagen" {...field} />
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


