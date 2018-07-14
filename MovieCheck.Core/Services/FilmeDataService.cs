using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MovieCheck.Core.Context;
using MovieCheck.Core.Interface.Services;
using MovieCheck.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace MovieCheck.Core.Services
{
    public class FilmeDataService :IFilmeDataService 
    {
        #region Atributos
        private readonly MovieCheckContext _contexto;
        #endregion

        #region Construtores
        public FilmeDataService(MovieCheckContext contexto)
        {
            this._contexto = contexto;
        }
        #endregion

        #region Filme
        private void IniciarBancoFilme()
        {
            Filme filme;
            if (!_contexto.Filme.Any(f => f.Titulo == "Capitão América: Guerra Civil"))
            {
                //Primeiro filme, primeiro exemplar
                filme = new Filme()
                {
                    Titulo = "Capitão América: Guerra Civil",
                    Ano = 2016,
                    Poster = "https://image.tmdb.org/t/p/w600_and_h900_bestv2//bL3Pp80AUgmwOE9K2EMcMH8566W.jpg",
                    Sinopse = "O ataque de Ultron faz com que os políticos decidam controlar os Vingadores, já que seus atos afetam toda a humanidade. Tal decisão coloca o Capitão América em rota de colisão com o Homem de Ferro.",
                    Midia = "0",
                    ClassificacaoIndicativa = _contexto.Classificacao.Where(c => c.ClassificacaoIndicativa == "12").FirstOrDefault()
                };

                AdicionarDiretoresAoFilme(filme, new List<Diretor>()
                {
                    new Diretor("Anthony Russo"),
                    new Diretor("Joe Russo")
                });

                AdicionarGenerosAoFilme(filme, new List<Genero>()
                {
                    new Genero("Ação"),
                    new Genero("Ficção Científica")
                });

                AdicionarAtoresAoFilme(filme, new List<Ator>()
                {
                    new Ator("Chris Evans"),
                    new Ator("Robert Downey Jr."),
                    new Ator("Sebastian Stan"),
                    new Ator("Scarlett Johansson"),
                    new Ator("Elizabeth Olsen"),
                    new Ator("Anthony Mackie"),
                    new Ator("Jeremy Renner"),
                    new Ator("Chadwick Boseman"),
                    new Ator("Paul Bettany"),
                    new Ator("Emily VanCamp"),
                    new Ator("Paul Rudd"),
                    new Ator("Daniel Brühl"),
                    new Ator("Do Cheadle"),
                    new Ator("Tom Holland")
                });

                _contexto.Filme.Add(filme);
                _contexto.SaveChanges();

                //Primeiro filme, segundo exemplar
                this.AdicionarExemplar(filme, "0");

                //Primeiro filme, terceiro exemplar
                this.AdicionarExemplar(filme, "1");
            }

            if (!_contexto.Filme.Any(f => f.Titulo == "Batman vs. Superman: A Origem da Justiça"))
            {
                //Segundo filme, primeiro exemplar
                filme = new Filme()
                {
                    Titulo = "Batman vs. Superman: A Origem da Justiça",
                    Ano = 2016,
                    Poster = "http://warwick-place.com/wp-content/uploads/2016/02/5203-thumb.jpg",
                    Sinopse = "O confronto entre Superman e Zod em Metrópolis fez com que a população mundial se dividisse acerca da existência de extraterrestres na Terra. Enquanto muitos consideram o Superman como um novo deus, há aqueles que consideram extremamente perigoso que haja um ser tão poderoso sem qualquer tipo de controle. Bruce Wayne é um dos que acreditam nesta segunda hipótese. Com isso, sob o manto de um Batman violento e obcecado, eles se enfrentam enquanto o mundo se pergunta que tipo de herói precisa.",
                    Midia = "1",
                    ClassificacaoIndicativa = _contexto.Classificacao.Where(c => c.ClassificacaoIndicativa == "12").FirstOrDefault()
                };

                AdicionarDiretoresAoFilme(filme, new List<Diretor>()
                {
                    new Diretor("Zack Snyder")
                });

                AdicionarGenerosAoFilme(filme, new List<Genero>()
                {
                    new Genero("Ação"),
                    new Genero("Fantasia"),
                    new Genero("Ficção Científica")
                });

                AdicionarAtoresAoFilme(filme, new List<Ator>()
                {
                    new Ator("Ben Affleck"),
                    new Ator("Gal Gadot"),
                    new Ator("Henry Cavill"),
                    new Ator("Jesse Eisenberg"),
                    new Ator("Amy Adams")
                });

                _contexto.Filme.Add(filme);
                _contexto.SaveChanges();

                //Segundo filme, segundo exemplar
                this.AdicionarExemplar(filme, "0");

                //Segundo filme, terceiro exemplar
                this.AdicionarExemplar(filme, "1");

                //Segundo filme, quarto exemplar
                this.AdicionarExemplar(filme, "1");
            }

            if (!_contexto.Filme.Any(f => f.Titulo == "Interestelar"))
            {
                //Terceiro filme, primeiro exemplar
                filme = new Filme()
                {
                    Titulo = "Interestelar",
                    Ano = 2014,
                    Poster = "http://br.web.img3.acsta.net/pictures/14/10/31/20/39/476171.jpg",
                    Sinopse = "As reservas naturais da Terra estão chegando ao fim e um grupo de astronautas recebe a missão de verificar possíveis planetas para receberem a população mundial, possibilitando a continuação da espécie. Cooper é chamado para liderar o grupo e aceita a missão sabendo que pode nunca mais ver os filhos. Ao lado de Brand, Jenkins e Doyle, ele seguirá em busca de um novo lar.",
                    Midia = "1",
                    ClassificacaoIndicativa = _contexto.Classificacao.Where(c => c.ClassificacaoIndicativa == "10").FirstOrDefault()
                };

                AdicionarDiretoresAoFilme(filme, new List<Diretor>()
                {
                    new Diretor("Christopher Nolan")
                });

                AdicionarGenerosAoFilme(filme, new List<Genero>()
                {
                    new Genero("Ação"),
                    new Genero("Fantasia"),
                    new Genero("Ficção Científica")
                });

                AdicionarAtoresAoFilme(filme, new List<Ator>()
                {
                    new Ator("Matthew McConaughey"),
                    new Ator("Timothée Chalamet"),
                    new Ator("Anne Hathaway"),
                    new Ator("Jessica Chastain"),
                    new Ator("Mackenzie Foy")
                });

                _contexto.Filme.Add(filme);
                _contexto.SaveChanges();

                //Terceiro filme, segundo exemplar
                this.AdicionarExemplar(filme, "0");

                //Terceiro filme, terceiro exemplar
                this.AdicionarExemplar(filme, "0");
            }
        }

        private Filme AdicionarAtoresAoFilme(Filme filme, IList<Ator> atores)
        {
            foreach (Ator ator in atores)
            {
                if (_contexto.Ator.Any(a => a.Nome == ator.Nome))
                {
                    filme.AdicionarAtor(_contexto.Ator.Where(a => a.Nome == ator.Nome).FirstOrDefault());
                }
                else
                {
                    filme.AdicionarAtor(ator);
                }
            }

            return filme;
        }

        private Filme AdicionarDiretoresAoFilme(Filme filme, IList<Diretor> diretores)
        {
            foreach (Diretor diretor in diretores)
            {
                if (_contexto.Diretor.Any(d => d.Nome == diretor.Nome))
                {
                    filme.AdicionarDiretor(_contexto.Diretor.Where(d => d.Nome == diretor.Nome).FirstOrDefault());
                }
                else
                {
                    filme.AdicionarDiretor(diretor);
                }
            }

            return filme;
        }

        private Filme AdicionarGenerosAoFilme(Filme filme, IList<Genero> generos)
        {
            foreach (Genero genero in generos)
            {
                if (_contexto.Genero.Any(gen => gen.Descricao == genero.Descricao))
                {
                    filme.AdicionarGenero(_contexto.Genero.Where(g => g.Descricao == genero.Descricao).FirstOrDefault());
                }
                else
                {
                    filme.AdicionarGenero(genero);
                }
            }

            return filme;
        }

        public void AdicionarFilme(Filme filme, IList<Ator> atores, IList<Diretor> diretores, IList<Genero> generos, Classificacao classificacao)
        {
            filme = AdicionarAtoresAoFilme(filme, atores);
            filme = AdicionarDiretoresAoFilme(filme, diretores);
            filme = AdicionarGenerosAoFilme(filme, generos);
            filme.ClassificacaoIndicativa = classificacao;

            _contexto.Filme.Add(filme);
            _contexto.SaveChanges();

        }

        public void ExcluirTitulo(string titulo)
        {
            var listaPendencia = _contexto.Pendencia
                .Include(f => f.Filme)
                .Where(pen => pen.Filme.Titulo == titulo).ToList();

            foreach (Pendencia pendencia in listaPendencia)
            {
                _contexto.Pendencia.Remove(pendencia);
            }

            var listaFilme = _contexto.Filme.Where(f => f.Titulo == titulo).ToList();

            foreach (Filme filme in _contexto.Filme.Where(f => f.Titulo == titulo).ToList())
            {
                _contexto.Filme.Remove(filme);
            }

            _contexto.SaveChanges();
        }

        public bool TituloJaExiste(string titulo)
        {
            if (_contexto.Filme.Any(f => f.Titulo == titulo))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AdicionarExemplar(Filme filme, string midia)
        {
            //Parâmetro mídia: 0 pra DVD e 1 pra Blu-Ray
            //Adicionar informações do filme
            Filme novoExemplar = new Filme()
            {
                Titulo = filme.Titulo,
                Ano = filme.Ano,
                Poster = filme.Poster,
                Sinopse = filme.Sinopse,
                Midia = midia,
                ClassificacaoIndicativa = filme.ClassificacaoIndicativa
            };

            //Atores
            foreach (Ator ator in filme.ObterAtores())
            {
                novoExemplar.AdicionarAtor(ator);
            }

            //Diretores
            foreach (Diretor diretor in filme.ObterDiretores())
            {
                novoExemplar.AdicionarDiretor(diretor);
            }

            //Generos
            foreach (Genero genero in filme.ObterGeneros())
            {
                novoExemplar.AdicionarGenero(genero);
            }

            //Contexto
            _contexto.Filme.Add(novoExemplar);
            _contexto.SaveChanges();
        }

        public void EditarTitulo(Filme filme, IList<Ator> atores, IList<Diretor> diretores, IList<Genero> generos)
        {
            Filme filmeEditar;

            foreach (var filmeBanco in ObterListaFilmePorTitulo(filme.Titulo))
            {
                filmeEditar = filmeBanco;

                filmeEditar.AtualizarTitulo(filme);
                filmeEditar.RemoverAtores();
                filmeEditar = AdicionarAtoresAoFilme(filmeEditar, atores);
                filmeEditar.RemoverDiretores();
                filmeEditar = AdicionarDiretoresAoFilme(filmeEditar, diretores);
                filmeEditar.RemoverGeneros();
                filmeEditar = AdicionarGenerosAoFilme(filmeEditar, generos);

                _contexto.Filme.Update(filmeEditar);
            }

            _contexto.SaveChanges();
        }

        public void ExcluirExemplar(Filme filme)
        {
            foreach (Pendencia pendencia in _contexto.Pendencia.Where(pen => pen.Filme.Id == filme.Id).ToList())
            {
                _contexto.Pendencia.Remove(pendencia);
            }

            _contexto.Remove(filme);
            _contexto.SaveChanges();
        }

        public Filme ObterFilmePorId(int id)
        {
            return _contexto.Filme
                .Include(a => a.Atores).ThenInclude(af => af.Ator)
                .Include(d => d.Diretores).ThenInclude(df => df.Diretor)
                .Include(c => c.ClassificacaoIndicativa)
                .Include(g => g.Generos).ThenInclude(gf => gf.Genero)
                .Include(p => p.Pendencias)
                .Where(f => f.Id == id)
                .FirstOrDefault();
        }

        public IList<Filme> ObterCatalogoCompleto()
        {
            return _contexto.Filme.Distinct()
                .Include(a => a.Atores).ThenInclude(af => af.Ator)
                .Include(d => d.Diretores).ThenInclude(df => df.Diretor)
                .Include(c => c.ClassificacaoIndicativa)
                .Include(g => g.Generos).ThenInclude(g => g.Genero)
                .Include(p => p.Pendencias)
                .ToList();
        }

        public IList<Filme> ObterListaDvds()
        {
            return _contexto.Filme.Distinct()
                .Include(a => a.Atores).ThenInclude(af => af.Ator)
                .Include(d => d.Diretores).ThenInclude(df => df.Diretor)
                .Include(c => c.ClassificacaoIndicativa)
                .Include(g => g.Generos).ThenInclude(g => g.Genero)
                .Include(p => p.Pendencias)
                .Where(f => f.Midia == "0").ToList();
        }

        public IList<Filme> ObterListaBluRays()
        {
            return _contexto.Filme.Distinct()
                .Include(a => a.Atores).ThenInclude(af => af.Ator)
                .Include(d => d.Diretores).ThenInclude(df => df.Diretor)
                .Include(c => c.ClassificacaoIndicativa)
                .Include(g => g.Generos).ThenInclude(g => g.Genero)
                .Include(p => p.Pendencias)
                .Where(f => f.Midia == "1").ToList();
        }

        public IList<Filme> ObterListaFilmePorTitulo(string titulo)
        {
            return _contexto.Filme.Distinct()
                .Include(a => a.Atores).ThenInclude(af => af.Ator)
                .Include(d => d.Diretores).ThenInclude(df => df.Diretor)
                .Include(c => c.ClassificacaoIndicativa)
                .Include(g => g.Generos).ThenInclude(g => g.Genero)
                .Include(p => p.Pendencias)
                .Where(f => f.Titulo.Contains(titulo)).ToList();
        }

        public IList<Filme> PesquisaAvancada(string titulo, int ano, string ator, string diretor, string classificacao, IList<string> listaGenero)
        {
            //Será estudado futuramente a transformação deste método em uma SP ou UF.

            //Preparar IDs para parâmetros da pesquisa.
            Ator a = _contexto.Ator.Where(at => at.Nome.Contains(ator)).FirstOrDefault();

            Diretor d = _contexto.Diretor.Where(di => di.Nome.Contains(diretor)).FirstOrDefault();

            Classificacao cla = _contexto.Classificacao.Where(clas => clas.ClassificacaoIndicativa == classificacao).FirstOrDefault();

            IList<Genero> listaGen = new List<Genero>();
            foreach (string genero in listaGenero)
            {
                listaGen.Add(_contexto.Genero.Where(g => g.Descricao == genero).FirstOrDefault());
            }

            //Pegar todos os filmes do banco
            IList<Filme> listaFilme = ObterCatalogoCompleto();

            //Filtro por título
            if (!string.IsNullOrEmpty(titulo))
            {
                foreach (Filme filme in listaFilme)
                {
                    if (!filme.Titulo.Contains(titulo))
                    {
                        listaFilme.Remove(filme);
                    }
                }
            }

            //Filtro por ator
            if (!(a is null))
            {
                foreach (Filme filme in listaFilme)
                {
                    if (!filme.ObterAtores().Contains(a))
                    {
                        listaFilme.Remove(filme);
                    }
                }
            }

            //Filtro por diretor
            if (!(d is null))
            {
                foreach (Filme filme in listaFilme)
                {
                    if (!filme.ObterDiretores().Contains(d))
                    {
                        listaFilme.Remove(filme);
                    }
                }
            }

            //Filtro por classificação
            if (!(cla is null))
            {
                foreach (Filme filme in listaFilme)
                {
                    if (filme.ClassificacaoId != cla.Id)
                    {
                        listaFilme.Remove(filme);
                    }
                }
            }

            //Filtro por gênero
            bool validacao = false;
            if (listaGen.Count > 0)
            {
                foreach (Filme filme in listaFilme)
                {
                    foreach (Genero gen in listaGen)
                    {
                        if (filme.ObterGeneros().Contains(gen))
                        {
                            validacao = true;
                            break;
                        }
                    }

                    if (!validacao)
                    {
                        listaFilme.Remove(filme);
                    }
                }
            }

            //Após todos os filtros, retorna os filmes que sobraram.
            return listaFilme;
        }
        #endregion

        #region Ator
        public Ator ObterAtorPorNome(string nome)
        {
            return _contexto.Ator.Where(a => a.Nome == nome).FirstOrDefault();
        }

        public void AdicionarAtor(Ator ator)
        {
            _contexto.Ator.Add(ator);
            _contexto.SaveChanges();
        }
        #endregion

        #region Diretor
        public Diretor ObterDiretorPorNome(string nome)
        {
            return _contexto.Diretor.Where(d => d.Nome == nome).FirstOrDefault();
        }

        public void AdicionarDiretor(Diretor diretor)
        {
            _contexto.Diretor.Add(diretor);
            _contexto.SaveChanges();
        }
        #endregion

        #region Genero
        public IList<Genero> ObterListaGenero()
        {
            return _contexto.Genero.ToList();
        }

        public Genero ObterGeneroPorDescricao(string descricao)
        {
            return _contexto.Genero.Where(g => g.Descricao == descricao).FirstOrDefault();
        }

        public void AdicionarGenero(Genero genero)
        {
            _contexto.Genero.Add(genero);
            _contexto.SaveChanges();
        }

        private void IniciarBancoGenero()
        {
            IList<Genero> listaGenero = new List<Genero>()
            {
                new Genero(){ Descricao = "Ação" },
                new Genero(){ Descricao = "Animação" },
                new Genero(){ Descricao = "Aventura" },
                new Genero(){ Descricao = "Biografias" },
                new Genero(){ Descricao = "Comédia" },
                new Genero(){ Descricao = "Fantasia" },
                new Genero(){ Descricao = "Faroeste" },
                new Genero(){ Descricao = "Ficção Científica" },
                new Genero(){ Descricao = "Ficção Histórica" },
                new Genero(){ Descricao = "Guerra" },
                new Genero(){ Descricao = "Infantil" },
                new Genero(){ Descricao = "Mistério" },
                new Genero(){ Descricao = "Musicais" },
                new Genero(){ Descricao = "Policial" },
                new Genero(){ Descricao = "Romance" },
                new Genero(){ Descricao = "Suspense" },
                new Genero(){ Descricao = "Terror" }
            };

            foreach (Genero genero in listaGenero)
            {
                if (!_contexto.Genero.Any(g => g.Descricao == genero.Descricao))
                {
                    _contexto.Genero.Add(genero);
                }
            }

            if (VerificarAdicao(_contexto.ChangeTracker.Entries()))
            {
                _contexto.SaveChanges();
            }
        }
        #endregion

        #region Classificacao
        public IList<Classificacao> ObterListaClassificacao()
        {
            return _contexto.Classificacao.ToList();
        }

        public Classificacao ObterClassificacaoPorSigla(string sigla)
        {
            return _contexto.Classificacao.Where(c => c.ClassificacaoIndicativa == sigla).FirstOrDefault();
        }

        public Classificacao ObterClassificacaoPorDescricao(string descricao)
        {
            return _contexto.Classificacao.Where(c => c.Descricao == descricao).FirstOrDefault();
        }

        public void AdicionarClassificacao(Classificacao classificacao)
        {
            _contexto.Classificacao.Add(classificacao);
            _contexto.SaveChanges();
        }

        public bool ExisteClassificacao(Classificacao classificacao)
        {
            if (_contexto.Classificacao.Any(c => c.Id == (!string.IsNullOrEmpty(classificacao.Id.ToString()) ? classificacao.Id : 0) || (c.ClassificacaoIndicativa == classificacao.ClassificacaoIndicativa && c.Descricao == classificacao.Descricao)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void IniciarBancoClassificacao()
        {
            IList<Classificacao> listaClassificacao = new List<Classificacao>()
            {
                new Classificacao() { ClassificacaoIndicativa = "l", Descricao = "Livre" },
                new Classificacao() { ClassificacaoIndicativa = "10", Descricao = "Não recomendado para menores de 10 anos" },
                new Classificacao() { ClassificacaoIndicativa = "12", Descricao = "Não recomendado para menores de 12 anos" },
                new Classificacao() { ClassificacaoIndicativa = "14", Descricao = "Não recomendado para menores de 14 anos" },
                new Classificacao() { ClassificacaoIndicativa = "16", Descricao = "Não recomendado para menores de 16 anos" },
                new Classificacao() { ClassificacaoIndicativa = "18", Descricao = "Não recomendado para menores de 18 anos" }
            };

            foreach (Classificacao classificacao in listaClassificacao)
            {
                if (!_contexto.Classificacao.Any(c => c.ClassificacaoIndicativa == classificacao.ClassificacaoIndicativa && c.Descricao == classificacao.Descricao))
                {
                    _contexto.Classificacao.Add(classificacao);
                }
            }

            if (VerificarAdicao(_contexto.ChangeTracker.Entries()))
            {
                _contexto.SaveChanges();
            }
        }

        private bool VerificarAdicao(IEnumerable<EntityEntry> entityEntries)
        {
            bool resultado = false;

            foreach (var entry in entityEntries)
            {
                if (entry.State.ToString() == "Added")
                {
                    resultado = true;
                }
            }

            return resultado;
        }
        #endregion
    }
}
