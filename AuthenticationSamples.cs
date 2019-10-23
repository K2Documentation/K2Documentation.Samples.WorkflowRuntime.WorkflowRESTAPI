using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Web;

namespace WorkflowRestAPISamples
{
    public static class AuthenticationSamples
    {
        //use Basic Authentication with the Workflow REST API. 
        //Note that basic authentication is not as secure as OAuth, and the connection will always be authenticated by K2 using the context of the username and password you pass in
        public static System.Net.Http.HttpClient BasicAuthHttpClient(string username, string password)
        {
            System.Net.Http.HttpClient k2WebClient;

            //Passing static credentials for authentication, using a client handler to store credentials.
            System.Net.NetworkCredential k2credentials = new NetworkCredential(username, password);
            System.Net.Http.HttpClientHandler loginHandler = new HttpClientHandler
            {
                Credentials = k2credentials
            };
            // Open the HTTPClient connection one time so that we only open one port/socket on the server.
            k2WebClient = new System.Net.Http.HttpClient(loginHandler, true);
            return k2WebClient;
        }

        public static async Task<System.Net.Http.HttpClient> OAuthSampleNoPrompt(string username, string password, string resource, string clientId, string clientSecret, string oAuthTokenUrl)
        {
            System.Net.Http.HttpClient k2WebClient = new HttpClient();

            //set up the keys-values for the OAuth token request
            var vals = new List<KeyValuePair<string, string>> {
                new KeyValuePair < string, string > ("grant_type", "password"),
                new KeyValuePair < string, string > ("scope", "openid"),
                new KeyValuePair < string, string > ("resource", resource), //Identifier of the target resource that is the recipient of the requested token. This value will most likely be https://api.k2.com/ in your environment.
                new KeyValuePair < string, string > ("client_id", clientId), //Client ID of the custom client app requesting the token (the app should have the K2 API permission scope). If using AAD for OAuth, provide the Application ID of your custom app registration; you can find this value in the Application ID field in the App Registration page, in Settings > Properties
                new KeyValuePair < string, string > ("client_secret", clientSecret), //TODO: the secret key for the custom client app requesting the token. If using AAD for OAuth, provide the Client Secret (also known as the Key value) from your AAD App Registration page, in the Settings > Keys page
                new KeyValuePair < string, string > ("username", username),
                new KeyValuePair < string, string > ("password", password)
            };

            //the outh2token endpoint URL from your AAD config
            var url = oAuthTokenUrl;

            //retrieve the OAuth token
            var hc = new HttpClient();
            HttpContent hcContent = new FormUrlEncodedContent(vals);
            HttpResponseMessage hcResponse = hc.PostAsync(url, hcContent).Result;
            if (!hcResponse.IsSuccessStatusCode)
            {
                throw new Exception("Error in OAuthSampleNoPrompt. Error:" + hcResponse.StatusCode);
            }
            //read in the token that was returned
            System.IO.Stream data = await hcResponse.Content.ReadAsStreamAsync();
            string responseData;
            using (var reader = new System.IO.StreamReader(data, Encoding.UTF8))
            {
                responseData = reader.ReadToEnd();
            }

            //construct an access token object using the NewtonSoft Json helper
            AccessToken authToken = Newtonsoft.Json.JsonConvert.DeserializeObject<AccessToken>(responseData);

            //set up the authentication headers for the K2 webclient
            k2WebClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken.Access_Token);
            
            return k2WebClient;
        }


        public static void OAuthSamplePromptForCredentials()
        {

        }

        class AccessToken
        {
            //represents a public string token_type;
            public string Scope { get; set; }
            public string ExpiresIn { get; set; }
            public string ExpiresOn { get; set; }
            public string NotBefore { get; set; }
            public string Resource { get; set; }
            public string Access_Token { get; set; }
            public string RefreshToken { get; set; }
            public string IdToken { get; set; }

        }

    }
}
