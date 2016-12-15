using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DemoCRUD.AcessoDados;
using DemoCRUD.Models;
using System.Collections;

namespace DemoCRUD.Controllers
{
    public class LivrosController : Controller
    {
        private LivroContexto db = new LivroContexto();

        // GET: Livros
        public ActionResult Index()
        {
            var livros = db.Livros.Include(l => l.Genero);
            return View(livros.ToList());
        }

        //Modifica a maneira de listar os livros.
        public JsonResult Listar(String searchPhrase, int current = 1, int rowCount = 5)
        {

            // sort[Titulo] || sort[Autor] || sort[AnoEdicao] || sort[Valor]
            //Expressão Lambda para o request não ficar HardCoded.
            String chave = Request.Form.AllKeys.Where(k => k.StartsWith("sort")).First();

            String ordenacao = Request[chave]; //Request["sort[Titulo]"];
            String campo = chave.Replace("sort[", String.Empty).Replace("]",  String.Empty);

            var livros = db.Livros.Include(l => l.Genero);

            var total = livros.Count();
            // Filtro multiampo da página.
            if (!String.IsNullOrWhiteSpace(searchPhrase))
            {
                int ano = 0;
                int.TryParse(searchPhrase, out ano);

                decimal valor = 0.0m;
                decimal.TryParse(searchPhrase, out valor);


                livros = livros.Where("Titulo.Contains(@0) OR Autor.Contains(@0) OR AnoEdicao == @1 OR Valor == @2", searchPhrase, ano, valor);

            }

            String campoOrdenacao = String.Format("{0} {1}", campo, ordenacao);

            //Ordenar os livros na página por título - livros.OrderBy(l => l.Titulo) e recuperar somente 5 Take(5) ou
            //pular os 5 primeiros (skip(5)) .
            var livrosPaginados = livros.OrderBy(campoOrdenacao).Skip((current - 1) * rowCount).Take(rowCount);

            //Iforma qual view irá listar os livros.
            return Json(new {
                        rows = livrosPaginados.ToList(),
                        current = current,
                        rowCount = rowCount,
                        total = total
                 }
                    , JsonRequestBehavior.AllowGet);
        }

        // GET: Livros/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Livro livro = db.Livros.Find(id);
            if (livro == null)
            {
                return HttpNotFound();
            }
            return PartialView(livro);
        }

        // GET: Livros/Create
        public ActionResult Create()
        {
            ViewBag.GeneroId = new SelectList(db.Generos, "Id", "Nome");
            return PartialView();
        }

        // POST: Livros/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Modificação do Create para utilizar a PartialView
        public JsonResult Create([Bind(Include = "Id,Titulo,Autor,AnoEdicao,Valor,GeneroId")] Livro livro)
        {
            if (ModelState.IsValid)
            {
                db.Livros.Add(livro);
                db.SaveChanges();

                //Retorno do Json Para quem chamar o Create.
                return Json(new { resultado = true, mensagem = "Livro cadastrado com sucesso!"});

            }
            else
            {
                IEnumerable<ModelError> erros = ModelState.Values.SelectMany(item => item.Errors);

                //Retorno do Json Para quem chamar o Create.
                return Json(new { resultado = false, mensagem = erros });
            }
          
        }

        // GET: Livros/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Livro livro = db.Livros.Find(id);
            if (livro == null)
            {
                return HttpNotFound();
            }

            ViewBag.GeneroId = new SelectList(db.Generos, "Id", "Nome", livro.GeneroId);

            return PartialView(livro);
        }

        // POST: Livros/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Titulo,Autor,AnoEdicao,Valor,GeneroId")] Livro livro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(livro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GeneroId = new SelectList(db.Generos, "Id", "Nome", livro.GeneroId);
            return View(livro);
        }

        // GET: Livros/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Livro livro = db.Livros.Find(id);
            if (livro == null)
            {
                return HttpNotFound();
            }
            return PartialView(livro);
        }

        // POST: Livros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Livro livro = db.Livros.Find(id);
            db.Livros.Remove(livro);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
