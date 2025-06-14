import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { COA, Contact } from './XeroEntities';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private responseType = 'code';
  private state = 'random_state_xyz';
  private clientId = '736D383C467F401AB234DED15586A738';
  private base = 'https://login.xero.com/identity/connect/authorize';
  private redirectUri = 'https://localhost:7085/Xero/XeroAuthCallBack';
  private scope = 'offline_access openid profile email accounting.transactions accounting.transactions.read accounting.contacts accounting.contacts.read accounting.attachments accounting.settings accounting.attachments.read accounting.settings.read';

  constructor(private http: HttpClient) { }

  xeroAuthenticate(): string {
    return `${this.base}?response_type=${this.responseType}&client_id=${this.clientId}&redirect_uri=${encodeURIComponent(this.redirectUri)}&scope=${encodeURIComponent(this.scope)}&state=${this.state}`;
  }

  downloadCustomers(accessToken: string, tenantId: string): Observable<Contact[]> {
    const url = `https://localhost:7085/Xero/GetCustomers/?accessToken=${accessToken}&tenantId=${tenantId}`;
    return this.http.get<Contact[]>(url);
  }

  downloadSuppliers(accessToken: string, tenantId: string): Observable<Contact[]> {
    const url = `https://localhost:7085/Xero/GetSuppliers/?accessToken=${accessToken}&tenantId=${tenantId}`;
    return this.http.get<Contact[]>(url);
  }

  downloadCOA(accessToken: string, tenantId: string): Observable<COA[]> {
    const url = `https://localhost:7085/Xero/GetCOA/?accessToken=${accessToken}&tenantId=${tenantId}`;
    return this.http.get<COA[]>(url);
  }
}
