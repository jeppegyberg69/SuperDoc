import type { Metadata } from 'next'
import { Inter } from 'next/font/google'

import '../styles/globals.css'
import 'styles/style.scss'
import '@fortawesome/fontawesome-svg-core/styles.css';
import Providers from './providers';

const inter = Inter({ subsets: ['latin'] })

export const metadata: Metadata = {
  title: 'Create Next App',
  description: 'Generated by create next app',
  icons: "/favicon.ico"
}

export default function RootLayout({
  children,
}: {
  children: React.ReactNode
}) {
  return (
    <html lang="en">
      <body className={inter.className}>
        <Providers>
          {children}
        </Providers>
      </body>
    </html>
  )
}
