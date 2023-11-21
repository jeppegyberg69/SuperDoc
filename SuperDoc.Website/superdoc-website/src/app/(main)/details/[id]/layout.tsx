import { DetailsHeader } from "../header/details-header";


export default function DetailsLayout({ id, children }: any) {
  return (
    <div className='detail-layout h-full gap-4'>
      <DetailsHeader caseId={id} />
      <div className='detail-layout-sidebar p-4 bg-neutral-100'> DSAD SAD</div>
      <div className="detail-layout-content">
        {children}
      </div>
    </div>
  );
}
