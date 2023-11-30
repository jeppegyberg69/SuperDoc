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
import { Skeleton } from '@/components/ui/skeleton';
import { Button } from '@/components/ui/button';

export type CaseOverviewTableProps = {};

export const columns: ColumnDef<Case, any>[] = [{
  id: "id",
  accessorKey: nameof<Case>('caseNumber'),
  header: "id",
  cell(props) {
    const data = props.getValue();
    return (
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
      <span className='data-column-title'>{data}</span>
    )
  },
},
{
  accessorKey: nameof<Case>('responsibleUser'),
  header: () => <span className='data-column-casemanagers'>Sagsansvarlig</span>,
  cell(props) {
    const user = props.getValue();
    return (
      <span className='data-column-casemanagers'>{[user.firstName, user.lastName].join(' ')}</span>
    )
  },
},
{
  accessorKey: nameof<Case>('caseManagers'),
  header: () => <span className='data-column-casemanagers'>Sagsbehandlere</span>,
  cell(props) {
    const caseManagers =
      props.getValue()
        .map(cm => {
          const fullName = [cm.firstName, cm.lastName].join(' ')
          return fullName.split(' ')
            .map(word => word[0])
            .filter(v => !!v)
            .slice(0, 2)
            .join('');
        });

    return (
      <span className='data-column-casemanagers'>{caseManagers.join(', ')}</span>
    )
  },
}];

export function CaseOverviewTable(props: CaseOverviewTableProps) {
  const [data, setData] = useState([])
  const [isDialogOpen, setIsDialogOpen] = useState(false);
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

  const onDialogOpenedChanged = (openState) => setIsDialogOpen(openState);

  const toolbar = (
    <div className='flex divide-x'>
      <h1 className="font-semibold text-xl px-2 self-center">Sager</h1>
      <div className='px-2'>
        <Button variant='default' onClick={() => { onDialogOpenedChanged(true) }}> Opret sag</Button>
        <CreateCaseDialog isDialogOpen={isDialogOpen} onOpenChanged={onDialogOpenedChanged} />
      </div>
    </div>
  );

  const loadingSkeleton = (
    <div>
      {[...Array(30)].map((_, index) => (
        <Skeleton key={index} className="w-full h-[20px] rounded-md my-6" />
      ))}
    </div>
  );


  return (
    <ListLayout
      toolbar={toolbar}
      list={data?.length === 0 ? loadingSkeleton : dataTable}
    />
  )
}