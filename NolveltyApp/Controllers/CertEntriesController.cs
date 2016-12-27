using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using NolveltyApp.Models;
using System.Diagnostics;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;


namespace NolveltyApp.Controllers
{
    public class CertEntriesController : Controller
    {
        private ApplicationDbContext adb = new ApplicationDbContext();
        public UserManager<ApplicationUser> UserManager { get; private set; }

        public CertEntriesController()
        {
            adb = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(adb));

        }

        public CertEntriesController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            UserManager = userManager;

        }
        
        // GET: CertEntries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CertEntries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,awardDate,createDate,timePeriod,recipientEmail,recipient,giverEmail,giverID,sendToGiver")] CertEntry certEntry)
        {
            if (ModelState.IsValid)
            {
                certEntry.giver = (from g in adb.Users where g.Email == certEntry.giverEmail select g.Id).FirstOrDefault();
                adb.CertEntries.Add(certEntry);
                await adb.SaveChangesAsync();

                string recip = certEntry.recipientEmail;
                string period = "";

                if (certEntry.timePeriod == 1)
                {
                    period = "Month";
                }

                if (certEntry.timePeriod == 0)
                {
                    period = "Week";
                }

                string file = createCertPDF(certEntry, period);

                if (certEntry.sendToGiver == true)
                {
                    //add a sleep of 3 seconds to allow pdflatex enough time
                    System.Threading.Thread.Sleep(3000);
                    string give = certEntry.giverEmail;
                    send_CertEmail(give, period, file);
                }

                //add a sleep of 3 seconds to allow pdflatex enough time
                System.Threading.Thread.Sleep(3000);
                send_CertEmail(recip, period, file);
                return View("Confirm");
            }

            //error splash page in case the modelstate is corrupt
            return View("Error");
        }

        //heavily borrowed from SO http://stackoverflow.com/questions/32260/sending-email-in-net-through-gmail/10784857
        public void send_CertEmail(string recip, string period, string file)
        {
            MailMessage newMail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            newMail.From = new MailAddress("noveltyweb3@gmail.com");
            newMail.To.Add(recip);
            newMail.Subject = "Employee Recognition";
            newMail.Body = "Congratulations on your Employee of the " + period + " award! Please find your certificate attached!";

            //string filepath = Server.MapPath(file);
            Attachment attachment = new Attachment(file);
            newMail.Attachments.Add(attachment);

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new NetworkCredential("noveltyweb3@gmail.com", "Thisispassword");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(newMail);
        }

        private string createCertPDF(CertEntry certEntry, string timeperiod)
        {
            string cwdPath = HttpContext.Server.MapPath("~/Content");

            //strings for PDF generation
            string recipient = certEntry.recipient;
            string giverFname = (from g in adb.Users where g.Id == certEntry.giver select g.FName).FirstOrDefault();
            string giverLname = (from g in adb.Users where g.Id == certEntry.giver select g.LName).FirstOrDefault();
            string date = certEntry.awardDate.Date.ToString("d");

            //getting signature file for cert generation
            byte[] imgArray = (from g in adb.Users where g.Id == certEntry.giver select g.Signature).FirstOrDefault();
            string sigFile;

            if (imgArray.Length > 0)
            {

                string imgFilePath = cwdPath + "\\" + "sigFile" + certEntry.giver + ".png";
                using (Image image = Image.FromStream(new MemoryStream(imgArray)))
                {
                    image.Save(imgFilePath, ImageFormat.Png);
                }

                sigFile = "sigFile" + certEntry.giver;
            }

            else
            {
                sigFile = "signature";
            }

            //prepping for kicking off a child process
            Process serverSideProcess = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = @"c:\Windows\System32\cmd.exe";
            startInfo.WorkingDirectory = cwdPath;

            //actual pdflatex call (REAALLLY LONG)
            startInfo.Arguments = "/c pdflatex \"\\newcommand{\\timeperiod}{" + timeperiod + "} \\newcommand{\\recipient}{" + recipient + "} \\newcommand{\\dmy}{" + date + "} \\newcommand{\\giver}{" + giverFname + " " + giverLname + "} \\newcommand{\\mgrsig}{" + sigFile + "} \\input{certificate.tex}\"";

            //starting process
            serverSideProcess.StartInfo = startInfo;
            serverSideProcess.Start();
            serverSideProcess.Close();

            //returns the absolute path of the certificate.pdf file
            return cwdPath + "\\certificate.pdf";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                adb.Dispose();
            }
            base.Dispose(disposing);
        }


        //Displays the certificates the logged in user has created.
        public async Task<ActionResult> Manage(string timeString, string searchString)
        {
            //Get the curent user ID
            ApplicationUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var Certs = adb.CertEntries.ToList();

            var userCerts = (from u in adb.CertEntries where user.Email == u.giverEmail select u);

            var timeList = new List<String>();
            timeList.Add("Month");
            timeList.Add("Week");
            ViewBag.timeString = new SelectList(timeList);

            if (!String.IsNullOrEmpty(searchString))
            {
                userCerts = userCerts.Where(s => s.recipientEmail.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(timeString))
            {
                int timeInt;
                if (timeString == "Month")
                {
                    timeInt = 1;
                }
                else
                {
                    timeInt = 0;
                }
                userCerts = userCerts.Where(s => s.timePeriod == timeInt);
            }

            return View(userCerts);

        }

        //Check if user wants to delete a certificate     
        public ActionResult Delete(Guid id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Find the certificate.
            CertEntry Cert = adb.CertEntries.Find(id);

            if (Cert == null)
            {
                return HttpNotFound();
            }
            return View(Cert);
        }

        // Delete Certificates from Cert Index.
        // POST: /Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                CertEntry Cert = adb.CertEntries.Find(id);

                if (Cert == null)
                {
                    return HttpNotFound();
                }

                adb.CertEntries.Remove(Cert);
                await adb.SaveChangesAsync();

                return RedirectToAction("Manage");
            }
            else
            {
                return View();
            }
        }
    }
}