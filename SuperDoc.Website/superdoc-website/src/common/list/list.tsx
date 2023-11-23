import React from 'react';

export type ListProps = {
  children: React.ReactNode;
  horizontalGridline?: 'enabled' | 'disabled'
};

export function List(props: ListProps) {

  return (
    <div>
      <ul
        data-gridline={props.horizontalGridline ?? 'disabled'}
        className='data-[gridline=enabled]:divide-y-2 data-[gridline=enabled]:divide-neutral-900/10'
      >
        {props.children}
      </ul>
    </div>
  );
}

export function ListItem({ children }: { children: any }) {
  return (
    <div className='h-12 px-4 text-left text-sm text-muted-foreground align-middle font-medium'>
      {children}
    </div>
  )
}
