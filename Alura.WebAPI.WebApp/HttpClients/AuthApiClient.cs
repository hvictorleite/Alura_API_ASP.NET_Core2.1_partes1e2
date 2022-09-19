using Alura.ListaLeitura.Seguranca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.HttpClients
{
    public class LoginResult
    {
        public bool Succeeded { get; set; }
        public string Token { get; set; }
    }


    public class AuthApiClient
    {
        private readonly HttpClient _httpClient;

        public AuthApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResult> PostLoginAsync(LoginModel model)
        {
            HttpResponseMessage responseMessage = await _httpClient.PostAsJsonAsync("login", model);
            return new LoginResult
            {
                Succeeded = responseMessage.IsSuccessStatusCode,
                Token = await responseMessage.Content.ReadAsStringAsync()
            };
        }


    }
}
