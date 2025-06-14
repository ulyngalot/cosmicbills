export class Tenant {
  id!: string;
  authEventId!: string;
  TenantId!: string;
  TenantType!: string;
  TenantName!: string;
  CreatedDateUtc!: string;
  UpdatedDateUtc!: string;
}

export class XeroOAuth2Token {
  Tenants!: Tenant[];
  AccessToken!: string;
  RefreshToken!: string;
  IdToken!: string | null;
  ExpiresAtUtc!: string;
}
