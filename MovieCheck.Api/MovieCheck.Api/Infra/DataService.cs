﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MovieCheck.Api.Models;

namespace MovieCheck.Api.Infra
{
    public class DataService : IDataService
    {
        #region Atributos
        private readonly MovieCheckContext _contexto;
        #endregion

        #region Construtores
        public DataService(MovieCheckContext contexto)
        {
            this._contexto = contexto;
        }
        #endregion

        #region Banco
        public void IniciarDb()
        {
            this._contexto.Database.Migrate();
            IniciarBancoGenero();
            IniciarBancoClassificacao();
            IniciarBancoFilme();
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

        public void HigienizaTelefone()
        {
            _contexto.Database.ExecuteSqlCommand(@"EXEC sp_higieniza_telefone");
        }

        public void HigienizaEndereco()
        {
            _contexto.Database.ExecuteSqlCommand(@"EXEC sp_higieniza_endereco");
        }

        public void HigienizaAtor()
        {
            _contexto.Database.ExecuteSqlCommand(@"EXEC sp_higieniza_ator");
        }

        public void HigienizaDiretor()
        {
            _contexto.Database.ExecuteSqlCommand(@"EXEC sp_higieniza_diretor");
        }
        #endregion

        #region Usuario
        public IList<Usuario> ObterListaUsuario()
        {
            return _contexto.Usuario
                .Include(e => e.Endereco)
                .ToList();
        }

        public Usuario ObterUsuarioCompletoPorId(int id)
        {
            var usuario = this._contexto.Usuario.Find(id);

            if (!(usuario is null))
            {
                if (this.ObterTipoUsuario(usuario) == "Cliente")
                {
                    Cliente cliente = _contexto.Cliente
                        .Include(t => t.Telefones).ThenInclude(ut => ut.Telefone)
                        .Include(e => e.Endereco)
                        .Include(d => d.Dependentes)
                        .Include(p => p.Pendencias).ThenInclude(f => f.Filme)
                        .Where(c => c.Id == usuario.Id)
                        .FirstOrDefault();

                    return cliente;
                }
                else
                {
                    Dependente dependente = _contexto.Dependente
                        .Include(t => t.Telefones).ThenInclude(ut => ut.Telefone)
                        .Include(e => e.Endereco)
                        .Include(d => d.Cliente)
                        .Where(c => c.Id == usuario.Id)
                        .FirstOrDefault();

                    return dependente;
                }
            }
            else
            {
                return null;
            }
        }

        public Usuario ObterUsuarioPorId(int id)
        {
            return this._contexto.Usuario
                .Include(e => e.Endereco)
                .Where(u => u.Id == id)
                .FirstOrDefault();
        }

        public Usuario ObterUsuarioPorEmail(string email)
        {
            return this._contexto.Usuario
                .Include(t => t.Telefones).ThenInclude(ut => ut.Telefone)
                .Include(e => e.Endereco)
                .Include(p => p.Pendencias).ThenInclude(f => f.Filme)
                .Where(u => u.Email == email)
                .FirstOrDefault();
        }

        public string ObterTipoUsuario(Usuario usuario)
        {
            return _contexto.Entry(usuario).Property("Discriminator").CurrentValue.ToString();
        }

        public bool VerificarUsuarioPorEmail(string email)
        {
            return _contexto.Usuario.Any(u => u.Email == email);
        }

        public void EditarUsuario(Usuario usuarioEditado)
        {
            var usuarioBanco = this.ObterUsuarioPorId(usuarioEditado.Id);

            usuarioBanco.AtualizarUsuario(usuarioEditado);

            //VERIFICA TELEFONE
            //FIXO
            if (usuarioEditado.ExisteTelefoneFixo())
            {
                if (!usuarioBanco.ObterTelefoneFixo().Equals(usuarioEditado.ObterTelefoneFixo()))
                {
                    if (this.ExisteTelefone(usuarioEditado.ObterTelefoneFixo()))
                    {
                        var fixo = this.ObterTelefone(usuarioEditado.ObterTelefoneFixo());
                        usuarioBanco.EditarTelefoneFixo(fixo);
                    }
                    else
                    {
                        usuarioBanco.AdicionarTelefone(usuarioEditado.ObterTelefoneFixo());
                    }
                }
            }
            else
            {
                usuarioBanco.RemoverTelefoneFixo();
            }

            //CELULAR
            if (usuarioEditado.ExisteTelefoneCelular())
            {
                if (!usuarioBanco.ObterTelefoneCelular().Equals(usuarioEditado.ObterTelefoneCelular()))
                {
                    if (this.ExisteTelefone(usuarioEditado.ObterTelefoneCelular()))
                    {
                        var celular = this.ObterTelefone(usuarioEditado.ObterTelefoneCelular());
                        usuarioBanco.EditarTelefoneCelular(celular);
                    }
                    else
                    {
                        usuarioBanco.AdicionarTelefone(usuarioEditado.ObterTelefoneCelular());
                    }
                }
            }
            else
            {
                usuarioBanco.RemoverTelefoneCelular();
            }

            //ATUALIZAR CONTEXTO
            _contexto.Usuario.Update(usuarioBanco);
            _contexto.SaveChanges();
        }

        public void ExcluirUsuario(Usuario usuario)
        {
            if (this.ObterTipoUsuario(usuario) != "Dependente")
            {
                var cliente = (Cliente)usuario;
                if (cliente.Dependentes.Count > 0)
                {
                    foreach (var dependente in cliente.Dependentes)
                    {
                        _contexto.Dependente.Remove(dependente);
                    }
                }
            }
            _contexto.Usuario.Remove(usuario);
            _contexto.SaveChanges();
        }

        public void AlterarSenha(Usuario usuario, string novaSenha)
        {
            usuario.Senha = Util.HashPassword(novaSenha);
            _contexto.Usuario.Update(usuario);
            _contexto.SaveChanges();
        }
        #endregion

        #region Cliente
        public bool VerificarClientePorCpf(string cpf)
        {
            return _contexto.Cliente.Any(c => c.Cpf == cpf);
        }

        public void AdicionarCliente(Cliente cliente)
        {
            //VERIFICA TELEFONE
            //FIXO
            if (cliente.ExisteTelefoneFixo())
            {
                if (this.ExisteTelefone(cliente.ObterTelefoneFixo()))
                {
                    var fixo = ObterTelefone(cliente.ObterTelefoneFixo());
                    cliente.EditarTelefoneFixo(fixo);
                }
            }

            //CELULAR
            if (cliente.ExisteTelefoneCelular())
            {
                if (this.ExisteTelefone(cliente.ObterTelefoneCelular()))
                {
                    var celular = ObterTelefone(cliente.ObterTelefoneCelular());
                    cliente.EditarTelefoneFixo(celular);
                }
            }

            //ATUALIZA CONTEXTO
            _contexto.Cliente.Add(cliente);
            _contexto.SaveChanges();
        }

        public IList<string> ObterEmailAdministradores()
        {
            IList<string> listaEmail = new List<string>();
            var listaAdministradores = _contexto.Cliente.Where(c => c.Tipo == 1).ToList();

            foreach (Cliente administrador in listaAdministradores)
            {
                listaEmail.Add(administrador.Email);
            }

            return listaEmail;
        }
        
        public void RemoverCliente(Cliente cliente)
        {
            cliente.RemoverTelefoneFixo();
            cliente.RemoverTelefoneCelular();
            cliente.RemoverEndereco();
            foreach (Dependente dependente in cliente.Dependentes)
            {
                _contexto.Dependente.Remove(dependente);
            }
            _contexto.Cliente.Remove(cliente);
            _contexto.SaveChanges();
        }
        #endregion

        #region Dependente
        public IList<Dependente> ObterListaDependente()
        {
            return _contexto.Dependente
                .ToList();
        }

        public IList<Dependente> ObterListaDependente(Cliente cliente)
        {
            return _contexto.Dependente
                .Where(d => d.ClienteId == cliente.Id).ToList();
        }

        public Dependente ObterDependente(int id)
        {
            return _contexto.Dependente.Find();
        }

        public void AdicionarDependente(Cliente responsavel, Dependente dependente)
        {
            //VERIFICA TELEFONE
            //FIXO
            if (dependente.ExisteTelefoneFixo())
            {
                if (this.ExisteTelefone(dependente.ObterTelefoneFixo()))
                {
                    var fixo = ObterTelefone(dependente.ObterTelefoneFixo());
                    dependente.EditarTelefoneFixo(fixo);
                }
            }
            //CELULAR
            if (dependente.ExisteTelefoneCelular())
            {
                if (this.ExisteTelefone(dependente.ObterTelefoneCelular()))
                {
                    var celular = ObterTelefone(dependente.ObterTelefoneCelular());
                    dependente.EditarTelefoneFixo(celular);
                }
            }

            //ATRIBUI DEPENDENTE AO CLIENTE
            responsavel.AdicionarDependente(dependente);
            dependente.AtribuirResponsavel(responsavel);

            //ATUALIZA CONTEXTO
            _contexto.Dependente.Add(dependente);
            _contexto.Cliente.Update(responsavel);
            _contexto.SaveChanges();
        }
        
        public void RemoverDependente(Dependente dependente)
        {
            dependente.RemoverTelefoneFixo();
            dependente.RemoverTelefoneCelular();
            dependente.RemoverEndereco();
            _contexto.Dependente.Remove(dependente);
            _contexto.SaveChanges();
        }

        public void EditarDependente(int idDependente, Dependente dependenteEditado)
        {
            var dependenteBanco = ObterDependente(idDependente);

            dependenteBanco.AtualizarUsuario(dependenteEditado);

            //VERIFICA TELEFONE
            //FIXO
            if (dependenteEditado.ExisteTelefoneFixo())
            {
                if (!dependenteBanco.ObterTelefoneFixo().Equals(dependenteEditado.ObterTelefoneFixo()))
                {
                    if (this.ExisteTelefone(dependenteEditado.ObterTelefoneFixo()))
                    {
                        var fixo = this.ObterTelefone(dependenteEditado.ObterTelefoneFixo());
                        dependenteBanco.EditarTelefoneFixo(fixo);
                    }
                    else
                    {
                        dependenteBanco.AdicionarTelefone(dependenteEditado.ObterTelefoneFixo());
                    }
                }
            }
            else
            {
                dependenteBanco.RemoverTelefoneFixo();
            }

            //CELULAR
            if (dependenteEditado.ExisteTelefoneCelular())
            {
                if (!dependenteBanco.ObterTelefoneCelular().Equals(dependenteEditado.ObterTelefoneCelular()))
                {
                    if (this.ExisteTelefone(dependenteEditado.ObterTelefoneCelular()))
                    {
                        var celular = this.ObterTelefone(dependenteEditado.ObterTelefoneCelular());
                        dependenteBanco.EditarTelefoneCelular(celular);
                    }
                    else
                    {
                        dependenteBanco.AdicionarTelefone(dependenteEditado.ObterTelefoneCelular());
                    }
                }
            }
            else
            {
                dependenteBanco.RemoverTelefoneCelular();
            }

            dependenteBanco.AtribuirResponsavel(dependenteEditado.Cliente);

            //ATUALIZAR CONTEXTO
            _contexto.Dependente.Update(dependenteBanco);
            _contexto.SaveChanges();
        }
        #endregion

        #region Telefone
        public bool ExisteTelefone(Telefone telefone)
        {
            if (_contexto.Telefone.Any(t => t.Tipo == telefone.Tipo && t.Ddd == telefone.Ddd && t.Numero == telefone.Numero))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ExisteOutroUsuarioTelefone(Usuario usuario, Telefone telefone)
        {
            var tel = _contexto.Telefone.Include(u => u.Usuarios).ThenInclude(ut => ut.Usuario).Where(t => t.Equals(telefone)).FirstOrDefault();
            if (tel.Usuarios.Any(u => !u.Usuario.Equals(usuario)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Telefone ObterTelefone(Telefone telefone)
        {
            return _contexto.Telefone.Where(t => t.Tipo == telefone.Tipo && t.Ddd == telefone.Ddd && t.Numero == telefone.Numero ).FirstOrDefault();
        }

        public Telefone ObterTelefonePorId(int id)
        {
            return _contexto.Telefone.Find(id);
        }

        public IList<Telefone> ObterTelefonesPorUsuario(Usuario usuario)
        {
            var listaTelefone = new List<Telefone>();

            if (usuario.ExisteTelefoneFixo())
            {
                listaTelefone.Add(usuario.ObterTelefoneFixo());
            }

            if (usuario.ExisteTelefoneCelular())
            {
                listaTelefone.Add(usuario.ObterTelefoneCelular());
            }

            return listaTelefone;
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
                if(_contexto.Ator.Any(a => a.Nome == ator.Nome))
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

        public IList<Filme> PesquisaAvancada (string titulo, int ano, string ator, string diretor, string classificacao, IList<string> listaGenero)
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
            IList <Genero> listaGenero = new List<Genero>()
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
            if (_contexto.Classificacao.Any(c => c.Id == (!string.IsNullOrEmpty(classificacao.Id.ToString()) ? classificacao.Id : 0)  || (c.ClassificacaoIndicativa == classificacao.ClassificacaoIndicativa && c.Descricao == classificacao.Descricao)))
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
                if(!_contexto.Classificacao.Any(c => c.ClassificacaoIndicativa == classificacao.ClassificacaoIndicativa && c.Descricao == classificacao.Descricao))
                {
                    _contexto.Classificacao.Add(classificacao);
                }
            }
            
            if (VerificarAdicao(_contexto.ChangeTracker.Entries()))
            {
                _contexto.SaveChanges();
            }
        }
        #endregion

        #region Pendencia
        public IList<Pendencia> ObterListaPendencia()
        {
            return _contexto.Pendencia
                .Include(f => f.Filme)
                .Include(u => u.Usuario)
                .ToList();
        }

        public bool ExistePendencia(Filme filme)
        {
            //O filme possui disponibilidade quando existe um filme 
            //com o mesmo título e mídia com situação que não seja 
            //reservado ou alugado.
            return _contexto.Pendencia.Any(p => p.Filme.Titulo == filme.Titulo && 
                                                p.Filme.Midia == filme.Midia && 
                                                (p.Status == "0" || p.Status == "2"));
        }

        public IList<Pendencia> ObterPendenciaPorUsuario(Usuario usuario)
        {
            return _contexto.Pendencia
                .Include(f => f.Filme)
                .Where(p => p.UsuarioId == usuario.Id)
                .ToList();
        }

        public Pendencia ObterPendenciaPorId(int id)
        {
            return _contexto.Pendencia
                .Include(f => f.Filme)
                .Include(u => u.Usuario)
                .Where(p => p.Id == id)
                .FirstOrDefault();
        }

        public void AdicionarPendencia(Pendencia pendencia)
        {
            _contexto.Pendencia.Add(pendencia);
            _contexto.SaveChanges();
        }

        public void EditarPendencia(Pendencia pendencia)
        {
            _contexto.Pendencia.Update(pendencia);
            _contexto.SaveChanges();
        }

        public void RemoverPendencia(Pendencia pendencia)
        {
            _contexto.Pendencia.Remove(pendencia);
            _contexto.SaveChanges();
        }
        #endregion
    }
}
