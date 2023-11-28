"use client";
import React, { useEffect, useState } from 'react';
import { ListLayout } from "@/common/list-layout/list-layout"
import { ColumnDef } from "@tanstack/react-table"
import { nameof } from '@/common/nameof/nameof';
import { DataTable } from '@/components/ui/data-table';
import { getCases } from '@/services/case-service';
import { Case } from '@/models/case';
import { useIsMounted } from '@/common/hooks/use-is-mounted';
import { CreateCaseDialog } from './create-case/create-case-dialog';

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

export const columns: ColumnDef<Case>[] = [
  {
    id: "id",
    accessorKey: nameof<Case>('id'),
    header: "id",
    cell(props) {
      const data = props.getValue();
      return (
        //@ts-ignore
        <span className='data-column-id'>{data}</span>
      )
    },
  },
  {
    id: "title",
    accessorKey: nameof<Case>('title'),
    header: "Title",
    cell(props) {
      const data = props.getValue();
      return (
        //@ts-ignore
        <span className='data-column-title'>{data}</span>
      )
    },
  },
  {
    accessorKey: nameof<Case>('caseManagers'),
    header: () => <span className='data-column-casemanagers'>Sagsbehandlere</span>,
    cell(props) {
      const caseManagers =
        props.getValue()
          //@ts-ignore
          .map(cm => {
            const fullName = [cm.firstName, cm.lastName].join(' ')
            return fullName.split(' ')
              .map(word => word[0])
              .filter(v => !!v)
              .slice(0, 2)
              .join('');
          });

      return (
        <span className='data-column-casemanagers'>{caseManagers.join(',')}</span>
      )
    },
    size: 50

  },

];

export function CaseOverviewTable(props: CaseOverviewTableProps) {
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

  const toolbar = (
    <div className='flex divide-x'>
      <h1 className="font-semibold text-xl px-2">Sager</h1>
      <div className='px-2'>
        <CreateCaseDialog />
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