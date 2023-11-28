import React from 'react';
import { LoginForm } from './login-form';
export type LoginPageProps = {};

export default function LoginPage(props: LoginPageProps) {

  return (
    <div className='flex h-full w-full items-center justify-center'>
      <div className='panel panel-shadow w-64 max-w-xl flex-1'>
        <LoginForm ></LoginForm>
      </div>
    </div>
  );
}