using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SoundCloud.API.Client.Internal.Client.Helpers;
using SoundCloud.API.Client.Internal.Infrastructure.Objects;
using SoundCloud.API.Client.Internal.Infrastructure.Objects.Uploading;
using File = SoundCloud.API.Client.Internal.Infrastructure.Objects.Uploading.File;
using HttpMethod = SoundCloud.API.Client.Internal.Infrastructure.Objects.HttpMethod;

namespace SoundCloud.API.Client.Internal.Infrastructure.Network
{
    internal class WebGateway : IWebGateway
    {
        public async Task<string> Request(IUriBuilder uriBuilder, HttpMethod method, Dictionary<string, object> parameters, byte[] body, string accessToken)
        {
            var request = await BuildRequest(uriBuilder, method, parameters, body, accessToken);

            return await GetContent(request);
        }

        public async Task<Stream> RequestStream(IUriBuilder uriBuilder, HttpMethod method, Dictionary<string, object> parameters, byte[] body, string accessToken)
        {
            var request = await BuildRequest(uriBuilder, method, parameters, body, accessToken);

            var response = await GetResponse(request);
            return response.GetResponseStream();
        }

        public async Task<string> Upload(IUriBuilder uriBuilder, Dictionary<string, object> parameters, params File[] files)
        {
            var uri = uriBuilder.Build();
            var mimeParts = new List<MimePart>();

            try
            {
                var request = WebRequest.Create(uri);

                foreach (var key in parameters.Keys)
                {
                    var part = new StringMimePart();

                    part.Headers["Content-Disposition"] = "form-data; name=\"" + key + "\"";
                    part.StringData = parameters[key].ToString();

                    mimeParts.Add(part);
                }

                var nameIndex = 0;

                foreach (var file in files)
                {
                    var part = new StreamMimePart();

                    if (string.IsNullOrEmpty(file.FieldName))
                        file.FieldName = "file" + nameIndex++;

                    part.Headers["Content-Disposition"] = "form-data; name=\"" + file.FieldName + "\"; filename=\"" + file.Path + "\"";
                    part.Headers["Content-Type"] = File.ContentType;

                    part.SetStream(file.Data);

                    mimeParts.Add(part);
                }

                var boundary = "----------" + DateTime.Now.Ticks.ToString("x");

                request.ContentType = "multipart/form-data; boundary=" + boundary;
                request.Method = "POST";

                var footer = Encoding.UTF8.GetBytes("--" + boundary + "--\r\n");

                var buffer = new byte[8192];
                var afterFile = Encoding.UTF8.GetBytes("\r\n");

                using (var s = await request.GetRequestStreamAsync())
                {
                    foreach (var part in mimeParts)
                    {
                        s.Write(part.Header, 0, part.Header.Length);

                        int read;
                        while ((read = part.Data.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            s.Write(buffer, 0, read);
                        }

                        part.Data.Dispose();

                        s.Write(afterFile, 0, afterFile.Length);
                    }

                    s.Write(footer, 0, footer.Length);
                }

                return await GetContent(request);
            }
            finally
            {
                foreach (var mimePart in mimeParts.Where(part => part.Data != null))
                {
                    mimePart.Data.Dispose();
                }
            }
        }

        private static async Task<WebRequest> BuildRequest(IUriBuilder uriBuilder, HttpMethod method, Dictionary<string, object> parameters, byte[] body, string accessToken)
        {
            var uri = uriBuilder.AddQueryParameters(parameters).Build();
            var request = (HttpWebRequest)WebRequest.Create(uri);

            request.Method = method.GetParameterName();

            request.ContentType = "application/json";

            body = body ?? new byte[0];
            if (method == HttpMethod.Post && body.Length > 0)
            {
                using (var requestStream = await request.GetRequestStreamAsync())
                {
                    await requestStream.WriteAsync(body, 0, body.Length);
                    await requestStream.FlushAsync();
                }
            }

            request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
            request.Headers["Authorization"] = "OAuth " + accessToken;

            return request;
        }

        private static async Task<HttpWebResponse> GetResponse(WebRequest request)
        {
            try
            {
                return (HttpWebResponse)await request.GetResponseAsync();
            }
            catch (WebException ex)
            {
                using (var response = (HttpWebResponse)ex.Response)
                using (var responseStream = response.GetResponseStream())
                {
                    var content = SmartReadContent(response, responseStream);
                    throw new WebGatewayException($"Error: {content}", response.StatusCode, ex);
                }
            }
            catch (Exception ex)
            {
                const HttpStatusCode statusCode = HttpStatusCode.ServiceUnavailable;
                throw new WebGatewayException($"Error status: {statusCode}", statusCode, ex);
            }
        }

        private static async Task<string> GetContent(WebRequest request)
        {
            using (var response = await GetResponse(request))
            using (var responseStream = response.GetResponseStream())
            {
                var content = SmartReadContent(response, responseStream);
                return content;
            }
        }

        private static string SmartReadContent(WebResponse response, Stream stream)
        {
            var contentEncoding = response.Headers[HttpResponseHeader.ContentEncoding];
            if (!string.IsNullOrEmpty(contentEncoding) && (contentEncoding.Contains("gzip") || contentEncoding.Contains("deflate")))
            {
                using (var gZipStream = new GZipStream(stream, CompressionMode.Decompress))
                {
                    //todo: maybe in catch error case should try stream without gZip?
                    return ReadContent(gZipStream);
                }
            }

            return ReadContent(stream);
        }

        private static string ReadContent(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}