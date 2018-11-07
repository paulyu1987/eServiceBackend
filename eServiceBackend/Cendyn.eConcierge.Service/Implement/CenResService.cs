using System;
using Cendyn.eConcierge.Service.Interface;
using System.Configuration;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using Cendyn.eConcierge.Data.Repository.eConcierge.Interface;
using Cendyn.eConcierge.EntityModel;
using Cendyn.eConcierge.Service.CenResModel;

namespace Cendyn.eConcierge.Service.Implement
{
    public class CenResService : ServiceBase, ICenResService
    {
        static readonly string keyXml = ConfigurationManager.AppSettings["CenResKeyXml"].ToString();
        static readonly string keyContainerName = ConfigurationManager.AppSettings["CenResKeyContainerName"].ToString();
        static readonly string appId = ConfigurationManager.AppSettings["CenResAppId"].ToString();
        
        static readonly string availabilitySimple = "/availability/simple/{0}";
        static readonly string availablityVerbose = "/availability/verbose/{0}";
        public IEUpgradeRequestRepository eUpgradeRequestRepo { get; set; }

        public string[] GetAvailableRoomTypes(string[] roomTypes, DateTime startDate, DateTime endDate, CenResParamsDTO cenResParams)
        {
            string method = "POST";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("StartDate", startDate.ToShortDateString());

            // Guest depart at endDate so last day in hotel is endDate-1
            endDate = endDate.AddDays(-1);
            parameters.Add("EndDate", endDate.ToShortDateString());
            if (!string.IsNullOrEmpty(cenResParams.Interface))
            {   // Only SandyLane uses Inventory for now.
                parameters.Add("InterfaceType", cenResParams.Interface);
            }
            var body = new { RoomTypeFilter = roomTypes };
            return SendCenResRequest<string[]>(cenResParams.ServiceUrl, availabilitySimple, method, cenResParams.CendynPropertyId, parameters, body);
        }

        public List<RoomInfo> GetRoomInfos(string[] roomTypes, DateTime startDate, DateTime endDate, CenResParamsDTO cenResParams)
        {
            string method = "POST";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("StartDate", startDate.ToString("yyyy-MM-dd"));
            parameters.Add("EndDate", endDate.ToString("yyyy-MM-dd"));
            //parameters.Add("StartDate", startDate.ToShortDateString());
            //parameters.Add("EndDate", endDate.ToShortDateString());
            if (!string.IsNullOrEmpty(cenResParams.Interface))
            {   // Only SandyLane uses Inventory for now.
                parameters.Add("InterfaceType", cenResParams.Interface);
            }
            var body = new { RoomTypeFilter = roomTypes };
            return SendCenResRequest<List<RoomInfo>>(cenResParams.ServiceUrl, availablityVerbose, method, cenResParams.CendynPropertyId, parameters, body);
        }

        public List<RoomInfo> GetRoomInfosRevised(string[] roomTypes, DateTime startDate, DateTime endDate, CenResParamsDTO cenResParams)
        {
            List<RoomInfo> roomInfos = GetRoomInfos(roomTypes, startDate, endDate, cenResParams);
            IList<EUpgradeRequestDTO> eUpgradeRequests = getConfirmedRequests();
            foreach(EUpgradeRequestDTO e in eUpgradeRequests)
            {
                roomInfos.FindAll(ri => ri.RoomType == e.BookedRoomType && ri.Date.Date >= e.StartDate.Value.Date && ri.Date.Date < e.EndDate.Value.Date).ForEach(ri => ri.AvailableRooms -= 1);
                roomInfos.FindAll(ri => ri.RoomType == e.UpgradeRoomType && ri.Date.Date >= e.StartDate.Value.Date && ri.Date.Date < e.EndDate.Value.Date).ForEach(ri => ri.AvailableRooms += 1);
            }
            return roomInfos;
        }

        IList<EUpgradeRequestDTO> getConfirmedRequests()
        {
            return eUpgradeRequestRepo.GetEUpgradeRequestConfirmed();
        }

