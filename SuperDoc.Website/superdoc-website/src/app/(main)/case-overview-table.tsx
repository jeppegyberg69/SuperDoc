"use client";
import React from 'react';
import Image from 'next/image';
import { ListLayout } from "@/common/list-layout/list-layout"
import { ColumnDef } from "@tanstack/react-table"
import {
  Table,
  TableBody,
  TableCaption,
  TableCell,
  TableFooter,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table"
import { nameof } from '@/common/nameof/nameof';
import { DataTable } from '@/components/ui/data-table';

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

export const columns: ColumnDef<Payment>[] = [
  {
    accessorKey: nameof<Payment>('invoice'),
    header: "Invoice",
  },
  {
    accessorKey: nameof<Payment>('paymentMethod'),
    header: "Payment method",
  },
  {
    accessorKey: nameof<Payment>('paymentStatus'),
    header: "Payment status",
  },
]
export function CaseOverviewTable(props: CaseOverviewTableProps) {
  const d = (
    <DataTable
      data={invoices}
      columns={columns}
    />
  )

  return (
    <ListLayout
      toolbar={(<h1 className="font-semibold text-xl">Sager</h1>)}
      list={d}
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