"use client";

import React, { useState } from 'react';
import { BannerItem, BannerItemLayout, PageBanner } from '@/common/page-layout/page-banner';
import { CaseDetails } from '@/models/case-details';
import { CaseManagers } from '@/models/case-manager';
import { EditCaseManagersDialog } from './dialogs/edit-case-managers-dialog';
import { useQueryClient } from '@tanstack/react-query';
import { EditCaseInformationDialog } from './dialogs/edit-case-information-dialog';
import { CaseServiceQueryKeys } from '@/services/case-service';
import { getWebSession } from '@/common/session-context/session-context';
import { Roles } from '@/common/access-control/access-control';

export type DetailsBannerProps = {
  details: CaseDetails;
};

export function DetailsBanner(props: DetailsBannerProps) {
  const queryClient = useQueryClient();

  const [caseManagerDialogOpen, setCaseManagerDialogOpen] = useState(false)
  const [caseInfoDialogOpen, setCaseInfoDialogOpen] = useState(false)
  const caseInfoDialogOpenChanged = (openState) => setCaseInfoDialogOpen(openState);
  const caseManagerDialogOpenChanged = (openState) => setCaseManagerDialogOpen(openState);

  const onDialogClose = () => {
    queryClient.invalidateQueries({ queryKey: [CaseServiceQueryKeys.useGetDetails] });
  }

  if (!props.details) {
    return <span></span>;
  }

  return (
    <PageBanner>
      <BannerItemLayout>
        <BannerItem
          className='w-[200px] !cursor-default'
          label='Sagsnummer'
          value={props.details.case.caseNumber}
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
          className={`flex-1 ${getWebSession().user.role !== Roles.User ? '' : '!cursor-default'}`}
          onClick={() => {
            if (getWebSession().user.role !== Roles.User) {
              caseInfoDialogOpenChanged(true)
            }
          }}
        />
        <BannerItem
          className={`flex-1 ${getWebSession().user.role !== Roles.User ? '' : '!cursor-default'}`}
          label='Sagsbehandlere'
          value={(
            <div>
              <div className='font-semibold'>
                {[props.details.case.responsibleUser.firstName, props.details.case.responsibleUser.lastName].filter(Boolean).join(' ')}
              </div>
              <div className='text-xs italic text-muted/90'>
                {caseManagersStringFormat(props.details.case.caseManagers ?? [])}
              </div>
            </div>
          )}
          onClick={() => {
            if (getWebSession().user.role !== Roles.User) {
              caseManagerDialogOpenChanged(true)
            }
          }}
        />

        <EditCaseManagersDialog onClose={onDialogClose} details={props.details} isDialogOpen={caseManagerDialogOpen} onOpenedChange={caseManagerDialogOpenChanged} />
        <EditCaseInformationDialog onClose={onDialogClose} details={props.details} isDialogOpen={caseInfoDialogOpen} onOpenedChange={caseInfoDialogOpenChanged} />
      </BannerItemLayout>
    </PageBanner>
  );
}


function caseManagersStringFormat(caseManagers: CaseManagers[]): string {
  const firstdata = caseManagers[0];

  return caseManagers.length > 1
    ? `${[firstdata.firstName, firstdata.lastName].join(' ')} [+${caseManagers.length - 1}]`
    : `${[firstdata.firstName, firstdata.lastName].join(' ')}`;
}