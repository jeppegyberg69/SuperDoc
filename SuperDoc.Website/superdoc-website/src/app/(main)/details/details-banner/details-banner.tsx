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

export type DetailsBannerProps = {
  details: CaseDetails;
};

export function DetailsBanner(props: DetailsBannerProps) {
  const [isDialogOpen, setIsDialogOpen] = useState(false)
  const onDialogOpenedChanged = (openState) => setIsDialogOpen(openState);
  const queryClient = useQueryClient();
  const onDialogClose = () => {
    console.log('dsa')
    queryClient.invalidateQueries({queryKey: ['snowball']});
  }

  
  if (!props.details)
    return <span></span>
  return (
    <PageBanner>
      <BannerItemLayout>
        <BannerItem
          className='w-[200px]'
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
          onClick={() => { setIsDialogOpen(true) }}
        />
        
        <EditCaseManagersDialog onClose={onDialogClose} details={props.details} isDialogOpen={isDialogOpen} onOpenedChange={onDialogOpenedChanged} />

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