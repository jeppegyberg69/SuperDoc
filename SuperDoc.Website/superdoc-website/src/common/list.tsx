import React from 'react';

export type ListProps = {
  children: React.ReactNode;
};

export function List(props: ListProps) {

  return (
    <div>
      <ul>
        {props.children}
      </ul>
    </div>
  );
}

export function ListItem({ children }: { children: any }) {
  return (
    <div className='h-12 px-4 text-left align-middle font-medium'>
      {children}
    </div>
  )
}