        returnType SendCenResRequest<returnType>(string serviceUrl, string endpoint, string method, string cendynPropertyId, Dictionary<string, string> parameters, object body)
        {
            string requestUrl = string.Format(serviceUrl + endpoint, cendynPropertyId);
            if (parameters != null)
                requestUrl = AppendParameters(requestUrl, parameters);
            string authHeader = createAuthHeader(appId);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(requestUrl);
            req.Method = method;
            req.ContentType = "application/json";
            req.Accept = "application/json";
            req.Headers.Add(HttpRequestHeader.Authorization, authHeader);
            if (body != null)
            {
                string b = JsonConvert.SerializeObject(body);
                byte[] postBytes = Encoding.UTF8.GetBytes(b);
                req.ContentLength = postBytes.Length;
                using (var reqStream = req.GetRequestStream())
                {
                    reqStream.Write(postBytes, 0, postBytes.Length);
                }
            }
            else
            {
                req.ContentLength = 0;
            }

            var response = (HttpWebResponse)req.GetResponse();
            var stream = response.GetResponseStream();
            var sr = new StreamReader(stream);
            string content = sr.ReadToEnd();
            stream.Close();
            returnType result = JsonConvert.DeserializeObject<returnType>(content);

            return result;
        }

        string AppendParameters(string requestUrl, Dictionary<string, string> parameters)
        {
            if (parameters == null || parameters.Count == 0)
                return requestUrl;

            StringBuilder sb = new StringBuilder(requestUrl);
            sb.Append("?");
            foreach (var keyvalue in parameters)
            {
                sb.Append(keyvalue.Key);
                sb.Append("=");
                if (!string.IsNullOrEmpty(keyvalue.Value))
                    sb.Append(keyvalue.Value);
                sb.Append("&");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }


        string createAuthHeader(string appId)
        {
            CspParameters cp = new CspParameters();  
            if(!string.IsNullOrEmpty(keyContainerName))
            {       
                cp.KeyContainerName = keyContainerName;
                cp.Flags = CspProviderFlags.UseMachineKeyStore;
            }
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cp))
            using (SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider())
            {
                // initialize the RSA provider – keyXml holds the application’s key-pair
                if(string.IsNullOrEmpty(keyContainerName))
                {
                    rsa.FromXmlString(keyXml);
                }
                // compute a random nonce
                Random r = new Random();
                byte[] nonceBuffer = new byte[16];
                r.NextBytes(nonceBuffer);
                string nonce = Convert.ToBase64String(nonceBuffer);
                // get the signed application token
                byte[] appIdBytes = Encoding.UTF8.GetBytes(appId), // must use UTF8
                signingBuffer = new byte[appIdBytes.Length + 16]; // must be exactly the space
                // for the AppId bytes + 16
                // or the hash won't match
                Array.Copy(nonceBuffer, signingBuffer, nonceBuffer.Length);
                Array.Copy(appIdBytes, 0, signingBuffer, nonceBuffer.Length, appIdBytes.Length);
                byte[] hashedApp = sha256.ComputeHash(signingBuffer), // must use SHA256
                signedHash = rsa.SignHash(hashedApp, CryptoConfig.MapNameToOID("SHA256"));
                string Base64EncodedSignature = Convert.ToBase64String(signedHash);
                // the variables, nonce and signature now hold the Base64 data used for authentication
                string Base64EncodedNonce = Convert.ToBase64String(nonceBuffer);
                string authHeader = string.Format("CSSO id={0},nonce={1},sig={2}", appId, Base64EncodedNonce, Base64EncodedSignature);
                return authHeader;
            }
        }

        //public class RoomInfo
        //{
        //    [JsonProperty("roomType")]
        //    public string RoomType { get; set; }
        //    [JsonProperty("date")]
        //    public DateTime Date { get; set; }
        //    [JsonProperty("totalRooms")]
        //    public int TotalRooms { get; set; }
        //    [JsonProperty("availableRooms")]
        //    public int AvailableRooms { get; set; }
        //}
    }
}
