using MovieCheck.Site.Models;
using MovieCheck.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieCheck.Site.Infra.Factory
{
    public static class PendenciaFactory
    {
        public static IList<PendenciaViewModel> ListarPendenciasUsuarioLogado(this IDataService dataService)
        {
            IList<PendenciaViewModel> listaPendenciaViewModel = new List<PendenciaViewModel>();

            foreach (Pendencia pendencia in dataService.ObterPendenciaPorUsuario(dataService.ObterUsuarioSessao()))
            {
                listaPendenciaViewModel.Add(new PendenciaViewModel(pendencia));
            }

            return listaPendenciaViewModel.OrderByDescending(p => p.Id).ToList();
        }

        public static void EfetuarReserva(this IDataService dataService, Filme filme)
        {
            try
            {
                if (filme.Disponivel())
                {
                    dataService.AdicionarPendencia(new Pendencia(dataService.ObterUsuarioSessao(), filme));
                }
                else
                {
                    throw new NewPendenciaFailedException("Filme não disponível.");
                }
            }
            catch (NewPendenciaFailedException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new NewPendenciaFailedException(e.Message);
            }
        }

        public static void CancelarReserva(this IDataService dataService, int idReserva)
        {
            try
            {
                Pendencia pendencia = dataService.ObterPendenciaPorId(idReserva);

                pendencia.CancelarReserva();

                dataService.EditarPendencia(pendencia);
            }
            catch (NewPendenciaFailedException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new NewPendenciaFailedException($"Falha ao cancelar a reserva. {e.Message}");
            }
            
        }
    }
}
