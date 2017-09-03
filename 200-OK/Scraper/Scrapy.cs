using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using _200_OK.Data.Entity;
using Facebook;

namespace _200_OK.Scraper
{
    public delegate void ScrapeHelper();
    public interface IScrapyBuilder
    {
        void RunGUI();
        void OpenScraper();
        void CloseScraper();
        bool Statues();
        void Intaite(ScrapeHelper helper);
    }
    public interface IScrapy
    {
        void Scrape();
    }
    public abstract class Scraper : IScrapyBuilder
    {
        public static Operation FBAction = null; // This is Operation Class for Getting Data from Facebook.
        protected Thread operation = null;
        protected ContextModel modelDB = null;
        public  void Intaite(ScrapeHelper helper)
        {
            operation = new Thread(helper.Invoke);
            modelDB = new ContextModel();
            FBAction = Program._operation;
        }
        public void RunGUI()
        {
            Thread th = new Thread(() =>
            {
                new ScrapyUI().ShowDialog();
            });
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }
        public void CloseScraper()
        {
            if (Statues())
            {
                operation.Abort();
            }
            else
            {
                Console.WriteLine("Operation not started yet..");
            }
        }

        public bool Statues()
        {
            return operation.IsAlive;
        }
        public void OpenScraper()
        {
            if (!Statues())
            {
                operation.Start();
            }
            else
            {
                Console.WriteLine("Operation Started Succsessfully..");
            }
        }
    }
    public class FullDataScrap : Scraper, IScrapy
    {
        public FullDataScrap()
        {
            base.Intaite(new ScrapeHelper(Scrape));
        }
        public void Scrape()
        {
            throw new NotImplementedException();
        }
    }
    public class EmailPhoneScrape : Scraper, IScrapy
    {
        public EmailPhoneScrape()
        {
            base.Intaite(new ScrapeHelper(Scrape));
        }
        public void Scrape()
        {
            Parallel.ForEach(modelDB.Users, user =>
            {
                    try
                    {
                        dynamic data = FBAction.Get(user.UserID + "?fields=email,mobile_phone");

                        if (!string.IsNullOrEmpty(data.email))
                        {
                            try
                            {
                                user.Email = data.email;
                            }
                            catch { }
                        }

                        if (!string.IsNullOrEmpty(data.mobile_phone))
                        {
                            try
                            {
                                user.Phone = data.mobile_phone;
                            }
                            catch { }
                        }

                        if (!string.IsNullOrEmpty(data.mobile_phone) || !string.IsNullOrEmpty(data.email))
                        modelDB.SaveChanges();
                    }
                    catch { }
            });
        }
    }
 
}
