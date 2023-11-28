"use client";
import React from 'react';
import { Button } from '@/components/ui/button';
import { useForm } from 'react-hook-form';
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form"
import { Input } from "@/components/ui/input"
import { zodResolver } from "@hookform/resolvers/zod"
import * as z from "zod"
import { login } from '@/services/login-service';
import { createSessionFromToken } from '@/models/session/session';
import { useRouter } from 'next/navigation'


const formSchema = z.object({
  email: z.string().min(2, {
    message: "Username must be at least 2 characters.",
  }),
  password: z.string().min(2, {
    message: ''
  }).max(128, {
    message: ''
  }),
})

export type LoginFormProps = {
};

export function LoginForm(props: LoginFormProps) {
  const router = useRouter();
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      email: "",
      password: "",
    },
  })


  const validateAndSubmit = async (values: z.infer<typeof formSchema>) => {
    const loginResponse = login(values.email, values.password)
      .then((response) => {
        if (response) {
          setLoginResponse(response);
          router.push("/");
        }
      });
  }


  return (
    <Form
      {...form}
    >
      <form
        onSubmit={form.handleSubmit(validateAndSubmit)}
        className="space-y-8"
      >
        <FormField
          control={form.control}
          name="email"
          render={({ field }) => (
            <FormItem>
              <FormLabel>email</FormLabel>
              <FormControl>
                <Input placeholder="shadcn" {...field} />
              </FormControl>
              <FormDescription>
                This is your public display name.
              </FormDescription>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="password"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Password</FormLabel>
              <FormControl>
                <Input type='password' placeholder='Adgangskode' {...field} />
              </FormControl>
              {/* <FormDescription>
                This is your public display name.
              </FormDescription>
              <FormMessage /> */}
            </FormItem>
          )}
        />
        <Button type="submit">Submit</Button>
      </form>
    </Form>
  );
}

export function setLoginResponse(response: any) {
  localStorage.removeItem('jpj_websession');
  const newSession = createSessionFromToken(response)
  localStorage.setItem('jpj_websession', JSON.stringify(newSession));
}