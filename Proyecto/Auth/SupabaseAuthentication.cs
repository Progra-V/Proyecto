using Supabase;
using Proyecto.SupabaseClient;

namespace Proyecto.Auth
{
    public static class SupabaseAuthentication
    {
        public static async Task<Supabase.Gotrue.Session?> SignIn(string txtEmail, string txtPwd)
        {
            try
            {
                Client client = await SupabClient.GetSupabaseClientAsync();

                var session = await client.Auth.SignIn(txtEmail, txtPwd);

                return session;
            }
            catch
            {
                return null;
            }
        }
    }
}