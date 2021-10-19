using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutoBogus;
using curso.api;
using curso.api.Models.Usuarios;
using curso.test.Configurations;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace curso.test.Integrations.Controllers
{
    public class UsuarioControllerTests : IClassFixture<WebApplicationFactory<Startup>>, IAsyncLifetime
    {
        private readonly WebApplicationFactory<Startup> _factory;
        protected readonly ITestOutputHelper _output;
        protected readonly HttpClient _httpClient;
        protected RegistroViewModelInput RegistroViewModelInput;
        protected UsuarioViewModelOutput UsuarioViewModelOutput;

        public UsuarioControllerTests(WebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
            _httpClient = _factory.CreateClient();
        }

        [Fact]
        public async Task Registrar_InformandoUsuarioEsenha_DeveRetornarSucesso()
        {
            // Arrange
            RegistroViewModelInput = new AutoFaker<RegistroViewModelInput>(AutoBogusConfiguration.LOCATE).RuleFor(p => p.Email,
                                                                                     faker => faker.Person.Email.ToLower())
                                                                            .RuleFor(p => p.Login,
                                                                                     faker => faker.Person.UserName.ToLower());
            
            StringContent content = new StringContent(JsonConvert.SerializeObject(RegistroViewModelInput),
                                                      Encoding.UTF8,
                                                      "application/json");

            // Act
            var httpClientRequest = await _httpClient.PostAsync("api/v1/usuario/registrar", content);

            // Assert
            _output.WriteLine($"{nameof(UsuarioControllerTests)}_{nameof(Registrar_InformandoUsuarioEsenha_DeveRetornarSucesso)} = {await httpClientRequest.Content.ReadAsStringAsync()}");
            Assert.Equal(HttpStatusCode.Created, httpClientRequest.StatusCode);
        }


        [Fact]
        public async Task Logar_InformandoUsuarioEsenhaExistentes_DeveRetornarSucesso()
        {
            // Arrange
            var loginViewModelInput = new LoginViewModelInput
            {
                Login = RegistroViewModelInput.Login,
                Senha = RegistroViewModelInput.Senha
            };

            StringContent content = new StringContent(JsonConvert.SerializeObject(loginViewModelInput),
                                                      Encoding.UTF8,
                                                      "application/json");

            // Act
            var httpClientRequest = await _httpClient.PostAsync("api/v1/usuario/logar", content);

            UsuarioViewModelOutput = JsonConvert.DeserializeObject<UsuarioViewModelOutput>(await httpClientRequest.Content.ReadAsStringAsync());

            // Assert
            Assert.Equal(HttpStatusCode.OK, httpClientRequest.StatusCode);
            Assert.Equal(loginViewModelInput.Login, UsuarioViewModelOutput.Login);
            Assert.NotNull(UsuarioViewModelOutput.Token);
            _output.WriteLine($"{nameof(UsuarioControllerTests)}_{nameof(Logar_InformandoUsuarioEsenhaExistentes_DeveRetornarSucesso)} = {await httpClientRequest.Content.ReadAsStringAsync()}");
        }

        public async Task InitializeAsync()
        {
            await Registrar_InformandoUsuarioEsenha_DeveRetornarSucesso();
            await Logar_InformandoUsuarioEsenhaExistentes_DeveRetornarSucesso();
        }

        public async Task DisposeAsync()
        {
            _httpClient.Dispose();
        }
    }
}