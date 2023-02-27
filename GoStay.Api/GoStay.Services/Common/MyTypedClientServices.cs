using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;


namespace GoStay.Services
{
    public class UploadImagesResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public string data { get; set; }
        public string size { get; set; }
    }

    public class MyTypedClientServices : IMyTypedClientServices
    {
        public HttpClient Client { get; set; }
        private IHttpClientFactory _client { get; }
        public MyTypedClientServices(HttpClient client, IHttpClientFactory _client)
        {
            this._client = _client;
            client.BaseAddress = new Uri("https://localhost:7247");
            this.Client = client;
        }

        public UploadImagesResponse PostImgAndGetData(List<IFormFile> files, int width, int Obj_Id, int userId, int type)
        {
            var httpClient = _client.CreateClient();
            var content = new MultipartFormDataContent();

            foreach (var file in files)
            {
                if (file.Length <= 0)
                    return null;

                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                content.Add(new StreamContent(file.OpenReadStream())
                {
                    Headers =
                    {

                        ContentLength = file.Length,
                        ContentType = new MediaTypeHeaderValue(file.ContentType)
                    }
                }, "File", fileName);
            }
            HttpResponseMessage response = new HttpResponseMessage();

            //response = httpClient.PostAsync("https://localhost:7247/api/UploadFile/UploadFile" + $"?width={width}"
            //                                            + $"&Obj_Id={Obj_Id}" + $"&type={type}", content).Result;

            response = httpClient.PostAsync("https://cdn.realtech.com.vn/api/UploadFile/UploadFilePartner" + $"?width={width}"
                                                        + $"&Obj_Id={Obj_Id}" + $"&userId={userId}" + $"&type={type}", content).Result;

            var json = response.Content.ReadAsStringAsync().Result;
            var obj = JsonConvert.DeserializeObject<UploadImagesResponse>(json);

            return obj;
        }

    }
}
