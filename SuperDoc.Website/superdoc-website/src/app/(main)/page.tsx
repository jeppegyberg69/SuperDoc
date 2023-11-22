import { PageHeader } from '@/common/page-layout/page-header';
import { CaseOverviewTable } from './case-overview-table';

export default function CaseOverview() {
  return (
    <div className="detail-layout">
      <PageHeader />
      <div className="detail-layout-sidebar"></div>
      <div className="detail-layout-content">
        <CaseOverviewTable />
      </div>
    </div>
  )
}