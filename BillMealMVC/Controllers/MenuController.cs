using BillMealMVC.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BillMealMVC.Controllers
{
    public class MenuController: BaseController
    {
        public MenuController() : base()
        {

        }
        public ActionResult Index()
        {
            return BuildMenu();
        }
        public ActionResult BuildMenu()
        {

            var MenuHeads = _context.MenuHeads
                                    .Include("MenuRows")
                                    .ToList();
            return View("Index",MenuHeads);
        }
        public ActionResult SaveData(string type)
        {
            ViewBag.ErrorMessage = null;
            var Description = HttpContext.Request.Form["Description"];
            var Name = HttpContext.Request.Form["Name"];
            var id = HttpContext.Request.Form["MenuHeadId"];
            if (type.Equals("head"))
            {
                //aggiungi nuovo menuhead
                var MenuHead = new MenuHead();
                if (string.IsNullOrEmpty(Name))
                {
                    ViewBag.ErrorMessage = "Inserire un nome della sezione";
                }
                MenuHead.Name = Name;
                _context.MenuHeads.Add(MenuHead);
            }
            else
            {
                if (string.IsNullOrEmpty(Name))
                {
                    ViewBag.ErrorMessage = "Inserire almeno il nome per la preparazione";
                }
                if (string.IsNullOrEmpty(id))
                    ViewBag.ErrorMessage = "Si è verificato un errore interno";
                var idnu = int.Parse(id);
                var row = new MenuRow
                {
                     Description = Description,
                      Name = Name
                };
                var menu = _context.MenuHeads
                                    .Include("MenuRows").Where(c => c.MenuHeadId == idnu)
                                    .FirstOrDefault();
                menu.MenuRows.Add(row);
            }
            _context.SaveChanges();
            return BuildMenu();
        }
        public ActionResult Edit()
        {
            var editype = HttpContext.Request.Form["edittype"];
            var editid = int.Parse(HttpContext.Request.Form["editid"]);
            var Name = HttpContext.Request.Form["Name"];
            var Description = HttpContext.Request.Form["Description"];
            if (editype == "head")
            {
                var head = _context.MenuHeads.Where(c => c.MenuHeadId == editid).FirstOrDefault();
                if (head != null)
                {
                    head.Name = Name;
                    _context.Entry(head).State = EntityState.Modified;
                }
            }
            if (editype == "row")
            {
                var row = _context.MenuRows.Where(c => c.MenuRowId == editid).FirstOrDefault();
                if (row != null)
                {
                    row.Name = Name;
                    row.Description = Description;
                    _context.Entry(row).State = EntityState.Modified;
                }
            }
            _context.SaveChanges();
            return BuildMenu();
        }
        public ActionResult Delete()
        {
            var deleteype = HttpContext.Request.Form["deletetype"];
            var deleteid = int.Parse(HttpContext.Request.Form["deleteid"]);
            if (deleteype == "head") 
            {
                var head = _context.MenuHeads.Where(c => c.MenuHeadId == deleteid).FirstOrDefault();
                if(head!=null)
                {
                    _context.MenuHeads.Remove(head);
                    _context.Entry(head).State = EntityState.Deleted;
                }
            }
            if (deleteype == "row") 
            {
                var row = _context.MenuRows.Where(c => c.MenuRowId == deleteid).FirstOrDefault();
                if (row != null)
                {
                    _context.MenuRows.Remove(row);
                    _context.Entry(row).State = EntityState.Deleted;
                }
            }
            _context.SaveChanges();
            return BuildMenu();
        }
    }
}