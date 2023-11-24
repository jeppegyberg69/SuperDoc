"use client";
import React, { useEffect, useState } from 'react';
import { ListLayout } from "@/common/list-layout/list-layout"
import { ColumnDef } from "@tanstack/react-table"
import { nameof } from '@/common/nameof/nameof';
import { DataTable } from '@/components/ui/data-table';
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
import { getCases } from '@/services/case-service';
import { Case } from '@/models/case';
import { useIsMounted } from '@/common/hooks/use-is-mounted';

export type CaseOverviewTableProps = {};

// This type is used to define the shape of our data.
// You can use a Zod schema here if you want.
export type Payment = {
  id: string
  invoice: string,
  paymentStatus: string,
  totalAmount: string,
  paymentMethod: string,
}

const formSchema = z.object({
  username: z.string().min(2, {
    message: "Username must be at least 2 characters.",
  }),
})

export const columns: ColumnDef<Case>[] = [
  {
    accessorKey: nameof<Case>('id'),
    header: "id",
  },
  {
    accessorKey: nameof<Case>('title'),
    header: "Title",
  },
];

export function CaseOverviewTable(props: CaseOverviewTableProps) {
  const form = useForm()
  const [data, setData] = useState([])

  const isMounted = useIsMounted()

  useEffect(() => {
    if (isMounted()) {
      getCases().then((data) => setData(data));
    }
  }, [isMounted()])

  const dataTable = (
    <DataTable
      data={data}
      columns={columns}
    />
  )

  const createCaseDialog = () => {
    const form = useForm<z.infer<typeof formSchema>>({
      resolver: zodResolver(formSchema),
      defaultValues: {
        username: "",
      },
    })

    function onSubmit(values: z.infer<typeof formSchema>) {
      console.log(values)
    }

    const dialogContent = (
      <Form {...form}>
        <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-8">
          <FormField
            control={form.control}
            name="username"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Username</FormLabel>
                <FormControl>
                  <Input placeholder="shadcn" {...field} />
                </FormControl>
                <FormDescription>
                  This is your public display name.
                </FormDescription>
                <FormMessage />
              </FormItem>
            )}
          />
          <Button type="submit">Submit</Button>
        </form>
      </Form>
    )

    return (
      <Dialog>
        <DialogTrigger className='h-full'>Open</DialogTrigger>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Opret sag</DialogTitle>
            {dialogContent}
          </DialogHeader>
        </DialogContent>
      </Dialog>
    )
  }


  const toolbar = (
    <div className='flex divide-x'>
      <h1 className="font-semibold text-xl px-2">Sager</h1>
      <div className='px-2'>
        {createCaseDialog()}
      </div>
    </div>
  );


  return (
    <ListLayout
      toolbar={toolbar}
      list={dataTable}
    />
  )
}

const invoices = [
  {
    id: "INV001",
    invoice: "INV001",
    paymentStatus: "Paid",
    totalAmount: "$250.00",
    paymentMethod: "Credit Card",
  },
  {
    id: "INV002",
    invoice: "INV002",
    paymentStatus: "Pending",
    totalAmount: "$150.00",
    paymentMethod: "PayPal",
  },
  {
    id: "INV003",
    invoice: "INV003",
    paymentStatus: "Unpaid",
    totalAmount: "$350.00",
    paymentMethod: "Bank Transfer",
  },
  {
    id: "INV004",
    invoice: "INV004",
    paymentStatus: "Paid",
    totalAmount: "$450.00",
    paymentMethod: "Credit Card",
  },
  {
    id: "INV005",
    invoice: "INV005",
    paymentStatus: "Paid",
    totalAmount: "$550.00",
    paymentMethod: "PayPal",
  },
  {
    id: "INV006",
    invoice: "INV006",
    paymentStatus: "Pending",
    totalAmount: "$200.00",
    paymentMethod: "Bank Transfer",
  },
  {
    id: "INV007",
    invoice: "INV007",
    paymentStatus: "Unpaid",
    totalAmount: "$300.00",
    paymentMethod: "Credit Card",
  },
]