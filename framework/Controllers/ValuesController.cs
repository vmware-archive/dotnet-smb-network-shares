using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Common.Net;
using System.IO;

namespace NET_framework4_6_1_win_network_smb_share.Controllers
{
    public class ValuesController : ApiController
    {
        private CloudFoundryApplicationOptions _appOptions { get; set; }
        private CloudFoundryServicesOptions _serviceOptions { get; set; }
        private NetworkCredential _shareCredential { get; set; }
        private string _shareFolderAddress { get; set; }

        public ValuesController()
        {
            _appOptions = ApplicationConfig.CloudFoundryApplication;
            _serviceOptions = ApplicationConfig.CloudFoundryServices;

            string userName = _serviceOptions.Services["credhub"]
                    .First(q => q.Name.Equals("test-network-share"))
                    .Credentials["share-username"].Value;
            string password = _serviceOptions.Services["credhub"]
                    .First(q => q.Name.Equals("test-network-share"))
                    .Credentials["share-password"].Value;

            _shareCredential = new NetworkCredential(userName, password);

            _shareFolderAddress = _serviceOptions.Services["user -provided"]
                    .First(q => q.Name.Equals("network-address"))
                    .Credentials["share-network-address"].Value;
        }

        // GET api/values/CopyFileToShareWithCreds
        [HttpGet]
        public IHttpActionResult CopyFileToShareWithCreds()
        {
            string destFilePath = _shareFolderAddress + @"\the-pivotal-story-copy.pdf";
            string sourceFilePath = @".\the-pivotal-story.pdf";

            //Using the Steeltoe.Common.Net package, which uses WNetUseConnection
            //     we authenticate ouselves and then copy the file
            IHttpActionResult badRequestObj;
            
            using (WindowsNetworkFileShare networkPath = new WindowsNetworkFileShare(_shareFolderAddress, _shareCredential)) {
                badRequestObj = copyFileToNetworkShare(sourceFilePath, destFilePath);
            }

            if (badRequestObj != null)
                return badRequestObj;

            return Json("Yeah! Your file got copied and validated.");
        }

        // GET api/values/ListFilesFromShareNoCreds
        [HttpGet]
        public IHttpActionResult ListFilesFromShareNoCreds()
        {
            string[] fileEntries = null;
            fileEntries = Directory.GetFiles(_shareFolderAddress);
            return Json(fileEntries);
        }

        private IHttpActionResult copyFileToNetworkShare(string sourceFilePath, string destFileAddress)
        {
            //copy the file to a shared SMB network drive
            try {
                System.IO.File.Copy(sourceFilePath, destFileAddress, true);
            } catch (UnauthorizedAccessException) {
                return BadRequest("You are not authorized to do this.");
            } catch (FileNotFoundException) {
                return BadRequest("The file was not found");
            } catch (IOException ioex) {
                return BadRequest("IO exception occurred - " + ioex.Message);
            } catch (Exception ex) {
                return BadRequest("General exception occurred - " + ex.Message);
            }

            //Validate the file exists
            if (!System.IO.File.Exists(destFileAddress))
                return BadRequest("Boo, your file could not be validated.");

            return null;
        }
        /*
        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
        */
    }
}
