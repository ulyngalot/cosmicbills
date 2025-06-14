import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiService } from './api.service';
import { XeroOAuth2Token } from './XeroAuthToken';
import { COA, Contact } from './XeroEntities';
import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  public accessToken: any;
  public tenantId: any;
  public contacts: Contact[] = [];
  public coas: COA[] = [];

  constructor(private route: ActivatedRoute, private router: Router,
    private apiService: ApiService,) { }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      const xeroToken = params['xeroToken'];

      if (xeroToken) {
        localStorage.setItem('xeroToken', JSON.stringify(xeroToken));

        // Clean up the URL (remove the query param)
        this.router.navigate([], {
          queryParams: {},
          replaceUrl: true
        });
      }
    });
    const storedToken = JSON.parse(localStorage.getItem('xeroToken') || '{}');
    const parsed: XeroOAuth2Token = JSON.parse(storedToken);

    this.accessToken = parsed.AccessToken;
    this.tenantId = parsed.Tenants[0].TenantId;
  }

  connectToXero() {
    window.location.href = this.apiService.xeroAuthenticate();
  }

  exportToExcel(data: any): void {
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(data);
    const workbook: XLSX.WorkBook = {
      Sheets: { 'Data': worksheet },
      SheetNames: ['Data']
    };
    const excelBuffer: any = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });

    const blob: Blob = new Blob([excelBuffer], {
      type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
    });

    FileSaver.saveAs(blob, 'my-data.xlsx');
  }

  downloadCustomers() {
    this.apiService.downloadCustomers(this.accessToken, this.tenantId).subscribe({
      next: data => {
        this.contacts = data;
        console.log(this.contacts);
        this.exportToExcel(this.contacts);
      },
      error: err => console.error('Error fetching values:', err)
    });
    
  }

  downloadSuppliers() {
    this.apiService.downloadSuppliers(this.accessToken, this.tenantId).subscribe({
      next: data => {
        this.contacts = data;
        console.log(this.contacts);
        this.exportToExcel(this.contacts);
      },
      error: err => console.error('Error fetching values:', err)
    });
  }

  downloadCOA() {
    this.apiService.downloadCOA(this.accessToken, this.tenantId).subscribe({
      next: data => {
        this.coas = data;
        console.log(this.coas);
        this.exportToExcel(this.coas);
      },
      error: err => console.error('Error fetching values:', err)
    });
  }

  title = 'cosmicbills_testapp.client';
}
