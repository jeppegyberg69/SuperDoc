import { BannerItem, BannerItemLayout, PageBanner } from '@/common/page-layout/page-banner';
import { CaseDetails } from '@/models/case-details';
import { CaseManagers } from '@/models/case-manager';
import React from 'react';

export type DetailsBannerProps = {
  details: CaseDetails;
};

export function DetailsBanner(props: DetailsBannerProps) {

  if (!props.details)
    return <span></span>

  return (
    <PageBanner>
      <BannerItemLayout>
        <BannerItem
          label='Sagsnummer'
          value={props.details?.case?.caseNumber}
        />
        <BannerItem
          label='Sagsansvarlig'
          value={[props.details?.case?.responsibleUser?.firstName, props.details?.case?.responsibleUser?.lastName].filter(Boolean).join(' ')}
        />
        <BannerItem
          label='Beskrivelse'
          value={props.details?.case?.description}
          className='flex-1'
        />
        <BannerItem
          label='Sagsbehandlere'
          value={formatStringValues(props.details?.case?.caseManagers ?? [])}
        />
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