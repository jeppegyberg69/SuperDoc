"use client";
import React, { useRef, useState } from 'react';
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
import { createDocument, uploadRevision } from '@/services/document-services';
import { CaseDocument } from '@/models/document';
import { toast } from '@/components/ui/use-toast';

// valider en email ud fra følgende pattern: 
// test@mail.dk;test@mail.dk ELLER test@mail.dk
const emailPattern = /^[\w.-]+@[a-z\d.-]+\.[a-z]{2,}$/i;

const customEmailListValidation = (value) => {
  const emails = value.split(';');
  return emails.every(email => emailPattern.test(email.trim()));
};

const formSchema = z.object({
  emails: z.string().refine(customEmailListValidation, {
    message: 'Der skal være mindst 1 email',
  })
})

export type UploadRevisionFormProps = {
  closeDialog: () => void;
  caseDocument: CaseDocument
};

export function UploadRevisionForm(props: UploadRevisionFormProps) {
  const fileInputRef = useRef<HTMLInputElement>();
  const [selectedFile, setSelectedFile] = useState<{ file: Blob, fileName: string }>();
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      emails: "",
    },
  })

  const onFileInputChange = async (event: React.ChangeEvent<HTMLInputElement>) => {
    const input = event.target;
    const files = input.files;

    if (files.length > 0) {
      const file = files[0];

      // Convert the selected file to a Blob
      const blob = new Blob([file], { type: file.type });
      setSelectedFile({ file: blob, fileName: file.name })
    }
    else {
      toast({
        title: "Advarsel",
        description: "Ingen filer fundet."
      })
    }
  }

  async function onSubmit(values: z.infer<typeof formSchema>) {
    if (!selectedFile) {
      toast({
        title: "Fejl",
        description: "Kan ikke oprette en revision uden et dokument."
      })
      return;
    }

    await uploadRevision({
      documentId: props.caseDocument.id,
      externalUserEmails: values.emails,
      file: selectedFile.file,
      fileName: selectedFile.fileName
    });

    props.closeDialog();
  }


  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-8">
        <FormField
          control={form.control}
          name="emails"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Email adresser</FormLabel>
              <FormControl>
                <Input placeholder="test@mail.dk;test@mail1.dk;..." {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <div className='w-full'>
          <Button
            variant='default'
            type='button'
            onClick={() => {
              fileInputRef.current.click();
            }}
          >
            Vælg fil
          </Button>
        </div>

        <input
          ref={fileInputRef}
          type='file'
          accept="application/pdf"
          style={{ display: "none" }}
          onChange={onFileInputChange}
        />

        <Button type="submit">Gem</Button>
      </form>
    </Form>
  );
}