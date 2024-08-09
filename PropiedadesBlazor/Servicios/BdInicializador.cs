using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PropiedadesBlazor.Data;

namespace PropiedadesBlazor.Servicios
{
    public class BdInicializador : IBdInicializador
    {
        private readonly ApplicationDbContext _bd;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public BdInicializador(ApplicationDbContext bd, 
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _bd = bd;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public void Inicializar()
        {
            try
            {
                if (_bd.Database.GetPendingMigrations().Count() > 0)
                {
                    _bd.Database.Migrate();
                }
            }
            catch (Exception)
            {
                
            }

            if (_bd.Roles.Any(x => x.Name == Configuraciones.Rol_Administrador)) return;

            _roleManager.CreateAsync(new IdentityRole(Configuraciones.Rol_Administrador)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Configuraciones.Rol_Publicador)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Configuraciones.Rol_Editador)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new IdentityUser
            {
                UserName = "joseandresmontoya@hotmail.com",
                Email = "joseandresmontoya@hotmail.com",
                EmailConfirmed = true,
            }, "Admin123*").GetAwaiter().GetResult();

            IdentityUser user = _bd.Users.FirstOrDefault(u => u.Email == "joseandresmontoya@hotmail.com");
            _userManager.AddToRoleAsync(user, Configuraciones.Rol_Administrador).GetAwaiter().GetResult();


            _userManager.CreateAsync( new IdentityUser
            {
                UserName = "solismarcos@gmail.com", // Cambia esto por el nombre de usuario deseado
                Email = "solismarcos@gmail.com", // Cambia esto por el correo electrónico deseado
                EmailConfirmed = true,
            }, "publicador123*").GetAwaiter().GetResult();


            IdentityUser user2 = _bd.Users.FirstOrDefault(u => u.Email == "solismarcos@gmail.com");
            _userManager.AddToRoleAsync(user2, Configuraciones.Rol_Publicador).GetAwaiter().GetResult();

           

        }
    }
}
