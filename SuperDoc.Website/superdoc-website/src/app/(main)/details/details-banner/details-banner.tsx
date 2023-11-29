"use client"
import { BannerItem, BannerItemLayout, PageBanner } from '@/common/page-layout/page-banner';
import { CaseDetails } from '@/models/case-details';
import { CaseManagers } from '@/models/case-manager';
import React, { useEffect, useState } from 'react';
import { EditCaseManagersDialog } from './dialogs/edit-case-managers-dialog';
import { useIsMounted } from '@/common/hooks/use-is-mounted';
import { Button } from '@/components/ui/button';
import { CreateCaseDialog } from '../../create-case/create-case-dialog';
import { useQueryClient } from '@tanstack/react-query';
import { EditCaseDescriptionDialog } from './dialogs/edit-case-description-dialog';

export type DetailsBannerProps = {
  details: CaseDetails;
};

export function DetailsBanner(props: DetailsBannerProps) {
  const [caseManagerDialogOpen, setCaseManagerDialogOpen] = useState(false)
  const [caseInfoDialogOpen, setCaseInfoDialogOpen] = useState(false)
  const caseInfoDialogOpenChanged = (openState) => setCaseInfoDialogOpen(openState);
  const caseManagerDialogOpenChanged = (openState) => setCaseManagerDialogOpen(openState);

  const queryClient = useQueryClient();
  const onDialogClose = () => {
    queryClient.invalidateQueries({ queryKey: ['snowball'] });
  }


  if (!props.details)
    return <span></span>
  return (
    <PageBanner>
      <BannerItemLayout>
        <BannerItem
          className='w-[200px] !cursor-default'
          label='Sagsnummer'
          value={props.details?.case?.caseNumber}
        />
        <BannerItem
          label='Sagsinformationer'
          value={(
            <div>

              <div className=''>
                {props.details.case.title}
              </div>
              <div className='text-xs italic text-muted/90'>
                {props.details.case.description}
              </div>
            </div>
          )}
          className='flex-1'
          onClick={() => { caseInfoDialogOpenChanged(true) }}
        />
        <BannerItem
          className='flex-1'
          label='Sagsbehandlere'
          value={(
            <div>
              <div className='font-semibold'>
                {[props.details?.case?.responsibleUser?.firstName, props.details?.case?.responsibleUser?.lastName].filter(Boolean).join(' ')}
              </div>
              <div className='text-xs italic text-muted/90'>
                {formatStringValues(props.details?.case?.caseManagers ?? [])}
              </div>
            </div>
          )}
          onClick={() => { caseManagerDialogOpenChanged(true) }}
        />

        <EditCaseManagersDialog onClose={onDialogClose} details={props.details} isDialogOpen={caseManagerDialogOpen} onOpenedChange={caseManagerDialogOpenChanged} />
        <EditCaseDescriptionDialog onClose={onDialogClose} details={props.details} isDialogOpen={caseInfoDialogOpen} onOpenedChange={caseInfoDialogOpenChanged} />
      </BannerItemLayout>
    </PageBanner>
  );
}


function formatStringValues(caseManagers: CaseManagers[]): string {
  const firstdata = caseManagers[0];

  return caseManagers.length > 1
    ? `${[firstdata.firstName, firstdata.lastName].join(' ')} [+${caseManagers.length - 1}]`
    : `${[firstdata.firstName, firstdata.lastName].join(' ')}`;
}