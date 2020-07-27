using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventCalendar.Controllers
{
    public class HomeController : Controller
    {
        // assumed Get by default
        public ActionResult Index()
        {
            return View();
            // return View("another view name", EventCalendarModel);
        }

        // assumed Get by default
        public JsonResult GetEvents()
        { 
            using (var ecm = new EventCalendarModel())
            {
                //System.Diagnostics.Debug.WriteLine("get events");         
                var events = ecm.Tables.ToList();
                /*foreach(var singleEvent in events)
                {
                    System.Diagnostics.Debug.WriteLine(singleEvent.EventID);
                    System.Diagnostics.Debug.WriteLine(singleEvent.Subject);
                    System.Diagnostics.Debug.WriteLine(singleEvent.Description);
                    System.Diagnostics.Debug.WriteLine(singleEvent.Start);
                    System.Diagnostics.Debug.WriteLine(singleEvent.End);
                }*/
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }          
        }

        // create or update for CRUD
        [HttpPost]
        public JsonResult SaveEvent(Table t)
        {
            var status = false;
            try
            {     
                using (EventCalendarModel ecm = new EventCalendarModel())
                {
                    //System.Diagnostics.Debug.WriteLine("event id is: " + e.EventID);
                    // if >0, then this entry is already in our db
                    if (t.EventID > 0) // 0 is the default, which is not allowed in our db
                    {
                        //Update the event
                        var entry = ecm.Tables.Where(e => e.EventID == t.EventID).FirstOrDefault();
                        if (entry != null)
                        {
                            entry.Subject = t.Subject;
                            entry.Start = t.Start;
                            entry.End = t.End;
                            entry.Description = t.Description;
                            entry.EventColor = t.EventColor;
                        }
                        // else, something unexpected occured.
                    }
                    else
                    {
                        t.EventID = 1;
                         // if table is not empty
                         if (ecm.Tables.Any())
                         {
                             t.EventID = ecm.Tables.Max(e => e.EventID) + 1;
                         }

                        ecm.Tables.Add(t);

                       /* System.Diagnostics.Debug.WriteLine("start is: " + e.Start.ToString());
                        System.Diagnostics.Debug.WriteLine("end is: " + e.End.ToString());*/
                    }

                    ecm.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex) //Catch Other Exception
            {
                System.Diagnostics.Debug.WriteLine("Exception message is: " + ex.ToString());
            }

            return new JsonResult { Data = new { status = status } };
        }

        // delete for CRUD
        [HttpPost]
        public JsonResult DeleteEvent(Table t)
        {
            var status = false;
            //System.Diagnostics.Debug.WriteLine("event ID to delete is: "+ e.EventID);
            try
            {
                using (var ecm = new EventCalendarModel())
                {
                    var entry = ecm.Tables.Where(e => e.EventID == t.EventID).FirstOrDefault();
                    if (entry != null)
                    {
                        ecm.Tables.Remove(entry);
                        ecm.SaveChanges();
                        status = true;
                    }
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Delete exception message is: " + ex.ToString());
            }
            
            return new JsonResult { Data = new { status = status } };
        }


    }
}