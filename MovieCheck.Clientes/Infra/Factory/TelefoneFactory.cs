using MovieCheck.Clientes.Models;
using System;
using System.Collections.Generic;

namespace MovieCheck.Clientes.Infra.Factory
{
    public static class TelefoneFactory
    {
        private static bool IsValidDdd(int ddd)
        {
            var listaDdd = new List<int>() { 11, 12, 13, 14, 15, 16, 17, 18, 19, 21, 22, 24, 27, 28, 31, 31, 32, 33, 34, 35, 37, 38, 41, 42, 43, 44, 45, 46, 47, 48, 49, 51, 53, 54, 55, 61, 62, 23, 64, 65, 66, 67, 68, 69, 71, 73, 74, 75, 77, 79, 81, 82, 83, 84, 85, 86, 87, 88, 89, 91, 92, 93, 94, 95, 96, 97, 98, 99 };
            //11, 12, 13, 14, 15, 16, 17, 18, 19, 21, 22, 24, 27, 28, 31, 31, 32, 33, 34, 35, 37, 38, 41, 42, 43, 44, 45, 46, 47, 48, 49, 51, 53, 54, 55, 61, 62, 23, 64, 65, 66, 67, 68, 69, 71, 73, 74, 75, 77, 79, 81, 82, 83, 84, 85, 86, 87, 88, 89, 91, 92, 93, 94, 95, 96, 97, 98, 99);

            if (ddd.ToString().Length == 2)
            {
                if (listaDdd.Contains(ddd))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static Telefone ValidaTelefone(string tipo, string numeroCompleto)
        {
            int tipoTelefone;
            string ddd;
            string numero;

            if (tipo == "celular")
            {
                tipoTelefone = 1;
                ddd = numeroCompleto.Substring(0, 2);
                numero = numeroCompleto.Substring(2, 9);
            }
            else
            {
                tipoTelefone = 0;
                ddd = numeroCompleto.Substring(0, 2);
                numero = numeroCompleto.Substring(2, 8);
            }

            return ValidaTelefone(tipoTelefone, ddd, numero);
        }

        public static Telefone ValidaTelefone (int tipo, string ddd, string numero)
        {
            if(DefaultFactory.IsNumeric(ddd))
            {
                int dddConvertido = Convert.ToInt32(ddd);
                if (IsValidDdd(dddConvertido))
                {
                    if (DefaultFactory.IsNumeric(numero))
                    {
                        return new Telefone() { Tipo = tipo, Ddd = dddConvertido, Numero = numero };
                    }
                    else
                    {
                        throw new NewUserFailedException("Número de telefone inválido.");
                    }
                }
                else
                {
                    throw new NewUserFailedException("DDD inválido.");
                }
            }
            else
            {
                throw new NewUserFailedException("DDD inválido.");
            }
        }
    }
}
