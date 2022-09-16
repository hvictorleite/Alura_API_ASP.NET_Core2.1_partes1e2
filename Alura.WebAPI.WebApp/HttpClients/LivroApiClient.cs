using Alura.ListaLeitura.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.HttpClients
{
    public class LivroApiClient
    {
        private readonly HttpClient _httpClient;


        public LivroApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LivroApi> GetLivroAsync(int id)
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync($"livros/{id}");
            responseMessage.EnsureSuccessStatusCode();

            return await responseMessage.Content.ReadAsAsync<LivroApi>();
        }

        public async Task<byte[]> GetImagemCapaAsync(int id)
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync($"livros/{id}/capa");
            responseMessage.EnsureSuccessStatusCode();

            return await responseMessage.Content.ReadAsByteArrayAsync();
        }
    }
}