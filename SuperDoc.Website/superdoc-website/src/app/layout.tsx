import type { Metadata } from 'next'
import { Inter } from 'next/font/google'

import '../styles/globals.css'
import 'styles/style.scss'
import '@fortawesome/fontawesome-svg-core/styles.css';

import CustomQueryClientProvider, { SessionProvider } from './providers';
import { RequireAuth } from '@/common/require-auth/require-auth';
import { Toaster } from "@/components/ui/toaster"
import { Settings } from 'luxon';
const inter = Inter({ subsets: ['latin'] })

export const metadata: Metadata = {
  title: 'JPJ Technologies',
  description: 'JPJ Technologies Sags- og dokumenthåndteringssystem',
  icons: "/favicon.ico"
}

export default function RootLayout({
  children,
}: {
  children: React.ReactNode
}) {
  // initialize luxon
  Settings.defaultLocale = "da";

  return (
    <html lang="en">
      <body className={inter.className + ' bg-background/30'}>
        <SessionProvider>
          <CustomQueryClientProvider>
            <RequireAuth>
              {children}
              <Toaster />
            </RequireAuth>
          </CustomQueryClientProvider>
        </SessionProvider>
      </body>
    </html>
  )
}
