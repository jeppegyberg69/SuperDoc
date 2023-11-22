"use client"
import { cn } from '@/lib/utils';
import React, { useRef } from 'react';

export type PdfViewerProps = {
  url: string;
  className?: string;
};

export function PdfViewer(props: PdfViewerProps) {
  const iFrameElement = useRef();
  const pdfViewerElement = useRef();

  return (
    <div className={cn('h-full', props.className)} ref={pdfViewerElement}>
      <iframe ref={iFrameElement} src={props.url} width="100%" height="100%"></iframe>
    </div>
  );
}