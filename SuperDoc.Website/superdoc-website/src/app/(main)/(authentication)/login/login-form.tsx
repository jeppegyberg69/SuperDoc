"use client";
import React from 'react';
import { nameof } from '@/common/nameof/nameof';
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
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      email: "",
      password: "",
    },
  })


  // const a = fetch("https://localhost:44304/api/User/Login", {
  //   method: 'POST',
  //   body: JSON.stringify({
  //     email: "dsadsa",
  //     password: "dsads"
  //   }),
  //   headers: {
  //     "accept": "*/*",
  //     "cache-control": "no-cache",
  //     "content-type": "application/json",
  //     "pragma": "no-cache",
  //     "Access-Control-Allow-Origin": "*",
  //     "origin": "*"
      
  //   },
    
  //   "referrer": "https://localhost:44304/swagger/index.html",
  //   "referrerPolicy": "strict-origin-when-cross-origin",
  // });


//   fetch("https://localhost:44304/api/User/Login", {
//   "headers": {
//     "accept": "*/*",
//     "cache-control": "no-cache",
//     "content-type": "application/json",
//     "pragma": "no-cache",
//     "sec-ch-ua": "\"Brave\";v=\"119\", \"Chromium\";v=\"119\", \"Not?A_Brand\";v=\"24\"",
//     "sec-ch-ua-mobile": "?0",
//     "sec-ch-ua-platform": "\"Windows\""
//   },
//   "referrer": "http://localhost:3000/",
//   "referrerPolicy": "strict-origin-when-cross-origin",
//   "body": "{\"email\":\"dsadsa\",\"password\":\"dsads\"}",
//   "method": "POST",
//   "mode": "cors",
//   "credentials": "omit"
// });


  const onSubmit = (values: z.infer<typeof formSchema>) => {
    console.log(values)
    login("dsadsa", "dsa");

    // const a = fetch("https://localhost:44304/api/User/Login", {
    //   method: 'POST',
    //   body: "email:dsadsa,password:dsadsa",
    //   headers: {
    //     "accept": "*/*",
    //     "cache-control": "no-cache",
    //     "content-type": "application/json",
    //     "pragma": "no-cache",
    //     "sec-fetch-dest": "empty",
    //     "sec-fetch-mode": "cors",
    //     "sec-fetch-site": "same-origin",
    //     "sec-gpc": "1"
    //   },
    //   "referrer": "https://localhost:44304/swagger/index.html",
    //   "referrerPolicy": "strict-origin-when-cross-origin",
    //   "mode": "cors",
    // });
    // return login("dsadsa", "dsa");
  }

  return (
    <Form
      {...form}
    >
      <form
        onSubmit={form.handleSubmit(onSubmit)}
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