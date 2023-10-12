namespace API.Helpers;

public class Authorizacion
{
    public enum Roles
    {
        Administrador,
        Gerente,
        Empleado
    }

    public const Roles rol_prdeterminado = Roles.Empleado;
}
