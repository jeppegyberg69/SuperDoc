"use client";
import React from 'react';
import { Button } from '@/components/ui/button';
import { useForm } from 'react-hook-form';
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage, } from "@/components/ui/form"
import { Input } from "@/components/ui/input"
import { zodResolver } from "@hookform/resolvers/zod"
import * as z from "zod"
import { login } from '@/services/login-service';
import { createSessionFromToken } from '@/models/session/session';
import { useRouter } from 'next/navigation'
import { setWebSession } from '@/common/session-context/session-context';
import { useToast } from '@/components/ui/use-toast';


const formSchema = z.object({
  email: z.string().min(2, {
    message: "email adressen skal mindst være 2 karakter langt.",
  }),
  password: z.string().min(2, {
    message: 'Adgangskoden skal mindst være 2 karakter langt'
  }).max(128, {
    message: 'Adgangskoden må ikke være længere end 128 karakter langt'
  }),
})

export type LoginFormProps = {
};

export function LoginForm(props: LoginFormProps) {
  const router = useRouter();
  const { toast } = useToast();
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      email: "",
      password: "",
    },
  })


  const validateAndSubmit = async (values: z.infer<typeof formSchema>) => {
    login(values.email, values.password)
      .then((response) => {
        if (response) {
          setLoginResponse(response);
          router.push("/");
        }
      })
      .catch((error) => {
        toast({
          title: "Fejl",
          description: "Login oplysninger er ikke korrekte, prøv igen."
        })
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
              <FormLabel>Emailadresse</FormLabel>
              <FormControl>
                <Input placeholder="mailadresse@mail.dk" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="password"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Adgangskode</FormLabel>
              <FormControl>
                <Input type='password' placeholder='Adgangskode' {...field} />
              </FormControl>
            </FormItem>
          )}
        />
        <Button type="submit">Gem</Button>
      </form>
    </Form>
  );
}

function setLoginResponse(response: any) {
  localStorage.removeItem('jpj_websession');
  const newSession = createSessionFromToken(response)
  setWebSession(newSession);
  localStorage.setItem('jpj_websession', JSON.stringify(newSession));
}