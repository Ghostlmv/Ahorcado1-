using System;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class Program
{
    static async System.Threading.Tasks.Task Main(string[] args)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://pokeapi.co/api/v2/pokemon?limit=100000&offset=0")
        };

        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            string body = await response.Content.ReadAsStringAsync();
            JObject jsonObject = JsonConvert.DeserializeObject<JObject>(body);
            JArray resultsArray = (JArray)jsonObject["results"];

            var nombresPokemon = new string[resultsArray.Count];
            for (int i = 0; i < resultsArray.Count; i++)
            {
                nombresPokemon[i] = (string)resultsArray[i]["name"];
            }

            Random random = new Random();
            string nombreSeleccionado = nombresPokemon[random.Next(nombresPokemon.Length)];

            char[] nombreOculto = new char[nombreSeleccionado.Length];
            for (int i = 0; i < nombreSeleccionado.Length; i++)
            {
                nombreOculto[i] = '_';
            }

            int intentosRestantes = 6;
            bool juegoTerminado = false;

            Console.WriteLine("¡Bienvenido al juego del Ahorcado!");
            Console.WriteLine(string.Join(" ", nombreOculto));

            while (!juegoTerminado)
            {
                Console.WriteLine($"Intentos restantes: {intentosRestantes}");
                Console.Write("Ingresa una letra: ");
                char letra = Console.ReadKey().KeyChar;
                Console.WriteLine();

                bool letraEncontrada = false;
                for (int i = 0; i < nombreSeleccionado.Length; i++)
                {
                    if (nombreSeleccionado[i] == letra)
                    {
                        nombreOculto[i] = letra;
                        letraEncontrada = true;
                    }
                }

                if (!letraEncontrada)
                {
                    intentosRestantes--;
                }

                Console.WriteLine(string.Join(" ", nombreOculto));

                if (intentosRestantes == 0)
                {
                    Console.WriteLine($"¡Perdiste! El Pokémon era: {nombreSeleccionado}");
                    juegoTerminado = true;
                }
                else if (!new string(nombreOculto).Contains("_"))
                {
                    Console.WriteLine("¡Ganaste! Has adivinado el Pokémon.");
                    juegoTerminado = true;
                }
            }
        }
    }
}





