﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SoundCloud.API.Client.Internal.Client.Helpers;
using SoundCloud.API.Client.Internal.Infrastructure.Objects;
using SoundCloud.API.Client.Objects.Auth;
using File = SoundCloud.API.Client.Internal.Infrastructure.Objects.Uploading.File;

namespace SoundCloud.API.Client.Internal.Client
{
    internal interface ISoundCloudRawClient
    {
        SCCredentials Credentials { get; }
        SCAccessToken AccessToken { get; set; }
        Task<T> Request<T>(string prefix, string command, HttpMethod method, Dictionary<string, object> parameters = null, byte[] body = null, string responseType = "json", Domain domain = Domain.Api) where T : class;
        Task Request(string prefix, string command, HttpMethod method, Dictionary<string, object> parameters = null, byte[] body = null, Domain domain = Domain.Api);
        Task<Stream> RequestStream(string apiPrefix, string command, HttpMethod method, Dictionary<string, object> parameters = null, byte[] body = null, Domain domain = Domain.Api);
        Uri BuildUri(string command, Dictionary<string, object> parameters, string responseType, Domain domain = Domain.Direct);
        Task<T> Upload<T>(string prefix, string command, Dictionary<string, object> parameters, string responseType = "json", Domain domain = Domain.Api, params File[] files);
    }
}