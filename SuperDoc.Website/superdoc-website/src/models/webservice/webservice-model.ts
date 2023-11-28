export type WebserviceResponse = {
  status: number,
  statusText: string,
  data?: {
    [key: string]: any
  }
}