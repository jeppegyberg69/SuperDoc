"use client"
import React, { useRef } from 'react';

export type PdfViewerProps = {
  url: string;
};

export function PdfViewer(props: PdfViewerProps) {
  const iFrameElement = useRef();
  const pdfViewerElement = useRef();

  return (
    <div className='h-full' ref={pdfViewerElement}>
      <iframe ref={iFrameElement} src={props.url} width="100%" height="100%"></iframe>
    </div>
  );
}