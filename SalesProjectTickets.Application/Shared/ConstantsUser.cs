﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesProjectTickets.Application.Shared
{
    public static class ConstantsUser
    {
        public const string ONLY_LETTERS = "^[a-zA-ZáéíóúÁÉÍÓÚ ]*$";
        // Minimo ocho y maximo 15 caracteres, al menos una letra mayuscula, una letra minuscula, un numero y un caracter especial
        public const string PASSWORD_EXPRESSIONS = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&#])[A-Za-z\\d@$!%*?&#]{8,15}$";
    }
}
