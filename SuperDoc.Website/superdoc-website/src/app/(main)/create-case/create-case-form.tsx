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
import { Popover, PopoverContent, PopoverTrigger, } from "@/components/ui/popover"
import { Button } from '@/components/ui/button';
import { Input } from "@/components/ui/input"
import { ChevronsUpDown } from "lucide-react"
import { zodResolver } from "@hookform/resolvers/zod"
import * as z from "zod"
import { getWebSession } from '@/common/session-context/session-context';
import { toast } from '@/components/ui/use-toast';
import { buildConfig } from '@/models/webservice/base-url';

const formSchema = z.object({
  title: z.string().min(1, {
    message: "Titlen skal være mindst 1 karakter langt",
  }),
  description: z.string().max(1024, {
    message: "Beskrivelsen må højst være 1024 karakter langt",
  }),
  caseManagers: z.array(z.string()).refine((value) => value.some((item) => item), {
    message: "Du skal vælge mindst 1 sagsbehandler fra listen.",
  }),
})

export type CreateCaseFormProps = {
  closeDialog: () => void;
};

export function CreateCaseForm(props: CreateCaseFormProps) {
  const router = useRouter();
  const userId = getWebSession().user?.id
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      title: "",
      description: "",
      caseManagers: [userId],
    },
  })

  const { data: items, isPending, isError, error } = useQuery({
    queryKey: [`${buildConfig.API}/api/Case/GetCaseManagers`, userId],
    async queryFn() {
      return await getCaseManagers();
    }
  })


  async function onSubmit(values: z.infer<typeof formSchema>) {
    const data: CaseDetails = await createCase({
      title: values.title,
      description: values.description,
      caseManagersId: [...values.caseManagers]
    }).catch((err) => {
      toast({
        title: "Fejl",
        description: "Kunne ikke oprette sagen. Prøv igen senere."
      })
      return null;
    });

    if (!data) {
      return;
    }

    // close dialog and push user into the case that was just created
    props.closeDialog();
    router.push('/details/' + data.case.id)
  }

  // make sure data is loaded..
  if (isPending) {
    return <span>Loading...</span>
  }

  if (isError) {
    return <span>Error: {error.message}</span>
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

        <FormField
          control={form.control}
          name="caseManagers"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Sagsbehandlere</FormLabel>
              <FormControl>
                <Popover>
                  <PopoverTrigger asChild>
                    <FormControl>
                      <Button
                        variant="outline"
                        role="combobox"
                        className={cn("w-full justify-between", !field.value && "text-muted-foreground")}
                      >
                        {field.value?.length > 0
                          ? items.find(
                            (item) => field.value.some(v => item.id === v)
                          )?.firstName + formatCheckboxSelectedValues(field.value)
                          : "Vælg sagsbehandlere"}
                        <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
                      </Button>
                    </FormControl>
                  </PopoverTrigger>
                  <PopoverContent className="w-[200px] p-0">
                    {items.map((item) => (
                      <FormField
                        key={item.id}
                        control={form.control}
                        name="caseManagers"
                        render={({ field }) => {
                          return (
                            <FormItem
                              key={item.id}
                              className="flex flex-row items-start space-x-3 space-y-0 m-2"
                            >
                              <FormControl>
                                <Checkbox
                                  checked={field.value?.includes(item.id)}
                                  disabled={item.id === userId}
                                  onCheckedChange={(checked) => {
                                    return checked
                                      ? field.onChange([...field.value, item.id])
                                      : field.onChange(
                                        field.value?.filter(
                                          (value) => value !== item.id
                                        )
                                      )
                                  }}
                                />
                              </FormControl>
                              <FormLabel className="font-normal">
                                {item.firstName}
                              </FormLabel>
                            </FormItem>
                          )
                        }}
                      />
                    ))}
                  </PopoverContent>
                </Popover>
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        <Button type="submit">Opret</Button>
      </form>
    </Form>
  );
}



// Returns a string with the following format, containing the extra "items" in the list:
//                                       [+(number)]
// string might look like the following: [+5]
export function formatCheckboxSelectedValues(items: string[]): string {
  return items.length > 1
    ? ` [+${items.length - 1}]`
    : '';
}