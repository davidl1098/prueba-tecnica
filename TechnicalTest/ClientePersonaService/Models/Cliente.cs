namespace ClientePersonaService.Models
{
    public class Cliente
    {
        public int Id { get; set; }  // Clave única
        public string Contrasena { get; set; }
        public bool Estado { get; set; }
    }
}
