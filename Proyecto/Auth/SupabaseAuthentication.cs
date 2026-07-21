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

        public static async Task<bool> ChangePassword(
            string accessToken,
            string refreshToken,
            string newPassword)
        {
            try
            {
                Client client = await SupabClient.GetSupabaseClientAsync();

                // Fijar explícitamente la sesión del usuario actual
                // antes de operar (mitiga el riesgo de cliente compartido)
                await client.Auth.SetSession(accessToken, refreshToken);

                var updatedUser = await client.Auth.Update(
                    new Supabase.Gotrue.UserAttributes
                    {
                        Password = newPassword
                    }
                );

                return updatedUser != null;
            }
            catch
            {
                return false;
            }
        }
    }
}