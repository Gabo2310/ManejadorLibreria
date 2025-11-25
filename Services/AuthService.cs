using ManejadorLibreria.Data;
using ManejadorLibreria.Models;
using Microsoft.EntityFrameworkCore;

namespace ManejadorLibreria.Services
{
    public class AuthService
    {
        private readonly LibreriaContext _context;

        public AuthService(LibreriaContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> RegistrarUsuario(string nombreUsuario, string email, string password)
        {
            // Verificar si el email ya existe
            if (await _context.Usuarios.AnyAsync(u => u.Email == email))
                return null;

            var usuario = new Usuario
            {
                NombreUsuario = nombreUsuario,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                FechaRegistro = DateTime.Now
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario?> ValidarUsuario(string email, string password)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
                return null;

            bool esValido = BCrypt.Net.BCrypt.Verify(password, usuario.PasswordHash);
            return esValido ? usuario : null;
        }
    }
}