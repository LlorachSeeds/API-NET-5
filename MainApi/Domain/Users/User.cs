using System;
using Domain.Persons;
using Domain.Roles;

namespace Domain.Users
{
    public class User
    {
        public Guid Id { get; set; }

        public Person Person { get; set; }

        public string UserName { get; set; }

        public Guid RolId { get; set; }

        public Rol Rol { get; set; }

        public string Password { get; set; }

        public Guid UserSettingsId { get; set; }

        public UserSettings Settings { get; set; }

        public bool IsDeleted { get; set; }

        public bool ResetPassword { get; set; }

        /* Esto va a quedar en otro contexto donde haya una relación de usaurio/reporte para tomar el correo y mandar distintas cosas
        public bool Reporting_ON { get; set; }
        public int Reports_EmailDaily_ID { get; set; }
        public Reports_DailyEmail Reports_DailyEmail { get; set; }
        */

        // Add to external authorization (google/facebook/microsoft365)
        public string NameIdentifier { get; set; } // el id que entrega el proveedor

        public string Provider { get; set; } // proveedor de oauth2

        public bool IsActive { get; set; } // poder reconocer si esta deshabilitado el usuario

        public string UserAccessEmail { get; set; } // Email especifico para el acceso, es algo que el usuario puede cambiar.

        public bool UserAccessEmailConfirmed { get; set; }

        public string TokenRecovery { get; set; }

        public DateTime? TokenDateCreated { get; set; }

        public bool TermsAndConditions { get; set; }

        public string RemoteToken { get; set; }
    }
}