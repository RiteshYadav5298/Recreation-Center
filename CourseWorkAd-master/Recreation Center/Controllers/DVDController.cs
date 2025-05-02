using CourseWorkAd.DBContext;
using CourseWorkAd.Models;

using Microsoft.AspNetCore.Mvc;

namespace CourseWorkAd.Controllers
{
    public class DVDController : Controller
    {
        private readonly ApplicationDBContext dbContext;

        public DVDController(ApplicationDBContext db)
        {
            dbContext = db;
        }
       

        public IActionResult DVD(bool Issuccess = false, bool Isdelete = false)
        {
            ViewBag.issuccess = Issuccess;
            ViewBag.isdelete = Isdelete;
            /*BookInventory bki */
            ViewBag.DVDList = dbContext.DVDCopy.ToArray();

            var DVDList = dbContext.DVDCopy.ToArray();

            return View(DVDList);
        }

        //search dvd
        [HttpGet]
        public IActionResult DVDSearch(int ActorNumber)
        {
            ViewBag.actorlist = dbContext.Actors.ToArray();
            var Listdata = from x in dbContext.Actors
                       from y in dbContext.CastMember
                       from z in dbContext.DVDTitles
                       where x.ActorNumber == ActorNumber && x.ActorNumber == y.ActorNumber && z.DVDNumber == y.DVDNumber
                       select z;


            return View(Listdata);
        }

     
        [HttpGet]
        public async Task<IActionResult> DVDListData(int ActorNumber)
        {

            ViewBag.actorlist = dbContext.Actors.ToArray();
            var ListData = from x in dbContext.Actors
                       from y in dbContext.CastMember
                       from z in dbContext.DVDTitles
                       from a in dbContext.Loans
                       from b in dbContext.DVDCopy
                       where x.ActorNumber == ActorNumber && x.ActorNumber == y.ActorNumber && z.DVDNumber == y.DVDNumber && y.DVDNumber == b.DVDNumber && b.CopyNumber == a.CopyNumber && a.DateReturned != null
                       select z;

            return View(ListData);


        }


        //this section for adding category

        public IActionResult Category(bool Issuccess = false, bool Isdeletecat = false)
        {
            ViewBag.issuccess = Issuccess;
            ViewBag.isdeletecat = Isdeletecat;
            var Listcategory = dbContext.DVDCategories.ToArray();
            return View(Listcategory);
        }


