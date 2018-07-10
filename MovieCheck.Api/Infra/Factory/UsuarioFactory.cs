using MovieCheck.Api.Infra.Exceptions;
using MovieCheck.Api.Models;
using System;
using System.Collections.Generic;

namespace MovieCheck.Api.Infra.Factory
{
    public static class UsuarioFactory
    {
        public static Usuario ObterUsuario(this IDataService dataService, int id)
        {
            var usuario = dataService.ObterUsuarioPorId(id);

            if (!(usuario is null))
            {
                return usuario;
            }
            else
            {
                throw new NotFoundException("Usuário não encontrado.");
            }
        }

        public static Usuario ObterUsuarioCompleto(this IDataService dataService, int id)
        {
            var usuario = dataService.ObterUsuarioCompletoPorId(id);

            if (!(usuario is null))
            {
                return usuario;
            }
            else
            {
                throw new NotFoundException("Usuário não encontrado");
            }
        }

        public static Cliente ObterCliente(this IDataService dataService, int id)
        {
            try
            {
                var cliente = dataService.ObterUsuario(id);

                if (dataService.ObterTipoUsuario(cliente).Equals("Cliente"))
                {
                    return (Cliente)cliente;
                }
                else
                {
                    throw new Exception("Cliente inválido.");
                }
            }
            catch (NotFoundException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Cliente ObterClienteCompleto(this IDataService dataService, int id)
        {
            try
            {
                var cliente = dataService.ObterUsuarioCompleto(id);

                if (dataService.ObterTipoUsuario(cliente).Equals("Cliente"))
                {
                    return (Cliente)cliente;
                }
                else
                {
                    throw new Exception("Cliente inválido.");
                }
            }
            catch (NotFoundException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Dependente ObterDependente(this IDataService dataService, int id)
        {
            try
            {
                var dependente = dataService.ObterUsuario(id);

                if (dataService.ObterTipoUsuario(dependente).Equals("Dependente"))
                {
                    return (Dependente)dependente;
                }
                else
                {
                    throw new Exception("Dependente inválido.");
                }
            }
            catch (NotFoundException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Dependente ObterDependenteCompleto(this IDataService dataService, int id)
        {
            try
            {
                var dependente = dataService.ObterUsuarioCompleto(id);

                if (dataService.ObterTipoUsuario(dependente).Equals("Dependente"))
                {
                    return (Dependente)dependente;
                }
                else
                {
                    throw new Exception("Dependente inválido.");
                }
            }
            catch (NotFoundException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static void ValidarLista(IList<Cliente> listaCliente)
        {
            if (listaCliente.Count <= 0)
            {
                throw new NotFoundException("Não existem usuários cadastrados.");
            }
        }

        private static void ValidarLista(IList<Dependente> listaDependente)
        {
            if (listaDependente.Count <= 0)
            {
                throw new NotFoundException("Não existem usuários cadastrados.");
            }
        }

        public static IList<Cliente> ObterClientes(this IDataService dataService)
        {
            try
            {
                var listaClienteBanco = dataService.ObterListaUsuario();
                var listaCliente = new List<Cliente>();

                foreach (var cliente in listaClienteBanco)
                {
                    if (dataService.ObterTipoUsuario(cliente).Equals("Cliente"))
                    {
                        listaCliente.Add((Cliente)cliente);
                    }
                }

                ValidarLista(listaCliente);

                return listaCliente;
            }
            catch (NotFoundException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static IList<Dependente> ObterDependentes(this IDataService dataService)
        {
            try
            {
                var listaDependenteBanco = dataService.ObterListaUsuario();
                var listaDependente = new List<Dependente>();

                foreach (var dependente in listaDependenteBanco)
                {
                    if (dataService.ObterTipoUsuario(dependente).Equals("Dependente"))
                    {
                        listaDependente.Add((Dependente)dependente);
                    }
                }
                ValidarLista(listaDependente);

                return listaDependente;
            }
            catch (NotFoundException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
