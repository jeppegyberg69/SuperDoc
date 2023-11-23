import React from 'react';

export type SplitViewProps = {
  left: any;
  right: any;
};

export function SplitView(props: SplitViewProps) {

  return (
    <div className='split-view grid grid-cols-[28rem,_1fr] gap-4 py-3'>
      <div className='h-full panel panel-shadow'>
        {props.left}
      </div>
      <div className='h-full panel panel-shadow'>
        {props.right}
      </div>
    </div>
  );
}