        [HttpPost]
        public async Task<IActionResult> AddCategory(DVDCategory catego, string catName, int age)
        {
            //categoriesIs.Id = id;
            catego.CategoryDescription = catName;
            catego.AgeRestricted = age;
            try
            {
                dbContext.DVDCategories.Add(catego);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Category", new { Issuccess = true });
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IActionResult DeleteCategory(int CategoryNumber)
        {
            var category = dbContext.DVDCategories.Where(a => a.CategoryNumber == CategoryNumber).First();
            dbContext.DVDCategories.Remove(category);
            dbContext.SaveChanges();
            return RedirectToAction("Category", new { Isdeletecat = true });
        }

        public async Task<IActionResult> EditCategory(DVDCategory categorydata, string catName, int age)
        {
            categorydata.CategoryDescription = catName;
            categorydata.AgeRestricted = age;
            try
            {
                dbContext.DVDCategories.Update(categorydata);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("~/DVD/Category");
            }
            catch (Exception)
            {
                return null;
            }
        }

        //this section for adding DVD Titles
        [HttpGet]
        public IActionResult AddDVDTitles(bool Issuccess = false)
        {
            ViewBag.issuccess = Issuccess;
            /*BookInventory bki */
            ViewBag.categoryList = dbContext.DVDCategories.ToArray();
            ViewBag.studioList = dbContext.Studio.ToArray();
            ViewBag.producerList = dbContext.Producers.ToArray();
            var data = dbContext.DVDTitles.ToArray();
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddDVDTitles(DVDTitle title, string dvdtitle, DateTime release, int stcharge, int pencharge, int category, int studio, int producer)
        {
            title.DVDtitle = dvdtitle;
            title.DateReleased = DateOnly.FromDateTime(release);
            title.StudioNumber = studio;
            title.ProducerNumber = producer;
            title.CategoryNumber = category;
            title.StandardCharge = stcharge;
            title.PenaltyCharge = pencharge;
            try
            {
                dbContext.DVDTitles.Add(title);
                await dbContext.SaveChangesAsync();
                var num = dbContext.DVDTitles.Where(x => x.DVDtitle == dvdtitle).FirstOrDefault();
                var casts = dbContext.CastMember.Where(x => x.DVDNumber == num.DVDNumber).FirstOrDefault();
                if (casts == null)
                {
                    return RedirectToAction("Cast");

                }
                else
                {
                    return RedirectToAction("AddDVDTitles", new { Issuccess = true });
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IActionResult DeleteDVDTitle(int DVDNumber)
        {
            var dvddata = dbContext.DVDTitles.Where(a => a.DVDNumber == DVDNumber).First();
            dbContext.DVDTitles.Remove(dvddata);
            dbContext.SaveChanges();
            return RedirectToAction("AddDVDTitles", new { Isdelete = true });
        }

        public async Task<IActionResult> EditDVDTitles(DVDTitle title, string dvdtitle, DateTime release, int stcharge, int pencharge, int category, int studio, int producer)
        {
            title.DVDtitle = dvdtitle;
            title.DateReleased = DateOnly.FromDateTime(release);
            title.StudioNumber = studio;
            title.ProducerNumber = producer;
            title.CategoryNumber = category;
            title.StandardCharge = stcharge;
            title.PenaltyCharge = pencharge;
            try
            {
                dbContext.DVDTitles.Update(title);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("AddDVDTitles");
            }
            catch (Exception)
            {
                return null;
            }
        }

        

        

        public IActionResult AddActor(Actor actors)
        {

            return View();
        }

        public IActionResult Actor(bool Issuccess = false, bool Isdeleteaut = false)
        {
            ViewBag.issuccess = Issuccess;
            ViewBag.isdeleteaut = Isdeleteaut;
            var actorList = dbContext.Actors.ToArray();
            return View(actorList);
        }


        [HttpPost]
        public async Task<IActionResult> AddActor(Actor actors, string actorfirstName, string actorsurname)
        {
            //authorIs.Id = id;
            actors.ActorFirstName = actorfirstName;
            actors.ActorSurname = actorsurname;

            try
            {
                dbContext.Actors.Add(actors);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Actor", new { Issuccess = true });
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IActionResult DeleteActors(int ActorNumber)
        {
            var actorsdata = dbContext.Actors.Where(x => x.ActorNumber == ActorNumber).First();
            dbContext.Actors.Remove(actorsdata);
            dbContext.SaveChanges();
            return RedirectToAction("Actor", new { Isdeleteaut = true });
        }
        

        public IActionResult AddProducers(Producer producer)
        {

            return View();
        }

        public IActionResult Producer(bool Issuccess = false, bool Isdeletepro = false)
        {
            ViewBag.issuccess = Issuccess;
            ViewBag.isdeletepro = Isdeletepro;
            var producerList = dbContext.Producers.ToArray();
            return View(producerList);
        }


        [HttpPost]
        public async Task<IActionResult> AddProducer(Producer producer, string prodName)
        {

            producer.ProducerName = prodName;

            try
            {
                dbContext.Producers.Add(producer);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Producer", new { Issuccess = true });
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IActionResult DeleteProducer(int ProducerNumber)
        {
            var producer_data = dbContext.Producers.Where(x => x.ProducerNumber == ProducerNumber).First();
            dbContext.Producers.Remove(producer_data);
            dbContext.SaveChanges();
            return RedirectToAction("Producer", new { Isdeletepro = true });
        }
       

        public IActionResult AddStudio(Studio studio)
        {

            return View();
        }

        public IActionResult Studio(bool Issuccess = false, bool Isdeletestu = false)
        {
            ViewBag.issuccess = Issuccess;
            ViewBag.isdeletestu = Isdeletestu;
            var studioList = dbContext.Studio.ToArray();
            return View(studioList);
        }


        [HttpPost]
        public async Task<IActionResult> AddStudio(Studio studio, string studioName)
        {
            //authorIs.Id = id;
            studio.StudioName = studioName;

            try
            {
                dbContext.Studio.Add(studio);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Studio", new { Issuccess = true });
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IActionResult DeleteStudio(int StudioNumber)
        {
            var studio_data = dbContext.Studio.Where(x => x.StudioNumber == StudioNumber).First();
            dbContext.Studio.Remove(studio_data);
            dbContext.SaveChanges();
            return RedirectToAction("Actors", new { Isdeletestu = true });
        }

        //this section for add author

        public IActionResult AddCopy(DVDCopy copy)
        {
           
            return View();
        }

        public IActionResult Copy(bool Issuccess = false, bool Isdeletecop = false)
        {
            ViewBag.issuccess = Issuccess;
            ViewBag.isdeletecop = Isdeletecop;
            var copylist = dbContext.DVDCopy.ToArray();
            ViewBag.DVDnumber = dbContext.DVDTitles.ToArray();
            return View(copylist);
        }


        [HttpPost]
        public async Task<IActionResult> AddCopy(DVDCopy copy, int num)
        {
            
            copy.DVDNumber = num;
            copy.DatePurchase = DateOnly.FromDateTime(DateTime.Now);

            try
            {
                dbContext.DVDCopy.Add(copy);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Copy", new { Issuccess = true });
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IActionResult DeleteCopy(int CopyNumber)
        {
            var copydata = dbContext.DVDCopy.Where(x => x.CopyNumber == CopyNumber).First();
            dbContext.DVDCopy.Remove(copydata);
            dbContext.SaveChanges();
            return RedirectToAction("Copy", new { Isdeletecop = true });
        }
        //this section for add author

        public IActionResult AddCast(CastMember cast)
        {
           
            return View();
        }

        public IActionResult Cast(bool Issuccess = false, bool Isdeletecas = false)
        {
            ViewBag.issuccess = Issuccess;
            ViewBag.isdeletecas = Isdeletecas;
            var castList = dbContext.CastMember.ToArray();
            ViewBag.actorList = dbContext.Actors.ToArray();
            ViewBag.titleList = dbContext.DVDTitles.ToArray();
            return View(castList);
        }


        [HttpPost]
        public async Task<IActionResult> AddCast(CastMember casts, int numb, int title)
        {
          
            casts.ActorNumber = numb;
            casts.DVDNumber = title;

            try
            {
                dbContext.CastMember.Add(casts);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Cast", new { Issuccess = true });
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IActionResult DeleteCast(int DVDNumber)
        {
            var casts_data = dbContext.CastMember.Where(x => x.DVDNumber == DVDNumber).First();
            dbContext.CastMember.Remove(casts_data);
            dbContext.SaveChanges();
            return RedirectToAction("Cast", new { Isdeletecas = true });
        }
    }
}
