

export type Route = {
  title: string;
  path: string;
  permissions?: boolean;
}


export function getGlobalNavigationRoutes(): Route[] {
  return [{
    title: "Sager",
    path: "/"
  }]
}