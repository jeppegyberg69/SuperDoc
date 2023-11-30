"use client"
import React, { useMemo, useState } from 'react';
import { Button } from '@/components/ui/button';
import { useForm } from 'react-hook-form';
import { useQuery } from '@tanstack/react-query';
import { cn } from "@/lib/utils"

import { Checkbox } from "@/components/ui/checkbox"
import { getCaseManagers } from '@/services/case-service';

import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage, } from "@/components/ui/form"
import { Popover, PopoverContent, PopoverTrigger, } from "@/components/ui/popover"
import { Check, ChevronsUpDown } from "lucide-react"
import { zodResolver } from "@hookform/resolvers/zod"
import * as z from "zod"
import { getWebSession } from '@/common/session-context/session-context';
import { formatCheckboxSelectedValues } from '@/app/(main)/create-case/create-case-form';
import {
  Command,
  CommandEmpty,
  CommandGroup,
  CommandInput,
  CommandItem,
} from "@/components/ui/command"
import { CaseDetails } from '@/models/case-details';
import { createCase } from '@/services/edit-case-services';
import { buildConfig } from '@/models/webservice/base-url';

const formSchema = z.object({
  caseManagers: z.array(z.string()).refine((value) => value.some((item) => item), {
    message: "Du skal vælge mindst 1 sagsbehandler fra listen.",
  }),
  caseResponsible: z.string().min(0, {
    message: ''
  }).optional()
})

export type EditCaseManagersFormProps = {
  details: CaseDetails;
  closeDialog: () => void;
  onClose: () => void
};

export function EditCaseManagersForm(props: EditCaseManagersFormProps) {
  const userId = getWebSession().user?.id;
  const [open, setOpen] = useState(false)
  const [value, setValue] = useState(props.details.case.responsibleUser.id)
  const { data, isPending, isError, error } = useQuery({
    queryKey: [`${buildConfig.API}/api/Case/GetCaseManagers`, userId],
    async queryFn() {
      return await getCaseManagers();
    }
  })

  const dataProvider = useMemo(() => {
    // const caseManagersData = data?.filter(v => v.id !== userId);
    return data ? data : []
  }, [data]);

  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      caseResponsible: props.details.case.responsibleUser.id ?? '',
      caseManagers: props.details.case.caseManagers.length !== 0 ? props.details.case.caseManagers?.map(id => id.id) : [],
    },
  })

  async function onSubmit(values: z.infer<typeof formSchema>) {
    await createCase({
      caseId: props.details.case.id,
      description: props.details.case.description,
      title: props.details.case.title,

      // new values
      responsibleUserId: value,
      // caseManagersId: [...values.caseManagers, userId]
      caseManagersId: [...values.caseManagers] // Always include the user who is logged in in the caseManagers array
    });

    // close dialog and push user into the case that was just created
    props.onClose();
    props.closeDialog();
  }

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
          disabled={(userId !== props.details.case.responsibleUser.id)}
          name="caseResponsible"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Sagsansvarlig</FormLabel>
              <FormControl>
                <Popover open={open} onOpenChange={setOpen}>
                  <PopoverTrigger asChild>
                    <Button
                      variant="outline"
                      disabled={(userId !== props.details.case.responsibleUser.id)}
                      role="combobox"
                      aria-expanded={open}
                      className="w-full justify-between"
                    >
                      {value
                        ? dataProvider.find((caseResponsible) => caseResponsible.id === value)?.firstName
                        : "Vælg Sagsansvarlig"}
                      <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
                    </Button>
                  </PopoverTrigger>
                  <PopoverContent className="w-[200px] p-0">
                    <Command>
                      <CommandGroup>
                        {dataProvider.map((item) => (
                          <CommandItem
                            key={item.id}
                            value={item.id}
                            onSelect={(currentValue) => {
                              setValue(currentValue === value ? "" : currentValue)
                              setOpen(false)
                            }}
                          >
                            <Check
                              className={cn(
                                "mr-2 h-4 w-4",
                                value === item.id ? "opacity-100" : "opacity-0"
                              )}
                            />
                            {item.firstName}
                          </CommandItem>
                        ))}
                      </CommandGroup>
                    </Command>
                  </PopoverContent>
                </Popover>
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
                        disabled={(userId !== props.details.case.responsibleUser.id)}
                        className={cn("w-full justify-between", !field.value && "text-muted-foreground")}
                      >
                        {field.value?.length > 0
                          ? dataProvider.find(
                            (item) => field.value.some(v => item.id === v)
                          )?.firstName + formatCheckboxSelectedValues(field.value)
                          : "Vælg sagsbehandlere"}
                        <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
                      </Button>
                    </FormControl>
                  </PopoverTrigger>

                  <PopoverContent className="w-[200px] p-0">
                    {dataProvider.map((item) => (
                      <FormField
                        key={item.id}
                        control={form.control}
                        disabled={(userId !== props.details.case.responsibleUser.id)}
                        name="caseManagers"
                        render={({ field }) => {
                          return (
                            <FormItem
                              key={item.id}
                              className="flex flex-row dataProvider-start space-x-3 space-y-0 m-2"
                            >
                              <FormControl>
                                <Checkbox
                                  checked={field.value?.includes(item.id)}
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

        <Button type="submit">Gem</Button>
      </form>
    </Form>
  );
}