using Alura.ListaLeitura.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Lista = Alura.ListaLeitura.Modelos.ListaLeitura;

namespace Alura.ListaLeitura.HttpClients
{
    public class LivroApiClient
    {
        private readonly HttpClient _httpClient;

        public LivroApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Lista> GetListaLeituraAsync(TipoListaLeitura tipo)
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync($"listasleitura/{tipo}");
            responseMessage.EnsureSuccessStatusCode();

            return await responseMessage.Content.ReadAsAsync<Lista>();
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

        public async Task PostLivroAsync(LivroUpload model)
        {
            HttpContent content = CreateMultipartFormDataContent(model);
            HttpResponseMessage responseMessage = await _httpClient.PostAsync("livros", content);
            responseMessage.EnsureSuccessStatusCode();
        }

        private HttpContent CreateMultipartFormDataContent(LivroUpload model)
        {
            var content = new MultipartFormDataContent();

            content.Add(new StringContent(model.Titulo), EnvolveComAspasDuplas("titulo"));
            content.Add(new StringContent(model.Lista.ParaString()), EnvolveComAspasDuplas("lista"));

            if (!string.IsNullOrEmpty(model.Subtitulo))
                content.Add(new StringContent(model.Subtitulo), EnvolveComAspasDuplas("subtitulo"));
            if (!string.IsNullOrEmpty(model.Resumo))
                content.Add(new StringContent(model.Resumo), EnvolveComAspasDuplas("resumo"));
            if (!string.IsNullOrEmpty(model.Autor))
                content.Add(new StringContent(model.Autor), EnvolveComAspasDuplas("autor"));
            
            if (model.Id > 0)
                content.Add(new StringContent(model.Id.ToString()), EnvolveComAspasDuplas("id"));

            if (model.Capa != null)
            {
                var imageContent = new ByteArrayContent(model.Capa.ConvertToBytes());
                imageContent.Headers.Add("content-type", "image/png");
                content.Add(imageContent, EnvolveComAspasDuplas("capa"), EnvolveComAspasDuplas("capa.png"));
            }

            return content;
        }

        private string EnvolveComAspasDuplas(string valor)
        {
            return $"\"{valor}\"";
        }

        public async Task PutLivroAsync(LivroUpload model)
        {
            HttpContent content = CreateMultipartFormDataContent(model);
            HttpResponseMessage responseMessage = await _httpClient.PutAsync("livros", content);
            responseMessage.EnsureSuccessStatusCode();
        }

        public async Task DeleteLivroAsync(int id)
        {
            HttpResponseMessage responseMessage = await _httpClient.DeleteAsync($"livros/{id}");
            responseMessage.EnsureSuccessStatusCode();
        }
    }
}