"use client"
import React from 'react';

export type PageBannerProps = {
  children: React.ReactNode;
};

export function PageBanner(props: PageBannerProps) {
  return (
    <div className={`page-layout-banner`}>
      {props.children}
    </div>
  );
}

export type BannerLabelValueLayoutProps = {
  children?: React.ReactNode
};

export function BannerItemLayout(props: BannerLabelValueLayoutProps) {

  return (
    <div className="banner-label-value-layout">{props.children}</div>
  );
}


export type BannerItemProps = {
  label: React.ReactNode;
  value: React.ReactNode;
  clickable?: boolean;
  onClick?: (e: React.MouseEvent) => void;
  noWrap?: boolean;
  ellipsis?: boolean;
  flex?: number;
  id?: string;
  className?: string;
};

export function BannerItem(props: BannerItemProps) {
  const style: React.CSSProperties = {
    flex: props.flex
  };

  return (
    <div
      id={props.id}
      className={`banner-label-value-item cursor-pointer hover:bg-secondary/30 ${props.className ?? ''}`}
      onClick={props.onClick}
      style={style}
    >
      <div className={`banner-label-value-item-label font-semibold spacing text-xs text-neutral-400 ${props.ellipsis ? 'banner-label-value-item-ellipsis' : ''}`}>{props.label}</div>
      <div className="banner-label-value-item-value font-medium text-sm mt-2">{props.value}</div>
    </div>
  );
}