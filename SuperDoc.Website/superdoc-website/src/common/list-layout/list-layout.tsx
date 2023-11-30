import { ReactNode } from 'react';

export type ListLayoutProps = {
  toolbar?: ReactNode;
  list: ReactNode;
  isPanelLayout?: boolean
};

export function ListLayout({ list, toolbar, isPanelLayout = true }: ListLayoutProps) {
  return (
    <div className='list-layout h-full gap-4 py-4'>
      <div className='list-layout-toolbar'>
        {toolbar}
      </div>
      <div className={`h-full list-layout-list ${isPanelLayout ? 'panel panel-shadow' : ''}`}>
        {list}
      </div>
    </div>
  );
}