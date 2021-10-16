using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using curso.api;
using curso.api.Models.Usuarios;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace curso.test.Integrations.Controllers
{
    public class UsuarioControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly ITestOutputHelper _output;
        private readonly HttpClient _httpClient;

        public UsuarioControllerTests(WebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
            _httpClient = _factory.CreateClient();
        }

        [Fact]
        public async Task Logar_InformandoUsuarioEsenhaExistentes_DeveRetornarSucesso()
        {
            // Arrange
            var loginViewModelInput = new LoginViewModelInput
            {
                Login = "nicolas",
                Senha = "123456"
            };

            StringContent content = new StringContent(JsonConvert.SerializeObject(loginViewModelInput),
                                                      Encoding.UTF8,
                                                      "application/json");

            // Act
            var httpClientRequest = await _httpClient.PostAsync("api/v1/usuario/logar", content);

            var usuarioViewModelOutput = JsonConvert.DeserializeObject<UsuarioViewModelOutput>(await httpClientRequest.Content.ReadAsStringAsync());
                                                                                                                        

            // Assert
            Assert.Equal(HttpStatusCode.OK, httpClientRequest.StatusCode);
            Assert.Equal(loginViewModelInput.Login, usuarioViewModelOutput.Login);
            Assert.NotNull(usuarioViewModelOutput.Token);
            _output.WriteLine(usuarioViewModelOutput.Token);
        }

        [Fact]
        public void Registrar_InformandoUsuarioEsenha_DeveRetornarSucesso()
        {
            // Arrange
            var hash = DateTime.Now.Ticks;

            var registroViewModelInput = new RegistroViewModelInput
            {
                Login = $"teste_{hash}",
                Email = $"teste_{hash}@email.com.br",
                Senha = "123456"
            };

            StringContent content = new StringContent(JsonConvert.SerializeObject(registroViewModelInput),
                                                      Encoding.UTF8,
                                                      "application/json");

            // Act
            var httpClientRequest = _httpClient.PostAsync("api/v1/usuario/registrar", content)
                                               .GetAwaiter()
                                               .GetResult();



            // Assert
            Assert.Equal(HttpStatusCode.Created, httpClientRequest.StatusCode);
        }
    }
}