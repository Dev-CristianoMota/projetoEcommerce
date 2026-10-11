using System.Text.Json;

namespace Ecommerce.Helpers
{
    public static class SessionExtensions
    {
        public static void SetObject<T>(this ISession session, string chave, T valor)
        {
            session.SetString(chave, JsonSerializer.Serialize(valor));
        }

        public static T? GetObject<T>(this ISession session, string chave)
        {
            var json = session.GetString(chave);
            return json is null ? default : JsonSerializer.Deserialize<T>(json);
        }
    }
}