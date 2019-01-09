/*
Copyright (C) 2019-Present Pivotal Software, Inc. All rights reserved. 

This program and the accompanying materials are made available under the terms of the 
under the Apache License, Version 2.0 (the "License”); you may not use this file except 
in compliance with the License. You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under the 
License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
either express or implied. See the License for the specific language governing permissions 
and limitations under the License.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Common.Net;
using System.IO;

namespace NET_framework4_6_1_network_smb_share.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private CloudFoundryApplicationOptions _appOptions { get; set; }
        private CloudFoundryServicesOptions _serviceOptions { get; set; }
        private NetworkCredential _shareCredential { get; set; }
        private string _shareFolderAddress { get; set; }

        public ValuesController(IOptions<CloudFoundryApplicationOptions> appOptions,
                            IOptions<CloudFoundryServicesOptions> serviceOptions)
        {
            _appOptions = appOptions.Value;
            _serviceOptions = serviceOptions.Value;

            string userName = _serviceOptions.Services["credhub"]
                    .First(q => q.Name.Equals("test-network-share"))
                    .Credentials["share-username"].Value;
            string password = _serviceOptions.Services["credhub"]
                    .First(q => q.Name.Equals("test-network-share"))
                    .Credentials["share-password"].Value;

            _shareCredential = new NetworkCredential(userName, password);

            _shareFolderAddress = _serviceOptions.Services["user-provided"]
                    .First(q => q.Name.Equals("network-address"))
                    .Credentials["share-network-address"].Value;
        }

        // GET api/values/copy-file-to-share-creds
        [HttpGet("copy-file-to-share-with-creds")]
        public ActionResult<string> CopyFileToShareWithCreds()
        {
            string destFilePath = _shareFolderAddress + @"\the-pivotal-story-copy.pdf";
            string sourceFilePath = @".\the-pivotal-story.pdf";
            
            //Using the Steeltoe.Common.Net package, which uses WNetUseConnection
            //     we authenticate ouselves and then copy the file
            BadRequestObjectResult badRequestObj;

            using (WindowsNetworkFileShare networkPath = new WindowsNetworkFileShare(_shareFolderAddress, _shareCredential)) {
                badRequestObj = copyFileToNetworkShare(sourceFilePath, destFilePath);
            }

            if (badRequestObj != null)
                return badRequestObj;
            
            return "Yeah! Your file got copied and validated.";
        }

        // GET api/values/list-files-from-share-no-cred
        [HttpGet("list-files-from-share-no-cred")]
        public ActionResult<IEnumerable<string>> ListFilesFromShareNoCreds()
        {
            string[] fileEntries = null;
            fileEntries = Directory.GetFiles(_shareFolderAddress);
            return fileEntries;
        }

        private BadRequestObjectResult copyFileToNetworkShare(string sourceFilePath, string destFileAddress)
        {
            //copy the file to a shared SMB network drive
            try {
                System.IO.File.Copy(sourceFilePath, destFileAddress,true);
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
        // GET api/values/5
        /* [HttpGet("{id}")]
         public ActionResult<string> Get(int id)
         {
             return "value";
         }

         // POST api/values
         [HttpPost]
         public void Post([FromBody] string value)
         {
         }

         // PUT api/values/5
         [HttpPut("{id}")]
         public void Put(int id, [FromBody] string value)
         {
         }

         // DELETE api/values/5
         [HttpDelete("{id}")]
         public void Delete(int id)
         {
         }*/
    }
}
