using Cosmicbills_TestAPP.Server.Accounting.Xero.Dtos;
using Cosmicbills_TestAPP.Server.Accounting.Xero.Models;
using Xero.NetStandard.OAuth2.Api;
using Xero.NetStandard.OAuth2.Client;
using Xero.NetStandard.OAuth2.Config;
using Xero.NetStandard.OAuth2.Token;

namespace Cosmicbills_TestAPP.Server.Accounting.Xero
{
    public class XeroProxy
    {
        public static string Authenticate()
        {
            var xeroConfig = new XeroConfiguration
            {
                ClientId = XeroConfig.ClientId,
                ClientSecret = XeroConfig.ClientSecret,
                CallbackUri = new Uri(XeroConfig.CallbackUrl ?? string.Empty),
                State = XeroConfig.State,
                Scope = XeroConfig.Scope
            };

            var client = new XeroClient(xeroConfig);

            return client.BuildLoginUri();
        }

        public static async Task<XeroOAuth2Token> GetSetXeroToken(string code, string session_state)
        {
            XeroConfig.Init();
            var xConfig = new XeroConfiguration
            {
                ClientId = XeroConfig.ClientId,
                ClientSecret = XeroConfig.ClientSecret,
                CallbackUri = new Uri(XeroConfig.CallbackUrl ?? string.Empty),
                Scope = XeroConfig.Scope
            };

            var client = new XeroClient(xConfig);
            var xeroToken = (XeroOAuth2Token)await client.RequestAccessTokenAsync(code);

            XeroConfig.AccessToken = xeroToken.AccessToken;
            XeroConfig.RefreshToken = xeroToken.RefreshToken;
            XeroConfig.TenantId = xeroToken.Tenants[0].TenantId.ToString();

            return xeroToken;
        }

        public static async Task<List<COADto>> GetAllChartOfAccountsAsync(string accessToken, string tenantId, int retryCtr)
        {
            var accounts = new List<COADto>();

            try
            {
                accessToken = XeroConfig.AccessToken ?? accessToken;
                tenantId = XeroConfig.TenantId ?? tenantId;

                var response = await new AccountingApi().GetAccountsAsync(accessToken, tenantId);

                foreach (var account in response._Accounts)
                {
                    accounts.Add(new COADto()
                    {
                        Id = account.AccountID.GetValueOrDefault().ToString(),
                        Code = account.Code,
                        Name = account.Name,
                    });
                }
            }
            catch (Exception ex)
            {
                if (ex != null && ex.ToString().Contains("Unauthorized"))
                {
                    if (retryCtr < 3)
                    {
                        return await GetAllChartOfAccountsAsync(accessToken, tenantId, retryCtr + 1);
                    }
                }
            }
            return accounts;
        }

        public static async Task<List<CustomerDetailDto>> GetAllCustomersAsync(string accessToken, string tenantId, int retryCtr)
        {
            var customers = new List<CustomerDetailDto>();

            try
            {
                accessToken = XeroConfig.AccessToken ?? accessToken;
                tenantId = XeroConfig.TenantId ?? tenantId;

                var response =  await new AccountingApi().GetContactsAsync(accessToken, tenantId);

                foreach (var customer in response._Contacts)
                {
                    if (customer.IsCustomer.GetValueOrDefault(false))
                        customers.Add(new CustomerDetailDto()
                        {
                            Id = customer.ContactID.GetValueOrDefault().ToString(),
                            Name = customer.Name,
                        });
                }
            }
            catch (Exception ex)
            {
                if (ex != null && ex.ToString().Contains("Unauthorized"))
                {
                    if (retryCtr < 3)
                    {
                        return await GetAllCustomersAsync(accessToken, tenantId, retryCtr + 1);
                    }
                }
            }
            return customers;
        }

        public static async Task<List<SupplierDto>> GetAllSuppliersAsync(string accessToken, string tenantId, int retryCtr)
        {
            var suppliers = new List<SupplierDto>();

            try
            {
                accessToken = XeroConfig.AccessToken ?? accessToken;
                tenantId = XeroConfig.TenantId ?? tenantId;

                var response = await new AccountingApi().GetContactsAsync(accessToken, tenantId);

                foreach (var supplier in response._Contacts)
                {
                    if (supplier.IsSupplier.GetValueOrDefault(false))
                        suppliers.Add(new SupplierDto()
                        {
                            Id = supplier.ContactID.GetValueOrDefault().ToString(),
                            Name = supplier.Name,
                        });
                }
            }
            catch (Exception ex)
            {
                if (ex != null && ex.ToString().Contains("Unauthorized"))
                {
                    if (retryCtr < 3)
                    {
                        return await GetAllSuppliersAsync(accessToken, tenantId, retryCtr + 1);
                    }
                }
            }
            return suppliers;
        }
    }
}
