using Microsoft.AspNetCore.Components;
using ArchivosMan.BLL.Services;

namespace ArchivosMan.Web.Components.Shared
{
    public partial class AuthGuard
    {
        [Inject] private AuthState Auth { get; set; } = default!;
        [Inject] private NavigationManager Nav { get; set; } = default!;

        [Parameter] public RenderFragment? ChildContent { get; set; }

        protected override void OnInitialized()
        {
            if (!Auth.EstaAutenticado)
            {
                // Si no hay sesión, redirigir a login, páginas protegidas.
                Nav.NavigateTo("/login");
            }
        }
    }
}
