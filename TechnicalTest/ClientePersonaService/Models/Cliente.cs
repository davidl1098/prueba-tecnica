namespace ClientePersonaService.Models
{
    public class Cliente: Persona
    {
        public int Id { get; set; }  // Clave única
        public string Contrasena { get; set; }
        public bool Estado { get; set; }

        // Método de lógica para validar la contraseña
        public bool EsContrasenaValida()
        {
            return !string.IsNullOrWhiteSpace(Contrasena) && Contrasena.Length == 4 && Contrasena.All(char.IsDigit);
        }
    }
}
