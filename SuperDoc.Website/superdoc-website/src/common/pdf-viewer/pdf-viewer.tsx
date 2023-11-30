"use client"
import { cn } from '@/lib/utils';
import React, { useEffect, useRef } from 'react';

export type PdfViewerProps = {
  url: string;
  className?: string;
};

export function PdfViewer(props: PdfViewerProps) {
  const iFrameElement = useRef();
  const pdfViewerElement = useRef();

  return (
    <div className={cn('h-full', props.className)} ref={pdfViewerElement}>
      <iframe id="pdfViewerIframe" ref={iFrameElement} width="100%" height="100%"></iframe>
    </div>
  );
}