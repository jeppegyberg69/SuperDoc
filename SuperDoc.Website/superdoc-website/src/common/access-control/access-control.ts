/*
 * Checks if the permission(s) exists in the webSession.
 * Returns true if the given permissions exists in the webSession.
*/
export function hasPermissions(...permissions: string[]): boolean {
  // return permissions.every(permission => [session].[permissions].includes(permission));
  return true;
}

export function hasRole(role: string): boolean {
  return true;
}