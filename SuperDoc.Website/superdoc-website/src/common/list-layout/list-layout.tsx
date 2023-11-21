import { ReactNode } from 'react';

export type ListLayoutProps = {
  toolbar?: ReactNode;
  list: ReactNode;
};

export function ListLayout({ list, toolbar }: ListLayoutProps) {
  return (
    <div className='list-layout h-full gap-4 py-4'>
      <div className='list-layout-toolbar'>
        {toolbar}
      </div>
      <div className='list-layout-list panel panel-shadow'>
        {list}
      </div>
    </div>
  );
}