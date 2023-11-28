import { PageHeader } from '@/common/page-layout/page-header';
import { CaseOverviewTable } from './case-overview-table';

export default function CaseOverview() {
  return (
    <div className="detail-layout">
      <PageHeader />
      <div className="detail-layout-content mx-4">
        <CaseOverviewTable />
      </div>
    </div>
  )
}